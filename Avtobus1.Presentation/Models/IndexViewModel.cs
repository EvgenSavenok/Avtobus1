using Avtobus1.Application.DTO;

namespace Avtobus1.Presentation.Models;

public class IndexViewModel
{
    public List<UrlDto> Urls { get; set; } = new();

    public string NewUrl { get; set; } = string.Empty;
    
    public string? ErrorMessage { get; set; }
}