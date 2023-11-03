using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.PostViewModels;
using BlogSimple.Web.BusinessManager;
using BlogSimple.Web.Services.Interfaces;
using FluentAssertions;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Tests.BusinessManagerTests;

public class PostBusinessManagerTests
{
    private PostBusinessManager _postBusinessManager;

    private readonly UserManager<User> _userManager;
    private readonly IPostService _postService;
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _commentReplyService;
    private readonly IWebHostEnvironment webHostEnvironment;

    public PostBusinessManagerTests()
    {
        // Dependencies
        _userManager = A.Fake<UserManager<User>>();
        _postService = A.Fake<IPostService>();
        _userService = A.Fake<IUserService>();
        _commentService = A.Fake<ICommentService>();
        _commentReplyService = A.Fake<ICommentReplyService>();
        webHostEnvironment = A.Fake<IWebHostEnvironment>();

        // SUT
        _postBusinessManager = new PostBusinessManager(_userManager, _postService, _userService, _commentService, _commentReplyService, webHostEnvironment);
    }

    //[Fact]
    //public async void PostBusinessManager_CreatePost_ReturnPost()
    //{
    //    // Arrange
    //    CreatePostViewModel createPostViewModel = A.Fake<CreatePostViewModel>();
    //    Post b = A.Fake<Post>();
    //    ClaimsPrincipal claimsPrincipal = A.Fake<ClaimsPrincipal>();
    //    User user = new User();
    //    IFormFile headerImage = A.Fake<IFormFile>();
    //    createPostViewModel.HeaderImage = headerImage;

    //    A.CallTo(() => _userManager.GetUserAsync(claimsPrincipal)).Returns(user);

    //    // Act
    //    createPostViewModel.Post = b;
    //    Post post = createPostViewModel.Post;
    //    post.CreatedBy = user;
    //    post.CreatedOn = DateTime.Now;
    //    post.UpdatedOn = DateTime.Now;
    //    A.CallTo(() => _postService.Create(post)).Returns(post);

    //    // stores image file name 
    //    string webRootPath = webHostEnvironment.WebRootPath;
    //    string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

    //    EnsureFolder(pathToImage);

    //    IFormFile headerImg = createPostViewModel.HeaderImage;

    //    using (var fileStream = new FileStream(pathToImage, FileMode.Create))
    //    {
    //        await headerImg.CopyToAsync(fileStream);
    //    }

    //    var result = _postBusinessManager.CreatePost(createPostViewModel, claimsPrincipal);

    //    // Assert
    //    result.Should().BeOfType<Task<Post>>();
    //    result.Result.CreatedBy.Should().BeOfType<User>();
    //}

    private void EnsureFolder(string path)
    {
        string directoryName = Path.GetDirectoryName(path);
        if (directoryName.Length > 0)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }

    [Fact]
    public async void PostBusinessManager_EditPost_ReturnPostEditViewModel()
    {
        // Arrange 
        EditPostViewModel editViewModel = A.Fake<EditPostViewModel>();
        Post post = new Post();
        User user = new User();
        post.Id = "1";
        editViewModel.Post = post;
        var edittedId = editViewModel.Post.Id;
        A.CallTo(() => _postService.Get(edittedId)).Returns(post);
        ClaimsPrincipal claimsPrincipal = A.Fake<ClaimsPrincipal>();
        IFormFile headerImage = A.Fake<IFormFile>();
        editViewModel.HeaderImage = headerImage;

        // setting up random values for old post
        post.Title = "Old Post";

        // setting up random values for editted post 
        editViewModel.Post.Title = "Editted Post";

        post.Title = editViewModel.Post.Title;
        post.Category = editViewModel.Post.Category;
        post.Description = editViewModel.Post.Description;
        post.Content = editViewModel.Post.Content;
        post.IsPublished = editViewModel.Post.IsPublished;
        post.UpdatedOn = DateTime.Now;

        if (editViewModel.HeaderImage != null)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await editViewModel.HeaderImage.CopyToAsync(fileStream);
            }
        }

        A.CallTo(() => _userManager.GetUserAsync(claimsPrincipal)).Returns(user);

        A.CallTo(() => _postService.Update(edittedId, post)).Returns(post);

        EditPostViewModel viewModel = new EditPostViewModel
        {
            Post = post,
            AccountUser = user,
        };

        // Act
        var result = _postBusinessManager.EditPost(viewModel, claimsPrincipal);

        // Assert
        result.Should().BeOfType<Task<ActionResult<EditPostViewModel>>>();
    }

    //[Fact]
    //public void PostBusinessManager_DeletePost_ReturnPost()
    //{
    //    // Arrange
    //    Post post = new Post();
    //    string deletePostId = "1";
    //    post.Id = deletePostId;
    //    A.CallTo(() => _postService.Get(deletePostId)).Returns(post);
    //    A.CallTo(() => _commentService.RemoveAllByPost(deletePostId));
    //    A.CallTo(() => _commentReplyService.RemoveAllByPost(deletePostId));

    //    string webRootPath = webHostEnvironment.WebRootPath;
    //    string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}";

    //    string[] files = Directory.GetFiles(pathToImage, "*", SearchOption.AllDirectories);
    //    foreach (string file in files)
    //    {
    //        File.Delete(file);
    //    }
    //    //then delete folder
    //    Directory.Delete(pathToImage);

    //    A.CallTo(() => _postService.Remove(post));

    //    // Act
    //    //var result = _postBusinessManager.DeletePost(deletePostId);

    //    // Assert
    //    result.Should().BeOfType<Task<ActionResult<Post>>>();
    //}


}
