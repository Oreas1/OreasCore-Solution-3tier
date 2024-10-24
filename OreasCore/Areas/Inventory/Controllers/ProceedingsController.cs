﻿using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OreasCore.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ProceedingsController : Controller
    {
        #region Ledger

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedInvLedgerAsync([FromServices] IAuthorizationScheme db, [FromServices] IInvLedger db2, [FromServices] IInventoryList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "InvLedgerIndexCtlr",
                        WildCard = null,
                        Reports = db2.GetRLInvLedger(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Ledger"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Ledger", Operation = "CanView")]
        public IActionResult InvLedgerIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Ledger", Operation = "CanView")]
        public async Task<IActionResult> InvLedgerLoad([FromServices] IInvLedger db,
            int CurrentPage = 1, int MasterID = 0, int WareHouseID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadInvLedger(CurrentPage, MasterID, WareHouseID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        [MyAuthorization(FormName = "Ledger", Operation = "CanViewReport")]
        public async Task<IActionResult> GetInvLedgerReport([FromServices] IInvLedger db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name ), "application/pdf");
        }


        #endregion
    }
}
