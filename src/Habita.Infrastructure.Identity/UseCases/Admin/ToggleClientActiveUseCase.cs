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

public sealed class ToggleClientActiveUseCase : IToggleClientActiveUseCase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<ToggleClientActiveRequest> _validator;
    private readonly ILogger<ToggleClientActiveUseCase> _logger;

    public ToggleClientActiveUseCase(
        UserManager<AppUser> userManager,
        ICurrentUserService currentUser,
        IValidator<ToggleClientActiveRequest> validator,
        ILogger<ToggleClientActiveUseCase> logger
    )
    {
        _userManager = userManager;
        _currentUser = currentUser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(
        ToggleClientActiveRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToResult();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result.Failure(Error.Unauthorized("Auth.NotAuthenticated", "Debe iniciar sesión."));

        if (!_currentUser.IsInRole(nameof(Roles.Admin)))
            return Result.Failure(
                Error.Forbidden("Auth.AdminOnly", "Solo administradores pueden gestionar clientes.")
            );

        var client = await _userManager.FindByIdAsync(request.ClientId);
        if (client is null)
            return Result.Failure(Error.NotFound("User.NotFound", "Cliente no encontrado."));

        if (!await _userManager.IsInRoleAsync(client, nameof(Roles.Client)))
            return Result.Failure(Error.Validation("User.NotClient", "El usuario no tiene rol de Cliente."));

        if (client.Id == _currentUser.UserId)
            return Result.Failure(
                Error.Validation("Admin.SelfToggle", "No puede cambiar el estado de su propio usuario.")
            );

        if (client.Active)
            client.Deactivate();
        else
            client.Activate();

        var update = await _userManager.UpdateAsync(client);
        if (!update.Succeeded)
        {
            var errors = string.Join(", ", update.Errors.Select(error => error.Description));
            _logger.LogWarning(
                "Fallo al cambiar el estado del cliente {ClientId}: {Errors}",
                request.ClientId,
                errors
            );
            return Result.Failure(Error.Failure("User.UpdateFailed", "No se pudo actualizar el estado del cliente."));
        }

        _logger.LogInformation(
            "Admin {AdminId} {Action} cliente {ClientId}.",
            _currentUser.UserId,
            client.Active ? "activó" : "inactivó",
            client.Id
        );

        return Result.Success();
    }
}
