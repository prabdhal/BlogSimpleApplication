using Microsoft.AspNetCore.Mvc;

namespace BlogSimple.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}