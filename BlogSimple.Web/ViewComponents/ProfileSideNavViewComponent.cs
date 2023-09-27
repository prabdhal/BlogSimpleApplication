using BlogSimple.Web.BusinessManager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSimple.Web.ViewComponents;

public class ProfileSideNavViewComponent : ViewComponent
{
    private readonly IAccountBusinessManager _accountBusinessManager;

    public ProfileSideNavViewComponent(IAccountBusinessManager accountBusinessManager)
    {
        _accountBusinessManager = accountBusinessManager;
    }

    public IViewComponentResult Invoke()
    {
        var result = _accountBusinessManager.GetMyAccountViewModel(User as ClaimsPrincipal);
        return View(result);
    }
}
