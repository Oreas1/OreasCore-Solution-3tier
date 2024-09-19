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
    public class CostingController : Controller
    {       

        #region CompositionCosting

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCompositionCostingAsync([FromServices] IAuthorizationScheme db, [FromServices] ICompositionCosting db2, [FromServices] IAccountsList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionCostingMasterCtlr",
                        WildCard = db2.GetWCLCompositionMaster(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Costing"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionCostingIndirectExpenseCtlr",
                        WildCard = db2.GetWCLCompositionCostingIndirectExpense(),
                        Reports = db2.GetRLCompositionDetail(),
                        Privilege = null,
                        Otherdata = new {
                            CostingIndirectExpenseList = await db3.GetCostingIndirectExpenseListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionCostingDetailRawCtlr",
                        WildCard = db2.GetWCLCompositionDetailRaw(),
                        Reports = db2.GetRLCompositionDetail(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionCostingDetailPackagingCtlr",
                        WildCard = db2.GetWCLCompositionDetailPackaging(),
                        Reports = db2.GetRLCompositionDetail(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public IActionResult CompositionCostingIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingMasterLoad([FromServices] ICompositionCosting db,
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


        #endregion

        #region CompositionCostingIndirectExpense

        [AjaxOnly]
        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingIndirectExpenseLoad([FromServices] ICompositionCosting db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionCostingIndirectExpense(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Costing", Operation = "CanPost")]
        public async Task<string> CompositionCostingIndirectExpensePost([FromServices] ICompositionCosting db, [FromBody] tbl_Ac_CompositionCostingIndirectExpense tbl_Ac_CompositionCostingIndirectExpense, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionCostingIndirectExpense(tbl_Ac_CompositionCostingIndirectExpense, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingIndirectExpenseGet([FromServices] ICompositionCosting db, int ID)
        {
            return Json(await db.GetCompositionCostingIndirectExpense(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Raw

        [AjaxOnly]
        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingDetailRawLoad([FromServices] ICompositionCosting db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionDetailRaw(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Costing", Operation = "CanPost")]
        public async Task<string> CompositionCostingDetailRawPost([FromServices] ICompositionCosting db, [FromBody] tbl_Pro_CompositionDetail_RawDetail_Items tbl_Pro_CompositionDetail_RawDetail_Items, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionDetailRaw(tbl_Pro_CompositionDetail_RawDetail_Items, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingDetailRawGet([FromServices] ICompositionCosting db, int ID)
        {
            return Json(await db.GetCompositionDetailRaw(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Packaging

        [AjaxOnly]
        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingDetailPackagingLoad([FromServices] ICompositionCosting db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionDetailPackaging(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Costing", Operation = "CanPost")]
        public async Task<string> CompositionCostingDetailPackagingPost([FromServices] ICompositionCosting db, [FromBody] tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionDetailPackaging(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> CompositionCostingDetailPackagingGet([FromServices] ICompositionCosting db, int ID)
        {
            return Json(await db.GetCompositionDetailPackaging(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Costing", Operation = "CanViewReport")]
        public async Task<IActionResult> GetCompositionCostingReport([FromServices] ICompositionCosting db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region BMRCosting

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBMRCostingAsync([FromServices] IAuthorizationScheme db, [FromServices] IBMRCosting db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRCostingMasterCtlr",
                        WildCard = db2.GetWCLBMRMaster(),
                        LoadByCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Accounts", User.Identity.Name, "Costing"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BMRCostingDetailPackagingMasterCtlr",
                        WildCard = db2.GetWCLBMRDetailPackagingMaster(),
                        Reports = db2.GetRLBMRDetail(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public IActionResult BMRCostingIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> BMRCostingMasterLoad([FromServices] IBMRCosting db,
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


        #endregion

        #region Detail Packaging

        [AjaxOnly]
        [MyAuthorization(FormName = "Costing", Operation = "CanView")]
        public async Task<IActionResult> BMRCostingDetailPackagingMasterLoad([FromServices] IBMRCosting db,
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


        #endregion

        #region Report

        [MyAuthorization(FormName = "Costing", Operation = "CanViewReport")]
        public async Task<IActionResult> GetBMRCostingReport([FromServices] IBMRCosting db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion


        #endregion
    }
}
