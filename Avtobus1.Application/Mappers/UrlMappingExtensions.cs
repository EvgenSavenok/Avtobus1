using Avtobus1.Application.DTO;
using Avtobus1.Domain.Entities;

namespace Avtobus1.Application.Mappers;

public static class UrlMappingExtensions
{
    public static UrlDto ToDto(this UrlRecord entity)
    {
        return new UrlDto
        {
            UrlId = entity.Id,
            OriginalUrl = entity.OriginalUrl,
            ShortCode = entity.ShortCode,
            ShortUrl = $"http://localhost:5000/{entity.ShortCode}", 
            ClickCount = entity.ClickCount,
            CreatedAt = entity.CreatedAt
        };
    }

    public static UrlRecord ToEntity(this UrlDto dto)
    {
        return new UrlRecord
        {
            Id = Guid.NewGuid(), 
            OriginalUrl = dto.OriginalUrl,
            ClickCount = 0
        };
    }
}