using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Http;

namespace BlogSimple.Model.ViewModels.PostViewModels;

public class CreatePostViewModel
{
    public Post Post { get; set; }
    public IFormFile HeaderImage { get; set; }
    public User AccountUser { get; set; }
}
