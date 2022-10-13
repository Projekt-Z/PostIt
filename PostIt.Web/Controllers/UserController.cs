using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Dtos.Authentication;
using PostIt.Web.Enums;
using PostIt.Web.Models;
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
    
    public IActionResult Index()
    {
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
            Console.WriteLine(ModelState.IsValid);
            
            var success = _userService.Add(creationRequest, EAuthType.Default, string.Empty);
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