using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Http;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class AuthorViewModel
{
    public List<string> PostCategories { get; set; }
    public User AccountUser { get; set; }
    public IFormFile HeaderImage { get; set; }
}
