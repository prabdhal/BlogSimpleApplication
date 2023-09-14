using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogSimple.Web.Services.Interfaces;

public interface IRoleService
{
    Task InitializeRoles();
    Task<UserRole> Create(UserRole role);
    Task<UserRole> Get(string id);
    Task<List<UserRole>> GetAll();
    void Remove(string id);
}
