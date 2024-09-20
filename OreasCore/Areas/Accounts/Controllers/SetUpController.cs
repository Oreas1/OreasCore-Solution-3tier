using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OreasCore.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class SetUpController : Controller
    {
        #region Currency And Country

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCurrencyAndCountryAsync([FromServices] IAuthorizationScheme db, [FromServices] ICurrencyAndCountry db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CurrencyAndCountryIndexCtlr",
                        WildCard = db2.GetWCLCurrencyAndCountry(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Currency And Country"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Currency And Country", Operation = "CanView")]
        public IActionResult CurrencyAndCountryIndex()
        {

            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> CurrencyAndCountryLoad([FromServices] ICurrencyAndCountry db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCurrencyAndCountry(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Currency And Country", Operation = "CanPost")]
        public async Task<string> CurrencyAndCountryPost([FromServices] ICurrencyAndCountry db, [FromBody] tbl_Ac_CurrencyAndCountry tbl_Ac_CurrencyAndCountry, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCurrencyAndCountry(tbl_Ac_CurrencyAndCountry, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Currency And Country", Operation = "CanView")]
        public async Task<IActionResult> CurrencyAndCountryGet([FromServices] ICurrencyAndCountry db, int ID)
        {
            return Json(await db.GetCurrencyAndCountry(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region FiscalYear & Closing

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedFiscalYearAsync([FromServices] IAuthorizationScheme db, [FromServices] IFiscalYear db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "FiscalYearIndexCtlr",
                        WildCard = db2.GetWCLFiscalYear(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "FiscalYear"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "FiscalYearCostingMasterCtlr",
                        WildCard = db2.GetWCLClosingMaster(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            ClosingTypeList = await db3.GetClosingTypeListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "FiscalYearCostingDetailCtlr",
                        WildCard = db2.GetWCLClosingDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "FiscalYear", Operation = "CanView")]
        public IActionResult FiscalYearIndex()
        {

            return View();
        }

        #region FiscalYear

        [AjaxOnly]
        public async Task<IActionResult> FiscalYearLoad([FromServices] IFiscalYear db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadFiscalYear(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "FiscalYear", Operation = "CanPost")]
        public async Task<string> FiscalYearPost([FromServices] IFiscalYear db, [FromBody] tbl_Ac_FiscalYear tbl_Ac_FiscalYear, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostFiscalYear(tbl_Ac_FiscalYear, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "FiscalYear", Operation = "CanView")]
        public async Task<IActionResult> FiscalYearGet([FromServices] IFiscalYear db, int ID)
        {
            return Json(await db.GetFiscalYear(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region ClosingMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "FiscalYear", Operation = "CanView")]
        public async Task<IActionResult> FiscalYearCostingMasterLoad([FromServices] IFiscalYear db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadClosingMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "FiscalYear", Operation = "CanPost")]
        public async Task<string> FiscalYearCostingMasterPost([FromServices] IFiscalYear db, [FromBody] tbl_Ac_FiscalYear_ClosingMaster tbl_Ac_FiscalYear_ClosingMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostClosingMaster(tbl_Ac_FiscalYear_ClosingMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "FiscalYear", Operation = "CanView")]
        public async Task<IActionResult> FiscalYearCostingMasterGet([FromServices] IFiscalYear db, int ID)
        {
            return Json(await db.GetClosingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region ClosingDetail
        [AjaxOnly]
        [MyAuthorization(FormName = "FiscalYear", Operation = "CanView")]
        public async Task<IActionResult> FiscalYearCostingDetailLoad([FromServices] IFiscalYear db,
           int CurrentPage = 1, int MasterID = 0,
           string FilterByText = null, string FilterValueByText = null,
           string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
           string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
           string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadClosingDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "FiscalYear", Operation = "CanPost")]
        public async Task<string> FiscalYearCostingDetailPost([FromServices] IFiscalYear db, [FromBody] tbl_Ac_FiscalYear_ClosingDetail tbl_Ac_FiscalYear_ClosingDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostClosingDetail(tbl_Ac_FiscalYear_ClosingDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "FiscalYear", Operation = "CanView")]
        public async Task<IActionResult> FiscalYearCostingDetailGet([FromServices] IFiscalYear db, int ID)
        {
            return Json(await db.GetClosingDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        #endregion

        #region ChartofAccountsType

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedChartOfAccountsTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IChartOfAccountsType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ChartOfAccountsTypeIndexCtlr",
                        WildCard = db2.GetWCLChartOfAccountsType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "ChartOfAccounts Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "ChartOfAccounts Type", Operation = "CanView")]
        public IActionResult ChartOfAccountsTypeIndex()
        {

            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> ChartOfAccountsTypeLoad([FromServices] IChartOfAccountsType db,
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
        [MyAuthorization(FormName = "ChartOfAccounts Type", Operation = "CanPost")]
        public async Task<string> ChartOfAccountsTypePost([FromServices] IChartOfAccountsType db, [FromBody] tbl_Ac_ChartOfAccounts_Type tbl_Ac_ChartOfAccounts_Type, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Ac_ChartOfAccounts_Type, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "ChartOfAccounts Type", Operation = "CanView")]
        public async Task<IActionResult> ChartOfAccountsTypeGet([FromServices] IChartOfAccountsType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region AcPolicyInventory

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedAcPolicyInventoryAsync([FromServices] IAuthorizationScheme db, [FromServices] IAcPolicyInventory db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AcPolicyInventoryIndexCtlr",
                        WildCard = db2.GetWCL(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Policy Inventory"),
                        Otherdata = new {
                            ProductTypeList = await db3.GetProductTypeListAsync(null,null)
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Policy Inventory", Operation = "CanView")]
        public IActionResult AcPolicyInventoryIndex()
        {

            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> AcPolicyInventoryLoad([FromServices] IAcPolicyInventory db,
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
        [MyAuthorization(FormName = "Policy Inventory", Operation = "CanPost")]
        public async Task<string> AcPolicyInventoryPost([FromServices] IAcPolicyInventory db, [FromBody] tbl_Ac_PolicyInventory tbl_Ac_PolicyInventory, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Ac_PolicyInventory, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Policy Inventory", Operation = "CanView")]
        public async Task<IActionResult> AcPolicyInventoryGet([FromServices] IAcPolicyInventory db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region AcPolicyWHTaxOnPurchase

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedAcPolicyWHTaxOnPurchaseAsync([FromServices] IAuthorizationScheme db, [FromServices] IAcPolicyWHTaxOnPurchase db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AcPolicyWHTaxOnPurchaseMasterCtlr",
                        WildCard = db2.GetWCLAcPolicyWHTaxOnPurchaseMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Policy WHTax On Purchase"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AcPolicyWHTaxOnPurchaseDetailCtlr",
                        WildCard = db2.GetWCLAcPolicyWHTaxOnPurchaseDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Policy WHTax On Purchase", Operation = "CanView")]
        public IActionResult AcPolicyWHTaxOnPurchaseIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy WHTax On Purchase", Operation = "CanView")]
        public async Task<IActionResult> AcPolicyWHTaxOnPurchaseMasterLoad([FromServices] IAcPolicyWHTaxOnPurchase db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAcPolicyWHTaxOnPurchaseMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Policy WHTax On Purchase", Operation = "CanPost")]
        public async Task<string> AcPolicyWHTaxOnPurchaseMasterPost([FromServices] IAcPolicyWHTaxOnPurchase db, [FromBody] tbl_Ac_PolicyWHTaxOnPurchase tbl_Ac_PolicyWHTaxOnPurchase, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostAcPolicyWHTaxOnPurchaseMaster(tbl_Ac_PolicyWHTaxOnPurchase, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Policy WHTax On Purchase", Operation = "CanView")]
        public async Task<IActionResult> AcPolicyWHTaxOnPurchaseMasterGet([FromServices] IAcPolicyWHTaxOnPurchase db, int ID)
        {
            return Json(await db.GetAcPolicyWHTaxOnPurchaseMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy WHTax On Purchase", Operation = "CanView")]
        public async Task<IActionResult> AcPolicyWHTaxOnPurchaseDetailLoad([FromServices] IAcPolicyWHTaxOnPurchase db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAcPolicyWHTaxOnPurchaseDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region AcPolicyWHTaxOnSales

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedAcPolicyWHTaxOnSalesAsync([FromServices] IAuthorizationScheme db, [FromServices] IAcPolicyWHTaxOnSales db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AcPolicyWHTaxOnSalesMasterCtlr",
                        WildCard = db2.GetWCLAcPolicyWHTaxOnSalesMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Policy WHTax On Sales"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AcPolicyWHTaxOnSalesDetailCtlr",
                        WildCard = db2.GetWCLAcPolicyWHTaxOnSalesDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Policy WHTax On Sales", Operation = "CanView")]
        public IActionResult AcPolicyWHTaxOnSalesIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy WHTax On Sales", Operation = "CanView")]
        public async Task<IActionResult> AcPolicyWHTaxOnSalesMasterLoad([FromServices] IAcPolicyWHTaxOnSales db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAcPolicyWHTaxOnSalesMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Policy WHTax On Sales", Operation = "CanPost")]
        public async Task<string> AcPolicyWHTaxOnSalesMasterPost([FromServices] IAcPolicyWHTaxOnSales db, [FromBody] tbl_Ac_PolicyWHTaxOnSales tbl_Ac_PolicyWHTaxOnSales, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostAcPolicyWHTaxOnSalesMaster(tbl_Ac_PolicyWHTaxOnSales, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Policy WHTax On Sales", Operation = "CanView")]
        public async Task<IActionResult> AcPolicyWHTaxOnSalesMasterGet([FromServices] IAcPolicyWHTaxOnSales db, int ID)
        {
            return Json(await db.GetAcPolicyWHTaxOnSalesMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy WHTax On Sales", Operation = "CanView")]
        public async Task<IActionResult> AcPolicyWHTaxOnSalesDetailLoad([FromServices] IAcPolicyWHTaxOnSales db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadAcPolicyWHTaxOnSalesDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region PaymentTerm

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPolicyPaymentTermAsync([FromServices] IAuthorizationScheme db, [FromServices] IPolicyPaymentTerm db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PolicyPaymentTermCtlr",
                        WildCard = db2.GetWCLPolicyPaymentTerm(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Policy Payment Term"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PolicyPaymentTermCOACtlr",
                        WildCard = db2.GetWCLPolicyPaymentTermCOA(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Policy Payment Term", Operation = "CanView")]
        public IActionResult AcPolicyPaymentTermIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy Payment Term", Operation = "CanView")]
        public async Task<IActionResult> PolicyPaymentTermLoad([FromServices] IPolicyPaymentTerm db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPolicyPaymentTerm(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Policy Payment Term", Operation = "CanPost")]
        public async Task<string> PolicyPaymentTermPost([FromServices] IPolicyPaymentTerm db, [FromBody] tbl_Ac_PolicyPaymentTerm tbl_Ac_PolicyPaymentTerm, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPolicyPaymentTerm(tbl_Ac_PolicyPaymentTerm, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Policy Payment Term", Operation = "CanView")]
        public async Task<IActionResult> PolicyPaymentTermGet([FromServices] IPolicyPaymentTerm db, int ID)
        {
            return Json(await db.GetPolicyPaymentTerm(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy Payment Term", Operation = "CanView")]
        public async Task<IActionResult> PolicyPaymentTermCOALoad([FromServices] IPolicyPaymentTerm db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPolicyPaymentTermCOA(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion        

        #region CompositionCostingOverHeadFactors

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCompositionCostingOverHeadFactorsAsync([FromServices] IAuthorizationScheme db, [FromServices] ICompositionCostingOverHeadFactors db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionCostingOverHeadFactorsMasterCtlr",
                        WildCard = db2.GetWCLCompositionCostingOverHeadFactorsMaster(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Composition Costing Factors"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionCostingOverHeadFactorsDetailCtlr",
                        WildCard = db2.GetWCLCompositionCostingOverHeadFactorsDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Composition Costing Factors", Operation = "CanView")]
        public IActionResult CompositionCostingOverHeadFactorsIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Costing Factors", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingOverHeadFactorsMasterLoad([FromServices] ICompositionCostingOverHeadFactors db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionCostingOverHeadFactorsMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition Costing Factors", Operation = "CanPost")]
        public async Task<string> CompositionCostingOverHeadFactorsMasterPost([FromServices] ICompositionCostingOverHeadFactors db, [FromBody] tbl_Ac_CompositionCostingOverHeadFactorsMaster tbl_Ac_CompositionCostingOverHeadFactorsMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionCostingOverHeadFactorsMaster(tbl_Ac_CompositionCostingOverHeadFactorsMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition Costing Factors", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingOverHeadFactorsMasterGet([FromServices] ICompositionCostingOverHeadFactors db, int ID)
        {
            return Json(await db.GetCompositionCostingOverHeadFactorsMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Costing Factors", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingOverHeadFactorsDetailLoad([FromServices] ICompositionCostingOverHeadFactors db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionCostingOverHeadFactorsDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition Costing Factors", Operation = "CanPost")]
        public async Task<string> CompositionCostingOverHeadFactorsDetailPost([FromServices] ICompositionCostingOverHeadFactors db, [FromBody] tbl_Ac_CompositionCostingOverHeadFactorsDetail tbl_Ac_CompositionCostingOverHeadFactorsDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionCostingOverHeadFactorsDetail(tbl_Ac_CompositionCostingOverHeadFactorsDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition Costing Factors", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingOverHeadFactorsDetailGet([FromServices] ICompositionCostingOverHeadFactors db, int ID)
        {
            return Json(await db.GetCompositionCostingOverHeadFactorsDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region CostingIndirectExpenseList

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCostingIndirectExpenseListAsync([FromServices] IAuthorizationScheme db, [FromServices] ICostingIndirectExpenseList db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CostingIndirectExpenseListIndexCtlr",
                        WildCard = db2.GetWCLCostingIndirectExpenseList(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Costing Indirect Expense List"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Costing Indirect Expense List", Operation = "CanView")]
        public IActionResult CostingIndirectExpenseListIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Costing Indirect Expense List", Operation = "CanView")]
        public async Task<IActionResult> CostingIndirectExpenseListLoad([FromServices] ICostingIndirectExpenseList db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCostingIndirectExpenseList(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Costing Indirect Expense List", Operation = "CanPost")]
        public async Task<string> CostingIndirectExpenseListPost([FromServices] ICostingIndirectExpenseList db, [FromBody] tbl_Ac_CostingIndirectExpenseList tbl_Ac_CostingIndirectExpenseList, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCostingIndirectExpenseList(tbl_Ac_CostingIndirectExpenseList, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Costing Indirect Expense List", Operation = "CanView")]
        public async Task<IActionResult> CostingIndirectExpenseListGet([FromServices] ICostingIndirectExpenseList db, int ID)
        {
            return Json(await db.GetCostingIndirectExpenseList(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        
    }
}
