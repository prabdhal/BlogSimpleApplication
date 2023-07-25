using BlogSimple.Model.ViewModels;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IHomeBusinessManager
{
    public HomeIndexViewModel GetHomeIndexViewModel();
}
