namespace PostIt.Web.Dtos.Smtp;

public class MailRequestDto
{
    public string ToAddress { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;
}