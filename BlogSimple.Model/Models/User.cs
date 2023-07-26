using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public class User
{
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }
    [Required(ErrorMessage = "Username is required")]
    [StringLength(16, ErrorMessage = "Must be between 4 and 16 characters", MinimumLength = 4)]
    public string Username { get; set; }
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, ErrorMessage = "Must be between 8 and 30 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [StringLength(30, ErrorMessage = "Must be between 8 and 30 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}
