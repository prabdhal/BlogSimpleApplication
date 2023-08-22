using BlogSimple.Model.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IAccountBusinessManager
{
    Task<AboutMeViewModel> GetAboutMeViewModel(ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<AboutMeViewModel>> EditUser(AboutMeViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal);
}
