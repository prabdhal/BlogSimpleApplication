using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface IEmailService
{
    Task SendTestEmail(UserEmailOptions userEmailOptions);
}
