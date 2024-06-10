using OreasModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public interface IPDRequest
    {
        Task<object> GetRequestDetailTR(int id);
        Task<object> GetRequestDetailTRCFP(int id);
        Task<object> GetRequestDetailTRCFPItem(int id);
        Task<object> GetRequestDetailTRProcedure(int id);
        Task<object> GetRequestMaster(int id);
        object GetWCLBRequestDetailTR();
        object GetWCLBRequestMaster();
        object GetWCLRequestDetailTR();
        object GetWCLRequestDetailTRCFP();
        object GetWCLRequestDetailTRCFPItem();
        object GetWCLRequestDetailTRProcedure();
        object GetWCLRequestMaster();
        Task<PagedData<object>> LoadRequestDetailTR(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadRequestDetailTRCFP(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadRequestDetailTRCFPItem(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadRequestDetailTRProcedure(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<PagedData<object>> LoadRequestMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "");
        Task<string> PostRequestDetailTR(tbl_PD_RequestDetailTR tbl_PD_RequestDetailTR, string operation = "", string userName = "");
        Task<string> PostRequestDetailTRCFP(tbl_PD_RequestDetailTR_CFP tbl_PD_RequestDetailTR_CFP, string operation = "", string userName = "");
        Task<string> PostRequestDetailTRCFPItem(tbl_PD_RequestDetailTR_CFP_Item tbl_PD_RequestDetailTR_CFP_Item, string operation = "", string userName = "");
        Task<string> PostRequestDetailTRProcedure(tbl_PD_RequestDetailTR_Procedure tbl_PD_RequestDetailTR_Procedure, string operation = "", string userName = "");
        Task<string> PostRequestMaster(tbl_PD_RequestMaster tbl_PD_RequestMaster, string operation = "", string userName = "");

        #region Report
        List<ReportCallingModel> GetRLTR();
        Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "");

        #endregion
    }

}
