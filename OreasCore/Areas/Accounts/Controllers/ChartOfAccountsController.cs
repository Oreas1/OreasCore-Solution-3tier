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

namespace OreasCore.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class ChartOfAccountsController : Controller
    {
        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetCOAListAsync([FromServices] IAccountsList IList, string QueryName = "", string COAFilterBy = "", string COAFilterValue = "", int FormID = 0)
        {
            return Json(
                await IList.GetCOAListAsync(QueryName, COAFilterBy, COAFilterValue, FormID)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Chart of accounts

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedChartOfAccountsAsync([FromServices] IAuthorizationScheme db, [FromServices] IChartOfAccounts db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ChartOfAccountsIndexCtlr",
                        WildCard = db2.GetWCLChartOfAccounts(),
                        Reports = db2.GetRLChartOfAccounts(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "ChartOfAccounts"),
                        Otherdata = new { 
                            COATypeList = await db3.GetCOATypeListAsync(null,null),
                            PolicyWHTaxOnPurchaseList = await db3.GetPolicyWHTaxOnPurchaseListAsync(null, null),
                            PolicyWHTaxOnSalesList = await db3.GetPolicyWHTaxOnSalesListAsync(null, null),
                            PolicyPaymentTermList = await db3.GetPolicyPaymentTermListAsync(null, null)
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "ChartOfAccounts", Operation = "CanView")]
        public IActionResult ChartOfAccountsIndex()
        {

            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "ChartOfAccounts", Operation = "CanView")]
        public async Task<IActionResult> ChartOfAccountsLoad([FromServices] IChartOfAccounts db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "ChartOfAccounts", Operation = "CanPost")]
        public async Task<string> ChartOfAccountsPost([FromServices] IChartOfAccounts db, [FromBody] tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Ac_ChartOfAccounts, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "ChartOfAccounts", Operation = "CanPost")]
        public async Task<string> ChartOfAccountsUploadExcelFile([FromServices] IChartOfAccounts db, int MasterID, IFormFile COAExcelFile, string operation = "")
        {
            if (COAExcelFile.Length > 0 && Path.GetExtension(COAExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await COAExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<COAExcelData> COAExcelDataList = new List<COAExcelData>();
                    try
                    {
                        await Task.Factory.StartNew(() =>
                        {            
                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                COAExcelDataList.Add(new COAExcelData()
                                {
                                    ParentID = MasterID,
                                    AccountName = worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString(),
                                    AcTypeID = worksheet.Cells[rowNo, 2].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 2].Value.ToString(), out int a0) ? Convert.ToInt32(worksheet.Cells[rowNo, 2].Value) : 0,
                                    WHTID = worksheet.Cells[rowNo, 3].Value == null ? null :
                                           int.TryParse(worksheet.Cells[rowNo, 3].Value.ToString(), out int a1) ? Convert.ToInt32(worksheet.Cells[rowNo, 3].Value) : null,
                                    WHTSalesID = worksheet.Cells[rowNo, 4].Value == null ? null :
                                           int.TryParse(worksheet.Cells[rowNo, 4].Value.ToString(), out int a2) ? Convert.ToInt32(worksheet.Cells[rowNo, 4].Value) : null,
                                    PayTermID = worksheet.Cells[rowNo, 5].Value == null ? null :
                                           int.TryParse(worksheet.Cells[rowNo, 5].Value.ToString(), out int a3) ? Convert.ToInt32(worksheet.Cells[rowNo, 5].Value) : null,
                                    CompanyName = worksheet.Cells[rowNo, 6].Value == null ? "" : worksheet.Cells[rowNo, 6].Value.ToString(),
                                    Address = worksheet.Cells[rowNo, 7].Value == null ? "" : worksheet.Cells[rowNo, 7].Value.ToString(),
                                    NTN = worksheet.Cells[rowNo, 8].Value == null ? "" : worksheet.Cells[rowNo, 8].Value.ToString(),
                                    STR = worksheet.Cells[rowNo, 9].Value == null ? "" : worksheet.Cells[rowNo, 9].Value.ToString(),
                                    Telephone = worksheet.Cells[rowNo, 10].Value == null ? "" : worksheet.Cells[rowNo, 10].Value.ToString(),
                                    Mobile = worksheet.Cells[rowNo, 11].Value == null ? "" : worksheet.Cells[rowNo, 11].Value.ToString(),
                                    Email = worksheet.Cells[rowNo, 12].Value == null ? "" : worksheet.Cells[rowNo, 12].Value.ToString(),
                                    ContactPersonName = worksheet.Cells[rowNo, 13].Value == null ? "" : worksheet.Cells[rowNo, 13].Value.ToString()
                                });
                            }
                        });

                        await db.COAUploadExcelFile(COAExcelDataList, operation, User.Identity.Name);

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


        [MyAuthorization(FormName = "ChartOfAccounts", Operation = "CanView")]
        public async Task<IActionResult> ChartOfAccountsGet([FromServices] IChartOfAccounts db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpGet]
        [MyAuthorization(FormName = "ChartOfAccounts", Operation = "CanView")]
        public async Task<IActionResult> GetNodes([FromServices] IChartOfAccounts db, int PID)
        {
            return Json(await db.GetNodesAsync(PID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "ChartOfAccounts", Operation = "CanViewReport")]
        public async Task<IActionResult> GetChartOfAccountsReport([FromServices] IChartOfAccounts db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID,User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion


        #region CustomerApprovedRateList

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCustomerRateListAsync([FromServices] IAuthorizationScheme db, [FromServices] ICustomerApprovedRateList db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CustomerRateListCtlr",
                        WildCard = db2.GetWCLCustomerRateListMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Customer Approved Rate List"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CustomerRateListDetailCtlr",
                        WildCard = db2.GetWCLCustomerRateListDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Customer Approved Rate List", Operation = "CanView")]
        public IActionResult CustomerRateListIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Customer Approved Rate List", Operation = "CanView")]
        public async Task<IActionResult> CustomerRateListLoad([FromServices] ICustomerApprovedRateList db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCustomerRateListMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Customer Approved Rate List", Operation = "CanView")]
        public async Task<IActionResult> CustomerRateListDetailLoad([FromServices] ICustomerApprovedRateList db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCustomerRateListDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Customer Approved Rate List", Operation = "CanPost")]
        public async Task<string> CustomerRateListDetailPost([FromServices] ICustomerApprovedRateList db, [FromBody] tbl_Ac_CustomerApprovedRateList tbl_Ac_CustomerApprovedRateList, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCustomerRateListDetail(tbl_Ac_CustomerApprovedRateList, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Customer Approved Rate List", Operation = "CanView")]
        public async Task<IActionResult> CustomerRateListDetailGet([FromServices] ICustomerApprovedRateList db, int ID)
        {
            return Json(await db.GetCustomerRateListDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

    }
}
