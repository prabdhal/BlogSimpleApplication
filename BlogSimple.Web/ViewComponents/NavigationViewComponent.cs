using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.ViewComponents;

public class NavigationViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.Factory.StartNew(() => { return View(); });
    }
}
