using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;

namespace BlogSimple.Web.BusinessManager;

public class HomeBusinessManager : IHomeBusinessManager
{
    private readonly IBlogService _blogService;

    public HomeBusinessManager(IBlogService blogService)
    {
        _blogService = blogService;
    }

    public async Task<HomeIndexViewModel> GetHomeIndexViewModel(string searchString)
    {
        IEnumerable<Blog> publishedBlogs = await _blogService.GetPublishedOnly(searchString ?? string.Empty);
        Blog featuredBlog = new Blog();
        List<string> blogCats = new List<string>();

        if (publishedBlogs.Any())
        {
            featuredBlog = await DetermineFeaturedBlog(publishedBlogs);
        }

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new HomeIndexViewModel
        {
            FeaturedBlog = featuredBlog,
            BlogCategories = blogCats,
            PublishedBlogs = publishedBlogs,
        };
    }

    /// <summary>
    /// Determines the featured blog by popularity and returns it. 
    /// </summary>
    /// <param name="blogs"></param>
    /// <returns></returns>
    private async Task<Blog> DetermineFeaturedBlog(IEnumerable<Blog> blogs)
    {
        var blog = blogs.OrderByDescending(b => b.UpdatedOn)
                        .First();

        foreach (Blog b in blogs)
        {
            if (b == blog)
            {
                blog.IsFeatured = true;
                await _blogService.Update(blog.Id, blog);
            }
            else
            {
                b.IsFeatured = false;
                await _blogService.Update(b.Id, b);
            }
        }

        return blog;
    }
}
