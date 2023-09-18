using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;

namespace BlogSimple.Model.Services;

public class PostService : IPostService
{
    private readonly IMongoCollection<Post> _posts;

    public PostService(
        IPostSimpleDatabaseSettings postSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(postSettings.DatabaseName);
        _posts = db.GetCollection<Post>(postSettings.PostsCollectionName);
    }

    public async Task<List<Post>> GetAll()
    {
        return await _posts.Find(_ => true).ToListAsync();
    }

    public async Task<List<Post>> GetAllByUser(User user)
    {
        return await _posts.Find(p => p.CreatedBy.Id == user.Id).ToListAsync();
    }

    public async Task<List<Post>> GetAll(string searchString)
    {
        var search = searchString.ToLower();

        // Need to filter by contains text
        var filterSearch = Builders<Post>.Filter.Where(b => b.Title.ToLower().Contains(search) |
            b.Description.ToLower().Contains(search) |
            b.Category.ToString().ToLower().Contains(search) |
            b.Content.ToLower().Contains(search));

        return await _posts.Find(filterSearch).ToListAsync();
    }

    public async Task<List<Post>> GetAll(User user)
    {
        // Need to filter by by user posts
        var filterSearch = Builders<Post>.Filter.Where(b => b.CreatedBy.UserName == user.UserName & b.IsPublished == true);

        return await _posts.Find(filterSearch).ToListAsync();
    }

    public async Task<List<Post>> GetPublishedOnly(string searchString)
    {
        var search = searchString.ToLower();

        // Need to filter by contains text
        var filterSearch = Builders<Post>.Filter.Where(b => b.Title.ToLower().Contains(search) |
            b.Description.ToLower().Contains(search) |
            b.Category.ToString().ToLower().Contains(search) |
            b.Content.ToLower().Contains(search));

        var filterByPublished = Builders<Post>.Filter.Where(b => b.IsPublished);

        return await _posts.Find(filterSearch & filterByPublished).ToListAsync();
    }

    public async Task<Post> Get(string id)
    {
        return await _posts.Find(post => post.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Post> Create(Post post)
    {
        await _posts.InsertOneAsync(post);
        return post;
    }

    public async Task<Post> Update(string id, Post post)
    {
        await _posts.ReplaceOneAsync(post => post.Id == id, post);
        return post;
    }

    public async void Remove(string id)
    {
        await _posts.DeleteOneAsync(post => post.Id == id);
    }

    public async void Remove(Post post)
    {
        await _posts.DeleteOneAsync(b => b == post);
    }
}
