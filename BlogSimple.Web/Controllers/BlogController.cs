using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.Controllers;

public class BlogController : Controller
{
    private readonly IBlogBusinessManager _blogBusinessManager;

    public BlogController(IBlogBusinessManager blogBusinessManager)
    {
        _blogBusinessManager = blogBusinessManager;
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> Index(string searchString)
    {
        DashboardIndexViewModel blogListViewModal = await _blogBusinessManager.GetDashboardIndexViewModel(searchString, User);

        return View(blogListViewModal);
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> FavoriteBlogs(string searchString)
    {
        FavoriteBlogsViewModel favoriteBlogsViewModel = await _blogBusinessManager.GetFavoriteBlogsViewModel(searchString, User);

        return View(favoriteBlogsViewModel);
    }

    // POST: BlogController/FavoriteBlog/5
    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FavoriteBlog(string id)
    {
        var blogDetailsViewModel = await _blogBusinessManager.FavoriteBlog(id, User);
        return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });
    }

    // GET: HomeController/Details/Id
    public async Task<IActionResult> Details(string id)
    {
        BlogDetailsViewModel dashboardDetailsViewModal = await _blogBusinessManager.GetBlogDetailsViewModel(id, User);

        return View(dashboardDetailsViewModal);
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    // GET: BlogController/Create
    public async Task<IActionResult> CreateBlog()
    {
        CreateBlogViewModel createBlogViewModel = await _blogBusinessManager.GetCreateViewModel(User);

        return View(createBlogViewModel);
    }

    // POST: BlogController/Create
    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBlog(CreateBlogViewModel createBlogViewModel)
    {
        //if (!ModelState.IsValid)
        //    return View("CreateBlog");

        await _blogBusinessManager.CreateBlog(createBlogViewModel, User);
        return RedirectToAction("Details", new { createBlogViewModel.Blog.Id });
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    // GET: BlogController/Edit/5
    public async Task<IActionResult> EditBlog(string id)
    {
        var editBlogViewModel = await _blogBusinessManager.GetEditBlogViewModel(id, User);

        if (editBlogViewModel is null)
            return new NotFoundResult();

        return View(editBlogViewModel);
    }

    // POST: BlogController/Edit/5
    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBlog(EditBlogViewModel editBlogViewModel)
    {
        //if (!ModelState.IsValid)
            //return View("EditBlog");

        await _blogBusinessManager.EditBlog(editBlogViewModel, User);
        return RedirectToAction("Details", new { editBlogViewModel.Blog.Id });
    }

    // POST: BlogController/DeleteBlog/5
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> DeleteBlog(string id)
    {
        await _blogBusinessManager.DeleteBlog(id);

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateComment(BlogDetailsViewModel blogDetailsViewModel)
    {
        // allow client side validation
        //if (!ModelState.IsValid)
        //    return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });

        await _blogBusinessManager.CreateComment(blogDetailsViewModel, User);

        return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateReply(BlogDetailsViewModel blogDetailsViewModel)
    {
        // allow client side validation
        //if (!ModelState.IsValid)
        //    return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });

        CommentReply reply = await _blogBusinessManager.CreateReply(blogDetailsViewModel, User);

        return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });
    }

    // POST: BlogController/EditComment/5
    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditComment(string id, BlogDetailsViewModel blogDetailsViewModel)
    {
        await _blogBusinessManager.EditComment(id, blogDetailsViewModel, User);
        return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });
    }

    // POST: BlogController/EditComment/5
    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditReply(string id, BlogDetailsViewModel blogDetailsViewModel)
    {
        await _blogBusinessManager.EditReply(id, blogDetailsViewModel, User);
        return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });
    }

    // POST: BlogController/DeleteBlog/5
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> DeleteComment(string id)
    {
        var viewModel = await _blogBusinessManager.GetEditBlogViewModelViaComment(id);
        _blogBusinessManager.DeleteComment(id);

        return RedirectToAction("Details", new { viewModel.Blog.Id });
    }

    // POST: BlogController/DeleteBlog/5
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> DeleteReply(string id)
    {
        var viewModel = await _blogBusinessManager.GetEditBlogViewModelViaReply(id);
        _blogBusinessManager.DeleteReply(id);

        return RedirectToAction("Details", new { viewModel.Blog.Id });
    }

    // POST: BlogController/LikeComment/5
    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LikeComment(string id, BlogDetailsViewModel blogDetailsViewModel)
    {
        await _blogBusinessManager.LikeComment(id, blogDetailsViewModel, User);
        return RedirectToAction("Details", new { blogDetailsViewModel.Blog.Id });
    }
}
