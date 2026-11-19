using AutoMapper;
using FluentValidation;
using Habita.Application.Services;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Dtos.Catalog.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Catalog;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Interfaces.Persistence.Repositories;

namespace Habita.Application.UseCases.Catalog;

public sealed class GetSaleTypeByIdUseCase(
    IGenericRepository<SaleType> repository,
    IMapper mapper,
    ICurrentUserService currentUser,
    IValidator<GetSaleTypeByIdRequest> validator
) : CatalogLookupService<SaleType, GetSaleTypeByIdRequest, SaleTypeResponse>(repository, mapper, currentUser, validator),
    IGetSaleTypeByIdUseCase
{
    protected override int GetId(GetSaleTypeByIdRequest request) => request.Id;

    protected override Error ForbiddenError =>
        Error.Forbidden("Auth.AdminOrDeveloperOnly", "Solo administradores o desarrolladores pueden consultar tipos de venta.");

    protected override Error MissingError =>
        Error.NotFound("SaleType.NotFound", "El tipo de venta solicitado no existe.");
}
