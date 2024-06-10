using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using OreasModel;
using System;

namespace OreasCore.Areas.QA.Controllers
{
    [Area("QA")]
    public class ProductionOrderController : Controller
    {
        
        [MyAuthorization]
        public async Task<IActionResult> GetInitializedProductionOrderAsync([FromServices] IAuthorizationScheme db, [FromServices] IQAProductionOrder db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductionOrderCtlr",
                        WildCard =  db2.GetWCLProductionOrder(),
                        LoadByCard = db2.GetWCLBProductionOrder(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QA", User.Identity.Name, "Production Order"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductionOrderBatchCtlr",
                        WildCard =  null,
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Production Order", Operation = "CanView")]
        public IActionResult ProductionOrderIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Production Order", Operation = "CanView")]
        public async Task<IActionResult> BMRBPRMasterLoad([FromServices] IQAProductionOrder db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductionOrder(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Production Order BMR

        [AjaxOnly]
        [MyAuthorization(FormName = "Production Order", Operation = "CanView")]
        public async Task<IActionResult> ProductionOrderBatchLoad([FromServices] IQAProductionOrder db,
        int CurrentPage = 1, string BatchNo = "",
        string FilterByText = null, string FilterValueByText = null,
        string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
        string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
        string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductionOrderBatch(CurrentPage, BatchNo, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "Production Order", Operation = "CanView")]
        public async Task<IActionResult> BMRAvailabilityGet([FromServices] IQAProductionOrder db, int ID)
        {
            return Json(await db.GetBMRAvailability(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Production Order", Operation = "CanPost")]
        public async Task<string> GenerateBMR([FromServices] IQAProductionOrder db, string operation = "", string SPOperation = "", int FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID = 0)
        {
            if (ModelState.IsValid)
                return await db.GenerateBMR(FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID,SPOperation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #endregion
    }
}
