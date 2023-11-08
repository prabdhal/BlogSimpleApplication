using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;

namespace BlogSimple.Web.BusinessManager;

public class HomeBusinessManager : IHomeBusinessManager
{
    private readonly IPostService _blogService;
    private readonly IUserService _userService;

    public HomeBusinessManager(
        IPostService blogService,
        IUserService userService
        )
    {
        _blogService = blogService;
        _userService = userService;
    }

    public async Task<HomeIndexViewModel> GetHomeIndexViewModel(string searchString)
    {
        IEnumerable<Post> publishedBlogs = await _blogService.GetPublishedOnly(searchString ?? string.Empty);
        Post featuredBlog = new Post();
        List<string> blogCats = new List<string>();

        if (publishedBlogs.Any())
        {
            featuredBlog = await DetermineFeaturedBlog(publishedBlogs);
            featuredBlog.CreatedBy = await _userService.Get(featuredBlog.CreatedById);
        }

        foreach (Post post in publishedBlogs)
        {
            post.CreatedBy = await _userService.Get(post.CreatedById);
        }

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new HomeIndexViewModel
        {
            FeaturedPost = featuredBlog,
            PostCategories = blogCats,
            PublishedPosts = publishedBlogs,
        };
    }

    /// <summary>
    /// Determines the featured blog by popularity and returns it. 
    /// </summary>
    /// <param name="blogs"></param>
    /// <returns></returns>
    private async Task<Post> DetermineFeaturedBlog(IEnumerable<Post> blogs)
    {
        var blog = blogs.OrderByDescending(b => b.UpdatedOn)
                        .First();

        foreach (Post b in blogs)
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
