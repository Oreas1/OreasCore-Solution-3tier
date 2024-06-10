using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using OreasModel;
using System;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using System.Linq;

namespace OreasCore.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class VoucherController : Controller
    {
        #region Bank Document

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBankDocumentAsync([FromServices] IAuthorizationScheme db, [FromServices] IBankDocument db2, [FromServices] IAccountsList db3, string IsFor = "")
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BankDocumentMasterCtlr",
                        WildCard = db2.GetWCLBankDocumentMaster(),
                        LoadByCard = db2.GetWCLBBankDocumentMaster(),
                        Reports = IsFor == "Payment" ? db2.GetRLBankPaymentMasterDocument() : IsFor == "Receive" ? db2.GetRLBankReceiveMasterDocument() : null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Bank Document"),
                        Otherdata = new { FYS = FiscalYear.Start, FYE = FiscalYear.End }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BankDocumentDetailCtlr",
                        WildCard = db2.GetWCLBankDocumentDetail(),
                        Reports = IsFor == "Payment" ? db2.GetRLBankPaymentDocument() : IsFor == "Receive" ? db2.GetRLBankReceiveDocument() : null,
                        Privilege = null,
                        Otherdata = new { BankTransactionModeList = await db3.GetBankTransactionModeListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Bank Document", Operation = "CanView")]
        public IActionResult BankPaymentDocumentIndex()
        {
            ViewBag.IsFor = "Payment";
            return View("BankDocumentIndex");
        }

        [MyAuthorization(FormName = "Bank Document", Operation = "CanView")]
        public IActionResult BankReceiveDocumentIndex()
        {
            ViewBag.IsFor = "Receive";
            return View("BankDocumentIndex");
            
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Bank Document", Operation = "CanView")]
        public async Task<IActionResult> BankDocumentMasterLoad([FromServices] IBankDocument db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null, string IsFor = "")
        {
            PagedData<object> pageddata =
                await db.LoadBankDocumentMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, IsFor);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Bank Document", Operation = "CanPost")]
        public async Task<string> BankDocumentMasterPost([FromServices] IBankDocument db, [FromBody] tbl_Ac_V_BankDocumentMaster tbl_Ac_V_BankDocumentMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBankDocumentMaster(tbl_Ac_V_BankDocumentMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Bank Document", Operation = "CanView")]
        public async Task<IActionResult> BankDocumentMasterGet([FromServices] IBankDocument db, int ID)
        {
            return Json(await db.GetBankDocumentMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Bank Document", Operation = "CanView")]
        public async Task<IActionResult> BankDocumentDetailLoad([FromServices] IBankDocument db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBankDocumentDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Bank Document", Operation = "CanPost")]
        public async Task<string> BankDocumentDetailPost([FromServices] IBankDocument db, [FromBody] tbl_Ac_V_BankDocumentDetail tbl_Ac_V_BankDocumentDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBankDocumentDetail(tbl_Ac_V_BankDocumentDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Bank Document", Operation = "CanView")]
        public async Task<IActionResult> BankDocumentDetailGet([FromServices] IBankDocument db, int ID)
        {
            return Json(await db.GetBankDocumentDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Bank Document", Operation = "CanViewReport")]
        public async Task<IActionResult> GetBankDocumentReport([FromServices] IBankDocument db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID,User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region Cash Document

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCashDocumentAsync([FromServices] IAuthorizationScheme db, [FromServices] ICashDocument db2, [FromServices] IAccountsList db3, string IsFor = "")
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CashDocumentMasterCtlr",
                        WildCard = db2.GetWCLCashDocumentMaster(),
                        LoadByCard = db2.GetWCLBCashDocumentMaster(),
                        Reports = IsFor == "Payment" ? db2.GetRLCashPaymentMasterDocument() : IsFor == "Receive" ? db2.GetRLCashReceiveMasterDocument() : null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Cash Document"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CashDocumentDetailCtlr",
                        WildCard = db2.GetWCLCashDocumentDetail(),
                        Reports = IsFor == "Payment" ? db2.GetRLCashPaymentDocument() : IsFor == "Receive" ? db2.GetRLCashReceiveDocument() : null,
                        Privilege = null,
                        Otherdata = new { CashTransactionModeList = await db3.GetCashTransactionModeListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        [MyAuthorization(FormName = "Cash Document", Operation = "CanView")]
        public IActionResult CashPaymentDocumentIndex()
        {
            ViewBag.IsFor = "Payment";
            return View("CashDocumentIndex");
        }

        [MyAuthorization(FormName = "Cash Document", Operation = "CanView")]
        public IActionResult CashReceiveDocumentIndex()
        {
            ViewBag.IsFor = "Receive";
            return View("CashDocumentIndex");

        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Cash Document", Operation = "CanView")]
        public async Task<IActionResult> CashDocumentMasterLoad([FromServices] ICashDocument db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null, string IsFor = "")
        {
            PagedData<object> pageddata =
                await db.LoadCashDocumentMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, IsFor);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Cash Document", Operation = "CanPost")]
        public async Task<string> CashDocumentMasterPost([FromServices] ICashDocument db, [FromBody] tbl_Ac_V_CashDocumentMaster tbl_Ac_V_CashDocumentMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCashDocumentMaster(tbl_Ac_V_CashDocumentMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Cash Document", Operation = "CanView")]
        public async Task<IActionResult> CashDocumentMasterGet([FromServices] ICashDocument db, int ID)
        {
            return Json(await db.GetCashDocumentMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Cash Document", Operation = "CanView")]
        public async Task<IActionResult> CashDocumentDetailLoad([FromServices] ICashDocument db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCashDocumentDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Cash Document", Operation = "CanPost")]
        public async Task<string> CashDocumentDetailPost([FromServices] ICashDocument db, [FromBody] tbl_Ac_V_CashDocumentDetail tbl_Ac_V_CashDocumentDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCashDocumentDetail(tbl_Ac_V_CashDocumentDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Cash Document", Operation = "CanView")]
        public async Task<IActionResult> CashDocumentDetailGet([FromServices] ICashDocument db, int ID)
        {
            return Json(await db.GetCashDocumentDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Cash Document", Operation = "CanViewReport")]
        public async Task<IActionResult> GetCashDocumentReport([FromServices] ICashDocument db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID,User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region Journal Document

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedJournalDocumentAsync([FromServices] IAuthorizationScheme db, [FromServices] IJournalDocument db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "JournalDocumentMasterCtlr",
                        WildCard = db2.GetWCLJournalDocumentMaster(),
                        LoadByCard = db2.GetWCLBJournalDocumentMaster(),
                        Reports = db2.GetRLJournalMasterDocument(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Journal Document"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "JournalDocumentDetailCtlr",
                        WildCard = db2.GetWCLJournalDocumentDetail(),
                        Reports = db2.GetRLJournalDocument(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Journal Document", Operation = "CanView")]
        public IActionResult JournalDocumentIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Journal Document", Operation = "CanView")]
        public async Task<IActionResult> JournalDocumentMasterLoad([FromServices] IJournalDocument db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadJournalDocumentMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Journal Document", Operation = "CanPost")]
        public async Task<string> JournalDocumentMasterPost([FromServices] IJournalDocument db, [FromBody] tbl_Ac_V_JournalDocumentMaster tbl_Ac_V_JournalDocumentMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostJournalDocumentMaster(tbl_Ac_V_JournalDocumentMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Journal Document", Operation = "CanView")]
        public async Task<IActionResult> JournalDocumentMasterGet([FromServices] IJournalDocument db, int ID)
        {
            return Json(await db.GetJournalDocumentMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Journal Document", Operation = "CanView")]
        public async Task<IActionResult> JournalDocumentDetailLoad([FromServices] IJournalDocument db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadJournalDocumentDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Journal Document", Operation = "CanPost")]
        public async Task<string> JournalDocumentDetailPost([FromServices] IJournalDocument db, [FromBody] tbl_Ac_V_JournalDocumentDetail tbl_Ac_V_JournalDocumentDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostJournalDocumentDetail(tbl_Ac_V_JournalDocumentDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Journal Document", Operation = "CanView")]
        public async Task<IActionResult> JournalDocumentDetailGet([FromServices] IJournalDocument db, int ID)
        {
            return Json(await db.GetJournalDocumentDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Journal Document", Operation = "CanPost")]
        public async Task<string> JournalDocumentDetailUploadExcelFile([FromServices] IJournalDocument db, int MasterID, IFormFile JDExcelFile, string operation = "")
        {
            if (JDExcelFile.Length > 0 && Path.GetExtension(JDExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await JDExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<JournalDocExcelData> JournalDocExcelDataList = new List<JournalDocExcelData>();

                    try
                    {

                        await Task.Factory.StartNew(() =>
                        {
                            DateTime postingDate;

                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                postingDate = worksheet.Cells[rowNo, 4].Value == null ? DateTime.Now :
                                              DateTime.TryParse(worksheet.Cells[rowNo, 4].Value.ToString(), out DateTime postingDate1) ? Convert.ToDateTime(worksheet.Cells[rowNo, 4].Value) : DateTime.Now;



                                JournalDocExcelDataList.Add(new JournalDocExcelData()
                                {
                                    MasterID = MasterID,
                                    AcCode = worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString(),
                                    Narration = worksheet.Cells[rowNo, 2].Value == null ? "" : worksheet.Cells[rowNo, 2].Value.ToString(),
                                    Amount = worksheet.Cells[rowNo, 3].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 3].Value.ToString(), out double a1) ? Convert.ToDouble(worksheet.Cells[rowNo, 3].Value) : 0,
                                    PostingDate = postingDate
                                });
                            }
                        });

                        await db.JournalDocumentDetailUploadExcelFile(JournalDocExcelDataList, operation, User.Identity.Name);

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

        #region Report

        [MyAuthorization(FormName = "Journal Document", Operation = "CanViewReport")]
        public async Task<IActionResult> GetJournalDocumentReport([FromServices] IJournalDocument db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion


        #endregion

        #region Journal Document 2

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedJournalDocument2Async([FromServices] IAuthorizationScheme db, [FromServices] IJournalDocument2 db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "JournalDocument2MasterCtlr",
                        WildCard = db2.GetWCLJournalDocument2Master(),
                        LoadByCard = db2.GetWCLBJournalDocument2Master(),
                        Reports = db2.GetRLJournalDocument2Master(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Journal Document2"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "JournalDocument2DetailCtlr",
                        WildCard = db2.GetWCLJournalDocument2Detail(),
                        Reports = db2.GetRLJournalDocument2Detail(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Journal Document2", Operation = "CanView")]
        public IActionResult JournalDocument2Index()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Journal Document2", Operation = "CanView")]
        public async Task<IActionResult> JournalDocument2MasterLoad([FromServices] IJournalDocument2 db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadJournalDocument2Master(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Journal Document2", Operation = "CanPost")]
        public async Task<string> JournalDocument2MasterPost([FromServices] IJournalDocument2 db, [FromBody] tbl_Ac_V_JournalDocument2Master tbl_Ac_V_JournalDocument2Master, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostJournalDocument2Master(tbl_Ac_V_JournalDocument2Master, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Journal Document2", Operation = "CanView")]
        public async Task<IActionResult> JournalDocument2MasterGet([FromServices] IJournalDocument2 db, int ID)
        {
            return Json(await db.GetJournalDocument2Master(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Journal Document2", Operation = "CanView")]
        public async Task<IActionResult> JournalDocument2DetailLoad([FromServices] IJournalDocument2 db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadJournalDocument2Detail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Journal Document2", Operation = "CanPost")]
        public async Task<string> JournalDocument2DetailPost([FromServices] IJournalDocument2 db, [FromBody] tbl_Ac_V_JournalDocument2Detail tbl_Ac_V_JournalDocument2Detail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostJournalDocument2Detail(tbl_Ac_V_JournalDocument2Detail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Journal Document2", Operation = "CanView")]
        public async Task<IActionResult> JournalDocument2DetailGet([FromServices] IJournalDocument2 db, int ID)
        {
            return Json(await db.GetJournalDocument2Detail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region Report

        [MyAuthorization(FormName = "Journal Document2", Operation = "CanViewReport")]
        public async Task<IActionResult> GetJournalDocument2Report([FromServices] IJournalDocument2 db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion


        #endregion
    }
}
