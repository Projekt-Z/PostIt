using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Models;
using PostIt.Web.Services;

namespace PostIt.Web.Controllers;

[Authorize]
[Route("[controller]")]
public class ProfileController : Controller
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [Route("{username}")]
    public IActionResult Index(string username)
    {
        var user = _userService.GetByUsername(username);
        var you = _userService.GetByUsername(User.Identity.Name);

        if (user is null)
        {
            return NotFound();
        }

        if (user.BlockedUsers.Any(x => x.BlockedUserId == you.Id))
        {
            return View("~/Views/Profile/NotFound.cshtml", user);
        }
        
        return View(user);
    }

    [Route("Block")]
    public IActionResult Block(string username)
    {
        _userService.BlockUser(username, User.Identity.Name);

        return RedirectToAction("Index", "Profile", new { username = username});
    }

    [Route("Unblock")]
    public IActionResult Unblock(string username)
    {
        _userService.UnblockUser(username, User.Identity.Name);

        return RedirectToAction("Index", "Profile", new { username = username });
    }
}