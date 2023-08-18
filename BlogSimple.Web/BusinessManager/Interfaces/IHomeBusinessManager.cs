using BlogSimple.Model.ViewModels.BlogViewModels;
using BlogSimple.Model.ViewModels.HomeViewModels;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IHomeBusinessManager
{
    Task<HomeIndexViewModel> GetHomeIndexViewModel(string searchString);
}
