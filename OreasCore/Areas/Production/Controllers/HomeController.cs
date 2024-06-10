using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OreasCore.Areas.Production.Controllers
{
    [Area("Production")]
    public class HomeController : Controller
    {
        [MyAuthorization]
        public IActionResult Index()
        {
            return View();
        }

        [MyAuthorization]
        public async Task<IActionResult> DashBoardGet([FromServices] IProductionDashboard db)
        {
            return Json(await db.GetDashBoardData(User.Identity.Name), new Newtonsoft.Json.JsonSerializerSettings());
        }

    }
}
