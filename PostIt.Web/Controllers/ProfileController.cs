using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Services;

namespace PostIt.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index([FromQuery] string username)
    {
        return View(_userService.GetByUsername(username));
    }
}