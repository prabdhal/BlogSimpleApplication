using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.HomeViewModels;

public class HomeIndexViewModel
{
    public List<string> PostCategories { get; set; }
    public Post FeaturedPost { get; set; } = new Post();
    public IEnumerable<Post> PublishedPosts { get; set; } = Enumerable.Empty<Post>();
}
