using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;

namespace BlogSimple.Model.Services;

public class BlogService : IBlogService
{
    private readonly IMongoCollection<Blog> _blogs;

    public BlogService(
        IBlogSimpleDatabaseSettings blogSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(blogSettings.DatabaseName);
        _blogs = db.GetCollection<Blog>(blogSettings.BlogsCollectionName);
    }

    public async Task<List<Blog>> GetAll()
    {
        return await _blogs.Find(_ => true).ToListAsync();
    }

    public async Task<List<Blog>> GetAll(string searchString)
    {
        var search = searchString.ToLower();

        // Need to filter by contains text
        var filterSearch = Builders<Blog>.Filter.Where(b => b.Title.ToLower().Contains(search) |
            b.Description.ToLower().Contains(search) |
            b.Category.ToString().ToLower().Contains(search) |
            b.Content.ToLower().Contains(search));

        return await _blogs.Find(filterSearch).ToListAsync();
    }

    public async Task<List<Blog>> GetPublishedOnly(string searchString)
    {
        var search = searchString.ToLower();

        // Need to filter by contains text
        var filterSearch = Builders<Blog>.Filter.Where(b => b.Title.ToLower().Contains(search) |
            b.Description.ToLower().Contains(search) |
            b.Category.ToString().ToLower().Contains(search) |
            b.Content.ToLower().Contains(search));

        var filterByPublished = Builders<Blog>.Filter.Where(b => b.isPublished);

        return await _blogs.Find(filterSearch & filterByPublished).ToListAsync();
    }

    public async Task<Blog> Get(string id)
    {
        return await _blogs.Find(blog => blog.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Blog> Create(Blog blog)
    {
        await _blogs.InsertOneAsync(blog);
        return blog;
    }

    public async Task<Blog> Update(string id, Blog blog)
    {
        await _blogs.ReplaceOneAsync(blog => blog.Id == id, blog);
        return blog;
    }

    public async void Remove(string id)
    {
        await _blogs.DeleteOneAsync(blog => blog.Id == id);
    }

    public async void Remove(Blog blog)
    {
        await _blogs.DeleteOneAsync(b => b == blog);
    }
}
