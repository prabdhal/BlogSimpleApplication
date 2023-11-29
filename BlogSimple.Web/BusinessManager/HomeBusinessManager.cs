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
        IEnumerable<Post> publishedPosts = await _blogService.GetPublishedOnly(searchString ?? string.Empty);
        List<PostAndCreator> publishedPostsAndCreator = new List<PostAndCreator>();
        PostAndCreator featuredBlogAndCreator = new PostAndCreator();
        List<string> blogCats = new List<string>();

        if (publishedPosts.Any())
        {
            featuredBlogAndCreator.Post = await DetermineFeaturedBlog(publishedPosts);
            featuredBlogAndCreator.Creator = await _userService.Get(featuredBlogAndCreator.Post.CreatedById);
        }

        foreach (Post post in publishedPosts)
        {
            PostAndCreator postAndCreatorModel = new PostAndCreator
            {
                Post = post,
                Creator = await _userService.Get(post.CreatedById),
            };
            publishedPostsAndCreator.Add(postAndCreatorModel);
        }

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new HomeIndexViewModel
        {
            FeaturedPost = featuredBlogAndCreator,
            PostCategories = blogCats,
            PublishedPosts = publishedPostsAndCreator,
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
