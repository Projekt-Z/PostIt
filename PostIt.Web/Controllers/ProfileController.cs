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

    [Route("[controller]/{username}")]
    public IActionResult Index(string username)
    {
        var user = _userService.GetByUsername(username);

        if (user is null)
        {
            return NotFound();
        }
        
        return View(user);
    }
}