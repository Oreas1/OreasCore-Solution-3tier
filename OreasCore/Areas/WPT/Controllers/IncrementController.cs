using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OreasCore.Areas.WPT.Controllers
{
    [Area("WPT")]
    public class IncrementController : Controller
    {
        #region Increment

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedIncrementAsync([FromServices] IAuthorizationScheme db, [FromServices] IIncrement db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "IncrementIndexCtlr",
                        WildCard = db2.GetWCLIncrementCalendar(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Increment"),
                        Otherdata = new{
                            DesignationList = await IList.GetDesignationListAsync(null,null),
                            DepartmentList = await IList.GetDepartmentListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "IncrementMasterCtlr",
                        WildCard = db2.GetWCLIncrementMaster(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "IncrementDetailEmployeeCtlr",
                        WildCard = db2.GetWCLIncrementDetailEmployee(),
                        Reports = db2.GetRLIncrement(),
                        Privilege = null,
                        Otherdata = new {
                            IncrementByList = await IList.GetIncrementByList()
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Increment Calendar

        [MyAuthorization(FormName = "Increment", Operation = "CanView")]
        public IActionResult IncrementIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Increment", Operation = "CanView")]
        public async Task<IActionResult> IncrementCalendarLoad([FromServices] IIncrement db,
           int CurrentPage = 1, int MasterID = 0,
           string FilterByText = null, string FilterValueByText = null,
           string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
           string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
           string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadIncrementCalendar(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Increment master

        [AjaxOnly]
        [MyAuthorization(FormName = "Increment", Operation = "CanView")]
        public async Task<IActionResult> IncrementMasterLoad([FromServices] IIncrement db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadIncrementMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Increment", Operation = "CanPost")]
        public async Task<string> IncrementMasterPost([FromServices] IIncrement db, [FromBody] tbl_WPT_IncrementMaster tbl_WPT_IncrementMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostIncrementMaster(tbl_WPT_IncrementMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        [MyAuthorization(FormName = "Increment", Operation = "CanView")]
        public async Task<IActionResult> IncrementMasterGet([FromServices] IIncrement db, int ID)
        {
            return Json(await db.GetIncrementMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Increment Detail Employee


        [AjaxOnly]
        [MyAuthorization(FormName = "Increment", Operation = "CanView")]
        public async Task<IActionResult> IncrementDetailEmployeeLoad([FromServices] IIncrement db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadIncrementDetailEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Increment", Operation = "CanPost")]
        public async Task<string> IncrementDetailEmployeePost([FromServices] IIncrement db, [FromBody] tbl_WPT_IncrementDetail tbl_WPT_IncrementDetail, string operation = "", int? MasterID = 0, int? DesignationID = 0, int? DepartmentID = 0, DateTime? JoiningDate = null)
        {
            if (ModelState.IsValid)
                return await db.PostIncrementDetailEmployee(tbl_WPT_IncrementDetail, operation, User.Identity.Name, MasterID, DesignationID, DepartmentID, JoiningDate);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Increment", Operation = "CanView")]
        public async Task<IActionResult> IncrementDetailEmployeeGet([FromServices] IIncrement db, int ID)
        {
            return Json(await db.GetIncrementDetailEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Increment", Operation = "CanPost")]
        public async Task<string> IncrementDetailEmployeeUploadExcelFile([FromServices] IIncrement db, int MasterID, IFormFile IncrementExcelFile, string operation = "")
        {
            if (IncrementExcelFile.Length > 0 && Path.GetExtension(IncrementExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await IncrementExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<IncrementExcelData> IncrementExcelDataList = new List<IncrementExcelData>();

                    try
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            DateTime EffectiveDateExcel;

                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {

                                EffectiveDateExcel = worksheet.Cells[rowNo, 2].Value == null ? DateTime.Now :
                                             DateTime.TryParse(worksheet.Cells[rowNo, 2].Value.ToString(), out DateTime EffectiveDateExcel1) ? Convert.ToDateTime(worksheet.Cells[rowNo, 2].Value) : DateTime.Now;


                                IncrementExcelDataList.Add(new IncrementExcelData()
                                {
                                    ATNo = worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString(),
                                    EffectiveFrom = EffectiveDateExcel,
                                    IncAmount = worksheet.Cells[rowNo, 3].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 3].Value.ToString(), out double a0) ? Convert.ToDouble(worksheet.Cells[rowNo, 3].Value) : 0,
                                    IncByCode = worksheet.Cells[rowNo, 4].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 4].Value.ToString(), out int a1) ? Convert.ToInt32(worksheet.Cells[rowNo, 4].Value) : 0,
                                    Arrear = worksheet.Cells[rowNo, 5].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 5].Value.ToString(), out double a2) ? Convert.ToDouble(worksheet.Cells[rowNo, 5].Value) : 0,
                                    ArrearMonthCode = worksheet.Cells[rowNo, 6].Value != null ? Convert.ToInt32(worksheet.Cells[rowNo, 6].Value) : (int?)null
                                });
                            }
                        });

                        await db.IncrementDetailEmployeeUploadExcelFile(IncrementExcelDataList, operation, User.Identity.Name, MasterID);

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


        [MyAuthorization(FormName = "Increment", Operation = "CanViewReport")]
        public async Task<IActionResult> GetReport([FromServices] IIncrement db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), "application/pdf");
        }
    }
}
