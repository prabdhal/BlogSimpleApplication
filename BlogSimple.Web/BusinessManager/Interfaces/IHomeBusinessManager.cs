using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IHomeBusinessManager
{
    Task<BlogDetailsViewModel> GetHomeDetailsViewModel(string id);
    Task<HomeIndexViewModel> GetHomeIndexViewModel(string searchString);
}
