using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BlogSimple.Web.Services;

public class SendGridEmailService : ISendGridEmailService
{
    private const string templatePath = @"EmailTemplate/{0}.html";
    private readonly ISendGridConfig _sendGridConfig;

    public SendGridEmailService(ISendGridConfig sendGridConfig)
    {
        _sendGridConfig = sendGridConfig;
    }


    public async Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = UpdatePlaceHolders("BlogSimple Verification", userEmailOptions.PlaceHolders);
        userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailOptions.PlaceHolders);

        await SendEmail(userEmailOptions);
    }

    public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = "BlogSimple Password Reset";//UpdatePlaceHolders("BlogSimple Password Reset", userEmailOptions.PlaceHolders);
        userEmailOptions.Body = "ForgotPassword"; //UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

        await SendEmail(userEmailOptions);
    }

    private async Task SendEmail(UserEmailOptions userEmailOptions)
    {
        var apiKey = _sendGridConfig.APIKey;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_sendGridConfig.SenderAddress, _sendGridConfig.SenderDisplayName);
        var to = new EmailAddress(userEmailOptions.ToEmail, userEmailOptions.FullName);
        var subject = userEmailOptions.Subject;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, userEmailOptions.Body, userEmailOptions.Body);
        var response = await client.SendEmailAsync(msg);
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
