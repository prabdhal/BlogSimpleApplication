using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.ViewModels.BlogViewModels;

public class EditBlogViewModel
{
    public Blog Blog { get; set; }
    [Display(Name = "Header Image")]
    public IFormFile HeaderImage { get; set; }
    public User AccountUser { get; set; }
}
