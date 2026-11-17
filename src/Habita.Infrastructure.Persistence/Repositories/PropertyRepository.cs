using Microsoft.EntityFrameworkCore;
using Habita.Domain.Common;
using Habita.Domain.Entities;
using Habita.Domain.Enums;
using Habita.Domain.Interfaces.Persistence.Repositories;
using Habita.Domain.ValueObjects;
using Habita.Infrastructure.Persistence.Contexts;

namespace Habita.Infrastructure.Persistence.Repositories;

public sealed class PropertyRepository : GenericRepository<Property>, IPropertyRepository
{
    public PropertyRepository(AppDbContext context)
        : base(context) { }

    public async Task<Property?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Code == PropertyCode.Unsafe(code), ct);
    }

    public async Task<IReadOnlyList<Property>> GetAvailableAsync(
        QueryOptions<Property>? options = null,
        CancellationToken ct = default
    )
    {
        var query = Query().Where(x => x.Status == PropertyStatus.Available);
        query = ApplyOptionsToQuery(query, options);
        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Property>> GetByAgentAsync(
        string agentId,
        PropertyStatus? status = null,
        QueryOptions<Property>? options = null,
        CancellationToken ct = default
    )
    {
        var query = Query().Where(x => x.AgentId == agentId);

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        query = ApplyOptionsToQuery(query, options);
        return await query.ToListAsync(ct);
    }

    public async Task<bool> IsPropertyOwnedByAgentAsync(
        int propertyId,
        string agentId,
        CancellationToken ct = default
    )
    {
        return await _dbSet.AnyAsync(x => x.Id == propertyId && x.AgentId == agentId, ct);
    }

                 
                                                                         
                  
    private static IQueryable<Property> ApplyOptionsToQuery(
        IQueryable<Property> query,
        QueryOptions<Property>? options
    )
    {
        if (options is null)
            return query;

        if (options.UseSplitQuery)
            query = query.AsSplitQuery();

        foreach (var include in options.Includes)
            query = query.Include(include);

        if (options.OrderBy is not null)
            query = options.OrderBy(query);

        if (options.Skip.HasValue)
            query = query.Skip(options.Skip.Value);

        if (options.Take.HasValue)
            query = query.Take(options.Take.Value);

        return query;
    }
}
