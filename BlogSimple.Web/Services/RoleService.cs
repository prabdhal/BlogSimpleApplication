using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace BlogSimple.Web.Services;

public class RoleService : IRoleService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMongoCollection<UserRole> _roles;

    public RoleService(
        IServiceProvider serviceProvider,
        IBlogSimpleDatabaseSettings blogSettings,
        IMongoClient mongoClient
        )
    {
        _serviceProvider = serviceProvider;
        var db = mongoClient.GetDatabase(blogSettings.DatabaseName);
        _roles = db.GetCollection<UserRole>(blogSettings.RolesCollectionName);
    }

    public async Task InitializeRoles()
    {
        var _roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roleNames = { "UnVerifiedUser", "VerifiedUser", "Admin" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public async Task<UserRole> Create(UserRole role)
    {
        await _roles.InsertOneAsync(role);
        return role;
    }

    public async Task<UserRole> Get(string id)
    {
        return await _roles.Find(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();
    }

    public async Task<List<UserRole>> GetAll()
    {
        return await _roles.Find(_ => true).ToListAsync();
    }
}
