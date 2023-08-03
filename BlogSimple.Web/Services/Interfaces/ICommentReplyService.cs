using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface ICommentReplyService
{
    List<CommentReply> GetAll(string commentId);
    CommentReply Get(string commentId);
    CommentReply Create(CommentReply comment);
    CommentReply Update(string commentId, CommentReply comment);
    void Remove(CommentReply comment);
    void RemoveAllByComment(string commentId);
}
