using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public class User : ApplicationUser
{
    [PersonalData]
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }
    [PersonalData]
    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, ErrorMessage = "Must be between 8 and 30 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [StringLength(30, ErrorMessage = "Must be between 8 and 30 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string Content { get; set; }
    [Display(Name = "Portfolio Website")]
    public string PortfolioLink { get; set; }
    [Display(Name = "Twitter Link")]
    public string TwitterLink { get; set; }
    [Display(Name = "GitHub Link")]
    public string GitHubLink { get; set; }
    [Display(Name = "LinkedIn Link")]
    public string LinkedInLink { get; set; }

    public List<Post> FavoritedPosts { get; set; } = new List<Post>();  
}
