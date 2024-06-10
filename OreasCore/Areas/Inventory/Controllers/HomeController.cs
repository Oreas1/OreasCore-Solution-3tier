using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using OfficeOpenXml.Drawing.Controls;
using MailKit.Security;
using MimeKit;
using System.IO;

namespace OreasCore.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        [MyAuthorization]
        public IActionResult Index()
        {
            return View();
        }

        [MyAuthorization]
        public async Task<IActionResult> DashBoardGet([FromServices] IInventoryDashboard db)
        {
            return Json(await db.GetDashBoardData(User.Identity.Name), new Newtonsoft.Json.JsonSerializerSettings());
        }

        



    }
}
