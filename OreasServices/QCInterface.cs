using OreasModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public interface IQcList
    {
        Task<object> GetActionTypeListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetQcLabListAsync(string FilterByText = null, string FilterValueByText = null);
    }
    public interface IProductRegistrationQcTestForPN
    {
        Task<object> GetProductRegistrationPNQcTest(int id);
        object GetWCLProductRegistrationPNQcTest();
        object GetWCLProductRegistration();
        Task<PagedData<object>> LoadProductRegistrationPNQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadProductRegistration(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostProductRegistrationPNQcTest(tbl_Inv_ProductRegistrationDetail_PNQcTest tbl_Inv_ProductRegistrationDetail_PNQcTest, string operation = "", string userName = "");
    }
    public interface IQcPurchaseNote
    {
        Task<object> Get(int id);
        object GetWCLQcPurchaseNote();
        object GetWCLBQcPurchaseNote();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> Post(tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "", string userName = "");

        #region Report     
        List<ReportCallingModel> GetRLQcQaPurchaseNote();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");
        #endregion
    }
    public interface IQcDashboard
    {
        Task<object> GetDashBoardData(string userName = "");
    }
    public interface IQCProcess
    {
        Task<object> GetBMRSample(int id);
        Task<object> GetBMRSampleQcTest(int id);
        Task<object> GetBPRSample(int id);
        Task<object> GetBPRSampleQcTest(int id);
        object GetWCLBatchRecordMaster();
        object GetWCLBBatchRecordMaster();
        object GetWCLBMRSample();
        object GetWCLBMRSampleQcTest();
        object GetWCLBPRSample();
        object GetWCLBPRSampleQcTest();
        Task<PagedData<object>> LoadBatchRecordMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRSample(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRSampleQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBPRSample(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBPRSampleQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRSample(tbl_Qc_SampleProcessBMR tbl_Qc_SampleProcessBMR, string operation = "", string userName = "");
        Task<string> PostBMRSampleQcTest(tbl_Qc_SampleProcessBMR_QcTest tbl_Qc_SampleProcessBMR_QcTest, string operation = "", string userName = "");
        Task<string> PostBPRSample(tbl_Qc_SampleProcessBPR tbl_Qc_SampleProcessBPR, string operation = "", string userName = "");
        Task<string> PostBPRSampleQcTest(tbl_Qc_SampleProcessBPR_QcTest tbl_Qc_SampleProcessBPR_QcTest, string operation = "", string userName = "");
    }
    public interface IQcLab
    {
        Task<object> Get(int id);
        object GetWCLQcLab();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Qc_Lab tbl_Qc_Lab, string operation = "", string userName = "");
    }
    public interface IQcTest
    {
        Task<object> Get(int id);
        object GetWCLQcTest();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> Post(tbl_Qc_Test tbl_Qc_Test, string operation = "", string userName = "");
    }
    public interface ICompositionQcTest
    {
        Task<object> GetBMRProcess(int id);
        Task<object> GetBMRProcessQcTest(int id);
        Task<object> GetBPRProcess(int id);
        Task<object> GetBPRProcessQcTest(int id);
        Task<object> GetCompositionMaster(int id);
        Task<object> GetCompositionPackagingMaster(int id);
        object GetWCLBMRProcess();
        object GetWCLBMRProcessQcTest();
        object GetWCLBPRProcess();
        object GetWCLBPRProcessQcTest();
        object GetWCLCompositionMaster();
        object GetWCLCompositionPackagingMaster();
        Task<PagedData<object>> LoadBMRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRProcessQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBPRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBPRProcessQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadCompositionPackagingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRProcess(tbl_Pro_CompositionMaster_ProcessBMR tbl_Pro_CompositionMaster_ProcessBMR, string operation = "", string userName = "");
        Task<string> PostBMRProcessQcTest(tbl_Pro_CompositionMaster_ProcessBMR_QcTest tbl_Pro_CompositionMaster_ProcessBMR_QcTest, string operation = "", string userName = "");
        Task<string> PostBPRProcess(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR, string operation = "", string userName = "");
        Task<string> PostBPRProcessQcTest(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest, string operation = "", string userName = "");
    }
}
