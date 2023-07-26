using BlogSimple.Model.ViewModels;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IHomeBusinessManager
{
    HomeDetailsViewModel GetHomeDetailsViewModel(string id);
    HomeIndexViewModel GetHomeIndexViewModel();
}
