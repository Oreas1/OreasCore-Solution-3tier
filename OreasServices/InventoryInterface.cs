using OreasModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public interface IInventoryList
    {        
        Task<object> GetMeasurementUnitListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetProductClassificationListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetProductTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetProductTypeCategoryListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetWareHouseListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetWHMListAsync(string QueryName = "", string WHMilterBy = "", string WHMFilterValue = "", int FormID = 0, string userName = "");
        Task<object> GetProductListAsync(string QueryName = "", string ProductFilterBy = "", string ProductFilterValue = "", int? masterid = null, DateTime? tillDate = null, int? detailid = null, string userid = "");
        Task<object> GetReferenceListAsync(string QueryName = "", string ReferenceFilterBy = "", string ReferenceFilterValue = "", int? masterid = null, DateTime? tillDate = null, int? detailid = null);
        Task<object> GetOrdinaryRequisitionTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetPurchaseOrderListAsync(string QueryName = "", string POFilterBy = "", string POFilterValue = "", int SupplierID = 0, int ProductID = 0);
        Task<object> GetSubDistributorListAsync(string QueryName = "", string SDFilterBy = "", string SDFilterValue = "", int CustomerID = 0);
        Task<object> GetOrderNoteListAsync(string QueryName = "", string ONFilterBy = "", string ONFilterValue = "", int CustomerID = 0, int ProductID = 0);
        Task<object> GetPOTermsConditionsListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetSalesNoteForReturnAsync(int CustomerID = 0, int? PurchaseRefID = null, int? BMRRefID = null, string SalesNoteFilterBy = null, string SalesNoteFilterValue = null);
        Task<object> GetPOSupplierListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetPOManufacturerListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetPOIndenterListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetPOImportTermListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetCurrencyCodeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetCountryListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetInternationalCommercialTermListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetTransportListAsync(string FilterByText = null, string FilterValueByText = null);

    }
    public interface IMeasurementUnit
    {
        Task<object> Get(int id);
        object GetWCLMeasurementUnit();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Inv_MeasurementUnit tbl_Inv_MeasurementUnit, string operation = "", string userName = "");
    }
    public interface IProductClassification
    {
        Task<object> Get(int id);
        object GetWCLProductClassification();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Inv_ProductClassification tbl_Inv_ProductClassification, string operation = "", string userName = "");
    }
    public interface IProductType
    {
        Task<object> GetProductTypeDetail(int id);
        Task<object> GetProductTypeMaster(int id);
        object GetWCLProductTypeDetail();
        object GetWCLProductTypeMaster();
        Task<PagedData<object>> LoadProductTypeDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadProductTypeMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostProductTypeDetail(tbl_Inv_ProductType_Category tbl_Inv_ProductType_Category, string operation = "", string userName = "");
        Task<string> PostProductTypeMaster(tbl_Inv_ProductType tbl_Inv_ProductType, string operation = "", string userName = "");

        #region report
        List<ReportCallingModel> GetRLProductTypeDetail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion

    }
    public interface IWareHouse
    {
        Task<object> GetWareHouseDetail(int id);
        Task<object> GetWareHouseMaster(int id);
        object GetWCLWareHouseDetail();
        object GetWCLWareHouseMaster();
        Task<PagedData<object>> LoadWareHouseDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadWareHouseMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostWareHouseDetail(tbl_Inv_WareHouseDetail tbl_Inv_WareHouseDetail, string operation = "", string userName = "");
        Task<string> PostWareHouseMaster(tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster, string operation = "", string userName = "");
    }
    public interface IProductRegistration
    {
        Task<object> GetProductRegistrationDetail(int id);
        Task<object> GetProductRegistrationMaster(int id);
        object GetWCLProductRegistrationDetail();
        object GetWCLProductRegistrationMaster();
        Task<PagedData<object>> LoadProductRegistrationDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadProductRegistrationMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostProductRegistrationDetail(tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail, string operation = "", string userName = "");
        Task<string> PostProductRegistrationMaster(tbl_Inv_ProductRegistrationMaster tbl_Inv_ProductRegistrationMaster, string operation = "", string userName = "");
        Task<object> GetNodesAsync(int PID = 0, int MasterID = 0);
        Task<string> ProductRegistrationUploadExcelFile(List<ProdRegExcelData> ProdRegExcelDataList, string operation, string userName);

        #region Report     
        List<ReportCallingModel> GetRLProductRegistration();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IPurchaseNote
    {
        Task<object> GetPurchaseNoteDetail(int id);
        Task<object> GetPurchaseNoteMaster(int id);
        object GetWCLPurchaseNoteMaster();
        object GetWCLBPurchaseNoteMaster();
        object GetWCLPurchaseNoteDetail();
        object GetWCLBPurchaseNoteDetail();        
        Task<PagedData<object>> LoadPurchaseNoteDetailOfDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostPurchaseNoteDetail(tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "", string userName = "");
        Task<string> PostPurchaseNoteMaster(tbl_Inv_PurchaseNoteMaster tbl_Inv_PurchaseNoteMaster, string operation = "", string userName = "");
        Task<string> PurchaseNoteDetailUploadExcelFile(List<PurchaseNoteExcelData> PurchaseNoteExcelDataList, int MasterID, string operation, string userName);
        #region Report     
        List<ReportCallingModel> GetRLPurchaseNoteMaster();
        List<ReportCallingModel> GetRLPurchaseNoteDetail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IPurchaseReturnNote
    {
        Task<object> GetPurchaseReturnNoteDetail(int id);
        Task<object> GetPurchaseReturnNoteMaster(int id);
        object GetWCLPurchaseReturnNoteDetail();
        object GetWCLPurchaseReturnNoteMaster();
        Task<PagedData<object>> LoadPurchaseReturnNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseReturnNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostPurchaseReturnNoteDetail(tbl_Inv_PurchaseReturnNoteDetail tbl_Inv_PurchaseReturnNoteDetail, string operation = "", string userName = "");
        Task<string> PostPurchaseReturnNoteMaster(tbl_Inv_PurchaseReturnNoteMaster tbl_Inv_PurchaseReturnNoteMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLPurchaseReturnNote();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface ISalesNote
    {
        Task<object> GetSalesNoteDetail(int id);
        Task<object> GetSalesNoteMaster(int id);
        object GetWCLSalesNoteDetail();
        object GetWCLSalesNoteMaster();
        Task<PagedData<object>> LoadSalesNoteDetailReturn(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalesNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalesNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostSalesNoteDetail(tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail, string operation = "", string userName = "");
        Task<string> PostSalesNoteMaster(tbl_Inv_SalesNoteMaster tbl_Inv_SalesNoteMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLSalesNoteMaster();
        List<ReportCallingModel> GetRLSalesNoteDetail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface ISalesReturnNote
    {
        Task<object> GetSalesReturnNoteDetail(int id);
        Task<object> GetSalesReturnNoteMaster(int id);
        object GetWCLSalesReturnNoteDetail();
        object GetWCLSalesReturnNoteMaster();
        Task<PagedData<object>> LoadSalesReturnNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalesReturnNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostSalesReturnNoteDetail(tbl_Inv_SalesReturnNoteDetail tbl_Inv_SalesReturnNoteDetail, string operation = "", string userName = "");
        Task<string> PostSalesReturnNoteMaster(tbl_Inv_SalesReturnNoteMaster tbl_Inv_SalesReturnNoteMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLSalesReturnNote();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IOrdinaryRequisition
    {
        Task<object> GetOrdinaryRequisitionDetail(int id);
        Task<object> GetOrdinaryRequisitionMaster(int id);
        object GetWCLOrdinaryRequisitionDetail();
        object GetWCLOrdinaryRequisitionMaster();
        object GetWCLBOrdinaryRequisitionMaster();
        Task<PagedData<object>> LoadOrdinaryRequisitionDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadOrdinaryRequisitionMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "", bool IsCanViewOnlyOwnData = false);
        Task<string> PostOrdinaryRequisitionDetail(tbl_Inv_OrdinaryRequisitionDetail tbl_Inv_OrdinaryRequisitionDetail, string operation = "", string userName = "");
        Task<string> PostOrdinaryRequisitionMaster(tbl_Inv_OrdinaryRequisitionMaster tbl_Inv_OrdinaryRequisitionMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLOrdinaryRequisition();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IOrdinaryRequisitionDispensing
    {
        Task<object> GetOrdinaryRequisitionDispensingDetail(int id);
        Task<object> GetOrdinaryRequisitionDispensingDetailDispensing(int id);
        Task<object> GetOrdinaryRequisitionDispensingMaster(int id);
        object GetWCLOrdinaryRequisitionDispensingDetail();
        object GetWCLOrdinaryRequisitionDispensingDetailDispensing();
        object GetWCLOrdinaryRequisitionDispensingMaster();
        object GetWCLBOrdinaryRequisitionDispensingMaster();
        Task<PagedData<object>> LoadOrdinaryRequisitionDispensingDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadOrdinaryRequisitionDispensingDetailDispensing(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadOrdinaryRequisitionDispensingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostOrdinaryRequisitionDispensingDetail(tbl_Inv_OrdinaryRequisitionDetail tbl_Inv_OrdinaryRequisitionDetail, string operation = "", string userName = "");
        Task<string> PostOrdinaryRequisitionDispensingDetailDispensing(tbl_Inv_OrdinaryRequisitionDispensing tbl_Inv_OrdinaryRequisitionDispensing, string operation = "", string userName = "");
        Task<string> PostStockIssuanceReservation(int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, bool IssueTrue_ReserveFalse = true, string userName = "");
        #region Report     
        List<ReportCallingModel> GetRLOrdinaryRequisitionDispensing();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IBMRAdditionalDispensing
    {
        Task<object> GetBMRAdditionalDispensingDetail(int id);
        Task<object> GetBMRAdditionalDispensingDetailDispensing(int id);
        Task<object> GetBMRAdditionalDispensingMaster(int id);
        object GetWCLBMRAdditionalDispensingDetail();
        object GetWCLBMRAdditionalDispensingDetailDispensing();
        object GetWCLBMRAdditionalDispensingMaster();
        object GetWCLBBMRAdditionalDispensingMaster();
        Task<PagedData<object>> LoadBMRAdditionalDispensingDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRAdditionalDispensingDetailDispensing(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRAdditionalDispensingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostBMRAdditionalDispensingDetailDispensing(tbl_Inv_BMRAdditionalDispensing tbl_Inv_BMRAdditionalDispensing, string operation = "", string userName = "");
        Task<string> PostBMRAdditionalDispensingDetail(tbl_Pro_BMRAdditionalDetail tbl_Pro_BMRAdditionalDetail, string operation = "", string userName = "");
        Task<string> PostStockIssuanceReservation(int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, bool IssueTrue_ReserveFalse = true, string userName = "");
        #region Report     
        List<ReportCallingModel> GetRLBMRAdditionalRequisitionDetailDispensing();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IBMRDispensing
    {
        Task<object> GetBMRDispensingDetailPackagingDetailItems(int id);
        Task<object> GetBMRDispensingDetailRawItems(int id);
        Task<object> GetBMRDispensingMaster(int id);
        Task<object> GetBMRDispensingPackaging(int id);
        Task<object> GetBMRDispensingRaw(int id);
        object GetWCLBBMRDispensingMaster();
        object GetWCLBMRDispensingDetailPackagingDetailItems();
        object GetWCLBMRDispensingDetailRawItems();
        object GetWCLBMRDispensingMaster();
        object GetWCLBMRDispensingPackaging();
        object GetWCLBMRDispensingRaw();
        Task<PagedData<object>> LoadBMRDispensingDetailPackagingDetailItems(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadBMRDispensingDetailRawItems(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadBMRDispensingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRDispensingPackaging(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRDispensingRaw(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRDispensingPackaging(tbl_Inv_BMRDispensingPackaging tbl_Inv_BMRDispensingPackaging, string operation = "", string userName = "");
        Task<string> PostBMRDispensingRaw(tbl_Inv_BMRDispensingRaw tbl_Inv_BMRDispensingRaw, string operation = "", string userName = "");
        Task<string> PostStockIssuanceReservation(int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, bool IssueTrue_ReserveFalse = true, string userName = "");
        #region Report     
        List<ReportCallingModel> GetRLBMRDispensingRaw();
        List<ReportCallingModel> GetRLBMRDispensingPackaging();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IInvProductionTransfer
    {
        Task<object> GetProductionTransferDetail(int id);
        object GetWCLBProductionTransferDetail();
        object GetWCLBProductionTransferMaster();
        object GetWCLProductionTransferDetail();
        object GetWCLProductionTransferMaster();
        Task<PagedData<object>> LoadProductionTransferDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadProductionTransferMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostProductionTransferDetail(tbl_Pro_ProductionTransferDetail tbl_Pro_ProductionTransferDetail, string operation = "", string userName = "");
    }
    public interface IPDRequestDispensing
    {
        Task<object> GetRequestCFPDetail(int id);
        Task<object> GetRequestCFPDetailDispensing(int id);
        object GetWCLBRequestCFPDetail();
        object GetWCLBRequestCFPMaster();
        object GetWCLRequestCFPDetail();
        object GetWCLRequestCFPMaster();
        Task<PagedData<object>> LoadRequestCFPDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadRequestCFPDetailDispensing(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadRequestCFPMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "", List<int> AuthStoreList = null);
        Task<string> PostRequestCFPDetail(tbl_PD_RequestDetailTR_CFP_Item tbl_PD_RequestDetailTR_CFP_Item, string operation = "", string userName = "");
        Task<string> PostRequestCFPDetailDispensing(tbl_Inv_PDRequestDispensing tbl_Inv_PDRequestDispensing, string operation = "", string userName = "");
    }
    public interface IInvLedger
    {
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        List<ReportCallingModel> GetRLInvLedger();
        List<ReportCallingModel> GetRLInvLedgerAc();
        Task<PagedData<object>> LoadInvLedger(int CurrentPage = 1, int MasterID = 0, int WareHouseID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
    }
    public interface IStockTransfer
    {
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        List<ReportCallingModel> GetRLStockTransfer();
        Task<object> GetStockTransferDetail(int id);
        Task<object> GetStockTransferMaster(int id);
        object GetWCLBStockTransferDetail();
        object GetWCLBStockTransferMaster();
        object GetWCLStockTransferDetail();
        object GetWCLStockTransferMaster();
        Task<PagedData<object>> LoadStockTransferDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadStockTransferMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostStockTransferDetail(tbl_Inv_StockTransferDetail tbl_Inv_StockTransferDetail, string operation = "", string userName = "");
        Task<string> PostStockTransferMaster(tbl_Inv_StockTransferMaster tbl_Inv_StockTransferMaster, string operation = "", string userName = "");
        Task<string> PostReceviedStockTransferDetail(tbl_Inv_StockTransferDetail tbl_Inv_StockTransferDetail, string operation = "", string userName = "");
    }

    /// <summary>
    /// Supply Chain
    /// </summary>
    public interface IInternationalCommercialTerm
    {
        Task<object> GetInternationalCommercialTerm(int id);
        object GetWCLInternationalCommercialTerm();
        Task<PagedData<object>> LoadInternationalCommercialTerm(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostInternationalCommercialTerm(tbl_Inv_InternationalCommercialTerm tbl_Inv_InternationalCommercialTerm, string operation = "", string userName = "");
    }
    public interface ITransportType
    {
        Task<object> GetTransportType(int id);
        object GetWCLTransportType();
        Task<PagedData<object>> LoadTransportType(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostTransportType(tbl_Inv_TransportType tbl_Inv_TransportType, string operation = "", string userName = "");
    }
    public interface ISupplier
    {
        Task<object> GetSupplier(int id);
        object GetWCLSupplier();
        Task<PagedData<object>> LoadSupplier(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostSupplier(tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts, string operation = "", string userName = "");

        #region report
        List<ReportCallingModel> GetRLSupplierMaster();
        List<ReportCallingModel> GetRLSupplierDetail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        
        #endregion
    }
    public interface ICustomerSubDistributorList
    {
        Task<object> GetCustomerSubDistributorListDetail(int id);
        object GetWCLCustomerSubDistributorListDetail();
        object GetWCLCustomerSubDistributorListMaster();
        Task<PagedData<object>> LoadCustomerSubDistributorListDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCustomerSubDistributorListMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostCustomerSubDistributorListDetail(tbl_Ac_CustomerSubDistributorList tbl_Ac_CustomerSubDistributorList, string operation = "", string userName = "");
    }
    public interface IOrderNote
    {
        Task<object> GetOrderNoteDetail(int id);
        Task<object> GetOrderNoteDetailSubDistributor(int id);
        Task<object> GetOrderNoteMaster(int id);
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        List<ReportCallingModel> GetRLOrderNote();
        List<ReportCallingModel> GetRLOrderNoteMaster();
        object GetWCLBMRDetail();
        object GetWCLBOrderNoteDetail();
        object GetWCLBOrderNoteMaster();
        object GetWCLOrderNoteDetail();
        object GetWCLOrderNoteDetailSubDistributor();
        object GetWCLOrderNoteMaster();
        object GetWCLSalesNoteDetail();
        object GetWCLSalesReturnNoteDetail();
        Task<PagedData<object>> LoadBMRDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadOrderNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadOrderNoteDetailSubDistributor(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadOrderNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalesNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalesReturnNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostOrderNoteDetail(tbl_Inv_OrderNoteDetail tbl_Inv_OrderNoteDetail, string operation = "", string userName = "");
        Task<string> PostOrderNoteDetailSubDistributor(tbl_Inv_OrderNoteDetail_SubDistributor tbl_Inv_OrderNoteDetail_SubDistributor, string operation = "", string userName = "");
        Task<string> PostOrderNoteMaster(tbl_Inv_OrderNoteMaster tbl_Inv_OrderNoteMaster, string operation = "", string userName = "");
    }
    public interface IPurchaseOrder
    {
        Task<object> GetPurchaseOrderDetail(int id);
        Task<object> GetPurchaseOrderImportDetail(int id);
        Task<object> GetPurchaseOrderMaster(int id);
        Task<object> GetPurchaseOrderImportMaster(int id);
        object GetWCLBPurchaseOrderDetail();
        object GetWCLBPurchaseOrderMaster();
        object GetWCLPurchaseOrderDetail();
        object GetWCLPurchaseOrderMaster();
        object GetWCLPurchaseOrderImportMaster();
        Task<PagedData<object>> LoadPurchaseOrderDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseOrderImportDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseOrderMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "", bool IsCanViewOnlyOwnData = false);
        Task<PagedData<object>> LoadPurchaseOrderImportMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "", bool IsCanViewOnlyOwnData = false);
        Task<PagedData<object>> LoadPurchaseOrderDetailPN(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseOrderDetailPRN(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostPurchaseOrderDetail(tbl_Inv_PurchaseOrderDetail tbl_Inv_PurchaseOrderDetail, string operation = "", string userName = "");
        Task<string> PostPurchaseOrderMaster(tbl_Inv_PurchaseOrderMaster tbl_Inv_PurchaseOrderMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLPurchaseOrder();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IPurchaseOrderTermsConditions
    {
        Task<object> Get(int id);
        object GetWCLPurchaseOrderTermsConditions();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Inv_PurchaseOrderTermsConditions tbl_Inv_PurchaseOrderTermsConditions, string operation = "", string userName = "");
    }
    public interface IPurchaseOrderImportTerms
    {
        Task<object> Get(int id);
        object GetWCLPurchaseOrderImportTerm();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Inv_PurchaseOrder_ImportTerms tbl_Inv_PurchaseOrder_ImportTerms, string operation = "", string userName = "");
    }
    public interface IPOSupplier
    {
        Task<object> Get(int id);
        object GetWCLSupplier();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Inv_PurchaseOrder_Supplier tbl_Inv_PurchaseOrder_Supplier, string operation = "", string userName = "");
    }
    public interface IPOIndenter
    {
        Task<object> Get(int id);
        object GetWCLIndenter();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Inv_PurchaseOrder_Indenter tbl_Inv_PurchaseOrder_Indenter, string operation = "", string userName = "");
    }
    public interface IPOManufacturer
    {
        Task<object> Get(int id);
        object GetWCLManufacturer();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Inv_PurchaseOrder_Manufacturer tbl_Inv_PurchaseOrder_Manufacturer, string operation = "", string userName = "");
    }
    public interface IPurchaseRequest
    {
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        Task<object> GetPurchaseRequestDetail(int id);
        Task<object> GetPurchaseRequestMaster(int id);
        List<ReportCallingModel> GetRLPurchaseRequest();
        object GetWCLBPurchaseRequestDetail();
        object GetWCLBPurchaseRequestMaster();
        object GetWCLPurchaseRequestDetail();
        object GetWCLPurchaseRequestMaster();
        Task<PagedData<object>> LoadPurchaseRequestDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadPurchaseRequestMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostPurchaseRequestDetail(tbl_Inv_PurchaseRequestDetail tbl_Inv_PurchaseRequestDetail, string operation = "", string userName = "");
        Task<string> PostPurchaseRequestMaster(tbl_Inv_PurchaseRequestMaster tbl_Inv_PurchaseRequestMaster, string operation = "", string userName = "");
    }
    public interface IPurchaseRequestBids
    {
        Task<object> GetPurchaseRequestBidsDetail(int id);
        Task<object> GetPurchaseRequestBidsMaster(int id);
        Task<object> GetPOSuggestions(int ProductID = 0);
        object GetWCLBPurchaseRequestBidsDetail();
        object GetWCLBPurchaseRequestBidsMaster();
        object GetWCLPurchaseRequestBidsDetail();
        object GetWCLPurchaseRequestBidsMaster();
        Task<object> IsPurchaseRequestAprrover(string userName = "");
        Task<PagedData<object>> LoadPurchaseRequestBidsDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadPurchaseRequestBidsMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostPurchaseRequestBidsDetail(tbl_Inv_PurchaseRequestDetail_Bids tbl_Inv_PurchaseRequestDetail_Bids, string operation = "", string userName = "");
        Task<string> PostPurchaseRequestBidsDecision(tbl_Inv_PurchaseRequestDetail_Bids tbl_Inv_PurchaseRequestDetail_Bids,int tbl_Inv_PurchaseRequestDetail_BidsID = 0, bool? Decision = null, string userName = "");
    }
    public interface IInventoryDashboard
    {
        Task<object> GetDashBoardData(string userName = "");
    }
}
