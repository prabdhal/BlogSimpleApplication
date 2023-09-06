using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Model.ViewModels.BlogViewModels;
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

public class BlogBusinessManagerTests
{
    private BlogBusinessManager _blogBusinessManager;

    private readonly UserManager<User> _userManager;
    private readonly IBlogService _blogService;
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    private readonly ICommentReplyService _commentReplyService;
    private readonly IWebHostEnvironment webHostEnvironment;

    public BlogBusinessManagerTests()
    {
        // Dependencies
        _userManager = A.Fake<UserManager<User>>();
        _blogService = A.Fake<IBlogService>();
        _userService = A.Fake<IUserService>();
        _commentService = A.Fake<ICommentService>();
        _commentReplyService = A.Fake<ICommentReplyService>();
        webHostEnvironment = A.Fake<IWebHostEnvironment>();

        // SUT
        _blogBusinessManager = new BlogBusinessManager(_userManager, _blogService, _userService, _commentService, _commentReplyService, webHostEnvironment);
    }

    [Fact]
    public async void BlogBusinessManager_CreateBlog_ReturnBlog()
    {
        // Arrange
        CreateBlogViewModel createViewModel = A.Fake<CreateBlogViewModel>();
        Blog b = A.Fake<Blog>();
        ClaimsPrincipal claimsPrincipal = A.Fake<ClaimsPrincipal>();
        User user = new User();
        IFormFile headerImage = A.Fake<IFormFile>();
        createViewModel.HeaderImage = headerImage;

        A.CallTo(() => _userManager.GetUserAsync(claimsPrincipal)).Returns(user);

        // Act
        createViewModel.Blog = b;
        Blog blog = createViewModel.Blog;
        blog.CreatedBy = user;
        blog.CreatedOn = DateTime.Now;
        blog.UpdatedOn = DateTime.Now;
        A.CallTo(() => _blogService.Create(blog)).Returns(blog);

        // stores image file name 
        string webRootPath = webHostEnvironment.WebRootPath;
        string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

        EnsureFolder(pathToImage);

        IFormFile headerImg = createViewModel.HeaderImage;

        using (var fileStream = new FileStream(pathToImage, FileMode.Create))
        {
            await headerImg.CopyToAsync(fileStream);
        }

        var result = _blogBusinessManager.CreateBlog(createViewModel, claimsPrincipal);

        // Assert
        result.Should().BeOfType<Task<Blog>>();
        result.Result.CreatedBy.Should().BeOfType<User>();
    }

    private void EnsureFolder(string path)
    {
        string directoryName = Path.GetDirectoryName(path);
        if (directoryName.Length > 0)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }

    [Fact]
    public async void BlogBusinessManager_EditBlog_ReturnEditViewModel()
    {
        // Arrange 
        EditBlogViewModel editViewModel = A.Fake<EditBlogViewModel>();
        Blog blog = new Blog();
        User user = new User();
        blog.Id = "1";
        editViewModel.Blog = blog;
        var edittedId = editViewModel.Blog.Id;
        A.CallTo(() => _blogService.Get(edittedId)).Returns(blog);
        ClaimsPrincipal claimsPrincipal = A.Fake<ClaimsPrincipal>();
        IFormFile headerImage = A.Fake<IFormFile>();
        editViewModel.HeaderImage = headerImage;

        // setting up random values for old blog
        blog.Title = "Old Blog";

        // setting up random values for editted blog 
        editViewModel.Blog.Title = "Editted Blog";

        blog.Title = editViewModel.Blog.Title;
        blog.Category = editViewModel.Blog.Category;
        blog.Description = editViewModel.Blog.Description;
        blog.Content = editViewModel.Blog.Content;
        blog.IsPublished = editViewModel.Blog.IsPublished;
        blog.UpdatedOn = DateTime.Now;

        if (editViewModel.HeaderImage != null)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await editViewModel.HeaderImage.CopyToAsync(fileStream);
            }
        }

        A.CallTo(() => _userManager.GetUserAsync(claimsPrincipal)).Returns(user);

        A.CallTo(() => _blogService.Update(edittedId, blog)).Returns(blog);

        EditBlogViewModel viewModel = new EditBlogViewModel
        {
            Blog = blog,
            AccountUser = user,
        };

        // Act
        var result = _blogBusinessManager.EditBlog(viewModel, claimsPrincipal);

        Console.WriteLine("result");
        // Assert
        result.Should().BeOfType<Task<ActionResult<EditBlogViewModel>>>();
    }
}
