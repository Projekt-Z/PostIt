using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Data;
using PostIt.Web.Services.Posts;

namespace PostIt.Web.Controllers;

[Route("[controller]")]
public class SearchController : Controller
{
    private readonly IPostService _postService;

    public SearchController(IPostService postService)
    {
        _postService = postService;
    }
    
    public IActionResult Search([FromQuery] string q)
    {
        var posts = _postService.GetAll();
        
        foreach (var post in posts)
        {
            if (post.Title.Contains(q) || post.Description.Contains(q))
            {
                return View(posts);
            }
        }
        
        return View();
    }
}