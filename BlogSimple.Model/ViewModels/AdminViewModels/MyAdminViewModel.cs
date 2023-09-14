using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.AdminViewModels;

public class MyAdminViewModel
{
    public User AccountUser { get; set; }
    public List<UserRole> UserRoles { get; set; }
    public UserRole UserRole { get; set; } = new UserRole();
    public List<User> RegisteredUsers { get; set; }
    public Dictionary<Guid, string> RoleNameMapping { get; set; } = new Dictionary<Guid, string>
    {
        { new Guid("34ad0d02-7521-49ff-8a55-bb75e50224bb"), "Admin"},
        { new Guid("ae5c1c60-4940-4623-ba3d-9468f58b38a6"), "Verified User"},
        { new Guid("4c2652f9-62a7-44b9-8f5e-975eb92cd71b"), "Unverified User"},
    };
}
