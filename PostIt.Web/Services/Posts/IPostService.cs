using PostIt.Web.Models;

namespace PostIt.Web.Services.Posts;

public interface IPostService
{
    IEnumerable<Post> GetAll();
    List<Post> GetAll(string username);
    List<Post> GetAllYours(string username);
    List<Post> GetAllLiked(string username);
    Post? Get(int id);
    void Add(Post post);
    bool Delete(int id);
    void Like(int postId, Guid userId);
    void Unlike(int postId, Guid userId);
    void Follow(Guid toFollow, Guid followedBy);
    void Unfollow(Guid followerId, Guid userId);
    List<User> GetFollowers(string username);
    List<User> GetFollowing(string username);
}