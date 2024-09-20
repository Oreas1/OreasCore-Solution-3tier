using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using OreasModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OreasServices
{
    public class OreasDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
        ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public OreasDbContext(DbContextOptions<OreasDbContext> options)
            : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ApplicationUser>()
                .Property(u => u.MyID)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<UDTVF_UserAuthorizedOperations>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<VM_WPT_EmployeeSearchModal>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<VM_WPT_LeavePolicySearchModal>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<VM_Ac_COASearchModal>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<VM_Inv_WHMSearchModal>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<VM_WPT_SectionSearchModal>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<UDTVF_Inv_GetLastPOWithBestSupplier>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<GetATOutComeOfEmployee>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<USP_WPT_DashboardTeamATSummary>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            builder.Entity<USP_Pro_Composition_GetMaterialAvailability>(entity =>
            {
                entity.HasNoKey()
                      .ToView(null);
            });
            //builder.Entity<VM_Inv_ProductSearchModal>(entity =>
            //{
            //    entity.HasNoKey()
            //          .ToView(null);

            //});
            builder.Entity<VM_Inv_ProductSearchModal>(entity =>
            {
                entity.HasNoKey()
                      .ToTable("VM_Inv_ProductSearchModal", t => t.ExcludeFromMigrations());
            });
            builder.Entity<VM_Inv_ReferenceSearchModal>(entity =>
            {
                entity.HasNoKey()
                      .ToTable("VM_Inv_ReferenceSearchModal", t => t.ExcludeFromMigrations());
            });
            builder.Entity<VM_Inv_OrderNoteSearchModal>(entity =>
            {
                entity.HasNoKey()
                      .ToTable("VM_Inv_OrderNoteSearchModal", t => t.ExcludeFromMigrations());
            });
            base.OnModelCreating(builder);

        }
        public virtual DbSet<UDTVF_UserAuthorizedOperations> UDTVF_UserAuthorizedOperations { get; set; }
        public virtual DbSet<VM_WPT_EmployeeSearchModal> VM_WPT_EmployeeSearchModals { get; set; }
        public virtual DbSet<VM_WPT_LeavePolicySearchModal> VM_WPT_LeavePolicySearchModals { get; set; }
        public virtual DbSet<VM_Ac_COASearchModal> VM_Ac_COASearchModals { get; set; }
        public virtual DbSet<VM_Inv_WHMSearchModal> VM_Inv_WHMSearchModals { get; set; }
        public virtual DbSet<VM_WPT_SectionSearchModal> VM_WPT_SectionSearchModals { get; set; }
        public virtual DbSet<VM_Inv_ProductSearchModal> VM_Inv_ProductSearchModals { get; set; }
        public virtual DbSet<VM_Inv_ReferenceSearchModal> VM_Inv_ReferenceSearchModals { get; set; }
        public virtual DbSet<VM_Inv_OrderNoteSearchModal> VM_Inv_OrderNoteSearchModals { get; set; }
        public virtual DbSet<UDTVF_Inv_GetLastPOWithBestSupplier> UDTVF_Inv_GetLastPOWithBestSuppliers { get; set; }
        public virtual DbSet<GetATOutComeOfEmployee> GetATOutComeOfEmployees { get; set; }
        public virtual DbSet<USP_WPT_DashboardTeamATSummary> USP_WPT_DashboardTeamATSummarys { get; set; }
        public virtual DbSet<USP_Pro_Composition_GetMaterialAvailability> USP_Pro_Composition_GetMaterialAvailabilitys { get; set; }

        #region Identity
        public DbSet<AspNetOreasGeneralSettings> AspNetOreasGeneralSettings { get; set; }
        public DbSet<AspNetOreasCompanyProfile> AspNetOreasCompanyProfile { get; set; }
        public DbSet<AspNetOreasArea> AspNetOreasAreas { get; set; }
        public DbSet<AspNetOreasArea_Form> AspNetOreasArea_Forms { get; set; }
        public DbSet<AspNetOreasAuthorizationScheme> AspNetOreasAuthorizationSchemes { get; set; }
        public DbSet<AspNetOreasAuthorizationScheme_Section> AspNetOreasAuthorizationScheme_Sections { get; set; }
        public DbSet<AspNetOreasAuthorizationScheme_WareHouse> AspNetOreasAuthorizationScheme_WareHouses { get; set; }
        public DbSet<AspNetOreasAuthorizationScheme_Area> AspNetOreasAuthorizationScheme_Areas { get; set; }
        public DbSet<AspNetOreasAuthorizationScheme_Area_Form> AspNetOreasAuthorizationScheme_Area_Forms { get; set; }
        public DbSet<AspNetOreasPriority> AspNetOreasPrioritys { get; set; }

        #endregion

        #region WPT

        public DbSet<tbl_WPT_CalculationMethod> tbl_WPT_CalculationMethods { get; set; }
        public DbSet<tbl_WPT_ActionList> tbl_WPT_ActionLists { get; set; }
        public DbSet<tbl_WPT_Bank> tbl_WPT_Banks { get; set; }
        public DbSet<tbl_WPT_Bank_Branch> tbl_WPT_Bank_Branchs { get; set; }
        public DbSet<tbl_WPT_CompanyBankDetail> tbl_WPT_CompanyBankDetails { get; set; }
        public DbSet<tbl_WPT_TransactionMode> tbl_WPT_TransactionModes { get; set; }
        public DbSet<tbl_WPT_EducationalLevelType> tbl_WPT_EducationalLevelTypes { get; set; }
        public DbSet<tbl_WPT_Machine> tbl_WPT_Machines { get; set; }
        public DbSet<tbl_WPT_Holiday> tbl_WPT_Holidays { get; set; }
        public DbSet<tbl_WPT_ATInOutMode> tbl_WPT_ATInOutModes { get; set; }
        public DbSet<tbl_WPT_ATType> tbl_WPT_ATTypes { get; set; }
        public DbSet<tbl_WPT_InActiveType> tbl_WPT_InActiveTypes { get; set; }
        public DbSet<tbl_WPT_EmploymentType> tbl_WPT_EmploymentTypes { get; set; }
        public DbSet<tbl_WPT_Employee> tbl_WPT_Employees { get; set; }
        public DbSet<tbl_WPT_EmployeeLevel> tbl_WPT_EmployeeLevels { get; set; }
        public DbSet<tbl_WPT_Employee_PFF> tbl_WPT_Employee_PFFs { get; set; }
        public DbSet<tbl_WPT_EmployeeBankDetail> tbl_WPT_EmployeeBankDetails { get; set; }


        public DbSet<tbl_WPT_Shift> tbl_WPT_Shifts { get; set; }
        public DbSet<tbl_WPT_ShiftRosterMaster> tbl_WPT_ShiftRosterMasters { get; set; }
        public DbSet<tbl_WPT_ShiftRosterDetail> tbl_WPT_ShiftRosterDetails { get; set; }
        public DbSet<tbl_WPT_ShiftRosterDetail_Employee> tbl_WPT_ShiftRosterDetail_Employees { get; set; }

        public DbSet<tbl_WPT_EmployeeSalaryStructure> tbl_WPT_EmployeeSalaryStructures { get; set; }
        public DbSet<tbl_WPT_EmployeeSalaryStructureAllowance> tbl_WPT_EmployeeSalaryStructureAllowances { get; set; }
        public DbSet<tbl_WPT_EmployeeSalaryStructureDeductible> tbl_WPT_EmployeeSalaryStructureDeductibles { get; set; }
        public DbSet<tbl_WPT_EmployeePensionStructure> tbl_WPT_EmployeePensionStructures { get; set; }

        //---------------------increment--------------------//
        public DbSet<tbl_WPT_IncrementMaster> tbl_WPT_IncrementMasters { get; set; }
        public DbSet<tbl_WPT_IncrementDetail> tbl_WPT_IncrementDetails { get; set; }
        public DbSet<tbl_WPT_IncrementBy> tbl_WPT_IncrementBys { get; set; }

        //-----------incentive-------------//
        public DbSet<tbl_WPT_IncentiveType> tbl_WPT_IncentiveTypes { get; set; }
        public DbSet<tbl_WPT_IncentivePolicy> tbl_WPT_IncentivePolicys { get; set; }
        public DbSet<tbl_WPT_IncentivePolicyDesignation> tbl_WPT_IncentivePolicyDesignations { get; set; }
        public DbSet<tbl_WPT_IncentivePolicyEmployees> tbl_WPT_IncentivePolicyEmployeess { get; set; }
        public DbSet<tbl_WPT_LoanType> tbl_WPT_LoanTypes { get; set; }
        public DbSet<tbl_WPT_LoanMaster> tbl_WPT_LoanMasters { get; set; }
        public DbSet<tbl_WPT_LoanDetail_Payment> tbl_WPT_LoanDetail_Payments { get; set; }
        public DbSet<tbl_WPT_LoanDetail> tbl_WPT_LoanDetails { get; set; }

        public DbSet<tbl_WPT_AllowanceType> tbl_WPT_AllowanceTypes { get; set; }
        public DbSet<tbl_WPT_DeductibleType> tbl_WPT_DeductibleTypes { get; set; }
        public DbSet<tbl_WPT_CalendarYear> tbl_WPT_CalendarYears { get; set; }
        public DbSet<tbl_WPT_CalendarYear_Months> tbl_WPT_CalendarYear_Monthss { get; set; }
        public DbSet<tbl_WPT_CalendarYear_Months_Holidays> tbl_WPT_CalendarYear_Months_Holidayss { get; set; }
        public DbSet<tbl_WPT_CalendarYear_LeaveEmps> tbl_WPT_CalendarYear_LeaveEmpss { get; set; }
        public DbSet<tbl_WPT_CalendarYear_LeaveEmps_Leaves> tbl_WPT_CalendarYear_LeaveEmps_Leavess { get; set; }

        //-----------------------------------------leavess----------------------------//
        public DbSet<tbl_WPT_LeaveCFOptions> tbl_WPT_LeaveCFOptionss { get; set; }
        public DbSet<tbl_WPT_LeavePolicy> tbl_WPT_LeavePolicys { get; set; }
        public DbSet<tbl_WPT_LeavePolicyNonPaid> tbl_WPT_LeavePolicyNonPaids { get; set; }
        public DbSet<tbl_WPT_LeavePolicyNonPaid_Designation> tbl_WPT_LeavePolicyNonPaid_Designations { get; set; }
        public DbSet<tbl_WPT_LeaveRequisition> tbl_WPT_LeaveRequisitions { get; set; }

        //-----------------------------------------xxxxxxxxxxxxxxxxxx-------------------------------//
        public DbSet<tbl_WPT_Department> tbl_WPT_Departments { get; set; }
        public DbSet<tbl_WPT_DepartmentDetail> tbl_WPT_DepartmentDetails { get; set; }
        public DbSet<tbl_WPT_DepartmentDetail_Section> tbl_WPT_DepartmentDetail_Sections { get; set; }
        public DbSet<tbl_WPT_DepartmentDetail_Section_HOS> tbl_WPT_DepartmentDetail_Section_HOSs { get; set; }
        public DbSet<tbl_WPT_Designation> tbl_WPT_Designations { get; set; }
        public DbSet<tbl_WPT_AttendanceLog> tbl_WPT_AttendanceLogs { get; set; }
        public DbSet<tbl_WPT_ATBulkManualMaster> tbl_WPT_ATBulkManualMasters { get; set; }
        public DbSet<tbl_WPT_ATBulkManualDetail_Employee> tbl_WPT_ATBulkManualDetail_Employees { get; set; }
        public DbSet<tbl_WPT_ATTimeGrace> tbl_WPT_ATTimeGraces { get; set; }
        public DbSet<tbl_WPT_ATTimeGraceEmployeeLink> tbl_WPT_ATTimeGraceEmployeeLinks { get; set; }


        //----------------------------payrun---------------------------------------------------------------------//
        public DbSet<tbl_WPT_PayRunToDo> tbl_WPT_PayRunToDos { get; set; }
        public DbSet<tbl_WPT_PayRunExemption> tbl_WPT_PayRunExemptions { get; set; }
        public DbSet<tbl_WPT_PayRunExemption_Emp> tbl_WPT_PayRunExemption_Emps { get; set; }
        public DbSet<tbl_WPT_PayRunMaster> tbl_WPT_PayRunMasters { get; set; }
        public DbSet<tbl_WPT_PayRunDetail_Emp> tbl_WPT_PayRunDetail_Emps { get; set; }
        public DbSet<tbl_WPT_PayRunDetail_EmpDetail_AT> tbl_WPT_PayRunDetail_EmpDetail_ATs { get; set; }

        public DbSet<tbl_WPT_PayRunDetail_EmpDetail_Wage> tbl_WPT_PayRunDetail_EmpDetail_Wages { get; set; }

        public DbSet<tbl_WPT_PayRunDetail_Payment> tbl_WPT_PayRunDetail_Payments { get; set; }

        //---------------------------------------------Reward---------------------------------------------------------//
        public DbSet<tbl_WPT_RewardType> tbl_WPT_RewardTypes { get; set; }
        public DbSet<tbl_WPT_RewardMaster> tbl_WPT_RewardMasters { get; set; }
        public DbSet<tbl_WPT_RewardDetail_Payment> tbl_WPT_RewardDetail_Payments { get; set; }
        public DbSet<tbl_WPT_RewardDetail> tbl_WPT_RewardDetails { get; set; }

        //-------------------------xxxxxxxxxxxx------------------------------------------------------------------//

        public DbSet<tbl_WPT_WageCalculationType> tbl_WPT_WageCalculationTypes { get; set; }

        //---------------------------------------------Policy---------------------------------------------------------//
        public DbSet<tbl_WPT_tbl_OTPolicy> tbl_WPT_tbl_OTPolicys { get; set; }
        public DbSet<tbl_WPT_PolicyGeneral> tbl_WPT_PolicyGenerals { get; set; }
        public DbSet<tbl_WPT_PolicyPenaltyOnWT> tbl_WPT_PolicyPenaltyOnWTs { get; set; }
        public DbSet<tbl_WPT_PolicyPenaltyOnWTDetail_Designation> tbl_WPT_PolicyPenaltyOnWTDetail_Designations { get; set; }

        //---------------------------------------------Job Position---------------------------------------------------------//
        public DbSet<tbl_WPT_Job_KPI> tbl_WPT_Job_KPIs { get; set; }
        public DbSet<tbl_WPT_JobPositionMaster> tbl_WPT_JobPositionMasters { get; set; }
        public DbSet<tbl_WPT_JobPositionDetail_Responsibility> tbl_WPT_JobPositionDetail_Responsibilitys { get; set; }
        public DbSet<tbl_WPT_JobPositionDetail_KPI> tbl_WPT_JobPositionDetail_KPIs { get; set; }

        //---------------------------------------------Appraisal---------------------------------------------------------//
        public DbSet<tbl_WPT_AppraisalMaster> tbl_WPT_AppraisalMasters { get; set; }
        public DbSet<tbl_WPT_AppraisalDetail> tbl_WPT_AppraisalDetails { get; set; }

        //---------------------------------------------Hiring---------------------------------------------------------//
        public DbSet<tbl_WPT_HiringType> tbl_WPT_HiringTypes { get; set; }
        public DbSet<tbl_WPT_HiringRequest> tbl_WPT_HiringRequests { get; set; }
        public DbSet<tbl_WPT_JobApplication> tbl_WPT_JobApplications { get; set; }
        public DbSet<tbl_WPT_HiringRequest_Interview> tbl_WPT_HiringRequest_Interviews { get; set; }

        #endregion

        #region Accounts
        public DbSet<tbl_Ac_CurrencyAndCountry> tbl_Ac_CurrencyAndCountrys { get; set; }
        public DbSet<tbl_Ac_FiscalYear> tbl_Ac_FiscalYears { get; set; }
        public DbSet<tbl_Ac_FiscalYear_ClosingEntryType> tbl_Ac_FiscalYear_ClosingEntryTypes { get; set; }
        public DbSet<tbl_Ac_FiscalYear_ClosingMaster> tbl_Ac_FiscalYear_ClosingMasters { get; set; }
        public DbSet<tbl_Ac_FiscalYear_ClosingDetail> tbl_Ac_FiscalYear_ClosingDetails { get; set; }
        public DbSet<tbl_Ac_ChartOfAccounts_Type> tbl_Ac_ChartOfAccounts_Types { get; set; }
        public DbSet<tbl_Ac_ChartOfAccounts> tbl_Ac_ChartOfAccountss { get; set; }
        public DbSet<tbl_Ac_CompositionCostingOverHeadFactorsMaster> tbl_Ac_CompositionCostingOverHeadFactorsMasters { get; set; }
        public DbSet<tbl_Ac_CompositionCostingOverHeadFactorsDetail> tbl_Ac_CompositionCostingOverHeadFactorsDetails { get; set; }
        public DbSet<tbl_Ac_CostingIndirectExpenseList> tbl_Ac_CostingIndirectExpenseLists { get; set; }
        public DbSet<tbl_Ac_CompositionCostingIndirectExpense> tbl_Ac_CompositionCostingIndirectExpenses { get; set; }
        public DbSet<tbl_Ac_CustomerApprovedRateList> tbl_Ac_CustomerApprovedRateLists { get; set; }
        public DbSet<tbl_Ac_CustomerSubDistributorList> tbl_Ac_CustomerSubDistributorLists { get; set; }
        public DbSet<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }
        public DbSet<tbl_Ac_PaymentPlanningMaster> tbl_Ac_PaymentPlanningMasters { get; set; }
        public DbSet<tbl_Ac_PaymentPlanningDetail> tbl_Ac_PaymentPlanningDetails { get; set; }
        public DbSet<tbl_Ac_PolicyPaymentTerm> tbl_Ac_PolicyPaymentTerms { get; set; }
        public DbSet<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys { get; set; }
        public DbSet<tbl_Ac_PolicyWHTaxOnPurchase> tbl_Ac_PolicyWHTaxOnPurchases { get; set; }
        public DbSet<tbl_Ac_PolicyWHTaxOnSales> tbl_Ac_PolicyWHTaxOnSaless { get; set; }

        public DbSet<tbl_Ac_V_BankTransactionMode> tbl_Ac_V_BankTransactionModes { get; set; }
        public DbSet<tbl_Ac_V_BankDocumentMaster> tbl_Ac_V_BankDocumentMasters { get; set; }
        public DbSet<tbl_Ac_V_BankDocumentDetail> tbl_Ac_V_BankDocumentDetails { get; set; }

        public DbSet<tbl_Ac_V_CashTransactionMode> tbl_Ac_V_CashTransactionModes { get; set; }
        public DbSet<tbl_Ac_V_CashDocumentMaster> tbl_Ac_V_CashDocumentMasters { get; set; }
        public DbSet<tbl_Ac_V_CashDocumentDetail> tbl_Ac_V_CashDocumentDetails { get; set; }

        public DbSet<tbl_Ac_V_JournalDocumentMaster> tbl_Ac_V_JournalDocumentMasters { get; set; }
        public DbSet<tbl_Ac_V_JournalDocumentDetail> tbl_Ac_V_JournalDocumentDetails { get; set; }

        public DbSet<tbl_Ac_V_JournalDocument2Master> tbl_Ac_V_JournalDocument2Masters { get; set; }
        public DbSet<tbl_Ac_V_JournalDocument2Detail> tbl_Ac_V_JournalDocument2Details { get; set; }

        #endregion

        #region Inventory

        public DbSet<tbl_Inv_MeasurementUnit> tbl_Inv_MeasurementUnits { get; set; }
        public DbSet<tbl_Inv_ProductClassification> tbl_Inv_ProductClassifications { get; set; }
        public DbSet<tbl_Inv_ProductType> tbl_Inv_ProductTypes { get; set; }
        public DbSet<tbl_Inv_ProductType_Category> tbl_Inv_ProductType_Categorys { get; set; }
        public DbSet<tbl_Inv_WareHouseMaster> tbl_Inv_WareHouseMasters { get; set; }
        public DbSet<tbl_Inv_WareHouseDetail> tbl_Inv_WareHouseDetails { get; set; }
        public DbSet<tbl_Inv_ProductRegistrationMaster> tbl_Inv_ProductRegistrationMasters { get; set; }
        public DbSet<tbl_Inv_ProductRegistrationDetail> tbl_Inv_ProductRegistrationDetails { get; set; }
        public DbSet<tbl_Inv_ProductRegistrationDetail_PNQcTest> tbl_Inv_ProductRegistrationDetail_PNQcTests { get; set; }
        public DbSet<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }        
        public DbSet<tbl_Inv_PurchaseNoteMaster> tbl_Inv_PurchaseNoteMasters { get; set; }
        public DbSet<tbl_Inv_PurchaseNoteDetail> tbl_Inv_PurchaseNoteDetails { get; set; }
        public DbSet<tbl_Inv_PurchaseReturnNoteMaster> tbl_Inv_PurchaseReturnNoteMasters { get; set; }
        public DbSet<tbl_Inv_PurchaseReturnNoteDetail> tbl_Inv_PurchaseReturnNoteDetails { get; set; }
        public DbSet<tbl_Inv_SalesNoteMaster> tbl_Inv_SalesNoteMasters { get; set; }
        public DbSet<tbl_Inv_SalesNoteDetail> tbl_Inv_SalesNoteDetails { get; set; }
        public DbSet<tbl_Inv_SalesReturnNoteMaster> tbl_Inv_SalesReturnNoteMasters { get; set; }
        public DbSet<tbl_Inv_SalesReturnNoteDetail> tbl_Inv_SalesReturnNoteDetails { get; set; }
        public DbSet<tbl_Inv_OrdinaryRequisitionType> tbl_Inv_OrdinaryRequisitionTypes { get; set; }
        public DbSet<tbl_Inv_OrdinaryRequisitionMaster> tbl_Inv_OrdinaryRequisitionMasters { get; set; }
        public DbSet<tbl_Inv_OrdinaryRequisitionDetail> tbl_Inv_OrdinaryRequisitionDetails { get; set; }
        public DbSet<tbl_Inv_OrdinaryRequisitionDispensing> tbl_Inv_OrdinaryRequisitionDispensings { get; set; }
        public DbSet<tbl_Inv_BMRAdditionalDispensing> tbl_Inv_BMRAdditionalDispensings { get; set; }
        public DbSet<tbl_Inv_BMRDispensingRaw> tbl_Inv_BMRDispensingRaws { get; set; }
        public DbSet<tbl_Inv_BMRDispensingPackaging> tbl_Inv_BMRDispensingPackagings { get; set; }
        public DbSet<tbl_Inv_PDRequestDispensing> tbl_Inv_PDRequestDispensings { get; set; }
        public DbSet<tbl_Inv_StockTransferMaster> tbl_Inv_StockTransferMasters { get; set; }
        public DbSet<tbl_Inv_StockTransferDetail> tbl_Inv_StockTransferDetails { get; set; }

        /// <summary>
        /// Supply Chain
        /// </summary>
        public DbSet<tbl_Inv_InternationalCommercialTerm> tbl_Inv_InternationalCommercialTerms { get; set; }
        public DbSet<tbl_Inv_TransportType> tbl_Inv_TransportTypes { get; set; }
        public DbSet<tbl_Inv_OrderNoteMaster> tbl_Inv_OrderNoteMasters { get; set; }
        public DbSet<tbl_Inv_OrderNoteDetail> tbl_Inv_OrderNoteDetails { get; set; }
        public DbSet<tbl_Inv_OrderNoteDetail_ProductionOrder> tbl_Inv_OrderNoteDetail_ProductionOrders { get; set; }
        public DbSet<tbl_Inv_OrderNoteDetail_SubDistributor> tbl_Inv_OrderNoteDetail_SubDistributors { get; set; }

        public DbSet<tbl_Inv_PurchaseOrderTermsConditions> tbl_Inv_PurchaseOrderTermsConditionss { get; set; }
        public DbSet<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }
        public DbSet<tbl_Inv_PurchaseOrderDetail> tbl_Inv_PurchaseOrderDetails { get; set; }        
        public DbSet<tbl_Inv_PurchaseOrder_ImportTerms> tbl_Inv_PurchaseOrder_ImportTermss { get; set; }
        public DbSet<tbl_Inv_PurchaseOrder_Manufacturer> tbl_Inv_PurchaseOrder_Manufacturers { get; set; }
        public DbSet<tbl_Inv_PurchaseOrder_Supplier> tbl_Inv_PurchaseOrder_Suppliers { get; set; }
        public DbSet<tbl_Inv_PurchaseOrder_Indenter> tbl_Inv_PurchaseOrder_Indenters { get; set; }

        public DbSet<tbl_Inv_PurchaseRequestMaster> tbl_Inv_PurchaseRequestMasters { get; set; }
        public DbSet<tbl_Inv_PurchaseRequestDetail> tbl_Inv_PurchaseRequestDetails { get; set; }
        public DbSet<tbl_Inv_PurchaseRequestDetail_Bids> tbl_Inv_PurchaseRequestDetail_Bidss { get; set; }

        #endregion

        #region Qc
        public DbSet<tbl_Qc_ActionType> tbl_Qc_ActionTypes { get; set; }
        public DbSet<tbl_Qc_SampleProcessBMR> tbl_Qc_SampleProcessBMRs { get; set; }
        public DbSet<tbl_Qc_SampleProcessBPR> tbl_Qc_SampleProcessBPRs { get; set; }
        public DbSet<tbl_Qc_SampleProcessBMR_QcTest> tbl_Qc_SampleProcessBMR_QcTests { get; set; }
        public DbSet<tbl_Qc_SampleProcessBPR_QcTest> tbl_Qc_SampleProcessBPR_QcTests { get; set; }
        public DbSet<tbl_Qc_PurchaseNoteDetail_QcTest> tbl_Qc_PurchaseNoteDetail_QcTests { get; set; }

        public DbSet<tbl_Qc_Lab> tbl_Qc_Labs { get; set; }
        public DbSet<tbl_Qc_Test> tbl_Qc_Tests { get; set; }

        #endregion

        #region QA
        public DbSet<tbl_Qa_DocumentControl> tbl_Qa_DocumentControls { get; set; }
        #endregion

        #region Production

        public DbSet<tbl_Pro_CompositionFilterPolicyMaster> tbl_Pro_CompositionFilterPolicyMasters { get; set; }
        public DbSet<tbl_Pro_CompositionFilterPolicyDetail> tbl_Pro_CompositionFilterPolicyDetails { get; set; }
        public DbSet<tbl_Pro_Procedure> tbl_Pro_Procedures { get; set; }

        //----------------------Composition---------------------------//
        public DbSet<tbl_Pro_CompositionMaster> tbl_Pro_CompositionMasters { get; set; }
        public DbSet<tbl_Pro_CompositionDetail_RawMaster> tbl_Pro_CompositionDetail_RawMasters { get; set; }
        public DbSet<tbl_Pro_CompositionDetail_RawDetail_Items> tbl_Pro_CompositionDetail_RawDetail_Itemss { get; set; }
        public DbSet<tbl_Pro_CompositionDetail_Coupling> tbl_Pro_CompositionDetail_Couplings { get; set; }
        public DbSet<tbl_Pro_CompositionDetail_Coupling_PackagingMaster> tbl_Pro_CompositionDetail_Coupling_PackagingMasters { get; set; }
        public DbSet<tbl_Pro_CompositionDetail_Coupling_PackagingDetail> tbl_Pro_CompositionDetail_Coupling_PackagingDetails { get; set; }
        public DbSet<tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items> tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss { get; set; }
        public DbSet<tbl_Pro_CompositionMaster_ProcessBMR> tbl_Pro_CompositionMaster_ProcessBMRs { get; set; }
        public DbSet<tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR> tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs { get; set; }
        public DbSet<tbl_Pro_CompositionMaster_ProcessBMR_QcTest> tbl_Pro_CompositionMaster_ProcessBMR_QcTests { get; set; }
        public DbSet<tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest> tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests { get; set; }

        //----------------------BMR---------------------------//
        public DbSet<tbl_Pro_BatchMaterialRequisitionMaster> tbl_Pro_BatchMaterialRequisitionMasters { get; set; }
        public DbSet<tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR> tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs { get; set; }
        public DbSet<tbl_Pro_BatchMaterialRequisitionDetail_RawMaster> tbl_Pro_BatchMaterialRequisitionDetail_RawMasters { get; set; }
        public DbSet<tbl_Pro_BatchMaterialRequisitionDetail_RawDetail> tbl_Pro_BatchMaterialRequisitionDetail_RawDetails { get; set; }
        public DbSet<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters { get; set; }
        public DbSet<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs { get; set; }
        public DbSet<tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail> tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails { get; set; }
        public DbSet<tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items> tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss { get; set; }

        //----------------------BMR Additional---------------------------//
        public DbSet<tbl_Pro_BMRAdditionalType> tbl_Pro_BMRAdditionalTypes { get; set; }
        public DbSet<tbl_Pro_BMRAdditionalMaster> tbl_Pro_BMRAdditionalMasters { get; set; }
        public DbSet<tbl_Pro_BMRAdditionalDetail> tbl_Pro_BMRAdditionalDetails { get; set; }

        //----------------------Production Transfer---------------------------//
        public DbSet<tbl_Pro_ProductionTransferMaster> tbl_Pro_ProductionTransferMasters { get; set; }
        public DbSet<tbl_Pro_ProductionTransferDetail> tbl_Pro_ProductionTransferDetails { get; set; }

        #endregion

        #region Product Development
        public DbSet<tbl_PD_RequestMaster> tbl_PD_RequestMasters { get; set; }
        public DbSet<tbl_PD_RequestDetailTR> tbl_PD_RequestDetailTRs { get; set; }
        public DbSet<tbl_PD_RequestDetailTR_Procedure> tbl_PD_RequestDetailTR_Procedures { get; set; }
        public DbSet<tbl_PD_RequestDetailTR_CFP> tbl_PD_RequestDetailTR_CFPs { get; set; }
        public DbSet<tbl_PD_RequestDetailTR_CFP_Item> tbl_PD_RequestDetailTR_CFP_Items { get; set; }

        #endregion
    }


}
