using BlogSimple.Model.ViewModels.AccountViewModels;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IAccountBusinessManager
{
    Task<AuthorViewModel> GetAuthorViewModel(string id, ClaimsPrincipal claimsPrincipal);
    Task<AuthorViewModel> GetAuthorViewModelForSignedInUser(ClaimsPrincipal claimsPrincipal);
    Task<AuthorViewModel> EditUser(AuthorViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal);
}
