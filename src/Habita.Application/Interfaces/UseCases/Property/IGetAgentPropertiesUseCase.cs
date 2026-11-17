using Habita.Application.Common.Interfaces;
using Habita.Application.Dtos.Property.Requests;
using Habita.Application.Dtos.Property.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.UseCases.Property;

public interface IGetAgentPropertiesUseCase
    : IUseCase<GetAgentPropertiesRequest, PagedResult<PropertySummaryResponse>>;
