using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using myWebApp.ViewModels.Student;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailOptions = _configuration.GetSection("EmailSenderOptions").Get<EmailSenderOptions>();
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress("Seerat Education System", emailOptions.Username));
        mimeMessage.To.Add(new MailboxAddress("", email));
        mimeMessage.Subject = subject;
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = message;
        mimeMessage.Body = bodyBuilder.ToMessageBody();
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(emailOptions.Host, emailOptions.Port, false);
            await client.AuthenticateAsync(emailOptions.Username, emailOptions.Password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
