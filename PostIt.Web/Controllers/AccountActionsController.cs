using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Services;

namespace PostIt.Web.Controllers;

[Route("Account")]
[Authorize]
public class AccountActionsController : Controller
{
    private readonly IUserService _userService;
    public AccountActionsController(IUserService userService)
    {
        _userService = userService;
    }
    
    public IActionResult Index()
    {
        var username = HttpContext.User.Identity.Name;
        return View(_userService.GetByUsername(username));
    }
}