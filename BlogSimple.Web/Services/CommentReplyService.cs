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

    public async Task<CommentReply> Get(string replyId)
    {
        return await _replies.Find(r => r.Id == replyId).FirstOrDefaultAsync();
    }

    public async Task<List<CommentReply>> GetAll()
    {
        return await _replies.Find(_ => true).ToListAsync();
    }

    public async Task<List<CommentReply>> GetAllByUser(User user)
    {
        // Need to filter by user comments
        var filterSearch = Builders<CommentReply>.Filter.Where(c => c.CreatedById == user.Id);

        return await _replies.Find(filterSearch).ToListAsync();
    }

    public async Task<List<CommentReply>> GetAllByComment(string commentId)
    {
        return await _replies.Find(r => r.RepliedCommentId == commentId).ToListAsync();
    }

    public async Task<List<CommentReply>> GetAllByPost(string blogId)
    {
        return await _replies.Find(r => r.RepliedPostId == blogId).ToListAsync();
    }

    public async Task<CommentReply> Create(CommentReply reply)
    {
        await _replies.InsertOneAsync(reply);
        return reply;
    }

    public async Task<CommentReply> Update(string replyId, CommentReply reply)
    {
        await _replies.ReplaceOneAsync(r => r.Id == replyId, reply);
        return reply;
    }

    public async Task<CommentReply> UpdateForRemoval(string replyId, CommentReply reply)
    {
        await _replies.ReplaceOneAsync(r => r.Id == replyId, reply);
        return reply;
    }

    public async void Remove(string replyId)
    {
        await _replies.DeleteOneAsync(r => r.Id == replyId);
    }

    public async void Remove(CommentReply reply)
    {
        await _replies.DeleteOneAsync(r => r == reply);
    }

    public async void RemoveAllByComment(string commentId)
    {
        await _replies.DeleteManyAsync(r => r.RepliedCommentId == commentId);
    }
    public async void RemoveAllByPost(string blogId)
    {
        await _replies.DeleteManyAsync(r => r.RepliedPostId == blogId);
    }
}
