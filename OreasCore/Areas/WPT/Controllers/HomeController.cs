using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OreasCore.Areas.WPT.Controllers
{
    [Area("WPT")]
    
    public class HomeController : Controller
    {
        [MyAuthorization]
        public IActionResult Index()
        {
            return View();
        }

        [MyAuthorization]
        public async Task<IActionResult> DashBoardGet([FromServices] IWPTDashboard db)
        {
            return Json(await db.GetDashBoardData(User.Identity.Name), new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}
