using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;

namespace BlogSimple.Web.BusinessManager;

public class HomeBusinessManager : IHomeBusinessManager
{
    private readonly IBlogService _blogService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _commentReplyService;

    public HomeBusinessManager(
        IBlogService blogService,
        ICommentService commentService,
        ICommentReplyService commentReplyService
        )
    {
        _blogService = blogService;
        _commentService = commentService;
        _commentReplyService = commentReplyService;
    }

    public async Task<BlogDetailsViewModel> GetHomeDetailsViewModel(string id)
    {
        Blog blog = await _blogService.Get(id);
        List<string> blogCats = new List<string>();
        var blogs = await _blogService.GetPublishedOnly("");
        var comments = await _commentService.GetAllByBlog(id);
        var replies = await _commentReplyService.GetAllByBlog(id);

        foreach (var cat in Enum.GetValues(typeof(BlogCategory)))
        {
            blogCats.Add(cat.ToString());
        }

        return new BlogDetailsViewModel
        {
            AllBlogs = blogs,
            BlogCategories = blogCats,
            Blog = blog,
            Comments = comments,
            CommentReplies = replies
        };
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
                blog.isFeatured = true;
                await _blogService.Update(blog.Id, blog);
            }
            else
            {
                b.isFeatured = false;
                await _blogService.Update(b.Id, b);
            }
        }

        return blog;
    }
}
