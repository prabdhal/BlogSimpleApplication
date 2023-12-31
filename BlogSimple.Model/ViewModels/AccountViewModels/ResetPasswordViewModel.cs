﻿using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class ResetPasswordViewModel
{
    [Required]
    public string UserId { get; set; }

    [Required]
    public string Token { get; set; }

    [Required, DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string NewPassword { get; set; }

    [Required, DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; }

    public bool IsSuccessful { get; set; }
}
