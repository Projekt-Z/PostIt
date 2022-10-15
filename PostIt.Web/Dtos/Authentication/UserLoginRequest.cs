namespace PostIt.Web.Dtos.Authentication;

public class UserLoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
