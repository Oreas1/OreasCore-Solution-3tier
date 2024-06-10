using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Models;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OreasCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index(string ReturnUrl = null)
        {
            ViewBag.returnUrl = ReturnUrl;

            if (HttpContext.Request.Cookies.TryGetValue("AuthWareHouseList", out string serializedStoreList))
            {
                var a = JsonConvert.DeserializeObject<List<int>>(serializedStoreList);
            }

            return View();       
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }


        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied(string ReturnUrl="") 
        {
            return View();

        }

        

    }
}
