using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface ISendGridEmailService
{
    Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);
    Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
}
