using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;

namespace OreasCore.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ChallanController : Controller
    {

        #region PurchaseNote

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseNote db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseNoteMasterCtlr",
                        WildCard = db2.GetWCLPurchaseNoteMaster(),
                        LoadByCard = db2.GetWCLBPurchaseNoteMaster(),
                        Reports = db2.GetRLPurchaseNoteMaster(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Purchase Note"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseNoteDetailCtlr",
                        WildCard = db2.GetWCLPurchaseNoteDetail(),
                        LoadByCard = db2.GetWCLBPurchaseNoteDetail(),
                        Reports = db2.GetRLPurchaseNoteDetail(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseNoteDetailOfDetailCtlr",
                        WildCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Note", Operation = "CanView")]
        public IActionResult PurchaseNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteMasterLoad([FromServices] IPurchaseNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseNoteMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Note", Operation = "CanPost")]
        public async Task<string> PurchaseNoteMasterPost([FromServices] IPurchaseNote db, [FromBody] tbl_Inv_PurchaseNoteMaster tbl_Inv_PurchaseNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseNoteMaster(tbl_Inv_PurchaseNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteMasterGet([FromServices] IPurchaseNote db, int ID)
        {
            return Json(await db.GetPurchaseNoteMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteDetailLoad([FromServices] IPurchaseNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseNoteDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Note", Operation = "CanPost")]
        public async Task<string> PurchaseNoteDetailPost([FromServices] IPurchaseNote db, [FromBody] tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseNoteDetail(tbl_Inv_PurchaseNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteDetailGet([FromServices] IPurchaseNote db, int ID)
        {
            return Json(await db.GetPurchaseNoteDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Note", Operation = "CanPost")]
        public async Task<string> PurchaseNoteDetailUploadExcelFile([FromServices] IPurchaseNote db, int MasterID, IFormFile PNDExcelFile, string operation = "")
        {
            string RespondMsg = "OK";
            if (PNDExcelFile.Length > 0 && Path.GetExtension(PNDExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await PNDExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<PurchaseNoteExcelData> PurchaseNoteExcelDataList = new List<PurchaseNoteExcelData>();

                    try
                    {

                        await Task.Factory.StartNew(() =>
                        {
                            DateTime? expiryDate;

                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                 expiryDate = worksheet.Cells[rowNo, 5].Value == null ? null :
                                             DateTime.TryParse(worksheet.Cells[rowNo, 5].Value.ToString(), out DateTime expiryDate1) ? Convert.ToDateTime(worksheet.Cells[rowNo, 5].Value) : null;


                                PurchaseNoteExcelDataList.Add(new PurchaseNoteExcelData()
                                {
                                    ProductCode = worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString(),
                                    PONo = worksheet.Cells[rowNo, 2].Value == null ? 0 : int.TryParse(worksheet.Cells[rowNo, 2].Value.ToString(), out int a0) ? a0 : 0,
                                    Quantity = worksheet.Cells[rowNo, 3].Value == null ? 0 : double.TryParse(worksheet.Cells[rowNo, 3].Value.ToString(), out double a1) ? a1 : 0,
                                    MfgBatchNo = worksheet.Cells[rowNo, 4].Value == null ? null : worksheet.Cells[rowNo, 4].Value.ToString(),
                                    ExpiryDate = expiryDate,
                                    Rate = worksheet.Cells[rowNo, 6].Value == null ? 0 : double.TryParse(worksheet.Cells[rowNo, 6].Value.ToString(), out double b1) ? b1 : 0,
                                    GSTPercentage = worksheet.Cells[rowNo, 7].Value == null ? 0 : double.TryParse(worksheet.Cells[rowNo, 7].Value.ToString(), out double b2) ? b2 : 0,
                                    WHTPercentage = worksheet.Cells[rowNo, 8].Value == null ? 0 : double.TryParse(worksheet.Cells[rowNo, 8].Value.ToString(), out double b3) ? b3 : 0
                                });
                            }
                        });

                        RespondMsg = await db.PurchaseNoteDetailUploadExcelFile(PurchaseNoteExcelDataList, MasterID, operation, User.Identity.Name);

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

            return RespondMsg;
        }


        #endregion

        #region DetailOfDetail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteDetailOfDetailLoad([FromServices] IPurchaseNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseNoteDetailOfDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Purchase Note", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPurchaseNoteReport([FromServices] IPurchaseNote db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region PurchaseReturnNote

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseReturnNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseReturnNote db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseReturnNoteMasterCtlr",
                        WildCard = db2.GetWCLPurchaseReturnNoteMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Purchase Return Note"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseReturnNoteDetailCtlr",
                        WildCard = db2.GetWCLPurchaseReturnNoteDetail(),
                        Reports = db2.GetRLPurchaseReturnNote(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Return Note", Operation = "CanView")]
        public IActionResult PurchaseReturnNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Return Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseReturnNoteMasterLoad([FromServices] IPurchaseReturnNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseReturnNoteMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Return Note", Operation = "CanPost")]
        public async Task<string> PurchaseReturnNoteMasterPost([FromServices] IPurchaseReturnNote db, [FromBody] tbl_Inv_PurchaseReturnNoteMaster tbl_Inv_PurchaseReturnNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseReturnNoteMaster(tbl_Inv_PurchaseReturnNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Return Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseReturnNoteMasterGet([FromServices] IPurchaseReturnNote db, int ID)
        {
            return Json(await db.GetPurchaseReturnNoteMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Return Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseReturnNoteDetailLoad([FromServices] IPurchaseReturnNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseReturnNoteDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Return Note", Operation = "CanPost")]
        public async Task<string> PurchaseReturnNoteDetailPost([FromServices] IPurchaseReturnNote db, [FromBody] tbl_Inv_PurchaseReturnNoteDetail tbl_Inv_PurchaseReturnNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseReturnNoteDetail(tbl_Inv_PurchaseReturnNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Return Note", Operation = "CanView")]
        public async Task<IActionResult> PurchaseReturnNoteDetailGet([FromServices] IPurchaseReturnNote db, int ID)
        {
            return Json(await db.GetPurchaseReturnNoteDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Purchase Return Note", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPurchaseReturnNoteReport([FromServices] IPurchaseReturnNote db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region SalesNote

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedSalesNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] ISalesNote db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesNoteMasterCtlr",
                        WildCard = db2.GetWCLSalesNoteMaster(),
                        Reports = db2.GetRLSalesNoteMaster(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Sales Note"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesNoteDetailCtlr",
                        WildCard = db2.GetWCLSalesNoteDetail(),
                        Reports = db2.GetRLSalesNoteDetail(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesNoteDetailReturnCtlr",
                        WildCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Sales Note", Operation = "CanView")]
        public IActionResult SalesNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Note", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteMasterLoad([FromServices] ISalesNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesNoteMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Sales Note", Operation = "CanPost")]
        public async Task<string> SalesNoteMasterPost([FromServices] ISalesNote db, [FromBody] tbl_Inv_SalesNoteMaster tbl_Inv_SalesNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSalesNoteMaster(tbl_Inv_SalesNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Sales Note", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteMasterGet([FromServices] ISalesNote db, int ID)
        {
            return Json(await db.GetSalesNoteMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Note", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteDetailLoad([FromServices] ISalesNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesNoteDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Sales Note", Operation = "CanPost")]
        public async Task<string> SalesNoteDetailPost([FromServices] ISalesNote db, [FromBody] tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSalesNoteDetail(tbl_Inv_SalesNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Sales Note", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteDetailGet([FromServices] ISalesNote db, int ID)
        {
            return Json(await db.GetSalesNoteDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Return

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Note", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteDetailReturnLoad([FromServices] ISalesNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesNoteDetailReturn(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Sales Note", Operation = "CanViewReport")]
        public async Task<IActionResult> GetSalesNoteReport([FromServices] ISalesNote db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region SalesReturnNote

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetSalesNoteForReturnAsync([FromServices] IInventoryList IList, int CustomerID = 0, int? PurchaseRefID = null, int? BMRRefID = null, string SalesNoteFilterBy = null, string SalesNoteFilterValue = null)
        {
            return Json(
                await IList.GetSalesNoteForReturnAsync(CustomerID, PurchaseRefID, BMRRefID, SalesNoteFilterBy, SalesNoteFilterValue)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedSalesReturnNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] ISalesReturnNote db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesReturnNoteMasterCtlr",
                        WildCard = db2.GetWCLSalesReturnNoteMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Sales Return Note"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesReturnNoteDetailCtlr",
                        WildCard = db2.GetWCLSalesReturnNoteDetail(),
                        Reports = db2.GetRLSalesReturnNote(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Sales Return Note", Operation = "CanView")]
        public IActionResult SalesReturnNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Return Note", Operation = "CanView")]
        public async Task<IActionResult> SalesReturnNoteMasterLoad([FromServices] ISalesReturnNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesReturnNoteMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Sales Return Note", Operation = "CanPost")]
        public async Task<string> SalesReturnNoteMasterPost([FromServices] ISalesReturnNote db, [FromBody] tbl_Inv_SalesReturnNoteMaster tbl_Inv_SalesReturnNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSalesReturnNoteMaster(tbl_Inv_SalesReturnNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Sales Return Note", Operation = "CanView")]
        public async Task<IActionResult> SalesReturnNoteMasterGet([FromServices] ISalesReturnNote db, int ID)
        {
            return Json(await db.GetSalesReturnNoteMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Return Note", Operation = "CanView")]
        public async Task<IActionResult> SalesReturnNoteDetailLoad([FromServices] ISalesReturnNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesReturnNoteDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Sales Return Note", Operation = "CanPost")]
        public async Task<string> SalesReturnNoteDetailPost([FromServices] ISalesReturnNote db, [FromBody] tbl_Inv_SalesReturnNoteDetail tbl_Inv_SalesReturnNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSalesReturnNoteDetail(tbl_Inv_SalesReturnNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Sales Return Note", Operation = "CanView")]
        public async Task<IActionResult> SalesReturnNoteDetailGet([FromServices] ISalesReturnNote db, int ID)
        {
            return Json(await db.GetSalesReturnNoteDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Sales Return Note", Operation = "CanViewReport")]
        public async Task<IActionResult> GetSalesReturnNoteReport([FromServices] ISalesReturnNote db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

    }
}
