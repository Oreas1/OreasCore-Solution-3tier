using Microsoft.AspNetCore.Mvc;

namespace OreasCore.Areas.QA.Controllers
{
    [Area("QA")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
