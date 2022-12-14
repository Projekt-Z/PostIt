using System.Diagnostics;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Dtos.Post;
using PostIt.Web.Enums;
using PostIt.Web.Models;
using PostIt.Web.Services;
using PostIt.Web.Services.Notification;
using PostIt.Web.Services.Posts;

namespace PostIt.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostService _postService;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;

    public HomeController(ILogger<HomeController> logger, IPostService postService, IUserService userService, INotificationService notificationService)
    {
        _logger = logger;
        _postService = postService;
        _userService = userService;
        _notificationService = notificationService;
    }

    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return View(null);
        }
        
        return View("~/Views/Home/Index.cshtml", _postService.GetAll(HttpContext.User.Identity!.Name!));
    }

    [Authorize]
    public IActionResult Post()
    {
        return View();
    }
    
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Post(CreatePost postRequest)
    {
        if (ModelState.IsValid)
        {
            var post = new Post
            {
                Title = postRequest.Title,
                Description = postRequest.Content,
                TimeAdded = DateTime.Now.ToString(),
                MediaLink = postRequest.MediaUrl ??= string.Empty,
                Author = _userService.GetByUsername(HttpContext.User.Identity.Name)
            };

            foreach (var authorFollower in post.Author!.Followers)
            {
                _notificationService.Push(authorFollower.FollowerId, new Notification
                {
                    Type = ENotificationType.Post,
                    Title = $"New Post from {post.Author.Name}!",
                    Content = postRequest.Content,
                    Time = DateTime.Now
                });
            }
            
            _postService.Add(post);
            return RedirectToAction(nameof(Index));
        }
        return View();
    }
    
    [Authorize]
    public IActionResult Delete([FromRoute] int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var post = _postService.Get((int)id);
        
        return View(post);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed([FromRoute] int id)
    {
        _postService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public IActionResult Like([FromRoute] int id)
    {
        var user = _userService.GetByUsername(User.Identity!.Name!);

        var post = _postService.Get(id);
        
        _notificationService.Push(post!.Author.Id, new Notification
        {
            Type = ENotificationType.Like,
            Title = $"{user.Username} likes yor post!",
            Content = $"{post.Description}",
            Time = DateTime.Now
        });
        
        _postService.Like(id, user.Id);
        return RedirectToAction("Index");
    }
    
    [Authorize]
    public IActionResult Unlike([FromRoute] int id)
    {
        var user = _userService.GetByUsername(User.Identity!.Name!);
        _postService.Unlike(id, user.Id);
        return RedirectToAction("Index");
    }
    
    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View("~/Views/Home/Privacy.cshtml");
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("~/Views/Shared/Error.cshtml",new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    [Authorize]
    public IActionResult Follow([FromRoute] Guid id)
    {
        var followedBy = _userService.GetByUsername(User.Identity!.Name!);
        
        _notificationService.Push(id, new Notification
        {
            Type = ENotificationType.Follow,
            Title = "New follower!",
            Content = $"{followedBy.Username} is now following you!",
            Time = DateTime.Now
        });
        
        _postService.Follow(id, followedBy.Id);
        return RedirectToAction("Index");
    }
    
    [Authorize]
    public IActionResult Unfollow([FromRoute] Guid id)
    {
        var followedBy = _userService.GetByUsername(User.Identity!.Name!);
        _postService.Unfollow(id, followedBy.Id);
        return RedirectToAction("Index");
    }
}