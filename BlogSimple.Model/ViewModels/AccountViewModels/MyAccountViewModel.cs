using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class MyAccountViewModel
{
    public User AccountUser { get; set; }
    public int PublishedBlogsCount { get; set; }
    public int TotalCommentsAndRepliesCount { get; set; }
    public int FavoriteBlogsCount { get; set; }
}
