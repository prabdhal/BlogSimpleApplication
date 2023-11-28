using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class MyAchievementsViewModel
{
    public User AccountUser { get; set; }
    public Achievements Achievements { get; set; }
    public int PublishedPostsCount { get; set; }
    public int PublishedCommentsCount { get; set; }
    public int PublishedRepliesCount { get; set; }
    public int TotalPublishedWordsCount { get; set; }
    public int TotalCommentsLiked { get; set; }
    public int SavedPostsCount { get; set; }
}
