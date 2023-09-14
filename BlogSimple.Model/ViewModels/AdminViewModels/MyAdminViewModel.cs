using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.AdminViewModels;

public class MyAdminViewModel
{
    public User AccountUser { get; set; }
    public List<UserRole> UserRoles { get; set; }
    public UserRole UserRole { get; set; } = new UserRole();
}
