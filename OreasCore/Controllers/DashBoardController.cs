using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OreasCore.Controllers
{
    public class DashBoardController : Controller
    {
        #region Credential DashBoard

        [MyAuthorization]
        public async Task<IActionResult> CredentialsIndexAsync([FromServices] IAuthorizationScheme db)
        {
            if (await db.IsUserAuthorizedDashBoardAsync(User.Identity.Name, "IsCredentialsDashBoard"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home", "AccessDenied");
            }   
        }

        [MyAuthorization]
        public async Task<IActionResult> LoadCredentialsInfo([FromServices] IDashBoard db, int ID)
        {

            return Json(await db.LoadCredentialsInfo(User.Identity.Name), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> PostChangedKey([FromServices] IDashBoard db, string ChangedKey = "", string UserId = "")
        {
            if (ModelState.IsValid)
                return await db.PostChangedKey(ChangedKey, UserId);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #endregion

        #region HR Dashboard

        [MyAuthorization]
        public async Task<IActionResult> HRIndexAsync([FromServices] IAuthorizationScheme db)
        {
            if (await db.IsUserAuthorizedDashBoardAsync(User.Identity.Name, "IsWPTDashBoard"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home", "AccessDenied");
            }
        }

        [MyAuthorization]
        public async Task<IActionResult> LoadUserInfo([FromServices] IDashBoard db, int ID)
        {

            return Json(await db.LoadUserInfo(User.Identity.Name), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> LoadSalaryDetail([FromServices] IDashBoard db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalary(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> LoadSalaryStructureDetail([FromServices] IDashBoard db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalaryStructure(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> LoadLoanDetail([FromServices] IDashBoard db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadLoan(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization]
        public async Task<IActionResult> LoadAttendance([FromServices] IDashBoard db, int EmpID = 0, int MonthID = 0)
        {
            return Json(await db.LoadAttendance(EmpID, MonthID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization]
        public async Task<IActionResult> LoadTeamAT([FromServices] IDashBoard db, int EmpID = 0, DateTime? ATDate = null)
        {
            return Json(await db.LoadTeamAT(EmpID, ATDate), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Management DashBoard

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedManagementDashBoardAsync([FromServices] IAuthorizationScheme db, [FromServices] IManagementDashBoard db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementDashBoardCtlr",
                        WildCard = null,
                        LoadByCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnDashBoardAsync(User.Identity.Name,"ManagementDashBoard"), 
                        Otherdata = await db2.GetDashBoardData(User.Identity.Name)
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementBankDocCtlr",
                        WildCard = db2.GetWCLBankDocument(),
                        WildCardDateRange = db2.GetWCLDRBankDocument(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementCashDocCtlr",
                        WildCard = db2.GetWCLCashDocument(),
                        WildCardDateRange = db2.GetWCLDRCashDocument(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementJournalDocCtlr",
                        WildCard = db2.GetWCLJournalDocument(),
                        WildCardDateRange = db2.GetWCLDRJournalDocument(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementJournalDoc2Ctlr",
                        WildCard = db2.GetWCLJournalDocument2(),
                        WildCardDateRange = db2.GetWCLDRJournalDocument2(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementPNCtlr",
                        WildCard = db2.GetWCLPurchaseNote(),
                        WildCardDateRange = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementPRNCtlr",
                        WildCard = db2.GetWCLPurchaseReturnNote(),
                        WildCardDateRange = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementSNCtlr",
                        WildCard = db2.GetWCLSalesNote(),
                        WildCardDateRange = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ManagementSRNCtlr",
                        WildCard = db2.GetWCLSalesReturnNote(),
                        WildCardDateRange = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }
       
        [MyAuthorization]
        public async Task<IActionResult> ManagementIndexAsync([FromServices] IAuthorizationScheme db)
        {
            if (await db.IsUserAuthorizedDashBoardAsync(User.Identity.Name, "IsManagementDashBoard"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home", "AccessDenied");
            }
        }
        
        #region Bank Doc   
        
        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> BankDocumentLoad([FromServices] IManagementDashBoard db,
        int CurrentPage = 1, string IsFor = "",
        string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBankDocument(CurrentPage, IsFor, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> BankDocumentSupervised([FromServices] IManagementDashBoard db, int ID = 0)
        {
            return await db.SupervisedBankDocument(ID, User.Identity.Name);
        }

        #endregion

        #region Cash Doc   

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> CashDocumentLoad([FromServices] IManagementDashBoard db,
        int CurrentPage = 1, string IsFor = "",
        string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCashDocument(CurrentPage, IsFor, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> CashDocumentSupervised([FromServices] IManagementDashBoard db, int ID = 0)
        {
            return await db.SupervisedCashDocument(ID, User.Identity.Name);
        }

        #endregion

        #region Journal Doc   

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> JournalDocumentLoad([FromServices] IManagementDashBoard db,
        int CurrentPage = 1, string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadJournalDocument(CurrentPage, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> JournalDocumentSupervised([FromServices] IManagementDashBoard db, int ID = 0)
        {
            return await db.SupervisedJournalDocument(ID, User.Identity.Name);
        }

        #endregion

        #region Journal Doc2   

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> JournalDocument2Load([FromServices] IManagementDashBoard db,
        int CurrentPage = 1, string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadJournalDocument2(CurrentPage, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> JournalDocument2Supervised([FromServices] IManagementDashBoard db, int ID = 0)
        {
            return await db.SupervisedJournalDocument2(ID, User.Identity.Name);
        }

        #endregion

        #region PurchaseNote  

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> PurchaseNoteLoad([FromServices] IManagementDashBoard db,
        int CurrentPage = 1, string IsFor = "",
        string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseNote(CurrentPage, IsFor, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> PurchaseNoteSupervised([FromServices] IManagementDashBoard db, int ID = 0)
        {
            return await db.SupervisedPurchaseNote(ID, User.Identity.Name);
        }

        #endregion

        #region PurchaseReturnNote  

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> PurchaseReturnNoteLoad([FromServices] IManagementDashBoard db,
        int CurrentPage = 1, string IsFor = "",
        string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseReturnNote(CurrentPage, IsFor, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> PurchaseReturnNoteSupervised([FromServices] IManagementDashBoard db, int ID = 0)
        {
            return await db.SupervisedPurchaseReturnNote(ID, User.Identity.Name);
        }

        #endregion

        #region SalesNote  

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> SalesNoteLoad([FromServices] IManagementDashBoard db,
        int CurrentPage = 1, string IsFor = "",
        string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesNote(CurrentPage, IsFor, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> SalesNoteSupervised([FromServices] IManagementDashBoard db, int ID = 0)
        {
            return await db.SupervisedSalesNote(ID, User.Identity.Name);
        }

        #endregion

        #region SalesReturnNote  

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> SalesReturnNoteLoad([FromServices] IManagementDashBoard db,
        int CurrentPage = 1, string IsFor = "",
        string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesReturnNote(CurrentPage, IsFor, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization]
        public async Task<string> SalesReturnNoteSupervised([FromServices] IManagementDashBoard db, int ID = 0)
        {
            return await db.SupervisedSalesReturnNote(ID, User.Identity.Name);
        }

        #endregion

        [MyAuthorization]
        public async Task<IActionResult> GetChartOfAccountsReport([FromServices] IChartOfAccounts db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        [MyAuthorization]
        public async Task<IActionResult> GetBankDocumentReport([FromServices] IBankDocument db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        [MyAuthorization]
        public async Task<IActionResult> GetCashDocumentReport([FromServices] ICashDocument db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        [MyAuthorization]
        public async Task<IActionResult> GetJournalDocumentReport([FromServices] IJournalDocument db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        [MyAuthorization]
        public async Task<IActionResult> GetJournalDocument2Report([FromServices] IJournalDocument2 db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        [MyAuthorization]
        public async Task<IActionResult> GetPurchaseNoteReport([FromServices] IPurchaseNoteInvoice db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        [MyAuthorization]
        public async Task<IActionResult> GetPurchaseReturnNoteReport([FromServices] IPurchaseReturnNoteInvoice db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        [MyAuthorization]
        public async Task<IActionResult> GetSalesNoteReport([FromServices] ISalesNoteInvoice db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        [MyAuthorization]
        public async Task<IActionResult> GetSalesReturnNoteReport([FromServices] ISalesReturnNoteInvoice db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }



        #endregion

    }
}
