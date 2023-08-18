using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.BlogViewModels;

public class DashboardIndexViewModel
{
    public Blog Blog { get; set; }
    public IEnumerable<Blog> UserBlogs { get; set; }
}
