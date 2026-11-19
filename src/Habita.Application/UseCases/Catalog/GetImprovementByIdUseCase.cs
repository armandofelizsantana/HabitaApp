using AutoMapper;
using FluentValidation;
using Habita.Application.Services;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Dtos.Catalog.Responses;
using Habita.Application.Interfaces.Services;
using Habita.Application.Interfaces.UseCases.Catalog;
using Habita.Domain.Common;
using Habita.Domain.Interfaces.Persistence.Repositories;
using ImprovementEntity = Habita.Domain.Entities.Improvement;

namespace Habita.Application.UseCases.Catalog;

public sealed class GetImprovementByIdUseCase(
    IGenericRepository<ImprovementEntity> repository,
    IMapper mapper,
    ICurrentUserService currentUser,
    IValidator<GetImprovementByIdRequest> validator
) : CatalogLookupService<ImprovementEntity, GetImprovementByIdRequest, ImprovementResponse>(repository, mapper, currentUser, validator),
    IGetImprovementByIdUseCase
{
    protected override int GetId(GetImprovementByIdRequest request) => request.Id;

    protected override Error ForbiddenError =>
        Error.Forbidden("Auth.AdminOrDeveloperOnly", "Solo administradores o desarrolladores pueden consultar mejoras.");

    protected override Error MissingError =>
        Error.NotFound("Improvement.NotFound", "La mejora solicitada no existe.");
}
