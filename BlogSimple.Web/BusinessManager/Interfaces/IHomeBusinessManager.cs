using BlogSimple.Model.ViewModels;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IHomeBusinessManager
{
    BlogDetailsViewModel GetHomeDetailsViewModel(string id);
    HomeIndexViewModel GetHomeIndexViewModel(string? searchString);
}
