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
    public class InvoiceController : Controller
    {

        #region PurchaseNote

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseNoteInvoice db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseNoteMasterCtlr",
                        WildCard = db2.GetWCLPurchaseNoteInvoiceMaster(),
                        LoadByCard = db2.GetWCLBPurchaseNoteInvoiceMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Purchase Note Invoice"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseNoteDetailCtlr",
                        WildCard = db2.GetWCLPurchaseNoteInvoiceDetail(),
                        Reports = db2.GetRLPurchaseNote(),
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

        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanView")]
        public IActionResult PurchaseNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteMasterLoad([FromServices] IPurchaseNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseNoteInvoiceMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanPost")]
        public async Task<string> PurchaseNoteMasterPost([FromServices] IPurchaseNoteInvoice db, [FromBody] tbl_Inv_PurchaseNoteMaster tbl_Inv_PurchaseNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseNoteInvoiceMaster(tbl_Inv_PurchaseNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteMasterGet([FromServices] IPurchaseNoteInvoice db, int ID)
        {
            return Json(await db.GetPurchaseNoteInvoiceMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteDetailLoad([FromServices] IPurchaseNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseNoteInvoiceDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanPost")]
        public async Task<string> PurchaseNoteDetailPost([FromServices] IPurchaseNoteInvoice db, [FromBody] tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseNoteInvoiceDetail(tbl_Inv_PurchaseNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteDetailGet([FromServices] IPurchaseNoteInvoice db, int ID)
        {
            return Json(await db.GetPurchaseNoteInvoiceDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region DetailOfDetail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteDetailOfDetailLoad([FromServices] IPurchaseNoteInvoice db,
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

        [MyAuthorization(FormName = "Purchase Note Invoice", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPurchaseNoteReport([FromServices] IPurchaseNoteInvoice db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region PurchaseReturnNote

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseReturnNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseReturnNoteInvoice db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseReturnNoteMasterCtlr",
                        WildCard = db2.GetWCLPurchaseReturnNoteInvoiceMaster(),
                        LoadByCard = db2.GetWCLBPurchaseReturnNoteInvoiceMaster(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Purchase Return Note Invoice"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseReturnNoteDetailCtlr",
                        WildCard = db2.GetWCLPurchaseReturnNoteInvoiceDetail(),
                        Reports = db2.GetRLPurchaseReturnNote(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Return Note Invoice", Operation = "CanView")]
        public IActionResult PurchaseReturnNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Return Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseReturnNoteMasterLoad([FromServices] IPurchaseReturnNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseReturnNoteInvoiceMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Return Note Invoice", Operation = "CanPost")]
        public async Task<string> PurchaseReturnNoteMasterPost([FromServices] IPurchaseReturnNoteInvoice db, [FromBody] tbl_Inv_PurchaseReturnNoteMaster tbl_Inv_PurchaseReturnNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseReturnNoteInvoiceMaster(tbl_Inv_PurchaseReturnNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Return Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseReturnNoteMasterGet([FromServices] IPurchaseReturnNoteInvoice db, int ID)
        {
            return Json(await db.GetPurchaseReturnNoteInvoiceMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Return Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseReturnNoteDetailLoad([FromServices] IPurchaseReturnNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseReturnNoteInvoiceDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Return Note Invoice", Operation = "CanPost")]
        public async Task<string> PurchaseReturnNoteDetailPost([FromServices] IPurchaseReturnNoteInvoice db, [FromBody] tbl_Inv_PurchaseReturnNoteDetail tbl_Inv_PurchaseReturnNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseReturnNoteInvoiceDetail(tbl_Inv_PurchaseReturnNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Return Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> PurchaseReturnNoteDetailGet([FromServices] IPurchaseReturnNoteInvoice db, int ID)
        {
            return Json(await db.GetPurchaseReturnNoteInvoiceDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region Report

        [MyAuthorization(FormName = "Purchase Return Note Invoice", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPurchaseReturnNoteReport([FromServices] IPurchaseReturnNoteInvoice db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region SalesNote

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedSalesNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] ISalesNoteInvoice db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesNoteMasterCtlr",
                        WildCard = db2.GetWCLSalesNoteInvoiceMaster(),
                        LoadByCard = db2.GetWCLBSalesNoteInvoiceMaster(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Sales Note Invoice"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesNoteDetailCtlr",
                        WildCard = db2.GetWCLSalesNoteInvoiceDetail(),
                        Reports = db2.GetRLSalesNote(),
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

        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanView")]
        public IActionResult SalesNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteMasterLoad([FromServices] ISalesNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesNoteInvoiceMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanPost")]
        public async Task<string> SalesNoteMasterPost([FromServices] ISalesNoteInvoice db, [FromBody] tbl_Inv_SalesNoteMaster tbl_Inv_SalesNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSalesNoteInvoiceMaster(tbl_Inv_SalesNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteMasterGet([FromServices] ISalesNoteInvoice db, int ID)
        {
            return Json(await db.GetSalesNoteInvoiceMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteDetailLoad([FromServices] ISalesNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesNoteInvoiceDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanPost")]
        public async Task<string> SalesNoteDetailPost([FromServices] ISalesNoteInvoice db, [FromBody] tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSalesNoteInvoiceDetail(tbl_Inv_SalesNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteDetailGet([FromServices] ISalesNoteInvoice db, int ID)
        {
            return Json(await db.GetSalesNoteInvoiceDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Return

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesNoteDetailReturnLoad([FromServices] ISalesNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesNoteInvoiceDetailReturn(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Sales Note Invoice", Operation = "CanViewReport")]
        public async Task<IActionResult> GetSalesNoteReport([FromServices] ISalesNoteInvoice db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region SalesReturnNote

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedSalesReturnNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] ISalesReturnNoteInvoice db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesReturnNoteMasterCtlr",
                        WildCard = db2.GetWCLSalesReturnNoteInvoiceMaster(),
                        LoadByCard = db2.GetWCLBSalesReturnNoteInvoiceMaster(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Sales Return Note Invoice"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SalesReturnNoteDetailCtlr",
                        WildCard = db2.GetWCLSalesReturnNoteInvoiceDetail(),
                        Reports = db2.GetRLSalesReturnNote(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Sales Return Note Invoice", Operation = "CanView")]
        public IActionResult SalesReturnNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Return Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesReturnNoteMasterLoad([FromServices] ISalesReturnNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesReturnNoteInvoiceMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Sales Return Note Invoice", Operation = "CanPost")]
        public async Task<string> SalesReturnNoteMasterPost([FromServices] ISalesReturnNoteInvoice db, [FromBody] tbl_Inv_SalesReturnNoteMaster tbl_Inv_SalesReturnNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSalesReturnNoteInvoiceMaster(tbl_Inv_SalesReturnNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Sales Return Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesReturnNoteMasterGet([FromServices] ISalesReturnNoteInvoice db, int ID)
        {
            return Json(await db.GetSalesReturnNoteInvoiceMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Sales Return Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesReturnNoteDetailLoad([FromServices] ISalesReturnNoteInvoice db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSalesReturnNoteInvoiceDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Sales Return Note Invoice", Operation = "CanPost")]
        public async Task<string> SalesReturnNoteDetailPost([FromServices] ISalesReturnNoteInvoice db, [FromBody] tbl_Inv_SalesReturnNoteDetail tbl_Inv_SalesReturnNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSalesReturnNoteInvoiceDetail(tbl_Inv_SalesReturnNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Sales Return Note Invoice", Operation = "CanView")]
        public async Task<IActionResult> SalesReturnNoteDetailGet([FromServices] ISalesReturnNoteInvoice db, int ID)
        {
            return Json(await db.GetSalesReturnNoteInvoiceDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Sales Return Note Invoice", Operation = "CanViewReport")]
        public async Task<IActionResult> GetSalesReturnNoteReport([FromServices] ISalesReturnNoteInvoice db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion
    }
}
