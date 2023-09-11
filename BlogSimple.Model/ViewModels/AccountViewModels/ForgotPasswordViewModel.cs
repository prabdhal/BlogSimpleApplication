using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class ForgotPasswordViewModel
{
    [Required, EmailAddress, Display(Name = "Registered email address")]
    public string Email { get; set; }
    public bool EmailSent { get; set; }

}
