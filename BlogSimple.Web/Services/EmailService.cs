using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BlogSimple.Web.Services;

public class EmailService : IEmailService
{
    private const string templatePath = @"EmailTemplate/{0}.html";
    private readonly SMTPConfigModel _smtpConfig;

    public EmailService(IOptions<SMTPConfigModel> smtpConfig)
    {
        _smtpConfig = smtpConfig.Value;
    }

    public async Task SendTestEmail(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = "BlogSimple Verification";
        userEmailOptions.Body = GetEmailBody("TestEmail");

        await SendEmail(userEmailOptions);
    }

    private async Task SendEmail(UserEmailOptions userEmailOptions)
    {
        MailMessage mail = new MailMessage
        {
            Subject = userEmailOptions.Subject,
            Body = userEmailOptions.Body,
            From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
            IsBodyHtml = _smtpConfig.IsBodyHTML
        };

        foreach (var toEmail in userEmailOptions.ToEmails)
        {
            mail.To.Add(toEmail);
        }

        NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

        SmtpClient smtpClient = new SmtpClient
        {
            Host = _smtpConfig.Host,
            Port = _smtpConfig.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = networkCredential
        };

        mail.BodyEncoding = Encoding.Default;

        await smtpClient.SendMailAsync(mail);
    }

    private string GetEmailBody(string templateName)
    {
        var body = File.ReadAllText(string.Format(templatePath, templateName));
        return body;
    }
}
