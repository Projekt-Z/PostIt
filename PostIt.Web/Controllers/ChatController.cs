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
    
    [Route("{username}")]
    public IActionResult Chat([FromRoute] string? username)
    {
        var receiverConnectionId = _userService.GetByUsername(username!)?.ConnectionId;
        return View("Chat", receiverConnectionId);
    }
}