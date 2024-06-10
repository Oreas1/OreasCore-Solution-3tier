using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OreasCore.Areas.QA.Controllers
{
    [Area("QA")]
    public class ProcessController : Controller
    {
        #region BMRBPR Process
        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBMRBPRProcessAsync([FromServices] IAuthorizationScheme db, [FromServices] IQAProcess db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRBPRMasterCtlr",
                        WildCard =  db2.GetWCLBatchRecordMaster(),
                        LoadByCard = db2.GetWCLBBatchRecordMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QA", User.Identity.Name, "BMR BPR Process"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRProcessCtlr",
                        WildCard =  db2.GetWCLBMRProcess(),
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
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public IActionResult BMRBPRIndex()
        {
            return View();
        }

        #region BMRMaster

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BMRBPRMasterLoad([FromServices] IQAProcess db,
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

        #endregion

        #region BMRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BMRProcessLoad([FromServices] IQAProcess db,
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
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanPost")]
        public async Task<string> BMRProcessPost([FromServices] IQAProcess db, [FromBody] tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRProcess(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BMRProcessGet([FromServices] IQAProcess db, int ID)
        {
            return Json(await db.GetBMRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion        

        #region BPRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BPRProcessLoad([FromServices] IQAProcess db,
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
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanPost")]
        public async Task<string> BPRProcessPost([FromServices] IQAProcess db, [FromBody] tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRProcess(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BPRProcessGet([FromServices] IQAProcess db, int ID)
        {
            return Json(await db.GetBPRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        
                
        #endregion
    }
}
