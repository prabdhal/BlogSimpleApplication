using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class MyProfileViewModel
{
    public User AccountUser { get; set; }
    public int PublishedPostsCount { get; set; }
    public int TotalCommentsAndRepliesCount { get; set; }
    public int TotalCommentsLiked { get; set; }
    public int SavedPostsCount { get; set; }
    public int TotalWordsCount { get; set; }
}
