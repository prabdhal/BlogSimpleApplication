using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogSimple.Web.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(
        IPostSimpleDatabaseSettings blogSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(blogSettings.DatabaseName);
        _users = db.GetCollection<User>(blogSettings.UsersCollectionName);

        //var update = Builders<User>.Update.Set("Bio", new List<User>());
        //var remove = Builders<User>.Update.Unset("Content");
        //var filter = Builders<User>.Filter.Empty;
        //var options = new UpdateOptions { IsUpsert = true };

        //_users.UpdateMany(filter, remove, options);
        //_users.UpdateMany(filter, update, options);
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

    public async Task<User> Get(Guid id)
    {
        var user = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        return user;
    }

    public async Task<List<User>> GetAll()
    {
        return await _users.Find(_ => true).ToListAsync();
    }

    public async Task<User> Update(string userName, User user)
    {
        //var filter = Builders<User>.Filter.Eq("UserName", userName);
        //var update = Builders<User>.Update
        //    .Set(u => u.FirstName, user.FirstName)
        //    .Set(u => u.LastName, user.LastName)
        //    .Set(u => u.UserName, user.UserName)
        //    .Set(u => u.Email, user.Email)
        //    .Set(u => u.EmailConfirmed, user.EmailConfirmed)
        //    .Set(u => u.Password, user.Password)
        //    .Set(u => u.ConfirmPassword, user.ConfirmPassword)
        //    .Set(u => u.PasswordHash, user.PasswordHash)
        //    .Set(u => u.Tokens, user.Tokens)
        //    .Set(u => u.Heading, user.Heading)
        //    .Set(u => u.Bio, user.Bio)
        //    .Set(u => u.CreatedOn, user.CreatedOn)
        //    .Set(u => u.FavoritedPosts, user.FavoritedPosts)
        //    .Set(u => u.GitHubLink, user.GitHubLink)
        //    .Set(u => u.LinkedInLink, user.LinkedInLink)
        //    .Set(u => u.PortfolioLink, user.PortfolioLink)
        //    .Set(u => u.TwitterLink, user.TwitterLink)
        //    .Set(u => u.HeaderImage, user.HeaderImage)
        //    .Set(u => u.ProfilePicture, user.ProfilePicture)
        //    .Set(u => u.Roles, user.Roles);
        //await _users.UpdateOneAsync(filter, update);
        await _users.ReplaceOneAsync(u => u.UserName == userName, user);
        return user;
    }

    public async void Remove(Guid id)
    {
        await _users.DeleteOneAsync(u => u.Id == id);
    }
}
