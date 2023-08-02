using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogSimple.Web.Controllers;

public class HomeController : Controller
{
    private readonly IHomeBusinessManager _homeBusinessManager;

    public HomeController(IHomeBusinessManager homeBusinessManager)
    {
        _homeBusinessManager = homeBusinessManager;
    }

    public IActionResult Index(string searchString)
    {
        HomeIndexViewModel homeIndexViewModal = _homeBusinessManager.GetHomeIndexViewModel(searchString);

        return View(homeIndexViewModal);
    }

    // GET: HomeController/Details/Id
    public ActionResult Details(string id)
    {
        BlogDetailsViewModel viewModel = _homeBusinessManager.GetHomeDetailsViewModel(id);

        return View(viewModel);
    }
}