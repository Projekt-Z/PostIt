using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Dtos.Authentication;
using PostIt.Web.Enums;
using PostIt.Web.Services;
using PostIt.Web.Services.DefaultAuthentication;

namespace PostIt.Web.Controllers;

[Route("auth")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IDefaultAuthenticationService _authService;

    public UserController(IUserService userService, IDefaultAuthenticationService authService)
    {
        _userService = userService;
        _authService = authService;
    }
    
    [Authorize]
    public IActionResult Index()
    {
        var user = _userService.GetByUsername(HttpContext.User.Identity!.Name!);

        if (user.Roles != ERoleType.Admin)
        {
            return BadRequest();
        }
        
        return View(_userService.GetAll());
    }

    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost, ActionName("Create")]
    [Route("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateConfirmed(UserCreationRequest creationRequest)
    {
        if (ModelState.IsValid)
        {
            var success = _userService.Add(creationRequest, EAuthType.Default);
            if (!success)
            {
                return RedirectToAction(nameof(Create));
            }
            
            _authService.Login();
        }

        return RedirectToAction(nameof(Index));
    }
    
    [Route("Login")]
    public IActionResult Login()
    {
        return View();
    }
}