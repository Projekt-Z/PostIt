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
            .Include(x => x.Comments)!.ThenInclude(x => x.Author)
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
        if (userId == followedBy) return;

        var user = _context.Users.Include(x => x.Followers).Include(x => x.Following).First(x => x.Id == followedBy);
        var userToFollow = _context.Users.Include(x => x.Followers).Include(x => x.Following).First(x => x.Id == userId);
        
        user.Following.Add(new Following
        {
            UserId = userToFollow.Id
        });
        
        userToFollow.Followers.Add(new Followers
        {
            UserId = user.Id
        });
        
        _context.SaveChanges();
    }
    
    public void Unfollow(Guid followerId, Guid userId)
    {
        if (userId == followerId) return;

        var user = _context.Users.Include(x => x.Followers).Include(x => x.Following).First(x => x.Id == userId);
        var userToFollow = _context.Users.Include(x => x.Followers).Include(x => x.Following).First(x => x.Id == followerId);

        var following = user.Following.FirstOrDefault(x => x.UserId == user.Id);
        var follower = userToFollow.Followers.FirstOrDefault(x => x.UserId == userToFollow.Id);

        user.Following.Remove(following);

        userToFollow.Followers.Remove(follower);

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