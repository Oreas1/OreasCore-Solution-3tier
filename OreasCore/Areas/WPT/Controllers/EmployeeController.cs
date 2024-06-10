using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OreasCore.Custom_Classes;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OreasCore.Areas.WPT.Controllers
{
    [Area("WPT")]
    public class EmployeeController : Controller
    {

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetEmployeeListAsync([FromServices] IWPTList IList, string QueryName = "", string EmployeeFilterBy = "", string EmployeeFilterValue = "", int FormID = 0)
        {
            return Json(
                await IList.GetEmployeesListAsync(QueryName, EmployeeFilterBy, EmployeeFilterValue, FormID)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        #region Employee

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedEmployeeAsync([FromServices] IAuthorizationScheme db, [FromServices] IEmployee db2, [FromServices] IWPTList IList)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "EmployeeIndexCtlr",
                        WildCard = db2.GetWCLEmployee(),
                        Reports = db2.GetRLEmployee(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("WPT", User.Identity.Name, "Employee"),
                        Otherdata = new 
                        { 
                            InActiveTypeList = await IList.GetInActiveTypeListAsync(null,null),
                            EmployeeLevelList = await IList.GetEmployeeLevelListAsync(null,null),
                            EmploymentTypeList = await IList.GetEmploymentTypeListAsync(null,null),
                            ShiftList = await IList.GetShiftListAsync(null,null),
                            SectionList = await IList.GetSectionListAsync("General",null,null,0,null),
                            DesignationList = await IList.GetDesignationListAsync(null,null),
                            EducationalTypeList = await IList.GetEducationalLevelTypeListAsync(null,null),
                            ATTypeList = await IList.GetATypeListAsync(null,null),
                            OTPolicyList = await IList.GetOTPolicyListAsync(null,null),
                            TransactionModeList = await IList.GetTransactionModeListAsync(null,null),
                            BankList = await IList.GetBankListAsync(null,null)
                        }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "EmployeeFFCPTemplateCtlr",
                        WildCard = null,
                        Reports = db2.GetRLEmployeeLetter(),
                        Privilege = null,
                        Otherdata = null
                    }
                    ,
                    new Init_ViewSetupStructure()
                    {
                        Controller = "EmployeeSalaryCtlr",
                        WildCard = db2.GetWCLEmployeeSalary(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            WageCalculationTypeList = await IList.GetWageCalculationTypeListAsync(null,null),
                            AllowanceTypeList = await IList.GetAllowanceTypeListAsync(null,null),
                            DeductibleTypeList = await IList.GetDeductibleTypeListAsync(null,null)
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        #region Employee Info

        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public IActionResult EmployeeIndex()
        {
            return View();
        }

        [AjaxOnly]
        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeLoad([FromServices] IEmployee db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadEmployee(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> EmployeePost([FromServices] IEmployee db, [FromBody] VM_EmployeeEnrollment VM_EmployeeEnrollment, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostEmployee(VM_EmployeeEnrollment, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeGet([FromServices] IEmployee db, int ID)
        {
            return Json(await db.GetEmployee(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> EmployeeUploadExcelFile([FromServices] IEmployee db, IFormFile EmpExcelFile, string operation = "")
        {
            if (EmpExcelFile.Length > 0 && Path.GetExtension(EmpExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await EmpExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<EmployeeExcelData> EmployeeExcelDataList = new List<EmployeeExcelData>();

                    try
                    {

                        await Task.Factory.StartNew(() =>
                        {
                            DateTime JoinDate;
                            DateTime? DateOfBirth;

                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                JoinDate = worksheet.Cells[rowNo, 9].Value == null ? DateTime.Now :
                                           DateTime.TryParse(worksheet.Cells[rowNo, 9].Value.ToString(), out DateTime JoinDate2) ? Convert.ToDateTime(worksheet.Cells[rowNo, 9].Value) : DateTime.Now;

                                DateOfBirth = worksheet.Cells[rowNo, 17].Value == null ? null :
                                            DateTime.TryParse(worksheet.Cells[rowNo, 17].Value.ToString(), out DateTime DateOfBirth2) ? Convert.ToDateTime(worksheet.Cells[rowNo, 17].Value) : null;

                                EmployeeExcelDataList.Add(new EmployeeExcelData()
                                {
                                    EmploymentTypeID = worksheet.Cells[rowNo, 1].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 1].Value.ToString(), out int a0) ? Convert.ToInt32(worksheet.Cells[rowNo, 1].Value) : 0,
                                    EmployeeName = worksheet.Cells[rowNo, 2].Value == null ? "" : worksheet.Cells[rowNo, 2].Value.ToString(),
                                    MachineID = worksheet.Cells[rowNo, 3].Value == null ? "" : worksheet.Cells[rowNo, 3].Value.ToString(),
                                    Gender = worksheet.Cells[rowNo, 4].Value == null ? "" : worksheet.Cells[rowNo, 4].Value.ToString(),
                                    SectionID = worksheet.Cells[rowNo, 5].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 5].Value.ToString(), out int a1) ? Convert.ToInt32(worksheet.Cells[rowNo, 5].Value) : 0,
                                    DesignationID = worksheet.Cells[rowNo, 6].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 6].Value.ToString(), out int a2) ? Convert.ToInt32(worksheet.Cells[rowNo, 6].Value) : 0,
                                    EmployeeLevelID = worksheet.Cells[rowNo, 7].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 7].Value.ToString(), out int a3) ? Convert.ToInt32(worksheet.Cells[rowNo, 7].Value) : 0,
                                    EducationID = worksheet.Cells[rowNo, 8].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 8].Value.ToString(), out int a4) ? Convert.ToInt32(worksheet.Cells[rowNo, 8].Value) : 0,
                                    JoiningDate = JoinDate, //9
                                    ShiftID = worksheet.Cells[rowNo, 10].Value == null ? 0 :
                                              int.TryParse(worksheet.Cells[rowNo, 10].Value.ToString(), out int a5) ? Convert.ToInt32(worksheet.Cells[rowNo, 10].Value) : 0,
                                    FatherName = worksheet.Cells[rowNo, 11].Value == null ? "" : worksheet.Cells[rowNo, 11].Value.ToString(),
                                    CNIC = worksheet.Cells[rowNo, 12].Value == null ? "" : worksheet.Cells[rowNo, 12].Value.ToString(),
                                    MaritalStatus = worksheet.Cells[rowNo, 13].Value == null ? "" : worksheet.Cells[rowNo, 13].Value.ToString(),
                                    CellNo = worksheet.Cells[rowNo, 14].Value == null ? "" : worksheet.Cells[rowNo, 14].Value.ToString(),
                                    Address = worksheet.Cells[rowNo, 15].Value == null ? "" : worksheet.Cells[rowNo, 15].Value.ToString(),
                                    Email = worksheet.Cells[rowNo, 16].Value == null ? "" : worksheet.Cells[rowNo, 16].Value.ToString(),
                                    DOB = DateOfBirth, //17
                                    BloodGroup = worksheet.Cells[rowNo, 18].Value == null ? "" : worksheet.Cells[rowNo, 18].Value.ToString(),
                                    BasicSalary = worksheet.Cells[rowNo, 19].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 19].Value.ToString(), out double a6) ? Convert.ToDouble(worksheet.Cells[rowNo, 19].Value) : 0,
                                    OTPolicyID = worksheet.Cells[rowNo, 20].Value == null ? 0 :
                                                 int.TryParse(worksheet.Cells[rowNo, 20].Value.ToString(), out int a7) ? Convert.ToInt32(worksheet.Cells[rowNo, 20].Value) : 0,
                                    TransactionModeID = worksheet.Cells[rowNo, 21].Value == null ? 0 :
                                                 int.TryParse(worksheet.Cells[rowNo, 21].Value.ToString(), out int a8) ? Convert.ToInt32(worksheet.Cells[rowNo, 21].Value) : 0,
                                });
                            }
                        });

                        await db.EmployeeUploadExcelFile(EmployeeExcelDataList, operation, User.Identity.Name);

                    }
                    catch (Exception ex)
                    {
                        return ex.InnerException.Message;
                    }
                }
            }
            else
            {
                return "File not Supported";
            }

            return "OK";
        }


        #endregion

        #region Employee Face Finger Card Pin Template


        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> EmployeeFFCPPost([FromServices] IEmployee db, int EmpID, string CardNo = null, string Paswd = null, int Privilege = 0, bool Enabled = true, bool RemoveFace = false, bool RemoveFinger = false, bool RemovePhoto = false, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostEmployeeFFCPTemplate(operation, User.Identity.Name,EmpID, CardNo, Paswd, Privilege, Enabled, RemoveFace, RemoveFinger, RemovePhoto);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeFFCPGet([FromServices] IEmployee db, int ID)
        {
            return Json(await db.GetEmployeeFFCPTemplate(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> SetEmployeePhoto([FromServices] IEmployee db, int PhotoTableID, int EmpID, IFormFile UserPhoto, string operation = "")
        {
            if (UserPhoto.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    UserPhoto.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string PhotoBase64 = Convert.ToBase64String(fileBytes);
                   await db.PostEmployeeFFCPPhoto(operation,PhotoTableID, EmpID, PhotoBase64);
                }
            }
            return "OK";
        }

        #endregion

        #region Employee Salary

        #region Structure

        [AjaxOnly]
        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeSalaryLoad([FromServices] IEmployee db, [FromServices] OreasDbContext context,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {



            PagedData<object> pageddata =
                await db.LoadEmployeeSalary(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> EmployeeSalaryPost([FromServices] IEmployee db, [FromBody] tbl_WPT_EmployeeSalaryStructure tbl_WPT_EmployeeSalaryStructure, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostEmployeeSalary(tbl_WPT_EmployeeSalaryStructure, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeSalaryGet([FromServices] IEmployee db, int ID)
        {
            return Json(await db.GetEmployeeSalary(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Allowance

        [AjaxOnly]
        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeSalaryAllowanceLoad([FromServices] IEmployee db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadEmployeeSalaryAllowance(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> EmployeeSalaryAllowancePost([FromServices] IEmployee db, [FromBody] tbl_WPT_EmployeeSalaryStructureAllowance tbl_WPT_DepartmentDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostEmployeeSalaryAllowance(tbl_WPT_DepartmentDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeSalaryAllowanceGet([FromServices] IEmployee db, int ID)
        {
            return Json(await db.GetEmployeeSalaryAllowance(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Deductible

        [AjaxOnly]
        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeSalaryDeductibleLoad([FromServices] IEmployee db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadEmployeeSalaryDeductible(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> EmployeeSalaryDeductiblePost([FromServices] IEmployee db, [FromBody] tbl_WPT_EmployeeSalaryStructureDeductible tbl_WPT_EmployeeSalaryStructureDeductible, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostEmployeeSalaryDeductible(tbl_WPT_EmployeeSalaryStructureDeductible, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeSalaryDeductibleGet([FromServices] IEmployee db, int ID)
        {
            return Json(await db.GetEmployeeSalaryDeductible(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #endregion

        #region Employee Pension

        [AjaxOnly]
        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeePensionLoad([FromServices] IEmployee db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadEmployeePension(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> EmployeePensionPost([FromServices] IEmployee db, [FromBody] tbl_WPT_EmployeePensionStructure tbl_WPT_EmployeePensionStructure, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostEmployeePension(tbl_WPT_EmployeePensionStructure, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeePensionGet([FromServices] IEmployee db, int ID)
        {
            return Json(await db.GetEmployeePension(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion        

        #region Employee Bank

        [AjaxOnly]
        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeBankLoad([FromServices] IEmployee db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {

            PagedData<object> pageddata =
                await db.LoadEmployeeBank(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Employee", Operation = "CanPost")]
        public async Task<string> EmployeeBankPost([FromServices] IEmployee db, [FromBody] tbl_WPT_EmployeeBankDetail tbl_WPT_EmployeeBankDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostEmployeeBank(tbl_WPT_EmployeeBankDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Employee", Operation = "CanView")]
        public async Task<IActionResult> EmployeeBankGet([FromServices] IEmployee db, int ID)
        {
            return Json(await db.GetEmployeeBank(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion


        #endregion

        [MyAuthorization(FormName = "Employee", Operation = "CanViewReport")]
        public async Task<IActionResult> GetReport([FromServices] IEmployee db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

    }
}
