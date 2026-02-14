using Avtobus1.Application.DTO;

namespace Avtobus1.Application.Interfaces;

public interface IUrlService
{
    Task<UrlDto> CreateShortUrlAsync(UrlDto request);

    Task<List<UrlDto>> GetAllUrlsAsync();

    Task<string> GetOriginalUrlAsync(string shortCode);
    
    Task DeleteUrlAsync(Guid id);

    Task<UrlDto> ShortenUrlAsync(string originalUrl);
}