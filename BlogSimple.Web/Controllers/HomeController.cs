﻿using BlogSimple.Model.ViewModels;
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