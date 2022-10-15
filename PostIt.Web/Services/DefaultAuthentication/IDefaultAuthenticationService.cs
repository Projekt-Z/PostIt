namespace PostIt.Web.Services.DefaultAuthentication;

public interface IDefaultAuthenticationService
{
    bool Login(string email, string password);
}