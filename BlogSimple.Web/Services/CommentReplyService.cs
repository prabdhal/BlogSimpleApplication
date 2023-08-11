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

    public CommentReply Get(string replyId)
    {
        return _replies.Find(r => r.Id == replyId).FirstOrDefault();
    }

    public List<CommentReply> GetAllByComment(string commentId)
    {
        var r = _replies.Find(r => r.RepliedComment.Id == commentId).ToList();
        return r;
    }

    public CommentReply Create(CommentReply reply)
    {
        _replies.InsertOne(reply);
        return reply;
    }

    public CommentReply Update(string replyId, CommentReply reply)
    {
        _replies.ReplaceOne(r => r.Id == replyId, reply);
        return reply;
    }

    public void Remove(string replyId)
    {
        _replies.DeleteOne(r => r.Id == replyId);
    }

    public void Remove(CommentReply reply)
    {
        _replies.DeleteOne(r => r == reply);
    }

    public void RemoveAllByComment(string commentId)
    {
        _replies.DeleteMany(r => r.RepliedComment.Id == commentId);
    }
}
