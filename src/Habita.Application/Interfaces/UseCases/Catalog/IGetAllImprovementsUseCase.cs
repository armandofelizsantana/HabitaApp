using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Catalog.Requests;
using Habita.Application.Dtos.Catalog.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.UseCases.Catalog;

public interface IGetAllImprovementsUseCase
    : IUseCase<GetAllImprovementsRequest, PagedResult<ImprovementResponse>>;
