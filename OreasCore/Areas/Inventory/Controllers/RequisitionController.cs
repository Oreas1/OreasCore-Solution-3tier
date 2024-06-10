using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OreasCore.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class RequisitionController : Controller
    {


        #region OrdinaryRequisition

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedOrdinaryRequisitionAsync([FromServices] IAuthorizationScheme db, [FromServices] IOrdinaryRequisition db2, [FromServices] IInventoryList db3, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrdinaryRequisitionMasterCtlr",
                        WildCard = db2.GetWCLOrdinaryRequisitionMaster(),
                        LoadByCard = db2.GetWCLBOrdinaryRequisitionMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Ordinary Requisition"),
                        Otherdata = new { SectionList = await IList.GetSectionListAsync("ByUser",null,null,0,User.Identity.Name) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OrdinaryRequisitionDetailCtlr",
                        WildCard = db2.GetWCLOrdinaryRequisitionDetail(),
                        Reports = db2.GetRLOrdinaryRequisition(),
                        Privilege = null,
                        Otherdata = new { OrdinaryRequisitionTypeList = await db3.GetOrdinaryRequisitionTypeListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Ordinary Requisition", Operation = "CanView")]
        public IActionResult OrdinaryRequisitionIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Ordinary Requisition", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionMasterLoad([FromServices] IOrdinaryRequisition db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null, bool IsCanViewOnlyOwnData = false)
        {
            PagedData<object> pageddata =
                await db.LoadOrdinaryRequisitionMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name, IsCanViewOnlyOwnData);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Ordinary Requisition", Operation = "CanPost")]
        public async Task<string> OrdinaryRequisitionMasterPost([FromServices] IOrdinaryRequisition db, [FromBody] tbl_Inv_OrdinaryRequisitionMaster tbl_Inv_OrdinaryRequisitionMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostOrdinaryRequisitionMaster(tbl_Inv_OrdinaryRequisitionMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Ordinary Requisition", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionMasterGet([FromServices] IOrdinaryRequisition db, int ID)
        {
            return Json(await db.GetOrdinaryRequisitionMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Ordinary Requisition", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionDetailLoad([FromServices] IOrdinaryRequisition db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadOrdinaryRequisitionDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Ordinary Requisition", Operation = "CanPost")]
        public async Task<string> OrdinaryRequisitionDetailPost([FromServices] IOrdinaryRequisition db, [FromBody] tbl_Inv_OrdinaryRequisitionDetail tbl_Inv_OrdinaryRequisitionDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostOrdinaryRequisitionDetail(tbl_Inv_OrdinaryRequisitionDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Ordinary Requisition", Operation = "CanView")]
        public async Task<IActionResult> OrdinaryRequisitionDetailGet([FromServices] IOrdinaryRequisition db, int ID)
        {
            return Json(await db.GetOrdinaryRequisitionDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Ordinary Requisition", Operation = "CanViewReport")]
        public async Task<IActionResult> GetOrdinaryRequisitionReport([FromServices] IOrdinaryRequisition db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }
        #endregion

        #endregion

        #region StockTransfer

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedStockTransferAsync([FromServices] IAuthorizationScheme db, [FromServices] IStockTransfer db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "StockTransferMasterCtlr",
                        WildCard = db2.GetWCLStockTransferMaster(),
                        LoadByCard = db2.GetWCLBStockTransferMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Stock Transfer"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "StockTransferDetailCtlr",
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

        [MyAuthorization(FormName = "Stock Transfer", Operation = "CanView")]
        public IActionResult StockTransferIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Stock Transfer", Operation = "CanView")]
        public async Task<IActionResult> StockTransferMasterLoad([FromServices] IStockTransfer db,
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

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Stock Transfer", Operation = "CanPost")]
        public async Task<string> StockTransferMasterPost([FromServices] IStockTransfer db, [FromBody] tbl_Inv_StockTransferMaster tbl_Inv_StockTransferMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostStockTransferMaster(tbl_Inv_StockTransferMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Stock Transfer", Operation = "CanView")]
        public async Task<IActionResult> StockTransferMasterGet([FromServices] IStockTransfer db, int ID)
        {
            return Json(await db.GetStockTransferMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Stock Transfer", Operation = "CanView")]
        public async Task<IActionResult> StockTransferDetailLoad([FromServices] IStockTransfer db,
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
        [MyAuthorization(FormName = "Stock Transfer", Operation = "CanPost")]
        public async Task<string> StockTransferDetailPost([FromServices] IStockTransfer db, [FromBody] tbl_Inv_StockTransferDetail tbl_Inv_StockTransferDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostStockTransferDetail(tbl_Inv_StockTransferDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Stock Transfer", Operation = "CanView")]
        public async Task<IActionResult> StockTransferDetailGet([FromServices] IStockTransfer db, int ID)
        {
            return Json(await db.GetStockTransferDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Stock Transfer", Operation = "CanViewReport")]
        public async Task<IActionResult> GetStockTransferReport([FromServices] IStockTransfer db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

    }

}
