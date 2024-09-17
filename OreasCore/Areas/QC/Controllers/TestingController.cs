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

namespace OreasCore.Areas.Qc.Controllers
{
    [Area("QC")]
    public class TestingController : Controller
    {

        #region Composition Qc Testing

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCompositionAsync([FromServices] IAuthorizationScheme db, [FromServices] ICompositionQcTest db2, [FromServices] IInventoryList db3, [FromServices] IProductionList db4)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionMasterCtlr",
                        WildCard =  db2.GetWCLCompositionMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QC", User.Identity.Name, "Composition Testing"),
                        Otherdata = new {
                            MeasurementUnitList = await db3.GetMeasurementUnitListAsync(null,null),
                            QcTestList = await db4.GetQcTestListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionPackagingMasterCtlr",
                        WildCard =  db2.GetWCLCompositionPackagingMaster(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionBMRProcessCtlr",
                        WildCard =  db2.GetWCLBMRProcess(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            BMRProcedureList = await db4.GetProProcedureListAsync("byBMRBPR","BMR")
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionBMRProcessQcTestCtlr",
                        WildCard =  db2.GetWCLBMRProcess(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },                   
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionBPRProcessCtlr",
                        WildCard =  db2.GetWCLBMRProcess(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            BPRProcedureList = await db4.GetProProcedureListAsync("byBMRBPR","BPR")
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionBPRProcessQcTestCtlr",
                        WildCard =  db2.GetWCLBMRProcess(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }

                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public IActionResult CompositionIndex()
        {
            return View();
        }

        #region Composition Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionMasterLoad([FromServices] ICompositionQcTest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionMasterGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetCompositionMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Composition BMRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionBMRProcessLoad([FromServices] ICompositionQcTest db,
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
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanPost")]
        public async Task<string> CompositionBMRProcessPost([FromServices] ICompositionQcTest db, [FromBody] tbl_Pro_CompositionMaster_ProcessBMR tbl_Pro_CompositionMaster_ProcessBMR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRProcess(tbl_Pro_CompositionMaster_ProcessBMR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionBMRProcessGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetBMRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition BMRProcess QcTest

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionBMRProcessQcTestLoad([FromServices] ICompositionQcTest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRProcessQcTest(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanPost")]
        public async Task<string> CompositionBMRProcessQcTestPost([FromServices] ICompositionQcTest db, [FromBody] tbl_Pro_CompositionMaster_ProcessBMR_QcTest tbl_Pro_CompositionMaster_ProcessBMR_QcTest, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRProcessQcTest(tbl_Pro_CompositionMaster_ProcessBMR_QcTest, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionBMRProcessQcTestGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetBMRProcessQcTest(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region CompositionPackagingMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionPackagingMasterLoad([FromServices] ICompositionQcTest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionPackagingMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionPackagingMasterGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetCompositionPackagingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition BPRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionBPRProcessLoad([FromServices] ICompositionQcTest db,
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
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanPost")]
        public async Task<string> CompositionBPRProcessPost([FromServices] ICompositionQcTest db, [FromBody] tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRProcess(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionBPRProcessGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetBPRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Composition BPRProcess QcTest

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionBPRProcessQcTestLoad([FromServices] ICompositionQcTest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBPRProcessQcTest(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition Testing", Operation = "CanPost")]
        public async Task<string> CompositionBPRProcessQcTestPost([FromServices] ICompositionQcTest db, [FromBody] tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRProcessQcTest(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition Testing", Operation = "CanView")]
        public async Task<IActionResult> CompositionBPRProcessQcTestGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetBPRProcessQcTest(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region Batch Qc Testing

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBatchAsync([FromServices] IAuthorizationScheme db, [FromServices] IQCBatch db2, [FromServices] IQcList db3, [FromServices] IProductionList db4, [FromServices] IInventoryList db5)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BatchMasterCtlr",
                        WildCard =  db2.GetWCLBatchRecordMaster(),
                        LoadByCard = db2.GetWCLBBatchRecordMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QC", User.Identity.Name, "Batch Testing"),
                        Otherdata = new {
                            ActionList = await db3.GetActionTypeListAsync(null,null),
                            QcTestList = await db4.GetQcTestListAsync(null,null),
                            MeasurementUnitList = await db5.GetMeasurementUnitListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BatchBMRSampleCtlr",
                        WildCard =  db2.GetWCLBMRSample(),
                        Reports = db2.GetRLBMRSample(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BatchBPRSampleCtlr",
                        WildCard =  db2.GetWCLBPRSample(),
                        Reports = db2.GetRLBPRSample(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BatchBMRSampleQcTestCtlr",
                        WildCard =  db2.GetWCLBMRSampleQcTest(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BatchBPRSampleQcTestCtlr",
                        WildCard =  db2.GetWCLBPRSampleQcTest(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public IActionResult BatchIndex()
        {
            return View();
        }

        #region BatchMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchMasterLoad([FromServices] IQCBatch db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBatchRecordMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanPost")]
        public async Task<string> BatchMasterPost([FromServices] IQCBatch db, [FromBody] tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster, string operation = "")
        {
            ModelState.Remove("BatchExpiryDate"); ModelState.Remove("FK_tbl_Inv_MeasurementUnit_ID_Dimension"); ModelState.Remove("FK_tbl_Pro_CompositionMaster_ID");
            if (ModelState.IsValid)
                return await db.PostBatchRecordMaster(tbl_Pro_BatchMaterialRequisitionMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchMasterGet([FromServices] IQCBatch db, int ID)
        {
            return Json(await db.GetBatchRecordMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion

        #region BatchBMRSample

        [AjaxOnly]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchBMRSampleLoad([FromServices] IQCBatch db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRSample(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanPost")]
        public async Task<string> BatchBMRSamplePost([FromServices] IQCBatch db, [FromBody] tbl_Qc_SampleProcessBMR tbl_Qc_SampleProcessBMR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRSample(tbl_Qc_SampleProcessBMR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchBMRSampleGet([FromServices] IQCBatch db, int ID)
        {
            return Json(await db.GetBMRSample(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BatchBPRSample

        [AjaxOnly]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchBPRSampleLoad([FromServices] IQCBatch db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBPRSample(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanPost")]
        public async Task<string> BatchBPRSamplePost([FromServices] IQCBatch db, [FromBody] tbl_Qc_SampleProcessBPR tbl_Qc_SampleProcessBPR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRSample(tbl_Qc_SampleProcessBPR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchBPRSampleGet([FromServices] IQCBatch db, int ID)
        {
            return Json(await db.GetBPRSample(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BatchBMRSample QcTest

        [AjaxOnly]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchBMRSampleQcTestLoad([FromServices] IQCBatch db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRSampleQcTest(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanPost")]
        public async Task<string> BatchBMRSampleQcTestPost([FromServices] IQCBatch db, [FromBody] tbl_Qc_SampleProcessBMR_QcTest tbl_Qc_SampleProcessBMR_QcTest, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRSampleQcTest(tbl_Qc_SampleProcessBMR_QcTest, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchBMRSampleQcTestGet([FromServices] IQCBatch db, int ID)
        {
            return Json(await db.GetBMRSampleQcTest(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanPost")]
        public async Task<string> BatchBMRSampleQcTestPostReplication([FromServices] IQCBatch db, int MasterID, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRSampleQcTestReplicationFromStandard(MasterID, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        #endregion

        #region BatchBPRSample QcTest

        [AjaxOnly]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchBPRSampleQcTestLoad([FromServices] IQCBatch db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBPRSampleQcTest(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanPost")]
        public async Task<string> BatchBPRSampleQcTestPost([FromServices] IQCBatch db, [FromBody] tbl_Qc_SampleProcessBPR_QcTest tbl_Qc_SampleProcessBPR_QcTest, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRSampleQcTest(tbl_Qc_SampleProcessBPR_QcTest, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Batch Testing", Operation = "CanView")]
        public async Task<IActionResult> BatchBPRSampleQcTestGet([FromServices] IQCBatch db, int ID)
        {
            return Json(await db.GetBPRSampleQcTest(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Batch Testing", Operation = "CanPost")]
        public async Task<string> BatchBPRSampleQcTestPostReplication([FromServices] IQCBatch db, int MasterID, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRSampleQcTestReplicationFromStandard(MasterID, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Batch Testing", Operation = "CanViewReport")]
        public async Task<IActionResult> GetBatchQcTestReport([FromServices] IQCBatch db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region ProductRegistration QcTest For PN

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedProductRegistrationPNQcTestAsync([FromServices] IAuthorizationScheme db, [FromServices] IProductRegistrationQcTestForPN db2, [FromServices] IInventoryList db3, [FromServices] IQcList db4)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductRegistrationPNQcTestCtlr",
                        WildCard = db2.GetWCLProductRegistration(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QC", User.Identity.Name, "Product Registration PN QcTest"),
                        Otherdata = new {
                            MeasurementUnitList = await db3.GetMeasurementUnitListAsync(null,null),
                            QcTestList = await db4.GetQcTestListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductRegistrationPNQcTestDetailCtlr",
                        WildCard = db2.GetWCLProductRegistrationPNQcTest(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Product Registration PN QcTest", Operation = "CanView")]
        public IActionResult ProductRegistrationPNQcTestIndex()
        {
            return View();
        }

        #region ProductRegistrationPNQcTest 

        [AjaxOnly]
        [MyAuthorization(FormName = "Product Registration PN QcTest", Operation = "CanView")]
        public async Task<IActionResult> ProductRegistrationPNQcTestLoad([FromServices] IProductRegistrationQcTestForPN db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductRegistration(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region ProductRegistrationPNQcTest Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Product Registration PN QcTest", Operation = "CanView")]
        public async Task<IActionResult> ProductRegistrationPNQcTestDetailLoad([FromServices] IProductRegistrationQcTestForPN db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductRegistrationPNQcTest(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Product Registration PN QcTest", Operation = "CanPost")]
        public async Task<string> ProductRegistrationPNQcTestDetailPost([FromServices] IProductRegistrationQcTestForPN db, [FromBody] tbl_Inv_ProductRegistrationDetail_PNQcTest tbl_Inv_ProductRegistrationDetail_PNQcTest, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostProductRegistrationPNQcTest(tbl_Inv_ProductRegistrationDetail_PNQcTest, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Product Registration PN QcTest", Operation = "CanView")]
        public async Task<IActionResult> ProductRegistrationPNQcTestDetailGet([FromServices] IProductRegistrationQcTestForPN db, int ID)
        {
            return Json(await db.GetProductRegistrationPNQcTest(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }


        #endregion        

        #endregion

        #region PurchaseNote Qc Testing

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseNoteQcTestAsync([FromServices] IAuthorizationScheme db, [FromServices] IQcPurchaseNote db2, [FromServices] IQcList db3, [FromServices] IInventoryList db4, [FromServices] IQcList db5)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseNoteQcTestCtlr",
                        WildCard = db2.GetWCLQcPurchaseNote(),
                        LoadByCard = db2.GetWCLBQcPurchaseNote(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QC", User.Identity.Name, "Purchase Note Testing"),
                        Otherdata = new { 
                            ActionList = await db3.GetActionTypeListAsync(null,null),
                            QcTestList = await db5.GetQcTestListAsync(null,null),
                            MeasurementUnitList = await db4.GetMeasurementUnitListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseNoteQcTestDetailCtlr",
                        WildCard = db2.GetWCLPurchaseNoteQcTest(),
                        LoadByCard = null,
                        Reports = db2.GetRLPurchaseNoteQcTest(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Note Testing", Operation = "CanView")]
        public IActionResult PurchaseNoteQcTestIndex()
        {
            return View();
        }

        #region PurchaseNoteQcTest
        [AjaxOnly]
        public async Task<IActionResult> PurchaseNoteQcTestLoad([FromServices] IQcPurchaseNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseNote(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Note Testing", Operation = "CanPost")]
        public async Task<string> PurchaseNoteQcTestPost([FromServices] IQcPurchaseNote db, [FromBody] tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseNote(tbl_Inv_PurchaseNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Note Testing", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteQcTestGet([FromServices] IQcPurchaseNote db, int ID)
        {
            return Json(await db.GetPurchaseNote(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PurchaseNoteQcTest Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Purchase Note Testing", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteQcTestDetailLoad([FromServices] IQcPurchaseNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadPurchaseNoteQcTest(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Note Testing", Operation = "CanPost")]
        public async Task<string> PurchaseNoteQcTestDetailPost([FromServices] IQcPurchaseNote db, [FromBody] tbl_Qc_PurchaseNoteDetail_QcTest tbl_Qc_PurchaseNoteDetail_QcTest, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseNoteQcTest(tbl_Qc_PurchaseNoteDetail_QcTest, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Note Testing", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteQcTestDetailGet([FromServices] IQcPurchaseNote db, int ID)
        {
            return Json(await db.GetPurchaseNoteQcTest(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Note Testing", Operation = "CanPost")]
        public async Task<string> PurchaseNoteQcTestDetailPostReplication([FromServices] IQcPurchaseNote db, int MasterID, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPurchaseNoteQcTestReplicationFromStandard(MasterID, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        #endregion        

        #region Report

        [MyAuthorization(FormName = "Purchase Note Testing", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPurchaseNoteQcTestReport([FromServices] IQcPurchaseNote db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

    }
}
