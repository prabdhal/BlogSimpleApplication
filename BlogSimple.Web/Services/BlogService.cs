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

    public List<Blog> GetAll()
    {
        return _blogs.Find(_ => true).ToList();
    }

    public List<Blog> GetAll(string searchString)
    {
        var search = searchString.ToLower();

        // Need to filter by contains text
        var filterSearch = Builders<Blog>.Filter.Where(b => b.Title.ToLower().Contains(search) |
            b.Description.ToLower().Contains(search) |
            b.Category.ToString().ToLower().Contains(search) |
            b.Content.ToLower().Contains(search));

        return _blogs.Find(filterSearch).ToList();
    }

    public List<Blog> GetPublishedOnly(string searchString)
    {
        var search = searchString.ToLower();

        // Need to filter by contains text
        var filterSearch = Builders<Blog>.Filter.Where(b => b.Title.ToLower().Contains(search) |
            b.Description.ToLower().Contains(search) |
            b.Category.ToString().ToLower().Contains(search) |
            b.Content.ToLower().Contains(search));

        var filterByPublished = Builders<Blog>.Filter.Where(b => b.isPublished);

        return _blogs.Find(filterSearch & filterByPublished).ToList();
    }

    public Blog Get(string id)
    {
        return _blogs.Find(blog => blog.Id == id).FirstOrDefault();
    }

    public Blog Create(Blog blog)
    {
        _blogs.InsertOne(blog);
        return blog;
    }

    public Blog Update(string id, Blog blog)
    {
        _blogs.ReplaceOne(blog => blog.Id == id, blog);
        return blog;
    }

    public void Remove(string id)
    {
        _blogs.DeleteOne(blog => blog.Id == id);
    }

    public void Remove(Blog blog)
    {
        _blogs.DeleteOne(b => b == blog);
    }
}
