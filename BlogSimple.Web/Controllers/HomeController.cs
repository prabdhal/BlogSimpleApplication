using BlogSimple.Model.ViewModels.AccountViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.Controllers;

public class HomeController : Controller
{
    private readonly IHomeBusinessManager _homeBusinessManager;
    private readonly IAccountBusinessManager _accountBusinessManager;

    public HomeController(IHomeBusinessManager homeBusinessManager, IAccountBusinessManager accountBusinessManager)
    {
        _homeBusinessManager = homeBusinessManager; 
        _accountBusinessManager = accountBusinessManager;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        HomeIndexViewModel homeIndexViewModel = await _homeBusinessManager.GetHomeIndexViewModel(searchString);

        return View(homeIndexViewModel);
    }

    public async Task<IActionResult> Author(string id)
    {
        AuthorViewModel authorViewModel = await _accountBusinessManager.GetAuthorViewModel(id);

        return View(authorViewModel);
    }
}