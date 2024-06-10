using OreasModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public interface IWPTList
    {
        Task<object> GetGroupEmailListAsync(int DesignationID = 0, int DepartmentID = 0);
        Task<object> GetDesignationListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetEmployeeLevelListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetEducationalLevelTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetHolidayListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetDepartmentListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetSectionListAsync(string QueryName = "", string SecFilterByText = "", string SecFilterValueByText = "", int FormID = 0, string UserName = "");
        Task<object> GetAllowanceTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetDeductibleTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetCalculationMethodListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetOTPolicyListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetBankListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetCompanyBankAcListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetLoanTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetEmploymentTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetHiringTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetRewardTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetInActiveTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetATypeListAsync(string FilterByText = null, string FilterValueByText = null);
        //-----------------------------------------------------------------------------------------------------//
        Task<object> GetEmployeesListAsync(string QueryName = "", string EmployeeFilterBy = "", string EmployeeFilterValue = "", int FormID = 0);
        Task<object> GetLeavePoliciesWithBalanceByEmployeeListAsync(string QueryName = "", int EmployeeID = 0, int MonthID = 0);
        Task<object> GetMonthListAsync(string QueryName = "", int SearchMonth = 1, int SearchYear = 2000, int FormID = 0);
        //-----------------------------------------------------------------------------------------------------//
        Task<object> GetShiftListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetTransactionModeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetWageCalculationTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetATInOutModeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetLeaveCFOptionsListAsync(string FilterByText = null, string FilterValueByText = null);
        object GetEncashablePeriodList();
        Task<object> GetPaidLeavePolicyListAsync(string FilterByText = null, string FilterValueByText = null);        
        Task<object> GetActionList();
        Task<object> GetIncrementByList();
        Task<object> GetIncentiveTypeList();
    }
    public interface IDesignation
    {       
        object GetWCLDesignation();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_Designation tbl_WPT_designation, string operation, string userName);
        Task<object> Get(int ID);
        List<ReportCallingModel> GetRLDesignation();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0);

    }
    public interface IEmployeeLevel
    {
        
        object GetWCLEmployeeLevel();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_EmployeeLevel tbl_WPT_EmployeeLevel, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IEducationalLevelType
    {
       
        object GetWCLEducationalLevelType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_EducationalLevelType tbl_WPT_EducationalLevelType, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IHoliday
    {
       
        object GetWCLHoliday();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_Holiday tbl_WPT_Holiday, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IDepartment
    {
      
        #region Master
        Task<object> GetNodesAsync(int PID = 0);
        object GetWCLDepartment();
        Task<PagedData<object>> LoadDepartment(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostDepartment(tbl_WPT_Department tbl_WPT_Department, string operation, string userName);

        Task<object> GetDepartment(int ID);

        #endregion

        #region Detail Designation
        object GetWCLDepartmentDesignation();
        Task<PagedData<object>> LoadDepartmentDesignation(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostDepartmentDesignation(tbl_WPT_DepartmentDetail tbl_WPT_DepartmentDetail, string operation, string userName);

        Task<object> GetDepartmentDesignation(int ID);

        #endregion

        #region Detail Section
        object GetWCLDepartmentSection();
        Task<PagedData<object>> LoadDepartmentSection(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostDepartmentSection(tbl_WPT_DepartmentDetail_Section tbl_WPT_DepartmentDetail_Section, string operation, string userName);

        Task<object> GetDepartmentSection(int ID);

        #endregion

        #region Detail Section HOS
        object GetWCLDepartmentSectionHOS();
        Task<PagedData<object>> LoadDepartmentSectionHOS(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostDepartmentSectionHOS(tbl_WPT_DepartmentDetail_Section_HOS tbl_WPT_DepartmentDetail_Section_HOS, string operation, string userName);

        Task<object> GetDepartmentSectionHOS(int ID);

        #endregion
    }
    public interface IAllowanceType
    {
        
        object GetWCLAllowanceType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_AllowanceType tbl_WPT_AllowanceType, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IDeductibleType
    {
       
        object GetWCLDeductibleType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_DeductibleType tbl_WPT_DeductibleType, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IOTPolicy
    {      

        object GetWCLOTPolicy();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_tbl_OTPolicy tbl_WPT_tbl_OTPolicy, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IBank
    {       

        #region Bank Master
        object GetWCLBankMaster();

        Task<PagedData<object>> LoadBankMaster(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostBankMaster(tbl_WPT_Bank tbl_WPT_Bank, string operation, string userName);

        Task<object> GetBankMaster(int ID);

        #endregion

        #region Bank Detail Branch
        object GetWCLBankDetailBranch();
        Task<PagedData<object>> LoadBankDetailBranch(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostBankDetailBranch(tbl_WPT_Bank_Branch tbl_WPT_Bank_Branch, string operation, string userName);
        Task<object> GetBankDetailBranch(int ID);

        #endregion

        #region Bank Detail Branch Company Ac
        object GetWCLBankDetailBranchCompanyAc();
        Task<PagedData<object>> LoadBankDetailBranchCompanyAc(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostBankDetailBranchCompanyAc(tbl_WPT_CompanyBankDetail tbl_WPT_CompanyBankDetail, string operation, string userName);
        Task<object> GetBankDetailBranchCompanyAc(int ID);

        #endregion

        #region Bank Detail Branch Employee Ac
        object GetWCLBankDetailBranchEmployeeAc();
        Task<PagedData<object>> LoadBankDetailBranchEmployeeAc(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostBankDetailBranchEmployeeAc(tbl_WPT_EmployeeBankDetail tbl_WPT_EmployeeBankDetail, string operation, string userName);
        Task<object> GetBankDetailBranchEmployeeAc(int ID);

        #endregion
    }
    public interface ILoanType
    {       

        object GetWCLLoanType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_LoanType tbl_WPT_LoanType, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IEmploymentType
    {
        object GetWCLEmploymentType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_EmploymentType tbl_WPT_EmploymentType, string operation, string userName);
        Task<object> Get(int ID);
    }
    public interface IHiringType
    {
        object GetWCLHiringType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_HiringType tbl_WPT_HiringType, string operation, string userName);
        Task<object> Get(int ID);
    }
    public interface IRewardType
    {
        object GetWCLRewardType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_RewardType tbl_WPT_RewardType, string operation, string userName);
        Task<object> Get(int ID);
    }
    public interface IInActiveType
    {
        object GetWCLInActiveType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_InActiveType tbl_WPT_InActiveType, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IEmployee
    {
        #region Employee
        object GetWCLEmployee();
        Task<PagedData<object>> LoadEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostEmployee(VM_EmployeeEnrollment VM_EmployeeEnrollment, string operation, string userName);
        Task<string> EmployeeUploadExcelFile(List<EmployeeExcelData> EmployeeExcelDataList, string operation, string userName);
        Task<object> GetEmployee(int ID);
        Task<tbl_WPT_Employee> GetEmployeeObject(int ID);
        Task<List<int>> GetEmployeeIDListObject();

        #endregion

        #region Employee Face Finger Card Pin

        Task<object> GetEmployeeFFCPTemplate(int EmpID);
        Task<string> PostEmployeeFFCPTemplate(string operation, string userName, int EmpID, string CardNo = null, string Paswd = null, int Privilege = 0, bool Enabled = true, bool RemoveFace = false, bool RemoveFinger = false, bool RemovePhoto = false);
        Task<string> PostEmployeeFFCPPhoto(string operation, int PhotoTableID, int EmpID, string PhotoBase64);
        Task<string> PostEmployeeFFCPTemplateObject(tbl_WPT_Employee_PFF tbl_WPT_Employee_PFF, string operation, string userName);
        Task<tbl_WPT_Employee_PFF> GetEmployeeFFCPTemplateObject(int EmpID);
        #endregion

        #region Employee Salary

        #region structure
        object GetWCLEmployeeSalary();
        Task<PagedData<object>> LoadEmployeeSalary(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostEmployeeSalary(tbl_WPT_EmployeeSalaryStructure tbl_WPT_EmployeeSalaryStructure, string operation, string userName);
        Task<object> GetEmployeeSalary(int ID);
        #endregion

        #region Allowance

        Task<PagedData<object>> LoadEmployeeSalaryAllowance(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostEmployeeSalaryAllowance(tbl_WPT_EmployeeSalaryStructureAllowance tbl_WPT_EmployeeSalaryStructureAllowance, string operation, string userName);
        Task<object> GetEmployeeSalaryAllowance(int ID);
        #endregion

        #region Deductible

        Task<PagedData<object>> LoadEmployeeSalaryDeductible(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostEmployeeSalaryDeductible(tbl_WPT_EmployeeSalaryStructureDeductible tbl_WPT_EmployeeSalaryStructureDeductible, string operation, string userName);
        Task<object> GetEmployeeSalaryDeductible(int ID);
        #endregion

        #endregion

        #region Employee Pension

        Task<PagedData<object>> LoadEmployeePension(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostEmployeePension(tbl_WPT_EmployeePensionStructure tbl_WPT_EmployeePensionStructure, string operation, string userName);
        Task<object> GetEmployeePension(int ID);
        #endregion

        #region Employee Bank

        Task<PagedData<object>> LoadEmployeeBank(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostEmployeeBank(tbl_WPT_EmployeeBankDetail tbl_WPT_EmployeeBankDetail, string operation, string userName);
        Task<object> GetEmployeeBank(int ID);
        #endregion

        #region Report   

        List<ReportCallingModel> GetRLEmployee();
        List<ReportCallingModel> GetRLEmployeeLetter();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion

    }
    public interface IPolicyGeneral
    {
        Task<object> Load();
        Task<string> Post(tbl_WPT_PolicyGeneral tbl_WPT_PolicyGeneral, string operation, string userName);
    }
    public interface IPolicyPenalty
    {
        #region Policy Penalty

        object GetWCLPolicyPenalty();
        Task<PagedData<object>> LoadPolicyPenalty(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostPolicyPenalty(tbl_WPT_PolicyPenaltyOnWT tbl_WPT_PolicyPenaltyOnWT, string operation, string userName);

        Task<object> GetPolicyPenalty(int ID);

        #endregion

        #region Policy Penalty Designation
        object GetWCLPolicyPenaltyDesignation();
        Task<PagedData<object>> LoadPolicyPenaltyDesignation(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostPolicyPenaltyDesignation(tbl_WPT_PolicyPenaltyOnWTDetail_Designation tbl_WPT_PolicyPenaltyOnWTDetail_Designation, string operation, string userName);

        Task<object> GetPolicyPenaltyDesignation(int ID);

        #endregion

    }
    public interface IShift
    {
        #region Shift

        object GetWCLShift();
        Task<PagedData<object>> LoadShift(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostShift(tbl_WPT_Shift tbl_WPT_Shift, string operation, string userName);
        Task<object> GetShift(int ID);

        #endregion

        #region Default Employees Shift
        object GetLBLDefaultEmployeeShift();
        object GetWCLDefaultEmployeeShift();
        Task<PagedData<object>> LoadDefaultEmployeeShift(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostDefaultEmployeeShiftBulk(int ShiftID = 0, int SectionID = 0, int DesignationID = 0, string BulkBy = "All", string userName = "");

        #endregion

    }
    public interface IMachine
    {
        object GetWCLMachine();
        Task<PagedData<object>> LoadMachine(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostMachine(tbl_WPT_Machine tbl_WPT_Machine, string operation, string userName);
        Task<object> GetMachine(int ID);
        Task<tbl_WPT_Machine> GetMachineObject(int id);
        Task<string> PostMachineAttendance(DataTable _table);
        Task<string> PostMachineAttendanceClear(int MachineID, int NoOfRecords);
    }
    public interface ITransactionMode
    {
        object GetWCLTransactionMode();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> Post(tbl_WPT_TransactionMode tbl_WPT_TransactionMode, string operation, string userName);
        Task<object> Get(int ID);

    }
    public interface IATTimeGrace
    {
        #region Master
        object GetWCLATTimeGrace();
        Task<PagedData<object>> LoadATTimeGrace(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostATTimeGrace(tbl_WPT_ATTimeGrace tbl_WPT_ATTimeGrace, string operation, string userName);
        Task<object> GetATTimeGrace(int ID);

        #endregion

        #region Detail Link Employee
        object GetWCLATTimeGraceEmployee();
        Task<PagedData<object>> LoadATTimeGraceEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostATTimeGraceEmployee(tbl_WPT_ATTimeGraceEmployeeLink tbl_WPT_ATTimeGraceEmployeeLink, string operation, string userName);
        Task<object> GetATTimeGraceEmployee(int ID);
        Task<string> ATTimeGraceEmployeeUploadExcelFile(List<string> ATGraceExcelDataList, string operation, string userName, int MasterID = 0);

        #endregion
    }
    public interface IAttendance
    {
        Task<object> GetLastOpenMonth();

        #region Attendance Individual
        Task<PagedData<object>> LoadAttendanceIndividual(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostAttendanceIndividual(tbl_WPT_AttendanceLog tbl_WPT_AttendanceLog, string operation, string userName);
        Task<object> GetAttendanceIndividual(int ID);

        #endregion

        #region Attendance Together
        object GetWCLATTogether();
        Task<PagedData<object>> LoadATTogether(int CurrentPage = 1, DateTime? MonthStart = null, DateTime? MonthEnd = null, string FilterByText = null, string FilterValueByText = null);
        #endregion

        #region Report        
        List<ReportCallingModel> GetRLAttendance();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion

    }
    public interface ILeavePolicy
    {
        #region Leave Policy Paid

        object GetWCLLeavePolicy();
        Task<PagedData<object>> LoadLeavePolicy(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostLeavePolicy(tbl_WPT_LeavePolicy tbl_WPT_LeavePolicy, string operation, string userName);
        Task<object> GetLeavePolicy(int ID);

        #endregion

        #region Leave Policy Non Paid

        object GetWCLLeavePolicyNonPaid();
        Task<PagedData<object>> LoadLeavePolicyNonPaid(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostLeavePolicyNonPaid(tbl_WPT_LeavePolicyNonPaid tbl_WPT_LeavePolicyNonPaid, string operation, string userName);
        Task<object> GetLeavePolicyNonPaid(int ID);

        #endregion

        #region Leave Policy Non Paid Designation

        object GetWCLLeavePolicyNonPaidDesignation();
        Task<PagedData<object>> LoadLeavePolicyNonPaidDesignation(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostLeavePolicyNonPaidDesignation(tbl_WPT_LeavePolicyNonPaid_Designation tbl_WPT_LeavePolicyNonPaid_Designation, string operation, string userName);
        Task<object> GetLeavePolicyNonPaidDesignation(int ID);

        #endregion
    }
    public interface ICalendar
    {

        #region Calendar

        object GetWCLCalendar();
        Task<PagedData<object>> LoadCalendar(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostCalendar(tbl_WPT_CalendarYear tbl_WPT_CalendarYear, string operation, string userName);
        Task<object> GetCalendar(int ID);
        Task<string> CloseYear(int CalendarID, string userName);

        #endregion

        #region Calendar Month

        object GetWCLCalendarMonth();
        Task<PagedData<object>> LoadCalendarMonth(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostCalendarMonth(VM_CalendarYear_Months_Adjustment VM_CalendarYear_Months_Adjustment, string operation, string userName);
        Task<object> GetCalendarMonth(int ID);

        #endregion

        #region Calendar Employee For Paid Leave

        object GetWCLCalendarEmployeeForPL();
        Task<PagedData<object>> LoadCalendarEmployeeForPL(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostCalendarEmployeeForPL(tbl_WPT_CalendarYear_LeaveEmps tbl_WPT_CalendarYear_LeaveEmps, string operation, string userName);
        Task<object> GetCalendarEmployeeForPL(int ID);

        #endregion

        #region Calendar Paid Leaves of Employees

        object GetWCLCalendarPLOfEmployee();
        Task<PagedData<object>> LoadCalendarPLOfEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostCalendarPLOfEmployee(tbl_WPT_CalendarYear_LeaveEmps_Leaves tbl_WPT_CalendarYear_LeaveEmps_Leaves, string operation, string userName);
        Task<object> GetCalendarPLOfEmployee(int ID);

        #endregion

        #region Report       
        
        List<ReportCallingModel> GetRLCalendar();

        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion

    }
    public interface ILeaveRequisition
    {
        #region Calendar Month
        object GetWCLCalendarMonth();
        Task<PagedData<object>> LoadCalendarMonth(int CurrentPage = 1, int MasterID = 0,
                 string FilterByText = null, string FilterValueByText = null,
                 string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
                 string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
                 string FilterByLoad = null);

        #endregion

        #region Leave Requisition
        object GetWCLLeaveRequisition();
        object GetWCLBLeaveRequisition();
        Task<PagedData<object>> LoadLeaveRequisition(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostLeaveRequisition(tbl_WPT_LeaveRequisition tbl_WPT_LeaveRequisition, string operation, string userName);
        Task<object> GetLeaveRequisition(int ID);

        #endregion
    }
    public interface IIncentivePolicy
    {

        #region Master

        object GetWCLIncentiveMaster();
        Task<PagedData<object>> LoadIncentiveMaster(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostIncentiveMaster(tbl_WPT_IncentivePolicy tbl_WPT_IncentivePolicy, string operation, string userName);
        Task<object> GetIncentiveMaster(int ID);

        #endregion

        #region Detail Designation

        object GetWCLIncentiveDetailDesignation();
        Task<PagedData<object>> LoadIncentiveDetailDesignation(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostIncentiveDetailDesignation(tbl_WPT_IncentivePolicyDesignation tbl_WPT_IncentivePolicyDesignation, string operation, string userName);
        Task<object> GetIncentiveDetailDesignation(int ID);

        #endregion

        #region Detail Employee

        object GetWCLIncentiveDetailEmployee();
        Task<PagedData<object>> LoadIncentiveDetailEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostIncentiveDetailEmployee(tbl_WPT_IncentivePolicyEmployees tbl_WPT_IncentivePolicyEmployees, string operation, string userName);
        Task<object> GetIncentiveDetailEmployee(int ID);

        #endregion
    }
    public interface IReward
    {

        #region Master

        object GetWCLRewardMaster();
        Task<PagedData<object>> LoadRewardMaster(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostRewardMaster(tbl_WPT_RewardMaster tbl_WPT_RewardMaster, string operation, string userName);
        Task<object> GetRewardMaster(int ID);

        #endregion        

        #region Detail Employee

        object GetWCLRewardDetailEmployee();
        Task<PagedData<object>> LoadRewardDetailEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostRewardDetailEmployee(tbl_WPT_RewardDetail tbl_WPT_RewardDetail, string operation, string userName, int? MasterID = 0, int? DesignationID = 0, int? DepartmentID = 0, DateTime? JoiningDate = null);
        Task<object> GetRewardDetailEmployee(int ID);
        Task<string> RewardDetailEmployeeUploadExcelFile(List<RewardExcelData> RewardExcelDataList, string operation, string userName, int MasterID = 0);

        #endregion

        #region Detail Payment

        object GetWCLRewardDetailPayment();
        Task<PagedData<object>> LoadRewardDetailPayment(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostRewardDetailPayment(tbl_WPT_RewardDetail_Payment tbl_WPT_RewardDetail_Payment, string operation, string userName);
        Task<object> GetRewardDetailPayment(int ID);

        #endregion

        #region Detail Payment Employees

        object GetWCLRewardDetailPaymentEmployee();
        Task<PagedData<object>> LoadRewardDetailPaymentEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostRewardDetailPaymentEmployee(string operation, string userName, int tbl_WPT_RewardDetailID = 0, int RewardPaymentID = 0, int DepartmentID = 0, int DesignationID = 0);
        Task<object> GetRewardDetailPaymentEmployee(int ID);

        #endregion

        #region Report       

        List<ReportCallingModel> GetRLReward();
        List<ReportCallingModel> GetRLRewardPayment();

        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion

    }
    public interface ILoan
    {

        #region Master

        object GetWCLLoanMaster();
        object GetWCLBLoanMaster();
        Task<PagedData<object>> LoadLoanMaster(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostLoanMaster(tbl_WPT_LoanMaster tbl_WPT_LoanMaster, string operation, string userName);
        Task<object> GetLoanMaster(int ID);

        #endregion        

        #region Detail Employee

        object GetWCLLoanDetailEmployee();
        Task<PagedData<object>> LoadLoanDetailEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostLoanDetailEmployee(tbl_WPT_LoanDetail tbl_WPT_LoanDetail, string operation, string userName, int? MasterID = 0, int? DesignationID = 0, int? DepartmentID = 0, DateTime? JoiningDate = null);
        Task<object> GetLoanDetailEmployee(int ID);
        Task<string> LoanDetailEmployeeUploadExcelFile(List<LoanExcelData> LoanExcelDataList, string operation, string userName, int MasterID = 0);

        #endregion

        #region Detail Payment

        object GetWCLLoanDetailPayment();
        Task<PagedData<object>> LoadLoanDetailPayment(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostLoanDetailPayment(tbl_WPT_LoanDetail_Payment tbl_WPT_LoanDetail_Payment, string operation, string userName);
        Task<object> GetLoanDetailPayment(int ID);

        #endregion

        #region Detail Payment Employees

        object GetWCLLoanDetailPaymentEmployee();
        Task<PagedData<object>> LoadLoanDetailPaymentEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostLoanDetailPaymentEmployee(string operation, string userName, int tbl_WPT_LoanDetailID = 0, int LoanPaymentID = 0, int DepartmentID = 0, int DesignationID = 0);
        Task<object> GetLoanDetailPaymentEmployee(int ID);

        #endregion

        #region Loan Individual
        Task<PagedData<object>> LoadLoanIndividual(int CurrentPage = 1, int EmployeeID = 0, DateTime DateTill = new DateTime(), int? LoanTypeID = 0);

        #endregion

        #region Report       

        List<ReportCallingModel> GetRLLoan();
        List<ReportCallingModel> GetRLLoanDetail();
        List<ReportCallingModel> GetRLLoanPayment();
        List<ReportCallingModel> GetRLLoanIndividual();

        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion

    }
    public interface IIncrement
    {
        #region Calendar
        object GetWCLIncrementCalendar();
        Task<PagedData<object>> LoadIncrementCalendar(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        #endregion

        #region Master

        object GetWCLIncrementMaster();
        Task<PagedData<object>> LoadIncrementMaster(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostIncrementMaster(tbl_WPT_IncrementMaster tbl_WPT_IncrementMaster, string operation, string userName);
        Task<object> GetIncrementMaster(int ID);

        #endregion        

        #region Detail Employee

        object GetWCLIncrementDetailEmployee();
        Task<PagedData<object>> LoadIncrementDetailEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostIncrementDetailEmployee(tbl_WPT_IncrementDetail tbl_WPT_IncrementDetail, string operation, string userName, int? MasterID = 0, int? DesignationID = 0, int? DepartmentID = 0, DateTime? JoiningDate = null);
        Task<object> GetIncrementDetailEmployee(int ID);
        Task<string> IncrementDetailEmployeeUploadExcelFile(List<IncrementExcelData> IncrementExcelDataList, string operation, string userName, int MasterID = 0);

        #endregion         

        #region Report       

        List<ReportCallingModel> GetRLIncrement();

        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion
    }
    public interface IPayRun
    {
        #region Calendar
        object GetWCLPayRunCalendar();
        Task<PagedData<object>> LoadPayRunCalendar(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostPayRunCalendarOpenClose(int MonthID, bool MonthIsClosed, string userName);

        #endregion               

        #region PayRun TO Do

        object GetWCLPayRunToDo();
        Task<PagedData<object>> LoadPayRunToDo(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunToDo(tbl_WPT_PayRunToDo tbl_WPT_PayRunToDo, string operation, string userName);
        Task<object> GetPayRunToDo(int ID);

        #endregion        

        #region PayRun Exemption

        object GetWCLPayRunExempt();
        Task<PagedData<object>> LoadPayRunExempt(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunExempt(tbl_WPT_PayRunExemption tbl_WPT_PayRunExemption, string operation, string userName);
        Task<object> GetPayRunExempt(int ID);

        #endregion        

        #region PayRun Exemption Employee

        object GetWCLPayRunExemptEmployee();
        Task<PagedData<object>> LoadPayRunExemptEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunExemptEmployee(tbl_WPT_PayRunExemption_Emp tbl_WPT_PayRunExemption_Emp, string operation, string userName);
        Task<object> GetPayRunExemptEmployee(int ID);

        #endregion

        #region PayRun Holiday

        Task<PagedData<object>> LoadPayRunHoliday(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunHoliday(tbl_WPT_CalendarYear_Months_Holidays tbl_WPT_CalendarYear_Months_Holidays, string operation, string userName);
        Task<object> GetPayRunHoliday(int ID);

        #endregion

        #region Payrun Leave Requisition
        //Used LeaveRequisition interface
        #endregion

        #region PayRun Roster Master

        object GetWCLPayRunRosterMaster();
        Task<PagedData<object>> LoadPayRunRosterMaster(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunRosterMaster(tbl_WPT_ShiftRosterMaster tbl_WPT_ShiftRosterMaster, string operation, string userName);
        Task<object> GetPayRunRosterMaster(int ID);

        #endregion

        #region PayRun Roster Detail Shift

        Task<PagedData<object>> LoadPayRunRosterDetailShift(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunRosterDetailShift(tbl_WPT_ShiftRosterDetail tbl_WPT_ShiftRosterDetail, string operation, string userName);
        Task<object> GetPayRunRosterDetailShift(int ID);

        #endregion

        #region PayRun Roster Detail Employee

        object GetWCLPayRunRosterDetailEmployee();
        Task<PagedData<object>> LoadPayRunRosterDetailEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunRosterDetailEmployee(tbl_WPT_ShiftRosterDetail_Employee tbl_WPT_ShiftRosterDetail_Employee, string operation, string userName, int? MasterID = 0, int? DesignationID = 0, int? DepartmentID = 0, DateTime? JoiningDate = null);
        Task<object> GetPayRunRosterDetailEmployee(int ID);

        #endregion

        //--------------------------------------------------------------------------------------------------------//

        #region PayRun Master

        Task<PagedData<object>> LoadPayRunMaster(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunMaster(tbl_WPT_PayRunMaster tbl_WPT_PayRunMaster, string operation, string userName);
        Task<object> GetPayRunMaster(int ID);

        #endregion        

        #region PayRun Detail Employee

        object GetWCLPayRunMasterDetailEmployee();
        Task<PagedData<object>> LoadPayRunMasterDetailEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunMasterDetailEmployee(tbl_WPT_PayRunDetail_Emp tbl_WPT_PayRunDetail_Emp, string operation, string userName);
        Task<object> GetPayRunMasterDetailEmployee(int ID);
        Task<string> PostPayRunProcessMasterDetailEmployee(int ID, bool PayRun, string userName);
        Task<List<int>> GetPayRunIDForBulk(int MasterID, string listfor);
        Task<List<PayRunDetail_Emp_EmailDetail>> GetPayRunDetail_Emp_EmailDetailList(int MasterID);

        #endregion        

        #region PayRun Detail Employee AT

        Task<PagedData<object>> LoadPayRunMasterDetailEmployeeAT(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);


        #endregion        

        #region PayRun Detail Employee Wage
        Task<PagedData<object>> LoadPayRunMasterDetailEmployeeWage(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        #endregion        

        #region PayRun Detail Payment

        object GetWCLPayRunDetailPayment();
        Task<PagedData<object>> LoadPayRunDetailPayment(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunDetailPayment(tbl_WPT_PayRunDetail_Payment tbl_WPT_PayRunDetail_Payment, string operation, string userName);
        Task<object> GetPayRunDetailPayment(int ID);

        #endregion

        #region PayRun Detail Payment Employees

        object GetWCLPayRunDetailPaymentEmployee();
        Task<PagedData<object>> LoadPayRunDetailPaymentEmployee(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostPayRunDetailPaymentEmployee(string operation, string userName, int tbl_WPT_PayRunDetail_EmpID = 0, int PayRunPaymentID = 0, int DepartmentID = 0, int DesignationID = 0);
        Task<object> GetPayRunDetailPaymentEmployee(int ID);

        #endregion

        #region Report       

        List<ReportCallingModel> GetRLPayRunDetail();

        Task<byte[]> GetPDFFilePayRunDetailAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        List<ReportCallingModel> GetRLPayRunPayment();

        Task<byte[]> GetPDFFilePayRunPaymentAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion
    }
    public interface IWPTDashboard
    {
        Task<object> GetDashBoardData(string userName = "");
    }
}
