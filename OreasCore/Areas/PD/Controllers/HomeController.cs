using Microsoft.AspNetCore.Mvc;

namespace OreasCore.Areas.PD.Controllers
{
    [Area("PD")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
