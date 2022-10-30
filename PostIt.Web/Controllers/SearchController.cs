using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Dtos.Search;
using PostIt.Web.Services;
using PostIt.Web.Services.Posts;

namespace PostIt.Web.Controllers;

[Route("[controller]")]
public class SearchController : Controller
{
    private readonly IPostService _postService;
    private readonly IUserService _userService;

    public SearchController(IPostService postService, IUserService userService)
    {
        _postService = postService;
        _userService = userService;
    }

    public IActionResult Search([FromQuery] string? q)
    {
        if (q is null)
        {
            return View(null);
        }

        var query = q.ToLower();
        var posts = _postService.GetAll().Where(x => x.Title.ToLower().Contains(query) || x.Description.ToLower().Contains(query));
        var users = _userService.GetAll().Where(x => x.Username.ToLower().Contains(query) || x.Name.ToLower().Contains(query));
        
        return View(new Search { Posts = posts, Users = users });
    }
}