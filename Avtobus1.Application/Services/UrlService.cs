using Avtobus1.Application.DTO;
using Avtobus1.Application.Interfaces;
using Avtobus1.Application.Mappers;
using Avtobus1.Domain.CustomExceptions;
using Avtobus1.Domain.Entities;
using Avtobus1.Domain.Interfaces;
using FluentValidation;

namespace Avtobus1.Application.Services;

public class UrlService(
    IRepositoryManager repository,
    IValidator<UrlRecord> validator) 
    : IUrlService
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int CodeLength = 7;

    public async Task<UrlDto> CreateShortUrlAsync(UrlDto request)
    {
        var urlRecord = request.ToEntity();
        
        urlRecord.CreatedAt = DateTime.UtcNow;
        urlRecord.ShortCode = await GenerateUniqueCodeAsync();

        var validationResult = await validator.ValidateAsync(urlRecord);
    
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await repository.Url.CreateAsync(urlRecord);
        await repository.SaveAsync();

        return urlRecord.ToDto();
    }

    public async Task<List<UrlDto>> GetAllUrlsAsync()
    {
        var records = await repository.Url.FindAllAsync(trackChanges: false);
        return records.Select(r => r.ToDto()).ToList();
    }

    public async Task<string> GetOriginalUrlAsync(string shortCode)
    {
        var record = await repository.Url.GetByCodeAsync(shortCode, trackChanges: true);
    
        if (record == null)
        {
            throw new NotFoundException("Short URL not found");
        }

        record.ClickCount++; 
        
        await repository.SaveAsync();

        return record.OriginalUrl;
    }

    public async Task DeleteUrlAsync(Guid id)
    {
        var entity = await repository.Url.GetByIdAsync(id, trackChanges: false);
        
        if (entity != null)
        {
            await repository.Url.DeleteAsync(entity);
            await repository.SaveAsync();
        }
    }
    
    public async Task<UrlDto> ShortenUrlAsync(string originalUrl)
    {
        return await CreateShortUrlAsync(new UrlDto { OriginalUrl = originalUrl });
    }
    
    private async Task<string> GenerateUniqueCodeAsync()
    {
        var random = new Random();
        while (true)
        {
            var codeChars = new char[CodeLength];
            for (int i = 0; i < CodeLength; i++)
            {
                codeChars[i] = Alphabet[random.Next(Alphabet.Length)];
            }
            var code = new string(codeChars);

            if (await repository.Url.IsCodeUniqueAsync(code))
            {
                return code;
            }
        }
    }
}