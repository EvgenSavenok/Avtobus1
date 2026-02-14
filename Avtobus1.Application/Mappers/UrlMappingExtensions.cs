using Avtobus1.Application.DTO;
using Avtobus1.Domain.Entities;

namespace Avtobus1.Application.Mappers;

/// <summary>
/// Used manual mapping because AutoMapper is slower and many errors
/// might be during reflection in the runtime, but errors in this methods can be tracked during compilation
/// </summary>
public static class UrlMappingExtensions
{
    public static UrlDto ToDto(this UrlRecord entity)
    {
        return new UrlDto
        {
            UrlId = entity.Id,
            OriginalUrl = entity.OriginalUrl,
            ShortCode = entity.ShortCode,
            ShortUrl = $"/{entity.ShortCode}",
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