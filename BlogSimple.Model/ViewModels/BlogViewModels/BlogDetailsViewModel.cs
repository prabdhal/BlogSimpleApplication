using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.BlogViewModels;

public class BlogDetailsViewModel
{
    public List<string> BlogCategories { get; set; }
    public Blog Blog { get; set; }
    public IEnumerable<Blog> AllBlogs { get; set; } = Enumerable.Empty<Blog>();
}
