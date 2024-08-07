using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

namespace OreasCore.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class DispensingController : Controller
    {

        #region OrdinaryRequisitionDispensing

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedOrdinaryRequisitionDispensingAsync([FromServices] IAuthorizationScheme db, [FromServices] IOrdinaryRequisitionDispensing db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrdinaryRequisitionDispensingMasterCtlr",
                        WildCard = db2.GetWCLOrdinaryRequisitionDispensingMaster(),
                        LoadByCard = db2.GetWCLBOrdinaryRequisitionDispensingMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Ordinary Requisition Dispensing"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrdinaryRequisitionDispensingDetailCtlr",
                        WildCard = db2.GetWCLOrdinaryRequisitionDispensingDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrdinaryRequisitionDispensingDetailDispensingCtlr",
                        WildCard = db2.GetWCLOrdinaryRequisitionDispensingDetail(),
                        Reports = db2.GetRLOrdinaryRequisitionDispensing(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanView")]
        public IActionResult OrdinaryRequisitionDispensingIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionDispensingMasterLoad([FromServices] IOrdinaryRequisitionDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadOrdinaryRequisitionDispensingMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad,User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionDispensingMasterGet([FromServices] IOrdinaryRequisitionDispensing db, int ID)
        {
            return Json(await db.GetOrdinaryRequisitionDispensingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionDispensingDetailLoad([FromServices] IOrdinaryRequisitionDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadOrdinaryRequisitionDispensingDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionDispensingDetailGet([FromServices] IOrdinaryRequisitionDispensing db, int ID)
        {
            return Json(await db.GetOrdinaryRequisitionDispensingDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
        
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanPost")]
        public async Task<string> OrdinaryRequisitionDispensingDetailPost([FromServices] IOrdinaryRequisitionDispensing db, [FromBody] tbl_Inv_OrdinaryRequisitionDetail tbl_Inv_OrdinaryRequisitionDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostOrdinaryRequisitionDispensingDetail(tbl_Inv_OrdinaryRequisitionDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #endregion

        #region Dispensing

        [AjaxOnly]
        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionDispensingDetailDispensingLoad([FromServices] IOrdinaryRequisitionDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadOrdinaryRequisitionDispensingDetailDispensing(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanPost")]
        public async Task<string> OrdinaryRequisitionDispensingDetailDispensingPost([FromServices] IOrdinaryRequisitionDispensing db, [FromBody] tbl_Inv_OrdinaryRequisitionDispensing tbl_Inv_OrdinaryRequisitionDispensing, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostOrdinaryRequisitionDispensingDetailDispensing(tbl_Inv_OrdinaryRequisitionDispensing, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionDispensingDetailDispensingGet([FromServices] IOrdinaryRequisitionDispensing db, int ID)
        {
            return Json(await db.GetOrdinaryRequisitionDispensingDetailDispensing(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanPost")]
        public async Task<string> OrdinaryStockIssuanceReservationItemPost([FromServices] IOrdinaryRequisitionDispensing db, int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostStockIssuanceReservation(BMR_RawItemID, BMR_PackagingItemID, BMR_AdditionalItemID, OR_ItemID, true, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #region Report

        [MyAuthorization(FormName = "Ordinary Requisition Dispensing", Operation = "CanViewReport")]
        public async Task<IActionResult> GetOrdinaryRequisitionDispensingReport([FromServices] IOrdinaryRequisitionDispensing db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region BMRAdditionalDispensing

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBMRAdditionalDispensingAsync([FromServices] IAuthorizationScheme db, [FromServices] IBMRAdditionalDispensing db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRAdditionalDispensingMasterCtlr",
                        WildCard = db2.GetWCLBMRAdditionalDispensingMaster(),
                        LoadByCard = db2.GetWCLBBMRAdditionalDispensingMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "BMR Additional Dispensing"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRAdditionalDispensingDetailCtlr",
                        WildCard = db2.GetWCLBMRAdditionalDispensingDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRAdditionalDispensingDetailDispensingCtlr",
                        WildCard = db2.GetWCLBMRAdditionalDispensingDetail(),
                        Reports = db2.GetRLBMRAdditionalRequisitionDetailDispensing(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanView")]
        public IActionResult BMRAdditionalDispensingIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalDispensingMasterLoad([FromServices] IBMRAdditionalDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRAdditionalDispensingMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalDispensingMasterGet([FromServices] IBMRAdditionalDispensing db, int ID)
        {
            return Json(await db.GetBMRAdditionalDispensingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalDispensingDetailLoad([FromServices] IBMRAdditionalDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRAdditionalDispensingDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalDispensingDetailGet([FromServices] IBMRAdditionalDispensing db, int ID)
        {
            return Json(await db.GetBMRAdditionalDispensingDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanPost")]
        public async Task<string> BMRAdditionalDispensingDetailPost([FromServices] IBMRAdditionalDispensing db, [FromBody] tbl_Pro_BMRAdditionalDetail tbl_Pro_BMRAdditionalDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRAdditionalDispensingDetail(tbl_Pro_BMRAdditionalDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #endregion

        #region Dispensing

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalDispensingDetailDispensingLoad([FromServices] IBMRAdditionalDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRAdditionalDispensingDetailDispensing(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanPost")]
        public async Task<string> BMRAdditionalDispensingDetailDispensingPost([FromServices] IBMRAdditionalDispensing db, [FromBody] tbl_Inv_BMRAdditionalDispensing tbl_Inv_BMRAdditionalDispensing, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRAdditionalDispensingDetailDispensing(tbl_Inv_BMRAdditionalDispensing, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalDispensingDetailDispensingGet([FromServices] IBMRAdditionalDispensing db, int ID)
        {
            return Json(await db.GetBMRAdditionalDispensingDetailDispensing(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanPost")]
        public async Task<string> BMRAdditionalStockIssuanceReservationItemPost([FromServices] IBMRAdditionalDispensing db, int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostStockIssuanceReservation(BMR_RawItemID, BMR_PackagingItemID, BMR_AdditionalItemID, OR_ItemID, true, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        #region Report

        [MyAuthorization(FormName = "BMR Additional Dispensing", Operation = "CanViewReport")]
        public async Task<IActionResult> GetBMRAdditionalDispensingDetailDispensingReport([FromServices] IBMRAdditionalDispensing db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region BMRDispensing

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBMRDispensingAsync([FromServices] IAuthorizationScheme db, [FromServices] IBMRDispensing db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDispensingMasterCtlr",
                        WildCard = db2.GetWCLBMRDispensingMaster(),
                        LoadByCard = db2.GetWCLBBMRDispensingMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "BMR Dispensing"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDispensingDetailRawItemsCtlr",
                        WildCard = db2.GetWCLBMRDispensingDetailRawItems(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDispensingRawCtlr",
                        WildCard = db2.GetWCLBMRDispensingRaw(),
                        Reports = db2.GetRLBMRDispensingRaw(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDispensingDetailPackagingDetailItemsCtlr",
                        WildCard = db2.GetWCLBMRDispensingDetailPackagingDetailItems(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDispensingPackagingCtlr",
                        WildCard = db2.GetWCLBMRDispensingPackaging(),
                        Reports = db2.GetRLBMRDispensingPackaging(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public IActionResult BMRDispensingIndex()
        {
            return View();
        }

        #region BMRDispensingMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingMasterLoad([FromServices] IBMRDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDispensingMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingMasterGet([FromServices] IBMRDispensing db, int ID)
        {
            return Json(await db.GetBMRDispensingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDispensingDetailRawItems

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingDetailRawItemsLoad([FromServices] IBMRDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDispensingDetailRawItems(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingDetailRawItemsGet([FromServices] IBMRDispensing db, int ID)
        {
            return Json(await db.GetBMRDispensingDetailRawItems(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region BMRDispensingRaw

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingRawLoad([FromServices] IBMRDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDispensingRaw(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanPost")]
        public async Task<string> BMRDispensingRawPost([FromServices] IBMRDispensing db, [FromBody] tbl_Inv_BMRDispensingRaw tbl_Inv_BMRDispensingRaw, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRDispensingRaw(tbl_Inv_BMRDispensingRaw, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingRawGet([FromServices] IBMRDispensing db, int ID)
        {
            return Json(await db.GetBMRDispensingRaw(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDispensingDetailPackagingDetailItems

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingDetailPackagingDetailItemsLoad([FromServices] IBMRDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDispensingDetailPackagingDetailItems(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingDetailPackagingDetailItemsGet([FromServices] IBMRDispensing db, int ID)
        {
            return Json(await db.GetBMRDispensingDetailPackagingDetailItems(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDispensingPackaging

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingPackagingLoad([FromServices] IBMRDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDispensingPackaging(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanPost")]
        public async Task<string> BMRDispensingPackagingPost([FromServices] IBMRDispensing db, [FromBody] tbl_Inv_BMRDispensingPackaging tbl_Inv_BMRDispensingPackaging, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRDispensingPackaging(tbl_Inv_BMRDispensingPackaging, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanView")]
        public async Task<IActionResult> BMRDispensingPackagingGet([FromServices] IBMRDispensing db, int ID)
        {
            return Json(await db.GetBMRDispensingPackaging(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanPost")]
        public async Task<string> BMRStockIssuanceReservationItemPost([FromServices] IBMRDispensing db, int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, string operation = "")
        {

            if (ModelState.IsValid)
                return await db.PostStockIssuanceReservation(BMR_RawItemID, BMR_PackagingItemID, BMR_AdditionalItemID, OR_ItemID, true, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #region Report

        [MyAuthorization(FormName = "BMR Dispensing", Operation = "CanViewReport")]
        public async Task<IActionResult> GetBMRDispensingReport([FromServices] IBMRDispensing db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region ProductionTransfer

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedProductionTransferAsync([FromServices] IAuthorizationScheme db, [FromServices] IInvProductionTransfer db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductionTransferMasterCtlr",
                        WildCard = db2.GetWCLProductionTransferMaster(),
                        LoadByCard = db2.GetWCLBProductionTransferMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Production Transfer"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductionTransferDetailCtlr",
                        WildCard = db2.GetWCLProductionTransferDetail(),
                        LoadByCard = db2.GetWCLBProductionTransferDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Production Transfer", Operation = "CanView")]
        public IActionResult ProductionTransferIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Production Transfer", Operation = "CanView")]
        public async Task<IActionResult> ProductionTransferMasterLoad([FromServices] IInvProductionTransfer db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductionTransferMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Production Transfer", Operation = "CanView")]
        public async Task<IActionResult> ProductionTransferDetailLoad([FromServices] IInvProductionTransfer db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductionTransferDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Production Transfer", Operation = "CanPost")]
        public async Task<string> ProductionTransferDetailPost([FromServices] IInvProductionTransfer db, [FromBody] tbl_Pro_ProductionTransferDetail tbl_Pro_ProductionTransferDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostProductionTransferDetail(tbl_Pro_ProductionTransferDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Production Transfer", Operation = "CanView")]
        public async Task<IActionResult> ProductionTransferDetailGet([FromServices] IInvProductionTransfer db, int ID)
        {
            return Json(await db.GetProductionTransferDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion



        #endregion

        #region PD Request Dispensing

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPDRequestAsync([FromServices] IAuthorizationScheme db, [FromServices] IPDRequestDispensing db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PDRequestCFPMasterCtlr",
                        WildCard = db2.GetWCLRequestCFPMaster(),
                        LoadByCard = db2.GetWCLBRequestCFPMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "PD Request"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PDRequestCFPDetailItemCtlr",
                        WildCard = db2.GetWCLRequestCFPDetail(),
                        LoadByCard = db2.GetWCLBRequestCFPDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PDRequestCFPDetailItemDispensingCtlr",
                        WildCard = null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "PD Request", Operation = "CanView")]
        public IActionResult PDRequestDispensingIndex()
        {
            return View();
        }

        #region PD Request CFP Master

        [AjaxOnly]
        [MyAuthorization(FormName = "PD Request", Operation = "CanView")]
        public async Task<IActionResult> PDRequestCFPMasterLoad([FromServices] IPDRequestDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            List<int> AuthStoreList = new List<int>();

            if (HttpContext.Request.Cookies.TryGetValue("AuthWareHouseList", out string serializedStoreList))
            {
                AuthStoreList = JsonConvert.DeserializeObject<List<int>>(serializedStoreList);
            }
            

            PagedData<object> pageddata =
                await db.LoadRequestCFPMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name, AuthStoreList);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PD Request CFP Detail Item

        [AjaxOnly]
        [MyAuthorization(FormName = "PD Request", Operation = "CanView")]
        public async Task<IActionResult> PDRequestCFPDetailItemLoad([FromServices] IPDRequestDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRequestCFPDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "PD Request", Operation = "CanView")]
        public async Task<IActionResult> PDRequestCFPDetailItemGet([FromServices] IPDRequestDispensing db, int ID)
        {
            return Json(await db.GetRequestCFPDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PD Request", Operation = "CanPost")]
        public async Task<string> PDRequestCFPDetailItemPost([FromServices] IPDRequestDispensing db, [FromBody] tbl_PD_RequestDetailTR_CFP_Item tbl_PD_RequestDetailTR_CFP_Item, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRequestCFPDetail(tbl_PD_RequestDetailTR_CFP_Item, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        #endregion

        #region PD Request CFP Detail Item Dispensing

        [AjaxOnly]
        [MyAuthorization(FormName = "PD Request", Operation = "CanView")]
        public async Task<IActionResult> PDRequestCFPDetailItemDispensingLoad([FromServices] IPDRequestDispensing db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRequestCFPDetailDispensing(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PD Request", Operation = "CanPost")]
        public async Task<string> PDRequestCFPDetailItemDispensingPost([FromServices] IPDRequestDispensing db, [FromBody] tbl_Inv_PDRequestDispensing tbl_Inv_PDRequestDispensing, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRequestCFPDetailDispensing(tbl_Inv_PDRequestDispensing, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PD Request", Operation = "CanView")]
        public async Task<IActionResult> PDRequestCFPDetailItemDispensingGet([FromServices] IPDRequestDispensing db, int ID)
        {
            return Json(await db.GetRequestCFPDetailDispensing(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion      

        #endregion

        #region StockTransfer Receiving

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedStockTransferReceivingAsync([FromServices] IAuthorizationScheme db, [FromServices] IStockTransfer db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "StockTransferReceivingMasterCtlr",
                        WildCard = db2.GetWCLStockTransferMaster(),
                        LoadByCard = db2.GetWCLBStockTransferMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Stock Transfer Receiving"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "StockTransferReceivingDetailCtlr",
                        WildCard = db2.GetWCLStockTransferDetail(),
                        LoadByCard = db2.GetWCLBStockTransferDetail(),
                        Reports = db2.GetRLStockTransfer(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Stock Transfer Receiving", Operation = "CanView")]
        public IActionResult StockTransferReceivingIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Stock Transfer Receiving", Operation = "CanView")]
        public async Task<IActionResult> StockTransferMasterReceivingLoad([FromServices] IStockTransfer db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadStockTransferMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [MyAuthorization(FormName = "Stock Transfer Receiving", Operation = "CanView")]
        public async Task<IActionResult> StockTransferMasterReceivingGet([FromServices] IStockTransfer db, int ID)
        {
            return Json(await db.GetStockTransferMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Stock Transfer Receiving", Operation = "CanView")]
        public async Task<IActionResult> StockTransferDetailReceivingLoad([FromServices] IStockTransfer db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadStockTransferDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Stock Transfer Receiving", Operation = "CanPost")]
        public async Task<string> StockTransferDetailReceivingPost([FromServices] IStockTransfer db, [FromBody] tbl_Inv_StockTransferDetail tbl_Inv_StockTransferDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostReceviedStockTransferDetail(tbl_Inv_StockTransferDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Stock Transfer Receiving", Operation = "CanView")]
        public async Task<IActionResult> StockTransferDetailReceivingGet([FromServices] IStockTransfer db, int ID)
        {
            return Json(await db.GetStockTransferDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Stock Transfer Receiving", Operation = "CanViewReport")]
        public async Task<IActionResult> GetStockTransferReceivingReport([FromServices] IStockTransfer db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion
    }
}
