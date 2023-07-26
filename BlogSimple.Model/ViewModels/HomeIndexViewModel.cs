using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels;

public class HomeIndexViewModel
{
    public Blog FeaturedBlog { get; set; } = new Blog();
    public IEnumerable<Blog> Blogs { get; set; } = Enumerable.Empty<Blog>();
}
