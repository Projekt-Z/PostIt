using PostIt.Web.Dtos.Authentication;
using PostIt.Web.Enums;
using PostIt.Web.Models;

namespace PostIt.Web.Services;

public interface IUserService
{
    bool Add(UserCreationRequest user, EAuthType authType, string image, string background);
    User Get(Guid id);
    User? GetByUsername(string username);
    User? GetByEmail(string email);
    List<User> GetAll();
    bool Remove(Guid id);
    bool Update(User user);
    IEnumerable<User> GetMostFollowedDesc();
    List<User> GetMostFollowedDesc(int first);
    List<User> GetMostFollowedDesc(int first, string username);
    void BlockUser(string usernameToBlock, string username);
    void UnblockUser(string usernameToBlock, string username);
    bool ChangePassword(Guid id, string current, string newPassword);
}