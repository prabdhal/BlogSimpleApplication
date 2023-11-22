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
        userEmailOptions.Body = UpdatePlaceHolders(GetEmailConfirmBody(), userEmailOptions.PlaceHolders);

        await SendEmail(userEmailOptions);
    }

    public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
    {
        userEmailOptions.Subject = UpdatePlaceHolders("BlogSimple Password Reset", userEmailOptions.PlaceHolders);
        userEmailOptions.Body = UpdatePlaceHolders(GetForgotPasswordBody(), userEmailOptions.PlaceHolders);

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
        // Need to see why this doesn't work in production... resulting in server error 500 
        var body = File.ReadAllText(string.Format(templatePath, templateName));
        return body;
    }

    private string GetForgotPasswordBody()
    {
        string body = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body>\r\n    <p>\r\n        Hello {{FirstName}} {{LastName}},\r\n        <br />\r\n        <br />\r\n        Welcome to BlogSimple!\r\n        <br />\r\n        You have requested for a password reset on BlogSimple.\r\n        Click on the following link to reset your password:\r\n        <a href=\"{{Link}}\">Reset Password</a>.\r\n        <br />\r\n        <br />\r\n        Regards,\r\n        <br />\r\n        BlogSimple Team\r\n    </p>\r\n</body>\r\n</html>";

        return body;
    }

    private string GetEmailConfirmBody()
    {
        string body = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title></title>\r\n</head>\r\n<body>\r\n    <p>\r\n        Hello {{FirstName}} {{LastName}},\r\n        <br />\r\n        <br />\r\n        Welcome to BlogSimple!\r\n        <br />\r\n        You have created a new account on BlogSimple. \r\n        Click on the following link to verify you email address: \r\n        <a href=\"{{Link}}\">Verify Email</a>.\r\n        <br />\r\n        <br />\r\n        Regards,\r\n        <br />\r\n        BlogSimple Team\r\n    </p>\r\n</body>\r\n</html>";

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
