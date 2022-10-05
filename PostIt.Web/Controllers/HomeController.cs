using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Models;

namespace PostIt.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("~/Views/Home/Index.cshtml");
    }

    public IActionResult Privacy()
    {
        return View("~/Views/Home/Privacy.cshtml");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("~/Views/Shared/Error.cshtml",new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}