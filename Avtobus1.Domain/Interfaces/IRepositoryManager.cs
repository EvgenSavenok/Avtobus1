namespace Avtobus1.Domain.Interfaces;

public interface IRepositoryManager
{
    IUrlRepository Url { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
}