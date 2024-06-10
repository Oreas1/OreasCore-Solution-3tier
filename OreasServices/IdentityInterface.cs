using OreasModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public interface IIdentityList
    {
        Task<object> GetAspNetOreasPriorityListAsync(string FilterByText = null, string FilterValueByText = null);
       
    }
    public interface IUser
    {
        object GetWCLUser();
        Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        Task<string> PostAsync(UserViewModel userViewModel, string operation, string userName);
        Task<object> GetAsync(string id);
        Task<string> GetUserIDAsync(string Name);
        Task<string> GetUserEmailSignatureAsync(string Name);

    }
    public interface IArea
    {
        Task<object> GetAreaListAsync(string FilterByText = null, string FilterValueByText = null);
        Task<object> GetFormsListAsync(string LoadByText = "", string LoadValueByText = "", string FilterByText = "", string FilterValueByText = "");
    }
    public interface IAuthorizationScheme
    {
        Task<object> GetAuthorizationSchemeListAsync(string FilterByText = null, string FilterValueByText = null);

        object GetWCLAuthorizationScheme();
        Task<PagedData<object>> LoadAuthorizationScheme(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);
        
        Task<string> PostAuthorizationSchemeAsync(AspNetOreasAuthorizationScheme aspNetOreasAuthorizationScheme, string operation, string userName);

        Task<object> GetAuthorizationSchemeAsync(int ID);

        #region WareHouse
        Task<object> GetAuthorizationSchemeSectionAsync(int ID);
        object GetWCLAuthorizationSchemeSection();
        Task<PagedData<object>> LoadAuthorizationSchemeSection(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostAuthorizationSchemeSectionAsync(AspNetOreasAuthorizationScheme_Section AspNetOreasAuthorizationScheme_Section, string operation, string userName);

        #endregion

        #region WareHouse
        Task<object> GetAuthorizationSchemeWHMAsync(int ID);
        object GetWCLAuthorizationSchemeWHM();
        Task<PagedData<object>> LoadAuthorizationSchemeWHM(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostAuthorizationSchemeWHMAsync(AspNetOreasAuthorizationScheme_WareHouse AspNetOreasAuthorizationScheme_WareHouse, string operation, string userName);
        Task<List<int>> GetAspNetOreasAuthorizedStoreListAsync(string UserName = "");
        #endregion

        #region Area

        object GetWCLAuthorizationSchemeArea();
        Task<PagedData<object>> LoadAuthorizationSchemeArea(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostAuthorizationSchemeAreaAsync(AspNetOreasAuthorizationScheme_Area aspNetOreasAuthorizationScheme_Area, string operation, string userName);

        Task<object> GetAuthorizationSchemeAreaAsync(int ID);
        #endregion

        #region Area Form

        object GetWCLAuthorizationSchemeAreaForm();
        Task<PagedData<object>> LoadAuthorizationSchemeAreaForm(int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null);

        Task<string> PostAuthorizationSchemeAreaFormAsync(AspNetOreasAuthorizationScheme_Area_Form aspNetOreasAuthorizationScheme_Area_Form, string operation, string userName);

        Task<object> GetAuthorizationSchemeAreaFormAsync(int ID);
        #endregion

        #region User Authorization
        Task<object> GetUserAuthorizatedOnOperationAsync(string Area = "", string UserName = "", string FormName = "");
        Task<object> GetUserAuthorizatedOnDashBoardAsync(string UserName = "", string DashBoardName = "");
        Task<object> GetUserAuthorizatedAreaListAsync(string UserName = "");
        Task<bool> IsUserAuthorizedDashBoardAsync(string UserName = "", string DashBoardName = "");

        #endregion

       
    }
    public interface IAspNetOreasCompanyProfile
    {
        Task<object> Load();
        Task<string> Post(AspNetOreasCompanyProfile AspNetOreasCompanyProfile, string operation, string userName);
    }
    public interface IAspNetOreasGeneralSettings
    {
        Task<object> Load();
        Task<string> Post(AspNetOreasGeneralSettings AspNetOreasGeneralSettings, string operation, string userName);

        Task<object> GetProductType(int id);
        object GetWCLProductType();
        Task<PagedData<object>> LoadProductType(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> PostProductType(tbl_Inv_ProductType tbl_Inv_ProductType, string operation = "", string userName = "");
    }
    public interface IDashBoard
    {
        Task<object> LoadCredentialsInfo(string identityUserName);
        Task<object> LoadUserInfo(string identityUserName);
        Task<string> PostChangedKey(string changedKey, string userId);
        Task<PagedData<object>> LoadSalary(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadSalaryStructure(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<PagedData<object>> LoadLoan(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<object> LoadAttendance(int EmpID = 0, int MonthID = 0);
        Task<object> LoadTeamAT(int _EmpID = 0, DateTime? Instance = null);
    }
    public interface IManagementDashBoard
    {
        Task<object> GetDashBoardData(string userName = "");

        #region Bank Doc
        object GetWCLBankDocument();
        object GetWCLDRBankDocument();
        Task<PagedData<object>> LoadBankDocument(int CurrentPage = 1, string IsFor = "", string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> SupervisedBankDocument(int ID, string userName = "");
        #endregion

        #region Cash Doc
        object GetWCLCashDocument();
        object GetWCLDRCashDocument();
        Task<PagedData<object>> LoadCashDocument(int CurrentPage = 1, string IsFor = "", string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> SupervisedCashDocument(int ID, string userName = "");
        #endregion

        #region Journal Doc
        object GetWCLJournalDocument();
        object GetWCLDRJournalDocument();
        Task<PagedData<object>> LoadJournalDocument(int CurrentPage = 1, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> SupervisedJournalDocument(int ID, string userName = "");
        #endregion

        #region Journal Doc2
        object GetWCLJournalDocument2();
        object GetWCLDRJournalDocument2();
        Task<PagedData<object>> LoadJournalDocument2(int CurrentPage = 1, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> SupervisedJournalDocument2(int ID, string userName = "");
        #endregion

        #region PurchaseNote
        object GetWCLPurchaseNote();
        Task<PagedData<object>> LoadPurchaseNote(int CurrentPage = 1, string IsFor = "", string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> SupervisedPurchaseNote(int ID, string userName = "");
        #endregion

        #region PurchaseReturnNote
        object GetWCLPurchaseReturnNote();
        Task<PagedData<object>> LoadPurchaseReturnNote(int CurrentPage = 1, string IsFor = "", string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> SupervisedPurchaseReturnNote(int ID, string userName = "");
        #endregion

        #region SalesNote
        object GetWCLSalesNote();
        Task<PagedData<object>> LoadSalesNote(int CurrentPage = 1, string IsFor = "", string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> SupervisedSalesNote(int ID, string userName = "");
        #endregion

        #region SalesReturnNote
        object GetWCLSalesReturnNote();
        Task<PagedData<object>> LoadSalesReturnNote(int CurrentPage = 1, string IsFor = "", string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null);
        Task<string> SupervisedSalesReturnNote(int ID, string userName = "");
        #endregion

    }
}
