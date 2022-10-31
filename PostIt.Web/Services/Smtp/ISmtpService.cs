using PostIt.Web.Dtos.Smtp;

namespace PostIt.Web.Services.Smtp;

public interface ISmtpService
{
    bool Send(MailRequestDto request);
    bool BugRequest(BugRequest request);
    bool Send(MailRequestDto request, string path);
}