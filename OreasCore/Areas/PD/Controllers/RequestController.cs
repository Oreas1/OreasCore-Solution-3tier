using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using OfficeOpenXml.Packaging.Ionic.Zlib;

namespace OreasCore.Areas.PD.Controllers
{
    [Area("PD")]
    public class RequestController : Controller
    {

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedRequestAsync([FromServices] IAuthorizationScheme db, [FromServices] IPDRequest db2, [FromServices] IInventoryList db3, [FromServices] IProductionList db4)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RequestMasterCtlr",
                        WildCard =  db2.GetWCLRequestMaster(),
                        LoadByCard = db2.GetWCLBRequestMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("PD", User.Identity.Name, "Request"),
                        Otherdata = new {
                            WareHouseList = await db3.GetWareHouseListAsync(null,null),
                            ProProcedureList = await db4.GetProProcedureListAsync(null,null),
                            CompositionFilterList = await db4.GetCompositionFilterPolicyListAsync(null)
                        }

                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RequestDetailTRCtlr",
                        WildCard =  db2.GetWCLRequestDetailTR(),
                        LoadByCard = db2.GetWCLBRequestDetailTR(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RequestDetailTRProcedureCtlr",
                        WildCard =  db2.GetWCLRequestDetailTRProcedure(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RequestDetailTRCFPCtlr",
                        WildCard =  db2.GetWCLRequestDetailTRCFP(),
                        Reports = db2.GetRLTR(),
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RequestDetailTRCFPItemCtlr",
                        WildCard =  db2.GetWCLRequestDetailTRCFPItem(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public IActionResult RequestIndex()
        {
            return View();
        }

        #region Request Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestMasterLoad([FromServices] IPDRequest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRequestMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Request", Operation = "CanPost")]
        public async Task<string> RequestMasterPost([FromServices] IPDRequest db, [FromBody] tbl_PD_RequestMaster tbl_PD_RequestMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRequestMaster(tbl_PD_RequestMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestMasterGet([FromServices] IPDRequest db, int ID)
        {
            return Json(await db.GetRequestMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Request Detail TR

        [AjaxOnly]
        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestDetailTRLoad([FromServices] IPDRequest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRequestDetailTR(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Request", Operation = "CanPost")]
        public async Task<string> RequestDetailTRPost([FromServices] IPDRequest db, [FromBody] tbl_PD_RequestDetailTR tbl_PD_RequestDetailTR, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRequestDetailTR(tbl_PD_RequestDetailTR, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestDetailTRGet([FromServices] IPDRequest db, int ID)
        {
            return Json(await db.GetRequestDetailTR(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Request Detail TR Procedure

        [AjaxOnly]
        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestDetailTRProcedureLoad([FromServices] IPDRequest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRequestDetailTRProcedure(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Request", Operation = "CanPost")]
        public async Task<string> RequestDetailTRProcedurePost([FromServices] IPDRequest db, [FromBody] tbl_PD_RequestDetailTR_Procedure tbl_PD_RequestDetailTR_Procedure, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRequestDetailTRProcedure(tbl_PD_RequestDetailTR_Procedure, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestDetailTRProcedureGet([FromServices] IPDRequest db, int ID)
        {
            return Json(await db.GetRequestDetailTRProcedure(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Request Detail TR CFP

        [AjaxOnly]
        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestDetailTRCFPLoad([FromServices] IPDRequest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRequestDetailTRCFP(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Request", Operation = "CanPost")]
        public async Task<string> RequestDetailTRCFPPost([FromServices] IPDRequest db, [FromBody] tbl_PD_RequestDetailTR_CFP tbl_PD_RequestDetailTR_CFP, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRequestDetailTRCFP(tbl_PD_RequestDetailTR_CFP, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestDetailTRCFPGet([FromServices] IPDRequest db, int ID)
        {
            return Json(await db.GetRequestDetailTRCFP(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Request Detail TR CFP

        [AjaxOnly]
        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestDetailTRCFPItemLoad([FromServices] IPDRequest db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadRequestDetailTRCFPItem(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Request", Operation = "CanPost")]
        public async Task<string> RequestDetailTRCFPItemPost([FromServices] IPDRequest db, [FromBody] tbl_PD_RequestDetailTR_CFP_Item tbl_PD_RequestDetailTR_CFP_Item, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostRequestDetailTRCFPItem(tbl_PD_RequestDetailTR_CFP_Item, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Request", Operation = "CanView")]
        public async Task<IActionResult> RequestDetailTRCFPItemGet([FromServices] IPDRequest db, int ID)
        {
            return Json(await db.GetRequestDetailTRCFPItem(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Request", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPDRequestReport([FromServices] IPDRequest db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

    }
}
