using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Auth.Requests;
using Habita.Application.Interfaces;
using Habita.Application.Interfaces.UseCases.Auth;
using Habita.Application.Models.Emails;
using Habita.Domain.Common;
using Habita.Domain.ValueObjects;
using Habita.Infrastructure.Identity.Entities;

namespace Habita.Infrastructure.Identity.UseCases.Auth;

public sealed class ForgotPasswordUseCase : IForgotPasswordUseCase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IValidator<ForgotPasswordRequest> _validator;
    private readonly ILogger<ForgotPasswordUseCase> _logger;

    public ForgotPasswordUseCase(
        UserManager<AppUser> userManager,
        IEmailService emailService,
        IValidator<ForgotPasswordRequest> validator,
        ILogger<ForgotPasswordUseCase> logger
    )
    {
        _userManager = userManager;
        _emailService = emailService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(
        ForgotPasswordRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult();

        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure(emailResult.GetError());

        var email = emailResult.GetValue();

        var user = await _userManager.FindByEmailAsync(email.Value);
        if (user is null || !user.Active)
        {
            _logger.LogInformation(
                "ForgotPassword ignorado para {Email}: usuario no encontrado o inactivo.",
                email.Value
            );
            return Result.Success();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendEmailAsync(
            user.Email!,
            "Restablecimiento de contraseña — Habita",
            "PasswordReset",
            new PasswordResetModel(
                user.GetDisplayName(),
                $"{request.Origin.TrimEnd('/')}/Auth/ResetPassword?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}"
            ),
            cancellationToken
        );

        _logger.LogInformation("Correo de restablecimiento enviado a {Email}.", user.Email);

        return Result.Success();
    }
}
