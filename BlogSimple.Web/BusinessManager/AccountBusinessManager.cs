using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.AccountViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using SkiaSharp;
using System.Security.Claims;

namespace BlogSimple.Web.BusinessManager;

public class AccountBusinessManager : IAccountBusinessManager
{
    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;
    private readonly IAchievementsService _achievementService;
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _replyService;
    private readonly ISendGridEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IAchievementsBusinessManager _achievementsBusinessManager;

    private readonly string AdminUserRoleName = "Admin";
    private readonly string VerifiedUserRoleName = "VerifiedUser";
    private readonly string UnverifiedUserRoleName = "UnverifiedUser";
    //private readonly int StandardImageWidth = 800;
    //private readonly int StandardImageHeight = 450;
    //private readonly int StandardProfileImageWidth = 150;
    //private readonly int StandardProfileImageHeight = 150;


    public AccountBusinessManager(
        UserManager<User> userManager,
        IUserService userService,
        IAchievementsService achievementService,
        IPostService postService,
        ICommentService commentService,
        ICommentReplyService replyService,
        ISendGridEmailService emailService,
        IConfiguration configuration,
        IAchievementsBusinessManager achievementsBusinessManager
        )
    {
        _userManager = userManager;
        _userService = userService;
        _achievementService = achievementService;
        _postService = postService;
        _commentService = commentService;
        _replyService = replyService;
        _emailService = emailService;
        _configuration = configuration;
        _achievementsBusinessManager = achievementsBusinessManager;
    }

