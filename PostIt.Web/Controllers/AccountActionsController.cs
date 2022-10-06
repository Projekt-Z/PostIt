using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PostIt.Web.Controllers;

[Route("Account")]
[Authorize]
public class AccountActionsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}