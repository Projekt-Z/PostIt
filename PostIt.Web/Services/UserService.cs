using System.Globalization;
using PostIt.Web.Data;
using PostIt.Web.Dtos.Authentication;
using PostIt.Web.Enums;
using PostIt.Web.Models;
using PostIt.Web.Services.DefaultAuthentication;

namespace PostIt.Web.Services;

public class UserService : IUserService
{
    private readonly ApplicationContext _context;

    public UserService(ApplicationContext context)
    {
        _context = context;
    }
    
    public bool Add(UserCreationRequest request, EAuthType authType)
    {
        var find = _context.Users.Any(x => x.Username == request.Username);

        if (find) return false;
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Name = request.Name,
            Surname = request.Surname,
            PhoneNumber = request.PhoneNumber ?? string.Empty,
            Email = request.Email,
            CreatedOn = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            Posts = new List<Post>(),
            PasswordHash = request.Password ?? string.Empty,
            Salt = string.Empty
        };
        
        if(authType == EAuthType.Default)
            user.ProvideSaltAndHash();
        
        _context.Users.Add(user);
        _context.SaveChanges();
        return true;
    }

    public User Get(Guid id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user ?? new User();
    }

    public User GetByUsername(string username)
    {
        var user = _context.Users.FirstOrDefault(x => x.Username == username);

        return user ?? new User();
    }

    public List<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public bool Remove(Guid id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);

        if (user is null) return false;
            
        _context.Users.Remove(user);
        _context.SaveChanges();
        return true;
    }

    public bool Update(User user)
    {
        var u = _context.Users.Find(user);
        if (u is null) return false;
        
        u = user;

        _context.SaveChanges();
        return true;
    }
}