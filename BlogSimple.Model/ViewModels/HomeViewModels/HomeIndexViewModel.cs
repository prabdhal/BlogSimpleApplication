using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.HomeViewModels;

public class HomeIndexViewModel
{
    public List<string> PostCategories { get; set; }
    public PostAndCreator FeaturedPost { get; set; } = new PostAndCreator();
    public IEnumerable<PostAndCreator> PublishedPosts { get; set; } = Enumerable.Empty<PostAndCreator>();
}

public class PostAndCreator
{
    public Post Post { get; set; }
    public User Creator { get; set; }
}