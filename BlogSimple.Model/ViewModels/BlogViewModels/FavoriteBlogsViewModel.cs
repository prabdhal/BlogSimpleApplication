using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.BlogViewModels;

public class FavoriteBlogsViewModel
{
    public IEnumerable<Blog> UsersFavoriteBlogs { get; set; }
    public User AccountUser { get; set; }
}
