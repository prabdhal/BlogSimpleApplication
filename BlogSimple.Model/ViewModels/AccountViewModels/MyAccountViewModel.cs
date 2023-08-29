using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class MyAccountViewModel
{
    public User AccountUser { get; set; }
    public int PublishedBlogsCount { get; set; }
}
