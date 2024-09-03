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
    public class SetUpController : Controller
    {


        #region Composition Filter Policy

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedCompositionFilterPolicyAsync([FromServices] IAuthorizationScheme db, [FromServices] ICompositionFilterPolicy db2, [FromServices] IInventoryList db3, [FromServices] IQcList db4)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionFilterPolicyMasterCtlr",
                        WildCard = null,
                        LoadByCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QA", User.Identity.Name, "Composition Filter Policy"),
                        Otherdata = new {
                            ProductType = await db3.GetProductTypeListAsync(null,null),
                            ActionList = await db4.GetActionTypeListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "CompositionFilterPolicyDetailCtlr",
                        WildCard = db2.GetWCLCompositionFilterPolicyDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Composition Filter Policy", Operation = "CanView")]
        public IActionResult CompositionFilterPolicyIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Filter Policy", Operation = "CanView")]
        public async Task<IActionResult> CompositionFilterPolicyMasterLoad([FromServices] ICompositionFilterPolicy db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionFilterPolicyMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition Filter Policy", Operation = "CanPost")]
        public async Task<string> CompositionFilterPolicyMasterPost([FromServices] ICompositionFilterPolicy db, [FromBody] tbl_Pro_CompositionFilterPolicyMaster tbl_Pro_CompositionFilterPolicyMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionFilterPolicyMaster(tbl_Pro_CompositionFilterPolicyMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition Filter Policy", Operation = "CanView")]
        public async Task<IActionResult> CompositionFilterPolicyMasterGet([FromServices] ICompositionFilterPolicy db, int ID)
        {
            return Json(await db.GetCompositionFilterPolicyMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Composition Filter Policy", Operation = "CanView")]
        public async Task<IActionResult> CompositionFilterPolicyDetailLoad([FromServices] ICompositionFilterPolicy db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadCompositionFilterPolicyDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Composition Filter Policy", Operation = "CanPost")]
        public async Task<string> CompositionFilterPolicyDetailPost([FromServices] ICompositionFilterPolicy db, [FromBody] tbl_Pro_CompositionFilterPolicyDetail tbl_Pro_CompositionFilterPolicyDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostCompositionFilterPolicyDetail(tbl_Pro_CompositionFilterPolicyDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Composition Filter Policy", Operation = "CanView")]
        public async Task<IActionResult> CompositionFilterPolicyDetailGet([FromServices] ICompositionFilterPolicy db, int ID)
        {
            return Json(await db.GetCompositionFilterPolicyDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region ProProcedure

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedProProcedureAsync([FromServices] IAuthorizationScheme db, [FromServices] IProProcedure db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProProcedureIndexCtlr",
                        WildCard = db2.GetWCLProProcedure(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QA", User.Identity.Name, "Procedure"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Procedure", Operation = "CanView")]
        public IActionResult ProProcedureIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> ProProcedureLoad([FromServices] IProProcedure db, string Caller="",
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(Caller,CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Procedure", Operation = "CanPost")]
        public async Task<string> ProProcedurePost([FromServices] IProProcedure db, [FromBody] tbl_Pro_Procedure tbl_Pro_Procedure, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Pro_Procedure, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Procedure", Operation = "CanView")]
        public async Task<IActionResult> ProProcedureGet([FromServices] IProProcedure db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion


    }
}
