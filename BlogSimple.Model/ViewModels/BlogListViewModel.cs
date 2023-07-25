using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels;

public class BlogListViewModel
{
    public Blog Blog { get; set; }
    public List<Blog> Blogs { get; set; }
}
