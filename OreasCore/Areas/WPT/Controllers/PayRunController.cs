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
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.IO;

namespace OreasCore.Areas.WPT.Controllers
{
    [Area("WPT")]
    public class PayRunController : Controller
    {
        #region PayRun

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPayRunAsync([FromServices] IAuthorizationScheme db, [FromServices] IPayRun db2, [FromServices] ILeaveRequisition db3, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunIndexCtlr",
                        WildCard = db2.GetWCLPayRunCalendar(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "PayRun"),
                        Otherdata = new {
                            TransactionModeList = await IList.GetTransactionModeListAsync(null,null),
                            DesignationList = await IList.GetDesignationListAsync(null,null),
                            DepartmentList = await IList.GetDepartmentListAsync(null,null),
                            HolidayList = await IList.GetHolidayListAsync(null,null),
                            ShiftList = await IList.GetShiftListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunToDoCtlr",
                        WildCard = db2.GetWCLPayRunToDo(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunExemptCtlr",
                        WildCard = db2.GetWCLPayRunExempt(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            DeductibleTypeList = await IList.GetDeductibleTypeListAsync(null,null),
                            LoanTypeList = await IList.GetLoanTypeListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunExemptEmployeeCtlr",
                        WildCard = db2.GetWCLPayRunExemptEmployee(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunHolidayCtlr",
                        WildCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunLeaveRequisitionCtlr",
                        WildCard = db3.GetWCLLeaveRequisition(),
                        LoadByCard = db3.GetWCLBLeaveRequisition(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { ActionList = await IList.GetActionList() }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunMasterDetailEmployeeCtlr",
                        WildCard = db2.GetWCLPayRunMasterDetailEmployee(),
                        Reports = db2.GetRLPayRunDetail(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunMasterDetailPaymentCtlr",
                        WildCard = db2.GetWCLPayRunDetailPayment(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new{
                            CompanyBankAcList = await IList.GetCompanyBankAcListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunMasterDetailPaymentEmployeeCtlr",
                        WildCard = db2.GetWCLPayRunDetailPaymentEmployee(),
                        Reports = db2.GetRLPayRunPayment(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunRosterMasterCtlr",
                        WildCard = db2.GetWCLPayRunRosterMaster(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PayRunRosterDetailEmployeeCtlr",
                        WildCard = db2.GetWCLPayRunRosterDetailEmployee(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }

                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region PayRun Calendar

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public IActionResult PayRunIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunCalendarLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadPayRunCalendar(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunMonthOpenClose([FromServices] IPayRun db, int MonthID = 0, bool MonthIsClosed=false)
        {
            if (ModelState.IsValid)
                return await db.PostPayRunCalendarOpenClose(MonthID,MonthIsClosed, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #endregion

        #region PayRun ToDo

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunToDoLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunToDo(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunToDoPost([FromServices] IPayRun db, [FromBody] tbl_WPT_PayRunToDo tbl_WPT_PayRunToDo, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPayRunToDo(tbl_WPT_PayRunToDo, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunToDoGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunToDo(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region PayRun Exemption

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunExemptLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunExempt(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunExemptPost([FromServices] IPayRun db, [FromBody] tbl_WPT_PayRunExemption tbl_WPT_PayRunExemption, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPayRunExempt(tbl_WPT_PayRunExemption, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunExemptGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunExempt(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region PayRun Exemption Employee

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunExemptEmployeeLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunExemptEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunExemptEmployeePost([FromServices] IPayRun db, [FromBody] tbl_WPT_PayRunExemption_Emp tbl_WPT_PayRunExemption_Emp, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPayRunExemptEmployee(tbl_WPT_PayRunExemption_Emp, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunExemptEmployeeGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunExemptEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region PayRun Holiday

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunHolidayLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunHoliday(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunHolidayGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunHoliday(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunHolidayPost([FromServices] IPayRun db, [FromBody] tbl_WPT_CalendarYear_Months_Holidays tbl_WPT_CalendarYear_Months_Holidays, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPayRunHoliday(tbl_WPT_CalendarYear_Months_Holidays, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        #endregion

        #region PayRun Leave Requisition

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunLeaveRequisitionLoad([FromServices] ILeaveRequisition db, [FromServices] OreasDbContext context,
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
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunLeaveRequisitionPost([FromServices] ILeaveRequisition db, [FromBody] tbl_WPT_LeaveRequisition tbl_WPT_LeaveRequisition, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostLeaveRequisition(tbl_WPT_LeaveRequisition, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunLeaveRequisitionGet([FromServices] ILeaveRequisition db, int ID)
        {
            return Json(await db.GetLeaveRequisition(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PayRun Roster Master

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunRosterMasterLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunRosterMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunRosterMasterPost([FromServices] IPayRun db, [FromBody] tbl_WPT_ShiftRosterMaster tbl_WPT_ShiftRosterMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPayRunRosterMaster(tbl_WPT_ShiftRosterMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunRosterMasterGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunRosterMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region PayRun Roster Detail Shift

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunRosterDetailShiftLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunRosterDetailShift(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunRosterDetailShiftPost([FromServices] IPayRun db, [FromBody] tbl_WPT_ShiftRosterDetail tbl_WPT_ShiftRosterDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPayRunRosterDetailShift(tbl_WPT_ShiftRosterDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunRosterDetailShiftGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunRosterDetailShift(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region PayRun Roster Detail Employee

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunRosterDetailEmployeeLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunRosterDetailEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunRosterDetailEmployeePost([FromServices] IPayRun db, [FromBody] tbl_WPT_ShiftRosterDetail_Employee tbl_WPT_ShiftRosterDetail_Employee, string operation = "", int? MasterID = 0, int? DesignationID = 0, int? DepartmentID = 0, DateTime? JoiningDate = null)
        {
            if (ModelState.IsValid)
                return await db.PostPayRunRosterDetailEmployee(tbl_WPT_ShiftRosterDetail_Employee, operation, User.Identity.Name, MasterID, DesignationID, DepartmentID, JoiningDate);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunRosterDetailEmployeeGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunRosterDetailEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region PayRun Master

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunMasterLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunMasterPost([FromServices] IPayRun db, [FromBody] tbl_WPT_PayRunMaster tbl_WPT_PayRunMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPayRunMaster(tbl_WPT_PayRunMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunMasterGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region PayRun Detail Employee

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunDetailEmployeeLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunMasterDetailEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunDetailEmployeePost([FromServices] IPayRun db, [FromBody] tbl_WPT_PayRunDetail_Emp tbl_WPT_PayRunDetail_Emp, string operation = "")
        {
            var IsInProcess = PayRunProcessStatus.PayRunProcessStatusList.Where(a => a.PayRunID == tbl_WPT_PayRunDetail_Emp.FK_tbl_WPT_PayRunMaster_ID).FirstOrDefault();

            if (IsInProcess != null)
            {                
                return "Cannot Insert employee individually until, Bulk Process get finish";
            }

            if (ModelState.IsValid)
                return await db.PostPayRunMasterDetailEmployee(tbl_WPT_PayRunDetail_Emp, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunDetailEmployeeGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunMasterDetailEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunProcessDetailEmployeePost([FromServices] IPayRun db, string operation = "", int ID = 0, bool PayRun = false, int MasterID = 0)
        {
           
            var IsInProcess = PayRunProcessStatus.PayRunProcessStatusList.Where(a => a.PayRunID == MasterID).FirstOrDefault();

            if (IsInProcess != null)
            {
                return "Cannot start payrun process for individual employee until, Bulk Process get finish";
            }
            if (ModelState.IsValid)
                return await db.PostPayRunProcessMasterDetailEmployee(ID, PayRun, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #endregion

        #region PayRun Detail Employee AT

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunDetailEmployeeATLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunMasterDetailEmployeeAT(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PayRun Detail Employee Wage

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunDetailEmployeeWageLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunMasterDetailEmployeeWage(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PayRun Detail Payment

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunDetailPaymentLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunDetailPayment(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunDetailPaymentPost([FromServices] IPayRun db, [FromBody] tbl_WPT_PayRunDetail_Payment tbl_WPT_PayRunDetail_Payment, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPayRunDetailPayment(tbl_WPT_PayRunDetail_Payment, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunDetailPaymentGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunDetailPayment(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PayRun Detail Employee Wage

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunDetailPaymentEmployeeLoad([FromServices] IPayRun db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPayRunDetailPaymentEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PayRun", Operation = "CanPost")]
        public async Task<string> PayRunDetailPaymentEmployeePost([FromServices] IPayRun db, string operation = "", int tbl_WPT_PayRunDetail_EmpID = 0, int PayRunPaymentID = 0, int DepartmentID = 0, int DesignationID = 0)
        {
            if (ModelState.IsValid)
                return await db.PostPayRunDetailPaymentEmployee(operation, User.Identity.Name, tbl_WPT_PayRunDetail_EmpID, PayRunPaymentID, DepartmentID, DesignationID);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<IActionResult> PayRunDetailPaymentEmployeeGet([FromServices] IPayRun db, int ID)
        {
            return Json(await db.GetPayRunDetailPaymentEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        [AjaxOnly]
        [MyAuthorization(FormName = "PayRun", Operation = "CanView")]
        public async Task<string> EmailPaySlip([FromServices] IPayRun IpayRun, int ID=0, string EmpName="", string EmpEmail="", DateTime MonthEnd= default(DateTime), bool Unknown = true)
        {
            
           return await Task.Run(async () => {

               try
               {
                   if (string.IsNullOrEmpty(Rpt_Shared.LicenseToEmail) || string.IsNullOrEmpty(Rpt_Shared.LicenseToEmailPswd) || string.IsNullOrEmpty(Rpt_Shared.LicenseToEmailHostName) || Rpt_Shared.LicenseToEmailPortNo <= 0 ||
                        string.IsNullOrEmpty(EmpName) || string.IsNullOrEmpty(EmpEmail)
                   )
                       return "Unable to send mail: Email Not Configured";

                   var message = new MimeMessage();
                   if(Unknown)
                       message.From.Add(new MailboxAddress("Company..", Rpt_Shared.LicenseToEmail));
                   else
                       message.From.Add(new MailboxAddress(Rpt_Shared.LicenseTo, Rpt_Shared.LicenseToEmail));

                   message.To.Add(new MailboxAddress(EmpName, EmpEmail));

                   message.Subject = "Pay Slip of " + MonthEnd.ToString("MMMM-yyyy");

                   var builder = new BodyBuilder();

                   if (!Unknown)
                   {
                       builder.HtmlBody = "<b>Dear " + EmpName + "</b><br>" + "Your salary has been generated in the system for the period of "
                          + MonthEnd.ToString("MMMM-yyyy") + "."
                          + "<br>" + "Please find attachment of Payslip" +
                          "<br>" + "Note: This is system generated email doesnot required signature." +
                          "<hr>" + Rpt_Shared.LicenseToEmailFooter.Replace("@whatsapp", "This is " + EmpName + " ");

                       builder.Attachments.Add("PaySlip", new MemoryStream(await IpayRun.GetPDFFilePayRunDetailAsync("PayRun Salary Slip Individual", ID, 0, 0, DateTime.Now, DateTime.Now, "", "", "", "", 0)).ToArray(), new ContentType("application", "pdf"));

                   }
                   else
                   {
                       builder.HtmlBody = "<b>Dear " + EmpName + "</b><br>" + "Your salary has been generated in the system for the period of "
                          + MonthEnd.ToString("MMMM-yyyy") + "."
                          + "<br>" + "Please find attachment of Payslip";

                       builder.Attachments.Add("PaySlip", new MemoryStream(await IpayRun.GetPDFFilePayRunDetailAsync("PayRun Salary Slip Individual Unknown", ID, 0, 0, DateTime.Now, DateTime.Now, "", "", "", "", 0)).ToArray(), new ContentType("application", "pdf"));
                   
                   }


                   message.Body = builder.ToMessageBody();

                   using (var client = new SmtpClient())
                   {
                       await client.ConnectAsync(Rpt_Shared.LicenseToEmailHostName, Rpt_Shared.LicenseToEmailPortNo, SecureSocketOptions.Auto);
                       await client.AuthenticateAsync(Rpt_Shared.LicenseToEmail, Rpt_Shared.LicenseToEmailPswd);

                       await client.SendAsync(message);
                       await client.DisconnectAsync(true);

                   }
                   return "OK";
               }
               catch (Exception ex)
               {
                   return "Some thing went wrong while sending email! Contact System Administrator";
               }      
                
            });

          
        }

        [MyAuthorization(FormName = "PayRun", Operation = "CanViewReport")]
        public async Task<IActionResult> GetReport([FromServices] IPayRun db, string _for="", string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            if (_for == "Emp")
                return File(await db.GetPDFFilePayRunDetailAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), "application/pdf");
            else if (_for == "Pay")
                return File(await db.GetPDFFilePayRunPaymentAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
            else
                return RedirectToAction("Error","Home", new { area = "", msg="Wrong Parameter" });
        }

        #endregion

    }
}
