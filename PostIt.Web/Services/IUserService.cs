using PostIt.Web.Dtos.Authentication;
using PostIt.Web.Models;

namespace PostIt.Web.Services;

public interface IUserService
{
    bool Add(UserCreationRequest user);
    User Get(Guid id);
    User GetByUsername(string username);
    List<User> GetAll();
    bool Remove(Guid id);
    bool Update(User user);
}