using Avtobus1.Application.DTO;
using Avtobus1.Application.Interfaces;
using Avtobus1.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Avtobus1.Presentation.Controllers;

public class HomeController(IUrlService urlService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = new IndexViewModel
        {
            Urls = await urlService.GetAllUrlsAsync()
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Shorten(IndexViewModel model)
    {
        await urlService.CreateShortUrlAsync(new UrlDto { OriginalUrl = model.NewUrl });
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await urlService.DeleteUrlAsync(id);
        return RedirectToAction("Index");
    }
}