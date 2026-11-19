using Habita.Application.Interfaces;
using Habita.Application.Models.Emails;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Admin.Requests;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Admin;
using Habita.Domain.Common;
using Habita.Domain.Enums;
using Habita.Infrastructure.Identity.Entities;

namespace Habita.Infrastructure.Identity.UseCases.Admin;

public sealed class ChangeAgentStatusUseCase : IChangeAgentStatusUseCase
{
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<ChangeAgentStatusRequest> _validator;
    private readonly ILogger<ChangeAgentStatusUseCase> _logger;

    public ChangeAgentStatusUseCase(
        IEmailService emailService,
        UserManager<AppUser> userManager,
        ICurrentUserService currentUser,
        IValidator<ChangeAgentStatusRequest> validator,
        ILogger<ChangeAgentStatusUseCase> logger
    )
    {
        _emailService = emailService;
        _userManager = userManager;
        _currentUser = currentUser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(
        ChangeAgentStatusRequest request,
        CancellationToken ct = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
            return validationResult.ToResult();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result.Failure(
                Error.Unauthorized("Auth.NotAuthenticated", "Debe iniciar sesión.")
            );

        if (!_currentUser.IsInRole(nameof(Roles.Admin)))
            return Result.Failure(
                Error.Forbidden("Auth.AdminOnly", "Solo administradores pueden gestionar agentes.")
            );

        var user = await _userManager.FindByIdAsync(request.AgentId);
        if (user is null)
            return Result.Failure(Error.NotFound("User.NotFound", "Agente no encontrado."));

        if (!await _userManager.IsInRoleAsync(user, nameof(Roles.Agent)))
            return Result.Failure(
                Error.Validation("User.NotAgent", "El usuario no tiene rol de Agente.")
            );

        var wasActive = user.Active;

        if (request.Status)
            user.Activate();
        else
            user.Deactivate();

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning(
                "Fallo al cambiar estado de agente {AgentId}: {Errors}",
                request.AgentId,
                errors
            );
            return Result.Failure(
                Error.Failure("User.UpdateFailed", "No se pudo actualizar el estado del agente.")
            );
        }

        _logger.LogInformation(
            "Admin {AdminId} estableció estado de agente {AgentId} a {Status}.",
            _currentUser.UserId,
            request.AgentId,
            request.Status ? "Activo" : "Inactivo"
        );

        if (!wasActive && user.Active)
        {
            var sent = await _emailService.SendEmailAsync(
                user.Email!,
                "Tu cuenta de agente en Habita ya está activa",
                "AgentActivated",
                new AgentActivatedModel(user.GetDisplayName()),
                ct);
            if (!sent)
            {
                _logger.LogWarning("Agente {AgentId} activado, pero no se pudo enviar el aviso por correo.", user.Id);
                return Result.Failure(Error.Failure(
                    "Agent.ActivationEmailFailed",
                    "La cuenta quedó activa, pero el servidor de correo no aceptó el aviso. Revisa la salida de Habita para ver el error SMTP."));
            }

            _logger.LogInformation(
                "Aviso de activación aceptado por SMTP para el agente {AgentId}.",
                user.Id
            );
        }

        return Result.Success();
    }
}
