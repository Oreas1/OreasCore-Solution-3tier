using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OreasCore.Areas.Production.Controllers
{
    [Area("Production")]
    public class ProcessController : Controller
    {
        #region BMRBPR Process
        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBMRBPRAsync([FromServices] IAuthorizationScheme db, [FromServices] IBMRBPRProcess db2, [FromServices] IInventoryList db3, [FromServices] IProductionList db4)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRBPRMasterCtlr",
                        WildCard =  db2.GetWCLBMRBPRMaster(),
                        LoadByCard = db2.GetWCLBBMRBPRMaster(),
                        Reports = db2.GetRLMaster(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Production", User.Identity.Name, "BMR BPR Process"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRBPRProcessCtlr",
                        WildCard =  null,
                        Reports = db2.GetRLDetail(),
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
                            BMRProcedureList = await db4.GetProProcedureListAsync("byBMRBPR","BMR BPR Process")
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRSampleCtlr",
                        WildCard =  db2.GetWCLBMRSample(),
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
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BPRSampleCtlr",
                        WildCard =  db2.GetWCLBPRSample(),
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
        public async Task<IActionResult> BMRBPRMasterLoad([FromServices] IBMRBPRProcess db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBMRBPRMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanPost")]
        public async Task<string> BMRBPRMasterPost([FromServices] IBMRBPRProcess db, [FromBody] tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster, string operation = "")
        {            
            if (ModelState.IsValid)
                return await db.PostBMRBPRMaster(tbl_Pro_BatchMaterialRequisitionMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BMRBPRMasterGet([FromServices] IBMRBPRProcess db, int ID)
        {
            return Json(await db.GetBMRBPRMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BMRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BMRProcessLoad([FromServices] IBMRBPRProcess db,
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
        public async Task<string> BMRProcessPost([FromServices] IBMRBPRProcess db, [FromBody] tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRProcess(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BMRProcessGet([FromServices] IBMRBPRProcess db, int ID)
        {
            return Json(await db.GetBMRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }
        #endregion

        #region BMRSample

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BMRSampleLoad([FromServices] IBMRBPRProcess db,
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
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanPost")]
        public async Task<string> BMRSamplePost([FromServices] IBMRBPRProcess db, [FromBody] tbl_Qc_SampleProcessBMR tbl_Qc_SampleProcessBMR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBMRSample(tbl_Qc_SampleProcessBMR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BMRSampleGet([FromServices] IBMRBPRProcess db, int ID)
        {
            return Json(await db.GetBMRSample(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region BPRProcess

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BPRProcessLoad([FromServices] IBMRBPRProcess db,
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
        public async Task<string> BPRProcessPost([FromServices] IBMRBPRProcess db, [FromBody] tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRProcess(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BPRProcessGet([FromServices] IBMRBPRProcess db, int ID)
        {
            return Json(await db.GetBPRProcess(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region BPRSample

        [AjaxOnly]
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BPRSampleLoad([FromServices] IBMRBPRProcess db,
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
        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanPost")]
        public async Task<string> BPRSamplePost([FromServices] IBMRBPRProcess db, [FromBody] tbl_Qc_SampleProcessBPR tbl_Qc_SampleProcessBPR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBPRSample(tbl_Qc_SampleProcessBPR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanView")]
        public async Task<IActionResult> BPRSampleGet([FromServices] IBMRBPRProcess db, int ID)
        {
            return Json(await db.GetBPRSample(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "BMR BPR Process", Operation = "CanViewReport")]
        public async Task<IActionResult> GetBMRReport([FromServices] IBMRBPRProcess db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

    }
}
