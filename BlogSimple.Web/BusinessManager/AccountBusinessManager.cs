using BlogSimple.Model.Models;
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
    private readonly IWebHostEnvironment webHostEnvironment;

    public AccountBusinessManager(
        UserManager<User> userManager,
        IUserService userService,
        IWebHostEnvironment webHostEnvironment
        )
    {
        _userManager = userManager;
        _userService = userService;
        this.webHostEnvironment = webHostEnvironment;
    }


    public async Task<AuthorViewModel> GetAuthorViewModel(string id, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userService.Get(id);

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

        user.Description = aboutViewModel.AccountUser.Description;
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
