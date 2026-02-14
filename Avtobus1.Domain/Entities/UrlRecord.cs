namespace Avtobus1.Domain.Entities;

/// <summary>
/// Clean architecture would be good decision here because it's simple and may be easy to extend
/// </summary>
public class UrlRecord
{
    public Guid Id { get; init; }
    
    public string OriginalUrl { get; set; }
    
    public string ShortCode { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int ClickCount { get; set; }
}