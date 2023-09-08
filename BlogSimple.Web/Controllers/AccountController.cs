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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
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
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailService = emailService;
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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        public IActionResult RegisterRole()
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
                ModelState.AddModelError(nameof(username), "Login Failed: Invalid Email or Password");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Content = null,
                    PortfolioLink = null,
                    TwitterLink = null,
                    GitHubLink = null,
                    LinkedInLink = null
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    if (!string.IsNullOrEmpty(token))
                    {
                        await _accountBusinessManager.SendEmailConfirmationEmail(newUser, token);
                    }
                    ViewBag.Message = "User created successfully!";
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterRole(UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole newRole = new ApplicationRole()
                {
                    Name = userRole.RoleName,
                };

                IdentityResult result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                    ViewBag.Message = "Role Created Successfully!";
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
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
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                var result = await _accountBusinessManager.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                }
            }

            return View();
        }
    }
}