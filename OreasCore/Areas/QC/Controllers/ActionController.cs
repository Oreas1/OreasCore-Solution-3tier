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
    public class ActionController : Controller
    {
        #region PurchaseNoteAction

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPurchaseNoteActionAsync([FromServices] IAuthorizationScheme db, [FromServices] IQcPurchaseNote db2, [FromServices] IQcList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PurchaseNoteActionIndexCtlr",
                        WildCard = db2.GetWCLQcPurchaseNote(),
                        LoadByCard = db2.GetWCLBQcPurchaseNote(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QC", User.Identity.Name, "PurchaseNote Action"),
                        Otherdata = new { ActionList = await db3.GetActionTypeListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "PurchaseNote Action", Operation = "CanView")]
        public IActionResult PurchaseNoteActionIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> PurchaseNoteActionLoad([FromServices] IQcPurchaseNote db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad, User.Identity.Name);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PurchaseNote Action", Operation = "CanPost")]
        public async Task<string> PurchaseNoteActionPost([FromServices] IQcPurchaseNote db, [FromBody] tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Inv_PurchaseNoteDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PurchaseNote Action", Operation = "CanView")]
        public async Task<IActionResult> PurchaseNoteActionGet([FromServices] IQcPurchaseNote db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #region Report

        [MyAuthorization(FormName = "PurchaseNote Action", Operation = "CanViewReport")]
        public async Task<IActionResult> GetPurchaseNoteActionReport([FromServices] IQcPurchaseNote db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

    }
}
