using Habita.Domain.Enums;

namespace Habita.Application.Dtos.Property.Requests;

public sealed record GetAgentPropertiesRequest(
    int Page = 1,
    int PageSize = 20,
    PropertyStatus? Status = null,
    PropertySearchCriteria? Filters = null
);
