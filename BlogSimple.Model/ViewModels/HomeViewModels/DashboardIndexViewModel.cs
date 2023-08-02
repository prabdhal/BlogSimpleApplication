using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.HomeViewModels;

public class DashboardIndexViewModel
{
    public Blog Blog { get; set; }
    public IEnumerable<Blog> Blogs { get; set; }
}
