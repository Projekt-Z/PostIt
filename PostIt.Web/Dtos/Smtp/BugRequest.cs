namespace PostIt.Web.Dtos.Smtp;

public class BugRequest
{
    public string Email { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}