using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.AdminViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.Controllers
{
    [Authorize(Roles = "VerifiedUser,Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IAdminBusinessManager _adminBusinessManager;

        public AdminController(
            RoleManager<UserRole> roleManager,
            IAdminBusinessManager adminBusinessManager
            )
        {
            _roleManager = roleManager;
            _adminBusinessManager = adminBusinessManager;
        }

        public async Task<IActionResult> Index()
        {
            MyAdminViewModel model = await _adminBusinessManager.GetMyAdminViewModel(User);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(MyAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var createdRole = await _adminBusinessManager.CreateRole(model.UserRole.RoleName);
                if (createdRole != null)
                    ViewBag.Message = "Role Created Successfully";
                else
                    ViewBag.Message = "Invalid Name";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteRole(string id)
        {
            _adminBusinessManager.DeleteRole(id);
            ViewBag.Message = "Role Deleted Successfully";

            return RedirectToAction("Index");
        }

    }
}
