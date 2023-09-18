using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface ICommentReplyService
{
    Task<CommentReply> Get(string replyId);
    Task<List<CommentReply>> GetAll();
    Task<List<CommentReply>> GetAllByUser(User user);
    Task<List<CommentReply>> GetAllByComment(string commentId);
    Task<List<CommentReply>> GetAllByPost(string blogId);
    Task<CommentReply> Create(CommentReply reply);
    Task<CommentReply> Update(string replyId, CommentReply reply);
    Task<CommentReply> UpdateForRemoval(string replyId, CommentReply reply);
    void Remove(string replyId);
    void Remove(CommentReply reply);
    void RemoveAllByComment(string commentId);
    void RemoveAllByPost(string blogId);
}
