﻿using BlogSimple.Model.Models;
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
    private readonly IRoleService _roleService;
    private readonly IBlogService _blogService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _replyService;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IConfiguration _configuration;

    public AccountBusinessManager(
        UserManager<User> userManager,
        IUserService userService,
        IRoleService roleService,
        IBlogService blogService,
        ICommentService commentService,
        ICommentReplyService replyService,
        IEmailService emailService,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment
        )
    {
        _userManager = userManager;
        _userService = userService;
        _roleService = roleService;
        _blogService = blogService;
        _commentService = commentService;
        _replyService = replyService;
        _emailService = emailService;
        _configuration = configuration;
        this.webHostEnvironment = webHostEnvironment;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
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

    //public async Task<SignInResult> PasswordSignInAsync(User user)
    //{
    //    return await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);
    //}

    public async Task<MyAccountViewModel> GetMyAccountViewModel(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        var publishedBlog = await _blogService.GetAll(user);
        var publishedBlogCount = publishedBlog.Count();

        var comments = await _commentService.GetAll(user);
        var replies = await _replyService.GetAll(user);
        var totalCommentsAndRepliesCount = comments.Count() + replies.Count();
        var favoritedBlogsCount = user.FavoritedBlogs.Count();

        return new MyAccountViewModel
        {
            AccountUser = user,
            PublishedBlogsCount = publishedBlogCount,
            TotalCommentsAndRepliesCount = totalCommentsAndRepliesCount,
            FavoriteBlogsCount = favoritedBlogsCount
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
            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Users\{user.Id}\HeaderImage.jpg";

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
        return await _userManager.ConfirmEmailAsync(user, token);
    }
}
