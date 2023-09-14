using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.ViewModels.PostViewModels;

public class EditPostViewModel
{
    public Post Post { get; set; }
    [Display(Name = "Header Image")]
    public IFormFile HeaderImage { get; set; }
    public User AccountUser { get; set; }
}
