using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.AccountViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager;

public class AccountBusinessManager : IAccountBusinessManager
{
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _replyService;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    private readonly string AdminUserRoleName = "Admin";
    private readonly string VerifiedUserRoleName = "VerifiedUser";
    private readonly string UnverifiedUserRoleName = "UnverifiedUser";

    public AccountBusinessManager(
        UserManager<User> userManager,
        IUserService userService,
        IPostService postService,
        ICommentService commentService,
        ICommentReplyService replyService,
        IEmailService emailService,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment
        )
    {
        _userManager = userManager;
        _userService = userService;
        _postService = postService;
        _commentService = commentService;
        _replyService = replyService;
        _emailService = emailService;
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user;
    }

    public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);

        return await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
    }

    public async Task<IdentityResult> CreateUserAsync(User user)
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
            if (user.ProfilePicture != null)
            {
                // stores image file name 
                string webRootPath = _webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Users\{newUser.Id}\ProfilePicture\ProfilePictureImage.jpg";

                EnsureFolder(pathToImage);

                IFormFile headerImg = user.ProfilePicture;

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await headerImg.CopyToAsync(fileStream);
                }
            }
            else
            {

                FormFile profileImg;
                // stores image file name 
                string webRootPath = _webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Users\{newUser.Id}\ProfilePicture\ProfilePictureImage.jpg";
                string pathToDefaultImage = $@"{webRootPath}\UserFiles\DefaultImages\DefaultProfilePictureImage.jpg";

                EnsureFolder(pathToImage);

                var stream = File.OpenRead(pathToDefaultImage);
                profileImg = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpg"
                };


                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await profileImg.CopyToAsync(fileStream);
                }
            }

            // assigns user to unverified user role 
            await ApplyUnverifiedUserRole(newUser);
            await GenerateEmailConfirmationTokenAsync(newUser);

        }
        return result;
    }

    public async Task GenerateEmailConfirmationTokenAsync(User user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        if (!string.IsNullOrEmpty(token))
        {
            await SendEmailConfirmationEmail(user, token);
        }
    }

    public async Task GenerateForgotPasswordConfirmationTokenAsync(User user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        if (!string.IsNullOrEmpty(token))
        {
            await SendForgotPasswordEmail(user, token);
        }
    }

    public async Task<MyAccountViewModel> GetMyAccountViewModel(ClaimsPrincipal claimsPrincipal, EmailConfirmViewModel emailConfirmViewModel)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var publishedPost = await _postService.GetAll(user);
        var publishedPostCount = publishedPost.Count();

        var comments = await _commentService.GetAll(user);
        var replies = await _replyService.GetAllByUser(user);
        var totalCommentsAndRepliesCount = comments.Count() + replies.Count();
        var favoritedPostsCount = user.FavoritedPosts.Count();

        if (user.EmailConfirmed)
        {
            // assigns user to verified user role 
            await ApplyVerifyUserRole(user);
        }

        return new MyAccountViewModel
        {
            AccountUser = user,
            PublishedPostsCount = publishedPostCount,
            TotalCommentsAndRepliesCount = totalCommentsAndRepliesCount,
            FavoritePostsCount = favoritedPostsCount,
            EmailConfirmViewModel = emailConfirmViewModel
        };
    }

    public async Task<MyAccountViewModel> GetMyAccountViewModel(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var publishedPost = await _postService.GetAll(user);
        var publishedPostCount = publishedPost.Count();

        var comments = await _commentService.GetAll(user);
        var replies = await _replyService.GetAllByUser(user);
        var totalCommentsAndRepliesCount = comments.Count() + replies.Count();
        var favoritedPostsCount = user.FavoritedPosts.Count();

        if (user.EmailConfirmed)
        {
            await ApplyVerifyUserRole(user);
        }
        // stores image file name 
        string webRootPath = _webHostEnvironment.WebRootPath;
        string pathToImage = $@"{webRootPath}\UserFiles\Users\{user.Id}\ProfilePicture\ProfilePictureImage.jpg";

        EnsureFolder(pathToImage);

        if (user.ProfilePicture != null)
        {
            IFormFile headerImg = user.ProfilePicture;

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await headerImg.CopyToAsync(fileStream);
            }
        }
        else
        {
            FormFile profileImg;
            string pathToDefaultImage = $@"{webRootPath}\UserFiles\DefaultImages\DefaultProfilePictureImage.jpg";

            var stream = File.OpenRead(pathToDefaultImage);
            profileImg = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };


            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await profileImg.CopyToAsync(fileStream);
            }
        }

        return new MyAccountViewModel
        {
            AccountUser = user,
            PublishedPostsCount = publishedPostCount,
            TotalCommentsAndRepliesCount = totalCommentsAndRepliesCount,
            FavoritePostsCount = favoritedPostsCount,
            EmailConfirmViewModel = new EmailConfirmViewModel()
        };
    }

    public async Task<MyAccountViewModel> UpdateUserProfile(MyAccountViewModel myAccountViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        user.ProfilePicture = myAccountViewModel.AccountUser.ProfilePicture;
        
        string webRootPath = _webHostEnvironment.WebRootPath;
        string pathToImage = $@"{webRootPath}\UserFiles\Users\{user.Id}\ProfilePicture\ProfilePictureImage.jpg";
        EnsureFolder(pathToImage);

        if (myAccountViewModel.AccountUser.ProfilePicture != null)
        {
            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await myAccountViewModel.AccountUser.ProfilePicture.CopyToAsync(fileStream);
            }
        }
        else
        {
            FormFile profileImg;
            string pathToDefaultImage = $@"{webRootPath}\UserFiles\DefaultImages\DefaultProfilePictureImage.jpg";

            var stream = File.OpenRead(pathToDefaultImage);
            profileImg = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await profileImg.CopyToAsync(fileStream);
            }
        }

        return new MyAccountViewModel
        {
            AccountUser = user,
            EmailConfirmViewModel = new EmailConfirmViewModel()
        };
    }

    public async Task<AuthorViewModel> GetAuthorViewModel(string userId)
    {
        var user = await _userService.Get(userId);

        return new AuthorViewModel
        {
            AccountUser = user,
        };
    }

    public async Task<AuthorViewModel> GetAuthorViewModelForSignedInUser(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        return new AuthorViewModel
        {
            AccountUser = user,
        };
    }

    public async Task<AuthorViewModel> EditUser(AuthorViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        user.Content = aboutViewModel.AccountUser.Content;
        user.PortfolioLink = aboutViewModel.AccountUser.PortfolioLink;
        user.TwitterLink = aboutViewModel.AccountUser.TwitterLink;
        user.GitHubLink = aboutViewModel.AccountUser.GitHubLink;
        user.LinkedInLink = aboutViewModel.AccountUser.LinkedInLink;

        if (aboutViewModel.HeaderImage != null)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Users\{user.Id}\CoverImage\UserCoverImage.jpg";

            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await aboutViewModel.HeaderImage.CopyToAsync(fileStream);
            }
        }

        return new AuthorViewModel
        {
            AccountUser = await _userService.Update(user.UserName, user)
        };
    }

    private void EnsureFolder(string path)
    {
        string directoryName = Path.GetDirectoryName(path);
        if (directoryName.Length > 0)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }

    public async Task SendEmailConfirmationEmail(User user, string token)
    {
        string appDomain = _configuration.GetSection("Application:AppDomain").Value;
        string configurationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

        UserEmailOptions options = new UserEmailOptions
        {
            ToEmails = new List<string>()
            {
                user.Email
            },
            PlaceHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{FirstName}}", user.FirstName),
                new KeyValuePair<string, string>("{{LastName}}", user.LastName),
                new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + configurationLink, user.Id, token)),
            }
        };

        await _emailService.SendEmailForEmailConfirmation(options);
    }

    public async Task SendForgotPasswordEmail(User user, string token)
    {
        string appDomain = _configuration.GetSection("Application:AppDomain").Value;
        string configurationLink = _configuration.GetSection("Application:ForgotPassword").Value;

        UserEmailOptions options = new UserEmailOptions
        {
            ToEmails = new List<string>()
            {
                user.Email
            },
            PlaceHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{FirstName}}", user.FirstName),
                new KeyValuePair<string, string>("{{LastName}}", user.LastName),
                new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + configurationLink, user.Id, token)),
            }
        };

        await _emailService.SendEmailForForgotPassword(options);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
    {
        var user = await _userManager.FindByIdAsync(uid);
        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            await ApplyVerifyUserRole(user);
        }
        return result;
    }

    public async Task<IdentityResult> ChangePasswordAsync(ChangePassword model, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        
        return result;
    }

    /// <summary>
    /// Applies "UnverifiedUser" role to the user. 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task ApplyUnverifiedUserRole(User user)
    {
        await _userManager.AddToRoleAsync(user, UnverifiedUserRoleName);
    }

    /// <summary>
    /// Applies "VerifiedUser" role to the user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task ApplyVerifyUserRole(User user)
    {
        await _userManager.RemoveFromRoleAsync(user, UnverifiedUserRoleName);
        await _userManager.AddToRoleAsync(user, VerifiedUserRoleName);
    }
}
