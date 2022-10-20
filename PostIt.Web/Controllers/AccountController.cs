using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Data;
using PostIt.Web.Dtos.AccountActions;
using PostIt.Web.Models;
using PostIt.Web.Services;

namespace PostIt.Web.Controllers;

[Authorize]
[Route("[controller]")]
public class AccountController : Controller
{
    private readonly IUserService _userService;
    private readonly ApplicationContext _context;

    public AccountController(IUserService userService, ApplicationContext context)
    {
        _userService = userService;
        _context = context;
    }

    [AllowAnonymous]
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

    [Route("Blocked")]
    public IActionResult BlockedUsers()
    {
        var blockedUsers = _userService.GetByUsername(User.Identity.Name).BlockedUsers;
        var users = new List<User>();

        foreach (var u in blockedUsers)
        {
            users.Add(_context.Users.FirstOrDefault(x => x.Id == u.BlockedUserId)!);
        }

        return View(users);
    }

    [Route("UpdateUsername")]
    public IActionResult UpdateUsername()
    {
        throw new NotImplementedException();
    }

    [Route("UpdatePhoneNumber")]
    public IActionResult UpdatePhoneNumber()
    {
        throw new NotImplementedException();
    }
}