using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
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
        var posts = new List<Post>();

        for (var i = 0; i < 10; i++)
        {
            posts.Add(new Post
            {
                Title = $"test title {i}",
                Description = $"test desc {i}",
                TimeAdded = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            });   
        }

        return View("~/Views/Home/Index.cshtml", posts);
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View("~/Views/Home/Privacy.cshtml");
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("~/Views/Shared/Error.cshtml",new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}