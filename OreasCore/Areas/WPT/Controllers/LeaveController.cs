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
    public class LeaveController : Controller
    {
        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetLeavePoliciesListAsync([FromServices] IWPTList IList, string QueryName = "", int EmployeeID = 0, int MonthID = 0)
        {
            return Json(
                await IList.GetLeavePoliciesWithBalanceByEmployeeListAsync(QueryName, EmployeeID, MonthID)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        #region Leave Policy

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedLeavePolicyAsync([FromServices] IAuthorizationScheme db, [FromServices] ILeavePolicy db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LeavePolicyIndexCtlr",
                        WildCard = db2.GetWCLLeavePolicy(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Leave Policy"),
                        Otherdata = new {
                            LeaveCFOptionsList = await IList.GetLeaveCFOptionsListAsync(null, null),
                            CalculationMethodList = await IList.GetCalculationMethodListAsync(null, null),
                            EncashablePeriodList = IList.GetEncashablePeriodList()
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Leave Policy", Operation = "CanView")]
        public IActionResult LeavePolicyIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Leave Policy", Operation = "CanView")]
        public async Task<IActionResult> LeavePolicyLoad([FromServices] ILeavePolicy db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadLeavePolicy(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Leave Policy", Operation = "CanPost")]
        public async Task<string> LeavePolicyPost([FromServices] ILeavePolicy db, [FromBody] tbl_WPT_LeavePolicy tbl_WPT_LeavePolicy, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostLeavePolicy(tbl_WPT_LeavePolicy, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Leave Policy", Operation = "CanView")]
        public async Task<IActionResult> LeavePolicyGet([FromServices] ILeavePolicy db, int ID)
        {
            return Json(await db.GetLeavePolicy(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Leave Policy Non Paid

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedLeavePolicyNonPaidAsync([FromServices] IAuthorizationScheme db, [FromServices] ILeavePolicy db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LeavePolicyNonPaidIndexCtlr",
                        WildCard = db2.GetWCLLeavePolicyNonPaid(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Leave Policy Non Paid"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LeavePolicyNonPaidDesignationCtlr",
                        WildCard = db2.GetWCLLeavePolicyNonPaidDesignation(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            DesignationList = await IList.GetDesignationListAsync(null, null)
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Policy


        [MyAuthorization(FormName = "Leave Policy Non Paid", Operation = "CanView")]
        public IActionResult LeavePolicyNonPaidIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Leave Policy Non Paid", Operation = "CanView")]
        public async Task<IActionResult> LeavePolicyNonPaidLoad([FromServices] ILeavePolicy db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadLeavePolicyNonPaid(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Leave Policy Non Paid", Operation = "CanPost")]
        public async Task<string> LeavePolicyNonPaidPost([FromServices] ILeavePolicy db, [FromBody] tbl_WPT_LeavePolicyNonPaid tbl_WPT_LeavePolicyNonPaid, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostLeavePolicyNonPaid(tbl_WPT_LeavePolicyNonPaid, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Leave Policy Non Paid", Operation = "CanView")]
        public async Task<IActionResult> LeavePolicyNonPaidGet([FromServices] ILeavePolicy db, int ID)
        {
            return Json(await db.GetLeavePolicyNonPaid(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Policy Designation

        [AjaxOnly]
        [MyAuthorization(FormName = "Leave Policy Non Paid", Operation = "CanView")]
        public async Task<IActionResult> LeavePolicyNonPaidDesignationLoad([FromServices] ILeavePolicy db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadLeavePolicyNonPaidDesignation(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Leave Policy Non Paid", Operation = "CanPost")]
        public async Task<string> LeavePolicyNonPaidDesignationPost([FromServices] ILeavePolicy db, [FromBody] tbl_WPT_LeavePolicyNonPaid_Designation tbl_WPT_LeavePolicyNonPaid_Designation, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostLeavePolicyNonPaidDesignation(tbl_WPT_LeavePolicyNonPaid_Designation, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Leave Policy Non Paid", Operation = "CanView")]
        public async Task<IActionResult> LeavePolicyNonPaidDesignationGet([FromServices] ILeavePolicy db, int ID)
        {
            return Json(await db.GetLeavePolicyNonPaidDesignation(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region Leave Requisition

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedLeaveRequisitionAsync([FromServices] IAuthorizationScheme db, [FromServices] ILeaveRequisition db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LeaveRequisitionMasterCtlr",
                        WildCard = db2.GetWCLCalendarMonth(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Leave Requisition"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LeaveRequisitionDetailCtlr",
                        WildCard = db2.GetWCLLeaveRequisition(),
                        LoadByCard = db2.GetWCLBLeaveRequisition(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { ActionList = await IList.GetActionList() }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Master

        [MyAuthorization(FormName = "Leave Requisition", Operation = "CanView")]
        public IActionResult LeaveRequisitionIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Leave Requisition", Operation = "CanView")]
        public async Task<IActionResult> LeaveRequisitionMasterLoad([FromServices] ILeaveRequisition db, [FromServices] OreasDbContext context,
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

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Leave Requisition", Operation = "CanView")]
        public async Task<IActionResult> LeaveRequisitionDetailLoad([FromServices] ILeaveRequisition db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadLeaveRequisition(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Leave Requisition", Operation = "CanPost")]
        public async Task<string> LeaveRequisitionDetailPost([FromServices] ILeaveRequisition db, [FromBody] tbl_WPT_LeaveRequisition tbl_WPT_LeaveRequisition, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostLeaveRequisition(tbl_WPT_LeaveRequisition, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Leave Requisition", Operation = "CanView")]
        public async Task<IActionResult> LeaveRequisitionDetailGet([FromServices] ILeaveRequisition db, int ID)
        {
            return Json(await db.GetLeaveRequisition(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion
    }
}
