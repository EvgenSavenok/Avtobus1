using Avtobus1.Domain.Interfaces;
using Avtobus1.Infrastructure.DbContexts;

namespace Avtobus1.Infrastructure.Repositories;

public class RepositoryManager(UrlDbContext repositoryContext) : IRepositoryManager
{
    private IUrlRepository? _urlRepository;

    public IUrlRepository Url
    {
        get
        {
            if (_urlRepository == null)
                _urlRepository = new UrlRepository(repositoryContext);
            
            return _urlRepository;
        }
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await repositoryContext.SaveChangesAsync(cancellationToken);
    }
}