using PostIt.Web.Models;

namespace PostIt.Web.Services.Posts;

public interface IPostService
{
    List<Post> GetAll();
    Post Get(int id);
    void Add(Post post);
    bool Delete(int id);
    void Like(int postId, Guid userId);
    void Unlike(int postId, Guid userId);
}