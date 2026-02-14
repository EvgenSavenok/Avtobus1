using Avtobus1.Domain.Entities;
using Avtobus1.Domain.Interfaces;
using Avtobus1.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Avtobus1.Infrastructure.Repositories;

public class UrlRepository(UrlDbContext repositoryContext) 
    : RepositoryBase<UrlRecord>(repositoryContext), IUrlRepository
{
    public async Task<UrlRecord?> GetByCodeAsync(string code, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var results = await FindByConditionAsync(u => u.ShortCode == code, trackChanges, cancellationToken);
        return results.FirstOrDefault();
    }

    public async Task<UrlRecord?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var results = await FindByConditionAsync(u => u.Id == id, trackChanges, cancellationToken);
        return results.FirstOrDefault();
    }

    public async Task<bool> IsCodeUniqueAsync(string code, CancellationToken cancellationToken = default)
    {
        return !await RepositoryContext.UrlRecords
            .AnyAsync(u => u.ShortCode == code, cancellationToken);
    }
}