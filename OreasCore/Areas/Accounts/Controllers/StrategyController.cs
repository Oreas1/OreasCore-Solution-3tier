using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OreasCore.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class StrategyController : Controller
    {
        #region PaymentPlanning

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPaymentPlanningAsync([FromServices] IAuthorizationScheme db, [FromServices] IPaymentPlanning db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PaymentPlanningCtlr",
                        WildCard = db2.GetWCLFiscalYear(),
                        LoadByCard = null,
                        Reports = db2.GetRLPaymentPlanning(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Payment Planning"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PaymentPlanningMasterCtlr",
                        WildCard = null,
                        Reports = db2.GetRLPaymentPlanning(),
                        Privilege = null,
                        Otherdata = new { 
                            MonthList = new[]
                                        {                                            
                                            new { ID = 7, Name = "July" },new { ID = 8, Name = "August" },
                                            new { ID = 9, Name = "September" },new { ID = 10, Name = "October" },
                                            new { ID = 11, Name = "November" },new { ID = 12, Name = "December" },
                                            new { ID = 1, Name = "January" },new { ID = 2, Name = "February" },
                                            new { ID = 3, Name = "March" },new { ID = 4, Name = "April" },
                                            new { ID = 5, Name = "May" },new { ID = 6, Name = "June" }
                                        }
                            }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PaymentPlanningDetailCtlr",
                        WildCard = db2.GetWCLPaymentPlanningDetail(),
                        Reports = db2.GetRLPaymentPlanningDetail(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Payment Planning", Operation = "CanView")]
        public IActionResult PaymentPlanningIndex()
        {
            return View();
        }

        #region PaymentPlanningFiscalYear

        [AjaxOnly]
        [MyAuthorization(FormName = "Payment Planning", Operation = "CanView")]
        public async Task<IActionResult> PaymentPlanningLoad([FromServices] IPaymentPlanning db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadFiscalYear(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Payment Planning", Operation = "CanView")]
        public async Task<IActionResult> PaymentPlanningMasterLoad([FromServices] IPaymentPlanning db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPaymentPlanningMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Payment Planning", Operation = "CanPost")]
        public async Task<string> PaymentPlanningMasterPost([FromServices] IPaymentPlanning db, [FromBody] tbl_Ac_PaymentPlanningMaster tbl_Ac_PaymentPlanningMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPaymentPlanningMaster(tbl_Ac_PaymentPlanningMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Payment Planning", Operation = "CanView")]
        public async Task<IActionResult> PaymentPlanningMasterGet([FromServices] IPaymentPlanning db, int ID)
        {
            return Json(await db.GetPaymentPlanningMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Payment Planning", Operation = "CanView")]
        public async Task<IActionResult> PaymentPlanningDetailLoad([FromServices] IPaymentPlanning db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPaymentPlanningDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Payment Planning", Operation = "CanPost")]
        public async Task<string> PaymentPlanningDetailPost([FromServices] IPaymentPlanning db, [FromBody] tbl_Ac_PaymentPlanningDetail tbl_Ac_PaymentPlanningDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPaymentPlanningDetail(tbl_Ac_PaymentPlanningDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Payment Planning", Operation = "CanView")]
        public async Task<IActionResult> PaymentPlanningDetailGet([FromServices] IPaymentPlanning db, int ID)
        {
            return Json(await db.GetPaymentPlanningDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "Payment Planning", Operation = "CanView")]
        public async Task<IActionResult> PaymentPlanningDetailOutStandingGet([FromServices] IPaymentPlanning db, int AcID, int MasterID)
        {
            return Json(await db.GetPaymentPlanningOutStanding(AcID, MasterID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Payment Planning", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPaymentPlanningReport([FromServices] IPaymentPlanning db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion
        #endregion
    }
}
