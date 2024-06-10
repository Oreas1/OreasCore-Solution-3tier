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
        Task<object> GetBPRSample(int id);
        object GetWCLBatchRecordMaster();
        object GetWCLBBatchRecordMaster();
        object GetWCLBMRSample();
        object GetWCLBPRSample();
        Task<PagedData<object>> LoadBatchRecordMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBMRSample(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadBPRSample(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostBMRSample(tbl_Qc_SampleProcessBMR tbl_Qc_SampleProcessBMR, string operation = "", string userName = "");
        Task<string> PostBPRSample(tbl_Qc_SampleProcessBPR tbl_Qc_SampleProcessBPR, string operation = "", string userName = "");
    }
}
