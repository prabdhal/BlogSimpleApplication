using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.ViewComponents;

public class AdminFooterViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.Factory.StartNew(() => { return View(); });
    }
}
