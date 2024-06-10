using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class CalendarController : Controller
    {
        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetMonthListAsync([FromServices] IWPTList IList,string QueryName = "", int SearchMonth = 1, int SearchYear = 2000, int FormID = 0)
        {
            
            return Json(
                await IList.GetMonthListAsync(QueryName, SearchMonth, SearchYear, FormID)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Calendar

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCalendarAsync([FromServices] IAuthorizationScheme db, [FromServices] ICalendar db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CalendarIndexCtlr",
                        WildCard = db2.GetWCLCalendar(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Calendar"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CalendarMonthCtlr",
                        WildCard = db2.GetWCLCalendarMonth(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CalendarEmployeeForPLCtlr",
                        WildCard = db2.GetWCLCalendarEmployeeForPL(),
                        Reports = db2.GetRLCalendar(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CalendarPLOfEmployeeCtlr",
                        WildCard = db2.GetWCLCalendarPLOfEmployee(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { LeavePolicyList=await IList.GetPaidLeavePolicyListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Calendar Index

        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public IActionResult CalendarIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public async Task<IActionResult> CalendarLoad([FromServices] ICalendar db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCalendar(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Calendar", Operation = "CanPost")]
        public async Task<string> CalendarPost([FromServices] ICalendar db, [FromBody] tbl_WPT_CalendarYear tbl_WPT_CalendarYear, string operation = "")
        {
            if (ModelState.IsValid)                
                return await db.PostCalendar(tbl_WPT_CalendarYear, operation, User.Identity.Name);                
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }
        
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Calendar", Operation = "CanPost")]
        public async Task<string> CalendarClose([FromServices] ICalendar db, int CalendarID, string operation)
        {
            if (ModelState.IsValid && operation =="Save Update")
                return await db.CloseYear(CalendarID, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public async Task<IActionResult> CalendarGet([FromServices] ICalendar db, int ID)
        {
            return Json(await db.GetCalendar(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        #region Calendar Month


        [AjaxOnly]
        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public async Task<IActionResult> CalendarMonthLoad([FromServices] ICalendar db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCalendarMonth(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Calendar", Operation = "CanPost")]
        public async Task<string> CalendarMonthPost([FromServices] ICalendar db, [FromBody] VM_CalendarYear_Months_Adjustment VM_CalendarYear_Months_Adjustment, string operation = "")
        {
            if (ModelState.IsValid)
            {
                return await db.PostCalendarMonth(VM_CalendarYear_Months_Adjustment, operation, User.Identity.Name);
            }

            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public async Task<IActionResult> CalendarMonthGet([FromServices] ICalendar db, int ID)
        {
            return Json(await db.GetCalendarMonth(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }



        #endregion

        #region Calendar Employee for Paid Leave


        [AjaxOnly]
        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public async Task<IActionResult> CalendarEmployeeForPLLoad([FromServices] ICalendar db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCalendarEmployeeForPL(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Calendar", Operation = "CanPost")]
        public async Task<string> CalendarEmployeeForPLPost([FromServices] ICalendar db, [FromBody] tbl_WPT_CalendarYear_LeaveEmps tbl_WPT_CalendarYear_LeaveEmps, string operation = "")
        {
            if (ModelState.IsValid)
            {
                return await db.PostCalendarEmployeeForPL(tbl_WPT_CalendarYear_LeaveEmps, operation, User.Identity.Name);
            }

            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public async Task<IActionResult> CalendarEmployeeForPLGet([FromServices] ICalendar db, int ID)
        {
            return Json(await db.GetCalendarEmployeeForPL(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        #region Calendar Paid Leave Of Employee

        [AjaxOnly]
        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public async Task<IActionResult> CalendarPLOfEmployeeLoad([FromServices] ICalendar db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCalendarPLOfEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Calendar", Operation = "CanPost")]
        public async Task<string> CalendarPLOfEmployeePost([FromServices] ICalendar db, [FromBody] tbl_WPT_CalendarYear_LeaveEmps_Leaves tbl_WPT_CalendarYear_LeaveEmps_Leaves, string operation = "")
        {
            if (ModelState.IsValid)
            {
                return await db.PostCalendarPLOfEmployee(tbl_WPT_CalendarYear_LeaveEmps_Leaves, operation, User.Identity.Name);
            }

            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Calendar", Operation = "CanView")]
        public async Task<IActionResult> CalendarPLOfEmployeeGet([FromServices] ICalendar db, int ID)
        {
            return Json(await db.GetCalendarPLOfEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        #endregion

        [MyAuthorization(FormName = "Calendar", Operation = "CanViewReport")]
        public async Task<IActionResult> GetReport([FromServices] ICalendar db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID), "application/pdf");
        }

    }
}
