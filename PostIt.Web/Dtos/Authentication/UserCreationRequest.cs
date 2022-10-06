namespace PostIt.Web.Dtos.Authentication;

public class UserCreationRequest
{
    public string Username { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string? Password { get; set; }
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
}