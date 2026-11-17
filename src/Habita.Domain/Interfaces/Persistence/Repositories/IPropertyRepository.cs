using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;

namespace Habita.Domain.Interfaces.Persistence.Repositories;

             
                                                                                       
              
public interface IPropertyRepository : IGenericRepository<Property>
{
    Task<Property?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Property>> GetAvailableAsync(
        QueryOptions<Property>? options = null,
        CancellationToken ct = default
    );
    Task<IReadOnlyList<Property>> GetByAgentAsync(
        string agentId,
        PropertyStatus? status = null,
        QueryOptions<Property>? options = null,
        CancellationToken ct = default
    );
    Task<bool> IsPropertyOwnedByAgentAsync(
        int propertyId,
        string agentId,
        CancellationToken ct = default
    );
}
