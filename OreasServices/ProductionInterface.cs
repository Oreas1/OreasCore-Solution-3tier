using Microsoft.EntityFrameworkCore;
using OreasModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public interface IProductionList
    {
        Task<object> GetCompositionFilterPolicyListAsync(bool? ForRaw1_Packaging0 = null);
        Task<object> GetProProcedureListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetBMRAdditionalTypeListAsync(string FilterByText = null, string FilterValueByText = null);

        Task<object> GetQcTestListAsync(string FilterByText = null, string FilterValueByText = null);
    }
    public interface ICompositionFilterPolicy
    {
        Task<object> GetCompositionFilterPolicyDetail(int id);
        Task<object> GetCompositionFilterPolicyMaster(int id);
        object GetWCLCompositionFilterPolicyDetail();
        Task<PagedData<object>> LoadCompositionFilterPolicyDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionFilterPolicyMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostCompositionFilterPolicyDetail(tbl_Pro_CompositionFilterPolicyDetail tbl_Pro_CompositionFilterPolicyDetail, string operation = "", string userName = "");
        Task<string> PostCompositionFilterPolicyMaster(tbl_Pro_CompositionFilterPolicyMaster tbl_Pro_CompositionFilterPolicyMaster, string operation = "", string userName = "");
    }
    public interface IProProcedure
    {
        Task<object> Get(int id);
        object GetWCLProProcedure();
        Task<PagedData<object>> Load(string caller = "", int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Pro_Procedure tbl_Pro_Procedure, string operation = "", string userName = "");
    }
    public interface IComposition
    {
        Task<object> GetCompositionDetailCouplingDetailPackagingDetailDetail(int id);
        Task<object> GetCompositionDetailCouplingDetailPackagingDetailMaster(int id);
        Task<object> GetCompositionDetailCouplingDetailPackagingMaster(int id);
        Task<object> GetCompositionDetailCouplingMaster(int id);
        Task<object> GetCompositionDetailRawDetail(int id);
        Task<object> GetCompositionDetailRawMaster(int id);
        Task<object> GetCompositionMaster(int id);
        object GetWCLCompositionDetailCouplingDetailPackagingDetailDetail();
        object GetWCLCompositionDetailCouplingDetailPackagingDetailMaster();
        object GetWCLCompositionDetailCouplingDetailPackagingMaster();
        object GetWCLCompositionDetailCouplingMaster();
        object GetWCLCompositionDetailRawDetail();
        object GetWCLCompositionDetailRawMaster();
        object GetWCLCompositionMaster();
        Task<PagedData<object>> LoadCompositionDetailCouplingDetailPackagingDetailDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionDetailCouplingDetailPackagingDetailMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionDetailCouplingDetailPackagingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionDetailCouplingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionDetailRawDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionDetailRawMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostCompositionDetailCouplingDetailPackagingDetailDetail(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items, string operation = "", string userName = "");
        Task<string> PostCompositionDetailCouplingDetailPackagingDetailMaster(tbl_Pro_CompositionDetail_Coupling_PackagingDetail tbl_Pro_CompositionDetail_Coupling_PackagingDetail, string operation = "", string userName = "");
        Task<string> PostCompositionDetailCouplingDetailPackagingMaster(tbl_Pro_CompositionDetail_Coupling_PackagingMaster tbl_Pro_CompositionDetail_Coupling_PackagingMaster, string operation = "", string userName = "");
        Task<string> PostCompositionDetailCouplingMaster(tbl_Pro_CompositionDetail_Coupling tbl_Pro_CompositionDetail_Coupling, string operation = "", string userName = "");
        Task<string> PostCompositionDetailRawDetail(tbl_Pro_CompositionDetail_RawDetail_Items tbl_Pro_CompositionDetail_RawDetail_Items, string operation = "", string userName = "");
        Task<string> PostCompositionDetailRawMaster(tbl_Pro_CompositionDetail_RawMaster tbl_Pro_CompositionDetail_RawMaster, string operation = "", string userName = "");
        Task<string> PostCompositionMaster(tbl_Pro_CompositionMaster tbl_Pro_CompositionMaster, string operation = "", string userName = "");

        #region BMRProcess
        Task<object> GetBMRProcess(int id);
        object GetWCLBMRProcess();
        Task<PagedData<object>> LoadBMRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRProcess(tbl_Pro_CompositionMaster_ProcessBMR tbl_Pro_CompositionMaster_ProcessBMR, string operation = "", string userName = "");
        #endregion

        #region BPRProcess
        Task<object> GetBPRProcess(int id);
        object GetWCLBPRProcess();
        Task<PagedData<object>> LoadBPRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBPRProcess(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR, string operation = "", string userName = "");
        #endregion

        #region Report
        List<ReportCallingModel> GetRLCompositionRawMaster();
        List<ReportCallingModel> GetRLCouplingPackagingMaster();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        
        #endregion
    }
    public interface IBMR
    {
        Task<object> GetBMRDetailPackagingDetailFilter(int id);
        Task<object> GetBMRDetailPackagingDetailFilterDetailItem(int id);
        Task<object> GetBMRDetailPackagingMaster(int id);
        Task<object> GetBMRDetailRawDetailItem(int id);
        Task<object> GetBMRDetailRawMaster(int id);
        Task<object> GetBMRMaster(int id);
        object GetWCLBMRDetailPackagingDetailFilter();
        object GetWCLBMRDetailPackagingDetailFilterDetailItem();
        object GetWCLBMRDetailPackagingMaster();
        object GetWCLBMRDetailRawDetailItem();
        object GetWCLBMRDetailRawMaster();
        object GetWCLBMRMaster();
        object GetWCLBBMRMaster();
        Task<PagedData<object>> LoadBMRDetailPackagingDetailFilter(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRDetailPackagingDetailFilterDetailItem(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRDetailPackagingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRDetailRawDetailItem(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRDetailRawMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRDetailPackagingDetailFilter(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail, string operation = "", string userName = "");
        Task<string> PostBMRDetailPackagingDetailFilterDetailItem(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items, string operation = "", string userName = "");
        Task<string> PostBMRDetailPackagingMaster(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster, string operation = "", string userName = "");
        Task<string> PostBMRDetailRawDetailItem(tbl_Pro_BatchMaterialRequisitionDetail_RawDetail tbl_Pro_BatchMaterialRequisitionDetail_RawDetail, string operation = "", string userName = "");
        Task<string> PostBMRDetailRawMaster(tbl_Pro_BatchMaterialRequisitionDetail_RawMaster tbl_Pro_BatchMaterialRequisitionDetail_RawMaster, string operation = "", string userName = "");
        Task<string> PostBMRMaster(tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster, string operation = "", string userName = "");

        Task<object> GetBMRProcess(int id);
        object GetWCLBMRProcess();
        Task<PagedData<object>> LoadBMRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRProcess(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, string operation = "", string userName = "");

        Task<object> GetBPRProcess(int id);
        object GetWCLBPRProcess();
        Task<PagedData<object>> LoadBPRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBPRProcess(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, string operation = "", string userName = "");

        Task<string> PostStockIssuanceReservation(int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, bool IssueTrue_ReserveFalse = true, string userName = "");
        #region Report
        List<ReportCallingModel> GetRLBMRDetail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion
    }

    public interface IBMRBPRProcess
    {
        Task<object> GetBMRBPRMaster(int id);
        Task<object> GetBMRProcess(int id);
        Task<object> GetBMRSample(int id);
        Task<object> GetBPRProcess(int id);
        Task<object> GetBPRSample(int id);
        object GetWCLBBMRBPRMaster();
        object GetWCLBMRBPRMaster();
        object GetWCLBMRProcess();
        object GetWCLBMRSample();
        object GetWCLBPRProcess();
        object GetWCLBPRSample();
        Task<PagedData<object>> LoadBMRBPRMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRSample(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBPRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBPRSample(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRBPRMaster(tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster, string operation = "", string userName = "");
        Task<string> PostBMRProcess(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, string operation = "", string userName = "");
        Task<string> PostBMRSample(tbl_Qc_SampleProcessBMR tbl_Qc_SampleProcessBMR, string operation = "", string userName = "");
        Task<string> PostBPRProcess(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, string operation = "", string userName = "");
        Task<string> PostBPRSample(tbl_Qc_SampleProcessBPR tbl_Qc_SampleProcessBPR, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLMaster();
        List<ReportCallingModel> GetRLDetail();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IBMRAdditional
    {
        Task<object> GetBatchNoList(string FilterBy = "BatchNo", string FilterValue = "");
        Task<object> GetBMRAdditionalDetail(int id);
        Task<object> GetBMRAdditionalMaster(int id);
        object GetWCLBMRAdditionalDetail();
        object GetWCLBMRAdditionalMaster();
        object GetWCLBBMRAdditionalMaster();
        Task<PagedData<object>> LoadBMRAdditionalDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRAdditionalMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRAdditionalDetail(tbl_Pro_BMRAdditionalDetail tbl_Pro_BMRAdditionalDetail, string operation = "", string userName = "");
        Task<string> PostBMRAdditionalMaster(tbl_Pro_BMRAdditionalMaster tbl_Pro_BMRAdditionalMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLBMRAdditional();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IProductionTransfer
    {
        Task<object> GetProductionTransferDetail(int id);
        Task<object> GetProductionTransferMaster(int id);
        object GetWCLProductionTransferDetail();
        object GetWCLBProductionTransferDetail();
        object GetWCLProductionTransferMaster();
        object GetWCLBProductionTransferMaster();
        Task<PagedData<object>> LoadProductionTransferDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadProductionTransferMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostProductionTransferDetail(tbl_Pro_ProductionTransferDetail tbl_Pro_ProductionTransferDetail, string operation = "", string userName = "");
        Task<string> PostProductionTransferMaster(tbl_Pro_ProductionTransferMaster tbl_Pro_ProductionTransferMaster, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLProductionTransfer();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IOrderNoteProduction
    {

        object GetWCLBOrderNoteDetail();
        object GetWCLOrderNoteDetail();
        Task<object> GetBMRAvailability(int id);
        Task<object> GetProductionOrder(int id);        
        Task<PagedData<object>> LoadOrderNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadProductionOrder(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);

        Task<string> PostProductionOrder(tbl_Inv_OrderNoteDetail_ProductionOrder tbl_Inv_OrderNoteDetail_ProductionOrder, string operation = "", string userName = "");
    }
    public interface IProductionDashboard
    {
        Task<object> GetDashBoardData(string userName = "");
    }

}
