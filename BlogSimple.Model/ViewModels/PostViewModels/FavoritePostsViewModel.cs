using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.PostViewModels;

public class FavoritePostsViewModel
{
    public IEnumerable<Post> UsersFavoritePosts { get; set; }
    public User AccountUser { get; set; }
}
