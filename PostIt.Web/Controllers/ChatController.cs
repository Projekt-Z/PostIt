using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Services;

namespace PostIt.Web.Controllers;

[Authorize]
[Route("Chat")]

public class ChatController : Controller
{
    private readonly IUserService _userService;

    public ChatController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index()
    {
        var your = _userService.GetByUsername(User.Identity!.Name!);
        return View("ChatPartial", your!.Following);
    }
    
    [Route("{username}")]
    public IActionResult Chat([FromRoute] string? username)
    {
        var user = _userService.GetByUsername(username!);
        return View("Chat", user);
    }
}