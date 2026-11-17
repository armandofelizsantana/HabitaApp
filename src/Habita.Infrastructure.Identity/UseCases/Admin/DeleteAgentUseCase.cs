using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Admin.Requests;
using Habita.Application.Interfaces;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Admin;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence;
using Habita.Domain.Interfaces.Persistence.Repositories;
using Habita.Infrastructure.Identity.Entities;

namespace Habita.Infrastructure.Identity.UseCases.Admin;

public sealed class DeleteAgentUseCase : IDeleteAgentUseCase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPropertyRepository _propertyRepo;
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<DeleteAgentRequest> _validator;
    private readonly ILogger<DeleteAgentUseCase> _logger;

    public DeleteAgentUseCase(
        UserManager<AppUser> userManager,
        IPropertyRepository propertyRepo,
        IFileService fileService,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IValidator<DeleteAgentRequest> validator,
        ILogger<DeleteAgentUseCase> logger
    )
    {
        _userManager = userManager;
        _propertyRepo = propertyRepo;
        _fileService = fileService;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(
        DeleteAgentRequest request,
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
                Error.Forbidden("Auth.AdminOnly", "Solo administradores pueden eliminar agentes.")
            );

        var agent = await _userManager.FindByIdAsync(request.AgentId);
        if (agent is null)
            return Result.Failure(Error.NotFound("User.NotFound", "Agente no encontrado."));

        if (!await _userManager.IsInRoleAsync(agent, nameof(Roles.Agent)))
            return Result.Failure(
                Error.Validation("User.NotAgent", "El usuario no tiene rol de Agente.")
            );

        if (agent.Id == _currentUser.UserId)
            return Result.Failure(
                Error.Validation("Admin.SelfDelete", "No puede eliminar su propio usuario.")
            );

        var properties = await _propertyRepo.GetByAgentAsync(
            request.AgentId,
            options: new QueryOptions<Property> { Includes = [p => p.Images] },
            ct: ct
        );

        int imageCount = 0;
        foreach (var property in properties)
        {
            foreach (var img in property.Images)
            {
                try
                {
                    _fileService.DeleteFile(img.Url);
                    imageCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Error al eliminar imagen {Url} de propiedad {PropId}.",
                        img.Url,
                        property.Id
                    );
                }
            }
            _propertyRepo.Delete(property);
        }

        await _unitOfWork.SaveChangesAsync(ct);

        var userResult = await _userManager.DeleteAsync(agent);
        if (!userResult.Succeeded)
        {
            var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
            _logger.LogError(
                "Fallo al eliminar usuario agente {AgentId}: {Errors}",
                request.AgentId,
                errors
            );
            return Result.Failure(
                Error.Failure("User.DeleteFailed", "No se pudo eliminar el agente.")
            );
        }

        _logger.LogInformation(
            "Admin {AdminId} eliminó agente {AgentId} ({PropertyCount} propiedades, {ImageCount} imágenes).",
            _currentUser.UserId,
            request.AgentId,
            properties.Count,
            imageCount
        );

        return Result.Success();
    }
}
