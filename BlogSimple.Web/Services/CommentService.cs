using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;

namespace BlogSimple.Web.Services;

public class CommentService : ICommentService
{
    private readonly IMongoCollection<Comment> _comments;

    public CommentService(
        IBlogSimpleDatabaseSettings blogSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(blogSettings.DatabaseName);
        _comments = db.GetCollection<Comment>(blogSettings.CommentsCollectionName);
    }

    public async Task<Comment> Get(string commentId)
    {
        return await _comments.Find(c => c.Id == commentId).FirstOrDefaultAsync();
    }

    public async Task<List<Comment>> GetAll()
    {
        return await _comments.Find(_ => true).ToListAsync();
    }

    public async Task<List<Comment>> GetAll(User user)
    {
        // Need to filter by user comments
        var filterSearch = Builders<Comment>.Filter.Where(c => c.CreatedBy.UserName == user.UserName);

        return await _comments.Find(filterSearch).ToListAsync();
    }

    public async Task<List<Comment>> GetAllByBlog(string blogId)
    {
        return await _comments.Find(c => c.CommentedBlog.Id == blogId).ToListAsync();
    }

    public async Task<Comment> Create(Comment comment)
    {
        await _comments.InsertOneAsync(comment);
        return comment;
    }

    public async Task<Comment> Update(string commentId, Comment comment)
    {
        await _comments.ReplaceOneAsync(comment => comment.Id == commentId, comment);
        return comment;
    }

    public async void Remove(string commentId)
    {
        await _comments.DeleteOneAsync(c => c.Id == commentId);
    }

    public async void Remove(Comment comment)
    {
        await _comments.DeleteOneAsync(c => c == comment);
    }

    public async void RemoveAllByBlog(string blogId)
    {
        await _comments.DeleteManyAsync(c => c.CommentedBlog.Id == blogId);
    }
}
