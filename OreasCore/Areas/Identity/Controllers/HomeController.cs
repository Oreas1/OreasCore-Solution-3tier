using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OreasCore.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class HomeController : Controller
    {
        [MyAuthorization]
        public IActionResult Index()
        {            
            return View();

        }
    }
}
