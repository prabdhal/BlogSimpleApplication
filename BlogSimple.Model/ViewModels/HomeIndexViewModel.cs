using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels;

public class HomeIndexViewModel
{
    public IEnumerable<Blog> Blogs { get; set; }
}
