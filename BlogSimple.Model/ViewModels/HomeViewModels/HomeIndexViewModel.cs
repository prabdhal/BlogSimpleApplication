using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.HomeViewModels;

public class HomeIndexViewModel
{
    public List<string> BlogCategories { get; set; }
    public Blog FeaturedBlog { get; set; } = new Blog();
    public IEnumerable<Blog> PublishedBlogs { get; set; } = Enumerable.Empty<Blog>();
}