    #region Achievement Event Handlers
    #endregion

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
            Bio = null,
            PortfolioLink = null,
            TwitterLink = null,
            GitHubLink = null,
            LinkedInLink = null
        };

        // Initialize Achievements
        Achievements achievements = await _achievementsBusinessManager.CreateAchievement();
        newUser.AchievementId = achievements.Id;

        IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

        if (result.Succeeded)
        {
            if (user.ProfilePictureInput == null)
            {
            }
            else if (user.ProfilePictureInput.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    user.ProfilePictureInput.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    byte[] resizedImage = fileBytes; //ResizeImage(fileBytes, StandardProfileImageWidth, StandardProfileImageHeight);
                    newUser.ProfilePicture = resizedImage;
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

    public async Task<MyAccountViewModel> GetMyAccountViewModel(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        return new MyAccountViewModel
        {
            AccountUser = user
        };
    }

    public async Task<MyProfileViewModel> GetMyProfileViewModel(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        user.ProfilePicture = GetImage(Convert.ToBase64String(user.ProfilePicture));
        var publishedPosts = await _postService.GetAll(user);
        var publishedPostsCount = publishedPosts.Count();
        int totalWordsCount = 0;

        var comments = await _commentService.GetAll(user);
        var replies = await _replyService.GetAllByUser(user);
        var totalCommentsAndRepliesCount = comments.Count() + replies.Count();
        var favoritedPostsCount = user.FavoritedPosts.Count();

        foreach (Post post in publishedPosts)
        {
            totalWordsCount += post.WordCount;
        }

        return new MyProfileViewModel
        {
            AccountUser = user,
            PublishedPostsCount = publishedPostsCount,
            TotalCommentsAndRepliesCount = totalCommentsAndRepliesCount,
            FavoritePostsCount = favoritedPostsCount,
            TotalWordsCount = totalWordsCount
        };
    }

    public async Task<MyAchievementsViewModel> GetMyAchievementsViewModel(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        Achievements achievements = await _achievementService.Get(user.AchievementId);
        var publishedPosts = await _postService.GetAll(user);
        var publishedPostsCount = publishedPosts.Count();
        int totalWordsCount = 0;

        var comments = await _commentService.GetAll(user);
        var replies = await _replyService.GetAllByUser(user);
        var totalCommentsCount = comments.Count();
        var totalRepliesCount = replies.Count();
        var favoritedPostsCount = user.FavoritedPosts.Count();

        foreach (Post post in publishedPosts)
        {
            totalWordsCount += post.WordCount;
        }

        return new MyAchievementsViewModel
        {
            AccountUser = user,
            Achievements = achievements,
        };
    }

    private byte[] GetImage(string sBase64String)
    {
        byte[] bytes = null;
        if (!string.IsNullOrEmpty(sBase64String))
        {
            bytes = Convert.FromBase64String(sBase64String);
        }
        Console.WriteLine(bytes);
        return bytes;
    }

    public ChangePasswordViewModel GetChangePasswordViewModel()
    {
        ChangePasswordViewModel model = new ChangePasswordViewModel();

        return model;
    }

    public async Task<EmailConfirmViewModel> GetEmailConfirmViewModel(ClaimsPrincipal claimsPrincipal, EmailConfirmViewModel model)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        return new EmailConfirmViewModel
        {
            Email = user.Email,
            EmailSent = model.EmailSent,
            EmailVerified = user.EmailConfirmed,
        };
    }

    public async Task<MyAccountViewModel> UpdateUserProfile(MyAccountViewModel myAccountViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        if (myAccountViewModel.AccountUser.ProfilePictureInput == null)
        {
        }
        else if (myAccountViewModel.AccountUser.ProfilePictureInput.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                await myAccountViewModel.AccountUser.ProfilePictureInput.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                byte[] resizedImage = fileBytes; // ResizeImage(fileBytes, StandardProfileImageWidth, StandardProfileImageHeight);
                user.ProfilePicture = resizedImage;
            }
        }

        return new MyAccountViewModel
        {
            AccountUser = await _userService.Update(user.UserName, user)
        };
    }

    private static byte[] ResizeImage(byte[] fileContents, int setWidth, int setHeight,
    SKFilterQuality quality = SKFilterQuality.Medium)
    {
        using MemoryStream ms = new MemoryStream(fileContents);
        using SKBitmap sourceBitmap = SKBitmap.Decode(ms);

        //int height = Math.Min(maxHeight, sourceBitmap.Height);
        //int width = Math.Min(maxWidth, sourceBitmap.Width);

        using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(setWidth, setHeight), quality);
        using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);
        using SKData data = scaledImage.Encode();

        return data.ToArray();
    }

    public async Task<AuthorViewModel> GetAuthorViewModel(string userId)
    {
        var user = await _userService.Get(userId);
        user.HeaderImage = GetImage(Convert.ToBase64String(user.HeaderImage));
        var posts = await _postService.GetAll();
        var authorsPosts = await _postService.GetAllPublishedByUser(user);

        List<string> postCats = new List<string>();
        IEnumerable<User> users = await _userService.GetAll();

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            postCats.Add(cat.ToString());
        }

        return new AuthorViewModel
        {
            PostCategories = postCats,
            AccountUser = user,
            Authors = users,
            Posts = posts,
            AuthorsPublishedPosts = authorsPosts
        };
    }

    public async Task<AuthorViewModel> GetAuthorViewModelForSignedInUser(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var posts = await _postService.GetAll();
        var authorsPosts = await _postService.GetAllPublishedByUser(user);

        List<string> postCats = new List<string>();
        IEnumerable<User> users = await _userService.GetAll();

        foreach (var cat in Enum.GetValues(typeof(PostCategory)))
        {
            postCats.Add(cat.ToString());
        }

        return new AuthorViewModel
        {
            PostCategories = postCats,
            AccountUser = user,
            Authors = users,
            Posts = posts,
            AuthorsPublishedPosts = authorsPosts
        };
    }

    public async Task<AuthorViewModel> EditUser(AuthorViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);

        user.Bio = aboutViewModel.AccountUser.Bio;
        user.Heading = aboutViewModel.AccountUser.Heading;
        user.PortfolioLink = aboutViewModel.AccountUser.PortfolioLink;
        user.TwitterLink = aboutViewModel.AccountUser.TwitterLink;
        user.GitHubLink = aboutViewModel.AccountUser.GitHubLink;
        user.LinkedInLink = aboutViewModel.AccountUser.LinkedInLink;


        if (aboutViewModel.AccountUser.HeaderImageInput == null)
        {
        }
        else if (aboutViewModel.AccountUser.HeaderImageInput.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                aboutViewModel.AccountUser.HeaderImageInput.CopyTo(ms);
                var fileBytes = ms.ToArray();
                byte[] resizedImage = fileBytes;//ResizeImage(fileBytes, StandardImageWidth, StandardImageHeight);
                user.HeaderImage = resizedImage;
            }
        }

        return new AuthorViewModel
        {
            AccountUser = await _userService.Update(user.UserName, user)
        };
    }

    public async Task SendEmailConfirmationEmail(User user, string token)
    {
        string appDomain = _configuration.GetSection("Application:ProdAppDomain").Value;
        string configurationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

        UserEmailOptions options = new UserEmailOptions
        {
            ToEmail = user.Email,
            FullName = user.FirstName + " " + user.LastName,
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
        string appDomain = _configuration.GetSection("Application:ProdAppDomain").Value;
        string configurationLink = _configuration.GetSection("Application:ForgotPassword").Value;

        UserEmailOptions options = new UserEmailOptions
        {
            ToEmail = user.Email,
            FullName = user.FirstName + " " + user.LastName,
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
            if (user.Email == "prab.dhaliwal95@gmail.com")
            {
                await ApplyAdminUserRole(user);
            }
            await ApplyVerifyUserRole(user);
        }
        return result;
    }

    public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model, ClaimsPrincipal claimsPrincipal)
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

    /// <summary>
    /// Applies "VerifiedUser" role to the user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task ApplyAdminUserRole(User user)
    {
        await _userManager.RemoveFromRoleAsync(user, UnverifiedUserRoleName);
        await _userManager.AddToRoleAsync(user, AdminUserRoleName);
    }
}
