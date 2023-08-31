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
    private readonly IBlogService _blogService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _replyService;
    private readonly IWebHostEnvironment webHostEnvironment;

    public AccountBusinessManager(
        UserManager<User> userManager,
        IUserService userService,
        IBlogService blogService,
        ICommentService commentService,
        ICommentReplyService replyService,
        IWebHostEnvironment webHostEnvironment
        )
    {
        _userManager = userManager;
        _userService = userService;
        _blogService = blogService;
        _commentService = commentService;
        _replyService = replyService;
        this.webHostEnvironment = webHostEnvironment;
    }

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
}
