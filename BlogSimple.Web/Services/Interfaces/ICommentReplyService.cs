using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface ICommentReplyService
{
    Task<CommentReply> Get(string replyId);
    Task<List<CommentReply>> GetAll();
    Task<List<CommentReply>> GetAllByComment(string commentId);
    Task<List<CommentReply>> GetAllByBlog(string blogId);
    Task<CommentReply> Create(CommentReply reply);
    Task<CommentReply> Update(string replyId, CommentReply reply);
    void Remove(string replyId);
    void Remove(CommentReply reply);
    void RemoveAllByComment(string commentId);
    void RemoveAllByBlog(string blogId);
}
