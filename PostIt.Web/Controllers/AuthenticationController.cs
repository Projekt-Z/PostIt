using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Dtos.Authentication;
using PostIt.Web.Enums;
using PostIt.Web.Services;

namespace PostIt.Web.Controllers;

[AllowAnonymous, Route("oauth")]
public class AuthenticationController : Controller
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Route("google-login")]
    public IActionResult GoogleLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback))
        };

        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [Route("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        {
            claim.Issuer,
            claim.OriginalIssuer,
            claim.Type,
            claim.Value
        });
        
        var clamType = HttpContext.User.Claims.Select(userClaim => userClaim.Value).ToList();

        _userService.Add(new UserCreationRequest
        {
            Username = clamType[1],
            Name = clamType[2],
            Surname = clamType[3],
            Email = clamType[4],
            PhoneNumber = string.Empty,
            Password = string.Empty,
        }, EAuthType.Google, User.Claims.Last(x => x.Issuer == "Google").Value, string.Empty);
        
        return RedirectToAction("Index", "Home");
        // return Json(claims);
    }

    public async Task<IActionResult> GoogleLogout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}