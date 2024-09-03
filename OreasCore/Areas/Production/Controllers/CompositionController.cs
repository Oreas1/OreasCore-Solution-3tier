using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using OfficeOpenXml.Packaging.Ionic.Zlib;

namespace OreasCore.Areas.Production.Controllers
{
    [Area("Production")]
    public class CompositionController : Controller
    {
        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCompositionAsync([FromServices] IAuthorizationScheme db, [FromServices] IComposition db2, [FromServices] IInventoryList db3, [FromServices] IProductionList db4)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionMasterCtlr",
                        WildCard =  db2.GetWCLCompositionMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Production", User.Identity.Name, "Composition"),
                        Otherdata = new {
                            MeasurementUnitList = await db3.GetMeasurementUnitListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionDetailRawMasterCtlr",
                        WildCard =  db2.GetWCLCompositionMaster(),
                        Reports = db2.GetRLCompositionRawMaster(),
                        Privilege = null,
                        Otherdata = new {
                            CompositionFilterList = await db4.GetCompositionFilterPolicyListAsync(true)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionDetailRawDetailCtlr",
                        WildCard =  db2.GetWCLCompositionDetailRawMaster(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionDetailCouplingMasterCtlr",
                        WildCard =  db2.GetWCLCompositionDetailCouplingMaster(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionDetailCouplingDetailPackagingMasterCtlr",
                        WildCard =  db2.GetWCLCompositionDetailCouplingDetailPackagingMaster(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionDetailCouplingDetailPackagingDetailMasterCtlr",
                        WildCard =  db2.GetWCLCompositionDetailCouplingDetailPackagingDetailMaster(),
                        Reports = db2.GetRLCouplingPackagingMaster(),
                        Privilege = null,
                        Otherdata = new {
                            CompositionFilterList = await db4.GetCompositionFilterPolicyListAsync(false)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionDetailCouplingDetailPackagingDetailDetailCtlr",
                        WildCard =  db2.GetWCLCompositionDetailCouplingDetailPackagingDetailDetail(),
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
        public async Task<IActionResult> CompositionMasterLoad([FromServices] IComposition db,
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

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionMasterPost([FromServices] IComposition db, [FromBody] tbl_Pro_CompositionMaster tbl_Pro_CompositionMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionMaster(tbl_Pro_CompositionMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionMasterGet([FromServices] IComposition db, int ID)
        {
            return Json(await db.GetCompositionMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition Detail Raw Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailRawMasterLoad([FromServices] IComposition db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionDetailRawMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionDetailRawMasterPost([FromServices] IComposition db, [FromBody] tbl_Pro_CompositionDetail_RawMaster tbl_Pro_CompositionDetail_RawMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionDetailRawMaster(tbl_Pro_CompositionDetail_RawMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailRawMasterGet([FromServices] IComposition db, int ID)
        {
            return Json(await db.GetCompositionDetailRawMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition Detail Raw Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailRawDetailLoad([FromServices] IComposition db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionDetailRawDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionDetailRawDetailPost([FromServices] IComposition db, [FromBody] tbl_Pro_CompositionDetail_RawDetail_Items tbl_Pro_CompositionDetail_RawDetail_Items, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionDetailRawDetail(tbl_Pro_CompositionDetail_RawDetail_Items, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailRawDetailGet([FromServices] IComposition db, int ID)
        {
            return Json(await db.GetCompositionDetailRawDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition Detail Coupling Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailCouplingMasterLoad([FromServices] IComposition db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionDetailCouplingMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionDetailCouplingMasterPost([FromServices] IComposition db, [FromBody] tbl_Pro_CompositionDetail_Coupling tbl_Pro_CompositionDetail_Coupling, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionDetailCouplingMaster(tbl_Pro_CompositionDetail_Coupling, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailCouplingMasterGet([FromServices] IComposition db, int ID)
        {
            return Json(await db.GetCompositionDetailCouplingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition Detail Coupling Detail Packaging Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailCouplingDetailPackagingMasterLoad([FromServices] IComposition db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionDetailCouplingDetailPackagingMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionDetailCouplingDetailPackagingMasterPost([FromServices] IComposition db, [FromBody] tbl_Pro_CompositionDetail_Coupling_PackagingMaster tbl_Pro_CompositionDetail_Coupling_PackagingMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionDetailCouplingDetailPackagingMaster(tbl_Pro_CompositionDetail_Coupling_PackagingMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailCouplingDetailPackagingMasterGet([FromServices] IComposition db, int ID)
        {
            return Json(await db.GetCompositionDetailCouplingDetailPackagingMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition Detail Coupling Detail Packaging Detail Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailCouplingDetailPackagingDetailMasterLoad([FromServices] IComposition db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionDetailCouplingDetailPackagingDetailMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionDetailCouplingDetailPackagingDetailMasterPost([FromServices] IComposition db, [FromBody] tbl_Pro_CompositionDetail_Coupling_PackagingDetail tbl_Pro_CompositionDetail_Coupling_PackagingDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionDetailCouplingDetailPackagingDetailMaster(tbl_Pro_CompositionDetail_Coupling_PackagingDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailCouplingDetailPackagingDetailMasterGet([FromServices] IComposition db, int ID)
        {
            return Json(await db.GetCompositionDetailCouplingDetailPackagingDetailMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Composition Detail Coupling Detail Packaging Detail Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailCouplingDetailPackagingDetailDetailLoad([FromServices] IComposition db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionDetailCouplingDetailPackagingDetailDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition", Operation = "CanPost")]
        public async Task<string> CompositionDetailCouplingDetailPackagingDetailDetailPost([FromServices] IComposition db, [FromBody] tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionDetailCouplingDetailPackagingDetailDetail(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition", Operation = "CanView")]
        public async Task<IActionResult> CompositionDetailCouplingDetailPackagingDetailDetailGet([FromServices] IComposition db, int ID)
        {
            return Json(await db.GetCompositionDetailCouplingDetailPackagingDetailDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        

        #region Report

        [MyAuthorization(FormName = "Composition", Operation = "CanViewReport")]
        public async Task<IActionResult> GetCompositionReport([FromServices] IComposition db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion
    }
}
