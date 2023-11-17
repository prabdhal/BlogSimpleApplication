using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IAccountBusinessManager
{
    Task<User> GetUserByEmailAsync(string email);
    Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model);
    Task<IdentityResult> CreateUserAsync(User user);
    Task<MyAccountViewModel> GetMyAccountViewModel(ClaimsPrincipal claimsPrincipal);
    Task<MyProfileViewModel> GetMyProfileViewModel(ClaimsPrincipal claimsPrincipal);
    Task<MyAchievementsViewModel> GetMyAchievementsViewModel(ClaimsPrincipal claimsPrincipal);
    Task<EmailConfirmViewModel> GetEmailConfirmViewModel(ClaimsPrincipal claimsPrincipal, EmailConfirmViewModel model);
    ChangePasswordViewModel GetChangePasswordViewModel();
    Task<AuthorViewModel> GetAuthorViewModel(string userId);
    Task<AuthorViewModel> GetAuthorViewModelForSignedInUser(ClaimsPrincipal claimsPrincipal);
    Task<AuthorViewModel> EditUser(AuthorViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal);
    Task<MyAccountViewModel> UpdateUserProfile(MyAccountViewModel myAccountViewModel, ClaimsPrincipal claimsPrincipal);
    Task SendEmailConfirmationEmail(User user, string token);
    Task SendForgotPasswordEmail(User user, string token);
    Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
    Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model, ClaimsPrincipal claimsPrincipal);
    Task GenerateEmailConfirmationTokenAsync(User user);
    Task GenerateForgotPasswordConfirmationTokenAsync(User user);
}
