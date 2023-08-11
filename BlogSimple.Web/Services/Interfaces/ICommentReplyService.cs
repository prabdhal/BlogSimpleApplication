using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface ICommentReplyService
{
    List<CommentReply> GetAllByComment(string commentId);
    CommentReply Get(string replyId);
    CommentReply Create(CommentReply reply);
    CommentReply Update(string replyId, CommentReply reply);
    void Remove(string replyId);
    void Remove(CommentReply reply);
    void RemoveAllByComment(string commentId);
}
