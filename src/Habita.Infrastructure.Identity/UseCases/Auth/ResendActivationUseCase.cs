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

public sealed class ResendActivationUseCase : IResendActivationUseCase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IValidator<ResendActivationRequest> _validator;
    private readonly ILogger<ResendActivationUseCase> _logger;

    public ResendActivationUseCase(
        UserManager<AppUser> userManager,
        IEmailService emailService,
        IValidator<ResendActivationRequest> validator,
        ILogger<ResendActivationUseCase> logger
    )
    {
        _userManager = userManager;
        _emailService = emailService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(
        ResendActivationRequest request,
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
        if (user is null || user.Active || user.EmailConfirmed)
        {
            _logger.LogInformation(
                "Reenvío de activación ignorado para {Email}: usuario no encontrado o ya activo.",
                email.Value
            );
            return Result.Success();
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _emailService.SendEmailAsync(
            user.Email!,
            "Activación de cuenta en Habita",
            "AccountActivation",
            new AccountActivationModel(
                user.GetDisplayName(),
                $"{request.Origin.TrimEnd('/')}/Auth/ActivateAccount?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(token)}"
            ),
            cancellationToken
        );

        _logger.LogInformation("Correo de activación reenviado a {Email}.", user.Email);

        return Result.Success();
    }
}
