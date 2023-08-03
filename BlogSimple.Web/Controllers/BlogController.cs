using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.Controllers;

[Authorize]
public class BlogController : Controller
{
    private readonly IBlogBusinessManager _blogBusinessManager;

    public BlogController(IBlogBusinessManager blogBusinessManager)
    {
        _blogBusinessManager = blogBusinessManager;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        DashboardIndexViewModel blogListViewModal = await _blogBusinessManager.GetDashboardIndexViewModel(searchString, User);

        return View(blogListViewModal);
    }

    // GET: HomeController/Details/Id
    public IActionResult Details(string id)
    {
        BlogDetailsViewModel dashboardDetailsViewModal = _blogBusinessManager.GetDashboardDetailViewModel(id);

        return View(dashboardDetailsViewModal);
    }

    // GET: BlogController/Create
    public IActionResult CreateBlog()
    {
        return View(new CreateBlogViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateComment(BlogDetailsViewModel blogDetailsViewModel)
    {
        Comment comment = await _blogBusinessManager.CreateComment(blogDetailsViewModel, User);
        return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });
    }

    // POST: BlogController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBlog(CreateBlogViewModel createBlogViewModel)
    {
        if (!ModelState.IsValid)
            return View("Create");

        Blog blog = await _blogBusinessManager.CreateBlog(createBlogViewModel, User);
        return RedirectToAction("Details", new { createBlogViewModel.Blog.Id });
    }

    // GET: BlogController/Edit/5
    public IActionResult EditBlog(string id)
    {
        var editBlogViewModel = _blogBusinessManager.GetEditBlogViewModel(id);

        if (editBlogViewModel is null)
            return new NotFoundResult();

        return View(editBlogViewModel);
    }

    // POST: BlogController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditBlog(string id, EditBlogViewModel editBlogViewModel)
    {
        if (!ModelState.IsValid)
            return View("Edit");

        _blogBusinessManager.EditBlog(editBlogViewModel, User);
        return RedirectToAction("Details", new { editBlogViewModel.Blog.Id });
    }

    // POST: BlogController/DeleteBlog/5
    public IActionResult DeleteBlog(string id)
    {
        _blogBusinessManager.DeleteBlog(id);

        return RedirectToAction("Index");
    }
}
