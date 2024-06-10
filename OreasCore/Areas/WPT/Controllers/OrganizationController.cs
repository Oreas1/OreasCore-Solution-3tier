using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
    public class OrganizationController : Controller
    {
        #region Designation

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedDesignationAsync([FromServices] IAuthorizationScheme db, [FromServices] IDesignation db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "DesignationIndexCtlr",
                        WildCard = db2.GetWCLDesignation(),
                        Reports = db2.GetRLDesignation(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Designation"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        [MyAuthorization(FormName = "Designation", Operation = "CanViewReport")]
        public async Task<IActionResult> GetReport([FromServices] IDesignation db,string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {           
             return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "/WPT/Employee/EmployeeReport", GroupID), "application/pdf");
        }

        [MyAuthorization(FormName = "Designation", Operation = "CanView")]
        public IActionResult DesignationIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> DesignationLoad([FromServices] IDesignation db,[FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            
            PagedData<object> pageddata = 
                await db.Load(CurrentPage, MasterID,FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return  Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        
        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Designation", Operation = "CanPost")]
        public async Task<string> DesignationPost([FromServices] IDesignation db, [FromBody]tbl_WPT_Designation tbl_WPT_Designation, string operation="")
        {

  
            
            
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_Designation, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }


        [MyAuthorization(FormName = "Designation", Operation = "CanView")]
        public async Task<IActionResult> DesignationGet([FromServices] IDesignation db, int ID)
        {          
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings()); 
        }

        #endregion

        #region Employee Level

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedEmployeeLevelAsync([FromServices] IAuthorizationScheme db, [FromServices] IEmployeeLevel db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "EmployeeLevelIndexCtlr",
                        WildCard = db2.GetWCLEmployeeLevel(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Employee Level"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Employee Level", Operation = "CanView")]
        public IActionResult EmployeeLevelIndex()
        {

            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> EmployeeLevelLoad([FromServices] IEmployeeLevel db, [FromServices] OreasDbContext context,
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
        [MyAuthorization(FormName = "Employee Level", Operation = "CanPost")]
        public async Task<string> EmployeeLevelPost([FromServices] IEmployeeLevel db, [FromBody] tbl_WPT_EmployeeLevel tbl_WPT_EmployeeLevel, string operation = "")
        {
            if(ModelState.IsValid)
                return await db .Post(tbl_WPT_EmployeeLevel, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employee Level", Operation = "CanView")]
        public async Task<IActionResult> EmployeeLevelGet([FromServices] IEmployeeLevel db, int ID)
        {
            return Json(await db .Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Educational Level Type

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedEducationalLevelTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IEducationalLevelType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "EducationalLevelIndexCtlr",
                        WildCard = db2.GetWCLEducationalLevelType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Educational Level Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Educational Level Type", Operation = "CanView")]
        public IActionResult EducationalLevelTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Educational Level Type", Operation = "CanView")]
        public async Task<IActionResult> EducationalLevelTypeLoad([FromServices] IEducationalLevelType db, [FromServices] OreasDbContext context,
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
        [MyAuthorization(FormName = "Educational Level Type", Operation = "CanPost")]
        public async Task<string> EducationalLevelTypePost([FromServices] IEducationalLevelType db, [FromBody] tbl_WPT_EducationalLevelType tbl_WPT_EducationalLevelType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_EducationalLevelType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Educational Level Type", Operation = "CanView")]
        public async Task<IActionResult> EducationalLevelTypeGet([FromServices] IEducationalLevelType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Holiday

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedHolidayAsync([FromServices] IAuthorizationScheme db, [FromServices] IHoliday db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "HolidayIndexCtlr",
                        WildCard = db2.GetWCLHoliday(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Holiday"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Holiday", Operation = "CanView")]
        public IActionResult HolidayIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Holiday", Operation = "CanView")]
        public async Task<IActionResult> HolidayLoad([FromServices] IHoliday db, [FromServices] OreasDbContext context,
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
        [MyAuthorization(FormName = "Holiday", Operation = "CanPost")]
        public async Task<string> HolidayPost([FromServices] IHoliday db, [FromBody] tbl_WPT_Holiday tbl_WPT_Holiday, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_Holiday, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Holiday", Operation = "CanView")]
        public async Task<IActionResult> HolidayGet([FromServices] IHoliday db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Department

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedDepartmentAsync([FromServices] IAuthorizationScheme db, [FromServices] IDepartment db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "DepartmentIndexCtlr",
                        WildCard = db2.GetWCLDepartment(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Department"),
                        Otherdata = new { DepartmentList = await IList.GetDepartmentListAsync(null, null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "DepartmentDesignationCtlr",
                        WildCard = db2.GetWCLDepartmentDesignation(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { DesignationList=await IList.GetDesignationListAsync(null,null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "DepartmentSectionsDetailCtlr",
                        WildCard = db2.GetWCLDepartmentSection(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "DepartmentSectionDetailHOSCtlr",
                        WildCard = db2.GetWCLDepartmentSectionHOS(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        #region Master
        [AjaxOnly]
        [HttpGet]
        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> GetNodes([FromServices] IDepartment db,int PID)
        { 
            return Json(await db.GetNodesAsync(PID) , new Newtonsoft.Json.JsonSerializerSettings());
        }

        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public IActionResult DepartmentIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> DepartmentLoad([FromServices] IDepartment db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadDepartment(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Department", Operation = "CanPost")]
        public async Task<string> DepartmentPost([FromServices] IDepartment db, [FromBody] tbl_WPT_Department tbl_WPT_Department, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostDepartment(tbl_WPT_Department, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> DepartmentGet([FromServices] IDepartment db, int ID)
        {
            return Json(await db.GetDepartment(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Designaiton

        [AjaxOnly]
        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> DepartmentDesignationLoad([FromServices] IDepartment db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadDepartmentDesignation(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Department", Operation = "CanPost")]
        public async Task<string> DepartmentDesignationPost([FromServices] IDepartment db, [FromBody] tbl_WPT_DepartmentDetail tbl_WPT_DepartmentDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostDepartmentDesignation(tbl_WPT_DepartmentDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> DepartmentDesignationGet([FromServices] IDepartment db, int ID)
        {
            return Json(await db.GetDepartmentDesignation(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Section


        [AjaxOnly]
        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> DepartmentSectionLoad([FromServices] IDepartment db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadDepartmentSection(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Department", Operation = "CanPost")]
        public async Task<string> DepartmentSectionPost([FromServices] IDepartment db, [FromBody] tbl_WPT_DepartmentDetail_Section tbl_WPT_DepartmentDetail_Section, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostDepartmentSection(tbl_WPT_DepartmentDetail_Section, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> DepartmentSectionGet([FromServices] IDepartment db, int ID)
        {
            return Json(await db.GetDepartmentSection(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Section HOS


        [AjaxOnly]
        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> DepartmentSectionHOSLoad([FromServices] IDepartment db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadDepartmentSectionHOS(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Department", Operation = "CanPost")]
        public async Task<string> DepartmentSectionHOSPost([FromServices] IDepartment db, [FromBody] tbl_WPT_DepartmentDetail_Section_HOS tbl_WPT_DepartmentDetail_Section_HOS, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostDepartmentSectionHOS(tbl_WPT_DepartmentDetail_Section_HOS, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Department", Operation = "CanView")]
        public async Task<IActionResult> DepartmentSectionHOSGet([FromServices] IDepartment db, int ID)
        {
            return Json(await db.GetDepartmentSectionHOS(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region Allowance Type

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedAllowanceTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IAllowanceType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "AllowanceTypeIndexCtlr",
                        WildCard = db2.GetWCLAllowanceType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Allowance Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Allowance Type", Operation = "CanView")]
        public IActionResult AllowanceTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Allowance Type", Operation = "CanView")]
        public async Task<IActionResult> AllowanceTypeLoad([FromServices] IAllowanceType db, 
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
        [MyAuthorization(FormName = "Allowance Type", Operation = "CanPost")]
        public async Task<string> AllowanceTypePost([FromServices] IAllowanceType db, [FromBody] tbl_WPT_AllowanceType tbl_WPT_AllowanceType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_AllowanceType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Allowance Type", Operation = "CanView")]
        public async Task<IActionResult> AllowanceTypeGet([FromServices] IAllowanceType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Deductible Type

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedDeductibleTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IDeductibleType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "DeductibleTypeIndexCtlr",
                        WildCard = db2.GetWCLDeductibleType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Deductible Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Deductible Type", Operation = "CanView")]
        public IActionResult DeductibleTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Deductible Type", Operation = "CanView")]
        public async Task<IActionResult> DeductibleTypeLoad([FromServices] IDeductibleType db,
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
        [MyAuthorization(FormName = "Deductible Type", Operation = "CanPost")]
        public async Task<string> DeductibleTypePost([FromServices] IDeductibleType db, [FromBody] tbl_WPT_DeductibleType tbl_WPT_DeductibleType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_DeductibleType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Deductible Type", Operation = "CanView")]
        public async Task<IActionResult> DeductibleTypeGet([FromServices] IDeductibleType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region OT Policy

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedOTPolicyAsync([FromServices] IAuthorizationScheme db, [FromServices] IOTPolicy db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "OTPolicyIndexCtlr",
                        WildCard = db2.GetWCLOTPolicy(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "OT Policy"),
                        Otherdata = new { CalculationMethodList = await IList.GetCalculationMethodListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "OT Policy", Operation = "CanView")]
        public IActionResult OTPolicyIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "OT Policy", Operation = "CanView")]
        public async Task<IActionResult> OTPolicyLoad([FromServices] IOTPolicy db, [FromServices] OreasDbContext context,
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
        [MyAuthorization(FormName = "OT Policy", Operation = "CanPost")]
        public async Task<string> OTPolicyPost([FromServices] IOTPolicy db, [FromBody] tbl_WPT_tbl_OTPolicy tbl_WPT_tbl_OTPolicy, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_tbl_OTPolicy, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "OT Policy", Operation = "CanView")]
        public async Task<IActionResult> OTPolicyGet([FromServices] IOTPolicy db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Bank

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedBankAsync([FromServices] IAuthorizationScheme db, [FromServices] IBank db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BanktIndexCtlr",
                        WildCard = db2.GetWCLBankMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Bank"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BankDetailBranchCtlr",
                        WildCard = db2.GetWCLBankDetailBranch(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BankDetailBranchCompanyAcCtlr",
                        WildCard = db2.GetWCLBankDetailBranchCompanyAc(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "BankDetailBranchEmployeeAcCtlr",
                        WildCard = db2.GetWCLBankDetailBranchEmployeeAc(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Bank Master

        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public IActionResult BankIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public async Task<IActionResult> BankLoad([FromServices] IBank db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadBankMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Bank", Operation = "CanPost")]
        public async Task<string> BankPost([FromServices] IBank db, [FromBody] tbl_WPT_Bank tbl_WPT_Bank, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBankMaster(tbl_WPT_Bank, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public async Task<IActionResult> BankGet([FromServices] IBank db, int ID)
        {
            return Json(await db.GetBankMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Bank Detail Branch

        [AjaxOnly]
        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public async Task<IActionResult> BankDetailBranchLoad([FromServices] IBank db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadBankDetailBranch(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Bank", Operation = "CanPost")]
        public async Task<string> BankDetailBranchPost([FromServices] IBank db, [FromBody] tbl_WPT_Bank_Branch tbl_WPT_Bank_Branch, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBankDetailBranch(tbl_WPT_Bank_Branch, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public async Task<IActionResult> BankDetailBranchGet([FromServices] IBank db, int ID)
        {
            return Json(await db.GetBankDetailBranch(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Branch Company Ac


        [AjaxOnly]
        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public async Task<IActionResult> BankDetailBranchCompanyAcLoad([FromServices] IBank db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadBankDetailBranchCompanyAc(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Bank", Operation = "CanPost")]
        public async Task<string> BankDetailBranchCompanyAcPost([FromServices] IBank db, [FromBody] tbl_WPT_CompanyBankDetail tbl_WPT_CompanyBankDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBankDetailBranchCompanyAc(tbl_WPT_CompanyBankDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public async Task<IActionResult> BankDetailBranchCompanyAcGet([FromServices] IBank db, int ID)
        {
            return Json(await db.GetBankDetailBranchCompanyAc(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail Branch Employee Ac


        [AjaxOnly]
        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public async Task<IActionResult> BankDetailBranchEmployeeAcLoad([FromServices] IBank db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadBankDetailBranchEmployeeAc(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Bank", Operation = "CanPost")]
        public async Task<string> BankDetailBranchEmployeeAcPost([FromServices] IBank db, [FromBody] tbl_WPT_EmployeeBankDetail tbl_WPT_EmployeeBankDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostBankDetailBranchEmployeeAc(tbl_WPT_EmployeeBankDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Bank", Operation = "CanView")]
        public async Task<IActionResult> BankDetailBranchEmployeeAcGet([FromServices] IBank db, int ID)
        {
            return Json(await db.GetBankDetailBranchEmployeeAc(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region Loan Type

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedLoanTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] ILoanType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "LoanTypeIndexCtlr",
                        WildCard = db2.GetWCLLoanType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Loan Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Loan Type", Operation = "CanView")]
        public IActionResult LoanTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Loan Type", Operation = "CanView")]
        public async Task<IActionResult> LoanTypeLoad([FromServices] ILoanType db,
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
        [MyAuthorization(FormName = "Loan Type", Operation = "CanPost")]
        public async Task<string> LoanTypePost([FromServices] ILoanType db, [FromBody] tbl_WPT_LoanType tbl_WPT_LoanType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_LoanType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Loan Type", Operation = "CanView")]
        public async Task<IActionResult> LoanTypeGet([FromServices] ILoanType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Employment Type

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedEmploymentTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IEmploymentType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "EmploymentTypeIndexCtlr",
                        WildCard = db2.GetWCLEmploymentType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Employment Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Employment Type", Operation = "CanView")]
        public IActionResult EmploymentTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Employment Type", Operation = "CanView")]
        public async Task<IActionResult> EmploymentTypeLoad([FromServices] IEmploymentType db,
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
        [MyAuthorization(FormName = "Employment Type", Operation = "CanPost")]
        public async Task<string> EmploymentTypePost([FromServices] IEmploymentType db, [FromBody] tbl_WPT_EmploymentType tbl_WPT_EmploymentType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_EmploymentType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employment Type", Operation = "CanView")]
        public async Task<IActionResult> EmploymentTypeGet([FromServices] IEmploymentType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Hiring Type

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedHiringTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IHiringType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "HiringTypeIndexCtlr",
                        WildCard = db2.GetWCLHiringType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Hiring Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Hiring Type", Operation = "CanView")]
        public IActionResult HiringTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Hiring Type", Operation = "CanView")]
        public async Task<IActionResult> HiringTypeLoad([FromServices] IHiringType db,
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
        [MyAuthorization(FormName = "Hiring Type", Operation = "CanPost")]
        public async Task<string> HiringTypePost([FromServices] IHiringType db, [FromBody] tbl_WPT_HiringType tbl_WPT_HiringType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_HiringType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Hiring Type", Operation = "CanView")]
        public async Task<IActionResult> HiringTypeGet([FromServices] IHiringType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Reward Type

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedRewardTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IRewardType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "RewardTypeIndexCtlr",
                        WildCard = db2.GetWCLRewardType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Reward Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Reward Type", Operation = "CanView")]
        public IActionResult RewardTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Reward Type", Operation = "CanView")]
        public async Task<IActionResult> RewardTypeLoad([FromServices] IRewardType db,
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
        [MyAuthorization(FormName = "Reward Type", Operation = "CanPost")]
        public async Task<string> RewardTypePost([FromServices] IRewardType db, [FromBody] tbl_WPT_RewardType tbl_WPT_RewardType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_RewardType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Reward Type", Operation = "CanView")]
        public async Task<IActionResult> RewardTypeGet([FromServices] IRewardType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region InActiveType

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedInActiveTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IInActiveType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "InActiveTypeIndexCtlr",
                        WildCard = db2.GetWCLInActiveType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "InActiveType"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "InActiveType", Operation = "CanView")]
        public IActionResult InActiveTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "InActiveType", Operation = "CanView")]
        public async Task<IActionResult> InActiveTypeLoad([FromServices] IInActiveType db, [FromServices] OreasDbContext context,
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
        [MyAuthorization(FormName = "InActiveType", Operation = "CanPost")]
        public async Task<string> InActiveTypePost([FromServices] IInActiveType db, [FromBody] tbl_WPT_InActiveType tbl_WPT_InActiveType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_InActiveType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "InActiveType", Operation = "CanView")]
        public async Task<IActionResult> InActiveTypeGet([FromServices] IInActiveType db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Policy General

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPolicyGeneralAsync([FromServices] IAuthorizationScheme db, [FromServices] IPolicyGeneral db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PolicyGeneralIndexCtlr",
                        WildCard = null,
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Policy General"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Policy General", Operation = "CanView")]
        public IActionResult PolicyGeneralIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy General", Operation = "CanView")]
        public async Task<IActionResult> PolicyGeneralLoad([FromServices] IPolicyGeneral db)
        {
            return Json(await db.Load(), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Policy General", Operation = "CanPost")]
        public async Task<string> PolicyGeneralPost([FromServices] IPolicyGeneral db, [FromBody] tbl_WPT_PolicyGeneral tbl_WPT_PolicyGeneral, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_WPT_PolicyGeneral, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        #endregion

        #region Policy Penalty

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPolicyPenaltyAsync([FromServices] IAuthorizationScheme db, [FromServices] IPolicyPenalty db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PolicyPenaltyIndexCtlr",
                        WildCard = db2.GetWCLPolicyPenalty(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Policy Penalty"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "PolicyPenaltyDesignationCtlr",
                        WildCard = db2.GetWCLPolicyPenaltyDesignation(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { DesignationList=await IList.GetDesignationListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Policy Penalty

        [MyAuthorization(FormName = "Policy Penalty", Operation = "CanView")]
        public IActionResult PolicyPenaltyIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy Penalty", Operation = "CanView")]
        public async Task<IActionResult> PolicyPenaltyLoad([FromServices] IPolicyPenalty db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadPolicyPenalty(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Policy Penalty", Operation = "CanPost")]
        public async Task<string> PolicyPenaltyPost([FromServices] IPolicyPenalty db, [FromBody] tbl_WPT_PolicyPenaltyOnWT tbl_WPT_PolicyPenaltyOnWT, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPolicyPenalty(tbl_WPT_PolicyPenaltyOnWT, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Policy Penalty", Operation = "CanView")]
        public async Task<IActionResult> PolicyPenaltyGet([FromServices] IPolicyPenalty db, int ID)
        {
            return Json(await db.GetPolicyPenalty(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Policy Penalty Designaiton

        [AjaxOnly]
        [MyAuthorization(FormName = "Policy Penalty", Operation = "CanView")]
        public async Task<IActionResult> PolicyPenaltyDesignationLoad([FromServices] IPolicyPenalty db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadPolicyPenaltyDesignation(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Policy Penalty", Operation = "CanPost")]
        public async Task<string> PolicyPenaltyDesignationPost([FromServices] IPolicyPenalty db, [FromBody] tbl_WPT_PolicyPenaltyOnWTDetail_Designation tbl_WPT_PolicyPenaltyOnWTDetail_Designation, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostPolicyPenaltyDesignation(tbl_WPT_PolicyPenaltyOnWTDetail_Designation, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Policy Penalty", Operation = "CanView")]
        public async Task<IActionResult> PolicyPenaltyDesignationGet([FromServices] IPolicyPenalty db, int ID)
        {
            return Json(await db.GetPolicyPenaltyDesignation(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion


    }
}
