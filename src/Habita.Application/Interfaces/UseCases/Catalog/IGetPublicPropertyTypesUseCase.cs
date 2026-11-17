using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Dtos.Catalog.Responses;

namespace Habita.Application.Interfaces.UseCases.Catalog;

public interface IGetPublicPropertyTypesUseCase
    : IUseCase<GetPublicPropertyTypesRequest, IReadOnlyList<PropertyTypeResponse>>;
