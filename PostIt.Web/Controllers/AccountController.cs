using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostIt.Web.Data;
using PostIt.Web.Dtos.AccountActions;
using PostIt.Web.Models;
using PostIt.Web.Services;
using PostIt.Web.Services.DefaultAuthentication;

namespace PostIt.Web.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly ApplicationContext _context;

    public AccountController(IUserService userService, ApplicationContext context)
    {
        _userService = userService;
        _context = context;
    }

    public IActionResult Index()
    {
        var username = HttpContext.User.Identity.Name;
        return View(_userService.GetByUsername(username));
    }

    [HttpGet("UpdatePassword")]
    public IActionResult UpdatePassword()
    {
        var username = HttpContext.User.Identity.Name;
        var user = _userService.GetByUsername(username);
        return View(new ChangePasswordRequest
        {
            Id = user.Id
        });
    }

    [HttpPost("UpdatePassword")]
    [ActionName("UpdatePassword")]
    [ValidateAntiForgeryToken]
    public IActionResult UpdatePasswordConfirmed(ChangePasswordRequest user)
    {
        var success = _userService.ChangePassword(user.Id, user.CurrentPassword, user.NewPassword);

        if(!success)
        {
            return RedirectToAction("UpdatePassword");
        }

        return RedirectToAction("Index");
    }

    public IActionResult UpdateUsername()
    {
        throw new NotImplementedException();
    }

    public IActionResult UpdatePhoneNumber()
    {
        throw new NotImplementedException();
    }
}