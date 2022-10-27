using System.Globalization;
using Microsoft.EntityFrameworkCore;
using PostIt.Web.Data;
using PostIt.Web.Dtos.Authentication;
using PostIt.Web.Enums;
using PostIt.Web.Helpers;
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
    
    public bool Add(UserCreationRequest request, EAuthType authType, string image, string background)
    {
        var find = _context.Users.Any(x => x.Username == request.Username);

        if (find) return false;

        if (!request.Email.IsValidEmail()) return false;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Name = request.Name,
            Surname = request.Surname,
            PhoneNumber = request.PhoneNumber ?? string.Empty,
            Email = request.Email,
            CreatedOn = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            ImageUrl = image,
            BackgroundUrl = background,
            Roles = ERoleType.User,
            AccountType = authType,
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

    public User? GetByUsername(string username)
    {
        var user = _context.Users.Include(x => x.Posts)
            .Include(x => x.PostLiked)
            .Include(x => x.Followers)
            .Include(x => x.Following)
            .Include(x => x.BlockedUsers)
            .Include(x => x.Messages)
            .FirstOrDefault(x => x.Username == username);
        
        return user ?? null;
    }

    public User? GetByEmail(string email)
    {
        var user = _context.Users.Include(x => x.Posts)
            .Include(x => x.PostLiked)
            .Include(x => x.Followers)
            .Include(x => x.Following)
            .FirstOrDefault(x => x.Email == email);
        
        return user ?? null;
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

    public IEnumerable<User> GetMostFollowedDesc()
    {
        var users = _context.Users
            .Include(x => x.BlockedUsers)
            .Include(x => x.Followers)
            .Include(x => x.Following)
            .Include(x => x.Posts)
            .Include(x => x.PostLiked).ToList()
            .OrderByDescending(x => x.Followers.Count);

        return users;
    }

    public List<User> GetMostFollowedDesc(int first)
    {
        return GetMostFollowedDesc().Take(first).ToList();
    }
    
    public List<User> GetMostFollowedDesc(int first, string username)
    {
        var you = GetByUsername(username);
        var most = GetMostFollowedDesc().ToList();

        most.Remove(you);

        /*foreach (var m in most.ToList())
        {
            if (you.BlockedUsers.Count < 1) break;

            var u = you.BlockedUsers.FirstOrDefault(x => x.BlockedUserId == m.Id);
            var usr = _context.Users.FirstOrDefault(x => x.Id == u.BlockedUserId);

            if (u is not null)
                    most.Remove(usr);
        }*/
            
        foreach(var f in you.Following)
        {
            var u = _context.Users.FirstOrDefault(x => x.Id == f.FollowingId);

            most.Remove(u);
        }
      
        return most.Take(first).ToList();
    }

    public void BlockUser(string usernameToBlock, string username) // TODO: BUG FIX
    {
        var userToBlock = GetByUsername(usernameToBlock);

        var user = GetByUsername(username);

        user.BlockedUsers ??= new List<BlockedUser>();

        user.BlockedUsers.Add(new BlockedUser { BlockedUserId = userToBlock.Id });

        _context.SaveChanges();
    }

    public void UnblockUser(string usernameToBlock, string usernam)
    {
        var blockedUser = GetByUsername(usernameToBlock);

        var user = GetByUsername(usernam);

        if (user.BlockedUsers is null) user.BlockedUsers = new List<BlockedUser>();

        var blockedUser2 = user.BlockedUsers.First(x => x.BlockedUserId == blockedUser.Id);

        user.BlockedUsers.Remove(blockedUser2);

        _context.SaveChanges();
    }

    public bool ChangePassword(Guid id, string current, string newPassword)
    {
        var user = _context.Users.Find(id);

        if (user is null) return false;

        if (user.PasswordHash != AuthenticationHelper.GenerateHash(current, user.Salt))
            return false;


        user.PasswordHash = newPassword;
        user.Salt = string.Empty;

        user.ProvideSaltAndHash();

        _context.SaveChanges();
        return true;
    }
}