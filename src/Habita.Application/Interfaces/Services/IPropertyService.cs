using Habita.Application.Dtos.Property.Requests;
using Habita.Application.Dtos.Property.Responses;
using Habita.Domain.Common;

namespace Habita.Application.Interfaces.Services;

public interface IPropertyService
{
    Task<Result<CreatePropertyResponse>> CreateAsync(
        CreatePropertyRequest request,
        CancellationToken ct = default
    );
    Task<Result> UpdateAsync(UpdatePropertyRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(int propertyId, CancellationToken ct = default);
    Task<Result<PagedResult<PropertyListItemResponse>>> GetListAsync(
        GetPropertyListRequest request,
        CancellationToken ct = default
    );
    Task<Result<PropertyDetailResponse>> GetDetailAsync(
        int propertyId,
        CancellationToken ct = default
    );
    Task<Result<PropertyDetailResponse>> SearchByCodeAsync(
        string code,
        CancellationToken ct = default
    );
    Task<Result<PagedResult<PropertySummaryResponse>>> GetAgentPropertiesAsync(
        GetAgentPropertiesRequest request,
        CancellationToken ct = default
    );
}
