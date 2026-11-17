using AutoMapper;
using FluentValidation;
using Habita.Application.Common.Validation;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Dtos.Catalog.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Catalog;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Catalog;

public sealed class CreateSaleTypeUseCase : ICreateSaleTypeUseCase
{
    private readonly IGenericRepository<SaleType> _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateSaleTypeRequest> _validator;

    public CreateSaleTypeUseCase(
        IGenericRepository<SaleType> repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IMapper mapper,
        IValidator<CreateSaleTypeRequest> validator
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<SaleTypeResponse>> ExecuteAsync(
        CreateSaleTypeRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToResult<SaleTypeResponse>();

        if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
            return Result<SaleTypeResponse>.Failure(
                Error.Unauthorized("Auth.NotAuthenticated", "Debe iniciar sesión.")
            );

        if (!_currentUser.IsInRole(nameof(Roles.Admin)))
            return Result<SaleTypeResponse>.Failure(
                Error.Forbidden(
                    "Auth.AdminOnly",
                    "Solo administradores pueden gestionar tipos de venta."
                )
            );

        var result = SaleType.Create(request.Code, request.Name, request.Description);
        if (result.IsFailure)
            return Result<SaleTypeResponse>.Failure(result.GetError());

        var entity = result.GetValue();

        var codeExists = await _repository.ExistsAsync(
            st => st.Code == request.Code, cancellationToken);
        if (codeExists)
            return Result<SaleTypeResponse>.Failure(
                Error.Validation("SaleType.CodeDuplicate",
                    "Ya existe un tipo de venta con ese código."));

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<SaleTypeResponse>.Success(_mapper.Map<SaleTypeResponse>(entity));
    }
}
