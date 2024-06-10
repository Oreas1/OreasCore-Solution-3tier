using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OreasCore.Areas.WPT.Controllers
{
    [Area("WPT")]
    public class IncentiveController : Controller
    {

        #region Incentive Policy

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedIncentivePolicyAsync([FromServices] IAuthorizationScheme db, [FromServices] IIncentivePolicy db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "IncentivePolicyIndexCtlr",
                        WildCard = db2.GetWCLIncentiveMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Incentive Policy"),
                        Otherdata = new { 
                            IncentiveTypeList = await IList.GetIncentiveTypeList(),
                            CalculationMethodList = await IList.GetCalculationMethodListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "IncentivePolicyDesignationCtlr",
                        WildCard = db2.GetWCLIncentiveDetailDesignation(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { DesignationList = await IList.GetDesignationListAsync(null,null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "IncentivePolicyEmployeeCtlr",
                        WildCard = db2.GetWCLIncentiveDetailEmployee(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Incentive Policy master

        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanView")]
        public IActionResult IncentivePolicyIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanView")]
        public async Task<IActionResult> IncentivePolicyLoad([FromServices] IIncentivePolicy db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadIncentiveMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanPost")]
        public async Task<string> IncentivePolicyPost([FromServices] IIncentivePolicy db, [FromBody] tbl_WPT_IncentivePolicy tbl_WPT_IncentivePolicy, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostIncentiveMaster(tbl_WPT_IncentivePolicy, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }
        

        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanView")]
        public async Task<IActionResult> IncentivePolicyGet([FromServices] IIncentivePolicy db, int ID)
        {
            return Json(await db.GetIncentiveMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Incentive Policy Designation


        [AjaxOnly]
        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanView")]
        public async Task<IActionResult> IncentivePolicyDesignationLoad([FromServices] IIncentivePolicy db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadIncentiveDetailDesignation(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanPost")]
        public async Task<string> IncentivePolicyDesignationPost([FromServices] IIncentivePolicy db, [FromBody] tbl_WPT_IncentivePolicyDesignation tbl_WPT_IncentivePolicyDesignation, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostIncentiveDetailDesignation(tbl_WPT_IncentivePolicyDesignation, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanView")]
        public async Task<IActionResult> IncentivePolicyDesignationGet([FromServices] IIncentivePolicy db, int ID)
        {
            return Json(await db.GetIncentiveDetailDesignation(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }



        #endregion

        #region Incentive Policy Employee


        [AjaxOnly]
        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanView")]
        public async Task<IActionResult>  IncentivePolicyEmployeeLoad([FromServices] IIncentivePolicy db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadIncentiveDetailEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanPost")]
        public async Task<string>  IncentivePolicyEmployeePost([FromServices] IIncentivePolicy db, [FromBody] tbl_WPT_IncentivePolicyEmployees tbl_WPT_IncentivePolicyEmployees, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostIncentiveDetailEmployee(tbl_WPT_IncentivePolicyEmployees, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Incentive Policy", Operation = "CanView")]
        public async Task<IActionResult>  IncentivePolicyEmployeeGet([FromServices] IIncentivePolicy db, int ID)
        {
            return Json(await db.GetIncentiveDetailEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #endregion

    }
}
