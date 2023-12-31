﻿using BlogSimple.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.ViewModels.AccountViewModels;

public class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password), Display(Name = "Current password")]
    public string CurrentPassword { get; set; }

    [Required, DataType(DataType.Password), Display(Name = "New Password")]
    public string NewPassword { get; set; }

    [Required, DataType(DataType.Password), Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "Confirm new password does not match")]
    public string ConfirmNewPassword { get; set; }

    public bool PasswordChanged { get; set; } = false;

    public User AccountUser { get; set; }
}
