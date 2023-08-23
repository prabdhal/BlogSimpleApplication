using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;

namespace BlogSimple.Web.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(
        IBlogSimpleDatabaseSettings blogSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(blogSettings.DatabaseName);
        _users = db.GetCollection<User>(blogSettings.UsersCollectionName);
    }
    public async Task<User> Create(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User> Get(string id)
    {
        var user = await _users.Find(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();
        return user;
    }

    public async Task<List<User>> GetAll()
    {
        return await _users.Find(_ => true).ToListAsync();
    }

    public async void Remove(string userName)
    {
        await _users.DeleteOneAsync(u => u.UserName == userName);
    }

    public async Task<User> Update(string userName, User user)
    {
        await _users.ReplaceOneAsync(u => u.UserName == userName, user);
        return user;
    }
}
