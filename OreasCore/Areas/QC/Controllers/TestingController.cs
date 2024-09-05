using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QC", User.Identity.Name, "Composition"),
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

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public IActionResult CompositionIndex()
        {
            return View();
        }

        #region Composition Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
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

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionMasterGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetCompositionMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Composition BMRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
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
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionBMRProcessPost([FromServices] ICompositionQcTest db, [FromBody] tbl_Pro_CompositionMaster_ProcessBMR tbl_Pro_CompositionMaster_ProcessBMR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRProcess(tbl_Pro_CompositionMaster_ProcessBMR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionBMRProcessGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetBMRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition BMRProcess QcTest

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
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
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionBMRProcessQcTestPost([FromServices] ICompositionQcTest db, [FromBody] tbl_Pro_CompositionMaster_ProcessBMR_QcTest tbl_Pro_CompositionMaster_ProcessBMR_QcTest, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRProcessQcTest(tbl_Pro_CompositionMaster_ProcessBMR_QcTest, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionBMRProcessQcTestGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetBMRProcessQcTest(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region CompositionPackagingMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
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

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionPackagingMasterGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetCompositionPackagingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition BPRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
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
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionBPRProcessPost([FromServices] ICompositionQcTest db, [FromBody] tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRProcess(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionBPRProcessGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetBPRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Composition BPRProcess QcTest

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
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
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionBPRProcessQcTestPost([FromServices] ICompositionQcTest db, [FromBody] tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRProcessQcTest(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionBPRProcessQcTestGet([FromServices] ICompositionQcTest db, int ID)
        {
            return Json(await db.GetBPRProcessQcTest(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion
    }
}
