using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PostIt.Web.Data;
using PostIt.Web.Dtos.Authentication;
using PostIt.Web.Dtos.Smtp;
using PostIt.Web.Enums;
using PostIt.Web.Helpers;
using PostIt.Web.Services;
using PostIt.Web.Services.DefaultAuthentication;
using PostIt.Web.Services.Smtp;

namespace PostIt.Web.Controllers;

[Route("auth")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IDefaultAuthenticationService _authService;
    private readonly IDistributedCache _cache;
    private readonly ISmtpService _smtpService;
    private readonly ApplicationContext _context;

    public UserController(
        IUserService userService, 
        IDefaultAuthenticationService authService, 
        IDistributedCache cache, ISmtpService smtpService, 
        ApplicationContext context)
    {
        _userService = userService;
        _authService = authService;
        _cache = cache;
        _smtpService = smtpService;
        _context = context;
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
    
    [Route("Create/Confirm")]
    public IActionResult CreateConfirm(CreateConfirm confirm)
    {
        return View("CreateConfirm", confirm);
    }
    
    [HttpPost, ActionName("Create/Confirm")]
    [Route("Create/Confirm")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateConfirmConfirmed(CreateConfirm confirm)
    {
        if (confirm.Email is null)
        {
            return RedirectToAction(nameof(CreateConfirm), confirm);
        }
    
        if (ModelState.IsValid)
        {
            var code = _cache.GetString(confirm.Email);

            if (confirm.Code != int.Parse(code))
            {
                return RedirectToAction("CreateConfirm", confirm);
            }

            var user = _userService.GetByEmail(confirm.Email!);

            user.VerifiedEmail = true;
            _context.SaveChanges();
            
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("CreateConfirm", confirm);
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

            var code = Random.Shared.Next();
            
            // Setting the code into redis cluster
            _cache.SetString(creationRequest.Email, code.ToString());
            
            // Send Mail with code
            _smtpService.Send(new MailRequestDto
            {
                ToAddress = creationRequest.Email,
                Subject = "Confirm your Account - PostIt",
                Body = @$"<p style=""text-align: center;""> Your code is: {code} </p>"
            });

            if (creationRequest.PhoneNumber != null && !creationRequest.PhoneNumber.IsValidPhoneNumber())
            {
                return Task.FromResult<IActionResult>(RedirectToAction("Index","User"));
            }
            
            if (!success)
            {
                return Task.FromResult<IActionResult>(RedirectToAction("Index","User"));
            }
        }

        return Task.FromResult<IActionResult>(RedirectToAction("CreateConfirm", new CreateConfirm
        {
            Email = creationRequest.Email
        }));
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
            var user = _userService.GetByEmail(loginRequest.EmailOrRUsername) ?? _userService.GetByUsername(loginRequest.EmailOrRUsername);

            if (!user!.VerifiedEmail)
            {
                var code = Random.Shared.Next();
            
                // Setting the code into redis cluster
                _cache.SetString(user.Email, code.ToString());
            
                // Send Mail with code
                _smtpService.Send(new MailRequestDto
                {
                    ToAddress = user.Email,
                    Subject = "Confirm your Account - PostIt",
                    Body = @$"<p style=""text-align: center;""> Your code is: {code} </p>"
                });

                return Task.FromResult<IActionResult>(RedirectToAction(nameof(CreateConfirm), new CreateConfirm
                {
                    Email = user.Email
                }));
            }

            var ip = new WebClient().DownloadString("https://ipv4.icanhazip.com/").TrimEnd();
            
            _smtpService.Send(new MailRequestDto
            {
                ToAddress = user.Email,
                Subject = "New login detected - PostIt",
                Body = @$"<p>Someone with IP: {ip} at {DateTime.UtcNow} UTC has logged in to tour account. If that was you, You can ignore this message.</p>"
            });
            
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