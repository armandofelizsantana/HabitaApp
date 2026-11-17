using FluentValidation;
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

public sealed class UpdateSaleTypeUseCase : IUpdateSaleTypeUseCase
{
    private readonly IGenericRepository<SaleType> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<UpdateSaleTypeRequest> _validator;

    public UpdateSaleTypeUseCase(
        IGenericRepository<SaleType> repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IValidator<UpdateSaleTypeRequest> validator
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _validator = validator;
    }

    public async Task<Result> ExecuteAsync(
        UpdateSaleTypeRequest request,
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
                    "Solo administradores pueden gestionar tipos de venta."
                )
            );

        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return Result.Failure(
                Error.NotFound("SaleType.NotFound", "No se encontró el tipo de venta especificado.")
            );

        var result = entity.Update(request.Code, request.Name, request.Description);
        if (result.IsFailure)
            return Result.Failure(result.GetError());

        var codeExists = await _repository.ExistsAsync(
            st => st.Code == request.Code && st.Id != request.Id, cancellationToken);
        if (codeExists)
            return Result.Failure(
                Error.Validation("SaleType.CodeDuplicate",
                    "Ya existe un tipo de venta con ese código."));

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
