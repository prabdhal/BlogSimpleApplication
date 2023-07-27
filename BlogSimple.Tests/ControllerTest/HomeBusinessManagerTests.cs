using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using FakeItEasy;

namespace BlogSimple.Tests.ControllerTest;

public class HomeBusinessManagerTests
{
    private IBlogService _blogService;
    public HomeBusinessManagerTests()
    {
        // Dependencies
        _blogService = A.Fake<IBlogService>();
    }
}
