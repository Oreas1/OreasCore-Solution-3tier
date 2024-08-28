using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace OreasCore.Areas.Qc.Controllers
{
    [Area("QC")]
    public class HomeController : Controller
    {
        [MyAuthorization]
        public IActionResult Index()
        {
            return View();
        }

        [MyAuthorization]
        public async Task<IActionResult> DashBoardGet([FromServices] IQcDashboard db)
        {
            return Json(await db.GetDashBoardData(User.Identity.Name), new Newtonsoft.Json.JsonSerializerSettings());
        }        

    }
}
