using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using PostIt.Web.Dtos.Smtp;

namespace PostIt.Web.Services.Smtp;

public class SmtpService : ISmtpService
{
    public bool Send(MailRequestDto request)
    {
        var workingDirectory = Environment.CurrentDirectory;
        var serialized = File.ReadAllText(workingDirectory + @"/smtp.json");
        var json = JsonConvert.DeserializeObject<Settings>(serialized);

        var username = json!.Username;
        var password = json.Password;
        
        using var mailMessage = new MailMessage
        {
            From = new MailAddress(username),
            To = { request.ToAddress },
            Subject = request.Subject,
            Body = request.Body,
            IsBodyHtml = true
        };
        
        using var smtp = new SmtpClient
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            EnableSsl = true,
            Host = "smtp.gmail.com",
            Port = 587,
            Credentials = new NetworkCredential(username, password)
        };

        try
        {
            smtp.Send(mailMessage);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return false;
        }
    }

    public bool Send(MailRequestDto request, string path)
    {
        throw new NotImplementedException();
    }
}