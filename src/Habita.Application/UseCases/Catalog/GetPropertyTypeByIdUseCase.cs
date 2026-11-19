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

public sealed class GetPropertyTypeByIdUseCase(
    IGenericRepository<PropertyType> repository,
    IMapper mapper,
    ICurrentUserService currentUser,
    IValidator<GetPropertyTypeByIdRequest> validator
) : CatalogLookupService<PropertyType, GetPropertyTypeByIdRequest, PropertyTypeResponse>(repository, mapper, currentUser, validator),
    IGetPropertyTypeByIdUseCase
{
    protected override int GetId(GetPropertyTypeByIdRequest request) => request.Id;

    protected override Error ForbiddenError =>
        Error.Forbidden("Auth.AdminOrDeveloperOnly", "Solo administradores o desarrolladores pueden consultar tipos de propiedad.");

    protected override Error MissingError =>
        Error.NotFound("PropertyType.NotFound", "El tipo de propiedad solicitado no existe.");
}
