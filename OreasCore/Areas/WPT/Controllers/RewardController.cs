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
    public class RewardController : Controller
    {
        #region Reward

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedRewardAsync([FromServices] IAuthorizationScheme db, [FromServices] IReward db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RewardIndexCtlr",
                        WildCard = db2.GetWCLRewardMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Reward"),
                        Otherdata = new{ 
                            RewardTypeList = await IList.GetRewardTypeListAsync(null,null),
                            DesignationList = await IList.GetDesignationListAsync(null,null),
                            DepartmentList = await IList.GetDepartmentListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RewardDetailEmployeeCtlr",
                        WildCard = db2.GetWCLRewardDetailEmployee(),
                        Reports = db2.GetRLReward(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RewardDetailPaymentCtlr",
                        WildCard = db2.GetWCLRewardDetailPayment(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new{
                            CompanyBankAcList = await IList.GetCompanyBankAcListAsync(null,null),
                            TransactionModeList = await IList.GetTransactionModeListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RewardDetailPaymentEmployeeCtlr",
                        WildCard = db2.GetWCLRewardDetailPaymentEmployee(),
                        Reports = db2.GetRLRewardPayment(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Reward master

        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public IActionResult RewardIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public async Task<IActionResult> RewardMasterLoad([FromServices] IReward db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRewardMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Reward", Operation = "CanPost")]
        public async Task<string> RewardMasterPost([FromServices] IReward db, [FromBody] tbl_WPT_RewardMaster tbl_WPT_RewardMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRewardMaster(tbl_WPT_RewardMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public async Task<IActionResult> RewardMasterGet([FromServices] IReward db, int ID)
        {
            return Json(await db.GetRewardMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Reward Detail Employee


        [AjaxOnly]
        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public async Task<IActionResult> RewardDetailEmployeeLoad([FromServices] IReward db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRewardDetailEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Reward", Operation = "CanPost")]
        public async Task<string> RewardDetailEmployeePost([FromServices] IReward db, [FromBody] tbl_WPT_RewardDetail tbl_WPT_RewardDetail, string operation = "", int? MasterID = 0, int? DesignationID = 0, int? DepartmentID = 0, DateTime? JoiningDate = null)
        {
            if (ModelState.IsValid)
                return await db.PostRewardDetailEmployee(tbl_WPT_RewardDetail, operation, User.Identity.Name, MasterID, DesignationID, DepartmentID, JoiningDate);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public async Task<IActionResult> RewardDetailEmployeeGet([FromServices] IReward db, int ID)
        {
            return Json(await db.GetRewardDetailEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Reward", Operation = "CanPost")]
        public async Task<string> RewardDetailEmployeeUploadExcelFile([FromServices] IReward db, int MasterID, IFormFile RewardExcelFile, string operation = "")
        {
            if (RewardExcelFile.Length > 0 && Path.GetExtension(RewardExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await RewardExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<RewardExcelData> RewardExcelDataList = new List<RewardExcelData>();

                    try
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                RewardExcelDataList.Add(new RewardExcelData()
                                {
                                    ATNo = worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString(),
                                    RewardAmount = worksheet.Cells[rowNo, 2].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 2].Value.ToString(), out double a1) ? Convert.ToDouble(worksheet.Cells[rowNo, 2].Value) : 0,
                                    withSalary = worksheet.Cells[rowNo, 3].Value == null ? false :
                                                 worksheet.Cells[rowNo, 3].Value.ToString().ToLower() == "yes" ? true : false
                                });
                            }
                        });

                        await db.RewardDetailEmployeeUploadExcelFile(RewardExcelDataList, operation, User.Identity.Name, MasterID);

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

        #region Reward Detail Payment


        [AjaxOnly]
        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public async Task<IActionResult> RewardDetailPaymentLoad([FromServices] IReward db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRewardDetailPayment(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Reward", Operation = "CanPost")]
        public async Task<string> RewardDetailPaymentPost([FromServices] IReward db, [FromBody] tbl_WPT_RewardDetail_Payment tbl_WPT_RewardDetail_Payment, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRewardDetailPayment(tbl_WPT_RewardDetail_Payment, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public async Task<IActionResult> RewardDetailPaymentGet([FromServices] IReward db, int ID)
        {
            return Json(await db.GetRewardDetailPayment(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Reward Detail Payment Employee


        [AjaxOnly]
        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public async Task<IActionResult> RewardDetailPaymentEmployeeLoad([FromServices] IReward db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRewardDetailPaymentEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Reward", Operation = "CanPost")]
        public async Task<string> RewardDetailPaymentEmployeePost([FromServices] IReward db, string operation = "", int tbl_WPT_RewardDetailID = 0, int RewardPaymentID = 0, int DepartmentID = 0, int DesignationID = 0)
        {
            if (ModelState.IsValid)
                return await db.PostRewardDetailPaymentEmployee(operation, User.Identity.Name, tbl_WPT_RewardDetailID, RewardPaymentID, DepartmentID, DesignationID);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Reward", Operation = "CanView")]
        public async Task<IActionResult> RewardDetailPaymentEmployeeGet([FromServices] IReward db, int ID)
        {
            return Json(await db.GetRewardDetailPaymentEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion



        #endregion

        [MyAuthorization(FormName = "Reward", Operation = "CanViewReport")]
        public async Task<IActionResult> GetReport([FromServices] IReward db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), "application/pdf");
        }

    }
}
