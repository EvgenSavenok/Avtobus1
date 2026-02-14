using Avtobus1.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Avtobus1.Presentation.Controllers;

public class RedirectController(IUrlService urlService) : Controller
{
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var originalUrl = await urlService.GetOriginalUrlAsync(shortCode);
        return Redirect(originalUrl);
    }
}