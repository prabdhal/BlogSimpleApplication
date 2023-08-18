using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Http;

namespace BlogSimple.Model.ViewModels.BlogViewModels;

public class EditBlogViewModel
{
    public Blog Blog { get; set; }
    public IFormFile HeaderImage { get; set; }
}
