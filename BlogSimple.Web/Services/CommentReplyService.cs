using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;

namespace BlogSimple.Web.Services;

public class CommentReplyService : ICommentReplyService
{
    private readonly IMongoCollection<CommentReply> _replies;

    public CommentReplyService(
        IBlogSimpleDatabaseSettings blogSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(blogSettings.DatabaseName);
        _replies = db.GetCollection<CommentReply>(blogSettings.RepliesCollectionName);
    }

    public CommentReply Get(string commentId)
    {
        return _replies.Find(c => c.Id == commentId).FirstOrDefault();
    }

    public List<CommentReply> GetAll(string commentId)
    {
        return _replies.Find(c => c.RepliedComment.Id == commentId).ToList();
    }

    public CommentReply Create(CommentReply comment)
    {
        _replies.InsertOne(comment);
        return comment;
    }

    public CommentReply Update(string commentId, CommentReply comment)
    {
        _replies.ReplaceOne(commentId, comment);
        return comment;
    }

    public void Remove(CommentReply comment)
    {
        _replies.DeleteOne(c => c == comment);
    }

    public void RemoveAllByComment(string commentId)
    {
        _replies.DeleteMany(c => c.RepliedComment.Id == commentId);
    }
}
