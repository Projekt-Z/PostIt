namespace PostIt.Web.Dtos.Authentication;

public class UserLoginRequest
{
    public string EmailOrRUsername { get; set; } = null!;
    public string Password { get; set; } = null!;
}
