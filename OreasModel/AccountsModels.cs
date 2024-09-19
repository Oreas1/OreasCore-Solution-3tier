using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace OreasModel
{
    [Table("tbl_Ac_CurrencyAndCountry")]
    public class tbl_Ac_CurrencyAndCountry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        [Required]
        [MaxLength(3)]
        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Currency Symbol")]
        public string CurrencySymbol { get; set; }

        [Required]
        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Ac_CurrencyAndCountry_Currency))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters_Currency { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Ac_CurrencyAndCountry_CountryOfOrigin))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters_CountryOfOrigin { get; set; }
    }

    [Table("tbl_Ac_FiscalYear")]
    public class tbl_Ac_FiscalYear
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Period Start")]
        public DateTime PeriodStart { get; set; }

        [Required]
        [Display(Name = "Period End")]
        public DateTime PeriodEnd { get; set; }

        [Required]
        [Display(Name = "Closed")]
        public bool IsClosed { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_FiscalYear_ClosingMaster.tbl_Ac_FiscalYear))]
        public virtual ICollection<tbl_Ac_FiscalYear_ClosingMaster> tbl_Ac_FiscalYear_ClosingMasters { get; set; }

        [InverseProperty(nameof(tbl_Ac_PaymentPlanningMaster.tbl_Ac_FiscalYear))]
        public virtual ICollection<tbl_Ac_PaymentPlanningMaster> tbl_Ac_PaymentPlanningMasters { get; set; }

    }

    [Table("tbl_Ac_FiscalYear_ClosingEntryType")]
    public class tbl_Ac_FiscalYear_ClosingEntryType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Entry Name")]
        public string EntryName { get; set; }

        [InverseProperty(nameof(tbl_Ac_FiscalYear_ClosingMaster.tbl_Ac_FiscalYear_ClosingEntryType))]
        public virtual ICollection<tbl_Ac_FiscalYear_ClosingMaster> tbl_Ac_FiscalYear_ClosingMasters { get; set; }

    }

    [Table("tbl_Ac_FiscalYear_ClosingMaster")]
    public class tbl_Ac_FiscalYear_ClosingMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_FiscalYear))]
        public int FK_tbl_Ac_FiscalYear_ID { get; set; }
        public virtual tbl_Ac_FiscalYear tbl_Ac_FiscalYear { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_FiscalYear_ClosingEntryType))]
        public int FK_tbl_Ac_FiscalYear_ClosingEntryType_ID { get; set; }
        public virtual tbl_Ac_FiscalYear_ClosingEntryType tbl_Ac_FiscalYear_ClosingEntryType { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        public double TotalDebit { get; set; }

        [Required]
        public double TotalCredit { get; set; }


        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_FiscalYear_ClosingDetail.tbl_Ac_FiscalYear_ClosingMaster))]
        public virtual ICollection<tbl_Ac_FiscalYear_ClosingDetail> tbl_Ac_FiscalYear_ClosingDetails { get; set; }

    }

    [Table("tbl_Ac_FiscalYear_ClosingDetail")]
    public class tbl_Ac_FiscalYear_ClosingDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_FiscalYear_ClosingMaster))]
        public int FK_tbl_Ac_FiscalYear_ClosingMaster_ID { get; set; }
        public virtual tbl_Ac_FiscalYear_ClosingMaster tbl_Ac_FiscalYear_ClosingMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        public double Debit { get; set; }

        [Required]
        public double Credit { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Ac_FiscalYear_ClosingDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

    }

    [Table("tbl_Ac_ChartOfAccounts_Type")]
    public class tbl_Ac_ChartOfAccounts_Type
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_ChartOfAccounts.tbl_Ac_ChartOfAccounts_Type))]
        public virtual ICollection<tbl_Ac_ChartOfAccounts> tbl_Ac_ChartOfAccountss { get; set; }

    }

    [Table("tbl_Ac_ChartOfAccounts")]
    public class tbl_Ac_ChartOfAccounts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_Parent))]
        public int? ParentID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_Parent { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_Type))]

        public int FK_tbl_Ac_ChartOfAccounts_Type_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts_Type tbl_Ac_ChartOfAccounts_Type { get; set; }


        [MaxLength(50)]
        public string AccountCode { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Required]
        public bool IsTransactional { get; set; }

        [Required]
        public bool IsDiscontinue { get; set; }

        [MaxLength(50)]
        public string CompanyName { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string NTN { get; set; }

        [MaxLength(50)]
        public string STR { get; set; }

        [MaxLength(50)]
        public string Telephone { get; set; }

        [MaxLength(50)]
        public string Mobile { get; set; }

        [MaxLength(50)]
        public string Fax { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string ContactPersonName { get; set; }

        [MaxLength(50)]
        public string ContactPersonNumber { get; set; }
        
        [ForeignKey(nameof(tbl_Ac_PolicyWHTaxOnPurchase))]
        public int? FK_tbl_Ac_PolicyWHTaxOnPurchase_ID { get; set; }
        public virtual tbl_Ac_PolicyWHTaxOnPurchase tbl_Ac_PolicyWHTaxOnPurchase { get; set; }

        [ForeignKey(nameof(tbl_Ac_PolicyWHTaxOnSales))]
        public int? FK_tbl_Ac_PolicyWHTaxOnSales_ID { get; set; }
        public virtual tbl_Ac_PolicyWHTaxOnSales tbl_Ac_PolicyWHTaxOnSales { get; set; }

        [ForeignKey(nameof(tbl_Ac_PolicyPaymentTerm))]
        public int? FK_tbl_Ac_PolicyPaymentTerm_ID { get; set; }
        public virtual tbl_Ac_PolicyPaymentTerm tbl_Ac_PolicyPaymentTerm { get; set; }

        
        public bool? Supplier_Approved { get; set; }
        public DateTime? Supplier_EvaluatedOn { get; set; }
        public double? Supplier_EvaluationScore { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------self relation---------------------//
        [InverseProperty(nameof(tbl_Ac_ChartOfAccounts.tbl_Ac_ChartOfAccounts_Parent))]
        public virtual ICollection<tbl_Ac_ChartOfAccounts> tbl_Ac_ChartOfAccounts_Parents { get; set; }

        //-----------------------Closing Fiscal Year----------------//
        [InverseProperty(nameof(tbl_Ac_FiscalYear_ClosingMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_FiscalYear_ClosingMaster> tbl_Ac_FiscalYear_ClosingMasters { get; set; }

        [InverseProperty(nameof(tbl_Ac_FiscalYear_ClosingDetail.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_FiscalYear_ClosingDetail> tbl_Ac_FiscalYear_ClosingDetails { get; set; }

        //-------------------Ac Ledger---------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Ac_ChartOfAccounts_For))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers_For { get; set; }

        //------voucher----//
        [InverseProperty(nameof(tbl_Ac_V_BankDocumentMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_V_BankDocumentMaster> tbl_Ac_V_BankDocumentMasters { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_BankDocumentDetail.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_V_BankDocumentDetail> tbl_Ac_V_BankDocumentDetails { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_BankDocumentDetail.tbl_Ac_ChartOfAccounts_For))]
        public virtual ICollection<tbl_Ac_V_BankDocumentDetail> tbl_Ac_V_BankDocumentDetails_For { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_CashDocumentMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_V_CashDocumentMaster> tbl_Ac_V_CashDocumentMasters { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_CashDocumentDetail.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_V_CashDocumentDetail> tbl_Ac_V_CashDocumentDetails { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_CashDocumentDetail.tbl_Ac_ChartOfAccounts_For))]
        public virtual ICollection<tbl_Ac_V_CashDocumentDetail> tbl_Ac_V_CashDocumentDetails_For { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_JournalDocumentMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_V_JournalDocumentMaster> tbl_Ac_V_JournalDocumentMasters { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_JournalDocumentDetail.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_V_JournalDocumentDetail> tbl_Ac_V_JournalDocumentDetails { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_JournalDocumentDetail.tbl_Ac_ChartOfAccounts_For))]
        public virtual ICollection<tbl_Ac_V_JournalDocumentDetail> tbl_Ac_V_JournalDocumentDetails_For { get; set; }

        //--------------------------Inventory Challan------------------------------//
        [InverseProperty(nameof(tbl_Inv_PurchaseNoteMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Inv_PurchaseNoteMaster> tbl_Inv_PurchaseNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseReturnNoteMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Inv_PurchaseReturnNoteMaster> tbl_Inv_PurchaseReturnNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesNoteMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Inv_SalesNoteMaster> tbl_Inv_SalesNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesNoteMaster.tbl_Ac_ChartOfAccounts_Transporter))]
        public virtual ICollection<tbl_Inv_SalesNoteMaster> tbl_Inv_SalesNoteMasters_Transporter { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteMaster> tbl_Inv_SalesReturnNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteMaster.tbl_Ac_ChartOfAccounts_Transporter))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteMaster> tbl_Inv_SalesReturnNoteMasters_Transporter { get; set; }

        //----------------------------supply chain-----------------------------------------------//
        [InverseProperty(nameof(tbl_Inv_OrderNoteMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Inv_OrderNoteMaster> tbl_Inv_OrderNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseRequestDetail_Bids.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Inv_PurchaseRequestDetail_Bids> tbl_Inv_PurchaseRequestDetail_Bidss { get; set; }

        //----------------------------account Policy-----------------------------------------------//
        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_Inv))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_Invs { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_COGS))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_COGSs { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_Expense))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_Expense { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_InProcess))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_InProcess { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_WHT_Purchase))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_WHT_Purchase { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_GST_Purchase))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_ChartOfAccounts_GST_Purchases { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_WHT_Sales))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_WHT_Sales { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_ST_Sales))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_ST_Sales { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_FST_Sales))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_FST_Sales { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_Sales))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_Sales { get; set; }

        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Ac_ChartOfAccounts_ExpenseTR))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys_ExpenseTR { get; set; }

        //----------------------------------------------/

        [InverseProperty(nameof(tbl_Ac_CustomerApprovedRateList.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_CustomerApprovedRateList> tbl_Ac_CustomerApprovedRateLists { get; set; }

        [InverseProperty(nameof(tbl_Ac_CustomerSubDistributorList.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_CustomerSubDistributorList> tbl_Ac_CustomerSubDistributorLists { get; set; }

        [InverseProperty(nameof(tbl_Ac_PaymentPlanningDetail.tbl_Ac_ChartOfAccounts))]
        public virtual ICollection<tbl_Ac_PaymentPlanningDetail> tbl_Ac_PaymentPlanningDetails { get; set; }

    }

    [Table("tbl_Ac_CustomerApprovedRateList")]
    public class tbl_Ac_CustomerApprovedRateList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        public DateTime AppliedDate { get; set; }

        [Required]
        public double PreviousRate { get; set; }

        [Required]
        public DateTime PreviousAppliedDate { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_Ac_CustomerSubDistributorList")]
    public class tbl_Ac_CustomerSubDistributorList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }


        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string ContactNo { get; set; }

        [MaxLength(50)]
        public string ContactPerson { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrderNoteDetail_SubDistributor.tbl_Ac_CustomerSubDistributorList))]
        public virtual ICollection<tbl_Inv_OrderNoteDetail_SubDistributor> tbl_Inv_OrderNoteDetail_SubDistributors { get; set; }


        [InverseProperty(nameof(tbl_Inv_SalesNoteMaster.tbl_Ac_CustomerSubDistributorList))]
        public virtual ICollection<tbl_Inv_SalesNoteMaster> tbl_Inv_SalesNoteMasters { get; set; }


        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteMaster.tbl_Ac_CustomerSubDistributorList))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteMaster> tbl_Inv_SalesReturnNoteMasters { get; set; }

    }

    [Table("tbl_Ac_CompositionCostingFactors")]
    public class tbl_Ac_CompositionCostingFactors
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Formula Name")]
        public string FormulaName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Formula Expression")]
        public string FormulaExpression { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

    [Table("tbl_Ac_CostingIndirectExpenseList")]
    public class tbl_Ac_CostingIndirectExpenseList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Indirect Expense Name")]
        public string IndirectExpenseName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_CompositionCostingIndirectExpense.tbl_Ac_CostingIndirectExpenseList))]
        public virtual ICollection<tbl_Ac_CompositionCostingIndirectExpense> tbl_Ac_CompositionCostingIndirectExpenses { get; set; }

    }

    [Table("tbl_Ac_CompositionCostingIndirectExpense")]
    public class tbl_Ac_CompositionCostingIndirectExpense
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster))]
        public int FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_Coupling_PackagingMaster tbl_Pro_CompositionDetail_Coupling_PackagingMaster { get; set; }

        [ForeignKey(nameof(tbl_Ac_CostingIndirectExpenseList))]
        public int? FK_tbl_Ac_CostingIndirectExpenseList_ID { get; set; }
        public virtual tbl_Ac_CostingIndirectExpenseList tbl_Ac_CostingIndirectExpenseList { get; set; }

        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int? FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        [Display(Name = "Custom Rate")]
        public double CustomRate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

    [Table("tbl_Ac_Ledger")]
    public class tbl_Ac_Ledger
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_For))]
        public int? FK_tbl_Ac_ChartOfAccounts_ID_For { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_For { get; set; }

        [Required]
        [Display(Name = "Posting Date")]
        public DateTime PostingDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Debit { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Credit { get; set; }

        [Required]
        public bool Posted { get; set; }

        [MaxLength(100)]
        public string Narration { get; set; }

        public int? PostingNo { get; set; }

        public string ExeCode { get; set; }

        [MaxLength(15)]
        public string TrackingNo { get; set; }

        //------------------------challan---------------------------//

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail { get; set; }

        public int? FK_tbl_Inv_PurchaseNoteMaster_ID { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseReturnNoteDetail))]
        public int? FK_tbl_Inv_PurchaseReturnNoteDetail_ID { get; set; }
        public virtual tbl_Inv_PurchaseReturnNoteDetail tbl_Inv_PurchaseReturnNoteDetail { get; set; }

        public int? FK_tbl_Inv_PurchaseReturnNoteMaster_ID { get; set; }

        [ForeignKey(nameof(tbl_Inv_SalesNoteDetail))]
        public int? FK_tbl_Inv_SalesNoteDetail_ID { get; set; }
        public virtual tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail { get; set; }

        public int? FK_tbl_Inv_SalesNoteMaster_ID { get; set; }

        [ForeignKey(nameof(tbl_Inv_SalesReturnNoteDetail))]
        public int? FK_tbl_Inv_SalesReturnNoteDetail_ID { get; set; }
        public virtual tbl_Inv_SalesReturnNoteDetail tbl_Inv_SalesReturnNoteDetail { get; set; }

        public int? FK_tbl_Inv_SalesReturnNoteMaster_ID { get; set; }

        //---------------------vouchers---------------------------//
        [ForeignKey(nameof(tbl_Ac_V_BankDocumentDetail))]
        public int? FK_tbl_Ac_V_BankDocumentDetail_ID { get; set; }
        public virtual tbl_Ac_V_BankDocumentDetail tbl_Ac_V_BankDocumentDetail { get; set; }

        [ForeignKey(nameof(tbl_Ac_V_CashDocumentDetail))]
        public int? FK_tbl_Ac_V_CashDocumentDetail_ID { get; set; }
        public virtual tbl_Ac_V_CashDocumentDetail tbl_Ac_V_CashDocumentDetail { get; set; }

        [ForeignKey(nameof(tbl_Ac_V_JournalDocumentDetail))]
        public int? FK_tbl_Ac_V_JournalDocumentDetail_ID { get; set; }
        public virtual tbl_Ac_V_JournalDocumentDetail tbl_Ac_V_JournalDocumentDetail { get; set; }

        [ForeignKey(nameof(tbl_Ac_V_JournalDocument2Detail))]
        public int? FK_tbl_Ac_V_JournalDocument2Detail_ID { get; set; }
        public virtual tbl_Ac_V_JournalDocument2Detail tbl_Ac_V_JournalDocument2Detail { get; set; }

        //---------------------Closing Fiscal Year---------------------------//
        [ForeignKey(nameof(tbl_Ac_FiscalYear_ClosingDetail))]
        public int? FK_tbl_Ac_FiscalYear_ClosingDetail_ID { get; set; }
        public virtual tbl_Ac_FiscalYear_ClosingDetail tbl_Ac_FiscalYear_ClosingDetail { get; set; }

        //---------------------Production---------------------------//
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionMaster))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster { get; set; }

        [ForeignKey(nameof(tbl_Inv_BMRDispensingRaw))]
        public int? FK_tbl_Inv_BMRDispensingRaw_ID { get; set; }
        public virtual tbl_Inv_BMRDispensingRaw tbl_Inv_BMRDispensingRaw { get; set; }

        [ForeignKey(nameof(tbl_Inv_BMRDispensingPackaging))]
        public int? FK_tbl_Inv_BMRDispensingPackaging_ID { get; set; }
        public virtual tbl_Inv_BMRDispensingPackaging tbl_Inv_BMRDispensingPackaging { get; set; }

        [ForeignKey(nameof(tbl_Inv_BMRAdditionalDispensing))]
        public int? FK_tbl_Inv_BMRAdditionalDispensing_ID { get; set; }
        public virtual tbl_Inv_BMRAdditionalDispensing tbl_Inv_BMRAdditionalDispensing { get; set; }

        [ForeignKey(nameof(tbl_Inv_OrdinaryRequisitionDispensing))]
        public int? FK_tbl_Inv_OrdinaryRequisitionDispensing_ID { get; set; }
        public virtual tbl_Inv_OrdinaryRequisitionDispensing tbl_Inv_OrdinaryRequisitionDispensing { get; set; }

        [ForeignKey(nameof(tbl_Pro_ProductionTransferDetail))]
        public int? FK_tbl_Pro_ProductionTransferDetail_ID { get; set; }
        public virtual tbl_Pro_ProductionTransferDetail tbl_Pro_ProductionTransferDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_StockTransferDetail))]
        public int? FK_tbl_Inv_StockTransferDetail_ID { get; set; }
        public virtual tbl_Inv_StockTransferDetail tbl_Inv_StockTransferDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_PDRequestDispensing))]
        public int? FK_tbl_Inv_PDRequestDispensing_ID { get; set; }
        public virtual tbl_Inv_PDRequestDispensing tbl_Inv_PDRequestDispensing { get; set; }

    }

    [Table("tbl_Ac_PaymentPlanningMaster")]
    public class tbl_Ac_PaymentPlanningMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_FiscalYear))]
        public int FK_tbl_Ac_FiscalYear_ID { get; set; }
        public virtual tbl_Ac_FiscalYear tbl_Ac_FiscalYear { get; set; }

        [Required]
        [Display(Name = "Month No")]
        public int MonthNo { get; set; }

        [Display(Name = "Month Start")]
        public DateTime? MonthStart { get; set; }

        [Display(Name = "Month End")]
        public DateTime? MonthEnd { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_PaymentPlanningDetail.tbl_Ac_PaymentPlanningMaster))]
        public virtual ICollection<tbl_Ac_PaymentPlanningDetail> tbl_Ac_PaymentPlanningDetails { get; set; }
    }

    [Table("tbl_Ac_PaymentPlanningDetail")]
    public class tbl_Ac_PaymentPlanningDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_PaymentPlanningMaster))]
        public int FK_tbl_Ac_PaymentPlanningMaster_ID { get; set; }
        public virtual tbl_Ac_PaymentPlanningMaster tbl_Ac_PaymentPlanningMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public bool Restricted { get; set; }

        [MaxLength(100)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_Ac_PolicyPaymentTerm")]
    public class tbl_Ac_PolicyPaymentTerm
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int DaysLimit { get; set; }

        [Required]
        public int AdvancePercentage { get; set; }


        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_ChartOfAccounts.tbl_Ac_PolicyPaymentTerm))]
        public virtual ICollection<tbl_Ac_ChartOfAccounts> tbl_Ac_ChartOfAccountss { get; set; }
    }

    [Table("tbl_Ac_PolicyInventory")]
    public class tbl_Ac_PolicyInventory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductType))]
        public int FK_tbl_Inv_ProductType_ID { get; set; }
        public virtual tbl_Inv_ProductType tbl_Inv_ProductType { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_Inv))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_Inv { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_Inv { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_COGS))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_COGS { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_COGS { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_Expense))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_Expense { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_Expense { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_InProcess))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_InProcess { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_InProcess { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_WHT_Purchase))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_WHT_Purchase { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_WHT_Purchase { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_GST_Purchase))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_GST_Purchase { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_GST_Purchase { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_WHT_Sales))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_WHT_Sales { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_WHT_Sales { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_ST_Sales))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_ST_Sales { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_ST_Sales { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_FST_Sales))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_FST_Sales { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_FST_Sales { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_Sales))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_Sales { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_Sales { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_ExpenseTR))]
        public int FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTR { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_ExpenseTR { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_Ac_PolicyWHTaxOnPurchase")]
    public class tbl_Ac_PolicyWHTaxOnPurchase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "WHTax Name")]
        public string WHTaxName { get; set; }

        [Required]
        [Display(Name = "WHTax %")]
        public double WHTaxPer { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_ChartOfAccounts.tbl_Ac_PolicyWHTaxOnPurchase))]
        public virtual ICollection<tbl_Ac_ChartOfAccounts> tbl_Ac_ChartOfAccountss { get; set; }


    }

    [Table("tbl_Ac_PolicyWHTaxOnSales")]
    public class tbl_Ac_PolicyWHTaxOnSales
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "WHTax Name")]
        public string WHTaxName { get; set; }

        [Required]
        [Display(Name = "WHTax %")]
        public double WHTaxPer { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_ChartOfAccounts.tbl_Ac_PolicyWHTaxOnSales))]
        public virtual ICollection<tbl_Ac_ChartOfAccounts> tbl_Ac_ChartOfAccountss { get; set; }

    }

    //----------------------------------------vouchers-----------------------------------------------//

    //---Bank
    [Table("tbl_Ac_V_BankTransactionMode")]
    public class tbl_Ac_V_BankTransactionMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string BankTransactionMode { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_BankDocumentDetail.tbl_Ac_V_BankTransactionMode))]
        public virtual ICollection<tbl_Ac_V_BankDocumentDetail> tbl_Ac_V_BankDocumentDetails { get; set; }
    }

    [Table("tbl_Ac_V_BankDocumentMaster")]
    public class tbl_Ac_V_BankDocumentMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Voucher Date")]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Voucher No")]
        public int? VoucherNo { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        [Display(Name = "Debit/Credit")]
        public bool Debit1_Credit0 { get; set; }

        [Required]
        public bool IsSupervisedAll { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


        [InverseProperty(nameof(tbl_Ac_V_BankDocumentDetail.tbl_Ac_V_BankDocumentMaster))]
        public virtual ICollection<tbl_Ac_V_BankDocumentDetail> tbl_Ac_V_BankDocumentDetails { get; set; }
    }

    [Table("tbl_Ac_V_BankDocumentDetail")]
    public class tbl_Ac_V_BankDocumentDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_V_BankDocumentMaster))]
        public int FK_tbl_Ac_V_BankDocumentMaster_ID { get; set; }
        public virtual tbl_Ac_V_BankDocumentMaster tbl_Ac_V_BankDocumentMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_V_BankTransactionMode))]
        public int FK_tbl_Ac_V_BankTransactionMode_ID { get; set; }
        public virtual tbl_Ac_V_BankTransactionMode tbl_Ac_V_BankTransactionMode { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        public DateTime PostingDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string InstrumentNo { get; set; }

        [Required]
        //[DataType(DataType.DateTime)]
        public DateTime InstrumentDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Narration { get; set; }

        [Required]
        public double Amount { get; set; }

        public bool? Cleared1_Cancelled0 { get; set; }

        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_For))]
        public int? FK_tbl_Ac_ChartOfAccounts_ID_For { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_For { get; set; }

        [Required]
        public bool IsSupervised { get; set; }

        [MaxLength(50)]
        public string SupervisedBy { get; set; }

        public DateTime? SupervisedDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Ac_V_BankDocumentDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

    }

    //---Cash
    [Table("tbl_Ac_V_CashTransactionMode")]
    public class tbl_Ac_V_CashTransactionMode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string CashTransactionMode { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_CashDocumentDetail.tbl_Ac_V_CashTransactionMode))]
        public virtual ICollection<tbl_Ac_V_CashDocumentDetail> tbl_Ac_V_CashDocumentDetails { get; set; }
    }

    [Table("tbl_Ac_V_CashDocumentMaster")]
    public class tbl_Ac_V_CashDocumentMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Voucher Date")]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Voucher No")]
        public int? VoucherNo { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]

        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        [Display(Name = "Debit/Credit")]
        public bool Debit1_Credit0 { get; set; }

        [Required]
        public bool IsSupervisedAll { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


        [InverseProperty(nameof(tbl_Ac_V_CashDocumentDetail.tbl_Ac_V_CashDocumentMaster))]
        public virtual ICollection<tbl_Ac_V_CashDocumentDetail> tbl_Ac_V_CashDocumentDetails { get; set; }
    }

    [Table("tbl_Ac_V_CashDocumentDetail")]
    public class tbl_Ac_V_CashDocumentDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_V_CashDocumentMaster))]

        public int FK_tbl_Ac_V_CashDocumentMaster_ID { get; set; }
        public virtual tbl_Ac_V_CashDocumentMaster tbl_Ac_V_CashDocumentMaster { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_Ac_V_CashTransactionMode))]

        public int FK_tbl_Ac_V_CashTransactionMode_ID { get; set; }
        public virtual tbl_Ac_V_CashTransactionMode tbl_Ac_V_CashTransactionMode { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]

        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        public DateTime PostingDate { get; set; }

        [MaxLength(50)]
        public string InstrumentNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Narration { get; set; }

        [Required]
        public double Amount { get; set; }

        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_For))]
        public int? FK_tbl_Ac_ChartOfAccounts_ID_For { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_For { get; set; }

        [Required]
        public bool IsSupervised { get; set; }

        [MaxLength(50)]
        public string SupervisedBy { get; set; }

        public DateTime? SupervisedDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Ac_V_CashDocumentDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }
    }

    //---Journal
    [Table("tbl_Ac_V_JournalDocumentMaster")]
    public class tbl_Ac_V_JournalDocumentMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Voucher Date")]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Voucher No")]
        public int? VoucherNo { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]

        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        [Display(Name = "Debit/Credit")]
        public bool Debit1_Credit0 { get; set; }

        [Required]
        public bool IsSupervisedAll { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_JournalDocumentDetail.tbl_Ac_V_JournalDocumentMaster))]
        public virtual ICollection<tbl_Ac_V_JournalDocumentDetail> tbl_Ac_V_JournalDocumentDetails { get; set; }
    }

    [Table("tbl_Ac_V_JournalDocumentDetail")]
    public class tbl_Ac_V_JournalDocumentDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_V_JournalDocumentMaster))]

        public int FK_tbl_Ac_V_JournalDocumentMaster_ID { get; set; }
        public virtual tbl_Ac_V_JournalDocumentMaster tbl_Ac_V_JournalDocumentMaster { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]

        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PostingDate { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        [MaxLength(100)]
        public string Narration { get; set; }

        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_For))]
        public int? FK_tbl_Ac_ChartOfAccounts_ID_For { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_For { get; set; }

        [Required]
        public bool IsSupervised { get; set; }

        [MaxLength(50)]
        public string SupervisedBy { get; set; }

        public DateTime? SupervisedDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Ac_V_JournalDocumentDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }
    }

    //---Journal2
    [Table("tbl_Ac_V_JournalDocument2Master")]
    public class tbl_Ac_V_JournalDocument2Master
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Voucher Date")]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Voucher No")]
        public int? VoucherNo { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        public double TotalDebit { get; set; }

        [Required]
        public double TotalCredit { get; set; }

        [Required]
        public bool IsPosted { get; set; }

        [Required]
        public bool IsSupervisedAll { get; set; }

        [MaxLength(50)]
        public string SupervisedBy { get; set; }

        public DateTime? SupervisedDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Ac_V_JournalDocument2Detail.tbl_Ac_V_JournalDocument2Master))]
        public virtual ICollection<tbl_Ac_V_JournalDocument2Detail> tbl_Ac_V_JournalDocument2Details { get; set; }
    }

    [Table("tbl_Ac_V_JournalDocument2Detail")]
    public class tbl_Ac_V_JournalDocument2Detail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_V_JournalDocument2Master))]

        public int FK_tbl_Ac_V_JournalDocument2Master_ID { get; set; }
        public virtual tbl_Ac_V_JournalDocument2Master tbl_Ac_V_JournalDocument2Master { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]

        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        public double Debit { get; set; }

        [Required]
        public double Credit { get; set; }

        [Required]
        [MaxLength(100)]
        public string Narration { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Ac_V_JournalDocument2Detail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }
    }

}
