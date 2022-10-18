using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostIt.Web.Data;
using PostIt.Web.Models;
using PostIt.Web.Services;
using PostIt.Web.Services.Posts;

namespace PostIt.Web.Controllers
{
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
                Post = post
            };

            if (post.Comments is null)
            {
                post.Comments = new List<Comment>();
            }

            post.Comments.Add(comment);

            _context.SaveChanges();

            return RedirectToAction("Index", "Post", new {id = post.Id});
        }
    }
}