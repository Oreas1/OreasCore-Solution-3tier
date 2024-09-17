using OreasModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public interface IQaDocumentControl
    {
        Task<object> GetQaDocumentControl(int id);
        object GetWCLQaDocumentControl();
        Task<PagedData<object>> LoadQaDocumentControl(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostQaDocumentControl(tbl_Qa_DocumentControl tbl_Qa_DocumentControl, string operation = "", string userName = "");
    }
    public interface IQAProcess
    {
        Task<object> GetBMRProcess(int id);
        Task<object> GetBPRProcess(int id);
        object GetWCLBatchRecordMaster();
        object GetWCLBBatchRecordMaster();
        object GetWCLBMRProcess();
        object GetWCLBPRProcess();
        Task<PagedData<object>> LoadBatchRecordMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBPRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRProcess(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, string operation = "", string userName = "");
        Task<string> PostBPRProcess(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, string operation = "", string userName = "");
    }
    public interface IQAProductionOrder
    {
        object GetWCLProductionOrder();
        object GetWCLBProductionOrder();
        Task<PagedData<object>> LoadProductionOrder(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<object> GetBMRAvailability(int id);
        Task<string> GenerateBMR(int FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID, string SPOperation = "", string userName = "");

        Task<PagedData<object>> LoadProductionOrderBatch(int CurrentPage = 1, string BatchNo = "", string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
    }
    public interface IQAProductionTransfer
    {
        Task<object> GetProductionTransferDetail(int id);
        object GetWCLBProductionTransferDetail();
        object GetWCLBProductionTransferMaster();
        object GetWCLProductionTransferDetail();
        object GetWCLProductionTransferMaster();
        Task<PagedData<object>> LoadProductionTransferDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadProductionTransferMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostProductionTransferDetail(tbl_Pro_ProductionTransferDetail tbl_Pro_ProductionTransferDetail, string operation = "", string userName = "");
    }

}
