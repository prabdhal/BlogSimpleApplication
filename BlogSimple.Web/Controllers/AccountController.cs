using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.AccountViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;
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

        public AccountController(
            UserManager<User> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<User> signInManager,
            IAccountBusinessManager accountBusinessManager,
            IEmailService emailService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountBusinessManager = accountBusinessManager;
        }

        public async Task<IActionResult> Author(string id)
        {
            AuthorViewModel authorViewModel = await _accountBusinessManager.GetAuthorViewModel(id);

            return View(authorViewModel);
        }

        [Authorize]
        public async Task<ActionResult> UpdateAuthor()
        {
            var authorViewModel = await _accountBusinessManager.GetAuthorViewModelForSignedInUser(User);

            if (authorViewModel is null)
                return new NotFoundResult();

            return View(authorViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAuthor(AuthorViewModel authorViewModel)
        {
            authorViewModel = await _accountBusinessManager.EditUser(authorViewModel, User);
            return RedirectToAction("Author", new { authorViewModel.AccountUser.Id });
        }

        [Authorize]
        public async Task<ActionResult> MyAccount()
        {
            var user = await _userManager.GetUserAsync(User);

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
                    if (result.Succeeded)
                    {
                        return Redirect(returnurl ?? "/Blog/Index");
                    }
                    if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "Please verify your email address");
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
            EmailConfirm model = new EmailConfirm
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
        public async Task<IActionResult> ConfirmEmail(EmailConfirm model)
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
    }
}