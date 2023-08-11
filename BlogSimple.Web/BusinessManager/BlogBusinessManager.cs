using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager;

public class BlogBusinessManager : IBlogBusinessManager
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBlogService _blogService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _commentReplyService;

    public BlogBusinessManager(
        UserManager<ApplicationUser> userManager,
        IBlogService blogService,
        ICommentService commentService,
        ICommentReplyService commentReplyService
        )
    {
        _userManager = userManager;
        _blogService = blogService;
        _commentService = commentService;
        _commentReplyService = commentReplyService;
    }

    public async Task<DashboardIndexViewModel> GetDashboardIndexViewModel(string searchString, ClaimsPrincipal claimsPrincipal)
    {
        IEnumerable<Blog> blogs = _blogService.GetAll(searchString ?? string.Empty);

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        var userBlogs = blogs.Where(b => b.CreatedBy.Email == user.Email);

        return new DashboardIndexViewModel
        {
            UserBlogs = userBlogs,
        };
    }

    public BlogDetailsViewModel GetDashboardDetailViewModel(string blogId)
    {
        var blog = _blogService.Get(blogId);
        List<string> blogCats = new List<string>();
        var comments = _commentService.GetAllByBlog(blogId);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comments = comments
        };
    }

    public async Task<Blog> CreateBlog(CreateBlogViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
    {
        Blog blog = createViewModel.Blog;

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        blog.CreatedBy = user;
        blog.CreatedOn = DateTime.Now;
        blog.UpdatedOn = DateTime.Now;

        blog = _blogService.Create(blog);

        return blog;
    }

    public async Task<Comment> CreateComment(BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        Comment comment = blogDetailsViewModel.Comment;

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        comment.CommentedBlog = blogDetailsViewModel.Blog;
        comment.CreatedBy = user;
        comment.CreatedOn = DateTime.Now;
        comment.UpdatedOn = DateTime.Now;

        comment = _commentService.Create(comment);

        return comment;
    }

    public async Task<CommentReply> CreateReply(BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        CommentReply reply = blogDetailsViewModel.CommentReply;

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        var blog = _blogService.Get(blogDetailsViewModel.Blog.Id);
        
        var comment = _commentService.Get(blogDetailsViewModel.Comment.Id);

        reply.RepliedBlog = blog;
        //reply.RepliedComment = comment;
        reply.CreatedBy = user;
        reply.CreatedOn = DateTime.Now;
        reply.UpdatedOn = DateTime.Now;

        reply = _commentReplyService.Create(reply);

        comment.Replies.Add(reply);
        _commentService.Update(comment.Id, comment);

        return reply;
    }

    public EditBlogViewModel GetEditBlogViewModel(string blogId)
    {
        var blog = _blogService.Get(blogId);

        return new EditBlogViewModel
        {
            Blog = blog
        };
    }

    public EditBlogViewModel GetEditBlogViewModelViaComment(string commentId)
    {
        var comment = _commentService.Get(commentId);
        var blog = _blogService.Get(comment.CommentedBlog.Id);

        return new EditBlogViewModel
        {
            Blog = blog
        };
    }

    public EditBlogViewModel GetEditBlogViewModelViaReply(string replyId)
    {
        var reply = _commentReplyService.Get(replyId);
        var comment = _commentService.Get(reply.RepliedComment.Id);
        var blog = _blogService.Get(comment.CommentedBlog.Id);

        return new EditBlogViewModel
        {
            Blog = blog
        };
    }

    public async Task<ActionResult<EditBlogViewModel>> EditBlog(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var blog = _blogService.Get(editBlogViewModel.Blog.Id);
        if (blog is null)
            return new NotFoundResult();

        blog.Title = editBlogViewModel.Blog.Title;
        blog.Category = editBlogViewModel.Blog.Category;
        blog.Description = editBlogViewModel.Blog.Description;
        blog.Content = editBlogViewModel.Blog.Content;
        blog.isPublished = editBlogViewModel.Blog.isPublished;
        blog.UpdatedOn = DateTime.Now;


        return new EditBlogViewModel
        {
            Blog = _blogService.Update(editBlogViewModel.Blog.Id, blog)
        };
    }

    public async Task<ActionResult<BlogDetailsViewModel>> EditComment(string commentId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var blog = _blogService.Get(blogDetailsViewModel.Blog.Id);
        if (blog is null)
            return new NotFoundResult();

        var comment = _commentService.Get(commentId);
        if (comment is null)
            return new NotFoundResult();

        comment.Content = blogDetailsViewModel.Comment.Content;

        List<string> blogCats = new List<string>();
        var comments = _commentService.GetAllByBlog(blog.Id);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comment = _commentService.Update(commentId, comment),
            Comments = comments,
        };
    }

    public async Task<ActionResult<BlogDetailsViewModel>> EditReply(string replyId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var blog = _blogService.Get(blogDetailsViewModel.Blog.Id);
        if (blog is null)
            return new NotFoundResult();

        var comment = _commentService.Get(blogDetailsViewModel.Comment.Id);
        if (comment is null)
            return new NotFoundResult();

        var reply = _commentReplyService.Get(replyId);
        if (reply is null)
            return new NotFoundResult();

        reply.Content = blogDetailsViewModel.CommentReply.Content;

        CommentReply cr = comment.Replies.Where(r => r.Id == replyId).FirstOrDefault();
        
        if (comment.Replies.Contains(cr))
        {
            comment.Replies.Remove(cr);
            comment.Replies.Add(reply);
        }

        List<string> blogCats = new List<string>();
        var comments = _commentService.GetAllByBlog(blog.Id);

        _commentService.Update(comment.Id, comment);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comment = comment,
            Comments = comments,
            CommentReply = _commentReplyService.Update(replyId, reply),
        };
    }

    public async Task<ActionResult<BlogDetailsViewModel>> LikeComment(string commentId, BlogDetailsViewModel blogDetailsViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        var blog = _blogService.Get(blogDetailsViewModel.Blog.Id);
        if (blog is null)
            return new NotFoundResult();

        var comment = _commentService.Get(commentId);
        if (comment is null)
            return new NotFoundResult();

        var userAlreadyLiked = comment.CommentLikedByUsers.Where(u => u.Id == user.Id).FirstOrDefault();

        if (userAlreadyLiked is null)
        {
            comment.CommentLikedByUsers.Add(user);
        }
        else
        {
            comment.CommentLikedByUsers.Remove(userAlreadyLiked);
        }

        List<string> blogCats = new List<string>();
        var comments = _commentService.GetAllByBlog(blog.Id);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comment = _commentService.Update(commentId, comment),
            Comments = comments,
        };
    }

    public ActionResult<Blog> DeleteBlog(string blogId)
    {
        var blog = _blogService.Get(blogId);
        if (blog is null)
            return new NotFoundResult();

        var comments = _commentService.GetAllByBlog(blogId);
        var blogRelatedComments = new List<Comment>();

        foreach (Comment comment in comments)
        {
            if (comment.CommentedBlog.Id == blog.Id)
            {
                blogRelatedComments.Add(comment);
            }
        }

        foreach (var comment in blogRelatedComments)
        {
            _commentReplyService.RemoveAllByComment(comment.Id);
        }

        _commentService.RemoveAllByBlog(blog.Id);

        _blogService.Remove(blog);
        return blog;
    }

    public void DeleteComment(string commentId)
    {
        _commentService.Remove(commentId);
    }

    public void DeleteReply(string replyId)
    {
        _commentReplyService.Remove(replyId);
    }
}
