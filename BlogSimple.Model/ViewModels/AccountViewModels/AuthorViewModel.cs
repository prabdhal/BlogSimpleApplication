using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Http;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class AuthorViewModel
{
    public List<string> PostCategories { get; set; }
    public User AccountUser { get; set; }
    public IFormFile CoverImage { get; set; }
    public IEnumerable<User> Authors { get; set; }
    public List<Post> Posts { get; set; }
    public List<Post> AuthorsPublishedPosts { get; set; }
}
