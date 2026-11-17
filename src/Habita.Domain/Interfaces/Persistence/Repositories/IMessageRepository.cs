using Habita.Domain.Common;
using Habita.Domain.Entities;

namespace Habita.Domain.Interfaces.Persistence.Repositories;

public interface IMessageRepository : IGenericRepository<Message>
{
    Task<IReadOnlyList<Message>> GetByPropertyAsync(
        int propertyId,
        QueryOptions<Message>? options = null,
        CancellationToken ct = default
    );
    Task<IReadOnlyList<Message>> GetByClientAsync(
        string clientId,
        QueryOptions<Message>? options = null,
        CancellationToken ct = default
    );
    Task<IReadOnlyList<Message>> GetConversationAsync(
        int propertyId,
        string clientId,
        string agentId,
        QueryOptions<Message>? options = null,
        CancellationToken ct = default
    );
}
