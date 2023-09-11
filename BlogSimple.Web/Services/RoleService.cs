using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;

namespace BlogSimple.Web.Services;

public class RoleService : IRoleService
{
    private readonly IMongoCollection<ApplicationRole> _roles;

    public RoleService(
        IBlogSimpleDatabaseSettings blogSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(blogSettings.DatabaseName);
        _roles = db.GetCollection<ApplicationRole>(blogSettings.RolesCollectionName);

        //var update = Builders<User>.Update.Set("FavoritedBlogs", new List<Blog>());
        //var remove = Builders<User>.Update.Unset("FavoriteBlogs");
        //var filter = Builders<User>.Filter.Empty;
        //var options = new UpdateOptions { IsUpsert = true };
        ApplicationRole verifiedUserRole = new ApplicationRole
        {
            Name = "VerifiedUser",
        };

        _roles.InsertOne(verifiedUserRole);

        //_roles.UpdateMany(filter, remove, options);
        //_roles.UpdateMany(filter, update, options);
    }

    public async Task<ApplicationRole> Create(ApplicationRole role)
    {
        await _roles.InsertOneAsync(role);
        return role;
    }

    public async Task<ApplicationRole> Get(string id)
    {
        return await _roles.Find(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();
    }

    public async Task<List<ApplicationRole>> GetAll()
    {
        return await _roles.Find(_ => true).ToListAsync();
    }
}
