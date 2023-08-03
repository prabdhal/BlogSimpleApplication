using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    public BlogDetailsViewModel GetDashboardDetailViewModel(string id)
    {
        var blog = _blogService.Get(id);
        List<string> blogCats = new List<string>();
        var comments = _commentService.GetAll(blog.Id);
        var replies = new List<CommentReply>();
        foreach (var comment in comments)
        {
            replies = _commentReplyService.GetAll(comment.Id);
        }

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
            Comments = comments,
            CommentReplies = replies
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

        // get and update comment into blog comments list
        var blog = _blogService.Get(blogDetailsViewModel.Blog.Id);
        blog.Comments.Add(comment);

        _blogService.Update(blogDetailsViewModel.Blog.Id, blog);

        return comment;
    }

    public EditBlogViewModel GetEditBlogViewModel(string id)
    {
        var blog = _blogService.Get(id);

        return new EditBlogViewModel
        {
            Blog = blog
        };
    }

    public ActionResult<EditBlogViewModel> EditBlog(EditBlogViewModel editBlogViewModel, ClaimsPrincipal claimsPrincipal)
    {
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

    public ActionResult<Blog> DeleteBlog(string id)
    {
        var blog = _blogService.Get(id);
        if (blog is null)
            return new NotFoundResult();

        var comments = _commentService.GetAll(blog.Id);

        foreach (var comment in comments)
        {
            _commentReplyService.RemoveAllByComment(comment.Id);
        }

        _commentService.RemoveAllByBlog(blog.Id);

        _blogService.Remove(blog);
        return blog;
    }
}
