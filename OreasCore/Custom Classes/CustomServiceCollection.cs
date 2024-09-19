using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using OreasCore.Custom_Classes;
using OreasServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OreasCore
{
    public static class CustomServiceCollection
    {
        public static void AddCustomServices(this IServiceCollection services)
        {       
            //------------------Identity-----------------------------------//
            services.AddScoped<IIdentityList, IdentityListRepository>();
            services.AddScoped<IUser, UserRepository>();
            services.AddScoped<IArea, AreaRepository>();
            services.AddScoped<IAuthorizationScheme, AuthorizationSchemeRepository>();
            services.AddScoped<IAspNetOreasCompanyProfile, AspNetOreasCompanyProfileRepository>();
            services.AddScoped<IAspNetOreasGeneralSettings, AspNetOreasGeneralSettingsRepository>();
            services.AddScoped<IDashBoard, DashBoardRepository>();
            services.AddScoped<IManagementDashBoard, ManagementDashBoardRepository>();

            //------------------WPT-----------------------------------//
            services.AddScoped<IWPTDashboard, WPTDashboardRepository>();
            services.AddScoped<IWPTList, WPTListRepository>();
            services.AddScoped<IHoliday, HolidayRepository>();
            services.AddScoped<IDepartment, DepartmentRepository>();
            services.AddScoped<IAllowanceType, AllowanceTypeRepository>();
            services.AddScoped<IDeductibleType, DeductibleTypeRepository>();
            services.AddScoped<IOTPolicy, OTPolicyRepository>();
            services.AddScoped<IBank, BankRepository>();
            services.AddScoped<ILoanType, LoanTypeRepository>();
            services.AddScoped<IEmploymentType, EmploymentTypeRepository>();
            services.AddScoped<IRewardType, RewardTypeRepository>();
            services.AddScoped<IHiringType, HiringTypeRepository>();
            services.AddScoped<IPolicyGeneral, PolicyGeneralRepository>();
            services.AddScoped<IPolicyPenalty, PolicyPenaltyRepository>();
            services.AddScoped<IInActiveType, InActiveTypeRepository>();
            services.AddScoped<IEmployee, EmployeeRepository>();
            services.AddScoped<IShift, ShiftRepository>();
            services.AddScoped<IDesignation, DesignationRepository>();
            services.AddScoped<IEmployeeLevel, EmployeeLevelRepository>();
            services.AddScoped<IEducationalLevelType, EducationalLevelTypeRepository>();
            services.AddScoped<IMachine, MachineRepository>();
            services.AddScoped<ITransactionMode, TransactionModeRepository>();
            services.AddScoped<IATTimeGrace, ATTimeGraceRepository>();
            services.AddScoped<IAttendance, AttendanceRepository>();
            services.AddScoped<IATBulkManual, ATBulkManualRepository>();
            services.AddScoped<ILeavePolicy, LeavePolicyRepository>();
            services.AddScoped<ICalendar, CalendarRepository>();
            services.AddScoped<ILeaveRequisition, LeaveRequisitionRepository>();
            services.AddScoped<IIncentivePolicy, IncentivePolicyRepository>();
            services.AddScoped<IReward, RewardRepository>();
            services.AddScoped<ILoan, LoanRepository>();
            services.AddScoped<IIncrement, IncrementRepository>();
            services.AddScoped<IPayRun, PayRunRepository>();

            //------------------Accounts-----------------------------------//
            services.AddScoped<IAccountsDashboard, AccountsDashboardRepository>();
            services.AddScoped<IAccountsList, AccountsListRepository>();
            services.AddScoped<ICurrencyAndCountry, CurrencyAndCountryRepository>();
            services.AddScoped<IFiscalYear, FiscalYearRepository>();
            services.AddScoped<IChartOfAccountsType, ChartOfAccountsTypeRepository>();
            services.AddScoped<IChartOfAccounts,ChartOfAccountsRepository>();
            services.AddScoped<ICompositionCostingFactors, CompositionCostingFactorsRepository>();
            services.AddScoped<ICostingIndirectExpenseList, CostingIndirectExpenseListRepository>();
            services.AddScoped<ICustomerApprovedRateList, CustomerApprovedRateListRepository>();
            services.AddScoped<IAcPolicyInventory, AcPolicyInventoryRepository>();
            services.AddScoped<IAcPolicyWHTaxOnPurchase, AcPolicyWHTaxOnPurchaseRepository>();
            services.AddScoped<IAcPolicyWHTaxOnSales, AcPolicyWHTaxOnSalesRepository>();
            services.AddScoped<IPaymentPlanning, PaymentPlanningRepository>();
            services.AddScoped<IAcLedger, AcLedgerRepository>();
            services.AddScoped<IPolicyPaymentTerm, PolicyPaymentTermRepository>();
            services.AddScoped<IBankDocument, BankDocumentRepository>();
            services.AddScoped<ICashDocument, CashDocumentRepository>();
            services.AddScoped<IJournalDocument, JournalDocumentRepository>();
            services.AddScoped<IJournalDocument2, JournalDocument2Repository>();
            services.AddScoped<IPurchaseNoteInvoice, PurchaseNoteInvoiceRepository>();
            services.AddScoped<IPurchaseReturnNoteInvoice, PurchaseReturnNoteInvoiceRepository>();
            services.AddScoped<ISalesNoteInvoice, SalesNoteInvoiceRepository>();
            services.AddScoped<ISalesReturnNoteInvoice, SalesReturnNoteInvoiceRepository>();
            services.AddScoped<ICompositionCosting, CompositionCostingRepository>();
            services.AddScoped<IBMRCosting, BMRCostingRepository>();

            //------------------Inventory-----------------------------------//
            services.AddScoped<IInventoryDashboard, InventoryDashboardRepository>();
            services.AddScoped<IInventoryList, InventoryListRepository>();
            services.AddScoped<IMeasurementUnit, MeasurementUnitRepository>();
            services.AddScoped<IProductClassification, ProductClassificationRepository>();
            services.AddScoped<IProductType, ProductTypeRepository>();
            services.AddScoped<IWareHouse, WareHouseRepository>();
            services.AddScoped<IProductRegistration, ProductRegistrationRepository>();
            services.AddScoped<IPurchaseNote, PurchaseNoteRepository>();
            services.AddScoped<IPurchaseReturnNote, PurchaseReturnNoteRepository>();
            services.AddScoped<ISalesNote, SalesNoteRepository>();
            services.AddScoped<ISalesReturnNote, SalesReturnNoteRepository>();
            services.AddScoped<IOrdinaryRequisition, OrdinaryRequisitionRepository>();
            services.AddScoped<IOrdinaryRequisitionDispensing, OrdinaryRequisitionDispensingRepository>();
            services.AddScoped<IBMRAdditionalDispensing, BMRAdditionalDispensingRepository>();
            services.AddScoped<IBMRDispensing, BMRDispensingRepository>();
            services.AddScoped<IInvProductionTransfer, InvProductionTransferRepository>();
            services.AddScoped<IInvLedger, InvLedgerRepository>();
            services.AddScoped<IPDRequestDispensing, PDRequestDispensingRepository>();
            services.AddScoped<IStockTransfer, StockTransferRepository>();

            //SupplyChain
            services.AddScoped<IInternationalCommercialTerm, InternationalCommercialTermRepository>();
            services.AddScoped<ITransportType, TransportTypeRepository>();
            services.AddScoped<ISupplier, SupplierRepository>();
            services.AddScoped<ICustomerSubDistributorList, CustomerSubDistributorListRepository>();
            services.AddScoped<IOrderNote, OrderNoteRepository>();
            services.AddScoped<IPurchaseOrder, PurchaseOrderRepository>();
            services.AddScoped<IPurchaseOrderTermsConditions, PurchaseOrderTermsConditionsRepository>();
            services.AddScoped<IPurchaseOrderImportTerms, PurchaseOrderImportTermsRepository>();
            services.AddScoped<IPOSupplier, POSupplierRepository>();
            services.AddScoped<IPOIndenter, POIndenterRepository>();
            services.AddScoped<IPOManufacturer, POManufacturerRepository>();
            services.AddScoped<IPurchaseRequest, PurchaseRequestRepository>();
            services.AddScoped<IPurchaseRequestBids, PurchaseRequestBidsRepository>();

            //------------------Qc-----------------------------------//
            services.AddScoped<IQcDashboard, QcDashboardRepository>();
            services.AddScoped<IQcList, QcListRepository>();
            services.AddScoped<IQcPurchaseNote, QcPurchaseNoteRepository>();
            services.AddScoped<IProductRegistrationQcTestForPN, ProductRegistrationQcTestForPNRepository>();
            services.AddScoped<IQCBatch, QCBatchRepository>();
            services.AddScoped<IQcLab, QcLabRepository>();
            services.AddScoped<IQcTest, QcTestRepository>();
            services.AddScoped<ICompositionQcTest, CompositionQcTestRepository>();

            //------------------Qa-----------------------------------//
            services.AddScoped<IQaDocumentControl, QaDocumentControlRepository>();
            services.AddScoped<IQAProcess, QAProcessRepository>();
            services.AddScoped<IQAProductionOrder, QAProductionOrder>();
            services.AddScoped<IQAProductionTransfer, QAProductionTransferRepository>();

            //------------------Production-----------------------------------//
            services.AddScoped<IProductionDashboard, ProductionDashboardRepository>();
            services.AddScoped<IProductionList, ProductionListRepository>();
            services.AddScoped<ICompositionFilterPolicy, CompositionFilterPolicyRepository>();
            services.AddScoped<IProProcedure, ProProcedureRepository>();
            services.AddScoped<IComposition, CompositionRepository>();
            services.AddScoped<IBMR, BMRRepository>();
            services.AddScoped<IBMRBPRProcess, BMRBPRProcessRepository>();
            services.AddScoped<IBMRAdditional, BMRAdditionalRepository>();
            services.AddScoped<IProductionTransfer, ProductionTransferRepository>();
            services.AddScoped<IOrderNoteProduction, OrderNoteProductionRepository>();

            //------------------Product Development-----------------------------------//
            services.AddScoped<IPDRequest, PDRequestRepository>();

        }
    }




}
