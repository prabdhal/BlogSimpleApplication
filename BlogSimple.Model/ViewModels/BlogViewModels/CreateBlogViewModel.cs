using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Http;

namespace BlogSimple.Model.ViewModels.BlogViewModels;

public class CreateBlogViewModel
{
    public Blog Blog { get; set; }
    public IFormFile HeaderImage { get; set; }
    public User AccountUser { get; set; }
}
