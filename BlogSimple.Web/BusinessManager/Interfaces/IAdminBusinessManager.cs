using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.AdminViewModels;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IAdminBusinessManager
{
    Task<MyAdminViewModel> GetMyAdminViewModel(ClaimsPrincipal claimsPrincipal);
    Task<UserRole> CreateRole(string roleName);
    void DeleteRole(string id);
}
