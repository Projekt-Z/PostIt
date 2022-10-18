using PostIt.Web.Data;
using PostIt.Web.Helpers;

namespace PostIt.Web.Services.DefaultAuthentication;

public class DefaultAuthenticationService : IDefaultAuthenticationService
{
    private readonly ApplicationContext _context;

    public DefaultAuthenticationService(ApplicationContext context)
    {
        _context = context;
    }
    
    public bool Login(string email, string password)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == email);

        if (user == null) return false;

        if (!user.Email.IsValidEmail()) return false;
        
        if (user.PasswordHash != AuthenticationHelper.GenerateHash(password, user.Salt))
            return false;

        return true;
    }
}