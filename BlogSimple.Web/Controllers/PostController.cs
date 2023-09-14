using BlogSimple.Model.ViewModels.PostViewModels;
using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.Controllers;

public class PostController : Controller
{
    private readonly IPostBusinessManager _postBusinessManager;

    public PostController(IPostBusinessManager postBusinessManager)
    {
        _postBusinessManager = postBusinessManager;
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> Index(string searchString)
    {
        DashboardIndexViewModel postListViewModal = await _postBusinessManager.GetDashboardIndexViewModel(searchString, User);

        return View(postListViewModal);
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> FavoritePosts(string searchString)
    {
        FavoritePostsViewModel favoritePostsViewModel = await _postBusinessManager.GetFavoritePostsViewModel(searchString, User);

        return View(favoritePostsViewModel);
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> CreatePost()
    {
        CreatePostViewModel createPostViewModel = await _postBusinessManager.GetCreatePostViewModel(User);

        return View(createPostViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> CreatePost(CreatePostViewModel createPostViewModel)
    {
        await _postBusinessManager.CreatePost(createPostViewModel, User);
        return RedirectToAction("PostDetails", new { createPostViewModel.Post.Id });
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> EditPost(string id)
    {
        var editPostViewModel = await _postBusinessManager.GetEditPostViewModel(id, User);

        if (editPostViewModel is null)
            return new NotFoundResult();

        return View(editPostViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> EditPost(EditPostViewModel editPostViewModel)
    {
        await _postBusinessManager.EditPost(editPostViewModel, User);
        return RedirectToAction("PostDetails", new { editPostViewModel.Post.Id });
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> DeletePost(string id)
    {
        await _postBusinessManager.DeletePost(id, User);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> PostDetails(string id)
    {
        PostDetailsViewModel dashboardDetailsViewModal = await _postBusinessManager.GetPostDetailsViewModel(id, User);

        return View(dashboardDetailsViewModal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> FavoritePost(string id)
    {
        var postDetailsViewModel = await _postBusinessManager.FavoritePost(id, User);
        return RedirectToAction("PostDetails", new { postDetailsViewModel.Post.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> CreateComment(PostDetailsViewModel postDetailsViewModel)
    {
        await _postBusinessManager.CreateComment(postDetailsViewModel, User);
        return RedirectToAction("PostDetails", new { postDetailsViewModel.Post.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> EditComment(string id, PostDetailsViewModel postDetailsViewModel)
    {
        await _postBusinessManager.EditComment(id, postDetailsViewModel, User);
        return RedirectToAction("PostDetails", new { postDetailsViewModel.Post.Id });
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> DeleteComment(string id)
    {
        var viewModel = await _postBusinessManager.GetEditPostViewModelViaComment(id);
        _postBusinessManager.DeleteComment(id, User);

        return RedirectToAction("PostDetails", new { viewModel.Post.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> LikeComment(string id, PostDetailsViewModel postDetailsViewModel)
    {
        await _postBusinessManager.LikeComment(id, postDetailsViewModel, User);
        return RedirectToAction("PostDetails", new { postDetailsViewModel.Post.Id });
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateReply(PostDetailsViewModel postDetailsViewModel)
    {
        await _postBusinessManager.CreateReply(postDetailsViewModel, User);
        return RedirectToAction("PostDetails", new { postDetailsViewModel.Post.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> EditReply(string id, PostDetailsViewModel postDetailsViewModel)
    {
        await _postBusinessManager.EditReply(id, postDetailsViewModel, User);
        return RedirectToAction("PostDetails", new { postDetailsViewModel.Post.Id });
    }

    [Authorize(Roles = "VerifiedUser,Admin")]
    public async Task<IActionResult> DeleteReply(string id)
    {
        var viewModel = await _postBusinessManager.GetEditPostViewModelViaReply(id);
        _postBusinessManager.DeleteReply(id, User);

        return RedirectToAction("PostDetails", new { viewModel.Post.Id });
    }
}
