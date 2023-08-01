using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.HomeViewModels;

public class HomeIndexViewModel
{
    public string SearchString { get; set; }
    public List<string> BlogCategories { get; set; }
    public Blog FeaturedBlog { get; set; } = new Blog();
    public IEnumerable<Blog> Blogs { get; set; } = Enumerable.Empty<Blog>();
    public IEnumerable<Blog> AllBlogs { get; set; } = Enumerable.Empty<Blog>();
}
