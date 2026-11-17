using System.Linq.Expressions;
using Habita.Domain.Common;

namespace Habita.Domain.Interfaces.Persistence.Repositories;

             
                                                          
                                               
              
public interface IGenericRepository<T>
    where T : BaseEntity<int>
{
    Task<T?> GetByIdAsync(
        int id,
        CancellationToken ct = default,
        params Expression<Func<T, object>>[] includes
    );
    Task<IReadOnlyList<T>> GetAllAsync(
        QueryOptions<T>? options = null,
        CancellationToken ct = default
    );
    Task<T?> GetFirstOrDefaultAsync(QueryOptions<T> options, CancellationToken ct = default);
    Task<int> CountAsync(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken ct = default
    );
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    IQueryable<T> Query();
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
    void Update(T entity);
    void Delete(T entity);
}
