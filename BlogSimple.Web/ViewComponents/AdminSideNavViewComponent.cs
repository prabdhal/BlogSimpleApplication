using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.ViewComponents;

public class AdminSideNavViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string id)
    {
        return await Task.Factory.StartNew(() => { return View(); });
    }
}
