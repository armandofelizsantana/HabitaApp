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
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Catalog;

public sealed class GetSaleTypeByIdUseCase : IGetSaleTypeByIdUseCase
{
    private readonly IGenericRepository<SaleType> _repository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IValidator<GetSaleTypeByIdRequest> _validator;

    public GetSaleTypeByIdUseCase(
        IGenericRepository<SaleType> repository,
        IMapper mapper,
        ICurrentUserService currentUser,
        IValidator<GetSaleTypeByIdRequest> validator
    )
    {
        _repository = repository;
        _mapper = mapper;
        _currentUser = currentUser;
        _validator = validator;
    }

    public async Task<Result<SaleTypeResponse>> ExecuteAsync(
        GetSaleTypeByIdRequest request,
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

        if (
            !_currentUser.IsInRole(nameof(Roles.Admin))
            && !_currentUser.IsInRole(nameof(Roles.Developer))
        )
            return Result<SaleTypeResponse>.Failure(
                Error.Forbidden(
                    "Auth.AdminOrDeveloperOnly",
                    "Solo administradores o desarrolladores pueden consultar tipos de venta."
                )
            );

        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return Result<SaleTypeResponse>.Failure(
                Error.NotFound("SaleType.NotFound", "El tipo de venta solicitado no existe.")
            );

        return Result<SaleTypeResponse>.Success(_mapper.Map<SaleTypeResponse>(entity));
    }
}
