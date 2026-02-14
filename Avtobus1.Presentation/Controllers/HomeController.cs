using Avtobus1.Application.DTO;
using Avtobus1.Application.Interfaces;
using Avtobus1.Domain.CustomExceptions;
using Avtobus1.Presentation.Models;
using Microsoft.AspNetCore.Diagnostics;
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
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        
        if (exceptionHandlerPathFeature == null)
        {
            return RedirectToAction("Index");
        }

        var exception = exceptionHandlerPathFeature.Error;
        string errorMessage;
        int statusCode;

        switch (exception)
        {
            case NotFoundException notFoundException:
                statusCode = 404;
                errorMessage = notFoundException.Message;
                break;
            case AlreadyExistsException conflictException:
                statusCode = 409;
                errorMessage = conflictException.Message;
                break;
            case BadRequestException badRequestException:
                statusCode = 400;
                errorMessage = badRequestException.Message;
                break;
            default:
                statusCode = 500;
                errorMessage = "Произошла внутренняя ошибка сервера.";
                break;
        }

        HttpContext.Response.StatusCode = statusCode;

        var errorModel = new ErrorViewModel
        {
            RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Message = errorMessage,
            StatusCode = statusCode
        };

        return View("Error", errorModel);
    }
}