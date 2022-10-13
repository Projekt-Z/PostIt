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
        return _context.Posts
            .Include(x => x.Author)
            .Include(x => x.Likes)
            .OrderByDescending(x => x.TimeAdded).ToList();
    }

    public List<Post> GetAllLiked(string username)
    {
        var user = _context.Users.First(x => x.Username == username);
        var p = _context.Posts.Include(x => x.Author)
            .Include(x => x.Likes).Where(x => x.Author.Id == user.Id)
            .OrderByDescending(x => x.TimeAdded).ToList();

        return p;
    }

    public Post Get(int id)
    {
        return _context.Posts
            .Include(x => x.Author)
            .First(x => x.Id == id);
    }

    public void Add(Post post)
    {
        _context.Posts.Add(post);
        post.Author.Posts.Add(post);

        post.Likes = new List<User>();
        
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

    public void Like(int postId, Guid userId)
    {
        var post = _context.Posts
            .Include(x => x.Author)
            .Include(x => x.Likes)
            .First(x => x.Id == postId);
        
        var user = _context.Users
            .Include(x => x.Posts)
            .Include(x => x.LikedPosts)
            .First(x => x.Id == userId);
        
        user.LikedPosts.Add(post);
        post.Likes.Add(user);

        _context.SaveChanges();
    }

    public void Unlike(int postId, Guid userId)
    {
        var post = _context.Posts.Include(x => x.Likes).First(x => x.Id == postId);
        var user = _context.Users.Include(x => x.LikedPosts).First(x => x.Id == userId);
        
        user.LikedPosts.Remove(post);
        post.Likes.Remove(user);

        _context.SaveChanges();
    }
}