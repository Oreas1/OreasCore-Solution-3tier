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
    public class SetUpController : Controller
    {


        #region QcLab

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedQcLabAsync([FromServices] IAuthorizationScheme db, [FromServices] IQcLab db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "QcLabIndexCtlr",
                        WildCard = db2.GetWCLQcLab(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QC", User.Identity.Name, "Qc Lab"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Qc Lab", Operation = "CanView")]
        public IActionResult QcLabIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> QcLabLoad([FromServices] IQcLab db,
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
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Qc Lab", Operation = "CanPost")]
        public async Task<string> QcLabPost([FromServices] IQcLab db, [FromBody] tbl_Qc_Lab tbl_Qc_Lab, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Qc_Lab, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Qc Lab", Operation = "CanView")]
        public async Task<IActionResult> QcLabGet([FromServices] IQcLab db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region QcTest

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedQcTestAsync([FromServices] IAuthorizationScheme db, [FromServices] IQcTest db2, [FromServices] IQcList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "QcTestIndexCtlr",
                        WildCard = db2.GetWCLQcTest(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("QC", User.Identity.Name, "Qc Test"),
                        Otherdata = new {
                            QcLabList = await db3.GetQcLabListAsync(null,null) 
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Qc Test", Operation = "CanView")]
        public IActionResult QcTestIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> QcTestLoad([FromServices] IQcTest db,
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
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Qc Test", Operation = "CanPost")]
        public async Task<string> QcTestPost([FromServices] IQcTest db, [FromBody] tbl_Qc_Test tbl_Qc_Test, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Qc_Test, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Qc Test", Operation = "CanView")]
        public async Task<IActionResult> QcTestGet([FromServices] IQcTest db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion
    }
}
