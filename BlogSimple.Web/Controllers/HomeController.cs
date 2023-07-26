using BlogSimple.Model.ViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.Controllers;

public class HomeController : Controller
{
    private readonly IHomeBusinessManager _homeBusinessManager;

    public HomeController(IHomeBusinessManager homeBusinessManager)
    {
        _homeBusinessManager = homeBusinessManager;
    }

    public IActionResult Index()
    {
        HomeIndexViewModel homeIndexViewModal = _homeBusinessManager.GetHomeIndexViewModel();

        return View(homeIndexViewModal);
    }

    // GET: HomeController/Details/Id
    public ActionResult Details(string id)
    {
        HomeDetailsViewModel homeDetailsViewModal = _homeBusinessManager.GetHomeDetailsViewModel(id);

        return View(homeDetailsViewModal);
    }
}