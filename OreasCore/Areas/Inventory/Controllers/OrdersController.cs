using iText.StyledXmlParser.Jsoup.Nodes;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MimeKit;
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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Formats.Asn1.AsnWriter;

namespace OreasCore.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class OrdersController : Controller
    {

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetPurchaseOrderListAsync([FromServices] IInventoryList IList, string QueryName = "", string POFilterBy = "", string POFilterValue = "", int SupplierID = 0, int ProductID = 0)
        {
            return Json(
                await IList.GetPurchaseOrderListAsync(QueryName, POFilterBy, POFilterValue, SupplierID, ProductID)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetSubDistributorListAsync([FromServices] IInventoryList IList, string QueryName = "", string SDFilterBy = "", string SDFilterValue = "", int FormID = 0)
        {
            return Json(
                await IList.GetSubDistributorListAsync(QueryName, SDFilterBy, SDFilterValue, FormID)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetOrderNoteListAsync([FromServices] IInventoryList IList, string QueryName = "", string ONFilterBy = "", string ONFilterValue = "", int CustomerID = 0, int ProductID = 0)
        {
            return Json(
                await IList.GetOrderNoteListAsync(QueryName, ONFilterBy, ONFilterValue, CustomerID, ProductID)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region OrderNote

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedOrderNoteAsync([FromServices] IAuthorizationScheme db, [FromServices] IOrderNote db2, [FromServices] IIdentityList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrderNoteMasterCtlr",
                        WildCard = db2.GetWCLOrderNoteMaster(),
                        LoadByCard = db2.GetWCLBOrderNoteMaster(),
                        Reports = db2.GetRLOrderNoteMaster(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Order Note"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrderNoteDetailCtlr",
                        WildCard = db2.GetWCLOrderNoteDetail(),
                        LoadByCard = db2.GetWCLBOrderNoteDetail(),
                        Reports = db2.GetRLOrderNote(),
                        Privilege = null,
                        Otherdata = new { AspNetOreasPriorityList = await IList.GetAspNetOreasPriorityListAsync(null,null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrderNoteDetailSubDistributorCtlr",
                        WildCard = db2.GetWCLOrderNoteDetailSubDistributor(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrderNoteDetailBMRDetailCtlr",
                        WildCard =  db2.GetWCLBMRDetail(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrderNoteDetailSalesNoteDetailCtlr",
                        WildCard =  db2.GetWCLSalesNoteDetail(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrderNoteDetailSalesReturnNoteDetailCtlr",
                        WildCard =  db2.GetWCLSalesReturnNoteDetail(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public IActionResult OrderNoteIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteMasterLoad([FromServices] IOrderNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadOrderNoteMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Order Note", Operation = "CanPost")]
        public async Task<string> OrderNoteMasterPost([FromServices] IOrderNote db, [FromBody] tbl_Inv_OrderNoteMaster tbl_Inv_OrderNoteMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostOrderNoteMaster(tbl_Inv_OrderNoteMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteMasterGet([FromServices] IOrderNote db, int ID)
        {
            return Json(await db.GetOrderNoteMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteDetailLoad([FromServices] IOrderNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadOrderNoteDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Order Note", Operation = "CanPost")]
        public async Task<string> OrderNoteDetailPost([FromServices] IOrderNote db, [FromBody] tbl_Inv_OrderNoteDetail tbl_Inv_OrderNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostOrderNoteDetail(tbl_Inv_OrderNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteDetailGet([FromServices] IOrderNote db, int ID)
        {
            return Json(await db.GetOrderNoteDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail SubDistributor

        [AjaxOnly]
        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteDetailSubDistributorLoad([FromServices] IOrderNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadOrderNoteDetailSubDistributor(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Order Note", Operation = "CanPost")]
        public async Task<string> OrderNoteDetailSubDistributorPost([FromServices] IOrderNote db, [FromBody] tbl_Inv_OrderNoteDetail_SubDistributor tbl_Inv_OrderNoteDetail_SubDistributor, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostOrderNoteDetailSubDistributor(tbl_Inv_OrderNoteDetail_SubDistributor, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteDetailSubDistributorGet([FromServices] IOrderNote db, int ID)
        {
            return Json(await db.GetOrderNoteDetailSubDistributor(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDetail
        [AjaxOnly]
        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteDetailBMRDetailLoad([FromServices] IOrderNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        #region SalesNoteDetail
        [AjaxOnly]
        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteDetailSalesNoteDetailLoad([FromServices] IOrderNote db,
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
        #endregion

        #region SalesReturnNoteDetail
        [AjaxOnly]
        [MyAuthorization(FormName = "Order Note", Operation = "CanView")]
        public async Task<IActionResult> OrderNoteDetailSalesReturnNoteDetailLoad([FromServices] IOrderNote db,
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
        #endregion

        #region Report

        [MyAuthorization(FormName = "Order Note", Operation = "CanViewReport")]
        public async Task<IActionResult> GetOrderNoteReport([FromServices] IOrderNote db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region PurchaseOrder Local AND Import

        //------Local PO-----//
        #region PurchaseOrder Local

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseOrderAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseOrder db2, [FromServices] IIdentityList IList, [FromServices] IInventoryList IList2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseOrderMasterCtlr",
                        WildCard = db2.GetWCLPurchaseOrderMaster(),
                        LoadByCard = db2.GetWCLBPurchaseOrderMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Purchase Order"),
                        Otherdata = new { POTermsConditionsList = await IList2.GetPOTermsConditionsListAsync(null,null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseOrderDetailCtlr",
                        WildCard = db2.GetWCLPurchaseOrderDetail(),
                        LoadByCard = db2.GetWCLBPurchaseOrderDetail(),
                        Reports = db2.GetRLPurchaseOrder(),
                        Privilege = null,
                        Otherdata = new { 
                            AspNetOreasPriorityList = await IList.GetAspNetOreasPriorityListAsync(null,null),
                            ManufacturerPOList = await IList2.GetPOManufacturerListAsync(null,null),
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseOrderDetailPNCtlr",
                        WildCard = null,
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseOrderDetailPRNCtlr",
                        WildCard = null,
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Order", Operation = "CanView")]
        public IActionResult PurchaseOrderIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Order", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderMasterLoad([FromServices] IPurchaseOrder db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null, bool IsCanViewOnlyOwnData = false)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseOrderMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad,User.Identity.Name, IsCanViewOnlyOwnData);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Order", Operation = "CanPost")]
        public async Task<string> PurchaseOrderMasterPost([FromServices] IPurchaseOrder db, [FromBody] tbl_Inv_PurchaseOrderMaster tbl_Inv_PurchaseOrderMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseOrderMaster(tbl_Inv_PurchaseOrderMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Order", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderMasterGet([FromServices] IPurchaseOrder db, int ID)
        {
            return Json(await db.GetPurchaseOrderMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Order", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderDetailLoad([FromServices] IPurchaseOrder db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseOrderDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Order", Operation = "CanPost")]
        public async Task<string> PurchaseOrderDetailPost([FromServices] IPurchaseOrder db, [FromBody] tbl_Inv_PurchaseOrderDetail tbl_Inv_PurchaseOrderDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseOrderDetail(tbl_Inv_PurchaseOrderDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Order", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderDetailGet([FromServices] IPurchaseOrder db, int ID)
        {
            return Json(await db.GetPurchaseOrderDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        //------Import PO----//
        #region PurchaseOrder Import

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseOrderImportAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseOrder db2, [FromServices] IIdentityList IList, [FromServices] IInventoryList IList2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseOrderImportMasterCtlr",
                        WildCard = db2.GetWCLPurchaseOrderImportMaster(),
                        LoadByCard = db2.GetWCLBPurchaseOrderMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Purchase Order Import"),
                        Otherdata = new {
                            SupplierList = await IList2.GetPOSupplierListAsync(null,null),
                            ManufacturerList = await IList2.GetPOManufacturerListAsync(null,null),
                            IndenterList = await IList2.GetPOIndenterListAsync(null,null),
                            ImportTermList = await IList2.GetPOImportTermListAsync(null,null),
                            CurrencyCodeList = await IList2.GetCurrencyCodeListAsync(null,null),
                            CountryList = await IList2.GetCountryListAsync(null,null),
                            InternationalCommercialTermList = await IList2.GetInternationalCommercialTermListAsync(null,null),
                            TransportList = await IList2.GetTransportListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseOrderImportDetailCtlr",
                        WildCard = db2.GetWCLPurchaseOrderDetail(),
                        LoadByCard = db2.GetWCLBPurchaseOrderDetail(),
                        Reports = db2.GetRLPurchaseOrder(),
                        Privilege = null,
                        Otherdata = new {
                            AspNetOreasPriorityList = await IList.GetAspNetOreasPriorityListAsync(null,null),
                            MeasurementUnitList = await IList2.GetMeasurementUnitListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseOrderDetailPNCtlr",
                        WildCard = null,
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseOrderDetailPRNCtlr",
                        WildCard = null,
                        LoadByCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Order Import", Operation = "CanView")]
        public IActionResult PurchaseOrderImportIndex()
        {
            return View();
        }

        #region Master Import

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Order Import", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderImportMasterLoad([FromServices] IPurchaseOrder db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null, bool IsCanViewOnlyOwnData = false)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseOrderImportMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name, IsCanViewOnlyOwnData);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Order Import", Operation = "CanPost")]
        public async Task<string> PurchaseOrderImportMasterPost([FromServices] IPurchaseOrder db, [FromBody] tbl_Inv_PurchaseOrderMaster tbl_Inv_PurchaseOrderMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseOrderMaster(tbl_Inv_PurchaseOrderMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Order Import", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderImportMasterGet([FromServices] IPurchaseOrder db, int ID)
        {
            return Json(await db.GetPurchaseOrderImportMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Import

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Order Import", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderImportDetailLoad([FromServices] IPurchaseOrder db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseOrderImportDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Order Import", Operation = "CanPost")]
        public async Task<string> PurchaseOrderImportDetailPost([FromServices] IPurchaseOrder db, [FromBody] tbl_Inv_PurchaseOrderDetail tbl_Inv_PurchaseOrderDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseOrderDetail(tbl_Inv_PurchaseOrderDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Order Import", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderImportDetailGet([FromServices] IPurchaseOrder db, int ID)
        {
            return Json(await db.GetPurchaseOrderImportDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        //---------------Common in Local & Import----------------//
        #region Detail PurchaseNote

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Order", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderDetailPNLoad([FromServices] IPurchaseOrder db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseOrderDetailPN(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail PurchaseReturnNote

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Order", Operation = "CanView")]
        public async Task<IActionResult> PurchaseOrderDetailPRNLoad([FromServices] IPurchaseOrder db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseOrderDetailPRN(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Order", Operation = "CanView")]
        public async Task<string> EmailPO([FromServices] IPurchaseOrder iPO, [FromServices] IUser iUser, int ID = 0, string ContactPersonName = "", string Email = "", string AcName = "", string TargetDate = "", string PONo = "")
        {
            

            return await Task.Run(async () => {

                try
                {
                    if (string.IsNullOrEmpty(Rpt_Shared.LicenseToEmail) || string.IsNullOrEmpty(Rpt_Shared.LicenseToEmailPswd) || string.IsNullOrEmpty(Rpt_Shared.LicenseToEmailHostName) 
                       || Rpt_Shared.LicenseToEmailPortNo <= 0  || string.IsNullOrEmpty(Email)
                    )
                        return "Unable to send mail: Email Not Configured";

                    string EmailSignature = await iUser.GetUserEmailSignatureAsync(User.Identity.Name);

                    var message = new MimeMessage();

                    message.From.Add(new MailboxAddress(Rpt_Shared.LicenseTo, Rpt_Shared.LicenseToEmail));

                    message.To.Add(new MailboxAddress(ContactPersonName, Email));

                    message.Subject = "Purchase Order #" + PONo + " Target Date " + TargetDate;

                    var builder = new BodyBuilder();


                    builder.HtmlBody = "<b>Dear " + (string.IsNullOrEmpty(ContactPersonName) ? AcName : ContactPersonName) + "</b><br>" + "Please find the Purchase Order attached with the mail for detail"
                        + "." +
                        "<br>" + "Note: This is system generated email doesnot required signature." +
                        "<hr>" + (string.IsNullOrEmpty(EmailSignature) ? Rpt_Shared.LicenseToEmailFooter.Replace("@whatsapp", "This is " + ContactPersonName + " ") : EmailSignature);

                    builder.Attachments.Add("PO# " + PONo, new MemoryStream(await iPO.GetPDFFileAsync("Current Purchase Order", ID, 0, 0, DateTime.Now, DateTime.Now, "", "", "", "", 0)).ToArray(), new ContentType("application", "pdf"));


                    message.Body = builder.ToMessageBody();

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(Rpt_Shared.LicenseToEmailHostName, Rpt_Shared.LicenseToEmailPortNo, SecureSocketOptions.Auto);
                        await client.AuthenticateAsync(Rpt_Shared.LicenseToEmail, Rpt_Shared.LicenseToEmailPswd);

                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);

                    }
                    return "OK";
                }
                catch (Exception ex)
                {
                    return "Some thing went wrong while sending email! Contact System Administrator";
                }

            });


        }

        #region Report

        [MyAuthorization(FormName = "Purchase Order", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPurchaseOrderReport([FromServices] IPurchaseOrder db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion
         

        #endregion

        #region PurchaseRequest

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseRequestAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseRequest db2, [FromServices] IInventoryList db3, [FromServices] IIdentityList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseRequestMasterCtlr",
                        WildCard = db2.GetWCLPurchaseRequestMaster(),
                        LoadByCard = db2.GetWCLBPurchaseRequestMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Purchase Request"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseRequestDetailCtlr",
                        WildCard = db2.GetWCLPurchaseRequestDetail(),
                        LoadByCard = db2.GetWCLBPurchaseRequestDetail(),
                        Reports = db2.GetRLPurchaseRequest(),
                        Privilege = null,
                        Otherdata = new { AspNetOreasPriorityList = await IList.GetAspNetOreasPriorityListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Request", Operation = "CanView")]
        public IActionResult PurchaseRequestIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Request", Operation = "CanView")]
        public async Task<IActionResult> PurchaseRequestMasterLoad([FromServices] IPurchaseRequest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseRequestMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Request", Operation = "CanPost")]
        public async Task<string> PurchaseRequestMasterPost([FromServices] IPurchaseRequest db, [FromBody] tbl_Inv_PurchaseRequestMaster tbl_Inv_PurchaseRequestMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseRequestMaster(tbl_Inv_PurchaseRequestMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Request", Operation = "CanView")]
        public async Task<IActionResult> PurchaseRequestMasterGet([FromServices] IPurchaseRequest db, int ID)
        {
            return Json(await db.GetPurchaseRequestMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Request", Operation = "CanView")]
        public async Task<IActionResult> PurchaseRequestDetailLoad([FromServices] IPurchaseRequest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseRequestDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Request", Operation = "CanPost")]
        public async Task<string> PurchaseRequestDetailPost([FromServices] IPurchaseRequest db, [FromBody] tbl_Inv_PurchaseRequestDetail tbl_Inv_PurchaseRequestDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseRequestDetail(tbl_Inv_PurchaseRequestDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Request", Operation = "CanView")]
        public async Task<IActionResult> PurchaseRequestDetailGet([FromServices] IPurchaseRequest db, int ID)
        {
            return Json(await db.GetPurchaseRequestDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Report

        [MyAuthorization(FormName = "Purchase Request", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPurchaseRequestReport([FromServices] IPurchaseRequest db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region PurchaseRequestBids

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseRequestBidsAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseRequestBids db2, [FromServices] IInventoryList db3, [FromServices] IIdentityList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseRequestBidsMasterCtlr",
                        WildCard = db2.GetWCLPurchaseRequestBidsMaster(),
                        LoadByCard = db2.GetWCLBPurchaseRequestBidsMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Purchase Request Bids"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseRequestBidsDetailCtlr",
                        WildCard = db2.GetWCLPurchaseRequestBidsDetail(),
                        LoadByCard = db2.GetWCLBPurchaseRequestBidsDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { 
                            AspNetOreasPriorityList = await IList.GetAspNetOreasPriorityListAsync(null,null),
                            ManufacturerPOList = await db3.GetPOManufacturerListAsync(null,null),
                            IsPurchaseRequestApprover = await db2.IsPurchaseRequestAprrover(User.Identity.Name)
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Request Bids", Operation = "CanView")]
        public IActionResult PurchaseRequestBidsIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Request Bids", Operation = "CanView")]
        public async Task<IActionResult> PurchaseRequestBidsMasterLoad([FromServices] IPurchaseRequestBids db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseRequestBidsMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [MyAuthorization(FormName = "Purchase Request Bids", Operation = "CanView")]
        public async Task<IActionResult> PurchaseRequestBidsMasterGet([FromServices] IPurchaseRequestBids db, int ID)
        {
            return Json(await db.GetPurchaseRequestBidsMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Request Bids", Operation = "CanView")]
        public async Task<IActionResult> PurchaseRequestBidsDetailLoad([FromServices] IPurchaseRequestBids db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseRequestBidsDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Request Bids", Operation = "CanPost")]
        public async Task<string> PurchaseRequestBidsDetailPost([FromServices] IPurchaseRequestBids db, [FromBody] tbl_Inv_PurchaseRequestDetail_Bids tbl_Inv_PurchaseRequestDetail_Bids, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseRequestBidsDetail(tbl_Inv_PurchaseRequestDetail_Bids, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Request Bids", Operation = "CanPost")]
        public async Task<string> PurchaseRequestBidsDetailPostDecision([FromServices] IPurchaseRequestBids db, [FromBody] tbl_Inv_PurchaseRequestDetail_Bids tbl_Inv_PurchaseRequestDetail_Bids, int ID = 0, bool? Decision = null, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseRequestBidsDecision(tbl_Inv_PurchaseRequestDetail_Bids, ID, Decision, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        [MyAuthorization(FormName = "Purchase Request Bids", Operation = "CanView")]
        public async Task<IActionResult> PurchaseRequestBidsDetailGet([FromServices] IPurchaseRequestBids db, int ID)
        {
            return Json(await db.GetPurchaseRequestBidsDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Request Bids", Operation = "CanView")]
        public async Task<IActionResult> GetPOSuggestions([FromServices] IPurchaseRequestBids db, int ProdID)
        {
            return Json(await db.GetPOSuggestions(ProdID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region Supplier

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedSupplierAsync([FromServices] IAuthorizationScheme db, [FromServices] ISupplier db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SupplierIndexCtlr",
                        WildCard = db2.GetWCLSupplier(),
                        Reports = db2.GetRLSupplierMaster(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Supplier"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "SupplierEvaluationCtlr",
                        WildCard = null,
                        Reports = db2.GetRLSupplierDetail(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Supplier", Operation = "CanView")]
        public IActionResult SupplierIndex()
        {

            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Supplier", Operation = "CanView")]
        public async Task<IActionResult> SupplierLoad([FromServices] ISupplier db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadSupplier(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Supplier", Operation = "CanPost")]
        public async Task<string> SupplierPost([FromServices] ISupplier db, [FromBody] tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostSupplier(tbl_Ac_ChartOfAccounts, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Supplier", Operation = "CanView")]
        public async Task<IActionResult> SupplierGet([FromServices] ISupplier db, int ID)
        {
            return Json(await db.GetSupplier(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #region Report

        [MyAuthorization(FormName = "Order Note", Operation = "CanViewReport")]
        public async Task<IActionResult> GetSupplierReport([FromServices] ISupplier db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region CustomerSubDistributorList

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCustomerSubDistributorListAsync([FromServices] IAuthorizationScheme db, [FromServices] ICustomerSubDistributorList db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CustomerSubDistributorListCtlr",
                        WildCard = db2.GetWCLCustomerSubDistributorListMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Customer Sub Distributor List"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CustomerSubDistributorListDetailCtlr",
                        WildCard = db2.GetWCLCustomerSubDistributorListDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Customer Sub Distributor List", Operation = "CanView")]
        public IActionResult CustomerSubDistributorListIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Customer Sub Distributor List", Operation = "CanView")]
        public async Task<IActionResult> CustomerSubDistributorListLoad([FromServices] ICustomerSubDistributorList db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCustomerSubDistributorListMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Customer Sub Distributor List", Operation = "CanView")]
        public async Task<IActionResult> CustomerSubDistributorListDetailLoad([FromServices] ICustomerSubDistributorList db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCustomerSubDistributorListDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Customer Sub Distributor List", Operation = "CanPost")]
        public async Task<string> CustomerSubDistributorListDetailPost([FromServices] ICustomerSubDistributorList db, [FromBody] tbl_Ac_CustomerSubDistributorList tbl_Ac_CustomerSubDistributorList, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCustomerSubDistributorListDetail(tbl_Ac_CustomerSubDistributorList, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Customer Sub Distributor List", Operation = "CanView")]
        public async Task<IActionResult> CustomerSubDistributorListDetailGet([FromServices] ICustomerSubDistributorList db, int ID)
        {
            return Json(await db.GetCustomerSubDistributorListDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion
    }
}
