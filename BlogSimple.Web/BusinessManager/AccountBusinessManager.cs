using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.AccountViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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


    public async Task<AboutMeViewModel> GetAboutMeViewModel(ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);


        return new AboutMeViewModel
        {
            User = user,
        };
    }

    public async Task<ActionResult<AboutMeViewModel>> EditUser(AboutMeViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        if (user is null)
            return new NotFoundResult();

        user.Description = aboutViewModel.User.Description;
        user.Content = aboutViewModel.User.Content;
        user.PortfolioLink = aboutViewModel.User.PortfolioLink;
        user.TwitterLink = aboutViewModel.User.TwitterLink;
        user.GitHubLink = aboutViewModel.User.GitHubLink;
        user.LinkedInLink = aboutViewModel.User.LinkedInLink;

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

        return new AboutMeViewModel
        {
            User = await _userService.Update(user.UserName, user)
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
