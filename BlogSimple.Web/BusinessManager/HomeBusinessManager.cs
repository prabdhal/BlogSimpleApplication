using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;

namespace BlogSimple.Web.BusinessManager;

public class HomeBusinessManager : IHomeBusinessManager
{
    private readonly IBlogService _blogService;

    public HomeBusinessManager(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public BlogDetailsViewModel GetHomeDetailsViewModel(string id)
    {
        Blog blog = _blogService.Get(id);

        return new BlogDetailsViewModel
        {
            Blog = blog
        };
    }

    public HomeIndexViewModel GetHomeIndexViewModel(string searchString)
    {
        IEnumerable<Blog> blogs = _blogService.GetAll(searchString ?? string.Empty);
        Blog featuredBlog = new Blog();

        if (blogs.Any())
        {
            featuredBlog = DetermineFeaturedBlog(blogs);
        }

        return new HomeIndexViewModel
        {
            FeaturedBlog = featuredBlog,
            Blogs = blogs
        };
    }

    /// <summary>
    /// Determines the featured blog by popularity and returns it. 
    /// </summary>
    /// <param name="blogs"></param>
    /// <returns></returns>
    private Blog DetermineFeaturedBlog(IEnumerable<Blog> blogs)
    {
        var blog = blogs.OrderByDescending(b => b.UpdatedOn)
                        .First();
        
        foreach (Blog b in blogs)
        {
            if (b == blog)
            {
                blog.isFeatured = true;
                _blogService.Update(blog.Id, blog);
            } 
            else
            {
                b.isFeatured = false;
                _blogService.Update(b.Id, b);
            }
        }


        return blog;
    }
}
