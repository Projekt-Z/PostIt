using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostIt.Web.Data;
using PostIt.Web.Models;
using PostIt.Web.Services;
using PostIt.Web.Services.Posts;

namespace PostIt.Web.Controllers;

public class PostController : Controller
{
    private readonly IPostService _postService;
    private readonly ApplicationContext _context;
    private readonly IUserService _userService;

    public PostController(IPostService postService, ApplicationContext context, IUserService userService)
    {
        _postService = postService;
        _context = context;
        _userService = userService;
    }

    public IActionResult Index([FromRoute] int id)
    {
        var post = _postService.Get(id);

        return View(post);
    }

    [Authorize]
    [HttpPost("Comment")]
    [ValidateAntiForgeryToken]
    public IActionResult Comment(string content, int id, string username)
    {
        var post = _context.Posts.Find(id);

        var comment = new Comment
        {
            Content = content,
            Author = _userService.GetByUsername(username),
            Post = post,
            TimeAdded = DateTime.Now.ToString()
        };

        if (post.Comments is null)
        {
            post.Comments = new List<Comment>();
        }

        post.Comments.Add(comment);

        _context.SaveChanges();

        return RedirectToAction("Index", "Post", new {id = post.Id});
    }

    [Authorize]
    [HttpPost("Reply")]
    [ValidateAntiForgeryToken]
    public IActionResult Reply(string content, int id, int postId, string username)
    {
        var post = _context.Posts.Include(x => x.Comments).First(x => x.Id == postId);

        var comment = post.Comments!.First(x => x.Id == id);

        comment.Comments ??= new();
        comment.Comments.Add(
        new Comment
        {
            Author = _userService.GetByUsername(username)!,
            Content = content,
            TimeAdded = DateTime.Now.ToString(),
            Reply = true,
            Post = post
        });

        _context.SaveChanges();
        
        return RedirectToAction("Index", "Post", new {id = post.Id});
    }
}