using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;
using static System.Reflection.Metadata.BlobBuilder;
using System.Reflection.Metadata;

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

    public Comment Get(string commentId)
    {
        return _comments.Find(c => c.Id == commentId).FirstOrDefault();
    }

    public List<Comment> GetAllByBlog(string blogId)
    {
        return _comments.Find(c => c.CommentedBlog.Id == blogId).ToList();
    }

    public Comment Create(Comment comment)
    {
        _comments.InsertOne(comment);
        return comment;
    }

    public Comment Update(string commentId, Comment comment)
    {
        _comments.ReplaceOne(comment => comment.Id == commentId, comment);
        return comment;
    }

    public void Remove(string commentId)
    {
        _comments.DeleteOne(c => c.Id == commentId);
    }

    public void Remove(Comment comment)
    {
        _comments.DeleteOne(c => c == comment);
    }

    public void RemoveAllByBlog(string blogId)
    {
        _comments.DeleteMany(c => c.CommentedBlog.Id == blogId);
    }
}
