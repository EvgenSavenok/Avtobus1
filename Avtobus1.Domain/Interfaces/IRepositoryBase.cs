using System.Linq.Expressions;

namespace Avtobus1.Domain.Interfaces;

public interface IRepositoryBase<T>
{
    Task<IEnumerable<T>> FindAllAsync(bool trackChanges, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<T>> FindByConditionAsync(
        Expression<Func<T, bool>> expression,
        bool trackChanges,
        CancellationToken cancellationToken = default);

    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}