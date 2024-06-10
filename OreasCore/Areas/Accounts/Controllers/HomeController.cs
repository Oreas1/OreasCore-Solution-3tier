using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;

namespace OreasCore.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class HomeController : Controller
    {
        [MyAuthorization]
        public IActionResult Index()
        {
            return View();
        }

        [MyAuthorization]
        public async Task<IActionResult> DashBoardGet([FromServices] IAccountsDashboard db)
        {
            return Json(await db.GetDashBoardData(User.Identity.Name), new Newtonsoft.Json.JsonSerializerSettings());
        }

        

    }
}
