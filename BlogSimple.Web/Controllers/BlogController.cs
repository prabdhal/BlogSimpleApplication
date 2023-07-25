using BlogSimple.Model.ViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.Controllers;

public class BlogController : Controller
{
    private readonly IBlogBusinessManager _blogBusinessManager;

    public BlogController(
        IBlogBusinessManager blogBusinessManager
        )
    {
        _blogBusinessManager = blogBusinessManager;
    }

    // GET: BlogController
    public ActionResult Index(string id)
    {
        BlogListViewModel blogListViewModal = _blogBusinessManager.GetBlogListViewModel();

        return View(blogListViewModal);
    }

    // GET: BlogController/Details/5
    public ActionResult Details(string id)
    {
        BlogViewModel blogViewModel = _blogBusinessManager.GetBlogViewModel(id);

        return View(blogViewModel);
    }

    // GET: BlogController/Create
    [Authorize]
    public ActionResult Create()
    {
        return View(new CreateBlogViewModel());
    }

    // POST: BlogController/Create
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public ActionResult Create(CreateBlogViewModel createBlogViewModel)
    {
        if (!ModelState.IsValid)
            return View("Create");

        _blogBusinessManager.CreateBlog(createBlogViewModel, User);
        return RedirectToAction("Details", new { createBlogViewModel.Blog.Id });
    }

    // GET: BlogController/Edit/5
    [Authorize]
    public ActionResult Edit(string id)
    {
        var editBlogViewModel = _blogBusinessManager.GetEditBlogViewModel(id);

        if (editBlogViewModel is null)
            return new NotFoundResult();

        return View(editBlogViewModel);
    }

    // POST: BlogController/Edit/5
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, EditBlogViewModel editBlogViewModel)
    {
        if (!ModelState.IsValid)
            return View("Edit");

        _blogBusinessManager.EditBlog(editBlogViewModel, User);
        return RedirectToAction("Details", new { editBlogViewModel.Blog.Id });
    }

    // POST: BlogController/Delete/5
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(string id)
    {
        _blogBusinessManager.DeleteBlog(id);

        return RedirectToAction("Index");
    }
}
