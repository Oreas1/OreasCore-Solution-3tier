using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OreasCore.Areas.WPT.Controllers
{
    [Area("WPT")]
    public class AttendanceController : Controller
    {

        #region AT Grace

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedATTimeGraceAsync([FromServices] IAuthorizationScheme db, [FromServices] IATTimeGrace db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ATTimeGraceIndexCtlr",
                        WildCard = db2.GetWCLATTimeGrace(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "ATTimeGrace"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ATTimeGraceEmployeeCtlr",
                        WildCard = db2.GetWCLATTimeGraceEmployee(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Master

        [MyAuthorization(FormName = "ATTimeGrace", Operation = "CanView")]
        public IActionResult ATTimeGraceIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "ATTimeGrace", Operation = "CanView")]
        public async Task<IActionResult> ATTimeGraceLoad([FromServices] IATTimeGrace db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadATTimeGrace(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "ATTimeGrace", Operation = "CanPost")]
        public async Task<string> ATTimeGracePost([FromServices] IATTimeGrace db, [FromBody] tbl_WPT_ATTimeGrace tbl_WPT_ATTimeGrace, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostATTimeGrace(tbl_WPT_ATTimeGrace, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "ATTimeGrace", Operation = "CanView")]
        public async Task<IActionResult> ATTimeGraceGet([FromServices] IATTimeGrace db, int ID)
        {
            return Json(await db.GetATTimeGrace(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Employee

        [AjaxOnly]
        [MyAuthorization(FormName = "ATTimeGrace", Operation = "CanView")]
        public async Task<IActionResult> ATTimeGraceEmployeeLoad([FromServices] IATTimeGrace db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadATTimeGraceEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "ATTimeGrace", Operation = "CanPost")]
        public async Task<string> ATTimeGraceEmployeePost([FromServices] IATTimeGrace db, [FromBody] tbl_WPT_ATTimeGraceEmployeeLink tbl_WPT_ATTimeGraceEmployeeLink, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostATTimeGraceEmployee(tbl_WPT_ATTimeGraceEmployeeLink, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "ATTimeGrace", Operation = "CanView")]
        public async Task<IActionResult> ATTimeGraceEmployeeGet([FromServices] IATTimeGrace db, int ID)
        {
            return Json(await db.GetATTimeGraceEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "ATTimeGrace", Operation = "CanPost")]
        public async Task<string> ATTimeGraceEmployeeUploadExcelFile([FromServices] IATTimeGrace db, int MasterID, IFormFile ATGraceExcelFile, string operation = "")
        {
            if (ATGraceExcelFile.Length > 0 && Path.GetExtension(ATGraceExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await ATGraceExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<string> ATGraceExcelDataList = new List<string>();

                    try
                    {

                        await Task.Factory.StartNew(() =>
                        {

                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                ATGraceExcelDataList.Add(worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString());
                            }
                        });

                        await db.ATTimeGraceEmployeeUploadExcelFile(ATGraceExcelDataList, operation, User.Identity.Name, MasterID);

                    }
                    catch (Exception ex)
                    {
                        return ex.InnerException.Message;
                    }
                }
            }
            else
            {
                return "File not Supported";
            }

            return "OK";
        }

        #endregion

        #endregion

        #region Attendance 

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedAttendanceAsync([FromServices] IAuthorizationScheme db, [FromServices] IAttendance db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AttendanceIndexCtlr",
                        WildCard = null,
                        Reports = db2.GetRLAttendance(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Attendance"),
                        Otherdata = new { 
                            ATInOutModeList=await IList.GetATInOutModeListAsync(null,null) ,
                            LastOpenMonth = await db2.GetLastOpenMonth()
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AttendanceTogetherCtlr",
                        WildCard = db2.GetWCLATTogether(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Attendance", Operation = "CanView")]
        public IActionResult AttendanceIndex()
        {

            return View();
        }
        #region Individual

        
        [AjaxOnly]
        [MyAuthorization(FormName = "Attendance", Operation = "CanView")]
        public async Task<IActionResult> AttendanceIndividualLoad([FromServices] IAttendance db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAttendanceIndividual(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Attendance", Operation = "CanPost")]
        public async Task<string> AttendanceIndividualPost([FromServices] IAttendance db, [FromBody] tbl_WPT_AttendanceLog tbl_WPT_AttendanceLog, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostAttendanceIndividual(tbl_WPT_AttendanceLog, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Attendance", Operation = "CanView")]
        public async Task<IActionResult> AttendanceIndividualGet([FromServices] IAttendance db, int ID)
        {
            return Json(await db.GetAttendanceIndividual(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Together
        [AjaxOnly]
        [MyAuthorization(FormName = "Attendance", Operation = "CanView")]
        public async Task<IActionResult> ATTogetherLoad([FromServices] IAttendance db,
            int CurrentPage = 1, DateTime? MonthEnd = null, DateTime? MonthStart = null, string FilterByText = null, string FilterValueByText = null)
        {
            PagedData<object> pageddata =
                await db.LoadATTogether(CurrentPage, MonthStart, MonthEnd, FilterByText, FilterValueByText);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        [MyAuthorization(FormName = "Attendance", Operation = "CanViewReport")]
        public async Task<IActionResult> GetReport([FromServices] IAttendance db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), "application/pdf");
        }


        #endregion

        #region ATBulkManual

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedATBulkManualAsync([FromServices] IAuthorizationScheme db, [FromServices] IATBulkManual db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ATBulkManualMasterCtlr",
                        WildCard = db2.GetWCLATBulkManualMaster(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "AT Bulk Manual"),
                        Otherdata = new {
                            ATInOutModeList= await IList.GetATInOutModeListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ATBulkManualDetailEmployeeCtlr",
                        WildCard = db2.GetWCLATBulkManualDetail(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "AT Bulk Manual", Operation = "CanView")]
        public IActionResult ATBulkManualIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "AT Bulk Manual", Operation = "CanView")]
        public async Task<IActionResult> ATBulkManualMasterLoad([FromServices] IATBulkManual db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadATBulkManualMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "AT Bulk Manual", Operation = "CanPost")]
        public async Task<string> ATBulkManualMasterPost([FromServices] IATBulkManual db, [FromBody] tbl_WPT_ATBulkManualMaster tbl_WPT_ATBulkManualMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostATBulkManualMaster(tbl_WPT_ATBulkManualMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "AT Bulk Manual", Operation = "CanView")]
        public async Task<IActionResult> ATBulkManualMasterGet([FromServices] IATBulkManual db, int ID)
        {
            return Json(await db.GetATBulkManualMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "AT Bulk Manual", Operation = "CanView")]
        public async Task<IActionResult> ATBulkManualDetailLoad([FromServices] IATBulkManual db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadATBulkManualDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "AT Bulk Manual", Operation = "CanPost")]
        public async Task<string> ATBulkManualDetailPost([FromServices] IATBulkManual db, [FromBody] tbl_WPT_ATBulkManualDetail_Employee tbl_WPT_ATBulkManualDetail_Employee, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostATBulkManualDetail(tbl_WPT_ATBulkManualDetail_Employee, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "AT Bulk Manual", Operation = "CanView")]
        public async Task<IActionResult> ATBulkManualDetailGet([FromServices] IATBulkManual db, int ID)
        {
            return Json(await db.GetATBulkManualDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "AT Bulk Manual", Operation = "CanPost")]
        public async Task<string> ATBulkManualDetailUploadExcelFile([FromServices] IATBulkManual db, int MasterID, IFormFile ELExcelFile, string operation = "")
        {
            if (ELExcelFile.Length > 0 && Path.GetExtension(ELExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await ELExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<string> ATGraceExcelDataList = new List<string>();

                    try
                    {

                        await Task.Factory.StartNew(() =>
                        {

                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                ATGraceExcelDataList.Add(worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString());
                            }
                        });

                        await db.ATBulkManualUploadExcelFile(ATGraceExcelDataList, MasterID, operation, User.Identity.Name);

                    }
                    catch (Exception ex)
                    {
                        return ex.InnerException.Message;
                    }
                }
            }
            else
            {
                return "File not Supported";
            }

            return "OK";
        }

        #endregion           

        #endregion

    }
}
