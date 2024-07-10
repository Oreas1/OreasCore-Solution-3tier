using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OreasModel
{

    #region

    [Table("tbl_WPT_CalculationMethod")]
    public class tbl_WPT_CalculationMethod
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Method Name")]
        public string MethodName { get; set; }

        [Required]
        [Display(Name = "On Basic Salary")]
        public bool OnBasicSalary { get; set; }

        [Required]
        [Display(Name = "On Net Salary")]
        public bool OnNetSalary { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeavePolicy.tbl_WPT_CalculationMethods))]
        public virtual ICollection<tbl_WPT_LeavePolicy> tbl_WPT_LeavePolicys { get; set; }

        [InverseProperty(nameof(tbl_WPT_tbl_OTPolicy.tbl_WPT_CalculationMethod))]
        public virtual ICollection<tbl_WPT_tbl_OTPolicy> tbl_WPT_tbl_OTPolicys { get; set; }

    }

    [Table("tbl_WPT_ActionList")]
    public class tbl_WPT_ActionList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Action Name")]
        public string ActionName { get; set; }

        [Required]
        [Display(Name = "Action Value")]
        public bool ActionValue { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_ActionList_HOD))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions_HOD { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_ActionList_HR))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions_HR { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_ActionList_Final))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions_Final { get; set; }

    }

    [Table("tbl_WPT_Bank")]
    public class tbl_WPT_Bank
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_Bank_Branch.tbl_WPT_Bank))]
        public virtual ICollection<tbl_WPT_Bank_Branch> tbl_WPT_Bank_Branchs { get; set; }

    }

    [Table("tbl_WPT_Bank_Branch")]
    public class tbl_WPT_Bank_Branch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Bank))]

        public int FK_tbl_WPT_Bank_ID { get; set; }
        public virtual tbl_WPT_Bank tbl_WPT_Bank { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [MaxLength(50)]
        [Display(Name = "City")]
        public string City { get; set; }

        [MaxLength(100)]
        [Display(Name = "Postal Address")]
        public string PostalAddress { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeBankDetail.tbl_WPT_Bank_Branch))]
        public virtual ICollection<tbl_WPT_EmployeeBankDetail> tbl_WPT_EmployeeBankDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_CompanyBankDetail.tbl_WPT_Bank_Branch))]
        public virtual ICollection<tbl_WPT_CompanyBankDetail> tbl_WPT_CompanyBankDetails { get; set; }

    }

    [Table("tbl_WPT_CompanyBankDetail")]
    public class tbl_WPT_CompanyBankDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Bank_Branch))]

        public int FK_tbl_WPT_Bank_Branch_ID { get; set; }
        public virtual tbl_WPT_Bank_Branch tbl_WPT_Bank_Branch { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Bank A/c No")]
        public string BankAccountNo { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Bank A/C Title")]
        public string BankAccountTitle { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Payment.tbl_WPT_CompanyBankDetail))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Payment> tbl_WPT_PayRunDetail_Payments { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardDetail_Payment.tbl_WPT_CompanyBankDetail))]
        public virtual ICollection<tbl_WPT_RewardDetail_Payment> tbl_WPT_RewardDetail_Payments { get; set; }

        [InverseProperty(nameof(tbl_WPT_LoanDetail_Payment.tbl_WPT_CompanyBankDetail))]
        public virtual ICollection<tbl_WPT_LoanDetail_Payment> tbl_WPT_LoanDetail_Payments { get; set; }

    }

    [Table("tbl_WPT_TransactionMode")]
    public class tbl_WPT_TransactionMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Transaction Mode")]
        public string TransactionMode { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructure.tbl_WPT_TransactionMode))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructure> tbl_WPT_EmployeeSalaryStructures { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructure.tbl_WPT_TransactionMode_Secondary))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructure> tbl_WPT_EmployeeSalaryStructures_Secondary { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Emp.tbl_WPT_TransactionMode_Primary))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emp_Primarys { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Emp.tbl_WPT_TransactionMode_Secondary))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emp_Secondarys { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Payment.tbl_WPT_TransactionMode))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Payment> tbl_WPT_PayRunDetail_Payments { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardDetail_Payment.tbl_WPT_TransactionMode))]
        public virtual ICollection<tbl_WPT_RewardDetail_Payment> tbl_WPT_RewardDetail_Payments { get; set; }

        [InverseProperty(nameof(tbl_WPT_LoanDetail_Payment.tbl_WPT_TransactionMode))]
        public virtual ICollection<tbl_WPT_LoanDetail_Payment> tbl_WPT_LoanDetail_Payments { get; set; }

    }

    [Table("tbl_WPT_EducationalLevelType")]
    public class tbl_WPT_EducationalLevelType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Level Name")]
        public string LevelName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee.tbl_WPT_EducationalLevelType))]
        public virtual ICollection<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }

        [InverseProperty(nameof(tbl_WPT_JobPositionMaster.tbl_WPT_EducationalLevelType))]
        public virtual ICollection<tbl_WPT_JobPositionMaster> tbl_WPT_JobPositionMasters { get; set; }

        [InverseProperty(nameof(tbl_WPT_HiringRequest.tbl_WPT_EducationalLevelType))]
        public virtual ICollection<tbl_WPT_HiringRequest> tbl_WPT_HiringRequests { get; set; }

        [InverseProperty(nameof(tbl_WPT_JobApplication.tbl_WPT_EducationalLevelType))]
        public virtual ICollection<tbl_WPT_JobApplication> tbl_WPT_JobApplications { get; set; }

    }

    [Table("tbl_WPT_Machine")]
    public class tbl_WPT_Machine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Machine Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Machine No")]
        public int No { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "IP Address")]
        public string IP { get; set; }

        [Required]
        [Display(Name = "Port No")]
        public int PortNo { get; set; }

        [Required]
        [Display(Name = "Auto ClearLog After Download")]
        public bool AutoClearLogAfterDownload { get; set; }

        [Display(Name = "Last ATLog Downloanded")]
        public DateTime? LastATLogDownloanded { get; set; }

        [Display(Name = "Last LATLog Clear")]
        public DateTime? LastATLogClear { get; set; }

        [Required]
        [Display(Name = "Last ATLog Count")]
        public int LastATLogCount { get; set; }

        [Display(Name = "Scheduled Download Daily At")]
        public TimeSpan? ScheduledDownloadDailyAT { get; set; }

        [Display(Name = "Scheduled Download Daily At2")]
        public TimeSpan? ScheduledDownloadDailyAT2 { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_AttendanceLog.tbl_WPT_Machine))]
        public virtual ICollection<tbl_WPT_AttendanceLog> tbl_WPT_AttendanceLogs { get; set; }

    }

    [Table("tbl_WPT_ATInOutMode")]
    public class tbl_WPT_ATInOutMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "AT InOut Mode Name")]
        public string ATInOutModeName { get; set; }

        [Required]
        [Display(Name = "AT InOut Mode Value")]
        public int ATInOutMode { get; set; }

        [InverseProperty(nameof(tbl_WPT_ATBulkManualMaster.tbl_WPT_ATInOutMode))]
        public virtual ICollection<tbl_WPT_ATBulkManualMaster> tbl_WPT_ATBulkManualMasters { get; set; }
    }

    [Table("tbl_WPT_ATType")]
    public class tbl_WPT_ATType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "AT Type")]
        public string ATType { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee.tbl_WPT_ATType))]
        public virtual ICollection<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }

    }

    [Table("tbl_WPT_InActiveType")]
    public class tbl_WPT_InActiveType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "In Active Type")]
        public string InActiveType { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee.tbl_WPT_InActiveType))]
        public virtual ICollection<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }

    }

    //--------------------------------Department --------------------------------//
    [Table("tbl_WPT_Department")]
    public class tbl_WPT_Department
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [ForeignKey(nameof(tbl_WPT_Department_Head))]
        public int? FK_tbl_WPT_Department_ID_Head { get; set; }
        public virtual tbl_WPT_Department tbl_WPT_Department_Head { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------self relation----------------------//
        [InverseProperty(nameof(tbl_WPT_Department.tbl_WPT_Department_Head))]
        public virtual ICollection<tbl_WPT_Department> tbl_WPT_Department_Heads { get; set; }

        [InverseProperty(nameof(tbl_WPT_DepartmentDetail.tbl_WPT_Department))]
        public virtual ICollection<tbl_WPT_DepartmentDetail> tbl_WPT_DepartmentDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_DepartmentDetail_Section.tbl_WPT_Department))]
        public virtual ICollection<tbl_WPT_DepartmentDetail_Section> tbl_WPT_DepartmentDetail_Sections { get; set; }

        [InverseProperty(nameof(tbl_WPT_JobPositionMaster.tbl_WPT_Department))]
        public virtual ICollection<tbl_WPT_JobPositionMaster> tbl_WPT_JobPositionMasters { get; set; }

    }

    [Table("tbl_WPT_DepartmentDetail")]
    public class tbl_WPT_DepartmentDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Department))]

        public int FK_tbl_WPT_Department_ID { get; set; }
        public virtual tbl_WPT_Department tbl_WPT_Department { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Designation))]

        public int FK_tbl_WPT_Designation_ID { get; set; }
        public virtual tbl_WPT_Designation tbl_WPT_Designation { get; set; }

        [Required]
        [Display(Name = "No Of Employees")]
        public int NoOfEmployees { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_DepartmentDetail_Section")]
    public class tbl_WPT_DepartmentDetail_Section
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Department))]

        public int FK_tbl_WPT_Department_ID { get; set; }
        public virtual tbl_WPT_Department tbl_WPT_Department { get; set; }

        [Required]
        [Display(Name = "Section Name")]
        public string SectionName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_DepartmentDetail_Section_HOS.tbl_WPT_DepartmentDetail_Section))]
        public virtual ICollection<tbl_WPT_DepartmentDetail_Section_HOS> tbl_WPT_DepartmentDetail_Section_HOSs { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee.tbl_WPT_DepartmentDetail_Section))]
        public virtual ICollection<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }

        [InverseProperty(nameof(AspNetOreasAuthorizationScheme_Section.tbl_WPT_DepartmentDetail_Section))]
        public virtual ICollection<AspNetOreasAuthorizationScheme_Section> AspNetOreasAuthorizationScheme_Sections { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrdinaryRequisitionMaster.tbl_WPT_DepartmentDetail_Section))]
        public virtual ICollection<tbl_Inv_OrdinaryRequisitionMaster> tbl_Inv_OrdinaryRequisitionMasters { get; set; }

    }

    [Table("tbl_WPT_DepartmentDetail_Section_HOS")]
    public class tbl_WPT_DepartmentDetail_Section_HOS
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_DepartmentDetail_Section))]

        public int FK_tbl_WPT_DepartmentDetail_Section_ID { get; set; }
        public virtual tbl_WPT_DepartmentDetail_Section tbl_WPT_DepartmentDetail_Section { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_Designation")]
    public class tbl_WPT_Designation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee.tbl_WPT_Designation))]
        public virtual ICollection<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncentivePolicyDesignation.tbl_WPT_Designation))]
        public virtual ICollection<tbl_WPT_IncentivePolicyDesignation> tbl_WPT_IncentivePolicyDesignations { get; set; }

        [InverseProperty(nameof(tbl_WPT_PolicyPenaltyOnWTDetail_Designation.tbl_WPT_Designation))]
        public virtual ICollection<tbl_WPT_PolicyPenaltyOnWTDetail_Designation> tbl_WPT_PolicyPenaltyOnWTDetail_Designations { get; set; }

        [InverseProperty(nameof(tbl_WPT_DepartmentDetail.tbl_WPT_Designation))]
        public virtual ICollection<tbl_WPT_DepartmentDetail> tbl_WPT_DepartmentDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_JobPositionMaster.tbl_WPT_Designation))]
        public virtual ICollection<tbl_WPT_JobPositionMaster> tbl_WPT_JobPositionMasters { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeavePolicyNonPaid_Designation.tbl_WPT_Designation))]
        public virtual ICollection<tbl_WPT_LeavePolicyNonPaid_Designation> tbl_WPT_LeavePolicyNonPaid_Designations { get; set; }

    }

    //---------------------------------employeee---------------------------//

    [Table("tbl_WPT_EmploymentType")]
    public class tbl_WPT_EmploymentType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee.tbl_WPT_EmploymentType))]
        public virtual ICollection<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }

        [InverseProperty(nameof(tbl_WPT_HiringRequest.tbl_WPT_EmploymentType))]
        public virtual ICollection<tbl_WPT_HiringRequest> tbl_WPT_HiringRequests { get; set; }

    }

    [Table("tbl_WPT_Employee")]
    public class tbl_WPT_Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Employee No")]

        public int? EmployeeNo { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EmploymentType))]

        public int FK_tbl_WPT_EmploymentType_ID { get; set; }
        public virtual tbl_WPT_EmploymentType tbl_WPT_EmploymentType { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_DepartmentDetail_Section))]

        public int FK_tbl_WPT_DepartmentDetail_Section_ID { get; set; }
        public virtual tbl_WPT_DepartmentDetail_Section tbl_WPT_DepartmentDetail_Section { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Designation))]

        public int FK_tbl_WPT_Designation_ID { get; set; }
        public virtual tbl_WPT_Designation tbl_WPT_Designation { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EmployeeLevel))]

        public int FK_tbl_WPT_EmployeeLevel_ID { get; set; }
        public virtual tbl_WPT_EmployeeLevel tbl_WPT_EmployeeLevel { get; set; }

        [ForeignKey(nameof(tbl_WPT_Shift))]

        public int? FK_tbl_WPT_Shift_ID_Default { get; set; }
        public virtual tbl_WPT_Shift tbl_WPT_Shift { get; set; }

        [MaxLength(25)]
        [Display(Name = "AT EnrollmentNo Default")]
        public string ATEnrollmentNo_Default { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_ATType))]

        public int FK_tbl_WPT_ATType_ID { get; set; }
        public virtual tbl_WPT_ATType tbl_WPT_ATType { get; set; }

        [Required]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }

        [Display(Name = "InActive Date")]
        public DateTime? InactiveDate { get; set; }

        [ForeignKey(nameof(tbl_WPT_InActiveType))]

        public int? FK_tbl_WPT_InActiveType_ID { get; set; }
        public virtual tbl_WPT_InActiveType tbl_WPT_InActiveType { get; set; }

        [Required]
        [Display(Name = "Pension Activated")]
        public bool IsPensionActive { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        [Display(Name = "Employee Name")]
        [Required]
        public string EmployeeName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Father OR Husband Name")]
        public string FatherORHusbandName { get; set; }

        [Required]
        [MaxLength(8)]
        public string Gender { get; set; }

        [MaxLength(20)]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [MaxLength(15)]
        public string CNIC { get; set; }

        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(14)]
        [Display(Name = "Cell Phone No")]
        public string CellPhoneNo { get; set; }

        [MaxLength(50)]
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }

        [MaxLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MaxLength(3)]
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [MaxLength(14)]
        [Display(Name = "Emergency No")]
        public string EmergencyNo { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EducationalLevelType))]

        public int FK_tbl_WPT_EducationalLevelType_ID { get; set; }
        public virtual tbl_WPT_EducationalLevelType tbl_WPT_EducationalLevelType { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_CalendarYear_LeaveEmps.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_CalendarYear_LeaveEmps> tbl_WPT_CalendarYear_LeaveEmpss { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Emp.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emps { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunExemption_Emp.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_PayRunExemption_Emp> tbl_WPT_PayRunExemption_Emps { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeBankDetail.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_EmployeeBankDetail> tbl_WPT_EmployeeBankDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructure.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructure> tbl_WPT_EmployeeSalaryStructures { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeePensionStructure.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_EmployeePensionStructure> tbl_WPT_EmployeePensionStructures { get; set; }

        [InverseProperty(nameof(tbl_WPT_LoanDetail.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_LoanDetail> tbl_WPT_LoanDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_ATTimeGraceEmployeeLink.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_ATTimeGraceEmployeeLink> tbl_WPT_ATTimeGraceEmployeeLinks { get; set; }

        [InverseProperty(nameof(tbl_WPT_AttendanceLog.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_AttendanceLog> tbl_WPT_AttendanceLogs { get; set; }

        [InverseProperty(nameof(tbl_WPT_ATBulkManualDetail_Employee.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_ATBulkManualDetail_Employee> tbl_WPT_ATBulkManualDetail_Employees { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncrementDetail.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_IncrementDetail> tbl_WPT_IncrementDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee_PFF.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_Employee_PFF> tbl_WPT_Employee_PFFs { get; set; }

        [InverseProperty(nameof(tbl_WPT_ShiftRosterDetail_Employee.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_ShiftRosterDetail_Employee> tbl_WPT_ShiftRosterDetail_Employees { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardDetail.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_RewardDetail> tbl_WPT_RewardDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncentivePolicyEmployees.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_IncentivePolicyEmployees> tbl_WPT_IncentivePolicyEmployeess { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_Employee_HOD))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions_HOD { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_Employee_HR))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions_HR { get; set; }

        [InverseProperty(nameof(tbl_WPT_AppraisalMaster.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_AppraisalMaster> tbl_WPT_AppraisalMasters { get; set; }

        [InverseProperty(nameof(tbl_WPT_HiringRequest.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_HiringRequest> tbl_WPT_HiringRequest_Replacements { get; set; }

        [InverseProperty(nameof(tbl_WPT_DepartmentDetail_Section_HOS.tbl_WPT_Employee))]
        public virtual ICollection<tbl_WPT_DepartmentDetail_Section_HOS> tbl_WPT_DepartmentDetail_Section_HOSs { get; set; }

        [InverseProperty(nameof(ApplicationUser.tbl_WPT_Employee))]
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }

    [Table("tbl_WPT_Employee_PFF")]
    public class tbl_WPT_Employee_PFF
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        public string Photo160X210 { get; set; }
        public string FaceTemplate { get; set; }
        public string FingerTemplate0 { get; set; }
        public string FingerTemplate1 { get; set; }
        public string FingerTemplate2 { get; set; }
        public string FingerTemplate3 { get; set; }
        public string FingerTemplate4 { get; set; }
        public string FingerTemplate5 { get; set; }
        public string FingerTemplate6 { get; set; }
        public string FingerTemplate7 { get; set; }
        public string FingerTemplate8 { get; set; }
        public string FingerTemplate9 { get; set; }
        public string CardNumber { get; set; }

        [MaxLength(10)]
        public string Password { get; set; }

        [Required]
        public int Privilege { get; set; }

        [Required]
        public bool Enabled { get; set; }

    }

    [Table("tbl_WPT_EmployeeBankDetail")]
    public class tbl_WPT_EmployeeBankDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Bank_Branch))]

        public int FK_tbl_WPT_Bank_Branch_ID { get; set; }
        public virtual tbl_WPT_Bank_Branch tbl_WPT_Bank_Branch { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Bank A/C No")]
        public string BankAccountNo { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Bank A/C Title")]
        public string BankAccountTitle { get; set; }

        [Required]
        [Display(Name = "Bank Default A/c")]
        public bool IsDefaultForBank { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Emp.tbl_WPT_EmployeeBankDetail))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emps { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardDetail.tbl_WPT_EmployeeBankDetail))]
        public virtual ICollection<tbl_WPT_RewardDetail> tbl_WPT_RewardDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_LoanDetail.tbl_WPT_EmployeeBankDetail))]
        public virtual ICollection<tbl_WPT_LoanDetail> tbl_WPT_LoanDetails { get; set; }
    }

    [Table("tbl_WPT_EmployeeLevel")]
    public class tbl_WPT_EmployeeLevel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Level Name")]
        public string LevelName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee.tbl_WPT_EmployeeLevel))]
        public virtual ICollection<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }

    }


    //-------------------------------salary structure--------------------//

    [Table("tbl_WPT_tbl_OTPolicy")]
    public class tbl_WPT_tbl_OTPolicy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Policy Name")]
        public string PolicyName { get; set; }

        [Required]
        [Display(Name = "Fixed Rate")]
        public double FixedRate { get; set; }

        [Required]
        [Display(Name = "Maximum Limit Rate")]
        public double MaximumLimitRate { get; set; }

        [Required]
        [Display(Name = "Multiply Factor")]
        public double MultiplyFactor { get; set; }

        [ForeignKey(nameof(tbl_WPT_CalculationMethod))]

        public int? FK_tbl_WPT_CalculationMethod_ID { get; set; }
        public virtual tbl_WPT_CalculationMethod tbl_WPT_CalculationMethod { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructure.tbl_WPT_tbl_OTPolicy))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructure> tbl_WPT_EmployeeSalaryStructures { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_tbl_OTPolicy))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    [Table("tbl_WPT_EmployeeSalaryStructure")]
    public class tbl_WPT_EmployeeSalaryStructure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Display(Name = "Basic Wage")]
        public double BasicWage { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_tbl_OTPolicy))]

        public int FK_tbl_WPT_tbl_OTPolicy_ID { get; set; }
        public virtual tbl_WPT_tbl_OTPolicy tbl_WPT_tbl_OTPolicy { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_TransactionMode))]

        public int FK_tbl_WPT_TransactionMode_ID { get; set; }
        public virtual tbl_WPT_TransactionMode tbl_WPT_TransactionMode { get; set; }

        [Required]
        [Display(Name = "Max Transaction Limit")]
        public double MaxTransactionLimit { get; set; }

        [ForeignKey(nameof(tbl_WPT_TransactionMode_Secondary))]

        public int? FK_tbl_WPT_TransactionMode_ID_Secondary { get; set; }
        public virtual tbl_WPT_TransactionMode tbl_WPT_TransactionMode_Secondary { get; set; }

        [MaxLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [ForeignKey(nameof(tbl_WPT_IncrementDetail_SalaryStructure))]

        public int? FK_tbl_WPT_IncrementDetail_ID { get; set; }
        public virtual tbl_WPT_IncrementDetail tbl_WPT_IncrementDetail_SalaryStructure { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructureAllowance.tbl_WPT_EmployeeSalaryStructure))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructureAllowance> tbl_WPT_EmployeeSalaryStructureAllowances { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructureDeductible.tbl_WPT_EmployeeSalaryStructure))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructureDeductible> tbl_WPT_EmployeeSalaryStructureDeductibles { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_EmployeeSalaryStructure))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wage_Basics { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Emp.tbl_WPT_EmployeeSalaryStructure))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emps { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncrementDetail.tbl_WPT_EmployeeSalaryStructure_Last))]
        public virtual ICollection<tbl_WPT_IncrementDetail> tbl_WPT_IncrementDetail_Last { get; set; }

    }

    [Table("tbl_WPT_EmployeeSalaryStructureAllowance")]
    public class tbl_WPT_EmployeeSalaryStructureAllowance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EmployeeSalaryStructure))]

        public int FK_tbl_WPT_EmployeeSalaryStructure_ID { get; set; }
        public virtual tbl_WPT_EmployeeSalaryStructure tbl_WPT_EmployeeSalaryStructure { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_AllowanceType))]

        public int FK_tbl_WPT_AllowanceType_ID { get; set; }
        public virtual tbl_WPT_AllowanceType tbl_WPT_AllowanceType { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_WageCalculationType))]

        public int FK_tbl_WPT_WageCalculationType_ID { get; set; }
        public virtual tbl_WPT_WageCalculationType tbl_WPT_WageCalculationType { get; set; }

        [Required]
        [Display(Name = "Min WD %")]
        public double Min_WD_Per { get; set; }

        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_EmployeeSalaryStructureAllowance))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    [Table("tbl_WPT_EmployeeSalaryStructureDeductible")]
    public class tbl_WPT_EmployeeSalaryStructureDeductible
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EmployeeSalaryStructure))]

        public int FK_tbl_WPT_EmployeeSalaryStructure_ID { get; set; }
        public virtual tbl_WPT_EmployeeSalaryStructure tbl_WPT_EmployeeSalaryStructure { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_DeductibleType))]

        public int FK_tbl_WPT_DeductibleType_ID { get; set; }
        public virtual tbl_WPT_DeductibleType tbl_WPT_DeductibleType { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_WageCalculationType))]

        public int FK_tbl_WPT_WageCalculationType_ID { get; set; }
        public virtual tbl_WPT_WageCalculationType tbl_WPT_WageCalculationType { get; set; }

        [Required]
        [Display(Name = "Min WD %")]
        public double Min_WD_Per { get; set; }

        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_EmployeeSalaryStructureDeductible))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    [Table("tbl_WPT_EmployeePensionStructure")]
    public class tbl_WPT_EmployeePensionStructure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Display(Name = "Pension Wage")]
        public double PensionWage { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_EmployeePensionStructure))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    [Table("tbl_WPT_AllowanceType")]
    public class tbl_WPT_AllowanceType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Allowance Name")]
        public string AllowanceName { get; set; }

        [Required]
        [MaxLength(2)]
        public string Prefix { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructureAllowance.tbl_WPT_AllowanceType))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructureAllowance> tbl_WPT_EmployeeSalaryStructureAllowances { get; set; }

    }

    [Table("tbl_WPT_DeductibleType")]
    public class tbl_WPT_DeductibleType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Deductible Name")]
        public string DeductibleName { get; set; }

        [Required]
        [MaxLength(2)]
        public string Prefix { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructureDeductible.tbl_WPT_DeductibleType))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructureDeductible> tbl_WPT_EmployeeSalaryStructureDeductibles { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunExemption.tbl_WPT_DeductibleType))]
        public virtual ICollection<tbl_WPT_PayRunExemption> tbl_WPT_PayRunExemptions { get; set; }

    }

    //------------------------------- Increment in Salary ---------------------------//

    [Table("tbl_WPT_IncrementMaster")]
    public class tbl_WPT_IncrementMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Doc No")]

        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear))]

        public int FK_tbl_WPT_CalendarYear_ID { get; set; }
        public virtual tbl_WPT_CalendarYear tbl_WPT_CalendarYear { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncrementDetail.tbl_WPT_IncrementMaster))]
        public virtual ICollection<tbl_WPT_IncrementDetail> tbl_WPT_IncrementDetails { get; set; }

    }

    [Table("tbl_WPT_IncrementDetail")]
    public class tbl_WPT_IncrementDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_IncrementMaster))]

        public int FK_tbl_WPT_IncrementMaster_ID { get; set; }
        public virtual tbl_WPT_IncrementMaster tbl_WPT_IncrementMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Display(Name = "Increment Value")]
        public double IncrementValue { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_IncrementBy))]

        public int FK_tbl_WPT_IncrementBy_ID { get; set; }
        public virtual tbl_WPT_IncrementBy tbl_WPT_IncrementBy { get; set; }

        [Required]
        [Display(Name = "Increment Arrear")]
        public double Arrear { get; set; }

        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months))]

        public int? FK_tbl_WPT_CalendarYear_Months_ID_ApplyArrear { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeeSalaryStructure_Last))]

        public int? FK_tbl_WPT_EmployeeSalaryStructure_ID_Last { get; set; }
        public virtual tbl_WPT_EmployeeSalaryStructure tbl_WPT_EmployeeSalaryStructure_Last { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructure.tbl_WPT_IncrementDetail_SalaryStructure))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructure> tbl_WPT_EmployeeSalaryStructures { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_IncrementDetail))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    [Table("tbl_WPT_IncrementBy")]
    public class tbl_WPT_IncrementBy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [Display(Name = "Increment By")]
        public string IncrementBy { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncrementDetail.tbl_WPT_IncrementBy))]
        public virtual ICollection<tbl_WPT_IncrementDetail> tbl_WPT_IncrementDetails { get; set; }

    }

    //---------------------------------- Incentives ----------------------------//


    [Table("tbl_WPT_IncentiveType")]
    public class tbl_WPT_IncentiveType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [Display(Name = "Incentive Type")]
        public string IncentiveType { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncentivePolicy.tbl_WPT_IncentiveType))]
        public virtual ICollection<tbl_WPT_IncentivePolicy> tbl_WPT_IncentivePolicys { get; set; }

    }

    [Table("tbl_WPT_IncentivePolicy")]
    public class tbl_WPT_IncentivePolicy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Doc No")]

        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "DocDate")]
        public DateTime DocDate { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Incentive Name")]
        public string IncentiveName { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_WPT_IncentiveType))]

        public int FK_tbl_WPT_IncentiveType_ID { get; set; }
        public virtual tbl_WPT_IncentiveType tbl_WPT_IncentiveType { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [ForeignKey(nameof(tbl_WPT_CalculationMethod))]

        public int? FK_tbl_WPT_CalculationMethod_ID { get; set; }
        public virtual tbl_WPT_CalculationMethod tbl_WPT_CalculationMethod { get; set; }

        [Required]
        [Display(Name = "Factor")]
        public double Factor { get; set; }

        [Required]
        [Display(Name = "OT Minutes From Per Day")]
        public int OT_MinutesFrom_PerDay { get; set; }

        [Required]
        [Display(Name = "OT Minutes Till Per Day")]
        public int OT_MinutesTill_PerDay { get; set; }

        [Required]
        [Display(Name = "OT After Shift Minutes From Per Day")]
        public int OT_AfterShiftMinutesFrom_PerDay { get; set; }

        [Required]
        [Display(Name = "OT After Shift Minutes Till Per Day")]
        public int OT_AfterShiftMinutesTill_PerDay { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncentivePolicyDesignation.tbl_WPT_IncentivePolicy))]
        public virtual ICollection<tbl_WPT_IncentivePolicyDesignation> tbl_WPT_IncentivePolicyDesignations { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncentivePolicyEmployees.tbl_WPT_IncentivePolicy))]
        public virtual ICollection<tbl_WPT_IncentivePolicyEmployees> tbl_WPT_IncentivePolicyEmployeess { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_IncentivePolicy))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    [Table("tbl_WPT_IncentivePolicyDesignation")]
    public class tbl_WPT_IncentivePolicyDesignation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_IncentivePolicy))]

        public int FK_tbl_WPT_IncentivePolicy_ID { get; set; }
        public virtual tbl_WPT_IncentivePolicy tbl_WPT_IncentivePolicy { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Designation))]

        public int FK_tbl_WPT_Designation_ID { get; set; }
        public virtual tbl_WPT_Designation tbl_WPT_Designation { get; set; }

        [MaxLength(10)]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_IncentivePolicyEmployees")]
    public class tbl_WPT_IncentivePolicyEmployees
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_IncentivePolicy))]

        public int FK_tbl_WPT_IncentivePolicy_ID { get; set; }
        public virtual tbl_WPT_IncentivePolicy tbl_WPT_IncentivePolicy { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        public bool Apply { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    //-------------------------- Loan / Advance ---------------------//
    [Table("tbl_WPT_LoanType")]
    public class tbl_WPT_LoanType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }

        [Required]
        [MaxLength(2)]
        public string Prefix { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_LoanMaster.tbl_WPT_LoanType))]
        public virtual ICollection<tbl_WPT_LoanMaster> tbl_WPT_LoanMasters { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunExemption.tbl_WPT_LoanType))]
        public virtual ICollection<tbl_WPT_PayRunExemption> tbl_WPT_PayRunExemptions { get; set; }

    }

    [Table("tbl_WPT_LoanMaster")]
    public class tbl_WPT_LoanMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Doc No")]

        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "DocDate")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_LoanType))]

        public int FK_tbl_WPT_LoanType_ID { get; set; }
        public virtual tbl_WPT_LoanType tbl_WPT_LoanType { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_LoanDetail.tbl_WPT_LoanMaster))]
        public virtual ICollection<tbl_WPT_LoanDetail> tbl_WPT_LoanDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_LoanDetail_Payment.tbl_WPT_LoanMaster))]
        public virtual ICollection<tbl_WPT_LoanDetail_Payment> tbl_WPT_LoanDetail_Payments { get; set; }

    }

    [Table("tbl_WPT_LoanDetail_Payment")]
    public class tbl_WPT_LoanDetail_Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_LoanMaster))]

        public int FK_tbl_WPT_LoanMaster_ID { get; set; }
        public virtual tbl_WPT_LoanMaster tbl_WPT_LoanMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CompanyBankDetail))]

        public int FK_tbl_WPT_CompanyBankDetail_ID { get; set; }
        public virtual tbl_WPT_CompanyBankDetail tbl_WPT_CompanyBankDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_TransactionMode))]

        public int FK_tbl_WPT_TransactionMode_ID { get; set; }
        public virtual tbl_WPT_TransactionMode tbl_WPT_TransactionMode { get; set; }

        [MaxLength(50)]
        [Display(Name = "Instrument No")]
        public string InstrumentNo { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_LoanDetail.tbl_WPT_LoanDetail_Payment))]
        public virtual ICollection<tbl_WPT_LoanDetail> tbl_WPT_LoanDetails { get; set; }

    }

    [Table("tbl_WPT_LoanDetail")]
    public class tbl_WPT_LoanDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_LoanMaster))]

        public int FK_tbl_WPT_LoanMaster_ID { get; set; }
        public virtual tbl_WPT_LoanMaster tbl_WPT_LoanMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Required]
        [Display(Name = "Installment Rate")]
        public double InstallmentRate { get; set; }

        [Required]
        [Display(Name = "Effective From")]
        public DateTime EffectiveFrom { get; set; }

        [ForeignKey(nameof(tbl_WPT_LoanDetail_Payment))]

        public int? FK_tbl_WPT_LoanDetail_Payment_ID { get; set; }
        public virtual tbl_WPT_LoanDetail_Payment tbl_WPT_LoanDetail_Payment { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeeBankDetail))]

        public int? FK_tbl_WPT_EmployeeBankDetail_ID { get; set; }
        public virtual tbl_WPT_EmployeeBankDetail tbl_WPT_EmployeeBankDetail { get; set; }

        [Required]
        [Display(Name = "Receiving Date")]
        public DateTime ReceivingDate { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_LoanDetail))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    //--------------------------------leave------------------------------//

    [Table("tbl_WPT_LeaveCFOptions")]
    public class tbl_WPT_LeaveCFOptions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [Display(Name = "Carry Forward Option")]
        [MaxLength(50)]
        public string CFOption { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeavePolicy.tbl_WPT_LeaveCFOptions))]
        public virtual ICollection<tbl_WPT_LeavePolicy> tbl_WPT_LeavePolicys { get; set; }

    }

    [Table("tbl_WPT_LeavePolicy")]
    public class tbl_WPT_LeavePolicy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Policy Name")]
        public string PolicyName { get; set; }

        [Required]
        [MaxLength(4)]
        [Display(Name = "Policy Prefix")]
        public string PolicyPrefix { get; set; }

        [Required]
        public double Leave { get; set; }

        [Required]
        [Display(Name = "With Out Request")]
        public bool WithOutRequest { get; set; }

        [Required]
        [Display(Name = "Max Leaves Can Avail in Month")]
        public double MonthlyRestrict_MaxNoOfLeavesCanAvail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_LeaveCFOptions))]

        public int FK_tbl_WPT_LeaveCFOptions_ID { get; set; }
        public virtual tbl_WPT_LeaveCFOptions tbl_WPT_LeaveCFOptions { get; set; }

        [Required]
        [Display(Name = "HOS Approval Required")]
        public bool IsHOSApprovalReq { get; set; }

        [Required]
        [Display(Name = "HR Approval Required")]
        public bool IsHRApprovalReq { get; set; }

        [Required]
        [Display(Name = "Final Granter")]
        [MaxLength(25)]
        public string FinalGranter { get; set; }

        [Required]
        [Display(Name = "Encashable Leave")]
        public double EncashableLeave { get; set; }

        [Required]
        [Display(Name = "Encashable Period")]
        [MaxLength(50)]
        public string EncashablePeriod { get; set; }

        [Required]
        [Display(Name = "EL Min Balance")]
        public double EL_MinBalance { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalculationMethods))]

        public int FK_tbl_WPT_CalculationMethod_ID_EL { get; set; }
        public virtual tbl_WPT_CalculationMethod tbl_WPT_CalculationMethods { get; set; }

        //on the month of encash check the min wd % to apply encashable leave
        [Required]
        [Display(Name = "Min WD% For EL Month")]
        [Range(0, 100)]
        public double Min_WD_Per_ForELMonth { get; set; }

        [Required]
        [Display(Name = "Carry Foward Leave")]
        public double CarryFowardLeave { get; set; }

        [Required]
        [Display(Name = "CFL Min Balance")]
        public double CFL_MinBalance { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_LeavePolicy))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions { get; set; }

        [InverseProperty(nameof(tbl_WPT_CalendarYear_LeaveEmps_Leaves.tbl_WPT_LeavePolicy))]
        public virtual ICollection<tbl_WPT_CalendarYear_LeaveEmps_Leaves> tbl_WPT_CalendarYear_LeaveEmps_Leavess { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_LeavePolicy))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    [Table("tbl_WPT_LeavePolicyNonPaid")]
    public class tbl_WPT_LeavePolicyNonPaid
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Policy Name")]
        public string PolicyName { get; set; }

        [Required]
        [MaxLength(4)]
        [Display(Name = "Policy Prefix")]
        public string PolicyPrefix { get; set; }

        [Required]
        [Display(Name = "HOS Approval Required")]
        public bool IsHOSApprovalReq { get; set; }

        [Required]
        [Display(Name = "HR Approval Required")]
        public bool IsHRApprovalReq { get; set; }

        [Required]
        [Display(Name = "Final Granter")]
        [MaxLength(25)]
        public string FinalGranter { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeavePolicyNonPaid_Designation.tbl_WPT_LeavePolicyNonPaid))]
        public virtual ICollection<tbl_WPT_LeavePolicyNonPaid_Designation> tbl_WPT_LeavePolicyNonPaid_Designations { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_LeavePolicyNonPaid))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions { get; set; }

    }

    [Table("tbl_WPT_LeavePolicyNonPaid_Designation")]
    public class tbl_WPT_LeavePolicyNonPaid_Designation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_LeavePolicyNonPaid))]

        public int FK_tbl_WPT_LeavePolicyNonPaid_ID { get; set; }
        public virtual tbl_WPT_LeavePolicyNonPaid tbl_WPT_LeavePolicyNonPaid { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Designation))]

        public int FK_tbl_WPT_Designation_ID { get; set; }
        public virtual tbl_WPT_Designation tbl_WPT_Designation { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_LeaveRequisition")]
    public class tbl_WPT_LeaveRequisition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Doc No")]

        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "DocDate")]
        public DateTime DocDate { get; set; }

        [Required]
        [Display(Name = "Leave Value")]
        public double LeaveValue { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months))]

        public int FK_tbl_WPT_CalendarYear_Months_ID { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [ForeignKey(nameof(tbl_WPT_LeavePolicy))]

        public int? FK_tbl_WPT_LeavePolicy_ID { get; set; }
        public virtual tbl_WPT_LeavePolicy tbl_WPT_LeavePolicy { get; set; }

        [ForeignKey(nameof(tbl_WPT_LeavePolicyNonPaid))]

        public int? FK_tbl_WPT_LeavePolicyNonPaid_ID { get; set; }
        public virtual tbl_WPT_LeavePolicyNonPaid tbl_WPT_LeavePolicyNonPaid { get; set; }

        [Required]
        [Display(Name = "Leave From")]
        public DateTime LeaveFrom { get; set; }

        [Required]
        [Display(Name = "Leave Till")]
        public DateTime LeaveTill { get; set; }

        [MaxLength(50)]
        public string Reason { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_ActionList_HOD))]

        public int FK_tbl_WPT_ActionList_ID_HOS { get; set; }
        public virtual tbl_WPT_ActionList tbl_WPT_ActionList_HOD { get; set; }

        [ForeignKey(nameof(tbl_WPT_Employee_HOD))]

        public int? FK_tbl_WPT_Employee_ID_HOS { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee_HOD { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_ActionList_HR))]

        public int FK_tbl_WPT_ActionList_ID_HR { get; set; }
        public virtual tbl_WPT_ActionList tbl_WPT_ActionList_HR { get; set; }

        [ForeignKey(nameof(tbl_WPT_Employee_HR))]

        public int? FK_tbl_WPT_Employee_ID_HR { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee_HR { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_ActionList_Final))]

        public int FK_tbl_WPT_ActionList_ID_Final { get; set; }
        public virtual tbl_WPT_ActionList tbl_WPT_ActionList_Final { get; set; }

        [ForeignKey(nameof(tbl_WPT_PayRunDetail_Emp))]

        public int? FK_tbl_WPT_PayRunDetail_Emp_AutoGenerated { get; set; }
        public virtual tbl_WPT_PayRunDetail_Emp tbl_WPT_PayRunDetail_Emp { get; set; }

        [Required]
        [Display(Name = "Requester Form Code")]
        public int RequesterFormCode { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_AT.tbl_WPT_LeaveRequisition))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_AT> tbl_WPT_PayRunDetail_EmpDetail_ATs { get; set; }

    }

    //--------------------------------calendar--------------//

    [Table("tbl_WPT_CalendarYear")]
    public class tbl_WPT_CalendarYear
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [Display(Name = "Calendar Year")]
        public int CalendarYear { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_CalendarYear_Months.tbl_WPT_CalendarYear))]
        public virtual ICollection<tbl_WPT_CalendarYear_Months> tbl_WPT_CalendarYear_Monthss { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncrementMaster.tbl_WPT_CalendarYear))]
        public virtual ICollection<tbl_WPT_IncrementMaster> tbl_WPT_IncrementMasters { get; set; }

        [InverseProperty(nameof(tbl_WPT_CalendarYear_LeaveEmps.tbl_WPT_CalendarYear))]
        public virtual ICollection<tbl_WPT_CalendarYear_LeaveEmps> tbl_WPT_CalendarYear_LeaveEmpss { get; set; }     

        [InverseProperty(nameof(tbl_WPT_AppraisalMaster.tbl_WPT_CalendarYear))]
        public virtual ICollection<tbl_WPT_AppraisalMaster> tbl_WPT_AppraisalMasters { get; set; }

    }

    [Table("tbl_WPT_CalendarYear_Months")]
    public class tbl_WPT_CalendarYear_Months
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear))]

        public int FK_tbl_WPT_CalendarYear_ID { get; set; }
        public virtual tbl_WPT_CalendarYear tbl_WPT_CalendarYear { get; set; }

        [Required]
        [Display(Name = "Month Start")]
        public DateTime MonthStart { get; set; }

        [Required]
        [Display(Name = "Month End")]
        public DateTime MonthEnd { get; set; }

        [Required]
        [Display(Name = "Closed")]
        public bool IsClosed { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_CalendarYear_Months_Holidays.tbl_WPT_CalendarYear_Months))]
        public virtual ICollection<tbl_WPT_CalendarYear_Months_Holidays> tbl_WPT_CalendarYear_Months_Holidayss { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunToDo.tbl_WPT_CalendarYear_Months))]
        public virtual ICollection<tbl_WPT_PayRunToDo> tbl_WPT_PayRunToDos { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunExemption.tbl_WPT_CalendarYear_Months))]
        public virtual ICollection<tbl_WPT_PayRunExemption> tbl_WPT_PayRunExemptions { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunMaster.tbl_WPT_CalendarYear_Months))]
        public virtual ICollection<tbl_WPT_PayRunMaster> tbl_WPT_PayRunMasters { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_CalendarYear_Months))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions { get; set; }

        [InverseProperty(nameof(tbl_WPT_IncrementDetail.tbl_WPT_CalendarYear_Months))]
        public virtual ICollection<tbl_WPT_IncrementDetail> tbl_WPT_IncrementDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardMaster.tbl_WPT_CalendarYear_Months))]
        public virtual ICollection<tbl_WPT_RewardMaster> tbl_WPT_RewardMasters { get; set; }

        [InverseProperty(nameof(tbl_WPT_ShiftRosterMaster.tbl_WPT_CalendarYear_Months))]
        public virtual ICollection<tbl_WPT_ShiftRosterMaster> tbl_WPT_ShiftRosterMasters { get; set; }       

        [ForeignKey("FK_tbl_WPT_CalendarYear_Months_ID_Apply")]
        [InverseProperty(nameof(tbl_WPT_CalendarYear_LeaveEmps_Leaves.tbl_WPT_CalendarYear_Months_Apply))]
        public virtual ICollection<tbl_WPT_CalendarYear_LeaveEmps_Leaves> tbl_WPT_CalendarYear_LeaveEmps_Leaves_Applys { get; set; }

        [ForeignKey("FK_tbl_WPT_CalendarYear_Months_ID_Expire")]
        [InverseProperty(nameof(tbl_WPT_CalendarYear_LeaveEmps_Leaves.tbl_WPT_CalendarYear_Months_Expire))]
        public virtual ICollection<tbl_WPT_CalendarYear_LeaveEmps_Leaves> tbl_WPT_CalendarYear_LeaveEmps_Leaves_Expires { get; set; }

    }

    [Table("tbl_WPT_CalendarYear_LeaveEmps")]
    public class tbl_WPT_CalendarYear_LeaveEmps
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear))]

        public int FK_tbl_WPT_CalendarYear_ID { get; set; }
        public virtual tbl_WPT_CalendarYear tbl_WPT_CalendarYear { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [Display(Name = "Is Closed")]
        public bool IsClosed { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_CalendarYear_LeaveEmps_Leaves.tbl_WPT_CalendarYear_LeaveEmps))]
        public virtual ICollection<tbl_WPT_CalendarYear_LeaveEmps_Leaves> tbl_WPT_CalendarYear_LeaveEmps_Leavess { get; set; }

    }

    [Table("tbl_WPT_CalendarYear_LeaveEmps_Leaves")]
    public class tbl_WPT_CalendarYear_LeaveEmps_Leaves
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_LeaveEmps))]

        public int FK_tbl_WPT_CalendarYear_LeaveEmps_ID { get; set; }
        public virtual tbl_WPT_CalendarYear_LeaveEmps tbl_WPT_CalendarYear_LeaveEmps { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_LeavePolicy))]

        public int FK_tbl_WPT_LeavePolicy_ID { get; set; }
        public virtual tbl_WPT_LeavePolicy tbl_WPT_LeavePolicy { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months_Apply))]

        public int FK_tbl_WPT_CalendarYear_Months_ID_Apply { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months_Apply { get; set; }

        [Required]
        public double Opening { get; set; }

        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months_Expire))]

        public int? FK_tbl_WPT_CalendarYear_Months_ID_Expire { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months_Expire { get; set; }

        [Required]
        [Display(Name = "Allowed From Next Year")]
        public double AllowedFromNextYear { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_CalendarYear_Months_Holidays")]
    public class tbl_WPT_CalendarYear_Months_Holidays
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months))]

        public int FK_tbl_WPT_CalendarYear_Months_ID { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Holiday))]

        public int FK_tbl_WPT_Holiday_ID { get; set; }
        public virtual tbl_WPT_Holiday tbl_WPT_Holiday { get; set; }

        [Required]
        [Display(Name = "Holiday Date")]
        public DateTime HolidayDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    //---------------------------------payRun-----------------------------------//
    [Table("tbl_WPT_PayRunToDo")]
    public class tbl_WPT_PayRunToDo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months))]

        public int FK_tbl_WPT_CalendarYear_Months_ID { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months { get; set; }

        [Required]
        [Display(Name = "To Do")]
        [MaxLength(100)]
        public string ToDo { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_PayRunExemption")]
    public class tbl_WPT_PayRunExemption
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months))]

        public int FK_tbl_WPT_CalendarYear_Months_ID { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months { get; set; }

        [Required]
        [Display(Name = "Apply To All")]
        public bool ApplyToAll { get; set; }

        [Required]
        [Display(Name = "Exemption %")]
        [Range(0, 100)]
        public double ExemptionPercentage { get; set; }

        [ForeignKey(nameof(tbl_WPT_DeductibleType))]

        public int? FK_tbl_WPT_DeductibleType_ID { get; set; }
        public virtual tbl_WPT_DeductibleType tbl_WPT_DeductibleType { get; set; }

        [ForeignKey(nameof(tbl_WPT_LoanType))]

        public int? FK_tbl_WPT_LoanType_ID { get; set; }
        public virtual tbl_WPT_LoanType tbl_WPT_LoanType { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunExemption_Emp.tbl_WPT_PayRunExemption))]
        public virtual ICollection<tbl_WPT_PayRunExemption_Emp> tbl_WPT_PayRunExemption_Emps { get; set; }

    }

    [Table("tbl_WPT_PayRunExemption_Emp")]
    public class tbl_WPT_PayRunExemption_Emp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_PayRunExemption))]

        public int FK_tbl_WPT_PayRunExemption_ID { get; set; }
        public virtual tbl_WPT_PayRunExemption tbl_WPT_PayRunExemption { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_PayRunMaster")]
    public class tbl_WPT_PayRunMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months))]

        public int FK_tbl_WPT_CalendarYear_Months_ID { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Emp.tbl_WPT_PayRunMaster))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emps { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Payment.tbl_WPT_PayRunMaster))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Payment> tbl_WPT_PayRunDetail_Payments { get; set; }

    }

    [Table("tbl_WPT_PayRunDetail_Emp")]
    public class tbl_WPT_PayRunDetail_Emp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_PayRunMaster))]

        public int FK_tbl_WPT_PayRunMaster_ID { get; set; }
        public virtual tbl_WPT_PayRunMaster tbl_WPT_PayRunMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [Display(Name = "Working Days Attendance")]
        public double WD_AT { get; set; }

        [Required]
        [Display(Name = "Working Days Manual")]
        public double WD_Manual { get; set; }

        [Required]
        [Display(Name = "Final Working Days")]
        public double WD { get; set; }

        [Required]
        [Display(Name = "OT Attendance")]
        public double OT_AT { get; set; }

        [Required]
        [Display(Name = "OT Manual")]
        public double OT_Manual { get; set; }

        [Required]
        [Display(Name = "Final OT")]
        public double OT { get; set; }

        [Required]
        [Display(Name = "Wage")]
        public double Wage { get; set; }

        [Required]
        [Display(Name = "Wage Bank")]
        public double WagePrimary { get; set; }

        [ForeignKey(nameof(tbl_WPT_TransactionMode_Primary))]

        public int? FK_tbl_WPT_TransactionMode_ID_Primary { get; set; }
        public virtual tbl_WPT_TransactionMode tbl_WPT_TransactionMode_Primary { get; set; }

        [Required]
        [Display(Name = "Wage Cash")]
        public double WageSecondary { get; set; }

        [ForeignKey(nameof(tbl_WPT_TransactionMode_Secondary))]

        public int? FK_tbl_WPT_TransactionMode_ID_Secondary { get; set; }
        public virtual tbl_WPT_TransactionMode tbl_WPT_TransactionMode_Secondary { get; set; }

        [ForeignKey(nameof(tbl_WPT_PayRunDetail_Payment_Primary))]

        public int? FK_tbl_WPT_PayRunDetail_Payment_ID_Primary { get; set; }
        public virtual tbl_WPT_PayRunDetail_Payment tbl_WPT_PayRunDetail_Payment_Primary { get; set; }

        [ForeignKey(nameof(tbl_WPT_PayRunDetail_Payment_Secondary))]

        public int? FK_tbl_WPT_PayRunDetail_Payment_ID_Secondary { get; set; }
        public virtual tbl_WPT_PayRunDetail_Payment tbl_WPT_PayRunDetail_Payment_Secondary { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeeSalaryStructure))]

        public int? FK_tbl_WPT_EmployeeSalaryStructure_ID { get; set; }
        public virtual tbl_WPT_EmployeeSalaryStructure tbl_WPT_EmployeeSalaryStructure { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeeBankDetail))]

        public int? FK_tbl_WPT_EmployeeBankDetail_ID { get; set; }
        public virtual tbl_WPT_EmployeeBankDetail tbl_WPT_EmployeeBankDetail { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_AT.tbl_WPT_PayRunDetail_Emp))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_AT> tbl_WPT_PayRunDetail_EmpDetail_ATs { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_PayRunDetail_Emp))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

        [InverseProperty(nameof(tbl_WPT_LeaveRequisition.tbl_WPT_PayRunDetail_Emp))]
        public virtual ICollection<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions { get; set; }
    }

    [Table("tbl_WPT_PayRunDetail_EmpDetail_AT")]
    public class tbl_WPT_PayRunDetail_EmpDetail_AT
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_PayRunDetail_Emp))]

        public int FK_tbl_WPT_PayRunDetail_Emp_ID { get; set; }
        public virtual tbl_WPT_PayRunDetail_Emp tbl_WPT_PayRunDetail_Emp { get; set; }

        [Required]
        [Display(Name = "Instance Date")]
        public DateTime InstanceDate { get; set; }

        [Display(Name = "Check-In")]
        public DateTime? CheckIn { get; set; }

        [Display(Name = "Check-Out")]
        public DateTime? CheckOut { get; set; }

        [Required]
        [Display(Name = "Present")]
        public bool Present { get; set; }

        [Required]
        [Display(Name = "Absent")]
        public bool Absent { get; set; }

        [Required]
        [Display(Name = "Absent In Holiday")]
        public bool AbsentInHoliday { get; set; }

        [Required]
        [Display(Name = "Absent Penalty")]
        public bool AbsentPenalty { get; set; }

        [Required]
        [Display(Name = "Holiday")]
        public bool Holiday { get; set; }

        [Required]
        [Display(Name = "Late-In")]
        public bool LateIn { get; set; }

        [Required]
        [Display(Name = "Early-Out")]
        public bool EarlyOut { get; set; }

        [Required]
        [Display(Name = "Half Shit")]
        public bool HalfShit { get; set; }

        [Required]
        [Display(Name = "Half Shit Penalty")]
        public bool HalfShitPenalty { get; set; }

        [Required]
        [Display(Name = "OverTime")]
        public int OT { get; set; }

        [Required]
        [Display(Name = "Shift Minutes")]
        public int ShiftMinutes { get; set; }

        [Required]
        [Display(Name = "Before Shift Minutes")]
        public int BeforeShiftMinutes { get; set; }

        [Required]
        [Display(Name = "After Shift Minutes")]
        public int AfterShiftMinutes { get; set; }

        [ForeignKey(nameof(tbl_WPT_LeaveRequisition))]

        public int? FK_tbl_WPT_LeaveRequisition_ID { get; set; }
        public virtual tbl_WPT_LeaveRequisition tbl_WPT_LeaveRequisition { get; set; }

        [ForeignKey(nameof(tbl_WPT_Shift))]

        public int? FK_tbl_WPT_Shift_ID { get; set; }
        public virtual tbl_WPT_Shift tbl_WPT_Shift { get; set; }

        [Required]
        [Display(Name = "Shift Working Minutes")]
        public int ShiftWorkingMinutes { get; set; }

        [Required]
        [Display(Name = "WDValue")]
        public double WDValue { get; set; }

        [Required]
        [Display(Name = "LeaveValue")]
        public double LeaveValue { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_PayRunDetail_EmpDetail_Wage")]
    public class tbl_WPT_PayRunDetail_EmpDetail_Wage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_PayRunDetail_Emp))]

        public int FK_tbl_WPT_PayRunDetail_Emp_ID { get; set; }
        public virtual tbl_WPT_PayRunDetail_Emp tbl_WPT_PayRunDetail_Emp { get; set; }

        [Required]
        [Display(Name = "Rate")]
        public double Rate { get; set; }

        [Required]
        [Display(Name = "Qty")]
        public double Qty { get; set; }

        [Required]
        [Display(Name = "Debit")]
        public double Debit { get; set; }

        [Required]
        [Display(Name = "Credit")]
        public double Credit { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeeSalaryStructure))]

        public int? FK_tbl_WPT_EmployeeSalaryStructure_ID_Basic { get; set; }
        public virtual tbl_WPT_EmployeeSalaryStructure tbl_WPT_EmployeeSalaryStructure { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeeSalaryStructureAllowance))]

        public int? FK_tbl_WPT_EmployeeSalaryStructureAllowance_ID { get; set; }
        public virtual tbl_WPT_EmployeeSalaryStructureAllowance tbl_WPT_EmployeeSalaryStructureAllowance { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeeSalaryStructureDeductible))]

        public int? FK_tbl_WPT_EmployeeSalaryStructureDeductible_ID { get; set; }
        public virtual tbl_WPT_EmployeeSalaryStructureDeductible tbl_WPT_EmployeeSalaryStructureDeductible { get; set; }

        [ForeignKey(nameof(tbl_WPT_tbl_OTPolicy))]

        public int? FK_tbl_WPT_tbl_OTPolicy_ID { get; set; }
        public virtual tbl_WPT_tbl_OTPolicy tbl_WPT_tbl_OTPolicy { get; set; }

        [ForeignKey(nameof(tbl_WPT_IncentivePolicy))]

        public int? FK_tbl_WPT_IncentivePolicy_ID { get; set; }
        public virtual tbl_WPT_IncentivePolicy tbl_WPT_IncentivePolicy { get; set; }

        [ForeignKey(nameof(tbl_WPT_LoanDetail))]

        public int? FK_tbl_WPT_LoanDetail_ID { get; set; }
        public virtual tbl_WPT_LoanDetail tbl_WPT_LoanDetail { get; set; }

        [ForeignKey(nameof(tbl_WPT_IncrementDetail))]

        public int? FK_tbl_WPT_IncrementDetail_ID_Arrear { get; set; }
        public virtual tbl_WPT_IncrementDetail tbl_WPT_IncrementDetail { get; set; }

        [ForeignKey(nameof(tbl_WPT_RewardDetail))]

        public int? FK_tbl_WPT_RewardDetail_ID { get; set; }
        public virtual tbl_WPT_RewardDetail tbl_WPT_RewardDetail { get; set; }

        [ForeignKey(nameof(tbl_WPT_LeavePolicy))]

        public int? FK_tbl_WPT_LeavePolicy_ID_EL { get; set; }
        public virtual tbl_WPT_LeavePolicy tbl_WPT_LeavePolicy { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeePensionStructure))]

        public int? FK_tbl_WPT_EmployeePensionStructure_ID { get; set; }
        public virtual tbl_WPT_EmployeePensionStructure tbl_WPT_EmployeePensionStructure { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_PayRunDetail_Payment")]
    public class tbl_WPT_PayRunDetail_Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_PayRunMaster))]

        public int FK_tbl_WPT_PayRunMaster_ID { get; set; }
        public virtual tbl_WPT_PayRunMaster tbl_WPT_PayRunMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CompanyBankDetail))]

        public int FK_tbl_WPT_CompanyBankDetail_ID { get; set; }
        public virtual tbl_WPT_CompanyBankDetail tbl_WPT_CompanyBankDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_TransactionMode))]

        public int FK_tbl_WPT_TransactionMode_ID { get; set; }
        public virtual tbl_WPT_TransactionMode tbl_WPT_TransactionMode { get; set; }

        [MaxLength(50)]
        [Display(Name = "Instrument No")]
        public string InstrumentNo { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Emp.tbl_WPT_PayRunDetail_Payment_Primary))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emp_Primarys { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_Emp.tbl_WPT_PayRunDetail_Payment_Secondary))]
        public virtual ICollection<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emp_Secondarys { get; set; }

    }

    //---------------------------------Reward-----------------------------------//

    [Table("tbl_WPT_RewardType")]
    public class tbl_WPT_RewardType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [Display(Name = "Reward Name")]
        [MaxLength(25)]
        public string RewardName { get; set; }

        [Display(Name = "Remarks")]
        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardMaster.tbl_WPT_RewardType))]
        public virtual ICollection<tbl_WPT_RewardMaster> tbl_WPT_RewardMasters { get; set; }

    }

    [Table("tbl_WPT_RewardMaster")]
    public class tbl_WPT_RewardMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months))]

        public int FK_tbl_WPT_CalendarYear_Months_ID { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_RewardType))]

        public int FK_tbl_WPT_RewardType_ID { get; set; }
        public virtual tbl_WPT_RewardType tbl_WPT_RewardType { get; set; }

        [Display(Name = "Remarks")]
        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardDetail.tbl_WPT_RewardMaster))]
        public virtual ICollection<tbl_WPT_RewardDetail> tbl_WPT_RewardDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardDetail_Payment.tbl_WPT_RewardMaster))]
        public virtual ICollection<tbl_WPT_RewardDetail_Payment> tbl_WPT_RewardDetail_Payments { get; set; }

    }

    [Table("tbl_WPT_RewardDetail_Payment")]
    public class tbl_WPT_RewardDetail_Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_RewardMaster))]

        public int FK_tbl_WPT_RewardMaster_ID { get; set; }
        public virtual tbl_WPT_RewardMaster tbl_WPT_RewardMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CompanyBankDetail))]

        public int FK_tbl_WPT_CompanyBankDetail_ID { get; set; }
        public virtual tbl_WPT_CompanyBankDetail tbl_WPT_CompanyBankDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_TransactionMode))]

        public int FK_tbl_WPT_TransactionMode_ID { get; set; }
        public virtual tbl_WPT_TransactionMode tbl_WPT_TransactionMode { get; set; }

        [MaxLength(50)]
        [Display(Name = "Instrument No")]
        public string InstrumentNo { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_RewardDetail.tbl_WPT_RewardDetail_Payment))]
        public virtual ICollection<tbl_WPT_RewardDetail> tbl_WPT_RewardDetails { get; set; }

    }

    [Table("tbl_WPT_RewardDetail")]
    public class tbl_WPT_RewardDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_RewardMaster))]

        public int FK_tbl_WPT_RewardMaster_ID { get; set; }
        public virtual tbl_WPT_RewardMaster tbl_WPT_RewardMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [Display(Name = "Reward Amount")]
        public double RewardAmount { get; set; }

        [Required]
        [Display(Name = "With Salary")]
        public bool WithSalary { get; set; }

        [Display(Name = "Remarks")]
        [MaxLength(50)]
        public string Remarks { get; set; }

        [ForeignKey(nameof(tbl_WPT_RewardDetail_Payment))]

        public int? FK_tbl_WPT_RewardDetail_Payment_ID { get; set; }
        public virtual tbl_WPT_RewardDetail_Payment tbl_WPT_RewardDetail_Payment { get; set; }

        [ForeignKey(nameof(tbl_WPT_EmployeeBankDetail))]

        public int? FK_tbl_WPT_EmployeeBankDetail_ID { get; set; }
        public virtual tbl_WPT_EmployeeBankDetail tbl_WPT_EmployeeBankDetail { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_Wage.tbl_WPT_RewardDetail))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

    }

    //----------------------------------------wage-------------------------------//

    [Table("tbl_WPT_WageCalculationType")]
    public class tbl_WPT_WageCalculationType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [Display(Name = "Calculation Name")]
        [MaxLength(10)]
        public string CalculationName { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructureAllowance.tbl_WPT_WageCalculationType))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructureAllowance> tbl_WPT_EmployeeSalaryStructureAllowances { get; set; }

        [InverseProperty(nameof(tbl_WPT_EmployeeSalaryStructureDeductible.tbl_WPT_WageCalculationType))]
        public virtual ICollection<tbl_WPT_EmployeeSalaryStructureDeductible> tbl_WPT_EmployeeSalaryStructureDeductibles { get; set; }

    }

    //----------------------------------shift & ShiftRoster-------------------------------------------//
    [Table("tbl_WPT_Shift")]
    public class tbl_WPT_Shift
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Shift Name")]
        public string ShiftName { get; set; }

        [Required]
        [MaxLength(8)]
        public string Prefix { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "LateIn Time")]
        public TimeSpan LateInTime { get; set; }

        [Required]
        [Display(Name = "EarlyOut Time")]
        public TimeSpan EarlyOutTime { get; set; }

        [Required]
        [Display(Name = "EndTime")]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Display(Name = "CheckIn Start Time")]
        public TimeSpan CheckInStartTime { get; set; }

        [Required]
        [Display(Name = "CheckOut EndTime")]
        public TimeSpan CheckOutEndTime { get; set; }

        [Required]
        [Display(Name = "HalfShift Limit In Minutes")]
        public int HalfShiftLimit_Minutes { get; set; }

        [Required]
        [Display(Name = "ShiftLimit Minutes")]
        public int ShiftLimit_Minutes { get; set; }

        [Required]
        [Display(Name = "OT Margin Minutes")]
        public int OTMargin_Minutes { get; set; }

        [Required]
        [Display(Name = "Late-In")]
        public bool LI { get; set; }

        [Required]
        [Display(Name = "Early-Out")]
        public bool EO { get; set; }
        //-------------------------------------OT-----------------------------//

        [Required]
        [Display(Name = "OT On Holiday")]
        public bool OT_HD { get; set; }

        [Required]
        [Display(Name = "OT Before Shift On Working Day")]
        public bool OT_BeforeShift_NON_HD { get; set; }

        [Required]
        [Display(Name = "OT After Shift On Working Day")]
        public bool OT_AfterShift_NON_HD { get; set; }

        //-------------------------------------xxxxxxxxxx-----------------------------//
        [Required]
        [Display(Name = "HalfShift")]
        public bool HS { get; set; }

        [Required]
        [Display(Name = "Holiday")]
        public bool HD { get; set; }

        [Required]
        [Display(Name = "Shift Limit")]
        public bool ShiftLimit { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_Employee.tbl_WPT_Shift))]
        public virtual ICollection<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }

        [InverseProperty(nameof(tbl_WPT_ShiftRosterDetail.tbl_WPT_Shift))]
        public virtual ICollection<tbl_WPT_ShiftRosterDetail> tbl_WPT_ShiftRosterDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_PayRunDetail_EmpDetail_AT.tbl_WPT_Shift))]
        public virtual ICollection<tbl_WPT_PayRunDetail_EmpDetail_AT> tbl_WPT_PayRunDetail_EmpDetail_ATs { get; set; }

    }

    [Table("tbl_WPT_ShiftRosterMaster")]
    public class tbl_WPT_ShiftRosterMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear_Months))]

        public int FK_tbl_WPT_CalendarYear_Months_ID { get; set; }
        public virtual tbl_WPT_CalendarYear_Months tbl_WPT_CalendarYear_Months { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Roster Name")]
        public string RosterName { get; set; }

        [MaxLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_ShiftRosterDetail.tbl_WPT_ShiftRosterMaster))]
        public virtual ICollection<tbl_WPT_ShiftRosterDetail> tbl_WPT_ShiftRosterDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_ShiftRosterDetail_Employee.tbl_WPT_ShiftRosterMaster))]
        public virtual ICollection<tbl_WPT_ShiftRosterDetail_Employee> tbl_WPT_ShiftRosterDetail_Employees { get; set; }

    }

    [Table("tbl_WPT_ShiftRosterDetail")]
    public class tbl_WPT_ShiftRosterDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_ShiftRosterMaster))]

        public int FK_tbl_WPT_ShiftRosterMaster_ID { get; set; }
        public virtual tbl_WPT_ShiftRosterMaster tbl_WPT_ShiftRosterMaster { get; set; }

        [Required]
        [Display(Name = "Roster Date")]
        public DateTime RosterDate { get; set; }


        [ForeignKey(nameof(tbl_WPT_Shift))]
        public int? FK_tbl_WPT_Shift_ID { get; set; }
        public virtual tbl_WPT_Shift tbl_WPT_Shift { get; set; }


        [ForeignKey(nameof(tbl_WPT_Holiday))]
        public int? FK_tbl_WPT_Holiday_ID { get; set; }
        public virtual tbl_WPT_Holiday tbl_WPT_Holiday { get; set; }

        [Required]
        public bool ApplyDefaultHoliday { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_ShiftRosterDetail_Employee")]
    public class tbl_WPT_ShiftRosterDetail_Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_ShiftRosterMaster))]

        public int FK_tbl_WPT_ShiftRosterMaster_ID { get; set; }
        public virtual tbl_WPT_ShiftRosterMaster tbl_WPT_ShiftRosterMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    //------------------------------------holiday--------------------------------------//
    [Table("tbl_WPT_Holiday")]
    public class tbl_WPT_Holiday
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Holiday Name")]
        public string HolidayName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_ShiftRosterDetail.tbl_WPT_Holiday))]
        public virtual ICollection<tbl_WPT_ShiftRosterDetail> tbl_WPT_ShiftRosterDetails { get; set; }

        [InverseProperty(nameof(tbl_WPT_CalendarYear_Months_Holidays.tbl_WPT_Holiday))]
        public virtual ICollection<tbl_WPT_CalendarYear_Months_Holidays> tbl_WPT_CalendarYear_Months_Holidayss { get; set; }

    }

    //------------------------------------log-----------------------------------//
    [Table("tbl_WPT_AttendanceLog")]
    public class tbl_WPT_AttendanceLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [ForeignKey(nameof(tbl_WPT_Employee))]
        public int? FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }


        [ForeignKey(nameof(tbl_WPT_Machine))]
        public int? FK_tbl_WPT_Machine_ID { get; set; }
        public virtual tbl_WPT_Machine tbl_WPT_Machine { get; set; }

        [MaxLength(25)]
        [Display(Name = "Machine Enrollment No")]
        public string ATEnrollmentNo { get; set; }

        [Required]
        [Display(Name = "AT InOut Mode")]
        public int ATInOutMode { get; set; }        

        [Required]
        [Display(Name = "AT DateTime")]
        public DateTime ATDateTime { get; set; }

        [Required]
        [Display(Name = "Logged by")]
        public int Loggedby { get; set; }

        [ForeignKey(nameof(tbl_WPT_ATBulkManualDetail_Employee))]
        public int? FK_tbl_WPT_ATBulkManualDetail_Employee_ID { get; set; }
        public virtual tbl_WPT_ATBulkManualDetail_Employee tbl_WPT_ATBulkManualDetail_Employee { get; set; }

    }

    [Table("tbl_WPT_ATBulkManualMaster")]
    public class tbl_WPT_ATBulkManualMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "AT DateTime")]
        public DateTime ATDateTime { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_ATInOutMode))]
        public int FK_tbl_WPT_ATInOutMode_ID { get; set; }
        public virtual tbl_WPT_ATInOutMode tbl_WPT_ATInOutMode { get; set; }

        [MaxLength(50)]
        public string Reason { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_ATBulkManualDetail_Employee.tbl_WPT_ATBulkManualMaster))]
        public virtual ICollection<tbl_WPT_ATBulkManualDetail_Employee> tbl_WPT_ATBulkManualDetail_Employees { get; set; }

    }

    [Table("tbl_WPT_ATBulkManualDetail_Employee")]
    public class tbl_WPT_ATBulkManualDetail_Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_ATBulkManualMaster))]
        public int FK_tbl_WPT_ATBulkManualMaster_ID { get; set; }
        public virtual tbl_WPT_ATBulkManualMaster tbl_WPT_ATBulkManualMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]
        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_AttendanceLog.tbl_WPT_ATBulkManualDetail_Employee))]
        public virtual ICollection<tbl_WPT_AttendanceLog> tbl_WPT_AttendanceLogs { get; set; }

    }

    //--------------------------------------grace----------------------------------//
    [Table("tbl_WPT_ATTimeGrace")]
    public class tbl_WPT_ATTimeGrace
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Doc No")]

        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }

        [Required]
        [Display(Name = "Date Till")]
        public DateTime DateTill { get; set; }

        [Required]
        [Display(Name = "Ignore Late-In")]
        public bool Ignore_LI { get; set; }

        [Required]
        [Display(Name = "Ignore Early-Out")]
        public bool Ignore_EO { get; set; }

        [Required]
        [Display(Name = "Ignore HalfShift")]
        public bool Ignore_HS { get; set; }

        [Required]
        [Display(Name = "Ignore OT")]
        public bool Ignore_OT { get; set; }

        [Required]
        [Display(Name = "Ignore Present")]
        public bool Ignore_Present { get; set; }

        [Required]
        [Display(Name = "Ignore Absent")]
        public bool Ignore_Absent { get; set; }

        [Required]
        [Display(Name = "Ignore Auto Leaves")]
        public bool Ignore_AutoLeaves { get; set; }

        [Required]
        [Display(Name = "Ignore Holidays")]
        public bool Ignore_Holidays { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_ATTimeGraceEmployeeLink.tbl_WPT_ATTimeGrace))]
        public virtual ICollection<tbl_WPT_ATTimeGraceEmployeeLink> tbl_WPT_ATTimeGraceEmployeeLinks { get; set; }
    }

    [Table("tbl_WPT_ATTimeGraceEmployeeLink")]
    public class tbl_WPT_ATTimeGraceEmployeeLink
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_WPT_ATTimeGrace))]
        public int FK_tbl_WPT_ATTimeGrace_ID { get; set; }
        public virtual tbl_WPT_ATTimeGrace tbl_WPT_ATTimeGrace { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]
        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }


        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    //-------------------------Policy----------------------------------//
    [Table("tbl_WPT_PolicyGeneral")]
    public class tbl_WPT_PolicyGeneral
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Calendar Year Start Month")]
        public int CalendarYear_StartMonth { get; set; }

        [Required]
        [Display(Name = "Calendar Year Start DayNo Of Every Month")]
        public int CalendarYear_StartDayNoOfEveryMonth { get; set; }

        [Required]
        [Display(Name = "Calendar Year Recreate On Closing Month")]
        public int CalendarYear_RecreateOnClosingMonth { get; set; }

        [Required]
        [Range(0, 9)]
        [Display(Name = "Wage Cash Round Ones Into Tens")]
        public int WageCash_RoundOnesIntoTens { get; set; }

        [Required]
        [Range(0, 28)]
        [Display(Name = "Min WD To Generate Wage FOR 1st Month")]
        public int MinWDToGenerateWageForFirstMonth { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_PolicyPenaltyOnWT")]
    public class tbl_WPT_PolicyPenaltyOnWT
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Policy Name")]
        public string PolicyName { get; set; }

        //-------------------------------Late In -----------------------//
        [Required]
        [Display(Name = "Penalty Absent At Every LI")]
        public int PenaltyAbsentAtEvery_LI { get; set; }

        [Required]
        [Display(Name = "Penalty HS From Every LI")]
        public int PenaltyHalfShiftFromEvery_LI { get; set; }

        //-------------------------------Late In Monthly-----------------------//
        [Required]
        [Display(Name = "Monthly LateIn Grace Limit In Minutes MGLI")]
        public int MonthlyLateInGraceLimit_Minutes_MGLI { get; set; }

        [Required]
        [Display(Name = "HSP From Monthly Grace MGLI in minutes")]
        public int PenaltyHalfShiftFromEveryMinutes_MGLI { get; set; }


        //-------------------------------EO-----------------------//
        [Required]
        [Display(Name = "Penalty Absent A tEvery EO")]
        public int PenaltyAbsentAtEvery_EO { get; set; }

        [Required]
        [Display(Name = "Penalty HS From Every EO")]
        public int PenaltyHalfShiftFromEvery_EO { get; set; }

        [Required]
        [Display(Name = "Penalty Absent At Every HS")]
        public int PenaltyAbsentAtEvery_HS { get; set; }

        [Required]
        [Display(Name = "Keep HS From Every HS")]
        public int KeepHalfShiftFromEvery_HS { get; set; }

        [Required]
        [Display(Name = "Penalty Absent On Missing IN or OUT")]
        public bool PenaltyAbsentOnMissingINorOUT { get; set; }

        [Required]
        [Display(Name = "Penalty HalfShift On Missing IN or OUT")]
        public bool PenaltyHalfShiftOnMissingINorOUT { get; set; }

        [MaxLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_PolicyPenaltyOnWTDetail_Designation.tbl_WPT_PolicyPenaltyOnWT))]
        public virtual ICollection<tbl_WPT_PolicyPenaltyOnWTDetail_Designation> tbl_WPT_PolicyPenaltyOnWTDetail_Designations { get; set; }

    }

    [Table("tbl_WPT_PolicyPenaltyOnWTDetail_Designation")]
    public class tbl_WPT_PolicyPenaltyOnWTDetail_Designation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_PolicyPenaltyOnWT))]

        public int FK_tbl_WPT_PolicyPenaltyOnWT_ID { get; set; }
        public virtual tbl_WPT_PolicyPenaltyOnWT tbl_WPT_PolicyPenaltyOnWT { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Designation))]

        public int FK_tbl_WPT_Designation_ID { get; set; }
        public virtual tbl_WPT_Designation tbl_WPT_Designation { get; set; }

        [MaxLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    //-----------------------------Job Position------------------------------------------//
    [Table("tbl_WPT_Job_KPI")]
    public class tbl_WPT_Job_KPI
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [MaxLength(10)]
        public string Prefix { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_JobPositionDetail_KPI.tbl_WPT_Job_KPI))]
        public virtual ICollection<tbl_WPT_JobPositionDetail_KPI> tbl_WPT_JobPositionDetail_KPIs { get; set; }

        [InverseProperty(nameof(tbl_WPT_AppraisalDetail.tbl_WPT_Job_KPI))]
        public virtual ICollection<tbl_WPT_AppraisalDetail> tbl_WPT_AppraisalDetails { get; set; }

    }

    [Table("tbl_WPT_JobPositionMaster")]
    public class tbl_WPT_JobPositionMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Job Code")]
        public string JobCode { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Department))]

        public int FK_tbl_WPT_Department_ID { get; set; }
        public virtual tbl_WPT_Department tbl_WPT_Department { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Designation))]

        public int FK_tbl_WPT_Designation_ID { get; set; }
        public virtual tbl_WPT_Designation tbl_WPT_Designation { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EducationalLevelType))]

        public int FK_tbl_WPT_EducationalLevelType_ID { get; set; }
        public virtual tbl_WPT_EducationalLevelType tbl_WPT_EducationalLevelType { get; set; }

        [Required]
        [Display(Name = "Standard Basic Salary")]
        public double StandardBasicSalary { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_JobPositionDetail_Responsibility.tbl_WPT_JobPositionMaster))]
        public virtual ICollection<tbl_WPT_JobPositionDetail_Responsibility> tbl_WPT_JobPositionDetail_Responsibilitys { get; set; }

        [InverseProperty(nameof(tbl_WPT_JobPositionDetail_KPI.tbl_WPT_JobPositionMaster))]
        public virtual ICollection<tbl_WPT_JobPositionDetail_KPI> tbl_WPT_JobPositionDetail_KPIs { get; set; }

        [InverseProperty(nameof(tbl_WPT_HiringRequest.tbl_WPT_JobPositionMaster))]
        public virtual ICollection<tbl_WPT_HiringRequest> tbl_WPT_HiringRequests { get; set; }

    }

    [Table("tbl_WPT_JobPositionDetail_Responsibility")]
    public class tbl_WPT_JobPositionDetail_Responsibility
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_JobPositionMaster))]

        public int FK_tbl_WPT_JobPositionMaster_ID { get; set; }
        public virtual tbl_WPT_JobPositionMaster tbl_WPT_JobPositionMaster { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_WPT_JobPositionDetail_KPI")]
    public class tbl_WPT_JobPositionDetail_KPI
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_JobPositionMaster))]

        public int FK_tbl_WPT_JobPositionMaster_ID { get; set; }
        public virtual tbl_WPT_JobPositionMaster tbl_WPT_JobPositionMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Job_KPI))]

        public int FK_tbl_WPT_Job_KPI_ID { get; set; }
        public virtual tbl_WPT_Job_KPI tbl_WPT_Job_KPI { get; set; }

        [Required]
        [Display(Name = "KPI %")]
        public double KPI_Percentage { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    //---------------------------------------------Appraisal----------------------------------//
    [Table("tbl_WPT_AppraisalMaster")]
    public class tbl_WPT_AppraisalMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_CalendarYear))]

        public int FK_tbl_WPT_CalendarYear_ID { get; set; }
        public virtual tbl_WPT_CalendarYear tbl_WPT_CalendarYear { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }

        [Required]
        [Display(Name = "Date Till")]
        public DateTime DateTill { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_AppraisalDetail.tbl_WPT_AppraisalMaster))]
        public virtual ICollection<tbl_WPT_AppraisalDetail> tbl_WPT_AppraisalDetails { get; set; }

    }

    [Table("tbl_WPT_AppraisalDetail")]
    public class tbl_WPT_AppraisalDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_AppraisalMaster))]

        public int FK_tbl_WPT_AppraisalMaster_ID { get; set; }
        public virtual tbl_WPT_AppraisalMaster tbl_WPT_AppraisalMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_Job_KPI))]

        public int FK_tbl_WPT_Job_KPI_ID { get; set; }
        public virtual tbl_WPT_Job_KPI tbl_WPT_Job_KPI { get; set; }

        [Required]
        [Display(Name = "KPI %")]
        public double KPI_Percentage { get; set; }

        [Required]
        [Display(Name = "KPI Score")]
        public double KPI_Score { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    //---------------------------------Hiring---------------------------//
    [Table("tbl_WPT_HiringType")]
    public class tbl_WPT_HiringType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_HiringRequest.tbl_WPT_HiringType))]
        public virtual ICollection<tbl_WPT_HiringRequest> tbl_WPT_HiringRequests { get; set; }

    }

    [Table("tbl_WPT_HiringRequest")]
    public class tbl_WPT_HiringRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Doc No")]

        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_JobPositionMaster))]

        public int FK_tbl_WPT_JobPositionMaster_ID { get; set; }
        public virtual tbl_WPT_JobPositionMaster tbl_WPT_JobPositionMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EmploymentType))]

        public int FK_tbl_WPT_EmploymentType_ID { get; set; }
        public virtual tbl_WPT_EmploymentType tbl_WPT_EmploymentType { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_HiringType))]

        public int FK_tbl_WPT_HiringType_ID { get; set; }
        public virtual tbl_WPT_HiringType tbl_WPT_HiringType { get; set; }

        [ForeignKey(nameof(tbl_WPT_Employee))]

        public int? FK_tbl_WPT_Employee_ID_Replacement { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        [MaxLength(8)]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Age From")]
        public int AgeFrom { get; set; }

        [Required]
        [Display(Name = "Age Till")]
        public int AgeTill { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EducationalLevelType))]

        public int FK_tbl_WPT_EducationalLevelType_ID { get; set; }
        public virtual tbl_WPT_EducationalLevelType tbl_WPT_EducationalLevelType { get; set; }

        [Required]
        [Display(Name = "CGPA")]
        public double CGPA { get; set; }

        [Required]
        [Display(Name = "Percentage")]
        public double Percentage { get; set; }

        [Required]
        [MaxLength(4)]
        [Display(Name = "Grade / Division")]
        public string Grade_Division { get; set; }

        [Required]
        [Display(Name = "Experience From")]
        public int ExperienceFrom { get; set; }

        [Required]
        [Display(Name = "Experience Till")]
        public int ExperienceTill { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Experience Detail")]
        public string ExperienceDetail { get; set; }

        [MaxLength(250)]
        [Display(Name = "Skills Detail")]
        public string SkillsDetail { get; set; }

        [MaxLength(250)]
        [Display(Name = "Additional Detail")]
        public string AdditionalDetail { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_HiringRequest_Interview.tbl_WPT_HiringRequest))]
        public virtual ICollection<tbl_WPT_HiringRequest_Interview> tbl_WPT_HiringRequest_Interviews { get; set; }

    }

    [Table("tbl_WPT_JobApplication")]
    public class tbl_WPT_JobApplication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Display(Name = "Application No")]

        public int? ApplicationNo { get; set; }

        [Required]
        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Full Name")]
        [Required]
        public string FullName { get; set; }

        [Required]
        [MaxLength(8)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Father OR Husband Name")]
        public string FatherORHusbandName { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        [Required]
        [MaxLength(13)]
        public string CNIC { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }

        [Required]
        [MaxLength(14)]
        [Display(Name = "Cell Phone No")]
        public string CellPhoneNo { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_EducationalLevelType))]

        public int FK_tbl_WPT_EducationalLevelType_ID_Last { get; set; }
        public virtual tbl_WPT_EducationalLevelType tbl_WPT_EducationalLevelType { get; set; }

        [Required]
        [Display(Name = "Experience Year")]
        public int ExperienceYear { get; set; }

        [MaxLength(50)]
        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [MaxLength(50)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_WPT_HiringRequest_Interview.tbl_WPT_JobApplication))]
        public virtual ICollection<tbl_WPT_HiringRequest_Interview> tbl_WPT_HiringRequest_Interviews { get; set; }

    }

    [Table("tbl_WPT_HiringRequest_Interview")]
    public class tbl_WPT_HiringRequest_Interview
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_HiringRequest))]

        public int FK_tbl_WPT_HiringRequest_ID { get; set; }
        public virtual tbl_WPT_HiringRequest tbl_WPT_HiringRequest { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_JobApplication))]

        public int FK_tbl_WPT_JobApplication_ID { get; set; }
        public virtual tbl_WPT_JobApplication tbl_WPT_JobApplication { get; set; }

        [Required]
        [Display(Name = "Qualification Score")]
        public int Score_Qualification { get; set; }

        [Required]
        [Display(Name = "Experience Score")]
        public int Score_Experience { get; set; }

        [Required]
        [Display(Name = "Skills Score")]
        public int Score_Skills { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        public bool Selected { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    #endregion
}
