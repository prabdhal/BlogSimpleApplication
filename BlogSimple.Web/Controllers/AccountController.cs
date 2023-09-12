using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.AccountViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountBusinessManager _accountBusinessManager;

        public AccountController(
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            SignInManager<User> signInManager,
            IAccountBusinessManager accountBusinessManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _accountBusinessManager = accountBusinessManager;
        }

        [Authorize(Roles = "VerifiedUser,Admin")]
        public async Task<IActionResult> Author(string id)
        {
            AuthorViewModel authorViewModel = await _accountBusinessManager.GetAuthorViewModel(id);

            return View(authorViewModel);
        }

        [Authorize(Roles = "VerifiedUser,Admin")]
        public async Task<ActionResult> UpdateAuthor()
        {
            var authorViewModel = await _accountBusinessManager.GetAuthorViewModelForSignedInUser(User);

            if (authorViewModel is null)
                return new NotFoundResult();

            return View(authorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "VerifiedUser,Admin")]
        public async Task<IActionResult> UpdateAuthor(AuthorViewModel authorViewModel)
        {
            authorViewModel = await _accountBusinessManager.EditUser(authorViewModel, User);
            return RedirectToAction("Author", new { authorViewModel.AccountUser.Id });
        }

        [Authorize(Roles = "UnverifiedUser,VerifiedUser,Admin")]
        public async Task<ActionResult> MyAccount()
        {
            var myAccountViewModel = await _accountBusinessManager.GetMyAccountViewModel(User);
            return View(myAccountViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountBusinessManager.CreateUserAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }

                    return View(user);
                }

                ModelState.Clear();
                return RedirectToAction("ConfirmEmail", new { email = user.Email });
            }

            return View(user);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Required] string username, [Required] string password, string returnurl)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                    if (result.Succeeded || user.EmailConfirmed == false)
                    {
                        if (user.EmailConfirmed == false)
                        {
                            return RedirectToAction("MyAccount");
                        }
                        return Redirect(returnurl ?? "/Blog/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid credentials");
                    }
                }
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmViewModel model = new EmailConfirmViewModel
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _accountBusinessManager.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }


        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmViewModel model)
        {
            User user = await _accountBusinessManager.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }

                await _accountBusinessManager.GenerateEmailConfirmationTokenAsync(user);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong");
            }
            return View(model);
        }

        [AllowAnonymous, HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous, HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountBusinessManager.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    await _accountBusinessManager.GenerateForgotPasswordConfirmationTokenAsync(user);
                }

                ModelState.Clear();
                model.EmailSent = true;
            }
            return View(model);
        }

        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordViewModel);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await _accountBusinessManager.ResetPasswordAsync(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccessful = true;
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(UserRole role)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new UserRole()
                {
                    Name = role.RoleName
                });
                if (result.Succeeded)
                    ViewBag.Message = "Role Created Successfully";
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
    }
}