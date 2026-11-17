using FluentValidation;
using Microsoft.Extensions.Logging;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Catalog;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Catalog;

public sealed class DeletePropertyTypeUseCase : IDeletePropertyTypeUseCase
{
    private readonly IGenericRepository<PropertyType> _propertyTypeRepo;
    private readonly IPropertyRepository _propertyRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<DeletePropertyTypeRequest> _validator;
    private readonly ILogger<DeletePropertyTypeUseCase> _logger;

    public DeletePropertyTypeUseCase(
        IGenericRepository<PropertyType> propertyTypeRepo,
        IPropertyRepository propertyRepo,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IValidator<DeletePropertyTypeRequest> validator,
        ILogger<DeletePropertyTypeUseCase> logger
    )
    {
        _propertyTypeRepo = propertyTypeRepo;
        _propertyRepo = propertyRepo;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(
        DeletePropertyTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result.Failure(
                Error.Unauthorized("Auth.NotAuthenticated", "Debe iniciar sesión.")
            );

        if (!_currentUser.IsInRole(nameof(Roles.Admin)))
            return Result.Failure(
                Error.Forbidden(
                    "Auth.AdminOnly",
                    "Solo administradores pueden eliminar tipos de propiedad."
                )
            );

        var propertyType = await _propertyTypeRepo.GetByIdAsync(request.Id, cancellationToken);
        if (propertyType is null)
            return Result.Failure(
                Error.NotFound(
                    "PropertyType.NotFound",
                    "No se encontró el tipo de propiedad especificado."
                )
            );

        var inUse = await _propertyRepo.ExistsAsync(
            p => p.PropertyTypeId == request.Id,
            cancellationToken
        );
        if (inUse)
            return Result.Failure(
                Error.Conflict(
                    "PropertyType.InUse",
                    "No se puede eliminar: tiene una o más propiedades asociadas."
                )
            );

        _propertyTypeRepo.Delete(propertyType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Admin {AdminId} eliminó tipo de propiedad {PropertyTypeId}.",
            _currentUser.UserId,
            request.Id
        );

        return Result.Success();
    }
}
