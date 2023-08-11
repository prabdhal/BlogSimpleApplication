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

    public BlogDetailsViewModel GetHomeDetailsViewModel(string id)
    {
        Blog blog = _blogService.Get(id);
        List<string> blogCats = new List<string>();
        var blogs = _blogService.GetPublishedOnly("");
        var comments = _commentService.GetAllByBlog(id);
        var replies = _commentReplyService.GetAllByBlog(id);

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

    public HomeIndexViewModel GetHomeIndexViewModel(string searchString)
    {
        IEnumerable<Blog> publishedBlogs = _blogService.GetPublishedOnly(searchString ?? string.Empty);
        Blog featuredBlog = new Blog();
        List<string> blogCats = new List<string>();

        if (publishedBlogs.Any())
        {
            featuredBlog = DetermineFeaturedBlog(publishedBlogs);
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
