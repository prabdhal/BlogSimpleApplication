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
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountBusinessManager _accountBusinessManager;
        private readonly IPostBusinessManager _postBusinessManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAccountBusinessManager accountBusinessManager,
            IPostBusinessManager postBusinessManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountBusinessManager = accountBusinessManager;
            _postBusinessManager = postBusinessManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                EmailConfirmViewModel model = new EmailConfirmViewModel
                {
                    EmailSent = true,
                    EmailVerified = false,
                };
                var emailConfirmModel = await _accountBusinessManager.GetEmailConfirmViewModel(User, model);
                return RedirectToAction("MyAccount", emailConfirmModel);
            }
            return View();
        }

        [AllowAnonymous]
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

                return RedirectToAction("Login");
            }

            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                var myAccountViewModel = await _accountBusinessManager.GetMyAccountViewModel(User);
                return RedirectToAction("MyProfile");
            }
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
                            EmailConfirmViewModel model = new EmailConfirmViewModel
                            {
                                Email = user.Email,
                                EmailSent = true,
                                EmailVerified = user.EmailConfirmed
                            };
                            return RedirectToAction("EmailVerification", model);
                        }
                        return Redirect(returnurl ?? "/Post/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid credentials");
                    }
                }
            }

            return View();
        }

        [Authorize(Roles = "UnverifiedUser,VerifiedUser,Admin")]
        public async Task<ActionResult> MyProfile()
        {
            var myProfileViewModel = await _accountBusinessManager.GetMyProfileViewModel(User);
            return View(myProfileViewModel);
        }

        [Authorize(Roles = "UnverifiedUser,VerifiedUser,Admin")]
        public async Task<ActionResult> EmailVerification(EmailConfirmViewModel model)
        {
            var emailConfirmModel = await _accountBusinessManager.GetEmailConfirmViewModel(User, model);
            return View(emailConfirmModel);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmViewModel emailConfirmViewModel = new EmailConfirmViewModel
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                var result = await _accountBusinessManager.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    emailConfirmViewModel.EmailVerified = true;
                }
            }

            return View(emailConfirmViewModel);
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
            return View("EmailVerification", model);
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
        public IActionResult ChangePassword()
        {
            var model = _accountBusinessManager.GetChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "UnverifiedUser,VerifiedUser,Admin")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountBusinessManager.ChangePasswordAsync(model, User);
                if (result.Succeeded)
                {
                    model.PasswordChanged = true;
                    ModelState.Clear();
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "UnverifiedUser,VerifiedUser,Admin")]
        public async Task<IActionResult> DeleteAccount()
        {
            var myAccountViewModel = await _accountBusinessManager.GetMyAccountViewModel(User);
            return View(myAccountViewModel);
        }

        [Authorize(Roles = "UnverifiedUser,VerifiedUser,Admin")]
        public IActionResult DeleteUser()
        {
            _postBusinessManager.DeleteUser(User);

            return RedirectToAction("Logout");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "UnverifiedUser, VerifiedUser, Admin")]
        public async Task<IActionResult> UpdateUserProfilePicture(MyAccountViewModel model)
        {
            await _accountBusinessManager.UpdateUserProfile(model, User);

            return RedirectToAction("MyProfile");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}