using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using OreasModel;
using System;

namespace OreasCore.Areas.QA.Controllers
{
    [Area("QA")]
    public class BMRController : Controller
    {

        #region BMR
        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBMRAsync([FromServices] IAuthorizationScheme db, [FromServices] IBMR db2, [FromServices] IInventoryList db3, [FromServices] IProductionList db4)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRMasterCtlr",
                        WildCard =  db2.GetWCLBMRMaster(),
                        LoadByCard = db2.GetWCLBBMRMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QA", User.Identity.Name, "BMR"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDetailRawMasterCtlr",
                        WildCard =  db2.GetWCLBMRDetailRawMaster(),
                        Reports = db2.GetRLBMRDetail(),
                        Privilege = null,
                        Otherdata = new {
                            CompositionFilterList = await db4.GetCompositionFilterPolicyListAsync(true)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDetailRawDetailItemCtlr",
                        WildCard =  db2.GetWCLBMRDetailRawDetailItem(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRProcessCtlr",
                        WildCard =  db2.GetWCLBMRProcess(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            BMRProcedureList = await db4.GetProProcedureListAsync("byBMRBPR","BMR")
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDetailPackagingMasterCtlr",
                        WildCard =  db2.GetWCLBMRDetailPackagingMaster(),
                        Reports = db2.GetRLBMRDetail(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDetailPackagingDetailFilterCtlr",
                        WildCard =  db2.GetWCLBMRDetailPackagingDetailFilter(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            CompositionFilterList = await db4.GetCompositionFilterPolicyListAsync(false)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRDetailPackagingDetailFilterDetailItemCtlr",
                        WildCard =  db2.GetWCLBMRDetailPackagingDetailFilterDetailItem(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BPRProcessCtlr",
                        WildCard =  db2.GetWCLBMRProcess(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            BPRProcedureList = await db4.GetProProcedureListAsync("byBMRBPR","BPR")
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public IActionResult BMRIndex()
        {
            return View();
        }

        #region BMRMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRMasterLoad([FromServices] IBMR db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BMRMasterPost([FromServices] IBMR db, [FromBody] tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster, string operation = "")
        {
            ModelState.Remove("BatchExpiryDate"); ModelState.Remove("FK_tbl_Inv_MeasurementUnit_ID_Dimension"); ModelState.Remove("FK_tbl_Pro_CompositionMaster_ID");
            if (ModelState.IsValid)
                return await db.PostBMRMaster(tbl_Pro_BatchMaterialRequisitionMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRMasterGet([FromServices] IBMR db, int ID)
        {
            return Json(await db.GetBMRMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDetailRawMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailRawMasterLoad([FromServices] IBMR db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDetailRawMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BMRDetailRawMasterPost([FromServices] IBMR db, [FromBody] tbl_Pro_BatchMaterialRequisitionDetail_RawMaster tbl_Pro_BatchMaterialRequisitionDetail_RawMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRDetailRawMaster(tbl_Pro_BatchMaterialRequisitionDetail_RawMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailRawMasterGet([FromServices] IBMR db, int ID)
        {
            return Json(await db.GetBMRDetailRawMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDetailRawDetailItem

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailRawDetailItemLoad([FromServices] IBMR db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDetailRawDetailItem(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BMRDetailRawDetailItemPost([FromServices] IBMR db, [FromBody] tbl_Pro_BatchMaterialRequisitionDetail_RawDetail tbl_Pro_BatchMaterialRequisitionDetail_RawDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRDetailRawDetailItem(tbl_Pro_BatchMaterialRequisitionDetail_RawDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailRawDetailItemGet([FromServices] IBMR db, int ID)
        {
            return Json(await db.GetBMRDetailRawDetailItem(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDetailPackagingMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailPackagingMasterLoad([FromServices] IBMR db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDetailPackagingMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BMRDetailPackagingMasterPost([FromServices] IBMR db, [FromBody] tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster, string operation = "")
        {            

            if (ModelState.IsValid)
                return await db.PostBMRDetailPackagingMaster(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailPackagingMasterGet([FromServices] IBMR db, int ID)
        {
            return Json(await db.GetBMRDetailPackagingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDetailPackagingDetailFilter

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailPackagingDetailFilterLoad([FromServices] IBMR db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDetailPackagingDetailFilter(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BMRDetailPackagingDetailFilterPost([FromServices] IBMR db, [FromBody] tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRDetailPackagingDetailFilter(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailPackagingDetailFilterGet([FromServices] IBMR db, int ID)
        {
            return Json(await db.GetBMRDetailPackagingDetailFilter(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRDetailPackagingDetailFilterDetailItem

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailPackagingDetailFilterDetailItemLoad([FromServices] IBMR db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRDetailPackagingDetailFilterDetailItem(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BMRDetailPackagingDetailFilterDetailItemPost([FromServices] IBMR db, [FromBody] tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRDetailPackagingDetailFilterDetailItem(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRDetailPackagingDetailFilterDetailItemGet([FromServices] IBMR db, int ID)
        {
            return Json(await db.GetBMRDetailPackagingDetailFilterDetailItem(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion                

        #region Stock Reservatiton

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BMRStockIssuanceReservationItemPost([FromServices] IBMR db, int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostStockIssuanceReservation(BMR_RawItemID, BMR_PackagingItemID, BMR_AdditionalItemID, OR_ItemID, false, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }
        #endregion

        #region BMRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRProcessLoad([FromServices] IBMR db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRProcess(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BMRProcessPost([FromServices] IBMR db, [FromBody] tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRProcess(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BMRProcessGet([FromServices] IBMR db, int ID)
        {
            return Json(await db.GetBMRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BPRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BPRProcessLoad([FromServices] IBMR db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBPRProcess(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR", Operation = "CanPost")]
        public async Task<string> BPRProcessPost([FromServices] IBMR db, [FromBody] tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRProcess(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR", Operation = "CanView")]
        public async Task<IActionResult> BPRProcessGet([FromServices] IBMR db, int ID)
        {
            return Json(await db.GetBPRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "BMR", Operation = "CanViewReport")]
        public async Task<IActionResult> GetBMRReport([FromServices] IBMR db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region BMRAdditional

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBMRAdditionalAsync([FromServices] IAuthorizationScheme db, [FromServices] IBMRAdditional db2, [FromServices] IProductionList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRAdditionalMasterCtlr",
                        WildCard = db2.GetWCLBMRAdditionalMaster(),
                        LoadByCard = db2.GetWCLBBMRAdditionalMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QA", User.Identity.Name, "BMR Additional"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRAdditionalDetailCtlr",
                        WildCard = db2.GetWCLBMRAdditionalDetail(),
                        Reports = db2.GetRLBMRAdditional(),
                        Privilege = null,
                        Otherdata = new { BMRAdditionalTypeList = await db3.GetBMRAdditionalTypeListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "BMR Additional", Operation = "CanView")]
        public IActionResult BMRAdditionalIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> LoadBatchNoList([FromServices] IBMRAdditional db, [FromServices] IUser user, string FilterBy = "BatchNo", string FilterValue = "")
        {   
            return Json(
                await db.GetBatchNoList(FilterBy, FilterValue)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Additional", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalMasterLoad([FromServices] IBMRAdditional db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRAdditionalMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR Additional", Operation = "CanPost")]
        public async Task<string> BMRAdditionalMasterPost([FromServices] IBMRAdditional db, [FromBody] tbl_Pro_BMRAdditionalMaster tbl_Pro_BMRAdditionalMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRAdditionalMaster(tbl_Pro_BMRAdditionalMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR Additional", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalMasterGet([FromServices] IBMRAdditional db, int ID)
        {
            return Json(await db.GetBMRAdditionalMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR Additional", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalDetailLoad([FromServices] IBMRAdditional db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRAdditionalDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR Additional", Operation = "CanPost")]
        public async Task<string> BMRAdditionalDetailPost([FromServices] IBMRAdditional db, [FromBody] tbl_Pro_BMRAdditionalDetail tbl_Pro_BMRAdditionalDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRAdditionalDetail(tbl_Pro_BMRAdditionalDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR Additional", Operation = "CanView")]
        public async Task<IActionResult> BMRAdditionalDetailGet([FromServices] IBMRAdditional db, int ID)
        {
            return Json(await db.GetBMRAdditionalDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "BMR Additional", Operation = "CanViewReport")]
        public async Task<IActionResult> GetBMRAdditionalReport([FromServices] IBMRAdditional db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion      

        #region ProductionTransfer

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedProductionTransferAsync([FromServices] IAuthorizationScheme db, [FromServices] IQAProductionTransfer db2, [FromServices] IInventoryList db3)
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
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Production", User.Identity.Name, "Production Transfer"),
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
        public async Task<IActionResult> ProductionTransferMasterLoad([FromServices] IQAProductionTransfer db,
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
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Production Transfer", Operation = "CanView")]
        public async Task<IActionResult> ProductionTransferDetailLoad([FromServices] IQAProductionTransfer db,
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
        public async Task<string> ProductionTransferDetailPost([FromServices] IQAProductionTransfer db, [FromBody] tbl_Pro_ProductionTransferDetail tbl_Pro_ProductionTransferDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostProductionTransferDetail(tbl_Pro_ProductionTransferDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Production Transfer", Operation = "CanView")]
        public async Task<IActionResult> ProductionTransferDetailGet([FromServices] IQAProductionTransfer db, int ID)
        {
            return Json(await db.GetProductionTransferDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        

        #endregion
    }
}
