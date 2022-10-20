using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        if (!User.Identity.IsAuthenticated)
        {
            return View();
        }

        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost, ActionName("Create")]
    [Route("Create")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> CreateConfirmed(UserCreationRequest creationRequest)
    {
        const string profilePicture = @"https://www.pngkey.com/png/detail/115-1150152_default-profile-picture-avatar-png-green.png";
        
        if (ModelState.IsValid)
        {
            var success = _userService.Add(creationRequest, EAuthType.Default, profilePicture, string.Empty);
            if (!success)
            {
                return Task.FromResult<IActionResult>(RedirectToAction("Index","User"));
            }
        }

        return LoginConfirmed(new UserLoginRequest
        {
            EmailOrRUsername = creationRequest.Email,
            Password = creationRequest.Password!
        });
    }
    
    [Route("Login")]
    public IActionResult Login()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return View();
        }

        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost, ActionName("Login")]
    [Route("Login")]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> LoginConfirmed(UserLoginRequest loginRequest)
    {
        if (ModelState.IsValid)
        {
            var user = _userService.GetByEmail(loginRequest.EmailOrRUsername);
            
            if (user is null)
            {
                user = _userService.GetByUsername(loginRequest.EmailOrRUsername);
            }

            var login = _authService.Login(loginRequest.EmailOrRUsername, loginRequest.Password);

            if (!login)
            {
                return Task.FromResult<IActionResult>(RedirectToAction(nameof(Login)));
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
            
            return Task.FromResult<IActionResult>(RedirectToAction("Index", "Home"));
        }

        return Task.FromResult<IActionResult>(RedirectToAction(nameof(Index)));
    }
}