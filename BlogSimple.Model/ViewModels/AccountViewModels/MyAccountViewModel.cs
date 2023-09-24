using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class MyAccountViewModel
{
    public User AccountUser { get; set; }
    public ChangePassword ChangePassword { get; set; } = new ChangePassword();
    public int PublishedPostsCount { get; set; }
    public int TotalCommentsAndRepliesCount { get; set; }
    public int FavoritePostsCount { get; set; }
    public EmailConfirmViewModel EmailConfirmViewModel { get; set; }
}
