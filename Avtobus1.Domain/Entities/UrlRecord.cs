namespace Avtobus1.Domain.Entities;

public class UrlRecord
{
    public Guid Id { get; init; }
    
    public string OriginalUrl { get; set; }
    
    public string ShortCode { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int ClickCount { get; set; }
}