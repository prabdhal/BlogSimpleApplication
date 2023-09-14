using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.PostViewModels;

public class DashboardIndexViewModel
{
    public IEnumerable<Post> UserPosts { get; set; }
    public User AccountUser { get; set; }
}
