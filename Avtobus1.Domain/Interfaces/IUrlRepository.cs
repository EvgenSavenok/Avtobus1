using Avtobus1.Domain.Entities;

namespace Avtobus1.Domain.Interfaces;

public interface IUrlRepository : IRepositoryBase<UrlRecord>
{
    Task<UrlRecord?> GetByCodeAsync(string code, bool trackChanges, CancellationToken cancellationToken = default);
    Task<bool> IsCodeUniqueAsync(string code, CancellationToken cancellationToken = default);
    Task<UrlRecord?> GetByIdAsync(Guid id, bool trackChanges, CancellationToken cancellationToken = default);
}