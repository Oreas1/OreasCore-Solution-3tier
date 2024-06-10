using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OreasCore.Areas.WPT.Controllers
{
    [Area("WPT")]
    public class MachineController : Controller
    {
        #region Machine

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedMachineAsync([FromServices] IAuthorizationScheme db, [FromServices] IMachine db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "MachineIndexCtlr",
                        WildCard = db2.GetWCLMachine(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Machine"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                ); ;
        }

        #region Machine Master

        [MyAuthorization(FormName = "Machine", Operation = "CanView")]
        public IActionResult MachineIndex([FromServices] IEnumerable<IHostedService> serviceProviders)
        {
            var service = serviceProviders.FirstOrDefault(w => w.GetType() == typeof(MyBackgroundTask));

            if (service != null)
            {
                ViewBag.CurrentDateTime = ((MyBackgroundTask)service).GetCurrentDateTime;
            }
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Machine", Operation = "CanView")]
        public async Task<IActionResult> MachineLoad([FromServices] IMachine db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadMachine(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Machine", Operation = "CanPost")]
        public async Task<string> MachinePost([FromServices] IMachine db, [FromBody] tbl_WPT_Machine tbl_WPT_Machine, string operation = "")
        {
            if (ModelState.IsValid)
            {
                var msg = await db.PostMachine(tbl_WPT_Machine, operation, User.Identity.Name);

                if (msg == "OK")
                {
                    Jobs.Queue.RemoveAll(x => x.MachineID == tbl_WPT_Machine.ID);

                    if (operation == "Save New" || operation == "Save Update")
                    {
                        if (tbl_WPT_Machine.ScheduledDownloadDailyAT.HasValue)
                            Jobs.Queue.Add(
                            new JobStructure(
                                tbl_WPT_Machine.ScheduledDownloadDailyAT.Value.Hours,
                                tbl_WPT_Machine.ScheduledDownloadDailyAT.Value.Minutes,
                                tbl_WPT_Machine.IP,
                                tbl_WPT_Machine.PortNo,
                                tbl_WPT_Machine.ID,
                                tbl_WPT_Machine.AutoClearLogAfterDownload
                                )
                            );
                        if (tbl_WPT_Machine.ScheduledDownloadDailyAT2.HasValue)
                            Jobs.Queue.Add(
                            new JobStructure(
                                tbl_WPT_Machine.ScheduledDownloadDailyAT2.Value.Hours,
                                tbl_WPT_Machine.ScheduledDownloadDailyAT2.Value.Minutes,
                                tbl_WPT_Machine.IP,
                                tbl_WPT_Machine.PortNo,
                                tbl_WPT_Machine.ID,
                                tbl_WPT_Machine.AutoClearLogAfterDownload
                                )
                            );
                    }

                    
                }
                return msg;
            }               
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Machine", Operation = "CanView")]
        public async Task<IActionResult> MachineGet([FromServices] IMachine db, int ID)
        {
            return Json(await db.GetMachine(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Machine Operations

        [MyAuthorization(FormName = "Machine", Operation = "CanView")]
        public async Task<IActionResult> MachineOperationsAsync([FromServices]IMachine db, int ID)
        {
            var machine = await db.GetMachineObject(ID);


            if (machine != null)
                return View(machine);
            else
                return RedirectToAction("Error", "Home", new { area = "", msg = "Machine Not found on given ID" });


        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Machine", Operation = "CanView")]
        public async Task<IActionResult> GetEmployeePFF([FromServices] IEmployee db,int EmpID = 0)
        {
            return Json(await db.GetEmployeeFFCPTemplate(EmpID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion
    }
}
