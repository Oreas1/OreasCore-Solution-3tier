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
    public class ShiftController : Controller
    {

        #region Shift

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedShiftAsync([FromServices] IAuthorizationScheme db, [FromServices] IShift db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ShiftIndexCtlr",
                        WildCard = db2.GetWCLShift(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Shift"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "DefaultEmployeeShiftCtlr",
                        WildCard = db2.GetWCLDefaultEmployeeShift(),
                        LoadByCard = db2.GetLBLDefaultEmployeeShift(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { 
                            SectionList=await IList.GetSectionListAsync("General",null,null,0,null), 
                            DesignationList=await IList.GetDesignationListAsync(null,null),
                            BulkByList = db2.GetLBLDefaultEmployeeShift()
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                ); 
        }

        #region Shift

        [MyAuthorization(FormName = "Shift", Operation = "CanView")]
        public IActionResult ShiftIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Shift", Operation = "CanView")]
        public async Task<IActionResult> ShiftLoad([FromServices] IShift db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadShift(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Shift", Operation = "CanPost")]
        public async Task<string> ShiftPost([FromServices] IShift db, [FromBody] tbl_WPT_Shift tbl_WPT_Shift, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostShift(tbl_WPT_Shift, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Shift", Operation = "CanView")]
        public async Task<IActionResult> ShiftGet([FromServices] IShift db, int ID)
        {   
            return Json(await db.GetShift(ID),new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Default Employees Shift

        [AjaxOnly]
        [MyAuthorization(FormName = "Shift", Operation = "CanView")]
        public async Task<IActionResult> DefaultEmployeeShiftLoad([FromServices] IShift db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadDefaultEmployeeShift(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Shift", Operation = "CanPost")]
        public async Task<string> DefaultEmployeeShiftBulkPost([FromServices] IShift db, int ShiftID = 0, int SectionID = 0, int DesignationID = 0, string BulkBy = "All")
        {
            if (ModelState.IsValid)
                return await db.PostDefaultEmployeeShiftBulk(ShiftID, SectionID,DesignationID, BulkBy, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        #endregion

        #endregion
    }
}
