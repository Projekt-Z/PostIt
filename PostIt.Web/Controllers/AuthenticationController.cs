using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PostIt.Web.Controllers;

[AllowAnonymous, Route("oauth")]
public class AuthenticationController : Controller
{
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

        return Json(claims);
    }

    public async Task<IActionResult> GoogleLogout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Privacy", "Home");
    }
}