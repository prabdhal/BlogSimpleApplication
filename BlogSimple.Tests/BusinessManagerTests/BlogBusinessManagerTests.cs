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
        CreateBlogViewModel viewModel = A.Fake<CreateBlogViewModel>();
        Blog b = A.Fake<Blog>();
        var u = A.Fake<ClaimsPrincipal>();
        User user = new User();
        IFormFile headerImage = A.Fake<IFormFile>();
        viewModel.HeaderImage = headerImage;

        A.CallTo(() => _userManager.GetUserAsync(u)).Returns(user);

        // Act
        viewModel.Blog = b;
        Blog blog = viewModel.Blog;
        blog.CreatedBy = user;
        blog.CreatedOn = DateTime.Now;
        blog.UpdatedOn = DateTime.Now;
        A.CallTo(() => _blogService.Create(blog)).Returns(blog);

        // stores image file name 
        string webRootPath = webHostEnvironment.WebRootPath;
        string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

        EnsureFolder(pathToImage);

        IFormFile headerImg = viewModel.HeaderImage;

        using (var fileStream = new FileStream(pathToImage, FileMode.Create))
        {
            await headerImg.CopyToAsync(fileStream);
        }

        var result = _blogBusinessManager.CreateBlog(viewModel, u);

        // Assert
        result.Should().BeOfType<Task<Blog>>();
    }

    private void EnsureFolder(string path)
    {
        string directoryName = Path.GetDirectoryName(path);
        if (directoryName.Length > 0)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
    }
}
