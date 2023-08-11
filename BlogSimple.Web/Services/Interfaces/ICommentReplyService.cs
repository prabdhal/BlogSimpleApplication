using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface ICommentReplyService
{
    CommentReply Get(string replyId);
    List<CommentReply> GetAll();
    List<CommentReply> GetAllByComment(string commentId);
    List<CommentReply> GetAllByBlog(string blogId);
    CommentReply Create(CommentReply reply);
    CommentReply Update(string replyId, CommentReply reply);
    void Remove(string replyId);
    void Remove(CommentReply reply);
    void RemoveAllByComment(string commentId);
    void RemoveAllByBlog(string blogId);
}
