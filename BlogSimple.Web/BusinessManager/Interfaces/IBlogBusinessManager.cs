using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IBlogBusinessManager
{
    Task<DashboardIndexViewModel> GetDashboardIndexViewModel(string searchString, ClaimsPrincipal claimsPrincipal);
    Task<BlogDetailsViewModel> GetBlogDetailsViewModel(string id);
    Task<Blog> CreateBlog(CreateBlogViewModel createViewModel, ClaimsPrincipal claimsPrincipal);
    Task<Comment> CreateComment(BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    Task<CommentReply> CreateReply(BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    Task<EditBlogViewModel> GetEditBlogViewModel(string blogId);
    Task<EditBlogViewModel> GetEditBlogViewModelViaComment(string commentId);
    Task<EditBlogViewModel> GetEditBlogViewModelViaReply(string replyId);
    Task<ActionResult<EditBlogViewModel>> EditBlog(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<BlogDetailsViewModel>> EditComment(string commentId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<BlogDetailsViewModel>> EditReply(string commentId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<BlogDetailsViewModel>> LikeComment(string commentId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal);
    Task<ActionResult<Blog>> DeleteBlog(string blogId);
    void DeleteComment(string commentId);
    void DeleteReply(string replyId);
}
