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
            .Include(x => x.Comments)
            .OrderByDescending(x => x.TimeAdded).ToList();
    }

    public List<Post> GetAllYours(string username)
    {
        return _context.Users.Include(x => x.Posts).First(x => x.Username == username).Posts;
    }

    public List<Post> GetAllLiked(string username)
    {
        var user = _context.Users.Include(x => x.PostLiked).First(x => x.Username == username);
        
        foreach (var post in user.PostLiked)
        {
            post.Author = FindAuthorByPostId(post.Id);
        }
        
        return user.PostLiked;
    }

    private User FindAuthorByPostId(int id)
    {
        var user = _context.Posts.Include(x => x.Author).First(x => x.Id == id).Author;
        return user;
    }

    public Post? Get(int id)
    {
        return _context.Posts
            .Include(x => x.Author)
            .Include(x => x.Likes)
            .Include(x => x.Comments)
            .FirstOrDefault(x => x.Id == id);
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
            .Include(x => x.PostLiked)
            .First(x => x.Id == userId);
        
        user.PostLiked.Add(post);
        post.Likes.Add(user);

        _context.SaveChanges();
    }

    public void Unlike(int postId, Guid userId)
    {
        var post = _context.Posts.Include(x => x.Likes).First(x => x.Id == postId);
        var user = _context.Users.Include(x => x.PostLiked).First(x => x.Id == userId);
        
        user.PostLiked.Remove(post);
        post.Likes.Remove(user);

        _context.SaveChanges();
    }

    public void Follow(Guid userId, Guid followedBy)
    {
        var user = _context.Users.Include(x => x.Followers).First(x => x.Id == followedBy);
        var follower = _context.Users.Include(x => x.Followers).First(x => x.Id == userId);
        
        user.Following.Add(new Following
        {
            UserId = followedBy
        });
        
        follower.Followers.Add(new Followers
        {
            UserId = userId
        });
        
        _context.SaveChanges();
    }
    
    public void Unfollow(Guid followerId, Guid userId)
    {
        var user = _context.Users.Include(x => x.Followers).First(x => x.Id == userId);
        var follower = _context.Users.Include(x => x.Followers).First(x => x.Id == followerId);
        
        user.Following.Remove(new Following
        {
            UserId = followerId
        });
        
        follower.Followers.Remove(new Followers
        {
            UserId = userId
        });
        
        _context.SaveChanges();
    }

    public List<User> GetFollowers(string username)
    {
        throw new NotImplementedException();
    }

    public List<User> GetFollowing(string username)
    {
        throw new NotImplementedException();
    }
}