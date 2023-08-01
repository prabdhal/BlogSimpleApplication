using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IHomeBusinessManager
{
    BlogDetailsViewModel GetHomeDetailsViewModel(string id);
    HomeIndexViewModel GetHomeIndexViewModel(string? searchString);
}
