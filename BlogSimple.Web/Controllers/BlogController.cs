using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels;
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
    public ActionResult Details(string id)
    {
        BlogDetailsViewModel dashboardDetailsViewModal = _blogBusinessManager.GetDashboardDetailViewModel(id);

        return View(dashboardDetailsViewModal);
    }

    // GET: BlogController/Create
    public ActionResult Create()
    {
        return View(new CreateBlogViewModel());
    }

    // POST: BlogController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateBlogViewModel createBlogViewModel)
    {
        if (!ModelState.IsValid)
            return View("Create");

        Blog blog = await _blogBusinessManager.CreateBlog(createBlogViewModel, User);
        return RedirectToAction("Details", new { createBlogViewModel.Blog.Id });
    }

    // GET: BlogController/Edit/5
    public ActionResult Edit(string id)
    {
        var editBlogViewModel = _blogBusinessManager.GetEditBlogViewModel(id);

        if (editBlogViewModel is null)
            return new NotFoundResult();

        return View(editBlogViewModel);
    }

    // POST: BlogController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(string id, EditBlogViewModel editBlogViewModel)
    {
        if (!ModelState.IsValid)
            return View("Edit");

        _blogBusinessManager.EditBlog(editBlogViewModel, User);
        return RedirectToAction("Details", new { editBlogViewModel.Blog.Id });
    }

    // POST: BlogController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(string id)
    {
        _blogBusinessManager.DeleteBlog(id);

        return RedirectToAction("Index");
    }
}
