using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BlogSimple.Model.Models;

public class User : ApplicationUser
{
    [PersonalData]
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }
    
    [PersonalData]
    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }

    [DataMember]
    [PersonalData]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, ErrorMessage = "Must be between 8 and 30 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "Confirm Password is required")]
    [StringLength(30, ErrorMessage = "Must be between 8 and 30 characters", MinimumLength = 8)]
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
    
    [Display(Name = "Profile Picture")]
    public IFormFile ProfilePicture { get; set; }

    public List<Post> FavoritedPosts { get; set; } = new List<Post>();  
}
