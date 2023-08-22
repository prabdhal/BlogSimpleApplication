using BlogSimple.Model.Models;
using Microsoft.AspNetCore.Http;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class AboutMeViewModel
{
    public User User { get; set; }
    public IFormFile HeaderImage { get; set; }
}
