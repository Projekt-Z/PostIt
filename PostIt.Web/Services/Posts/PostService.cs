using Microsoft.EntityFrameworkCore;
using PostIt.Web.Data;
using PostIt.Web.Models;

namespace PostIt.Web.Services.Posts;

public class PostService : IPostService
{
    private readonly ApplicationContext _context;

    public PostService(ApplicationContext context)
    {
        _context = context;
    }
    
    public List<Post> GetAll()
    {
        return _context.Posts.Include(x => x.Author)
            .OrderByDescending(x => x.TimeAdded).ToList();
    }

    public Post Get(int id)
    {
        return _context.Posts.First(x => x.Id == id);
    }

    public void Add(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
    }

    public bool Delete(int id)
    {
        var post = _context.Posts.FirstOrDefault(x => x.Id == id);

        if (post is null) return false;

        _context.Posts.Remove(post);
        _context.SaveChanges();
        return true;
    }
}