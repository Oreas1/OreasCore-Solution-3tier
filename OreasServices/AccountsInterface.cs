using OreasModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public interface IAccountsList
    {
        Task<object> GetCOAListAsync(string QueryName = "", string COAFilterBy = "", string COAFilterValue = "", int FormID = 0);
        Task<object> GetCOATypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetPolicyWHTaxOnPurchaseListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetPolicyWHTaxOnSalesListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetPolicyPaymentTermListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetBankTransactionModeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetCashTransactionModeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetClosingTypeListAsync(string FilterByText = null, string FilterValueByText = null);
    }
    public interface ICurrencyAndCountry
    {
        Task<object> GetCurrencyAndCountry(int id);
        object GetWCLCurrencyAndCountry();
        Task<PagedData<object>> LoadCurrencyAndCountry(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostCurrencyAndCountry(tbl_Ac_CurrencyAndCountry tbl_Ac_CurrencyAndCountry, string operation = "", string userName = "");
    }
    public interface IFiscalYear
    {
        Task<object> GetClosingDetail(int id);
        Task<object> GetClosingMaster(int id);
        Task<object> GetFiscalYear(int id);
        object GetWCLClosingDetail();
        object GetWCLClosingMaster();
        object GetWCLFiscalYear();
        Task<PagedData<object>> LoadClosingDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadClosingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadFiscalYear(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostClosingDetail(tbl_Ac_FiscalYear_ClosingDetail tbl_Ac_FiscalYear_ClosingDetail, string operation = "", string userName = "");
        Task<string> PostClosingMaster(tbl_Ac_FiscalYear_ClosingMaster tbl_Ac_FiscalYear_ClosingMaster, string operation = "", string userName = "");
        Task<string> PostFiscalYear(tbl_Ac_FiscalYear tbl_Ac_FiscalYear, string operation = "", string userName = "");
    }
    public interface IChartOfAccountsType
    {
        Task<object> Get(int id);
        object GetWCLChartOfAccountsType();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Ac_ChartOfAccounts_Type tbl_Ac_ChartOfAccounts_Type, string operation = "", string userName = "");
    }
    public interface IChartOfAccounts
    {
        Task<object> Get(int id);
        object GetWCLChartOfAccounts();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts, string operation = "", string userName = "");
        Task<object> GetNodesAsync(int PID = 0);
        Task<string> COAUploadExcelFile(List<COAExcelData> COAExcelDataList, string operation, string userName);

        #region Report     
        List<ReportCallingModel> GetRLChartOfAccounts();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface ICompositionCostingFactors
    {
        Task<object> Get(int id);
        object GetWCL();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Ac_CompositionCostingFactors tbl_Ac_CompositionCostingFactors, string operation = "", string userName = "");
    }
    public interface ICustomerApprovedRateList
    {
        Task<object> GetCustomerRateListDetail(int id);
        object GetWCLCustomerRateListDetail();
        object GetWCLCustomerRateListMaster();
        Task<PagedData<object>> LoadCustomerRateListDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCustomerRateListMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostCustomerRateListDetail(tbl_Ac_CustomerApprovedRateList tbl_Ac_CustomerApprovedRateList, string operation = "", string userName = "");
    }
    public interface IAcPolicyInventory
    {
        Task<object> Get(int id);
        object GetWCL();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Ac_PolicyInventory tbl_Ac_PolicyInventory, string operation = "", string userName = "");
    }
    public interface IAcPolicyWHTaxOnPurchase
    {
        Task<object> GetAcPolicyWHTaxOnPurchaseMaster(int id);
        object GetWCLAcPolicyWHTaxOnPurchaseDetail();
        object GetWCLAcPolicyWHTaxOnPurchaseMaster();
        Task<PagedData<object>> LoadAcPolicyWHTaxOnPurchaseDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadAcPolicyWHTaxOnPurchaseMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostAcPolicyWHTaxOnPurchaseMaster(tbl_Ac_PolicyWHTaxOnPurchase tbl_Ac_PolicyWHTaxOnPurchase, string operation = "", string userName = "");
    }
    public interface IAcPolicyWHTaxOnSales
    {
        Task<object> GetAcPolicyWHTaxOnSalesMaster(int id);
        object GetWCLAcPolicyWHTaxOnSalesDetail();
        object GetWCLAcPolicyWHTaxOnSalesMaster();
        Task<PagedData<object>> LoadAcPolicyWHTaxOnSalesDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadAcPolicyWHTaxOnSalesMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostAcPolicyWHTaxOnSalesMaster(tbl_Ac_PolicyWHTaxOnSales tbl_Ac_PolicyWHTaxOnSales, string operation = "", string userName = "");
    }
    public interface IBankDocument
    {
        Task<object> GetBankDocumentDetail(int id);
        Task<object> GetBankDocumentMaster(int id);
        object GetWCLBankDocumentDetail();
        object GetWCLBankDocumentMaster();
        object GetWCLBBankDocumentMaster();
        Task<PagedData<object>> LoadBankDocumentDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBankDocumentMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string IsFor = "");
        Task<string> PostBankDocumentDetail(tbl_Ac_V_BankDocumentDetail tbl_Ac_V_BankDocumentDetail, string operation = "", string userName = "");
        Task<string> PostBankDocumentMaster(tbl_Ac_V_BankDocumentMaster tbl_Ac_V_BankDocumentMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLBankReceiveDocument();
        List<ReportCallingModel> GetRLBankReceiveMasterDocument();

        List<ReportCallingModel> GetRLBankPaymentMasterDocument();
        List<ReportCallingModel> GetRLBankPaymentDocument();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface ICashDocument
    {
        Task<object> GetCashDocumentDetail(int id);
        Task<object> GetCashDocumentMaster(int id);
        object GetWCLCashDocumentDetail();
        object GetWCLCashDocumentMaster();
        object GetWCLBCashDocumentMaster();
        Task<PagedData<object>> LoadCashDocumentDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCashDocumentMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string IsFor = "");
        Task<string> PostCashDocumentDetail(tbl_Ac_V_CashDocumentDetail tbl_Ac_V_CashDocumentDetail, string operation = "", string userName = "");
        Task<string> PostCashDocumentMaster(tbl_Ac_V_CashDocumentMaster tbl_Ac_V_CashDocumentMaster, string operation = "", string userName = "");
        
        #region Report    
        List<ReportCallingModel> GetRLCashPaymentMasterDocument();
        List<ReportCallingModel> GetRLCashPaymentDocument();
        List<ReportCallingModel> GetRLCashReceiveMasterDocument();
        List<ReportCallingModel> GetRLCashReceiveDocument();

        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IJournalDocument
    {
        Task<object> GetJournalDocumentDetail(int id);
        Task<object> GetJournalDocumentMaster(int id);
        object GetWCLJournalDocumentDetail();
        object GetWCLJournalDocumentMaster();
        object GetWCLBJournalDocumentMaster();
        Task<PagedData<object>> LoadJournalDocumentDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadJournalDocumentMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostJournalDocumentDetail(tbl_Ac_V_JournalDocumentDetail tbl_Ac_V_JournalDocumentDetail, string operation = "", string userName = "");
        Task<string> PostJournalDocumentMaster(tbl_Ac_V_JournalDocumentMaster tbl_Ac_V_JournalDocumentMaster, string operation = "", string userName = "");
        Task<string> JournalDocumentDetailUploadExcelFile(List<JournalDocExcelData> JournalDocExcelDataList, string operation, string userName);
        #region Report    
        List<ReportCallingModel> GetRLJournalMasterDocument();
        List<ReportCallingModel> GetRLJournalDocument();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IJournalDocument2
    {
        Task<object> GetJournalDocument2Detail(int id);
        Task<object> GetJournalDocument2Master(int id);
        object GetWCLBJournalDocument2Master();
        object GetWCLJournalDocument2Detail();
        object GetWCLJournalDocument2Master();
        Task<PagedData<object>> LoadJournalDocument2Detail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadJournalDocument2Master(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostJournalDocument2Detail(tbl_Ac_V_JournalDocument2Detail tbl_Ac_V_JournalDocument2Detail, string operation = "", string userName = "");
        Task<string> PostJournalDocument2Master(tbl_Ac_V_JournalDocument2Master tbl_Ac_V_JournalDocument2Master, string operation = "", string userName = "");

        #region Report    
        List<ReportCallingModel> GetRLJournalDocument2Master();
        List<ReportCallingModel> GetRLJournalDocument2Detail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IPurchaseNoteInvoice
    {
        Task<object> GetPurchaseNoteInvoiceDetail(int id);
        Task<object> GetPurchaseNoteInvoiceMaster(int id);
        object GetWCLPurchaseNoteInvoiceDetail();
        object GetWCLPurchaseNoteInvoiceMaster();
        object GetWCLBPurchaseNoteInvoiceMaster();
        Task<PagedData<object>> LoadPurchaseNoteDetailOfDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseNoteInvoiceDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseNoteInvoiceMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostPurchaseNoteInvoiceDetail(tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "", string userName = "");
        Task<string> PostPurchaseNoteInvoiceMaster(tbl_Inv_PurchaseNoteMaster tbl_Inv_PurchaseNoteMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLPurchaseNote();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IPurchaseReturnNoteInvoice
    {
        Task<object> GetPurchaseReturnNoteInvoiceDetail(int id);
        Task<object> GetPurchaseReturnNoteInvoiceMaster(int id);
        object GetWCLPurchaseReturnNoteInvoiceDetail();
        object GetWCLPurchaseReturnNoteInvoiceMaster();
        object GetWCLBPurchaseReturnNoteInvoiceMaster();
        Task<PagedData<object>> LoadPurchaseReturnNoteInvoiceDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseReturnNoteInvoiceMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostPurchaseReturnNoteInvoiceDetail(tbl_Inv_PurchaseReturnNoteDetail tbl_Inv_PurchaseReturnNoteDetail, string operation = "", string userName = "");
        Task<string> PostPurchaseReturnNoteInvoiceMaster(tbl_Inv_PurchaseReturnNoteMaster tbl_Inv_PurchaseReturnNoteMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLPurchaseReturnNote();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface ISalesNoteInvoice
    {
        Task<object> GetSalesNoteInvoiceDetail(int id);
        Task<object> GetSalesNoteInvoiceMaster(int id);
        object GetWCLSalesNoteInvoiceDetail();
        object GetWCLSalesNoteInvoiceMaster();
        object GetWCLBSalesNoteInvoiceMaster();
        Task<PagedData<object>> LoadSalesNoteInvoiceDetailReturn(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalesNoteInvoiceDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalesNoteInvoiceMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostSalesNoteInvoiceDetail(tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail, string operation = "", string userName = "");
        Task<string> PostSalesNoteInvoiceMaster(tbl_Inv_SalesNoteMaster tbl_Inv_SalesNoteMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLSalesNote();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface ISalesReturnNoteInvoice
    {
        Task<object> GetSalesReturnNoteInvoiceDetail(int id);
        Task<object> GetSalesReturnNoteInvoiceMaster(int id);
        object GetWCLSalesReturnNoteInvoiceDetail();
        object GetWCLSalesReturnNoteInvoiceMaster();
        object GetWCLBSalesReturnNoteInvoiceMaster();
        Task<PagedData<object>> LoadSalesReturnNoteInvoiceDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalesReturnNoteInvoiceMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostSalesReturnNoteInvoiceDetail(tbl_Inv_SalesReturnNoteDetail tbl_Inv_SalesReturnNoteDetail, string operation = "", string userName = "");
        Task<string> PostSalesReturnNoteInvoiceMaster(tbl_Inv_SalesReturnNoteMaster tbl_Inv_SalesReturnNoteMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLSalesReturnNote();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IAcLedger
    {
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        List<ReportCallingModel> GetRLAcLedger();
        Task<PagedData<object>> LoadAcLedger(int CurrentPage = 1, int MasterID = 0, bool? TStatus = null, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
    }
    public interface IPolicyPaymentTerm
    {
        Task<object> GetPolicyPaymentTerm(int id);
        object GetWCLPolicyPaymentTerm();
        object GetWCLPolicyPaymentTermCOA();
        Task<PagedData<object>> LoadPolicyPaymentTerm(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPolicyPaymentTermCOA(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostPolicyPaymentTerm(tbl_Ac_PolicyPaymentTerm tbl_Ac_PolicyPaymentTerm, string operation = "", string userName = "");
    }

    //---------------------Production------------------------//
    public interface ICompositionCosting
    {
        Task<object> GetCompositionDetailPackaging(int id);
        Task<object> GetCompositionDetailRaw(int id);
        object GetWCLCompositionDetailPackaging();
        object GetWCLCompositionDetailRaw();
        object GetWCLCompositionMaster();
        Task<PagedData<object>> LoadCompositionDetailPackaging(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionDetailRaw(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostCompositionDetailPackaging(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items, string operation = "", string userName = "");
        Task<string> PostCompositionDetailRaw(tbl_Pro_CompositionDetail_RawDetail_Items tbl_Pro_CompositionDetail_RawDetail_Items, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLCompositionDetail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IBMRCosting
    {
        object GetWCLBMRDetailPackagingMaster();
        object GetWCLBMRMaster();

        Task<PagedData<object>> LoadBMRDetailPackagingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);

        List<ReportCallingModel> GetRLBMRDetail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        


    }
    public interface IAccountsDashboard
    {
        Task<object> GetDashBoardData(string userName = "");
    }

}
