using System.Linq.Expressions;
using Avtobus1.Domain.Interfaces;
using Avtobus1.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Avtobus1.Infrastructure.Repositories;

/// <summary>
/// Repository pattern will be suitable here because it allows us to change DB without any problems
/// </summary>
/// <param name="repositoryContext"></param>
/// <typeparam name="T"></typeparam>
public abstract class RepositoryBase<T>(UrlDbContext repositoryContext) : IRepositoryBase<T>
    where T : class
{
    protected UrlDbContext RepositoryContext = repositoryContext;

    public async Task<IEnumerable<T>> FindAllAsync(bool trackChanges, CancellationToken cancellationToken = default) =>
        await (!trackChanges ?
                RepositoryContext.Set<T>().AsNoTracking() :
                RepositoryContext.Set<T>())
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<T>> FindByConditionAsync(
        Expression<Func<T, bool>> expression,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = RepositoryContext.Set<T>();

        if (!trackChanges)
            query = query.AsNoTracking();

        query = query.Where(expression);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await RepositoryContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        RepositoryContext.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        RepositoryContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
}