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
    public class LoanController : Controller
    {
        #region Loan

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedLoanAsync([FromServices] IAuthorizationScheme db, [FromServices] ILoan db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LoanIndexCtlr",
                        WildCard = db2.GetWCLLoanMaster(),
                        LoadByCard = db2.GetWCLBLoanMaster(),
                        Reports = db2.GetRLLoan(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Loan"),
                        Otherdata = new{
                            LoanTypeList = await IList.GetLoanTypeListAsync(null,null),
                            DesignationList = await IList.GetDesignationListAsync(null,null),
                            DepartmentList = await IList.GetDepartmentListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LoanDetailEmployeeCtlr",
                        WildCard = db2.GetWCLLoanDetailEmployee(),
                        Reports = db2.GetRLLoanDetail(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LoanDetailPaymentCtlr",
                        WildCard = db2.GetWCLLoanDetailPayment(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new{
                            CompanyBankAcList = await IList.GetCompanyBankAcListAsync(null,null),
                            TransactionModeList = await IList.GetTransactionModeListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LoanDetailPaymentEmployeeCtlr",
                        WildCard = db2.GetWCLLoanDetailPaymentEmployee(),
                        Reports = db2.GetRLLoanPayment(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );;
        }

        #region Loan master

        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public IActionResult LoanIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanMasterLoad([FromServices] ILoan db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadLoanMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Loan", Operation = "CanPost")]
        public async Task<string> LoanMasterPost([FromServices] ILoan db, [FromBody] tbl_WPT_LoanMaster tbl_WPT_LoanMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostLoanMaster(tbl_WPT_LoanMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanMasterGet([FromServices] ILoan db, int ID)
        {
            return Json(await db.GetLoanMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Loan Detail Employee


        [AjaxOnly]
        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanDetailEmployeeLoad([FromServices] ILoan db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadLoanDetailEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Loan", Operation = "CanPost")]
        public async Task<string> LoanDetailEmployeePost([FromServices] ILoan db, [FromBody] tbl_WPT_LoanDetail tbl_WPT_LoanDetail, string operation = "", int? MasterID = 0, int? DesignationID = 0, int? DepartmentID = 0, DateTime? JoiningDate = null)
        {
            if (ModelState.IsValid)
                return await db.PostLoanDetailEmployee(tbl_WPT_LoanDetail, operation, User.Identity.Name, MasterID, DesignationID, DepartmentID, JoiningDate);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanDetailEmployeeGet([FromServices] ILoan db, int ID)
        {
            return Json(await db.GetLoanDetailEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Loan", Operation = "CanPost")]
        public async Task<string> LoanDetailEmployeeUploadExcelFile([FromServices] ILoan db, int MasterID, IFormFile LoanExcelFile, string operation = "")
        {
            if (LoanExcelFile.Length > 0 && Path.GetExtension(LoanExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await LoanExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<LoanExcelData> LoanExcelDataList = new List<LoanExcelData>();

                    try
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            DateTime EffectiveDateExcel;
                            DateTime ReceivingDateExcel;

                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                EffectiveDateExcel = worksheet.Cells[rowNo, 4].Value == null ? DateTime.Now :
                                            DateTime.TryParse(worksheet.Cells[rowNo, 4].Value.ToString(), out DateTime EffectiveDateExcel2) ? Convert.ToDateTime(worksheet.Cells[rowNo, 4].Value) : DateTime.Now;

                                ReceivingDateExcel = worksheet.Cells[rowNo, 5].Value == null ? DateTime.Now :
                                            DateTime.TryParse(worksheet.Cells[rowNo, 5].Value.ToString(), out DateTime ReceivingDateExcel2) ? Convert.ToDateTime(worksheet.Cells[rowNo, 5].Value) : DateTime.Now;
                    

                                LoanExcelDataList.Add(new LoanExcelData()
                                {
                                    ATNo = worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString(),
                                    Amount = worksheet.Cells[rowNo, 2].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 2].Value.ToString(), out double a0) ? Convert.ToDouble(worksheet.Cells[rowNo, 2].Value) : 0,
                                    Rate = worksheet.Cells[rowNo, 3].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 3].Value.ToString(), out double a1) ? Convert.ToDouble(worksheet.Cells[rowNo, 3].Value) : 0,
                                    EffectiveFrom = EffectiveDateExcel,
                                    ReceivingDate = ReceivingDateExcel
                                });
                            }
                        });

                        await db.LoanDetailEmployeeUploadExcelFile(LoanExcelDataList, operation, User.Identity.Name, MasterID);

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

        #region Loan Detail Payment


        [AjaxOnly]
        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanDetailPaymentLoad([FromServices] ILoan db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadLoanDetailPayment(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Loan", Operation = "CanPost")]
        public async Task<string> LoanDetailPaymentPost([FromServices] ILoan db, [FromBody] tbl_WPT_LoanDetail_Payment tbl_WPT_LoanDetail_Payment, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostLoanDetailPayment(tbl_WPT_LoanDetail_Payment, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanDetailPaymentGet([FromServices] ILoan db, int ID)
        {
            return Json(await db.GetLoanDetailPayment(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Loan Detail Payment Employee


        [AjaxOnly]
        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanDetailPaymentEmployeeLoad([FromServices] ILoan db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadLoanDetailPaymentEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Loan", Operation = "CanPost")]
        public async Task<string> LoanDetailPaymentEmployeePost([FromServices] ILoan db, string operation = "", int tbl_WPT_LoanDetailID = 0, int LoanPaymentID = 0, int DepartmentID = 0, int DesignationID = 0)
        {
            if (ModelState.IsValid)
                return await db.PostLoanDetailPaymentEmployee(operation, User.Identity.Name, tbl_WPT_LoanDetailID, LoanPaymentID, DepartmentID, DesignationID);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanDetailPaymentEmployeeGet([FromServices] ILoan db, int ID)
        {
            return Json(await db.GetLoanDetailPaymentEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region Loan Individual

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedLoanIndividualAsync([FromServices] IAuthorizationScheme db, [FromServices] ILoan db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LoanIndividualIndexCtlr",
                        Reports = db2.GetRLLoanIndividual(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Loan"),
                        Otherdata = new{
                            LoanTypeList = await IList.GetLoanTypeListAsync(null,null)
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                ); ;
        }


        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public IActionResult LoanIndividualIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Loan", Operation = "CanView")]
        public async Task<IActionResult> LoanIndividualLoad([FromServices] ILoan db,
            int CurrentPage = 1, int EmployeeID = 0, DateTime DateTill = new DateTime(), int? LoanTypeID = 0)
        {
            PagedData<object> pageddata =
                await db.LoadLoanIndividual(CurrentPage, EmployeeID, DateTill, LoanTypeID);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        [MyAuthorization(FormName = "Loan", Operation = "CanViewReport")]
        public async Task<IActionResult> GetReport([FromServices] ILoan db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), "application/pdf");
        }

    }
}
