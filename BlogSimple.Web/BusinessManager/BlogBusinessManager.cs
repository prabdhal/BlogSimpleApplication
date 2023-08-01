using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager;

public class BlogBusinessManager : IBlogBusinessManager
{
    private readonly IBlogService _blogService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BlogBusinessManager(
        UserManager<ApplicationUser> userManager,
        IBlogService blogService
        )
    {
        _blogService = blogService;
        _userManager = userManager;
    }

    public async Task<DashboardIndexViewModel> GetDashboardIndexViewModel(string searchString, ClaimsPrincipal claimsPrincipal)
    {
        IEnumerable<Blog> blogs = _blogService.GetAll(searchString ?? string.Empty);

        var user = await _userManager.GetUserAsync(claimsPrincipal);

        var userBlogs = blogs.Where(b => b.CreatedBy.Email == user.Email);

        return new DashboardIndexViewModel
        {
            Blogs = userBlogs,
        };
    }

    public BlogDetailsViewModel GetDashboardDetailViewModel(string id)
    {
        var blog = _blogService.Get(id);
        List<string> blogCats = new List<string>();

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            BlogCategories = blogCats,
            Blog = blog,
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
        blog.Description = editBlogViewModel.Blog.Description;
        blog.Content = editBlogViewModel.Blog.Content;
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

        _blogService.Remove(blog);
        return blog;
    }
}
