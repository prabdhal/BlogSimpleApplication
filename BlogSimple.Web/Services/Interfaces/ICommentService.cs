using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface ICommentService
{
    List<Comment> GetAll();
    List<Comment> GetAllByBlog(string blogId);
    Comment Get(string commentId);
    Comment Create(Comment comment);
    Comment Update(string commentId, Comment comment);
    void Remove(string commentId);
    void Remove(Comment comment);
    void RemoveAllByBlog(string blogId);
}
