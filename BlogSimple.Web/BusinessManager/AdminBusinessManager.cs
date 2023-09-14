using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.AdminViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager;

public class AdminBusinessManager : IAdminBusinessManager
{
    private UserManager<User> _userManager;
    private readonly IRoleService _roleService;

    public AdminBusinessManager(
        UserManager<User> userManager,
        IRoleService roleService
        )
    {
        _roleService = roleService;
        _userManager = userManager;
    }

    public async Task<MyAdminViewModel> GetMyAdminViewModel(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var roles = await _roleService.GetAll();

        return new MyAdminViewModel
        {
            AccountUser = user,
            UserRole = new UserRole(),
            UserRoles = roles
        };
    }

    [HttpPost]
    public async Task<UserRole> CreateRole(string roleName)
    {
        UserRole role = new UserRole
        {
            Name = roleName,
            NormalizedName = roleName.ToUpper(),
            RoleName = roleName
        };
        return await _roleService.Create(role);
    }

    public void DeleteRole(string id)
    {
        _roleService.Remove(id);
    }
}
