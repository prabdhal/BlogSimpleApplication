using BlogSimple.Model.ViewModels.AccountViewModels;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IAccountBusinessManager
{
    Task<MyAccountViewModel> GetMyAccountViewModel(ClaimsPrincipal claimsPrincipal);
    Task<AuthorViewModel> GetAuthorViewModel(string userId);
    Task<AuthorViewModel> GetAuthorViewModelForSignedInUser(ClaimsPrincipal claimsPrincipal);
    Task<AuthorViewModel> EditUser(AuthorViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal);
}
