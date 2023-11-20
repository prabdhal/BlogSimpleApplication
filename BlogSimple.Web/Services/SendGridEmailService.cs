using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BlogSimple.Web.Services;

public class SendGridEmailService : ISendGridEmailService
{
    private const string templatePath = @"EmailTemplate/{0}.html";
    private readonly SendGridConfigModel _sendGridConfig;

    public SendGridEmailService(IOptions<SendGridConfigModel> sendGridConfig)
    {
        _sendGridConfig = sendGridConfig.Value;
    }


    public async Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = UpdatePlaceHolders("BlogSimple Verification", userEmailOptions.PlaceHolders);
        userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailOptions.PlaceHolders);

        await SendEmail(userEmailOptions);
    }

    public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = UpdatePlaceHolders("BlogSimple Password Reset", userEmailOptions.PlaceHolders);
        userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

        await SendEmail(userEmailOptions);
    }

    private async Task SendEmail(UserEmailOptions userEmailOptions)
    {
        //List<EmailAddress> to = new List<EmailAddress>();
        //foreach (var toEmail in userEmailOptions.ToEmails)
        //{
        //    to.Add(new EmailAddress(toEmail, "Example User"));
        //}

        var apiKey = _sendGridConfig.APIKeyPassword;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_sendGridConfig.SenderAddress, _sendGridConfig.SenderDisplayName);
        var to = new EmailAddress(userEmailOptions.ToEmail, userEmailOptions.FullName);
        var subject = userEmailOptions.Subject;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, userEmailOptions.Body, userEmailOptions.Body);
        var response = await client.SendEmailAsync(msg);
        //Console.WriteLine(response.StatusCode);
        //Console.WriteLine(response.Body);
    }

    private string GetEmailBody(string templateName)
    {
        var body = File.ReadAllText(string.Format(templatePath, templateName));
        return body;
    }

    private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
    {
        if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
        {
            foreach (var placeholder in keyValuePairs)
            {
                if (text.Contains(placeholder.Key))
                {
                    text = text.Replace(placeholder.Key, placeholder.Value);
                }
            }
        }

        return text;
    }
}
