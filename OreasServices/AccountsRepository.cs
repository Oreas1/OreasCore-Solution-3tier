using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Navigation;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OreasModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Border = iText.Layout.Borders.Border;
using iText.IO.Font.Cmap;
using iText.Kernel.Pdf;

namespace OreasServices
{    
    public class AccountsListRepository : IAccountsList
    {
        private readonly OreasDbContext db;
        public AccountsListRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetCOAListAsync(string QueryName = "", string COAFilterBy = "", string COAFilterValue = "", int FormID = 0)
        {

            var qry = from o in await db.VM_Ac_COASearchModals.FromSqlRaw("EXECUTE [dbo].[VM_Ac_COASearchModal] @QueryName={0}, @FilterBy={1}, @FilterValue={2}, @FormID={3}", QueryName, COAFilterBy, COAFilterValue, FormID).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.AccountCode,
                          o.AccountName
                      };
            return qry;
        }
        public async Task<object> GetCOATypeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_ChartOfAccounts_Types
                              select new
                              {
                                  a.ID,
                                  a.AccountType
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Ac_ChartOfAccounts_Types
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.AccountType.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.AccountType
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetPolicyWHTaxOnPurchaseListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_PolicyWHTaxOnPurchases
                              select new
                              {
                                  a.ID,
                                  WHTaxName = "[" + a.WHTaxPer.ToString() + "%] " + a.WHTaxName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Ac_PolicyWHTaxOnPurchases
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.WHTaxName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  WHTaxName = "[" + a.WHTaxPer.ToString() + "%] " + a.WHTaxName
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetPolicyWHTaxOnSalesListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_PolicyWHTaxOnSaless
                              select new
                              {
                                  a.ID,
                                  WHTaxName = "[" + a.WHTaxPer.ToString() + "%] " + a.WHTaxName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Ac_PolicyWHTaxOnSaless
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.WHTaxName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  WHTaxName = "[" + a.WHTaxPer.ToString() + "%] " + a.WHTaxName
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetPolicyPaymentTermListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_PolicyPaymentTerms.OrderBy(o => o.Name)
                              select new
                              {
                                  a.ID,
                                  Name = a.Name + "[DL:" + a.DaysLimit.ToString() + "][Ad%:" + a.AdvancePercentage.ToString() + "]"
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Ac_PolicyPaymentTerms.OrderBy(o => o.Name)
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.Name.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  Name = a.Name + "[DL:" + a.DaysLimit.ToString() + "][Ad%:" + a.AdvancePercentage.ToString() + "]"
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetBankTransactionModeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_V_BankTransactionModes
                              select new
                              {
                                  a.ID,
                                  a.BankTransactionMode
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Ac_V_BankTransactionModes
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.BankTransactionMode.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.BankTransactionMode
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetCashTransactionModeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_V_CashTransactionModes
                              select new
                              {
                                  a.ID,
                                  a.CashTransactionMode
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Ac_V_CashTransactionModes
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.CashTransactionMode.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.CashTransactionMode
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetClosingTypeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_FiscalYear_ClosingEntryTypes
                              select new
                              {
                                  a.ID,
                                  a.EntryName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Ac_FiscalYear_ClosingEntryTypes
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.EntryName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.EntryName
                              }).Take(5).ToListAsync();
        }
    }
    public class CurrencyAndCountryRepository : ICurrencyAndCountry
    {
        private readonly OreasDbContext db;
        public CurrencyAndCountryRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetCurrencyAndCountry(int id)
        {
            var qry = from o in await db.tbl_Ac_CurrencyAndCountrys.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.CountryName,
                          o.CurrencyCode,
                          o.CurrencySymbol,
                          o.IsDefault,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCurrencyAndCountry()
        {
            return new[]
            {
                new { n = "by Country Name", v = "byCountryName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCurrencyAndCountry(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_CurrencyAndCountrys
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byCountryName" && w.CountryName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_CurrencyAndCountrys
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byCountryName" && w.CountryName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.CountryName,
                          o.CurrencyCode,
                          o.CurrencySymbol,
                          o.IsDefault,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCurrencyAndCountry(tbl_Ac_CurrencyAndCountry tbl_Ac_CurrencyAndCountry, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_CurrencyAndCountry.CreatedBy = userName;
                tbl_Ac_CurrencyAndCountry.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_CurrencyAndCountry.ModifiedBy = userName;
                tbl_Ac_CurrencyAndCountry.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_CurrencyAndCountry] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@CountryName={4},@CurrencyCode={5},@CurrencySymbol={6},@IsDefault={7}
                ,@CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_CurrencyAndCountry.ID, tbl_Ac_CurrencyAndCountry.CountryName, tbl_Ac_CurrencyAndCountry.CurrencyCode, tbl_Ac_CurrencyAndCountry.CurrencySymbol, tbl_Ac_CurrencyAndCountry.IsDefault,
                tbl_Ac_CurrencyAndCountry.CreatedBy, tbl_Ac_CurrencyAndCountry.CreatedDate, tbl_Ac_CurrencyAndCountry.ModifiedBy, tbl_Ac_CurrencyAndCountry.ModifiedDate);


            if ((string)CRUD_Msg.Value == "Successful")
            {
                var fy = await db.tbl_Ac_FiscalYears.Where(f => f.IsClosed == false).OrderByDescending(d => d.PeriodStart).FirstOrDefaultAsync();
                if (fy != null)
                {
                    FiscalYear.Set(fy.PeriodStart, fy.PeriodEnd);
                }
                else
                {
                    FiscalYear.Set(null, null);
                }
                return "OK";
            }
            else
                return (string)CRUD_Msg.Value;
        }

    }
    public class FiscalYearRepository : IFiscalYear
    {
        private readonly OreasDbContext db;
        public FiscalYearRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region FiscalYear
        public async Task<object> GetFiscalYear(int id)
        {
            var qry = from o in await db.tbl_Ac_FiscalYears.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          PeriodStart = o.PeriodStart.ToString(),
                          PeriodEnd = o.PeriodEnd.ToString(),
                          o.IsClosed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLFiscalYear()
        {
            return new[]
            {
                new { n = "by Year", v = "byYear" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadFiscalYear(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_FiscalYears
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byYear" && w.PeriodStart.Year.ToString() == FilterValueByText.ToLower()
                                                       ||
                                                       FilterByText == "byYear" && w.PeriodEnd.Year.ToString() == FilterValueByText.ToLower()
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_FiscalYears
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byYear" && w.PeriodStart.Year.ToString() == FilterValueByText.ToLower()
                                        ||
                                        FilterByText == "byYear" && w.PeriodEnd.Year.ToString() == FilterValueByText.ToLower()
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          PeriodStart = o.PeriodStart.ToString("dd-MMM-yyyy"),
                          PeriodEnd = o.PeriodEnd.ToString("dd-MMM-yyyy"),
                          o.IsClosed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostFiscalYear(tbl_Ac_FiscalYear tbl_Ac_FiscalYear, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New" && tbl_Ac_FiscalYear.ID == 0)
            {
                tbl_Ac_FiscalYear.CreatedBy = userName;
                tbl_Ac_FiscalYear.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update" || (operation == "Save New" && tbl_Ac_FiscalYear.ID > 0))
            {
                tbl_Ac_FiscalYear.ModifiedBy = userName;
                tbl_Ac_FiscalYear.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_FiscalYear] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@PeriodStart={4},@PeriodEnd={5},@IsClosed={6}
                  ,@CreatedBy={7},@CreatedDate={8},@ModifiedBy={9},@ModifiedDate={10}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_FiscalYear.ID, tbl_Ac_FiscalYear.PeriodStart, tbl_Ac_FiscalYear.PeriodEnd, tbl_Ac_FiscalYear.IsClosed,
                tbl_Ac_FiscalYear.CreatedBy, tbl_Ac_FiscalYear.CreatedDate, tbl_Ac_FiscalYear.ModifiedBy, tbl_Ac_FiscalYear.ModifiedDate);


            if ((string)CRUD_Msg.Value == "Successful")
            {
                var fy = await db.tbl_Ac_FiscalYears.Where(f => f.IsClosed == false).OrderByDescending(d => d.PeriodStart).FirstOrDefaultAsync();
                if (fy != null)
                {
                    FiscalYear.Set(fy.PeriodStart, fy.PeriodEnd);
                }
                else
                {
                    FiscalYear.Set(null, null);
                }
                return "OK";
            }
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region ClosingMaster
        public async Task<object> GetClosingMaster(int id)
        {
            var qry = from o in await db.tbl_Ac_FiscalYear_ClosingMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_FiscalYear_ID,
                          o.FK_tbl_Ac_FiscalYear_ClosingEntryType_ID,
                          FK_tbl_Ac_FiscalYear_ClosingEntryType_IDName = o.tbl_Ac_FiscalYear_ClosingEntryType.EntryName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.TotalDebit,
                          o.TotalCredit,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLClosingMaster()
        {
            return new[]
            {
                new { n = "by Closing Type", v = "byClosingType" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadClosingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_FiscalYear_ClosingMasters.Where(w => w.FK_tbl_Ac_FiscalYear_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byClosingType" && w.tbl_Ac_FiscalYear_ClosingEntryType.EntryName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_FiscalYear_ClosingMasters.Where(w => w.FK_tbl_Ac_FiscalYear_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byClosingType" && w.tbl_Ac_FiscalYear_ClosingEntryType.EntryName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_FiscalYear_ID,
                          o.FK_tbl_Ac_FiscalYear_ClosingEntryType_ID,
                          FK_tbl_Ac_FiscalYear_ClosingEntryType_IDName = o.tbl_Ac_FiscalYear_ClosingEntryType.EntryName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.TotalDebit,
                          o.TotalCredit,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostClosingMaster(tbl_Ac_FiscalYear_ClosingMaster tbl_Ac_FiscalYear_ClosingMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_FiscalYear_ClosingMaster.ModifiedBy = userName;
                tbl_Ac_FiscalYear_ClosingMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            //else if (operation == "Save Update")
            //{
            //    tbl_Ac_FiscalYear_ClosingMaster.ModifiedBy = userName;
            //    tbl_Ac_FiscalYear_ClosingMaster.ModifiedDate = DateTime.Now;
            //    CRUD_Type.Value = "Update";
            //}
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_FiscalYear_ClosingMaster] 
                 @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Ac_FiscalYear_ID={4},@FK_tbl_Ac_FiscalYear_ClosingEntryType_ID={5}
                ,@FK_tbl_Ac_ChartOfAccounts_ID={6},@TotalDebit={7},@TotalCredit={8}
                ,@CreatedBy={9},@CreatedDate={10},@ModifiedBy={11},@ModifiedDate={12}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_FiscalYear_ClosingMaster.ID, tbl_Ac_FiscalYear_ClosingMaster.FK_tbl_Ac_FiscalYear_ID, tbl_Ac_FiscalYear_ClosingMaster.FK_tbl_Ac_FiscalYear_ClosingEntryType_ID,
                tbl_Ac_FiscalYear_ClosingMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_FiscalYear_ClosingMaster.TotalDebit, tbl_Ac_FiscalYear_ClosingMaster.TotalCredit,
                tbl_Ac_FiscalYear_ClosingMaster.CreatedBy, tbl_Ac_FiscalYear_ClosingMaster.CreatedDate, tbl_Ac_FiscalYear_ClosingMaster.ModifiedBy, tbl_Ac_FiscalYear_ClosingMaster.ModifiedDate);


            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region ClosingDetail
        public async Task<object> GetClosingDetail(int id)
        {
            var qry = from o in await db.tbl_Ac_FiscalYear_ClosingDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_FiscalYear_ClosingMaster_ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit,
                          o.Credit,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLClosingDetail()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadClosingDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_FiscalYear_ClosingDetails.Where(w => w.FK_tbl_Ac_FiscalYear_ClosingMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_FiscalYear_ClosingDetails.Where(w => w.FK_tbl_Ac_FiscalYear_ClosingMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_FiscalYear_ClosingMaster_ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit,
                          o.Credit,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostClosingDetail(tbl_Ac_FiscalYear_ClosingDetail tbl_Ac_FiscalYear_ClosingDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_FiscalYear_ClosingDetail.ModifiedBy = userName;
                tbl_Ac_FiscalYear_ClosingDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            //else if (operation == "Save Update")
            //{
            //    tbl_Ac_FiscalYear_ClosingDetail.ModifiedBy = userName;
            //    tbl_Ac_FiscalYear_ClosingDetail.ModifiedDate = DateTime.Now;
            //    CRUD_Type.Value = "Update";
            //}
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_FiscalYear_ClosingDetail] 
                 @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Ac_FiscalYear_ClosingMaster_ID={4}
                ,@FK_tbl_Ac_ChartOfAccounts_ID={5},@Debit={6},@Credit={7}
                ,@CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_FiscalYear_ClosingDetail.ID, tbl_Ac_FiscalYear_ClosingDetail.FK_tbl_Ac_FiscalYear_ClosingMaster_ID,
                tbl_Ac_FiscalYear_ClosingDetail.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_FiscalYear_ClosingDetail.Debit, tbl_Ac_FiscalYear_ClosingDetail.Credit,
                tbl_Ac_FiscalYear_ClosingDetail.CreatedBy, tbl_Ac_FiscalYear_ClosingDetail.CreatedDate, tbl_Ac_FiscalYear_ClosingDetail.ModifiedBy, tbl_Ac_FiscalYear_ClosingDetail.ModifiedDate);


            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

    }
    public class ChartOfAccountsTypeRepository : IChartOfAccountsType
    {
        private readonly OreasDbContext db;
       
        public ChartOfAccountsTypeRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Ac_ChartOfAccounts_Types.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.AccountType,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""                          
                      };

            return qry.FirstOrDefault();
        }

        public object GetWCLChartOfAccountsType()
        {
            return new[]
            {
                new { n = "by Account Type", v = "byAccountType" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_ChartOfAccounts_Types
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountType" && w.AccountType.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_ChartOfAccounts_Types
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountType" && w.AccountType.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.AccountType,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }

        public async Task<string> Post(tbl_Ac_ChartOfAccounts_Type tbl_Ac_ChartOfAccounts_Type, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Ac_ChartOfAccounts_Type.CreatedBy = userName;
                tbl_Ac_ChartOfAccounts_Type.CreatedDate = DateTime.Now;
                db.tbl_Ac_ChartOfAccounts_Types.Add(tbl_Ac_ChartOfAccounts_Type);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_ChartOfAccounts_Type.ModifiedBy = userName;
                tbl_Ac_ChartOfAccounts_Type.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Ac_ChartOfAccounts_Type).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Ac_ChartOfAccounts_Types.Remove(db.tbl_Ac_ChartOfAccounts_Types.Find(tbl_Ac_ChartOfAccounts_Type.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }   
    public class ChartOfAccountsRepository : IChartOfAccounts
    {
        private readonly OreasDbContext db;
        private readonly IAcLedger iAcLedger;
        public ChartOfAccountsRepository(OreasDbContext oreasDbContext, IAcLedger _IAcLedger)
        {
            this.db = oreasDbContext;
            iAcLedger = _IAcLedger;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Ac_ChartOfAccountss.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.ParentID,
                          ParentName = o.ParentID.HasValue ? db.tbl_Ac_ChartOfAccountss.Where(i => i.ID == o.ParentID).FirstOrDefault().AccountName : "",
                          o.FK_tbl_Ac_ChartOfAccounts_Type_ID,
                          o.AccountCode,
                          o.AccountName,
                          o.IsTransactional,
                          o.IsDiscontinue,
                          o.CompanyName,
                          o.Address,
                          o.NTN,
                          o.STR,
                          o.Telephone,
                          o.Mobile,
                          o.Fax,
                          o.Email,
                          o.ContactPersonName,
                          o.ContactPersonNumber,
                          o.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID,
                          FK_tbl_Ac_PolicyWHTaxOnPurchase_IDName = o.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID.HasValue ? o.tbl_Ac_PolicyWHTaxOnPurchase.WHTaxName : "",
                          o.FK_tbl_Ac_PolicyWHTaxOnSales_ID,
                          FK_tbl_Ac_PolicyWHTaxOnSales_IDName = o.FK_tbl_Ac_PolicyWHTaxOnSales_ID.HasValue ? o.tbl_Ac_PolicyWHTaxOnSales.WHTaxName : "",
                          o.FK_tbl_Ac_PolicyPaymentTerm_ID,
                          FK_tbl_Ac_PolicyPaymentTerm_IDName = o.FK_tbl_Ac_PolicyPaymentTerm_ID.HasValue ? o.tbl_Ac_PolicyPaymentTerm.Name + "[DL:" + o.tbl_Ac_PolicyPaymentTerm.DaysLimit.ToString() + "][Ad%:" + o.tbl_Ac_PolicyPaymentTerm.AdvancePercentage.ToString() + "]" : "",
                          o.Supplier_Approved,
                          o.Supplier_EvaluatedOn,
                          o.Supplier_EvaluationScore,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          ChildCount = o.tbl_Ac_ChartOfAccounts_Parents.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLChartOfAccounts()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_ChartOfAccountss
                                               .Where(w => (w.ParentID ?? 0) > 0)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_ChartOfAccountss
                                  .Where(w => (w.ParentID ?? 0) > 0)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.ParentID,
                          ParentName = o.ParentID.HasValue ? db.tbl_Ac_ChartOfAccountss.Where(i => i.ID == o.ParentID).FirstOrDefault().AccountName : "",
                          o.FK_tbl_Ac_ChartOfAccounts_Type_ID,
                          FK_tbl_Ac_ChartOfAccounts_Type_IDName = o.tbl_Ac_ChartOfAccounts_Type.AccountType,
                          o.AccountCode,
                          o.AccountName,
                          o.IsTransactional,
                          o.IsDiscontinue,
                          o.CompanyName,
                          o.Address,
                          o.NTN,
                          o.STR,
                          o.Telephone,
                          o.Mobile,
                          o.Fax,
                          o.Email,
                          o.ContactPersonName,
                          o.ContactPersonNumber,
                          o.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID,
                          FK_tbl_Ac_PolicyWHTaxOnPurchase_IDName = o.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID.HasValue ? o.tbl_Ac_PolicyWHTaxOnPurchase.WHTaxName + " [" + o.tbl_Ac_PolicyWHTaxOnPurchase.WHTaxPer + "%]" : "",
                          o.FK_tbl_Ac_PolicyWHTaxOnSales_ID,
                          FK_tbl_Ac_PolicyWHTaxOnSales_IDName = o.FK_tbl_Ac_PolicyWHTaxOnSales_ID.HasValue ? o.tbl_Ac_PolicyWHTaxOnSales.WHTaxName + " [" + o.tbl_Ac_PolicyWHTaxOnSales.WHTaxPer + "%]" : "",
                          o.FK_tbl_Ac_PolicyPaymentTerm_ID,
                          FK_tbl_Ac_PolicyPaymentTerm_IDName = o.FK_tbl_Ac_PolicyPaymentTerm_ID.HasValue ? o.tbl_Ac_PolicyPaymentTerm.Name + "[DL:" + o.tbl_Ac_PolicyPaymentTerm.DaysLimit.ToString() + "][Ad%:" + o.tbl_Ac_PolicyPaymentTerm.AdvancePercentage.ToString() + "]" : "",
                          o.Supplier_Approved,
                          o.Supplier_EvaluatedOn,
                          o.Supplier_EvaluationScore,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          ChildCount = o.tbl_Ac_ChartOfAccounts_Parents.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_ChartOfAccounts.ModifiedBy = userName;
                tbl_Ac_ChartOfAccounts.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_ChartOfAccounts.ModifiedBy = userName;
                tbl_Ac_ChartOfAccounts.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_ChartOfAccounts] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@ParentID={4},@FK_tbl_Ac_ChartOfAccounts_Type_ID={5}
              ,@AccountCode={6},@AccountName={7},@IsTransactional={8},@IsDiscontinue={9},@CompanyName={10}
              ,@Address={11},@NTN={12},@STR={13},@Telephone={14}
              ,@Mobile={15},@Fax={16},@Email={17},@ContactPersonName={18}
              ,@ContactPersonNumber={19},@FK_tbl_Ac_PolicyWHTaxOnPurchase_ID={20},@FK_tbl_Ac_PolicyWHTaxOnSales_ID={21},@FK_tbl_Ac_PolicyPaymentTerm_ID={22}
              ,@Supplier_Approved={23},@Supplier_EvaluatedOn={24},@Supplier_EvaluationScore={25}
              ,@CreatedBy={26},@CreatedDate={27},@ModifiedBy={28},@ModifiedDate={29}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_ChartOfAccounts.ID, tbl_Ac_ChartOfAccounts.ParentID, tbl_Ac_ChartOfAccounts.FK_tbl_Ac_ChartOfAccounts_Type_ID,
                tbl_Ac_ChartOfAccounts.AccountCode, tbl_Ac_ChartOfAccounts.AccountName, tbl_Ac_ChartOfAccounts.IsTransactional, tbl_Ac_ChartOfAccounts.IsDiscontinue, tbl_Ac_ChartOfAccounts.CompanyName,
                tbl_Ac_ChartOfAccounts.Address, tbl_Ac_ChartOfAccounts.NTN, tbl_Ac_ChartOfAccounts.STR, tbl_Ac_ChartOfAccounts.Telephone,
                tbl_Ac_ChartOfAccounts.Mobile, tbl_Ac_ChartOfAccounts.Fax, tbl_Ac_ChartOfAccounts.Email, tbl_Ac_ChartOfAccounts.ContactPersonName,
                tbl_Ac_ChartOfAccounts.ContactPersonNumber, tbl_Ac_ChartOfAccounts.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID, tbl_Ac_ChartOfAccounts.FK_tbl_Ac_PolicyWHTaxOnSales_ID, tbl_Ac_ChartOfAccounts.FK_tbl_Ac_PolicyPaymentTerm_ID,
                tbl_Ac_ChartOfAccounts.Supplier_Approved, tbl_Ac_ChartOfAccounts.Supplier_EvaluatedOn, tbl_Ac_ChartOfAccounts.Supplier_EvaluationScore,
                tbl_Ac_ChartOfAccounts.CreatedBy, tbl_Ac_ChartOfAccounts.CreatedDate, tbl_Ac_ChartOfAccounts.ModifiedBy, tbl_Ac_ChartOfAccounts.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        public async Task<object> GetNodesAsync(int PID = 0)
        {
            List<TreeView_ChartofAccounts> treenodes = new List<TreeView_ChartofAccounts>();
            if (PID >= 0)
            {
                var rootNodes = await db.tbl_Ac_ChartOfAccountss.Where(i => (i.ParentID ?? 0) == PID).ToListAsync();
                int _ChildCount = 0;
                foreach (var nodes in rootNodes)
                {
                    _ChildCount = await db.tbl_Ac_ChartOfAccountss.Where(i => i.ParentID == nodes.ID).CountAsync();

                    treenodes.Add(new TreeView_ChartofAccounts()
                    {
                        ID = nodes.ID,
                        AccountName = nodes.AccountName + " # " + nodes?.AccountCode ?? "",
                        ParentID = nodes.ParentID ?? 0,
                        ChildCount = _ChildCount,
                        spacing = "",
                        sign = (_ChildCount > 0) ? "+" : "",
                        IsParent = !nodes.IsTransactional
                    });
                }

            }
            return treenodes;
        }
        public async Task<string> COAUploadExcelFile(List<COAExcelData> COAExcelDataList, string operation, string userName)
        {
            if (operation == "Save New")
            {
                List<tbl_Ac_ChartOfAccounts> COADetails = new List<tbl_Ac_ChartOfAccounts>();

                foreach (var item in COAExcelDataList)
                {
                    if (!(item.ParentID > 0) || string.IsNullOrEmpty(item.AccountName) || !(item.AcTypeID>0))
                    {
                        continue;
                    }
                    COADetails.Add(
                        new tbl_Ac_ChartOfAccounts() 
                        { 
                            ID = 0, 
                            ParentID = item.ParentID,
                            AccountName = item.AccountName,
                            IsTransactional = true,
                            IsDiscontinue = false,
                            FK_tbl_Ac_ChartOfAccounts_Type_ID = item.AcTypeID,
                            FK_tbl_Ac_PolicyWHTaxOnPurchase_ID = item.WHTID,
                            FK_tbl_Ac_PolicyWHTaxOnSales_ID = item.WHTSalesID,
                            FK_tbl_Ac_PolicyPaymentTerm_ID = item.PayTermID,
                            CompanyName = item.CompanyName,
                            Address = item.Address,
                            NTN = item.NTN,
                            STR = item.STR,
                            Telephone = item.Telephone,
                            Mobile = item.Mobile,
                            Email = item.Email,
                            ContactPersonName = item.ContactPersonName,
                            CreatedBy = userName, 
                            CreatedDate = DateTime.Now 
                        });

                }

                //------------Add compiled record to database--------------------//
                SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
                SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
                SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                CRUD_Type.Value = "Insert";

                foreach (var coa in COADetails)
                {
                  await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_ChartOfAccounts] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@ParentID={4},@FK_tbl_Ac_ChartOfAccounts_Type_ID={5}
                  ,@AccountCode={6},@AccountName={7},@IsTransactional={8},@IsDiscontinue={9},@CompanyName={10}
                  ,@Address={11},@NTN={12},@STR={13},@Telephone={14}
                  ,@Mobile={15},@Fax={16},@Email={17},@ContactPersonName={18}
                  ,@ContactPersonNumber={19},@FK_tbl_Ac_PolicyWHTaxOnPurchase_ID={20},@FK_tbl_Ac_PolicyWHTaxOnSales_ID={21},@FK_tbl_Ac_PolicyPaymentTerm_ID={22}
                  ,@Supplier_Approved={23},@Supplier_EvaluatedOn={24},@Supplier_EvaluationScore={25}
                  ,@CreatedBy={26},@CreatedDate={27},@ModifiedBy={28},@ModifiedDate={29}",
                    CRUD_Type, CRUD_Msg, CRUD_ID,
                    coa.ID, coa.ParentID, coa.FK_tbl_Ac_ChartOfAccounts_Type_ID,
                    coa.AccountCode, coa.AccountName, coa.IsTransactional, coa.IsDiscontinue, coa.CompanyName,
                    coa.Address, coa.NTN, coa.STR, coa.Telephone,
                    coa.Mobile, coa.Fax, coa.Email, coa.ContactPersonName,
                    coa.ContactPersonNumber, coa.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID, coa.FK_tbl_Ac_PolicyWHTaxOnSales_ID, coa.FK_tbl_Ac_PolicyPaymentTerm_ID,
                    coa.Supplier_Approved, coa.Supplier_EvaluatedOn, coa.Supplier_EvaluationScore,
                    coa.CreatedBy, coa.CreatedDate, coa.ModifiedBy, coa.ModifiedDate);
                }


            }
            else
            {
                return "Wrong Operation";
            }

            return "OK";
        }

        #region Report   
        public List<ReportCallingModel> GetRLChartOfAccounts()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Accounts List",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = new List<string>(){ "ASSETS", "CAPITAL", "LIABILITIES", "EXPENSE", "REVENUE" }
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Payables OutStanding",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Receivables OutStanding",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Trial3",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Accounts List")
            {
                return await Task.Run(() => AccountsList(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Payables OutStanding")
            {
                return await Task.Run(() => PayablesOutStanding(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Receivables OutStanding")
            {
                return await Task.Run(() => ReceivablesOutStanding(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Trial3")
            {
                return await iAcLedger.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, GroupID, userName);
            }
            else if (rn == "TrialDetail")
            {
                return await iAcLedger.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, GroupID, userName);
            }
            else if (rn == "Ledger")
            {
                return await iAcLedger.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, GroupID, userName);
            }
            else if (rn == "Child Accounts List Excel")
            {
                return await Task.Run(() => ChildAccountsListExcel(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> ChildAccountsListExcel(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("COAChilds");

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = "EXECUTE [dbo].[Report_Ac_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                Sheet.Row(1).Height = 20; Sheet.Row(1).Style.Font.Bold = true; Sheet.Row(1).Style.Font.Size = 14;

                Sheet.Cells[1, 1].Value = "S.No";
                Sheet.Cells[1, 2].Value = "Ac Code";
                Sheet.Cells[1, 3].Value = "Ac Name";
                Sheet.Cells[1, 4].Value = "Ac Type";
                Sheet.Cells[1, 5].Value = "Parent / Child";
                Sheet.Cells[1, 6].Value = "Active";
                Sheet.Cells[1, 7].Value = "CompanyName";
                Sheet.Cells[1, 8].Value = "Address";
                Sheet.Cells[1, 9].Value = "NTN";
                Sheet.Cells[1, 10].Value = "STR";
                Sheet.Cells[1, 11].Value = "Telephone";
                Sheet.Cells[1, 12].Value = "Mobile";
                Sheet.Cells[1, 13].Value = "Email";
                Sheet.Cells[1, 14].Value = "ContactPersonName";
                Sheet.Cells[1, 15].Value = "ContactPersonNumber";
                Sheet.Cells[1, 16].Value = "WHT Policy";
                Sheet.Cells[1, 17].Value = "Payment Term Policy";

                for (int c = 1; c <= 17; c++)
                {
                    Sheet.Cells[1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[1, c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
                }

                int row = 2; int Sno = 1;
                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        Sheet.Row(row).Height = 18;
                        Sheet.Row(row).Style.Font.Size = 10;

                        Sheet.Cells[row, 1].Value = Sno;
                        Sheet.Cells[row, 2].Value = sqlReader["AccountCode"].ToString();
                        Sheet.Cells[row, 3].Value = sqlReader["AccountName"].ToString();
                        Sheet.Cells[row, 4].Value = sqlReader["AccountType"].ToString();
                        Sheet.Cells[row, 5].Value = sqlReader["IsTransactional"].ToString();
                        Sheet.Cells[row, 6].Value = sqlReader["Active"].ToString();
                        Sheet.Cells[row, 7].Value = sqlReader["CompanyName"].ToString();
                        Sheet.Cells[row, 8].Value = sqlReader["Address"].ToString();
                        Sheet.Cells[row, 9].Value = sqlReader["NTN"].ToString();
                        Sheet.Cells[row, 10].Value = sqlReader["STR"].ToString();
                        Sheet.Cells[row, 11].Value = sqlReader["Telephone"].ToString();
                        Sheet.Cells[row, 12].Value = sqlReader["Mobile"].ToString();
                        Sheet.Cells[row, 13].Value = sqlReader["Email"].ToString();
                        Sheet.Cells[row, 14].Value = sqlReader["ContactPersonName"].ToString();
                        Sheet.Cells[row, 15].Value = sqlReader["ContactPersonNumber"].ToString();
                        Sheet.Cells[row, 16].Value = sqlReader["WHTaxName"].ToString();
                        Sheet.Cells[row, 17].Value = sqlReader["PaymentTermName"].ToString();

                        row++;
                        Sno++;
                    }
                }

                Sheet.Cells[1, 1, row - 1, 17].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[1, 1, row - 1, 17].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[1, 1, row - 1, 17].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[1, 1, row - 1, 17].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                for (int i = 1; i <= 17; i++) {
                    Sheet.Column(i).BestFit = true;
                };               


                Sheet.Cells["A:AZ"].AutoFitColumns();
            }

            return Ep.GetAsByteArray();
        }
        private async Task<byte[]> AccountsList(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Accounts List by " + SeekBy + " -----", true);

            /////////////------------------------------table for Detail 2------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05), //
                        (float)(PageSize.A4.GetWidth() * 0.05), //
                        (float)(PageSize.A4.GetWidth() * 0.05), //
                        (float)(PageSize.A4.GetWidth() * 0.40), //
                        (float)(PageSize.A4.GetWidth() * 0.45) // 
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);
                                   

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();

                string A3 = "";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {

                        if (A3 != sqlReader["A3"].ToString())
                        {
                            A3 = sqlReader["A3"].ToString();

                            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["A1"].ToString())).SetBold().SetKeepTogether(true));

                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(" ")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add(sqlReader["A2"].ToString())).SetBold().SetKeepTogether(true));

                            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["A3"].ToString())).SetBold().SetKeepTogether(true));

                        }
                        pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add(" ")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["A4"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["A5"].ToString())).SetKeepTogether(true));

                    }

                }

            }

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> ReceivablesOutStanding(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Receivables as On " + DateTime.Now.ToString("dd-MMM-yy hh:mm tt") + " -----", true);

            /////////////------------------------------Main table------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05), // SNo
                        (float)(PageSize.A4.GetWidth() * 0.30), // Account Name
                        (float)(PageSize.A4.GetWidth() * 0.15), // Payment term
                        (float)(PageSize.A4.GetWidth() * 0.10), // 91<
                        (float)(PageSize.A4.GetWidth() * 0.10), // 61-90
                        (float)(PageSize.A4.GetWidth() * 0.10), // 31-60
                        (float)(PageSize.A4.GetWidth() * 0.10), // 30-01
                        (float)(PageSize.A4.GetWidth() * 0.10) // Balance
                }
           ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddCell(new Cell(1,3).Add(new Paragraph().Add("")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Payment due Period In Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add("")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("S.No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Customer Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Payment Terms")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Over 90 Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(255, 192, 203)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("61-90 Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(255, 255, 153)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("31-60 Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("01-30 Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Total Balance")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            double Total_91ToEnd = 0, Total_61To90 = 0, Total_31To60 = 0, Total_1To30 = 0, TotalBalance = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();

                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AcName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PaymentTerm"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        if (Convert.ToDouble(sqlReader["O91_End"]) > 0)
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O91_End"].ToString())).SetBackgroundColor(new DeviceRgb(255, 192, 203)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        else
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O91_End"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        if (Convert.ToDouble(sqlReader["O61_90"]) > 0)
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O61_90"].ToString())).SetBackgroundColor(new DeviceRgb(255, 255, 153)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        else
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O61_90"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O31_60"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O1_30"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Balance"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        Total_91ToEnd += Convert.ToDouble(sqlReader["O91_End"]);
                        Total_61To90 += Convert.ToDouble(sqlReader["O61_90"]);
                        Total_31To60 += Convert.ToDouble(sqlReader["O31_60"]);
                        Total_1To30 += Convert.ToDouble(sqlReader["O1_30"]);
                        TotalBalance += Convert.ToDouble(sqlReader["Balance"]);

                        SNo++;
                    }

                }

            }

            pdftableMain.AddCell(new Cell(1,3).Add(new Paragraph().Add("Total")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Total_91ToEnd.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(255, 192, 203)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Total_61To90.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(255, 255, 153)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Total_31To60.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Total_1To30.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(TotalBalance.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> PayablesOutStanding(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Payables as On " + DateTime.Now.ToString("dd-MMM-yy hh:mm tt") + " -----", true);

            /////////////------------------------------Main table------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05), // SNo
                        (float)(PageSize.A4.GetWidth() * 0.30), // Account Name
                        (float)(PageSize.A4.GetWidth() * 0.15), // Payment term
                        (float)(PageSize.A4.GetWidth() * 0.10), // 91<
                        (float)(PageSize.A4.GetWidth() * 0.10), // 61-90
                        (float)(PageSize.A4.GetWidth() * 0.10), // 31-60
                        (float)(PageSize.A4.GetWidth() * 0.10), // 30-01
                        (float)(PageSize.A4.GetWidth() * 0.10) // Balance
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Payment due Period In Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add("")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("S.No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Supplier Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Payment Terms")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Over 90 Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(255, 192, 203)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("61-90 Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(255, 255, 153)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("31-60 Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("01-30 Days")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Total Balance")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            double Total_91ToEnd = 0, Total_61To90 = 0, Total_31To60 = 0, Total_1To30 = 0, TotalBalance = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();

                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AcName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PaymentTerm"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        if (Convert.ToDouble(sqlReader["O91_End"]) > 0)
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O91_End"].ToString())).SetBackgroundColor(new DeviceRgb(255, 192, 203)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        else
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O91_End"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        if (Convert.ToDouble(sqlReader["O61_90"]) > 0)
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O61_90"].ToString())).SetBackgroundColor(new DeviceRgb(255, 255, 153)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        else
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O61_90"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O31_60"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["O1_30"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Balance"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        Total_91ToEnd += Convert.ToDouble(sqlReader["O91_End"]);
                        Total_61To90 += Convert.ToDouble(sqlReader["O61_90"]);
                        Total_31To60 += Convert.ToDouble(sqlReader["O31_60"]);
                        Total_1To30 += Convert.ToDouble(sqlReader["O1_30"]);
                        TotalBalance += Convert.ToDouble(sqlReader["Balance"]);

                        SNo++;
                    }

                }

            }

            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Total")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Total_91ToEnd.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(255, 192, 203)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Total_61To90.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(255, 255, 153)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Total_31To60.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Total_1To30.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(TotalBalance.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
       
        #endregion

    }
    public class CompositionCostingFactorsRepository : ICompositionCostingFactors
    {
        private readonly OreasDbContext db;
        public CompositionCostingFactorsRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Ac_CompositionCostingFactorss.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FormulaName,
                          o.FormulaExpression,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCL()
        {
            return new[]
            {
                new { n = "by Formula Name", v = "byFormulaName" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_CompositionCostingFactorss
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byFormulaName" && w.FormulaName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_CompositionCostingFactorss
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byFormulaName" && w.FormulaName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FormulaName,
                          o.FormulaExpression,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Ac_CompositionCostingFactors tbl_Ac_CompositionCostingFactors, string operation = "", string userName = "")
        {
            
            if (operation == "Save New")
            {
                if (ValidExpression(tbl_Ac_CompositionCostingFactors.FormulaExpression) == false)
                    return "Invalid Expression";

                tbl_Ac_CompositionCostingFactors.CreatedBy = userName;
                tbl_Ac_CompositionCostingFactors.CreatedDate = DateTime.Now;
                db.tbl_Ac_CompositionCostingFactorss.Add(tbl_Ac_CompositionCostingFactors);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                if (ValidExpression(tbl_Ac_CompositionCostingFactors.FormulaExpression) == false)
                    return "Invalid Expression";

                tbl_Ac_CompositionCostingFactors.ModifiedBy = userName;
                tbl_Ac_CompositionCostingFactors.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Ac_CompositionCostingFactors).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Ac_CompositionCostingFactorss.Remove(db.tbl_Ac_CompositionCostingFactorss.Find(tbl_Ac_CompositionCostingFactors.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        public bool ValidExpression(string expression)
        {
            try
            {
                expression = expression.Replace("c","1");
                expression = expression.Replace("C", "1");

                DataTable table = new DataTable();
                // Use the Compute method to evaluate the expression
                object result = table.Compute(expression, "");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
  
        }
    }
    public class CustomerApprovedRateListRepository : ICustomerApprovedRateList
    {
        private readonly OreasDbContext db;
        public CustomerApprovedRateListRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public object GetWCLCustomerRateListMaster()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCustomerRateListMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_ChartOfAccountss
                                               .Where(w => w.tbl_Ac_ChartOfAccounts_Type.AccountType.Contains("Customer"))
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_ChartOfAccountss
                                  .Where(w => w.tbl_Ac_ChartOfAccounts_Type.AccountType.Contains("Customer"))
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.AccountName,
                          o.AccountCode,
                          ParentAccountName = o.tbl_Ac_ChartOfAccounts_Parent.AccountName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalProducts = o.tbl_Ac_CustomerApprovedRateLists.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        #endregion

        #region Detail
        public async Task<object> GetCustomerRateListDetail(int id)
        {
            var qry = from o in await db.tbl_Ac_CustomerApprovedRateLists.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.Split_Into.ToString() + "'s]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Rate,
                          AppliedDate = o.AppliedDate.ToString("dd-MMM-yy"),
                          o.PreviousRate,
                          PreviousAppliedDate = o.PreviousAppliedDate.ToString("dd-MMM-yy"),
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCustomerRateListDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCustomerRateListDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_CustomerApprovedRateLists
                                               .Where(w => w.FK_tbl_Ac_ChartOfAccounts_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_CustomerApprovedRateLists
                                  .Where(w => w.FK_tbl_Ac_ChartOfAccounts_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.Split_Into.ToString() + "'s]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Rate,
                          AppliedDate = o.AppliedDate.ToString("dd-MMM-yy"),
                          o.PreviousRate,
                          PreviousAppliedDate = o.PreviousAppliedDate.ToString("dd-MMM-yy"),
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCustomerRateListDetail(tbl_Ac_CustomerApprovedRateList tbl_Ac_CustomerApprovedRateList, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_CustomerApprovedRateList.CreatedBy = userName;
                tbl_Ac_CustomerApprovedRateList.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_CustomerApprovedRateList.ModifiedBy = userName;
                tbl_Ac_CustomerApprovedRateList.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_CustomerApprovedRateList] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Ac_ChartOfAccounts_ID={4},@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
                  ,@Rate={6},@AppliedDate={7},@PreviousRate={8},@PreviousAppliedDate={9},@Remarks={10}
                  ,@CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_CustomerApprovedRateList.ID, tbl_Ac_CustomerApprovedRateList.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_CustomerApprovedRateList.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Ac_CustomerApprovedRateList.Rate, tbl_Ac_CustomerApprovedRateList.AppliedDate, tbl_Ac_CustomerApprovedRateList.PreviousRate, tbl_Ac_CustomerApprovedRateList.PreviousAppliedDate, tbl_Ac_CustomerApprovedRateList.Remarks,
                tbl_Ac_CustomerApprovedRateList.CreatedBy, tbl_Ac_CustomerApprovedRateList.CreatedDate, tbl_Ac_CustomerApprovedRateList.ModifiedBy, tbl_Ac_CustomerApprovedRateList.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

    }
    public class AcPolicyInventoryRepository : IAcPolicyInventory
    {
        private readonly OreasDbContext db;
        public AcPolicyInventoryRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Ac_PolicyInventorys.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductType_ID,
                          FK_tbl_Inv_ProductType_IDName = o.tbl_Inv_ProductType.ProductType,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Inv,
                          FK_tbl_Ac_ChartOfAccounts_ID_InvName = o.tbl_Ac_ChartOfAccounts_Inv.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_COGS,
                          FK_tbl_Ac_ChartOfAccounts_ID_COGSName = o.tbl_Ac_ChartOfAccounts_COGS.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Expense,                          
                          FK_tbl_Ac_ChartOfAccounts_ID_ExpenseName = o.tbl_Ac_ChartOfAccounts_Expense.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_InProcess,
                          FK_tbl_Ac_ChartOfAccounts_ID_InProcessName = o.tbl_Ac_ChartOfAccounts_InProcess.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_WHT_Purchase,
                          FK_tbl_Ac_ChartOfAccounts_ID_WHT_PurchaseName = o.tbl_Ac_ChartOfAccounts_WHT_Purchase.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_GST_Purchase,
                          FK_tbl_Ac_ChartOfAccounts_ID_GST_PurchaseName = o.tbl_Ac_ChartOfAccounts_GST_Purchase.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_WHT_Sales,
                          FK_tbl_Ac_ChartOfAccounts_ID_WHT_SalesName = o.tbl_Ac_ChartOfAccounts_WHT_Sales.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_ST_Sales,
                          FK_tbl_Ac_ChartOfAccounts_ID_ST_SalesName = o.tbl_Ac_ChartOfAccounts_ST_Sales.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_FST_Sales,
                          FK_tbl_Ac_ChartOfAccounts_ID_FST_SalesName = o.tbl_Ac_ChartOfAccounts_FST_Sales.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Sales,
                          FK_tbl_Ac_ChartOfAccounts_ID_SalesName = o.tbl_Ac_ChartOfAccounts_Sales.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTR,
                          FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTRName = o.tbl_Ac_ChartOfAccounts_ExpenseTR.AccountName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCL()
        {
            return new[]
            {
                new { n = "by Product Type", v = "byProductType" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_PolicyInventorys
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductType" && w.tbl_Inv_ProductType.ProductType.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_PolicyInventorys
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductType" && w.tbl_Inv_ProductType.ProductType.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductType_ID,
                          FK_tbl_Inv_ProductType_IDName = o.tbl_Inv_ProductType.ProductType,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Inv,
                          FK_tbl_Ac_ChartOfAccounts_ID_InvName = o.tbl_Ac_ChartOfAccounts_Inv.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_COGS,
                          FK_tbl_Ac_ChartOfAccounts_ID_COGSName = o.tbl_Ac_ChartOfAccounts_COGS.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Expense,
                          FK_tbl_Ac_ChartOfAccounts_ID_ExpenseName = o.tbl_Ac_ChartOfAccounts_Expense.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_InProcess,
                          FK_tbl_Ac_ChartOfAccounts_ID_InProcessName = o.tbl_Ac_ChartOfAccounts_InProcess.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_WHT_Purchase,
                          FK_tbl_Ac_ChartOfAccounts_ID_WHT_PurchaseName = o.tbl_Ac_ChartOfAccounts_WHT_Purchase.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_GST_Purchase,
                          FK_tbl_Ac_ChartOfAccounts_ID_GST_PurchaseName = o.tbl_Ac_ChartOfAccounts_GST_Purchase.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_WHT_Sales,
                          FK_tbl_Ac_ChartOfAccounts_ID_WHT_SalesName = o.tbl_Ac_ChartOfAccounts_WHT_Sales.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_ST_Sales,
                          FK_tbl_Ac_ChartOfAccounts_ID_ST_SalesName = o.tbl_Ac_ChartOfAccounts_ST_Sales.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_FST_Sales,
                          FK_tbl_Ac_ChartOfAccounts_ID_FST_SalesName = o.tbl_Ac_ChartOfAccounts_FST_Sales.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Sales,
                          FK_tbl_Ac_ChartOfAccounts_ID_SalesName = o.tbl_Ac_ChartOfAccounts_Sales.AccountName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTR,
                          FK_tbl_Ac_ChartOfAccounts_ID_ExpenseTRName = o.tbl_Ac_ChartOfAccounts_ExpenseTR.AccountName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Ac_PolicyInventory tbl_Ac_PolicyInventory, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Ac_PolicyInventory.CreatedBy = userName;
                tbl_Ac_PolicyInventory.CreatedDate = DateTime.Now;
                db.tbl_Ac_PolicyInventorys.Add(tbl_Ac_PolicyInventory);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_PolicyInventory.ModifiedBy = userName;
                tbl_Ac_PolicyInventory.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Ac_PolicyInventory).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Ac_PolicyInventorys.Remove(db.tbl_Ac_PolicyInventorys.Find(tbl_Ac_PolicyInventory.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class AcPolicyWHTaxOnPurchaseRepository : IAcPolicyWHTaxOnPurchase
    {
        private readonly OreasDbContext db;
        public AcPolicyWHTaxOnPurchaseRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetAcPolicyWHTaxOnPurchaseMaster(int id)
        {
            var qry = from o in await db.tbl_Ac_PolicyWHTaxOnPurchases.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.WHTaxName,
                          o.WHTaxPer,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLAcPolicyWHTaxOnPurchaseMaster()
        {
            return new[]
            {
                new { n = "by Account", v = "byAccount" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadAcPolicyWHTaxOnPurchaseMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_PolicyWHTaxOnPurchases
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccount" && w.tbl_Ac_ChartOfAccountss.Any(a => a.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_PolicyWHTaxOnPurchases
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccount" && w.tbl_Ac_ChartOfAccountss.Any(a => a.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.WHTaxName,
                          o.WHTaxPer,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalAc = o.tbl_Ac_ChartOfAccountss.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostAcPolicyWHTaxOnPurchaseMaster(tbl_Ac_PolicyWHTaxOnPurchase tbl_Ac_PolicyWHTaxOnPurchase, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Ac_PolicyWHTaxOnPurchase.CreatedBy = userName;
                tbl_Ac_PolicyWHTaxOnPurchase.CreatedDate = DateTime.Now;
                db.tbl_Ac_PolicyWHTaxOnPurchases.Add(tbl_Ac_PolicyWHTaxOnPurchase);
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Update")
            {
                tbl_Ac_PolicyWHTaxOnPurchase.ModifiedBy = userName;
                tbl_Ac_PolicyWHTaxOnPurchase.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Ac_PolicyWHTaxOnPurchase).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Ac_PolicyWHTaxOnPurchases.Remove(db.tbl_Ac_PolicyWHTaxOnPurchases.Find(tbl_Ac_PolicyWHTaxOnPurchase.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        #endregion

        #region Detail
        public object GetWCLAcPolicyWHTaxOnPurchaseDetail()
        {
            return new[]
            {
                new { n = "by Account", v = "byAccount" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadAcPolicyWHTaxOnPurchaseDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_ChartOfAccountss
                                               .Where(w => w.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccount" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_ChartOfAccountss
                                  .Where(w => w.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccount" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID,
                          o.ParentID,
                          ParentName = o.ParentID.HasValue ? db.tbl_Ac_ChartOfAccountss.Where(i => i.ID == o.ParentID).FirstOrDefault().AccountName : "",
                          o.FK_tbl_Ac_ChartOfAccounts_Type_ID,
                          FK_tbl_Ac_ChartOfAccounts_Type_IDName = o.tbl_Ac_ChartOfAccounts_Type.AccountType,
                          o.AccountCode,
                          o.AccountName,
                          o.IsTransactional
                      };




            pageddata.Data = qry;

            return pageddata;
        }

        #endregion
    }
    public class AcPolicyWHTaxOnSalesRepository : IAcPolicyWHTaxOnSales
    {
        private readonly OreasDbContext db;
        public AcPolicyWHTaxOnSalesRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetAcPolicyWHTaxOnSalesMaster(int id)
        {
            var qry = from o in await db.tbl_Ac_PolicyWHTaxOnSaless.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.WHTaxName,
                          o.WHTaxPer,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLAcPolicyWHTaxOnSalesMaster()
        {
            return new[]
            {
                new { n = "by Account", v = "byAccount" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadAcPolicyWHTaxOnSalesMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_PolicyWHTaxOnSaless
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccount" && w.tbl_Ac_ChartOfAccountss.Any(a => a.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_PolicyWHTaxOnSaless
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccount" && w.tbl_Ac_ChartOfAccountss.Any(a => a.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.WHTaxName,
                          o.WHTaxPer,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalAc = o.tbl_Ac_ChartOfAccountss.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostAcPolicyWHTaxOnSalesMaster(tbl_Ac_PolicyWHTaxOnSales tbl_Ac_PolicyWHTaxOnSales, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Ac_PolicyWHTaxOnSales.CreatedBy = userName;
                tbl_Ac_PolicyWHTaxOnSales.CreatedDate = DateTime.Now;
                db.tbl_Ac_PolicyWHTaxOnSaless.Add(tbl_Ac_PolicyWHTaxOnSales);
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Update")
            {
                tbl_Ac_PolicyWHTaxOnSales.ModifiedBy = userName;
                tbl_Ac_PolicyWHTaxOnSales.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Ac_PolicyWHTaxOnSales).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Ac_PolicyWHTaxOnSaless.Remove(db.tbl_Ac_PolicyWHTaxOnSaless.Find(tbl_Ac_PolicyWHTaxOnSales.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        #endregion

        #region Detail
        public object GetWCLAcPolicyWHTaxOnSalesDetail()
        {
            return new[]
            {
                new { n = "by Account", v = "byAccount" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadAcPolicyWHTaxOnSalesDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_ChartOfAccountss
                                               .Where(w => w.FK_tbl_Ac_PolicyWHTaxOnSales_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccount" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_ChartOfAccountss
                                  .Where(w => w.FK_tbl_Ac_PolicyWHTaxOnSales_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccount" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_PolicyWHTaxOnSales_ID,
                          o.ParentID,
                          ParentName = o.ParentID.HasValue ? db.tbl_Ac_ChartOfAccountss.Where(i => i.ID == o.ParentID).FirstOrDefault().AccountName : "",
                          o.FK_tbl_Ac_ChartOfAccounts_Type_ID,
                          FK_tbl_Ac_ChartOfAccounts_Type_IDName = o.tbl_Ac_ChartOfAccounts_Type.AccountType,
                          o.AccountCode,
                          o.AccountName,
                          o.IsTransactional
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion
    }
    public class BankDocumentRepository : IBankDocument
    {
        private readonly OreasDbContext db;
        public BankDocumentRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetBankDocumentMaster(int id)
        {
            var qry = from o in await db.tbl_Ac_V_BankDocumentMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          VoucherDate = o.VoucherDate.ToString("dd-MMM-yyyy") ?? "",
                          o.VoucherNo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit1_Credit0,
                          o.IsSupervisedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""                                                   
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBankDocumentMaster()
        {
            return new[]
            {
                new { n = "by Account From", v = "byAccountFrom" }, new { n = "by Account To", v = "byAccountTo" }, new { n = "by Voucher No", v = "byVoucherNo" }
            }.ToList();
        }
        public object GetWCLBBankDocumentMaster()
        {
            return new[]
            {
                new { n = "by Cleared", v = "byCleared" }, new { n = "by Cancelled", v = "byCancelled" }, new { n = "by Pending", v = "byPending" }, new { n = "by Not Supervised", v = "byNotSupervised" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBankDocumentMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string IsFor = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_V_BankDocumentMasters
                                               .Where(w=>
                                                        string.IsNullOrEmpty(IsFor) && w.ID == 0
                                                        ||
                                                        IsFor == "Receive" && w.Debit1_Credit0 == true
                                                        ||
                                                        IsFor == "Payment" && w.Debit1_Credit0 == false
                                                        )
                                               .Where(w =>
                                                        string.IsNullOrEmpty(FilterByLoad)
                                                        ||
                                                        FilterByLoad == "byPending" && w.tbl_Ac_V_BankDocumentDetails.Any(a => a.Cleared1_Cancelled0.HasValue == false)
                                                        ||
                                                        FilterByLoad == "byCleared" && w.tbl_Ac_V_BankDocumentDetails.Any(a=> a.Cleared1_Cancelled0.Value == true)
                                                        ||
                                                        FilterByLoad == "byCancelled" && w.tbl_Ac_V_BankDocumentDetails.Any(a => a.Cleared1_Cancelled0.Value == false)
                                                        ||
                                                        FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                     )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountFrom" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byAccountTo" && w.tbl_Ac_V_BankDocumentDetails.Any(a => a.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byVoucherNo" && w.VoucherNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_V_BankDocumentMasters
                                  .Where(w =>
                                            string.IsNullOrEmpty(IsFor) && w.ID == 0
                                            ||
                                            IsFor == "Receive" && w.Debit1_Credit0 == true
                                            ||
                                            IsFor == "Payment" && w.Debit1_Credit0 == false
                                            )
                                  .Where(w =>
                                             string.IsNullOrEmpty(FilterByLoad)
                                             ||
                                             FilterByLoad == "byPending" && w.tbl_Ac_V_BankDocumentDetails.Any(a => a.Cleared1_Cancelled0.HasValue == false)
                                             ||
                                             FilterByLoad == "byCleared" && w.tbl_Ac_V_BankDocumentDetails.Any(a => a.Cleared1_Cancelled0.Value == true)
                                             ||
                                             FilterByLoad == "byCancelled" && w.tbl_Ac_V_BankDocumentDetails.Any(a => a.Cleared1_Cancelled0.Value == false)
                                             ||
                                             FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                          )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountFrom" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byAccountTo" && w.tbl_Ac_V_BankDocumentDetails.Any(a => a.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byVoucherNo" && w.VoucherNo.ToString() == FilterValueByText
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          VoucherDate = o.VoucherDate.ToString("dd-MMM-yyyy") ?? "",
                          o.VoucherNo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit1_Credit0,
                          o.IsSupervisedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          Total = (double?)o.tbl_Ac_V_BankDocumentDetails.Sum(s => s.Amount) ?? 0
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBankDocumentMaster(tbl_Ac_V_BankDocumentMaster tbl_Ac_V_BankDocumentMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_V_BankDocumentMaster.CreatedBy = userName;
                tbl_Ac_V_BankDocumentMaster.CreatedDate = DateTime.Now;
                //db.tbl_Ac_V_BankDocumentMasters.Add(tbl_Ac_V_BankDocumentMaster);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_V_BankDocumentMaster.ModifiedBy = userName;
                tbl_Ac_V_BankDocumentMaster.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Ac_V_BankDocumentMaster).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Ac_V_BankDocumentMasters.Remove(db.tbl_Ac_V_BankDocumentMasters.Find(tbl_Ac_V_BankDocumentMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_BankDocumentMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
                @ID={3},@VoucherNo={4},@VoucherDate={5},@FK_tbl_Ac_ChartOfAccounts_ID={6},@Debit1_Credit0={7},
                @CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_V_BankDocumentMaster.ID, tbl_Ac_V_BankDocumentMaster.VoucherNo, tbl_Ac_V_BankDocumentMaster.VoucherDate, tbl_Ac_V_BankDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_V_BankDocumentMaster.Debit1_Credit0,
                tbl_Ac_V_BankDocumentMaster.CreatedBy, tbl_Ac_V_BankDocumentMaster.CreatedDate, tbl_Ac_V_BankDocumentMaster.ModifiedBy, tbl_Ac_V_BankDocumentMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetBankDocumentDetail(int id)
        {
            var qry = from o in await db.tbl_Ac_V_BankDocumentDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_V_BankDocumentMaster_ID,
                          o.FK_tbl_Ac_V_BankTransactionMode_ID,
                          FK_tbl_Ac_V_BankTransactionMode_IDName = o.tbl_Ac_V_BankTransactionMode.BankTransactionMode,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          PostingDate = o.PostingDate.ToString() ?? "",
                          o.InstrumentNo,
                          InstrumentDate = o.InstrumentDate.ToString() ?? "",
                          o.Narration,
                          o.Amount,                          
                          o.Cleared1_Cancelled0,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_For,
                          FK_tbl_Ac_ChartOfAccounts_ID_ForName = o?.tbl_Ac_ChartOfAccounts_For?.AccountName ?? "",
                          o.IsSupervised,
                          o.SupervisedBy,
                          SupervisedDate = o.SupervisedDate.HasValue ? o.SupervisedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBankDocumentDetail()
        {
            return new[]
            {
                new { n = "by Account To", v = "byAccountTo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBankDocumentDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_V_BankDocumentDetails
                                               .Where(w => w.FK_tbl_Ac_V_BankDocumentMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountTo" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_V_BankDocumentDetails
                                  .Where(w => w.FK_tbl_Ac_V_BankDocumentMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountTo" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_V_BankDocumentMaster_ID,
                          o.FK_tbl_Ac_V_BankTransactionMode_ID,
                          FK_tbl_Ac_V_BankTransactionMode_IDName = o.tbl_Ac_V_BankTransactionMode.BankTransactionMode,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          PostingDate = o.PostingDate.ToString("dd-MMM-yyyy hh:mm tt") ?? "",
                          o.InstrumentNo,
                          InstrumentDate = o.InstrumentDate.ToString("dd-MMM-yyyy hh:mm tt") ?? "",
                          o.Narration,
                          o.Amount,
                          o.Cleared1_Cancelled0,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_For,
                          FK_tbl_Ac_ChartOfAccounts_ID_ForName = o?.tbl_Ac_ChartOfAccounts_For?.AccountName ?? "",
                          o.IsSupervised,
                          o.SupervisedBy,
                          SupervisedDate = o.SupervisedDate.HasValue ? o.SupervisedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBankDocumentDetail(tbl_Ac_V_BankDocumentDetail tbl_Ac_V_BankDocumentDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            { 
                //-----------------new record--------------//
                if (tbl_Ac_V_BankDocumentDetail.ID == 0)
                {
                    tbl_Ac_V_BankDocumentDetail.CreatedBy = userName;
                    tbl_Ac_V_BankDocumentDetail.CreatedDate = DateTime.Now;
                    CRUD_Type.Value = "Insert";
                }
                //-----------------Old record posting only account to & posting status--------------//
                else if (tbl_Ac_V_BankDocumentDetail.ID > 0)
                {
                    tbl_Ac_V_BankDocumentDetail.ModifiedBy = userName;
                    tbl_Ac_V_BankDocumentDetail.ModifiedDate = DateTime.Now;
                    CRUD_Type.Value = "InsertPosting";
                }
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_V_BankDocumentDetail.ModifiedBy = userName;
                tbl_Ac_V_BankDocumentDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_BankDocumentDetail] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Ac_V_BankDocumentMaster_ID={4},@FK_tbl_Ac_V_BankTransactionMode_ID={5}
                  ,@FK_tbl_Ac_ChartOfAccounts_ID={6},@PostingDate={7}
                  ,@InstrumentNo={8},@InstrumentDate={9},@Narration={10},@Amount={11}
                  ,@Cleared1_Cancelled0={12},@FK_tbl_Ac_ChartOfAccounts_ID_For={13}
                  ,@IsSupervised={14},@SupervisedBy={15},@SupervisedDate={16}
                  ,@CreatedBy={17},@CreatedDate={18},@ModifiedBy={19},@ModifiedDate={20}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_V_BankDocumentDetail.ID, tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_V_BankDocumentMaster_ID, tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_V_BankTransactionMode_ID,
                tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_V_BankDocumentDetail.PostingDate,
                tbl_Ac_V_BankDocumentDetail.InstrumentNo, tbl_Ac_V_BankDocumentDetail.InstrumentDate, tbl_Ac_V_BankDocumentDetail.Narration, tbl_Ac_V_BankDocumentDetail.Amount,
                tbl_Ac_V_BankDocumentDetail.Cleared1_Cancelled0, tbl_Ac_V_BankDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For,
                tbl_Ac_V_BankDocumentDetail.IsSupervised, tbl_Ac_V_BankDocumentDetail.SupervisedBy, tbl_Ac_V_BankDocumentDetail.SupervisedDate, 
                tbl_Ac_V_BankDocumentDetail.CreatedBy, tbl_Ac_V_BankDocumentDetail.CreatedDate, tbl_Ac_V_BankDocumentDetail.ModifiedBy, tbl_Ac_V_BankDocumentDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }

        #endregion

        #region Report   

        public List<ReportCallingModel> GetRLBankReceiveMasterDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Bank Receive Pendings",
                    GroupBy = new List<string>(){ "Account From", "Account To" },
                    OrderBy = null,
                    SeekBy = new List<string>(){ "All", "Instrument Date Till Today" }
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Bank Receive Doc",
                    GroupBy = new List<string>(){ "Account From", "Account To" },
                    OrderBy = new List<string>(){ "Voucher Date", "Voucher No" },
                    SeekBy = new List<string>(){ "All", "Cleared", "Cancelled" }
                }
            };
        }
        public List<ReportCallingModel> GetRLBankReceiveDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Bank Receive Doc",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }

        public List<ReportCallingModel> GetRLBankPaymentMasterDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Bank Payment Pendings",
                    GroupBy = new List<string>(){ "Account From", "Account To" },
                    OrderBy = null,
                    SeekBy = new List<string>(){ "All", "Instrument Date Till Today" }
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Bank Payment Doc",
                    GroupBy = new List<string>(){ "Account From", "Account To" },
                    OrderBy = new List<string>(){ "Voucher Date", "Voucher No" },
                    SeekBy = new List<string>(){ "All", "Cleared", "Cancelled" }
                }
            };
        }
        public List<ReportCallingModel> GetRLBankPaymentDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Bank Payment Doc",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Bank Payment Doc" || rn == "Bank Receive Doc")
            {
                return await Task.Run(() => CurrentBankDoc(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Bank Payment Doc" || rn == "Register Bank Receive Doc")
            {
                return await Task.Run(() => RegisterBankDoc(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Bank Payment Pendings" || rn == "Bank Receive Pendings")
            {
                return await Task.Run(() => BankDocPendings(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentBankDoc(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + rn + " -----", true);
            var ColorSteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorGray = new MyDeviceRgb(MyColor.Gray).color;
   

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //Voucher No
                        (float)(PageSize.A4.GetWidth() * 0.15), //Voucher Date
                        (float)(PageSize.A4.GetWidth() * 0.60),  //Account From 
                        (float)(PageSize.A4.GetWidth() * 0.10)  //DebitCredit
                }
                ).SetFontSize(10).SetFixedLayout().SetBorder(Border.NO_BORDER);
              

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Account From")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Flow")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = "";
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {     
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DebitCredit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                pdftableMaster.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 6------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.07), //S No
                        (float)(PageSize.A4.GetWidth() * 0.40), //AccountName
                        (float)(PageSize.A4.GetWidth() * 0.21),  //BankTransactionMode
                        (float)(PageSize.A4.GetWidth() * 0.12),  //InstrumentNo
                        (float)(PageSize.A4.GetWidth() * 0.09),  //InstrumentDate
                        (float)(PageSize.A4.GetWidth() * 0.11)  //Amount
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Account To")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Mode")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Instrument #")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Inst Date")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Amount")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalAmount = 0;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell(2, 1).Add(new Paragraph().Add(SNo.ToString())).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BankTransactionMode"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["InstrumentNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["InstrumentDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Amount"])).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell(1,5).Add(new Paragraph().Add("Narration: " + sqlReader["Narration"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        TotalAmount = TotalAmount + (double)sqlReader["Amount"];
                        SNo++;

                    }
                }


                pdftableDetail.AddCell(new Cell(1, 6).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                
                pdftableDetail.AddCell(new Cell(1, 4).Add(new Paragraph().Add(AmountIntoWords.ConvertToWords((long)TotalAmount))).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", TotalAmount))).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }


            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterBankDoc(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- " + rn + " From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 13------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//VoucherNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//VoucherDate 
                        (float)(PageSize.A4.GetWidth() * 0.15),//AccountFrom 
                        (float)(PageSize.A4.GetWidth() * 0.05),//DebitCredit 
                        (float)(PageSize.A4.GetWidth() * 0.08),//BankTransactionMode
                        (float)(PageSize.A4.GetWidth() * 0.15),//AccountTo
                        (float)(PageSize.A4.GetWidth() * 0.05),//PostingDate 
                        (float)(PageSize.A4.GetWidth() * 0.05),//InstrumentNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//InstrumentDate
                        (float)(PageSize.A4.GetWidth() * 0.15),//Narration
                        (float)(PageSize.A4.GetWidth() * 0.07),//Amount
                        (float)(PageSize.A4.GetWidth() * 0.05)//PostingStatus
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account From")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Flow")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Mode")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account To")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Inst #")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Inst Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Amount")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Status")).SetBold());


            int SNo = 1;

            double GrandTotalAmount = 0, GroupTotalAmount = 0;


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "Account From" ? "AccountFrom" :
                                          GroupBy == "Account To" ? "AccountTo" :
                                          "";


                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {

                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }


                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 13).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 13).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalAmount = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountFrom"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DebitCredit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BankTransactionMode"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountTo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["InstrumentNo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["InstrumentDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Amount"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PostingStatus"].ToString())).SetKeepTogether(true));

                        GroupTotalAmount += Convert.ToDouble(sqlReader["Amount"]);
                        GrandTotalAmount += Convert.ToDouble(sqlReader["Amount"]);
                    }


                }
            }

            pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 13).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> BankDocPendings(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- " + rn + " " + SeekBy + " -----", false);

            /////////////------------------------------table for Detail 10------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.07),//VoucherNo
                        (float)(PageSize.A4.GetWidth() * 0.07),//VoucherDate 
                        (float)(PageSize.A4.GetWidth() * 0.20),//AccountFrom 
                        (float)(PageSize.A4.GetWidth() * 0.07),//DebitCredit 
                        (float)(PageSize.A4.GetWidth() * 0.20),//AccountTo
                        (float)(PageSize.A4.GetWidth() * 0.07),//PostingDate 
                        (float)(PageSize.A4.GetWidth() * 0.10),//InstrumentNo
                        (float)(PageSize.A4.GetWidth() * 0.07),//InstrumentDate
                        (float)(PageSize.A4.GetWidth() * 0.10)//Amount
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account From")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Flow")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account To")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Inst #")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Inst Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Amount")).SetBold());


            int SNo = 1;

            double GrandTotalAmount = 0, GroupTotalAmount = 0;


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "Account From" ? "AccountFrom" :
                                          GroupBy == "Account To" ? "AccountTo" :
                                          "";


                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {

                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }


                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 10).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 10).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalAmount = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountFrom"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DebitCredit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountTo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["InstrumentNo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["InstrumentDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Amount"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));

                        GroupTotalAmount += Convert.ToDouble(sqlReader["Amount"]);
                        GrandTotalAmount += Convert.ToDouble(sqlReader["Amount"]);
                    }


                }
            }

            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));


            pdftableMain.AddCell(new Cell(1, 10).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));


            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        
        #endregion
    }
    public class CashDocumentRepository : ICashDocument
    {
        private readonly OreasDbContext db;
        public CashDocumentRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetCashDocumentMaster(int id)
        {
            var qry = from o in await db.tbl_Ac_V_CashDocumentMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          VoucherDate = o.VoucherDate.ToString("dd-MMM-yyyy") ?? "",
                          o.VoucherNo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit1_Credit0,
                          o.IsSupervisedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCashDocumentMaster()
        {
            return new[]
            {
                new { n = "by Account From", v = "byAccountFrom" }, new { n = "by Account To", v = "byAccountTo" }, new { n = "by Voucher No", v = "byVoucherNo" }, new { n = "by Pending", v = "byPending" }
            }.ToList();
        }
        public object GetWCLBCashDocumentMaster()
        {
            return new[]
            {
                new { n = "by Not Supervised", v = "byNotSupervised" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCashDocumentMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string IsFor = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_V_CashDocumentMasters
                                               .Where(w =>
                                                        string.IsNullOrEmpty(IsFor) && w.ID == 0
                                                        ||
                                                        IsFor == "Receive" && w.Debit1_Credit0 == true
                                                        ||
                                                        IsFor == "Payment" && w.Debit1_Credit0 == false
                                                        )
                                               .Where(w =>
                                                        string.IsNullOrEmpty(FilterByLoad)
                                                        ||
                                                        FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                     )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountFrom" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byAccountTo" && w.tbl_Ac_V_CashDocumentDetails.Any(a => a.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byVoucherNo" && w.VoucherNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_V_CashDocumentMasters
                                  .Where(w =>
                                            string.IsNullOrEmpty(IsFor) && w.ID == 0
                                            ||
                                            IsFor == "Receive" && w.Debit1_Credit0 == true
                                            ||
                                            IsFor == "Payment" && w.Debit1_Credit0 == false
                                            )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                            )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountFrom" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byAccountTo" && w.tbl_Ac_V_CashDocumentDetails.Any(a => a.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byVoucherNo" && w.VoucherNo.ToString() == FilterValueByText
                                        ||
                                        FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          VoucherDate = o.VoucherDate.ToString("dd-MMM-yyyy") ?? "",
                          o.VoucherNo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit1_Credit0,
                          o.IsSupervisedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          Total = (double?)o.tbl_Ac_V_CashDocumentDetails.Sum(s => s.Amount) ?? 0
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCashDocumentMaster(tbl_Ac_V_CashDocumentMaster tbl_Ac_V_CashDocumentMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_V_CashDocumentMaster.CreatedBy = userName;
                tbl_Ac_V_CashDocumentMaster.CreatedDate = DateTime.Now;
                //db.tbl_Ac_V_CashDocumentMasters.Add(tbl_Ac_V_CashDocumentMaster);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_V_CashDocumentMaster.ModifiedBy = userName;
                tbl_Ac_V_CashDocumentMaster.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Ac_V_CashDocumentMaster).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Ac_V_CashDocumentMasters.Remove(db.tbl_Ac_V_CashDocumentMasters.Find(tbl_Ac_V_CashDocumentMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_CashDocumentMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
                @ID={3},@VoucherNo={4},@VoucherDate={5},@FK_tbl_Ac_ChartOfAccounts_ID={6},@Debit1_Credit0={7},
                @CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_V_CashDocumentMaster.ID, tbl_Ac_V_CashDocumentMaster.VoucherNo, tbl_Ac_V_CashDocumentMaster.VoucherDate, tbl_Ac_V_CashDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_V_CashDocumentMaster.Debit1_Credit0,
                tbl_Ac_V_CashDocumentMaster.CreatedBy, tbl_Ac_V_CashDocumentMaster.CreatedDate, tbl_Ac_V_CashDocumentMaster.ModifiedBy, tbl_Ac_V_CashDocumentMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetCashDocumentDetail(int id)
        {
            var qry = from o in await db.tbl_Ac_V_CashDocumentDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_V_CashDocumentMaster_ID,
                          o.FK_tbl_Ac_V_CashTransactionMode_ID,
                          FK_tbl_Ac_V_CashTransactionMode_IDName = o.tbl_Ac_V_CashTransactionMode.CashTransactionMode,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          PostingDate = o.PostingDate.ToString() ?? "",
                          o.InstrumentNo,
                          o.Narration,
                          o.Amount,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_For,
                          FK_tbl_Ac_ChartOfAccounts_ID_ForName = o?.tbl_Ac_ChartOfAccounts_For?.AccountName ?? "",
                          o.IsSupervised,
                          o.SupervisedBy,
                          SupervisedDate = o.SupervisedDate.HasValue ? o.SupervisedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCashDocumentDetail()
        {
            return new[]
            {
                new { n = "by Account To", v = "byAccountTo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCashDocumentDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_V_CashDocumentDetails
                                               .Where(w => w.FK_tbl_Ac_V_CashDocumentMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountTo" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_V_CashDocumentDetails
                                  .Where(w => w.FK_tbl_Ac_V_CashDocumentMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountTo" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_V_CashDocumentMaster_ID,
                          o.FK_tbl_Ac_V_CashTransactionMode_ID,
                          FK_tbl_Ac_V_CashTransactionMode_IDName = o.tbl_Ac_V_CashTransactionMode.CashTransactionMode,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          PostingDate = o.PostingDate.ToString("dd-MMM-yyyy hh:mm tt") ?? "",
                          o.InstrumentNo,
                          o.Narration,
                          o.Amount,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_For,
                          FK_tbl_Ac_ChartOfAccounts_ID_ForName = o?.tbl_Ac_ChartOfAccounts_For?.AccountName ?? "",
                          o.IsSupervised,
                          o.SupervisedBy,
                          SupervisedDate = o.SupervisedDate.HasValue ? o.SupervisedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCashDocumentDetail(tbl_Ac_V_CashDocumentDetail tbl_Ac_V_CashDocumentDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_V_CashDocumentDetail.CreatedBy = userName;
                tbl_Ac_V_CashDocumentDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_V_CashDocumentDetail.ModifiedBy = userName;
                tbl_Ac_V_CashDocumentDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_CashDocumentDetail] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Ac_V_CashDocumentMaster_ID={4}
                  ,@FK_tbl_Ac_V_CashTransactionMode_ID={5},@FK_tbl_Ac_ChartOfAccounts_ID={6}
                  ,@PostingDate={7},@InstrumentNo={8},@Narration={9},@Amount={10},@FK_tbl_Ac_ChartOfAccounts_ID_For={11} 
                  ,@IsSupervised={12},@SupervisedBy={13},@SupervisedDate={14}
                  ,@CreatedBy={15},@CreatedDate={16},@ModifiedBy={17},@ModifiedDate={18}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_V_CashDocumentDetail.ID, tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_V_CashDocumentMaster_ID,
                tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_V_CashTransactionMode_ID, tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID,
                tbl_Ac_V_CashDocumentDetail.PostingDate, tbl_Ac_V_CashDocumentDetail.InstrumentNo, tbl_Ac_V_CashDocumentDetail.Narration, tbl_Ac_V_CashDocumentDetail.Amount, tbl_Ac_V_CashDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For,
                tbl_Ac_V_CashDocumentDetail.IsSupervised, tbl_Ac_V_CashDocumentDetail.SupervisedBy, tbl_Ac_V_CashDocumentDetail.SupervisedDate,
                tbl_Ac_V_CashDocumentDetail.CreatedBy, tbl_Ac_V_CashDocumentDetail.CreatedDate, tbl_Ac_V_CashDocumentDetail.ModifiedBy, tbl_Ac_V_CashDocumentDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Report     
        public List<ReportCallingModel> GetRLCashPaymentMasterDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Cash Payment Doc",
                    GroupBy = new List<string>(){ "Account From", "Account To" },
                    OrderBy = new List<string>(){ "Voucher Date", "Voucher No" },
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLCashPaymentDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Cash Payment Doc",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLCashReceiveMasterDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Cash Receive Doc",
                    GroupBy = new List<string>(){ "Account From", "Account To" },
                    OrderBy = new List<string>(){ "Voucher Date", "Voucher No" },
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLCashReceiveDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Cash Receive Doc",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Cash Payment Doc" || rn == "Cash Receive Doc")
            {
                return await Task.Run(() => CurrentCashDoc(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Cash Payment Doc" || rn == "Register Cash Receive Doc")
            {
                return await Task.Run(() => RegisterCashDoc(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentCashDoc(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + rn + " -----", true);
            var ColorSteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorGray = new MyDeviceRgb(MyColor.Gray).color;
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //Voucher No
                        (float)(PageSize.A4.GetWidth() * 0.15), //Voucher Date
                        (float)(PageSize.A4.GetWidth() * 0.60),  //Account From 
                        (float)(PageSize.A4.GetWidth() * 0.10)  //DebitCredit
                }
                ).SetFontSize(10).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Account From")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Flow")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = "";
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DebitCredit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                pdftableMaster.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 5------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.08), //S No
                        (float)(PageSize.A4.GetWidth() * 0.45), //AccountName
                        (float)(PageSize.A4.GetWidth() * 0.18),  //CashTransactionMode
                        (float)(PageSize.A4.GetWidth() * 0.14),  //InstrumentNo
                        (float)(PageSize.A4.GetWidth() * 0.15)  //Amount
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Account To")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Mode")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Instrument #")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Amount")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                
                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalAmount = 0;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell(2, 1).Add(new Paragraph().Add(SNo.ToString())).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CashTransactionMode"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["InstrumentNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Amount"])).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Narration: " + sqlReader["Narration"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        TotalAmount = TotalAmount + (double)sqlReader["Amount"];
                        SNo++;

                    }
                }

                pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell(1, 4).Add(new Paragraph().Add(AmountIntoWords.ConvertToWords((long)TotalAmount))).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", TotalAmount))).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterCashDoc(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- " + rn + " From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 11------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//VoucherNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//VoucherDate 
                        (float)(PageSize.A4.GetWidth() * 0.17),//AccountFrom 
                        (float)(PageSize.A4.GetWidth() * 0.05),//DebitCredit 
                        (float)(PageSize.A4.GetWidth() * 0.08),//CashTransactionMode
                        (float)(PageSize.A4.GetWidth() * 0.17),//AccountTo
                        (float)(PageSize.A4.GetWidth() * 0.05),//PostingDate 
                        (float)(PageSize.A4.GetWidth() * 0.05),//InstrumentNo 
                        (float)(PageSize.A4.GetWidth() * 0.20),//Narration
                        (float)(PageSize.A4.GetWidth() * 0.07)//Amount
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account From")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Flow")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Mode")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account To")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Inst #")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Amount")).SetBold());


            int SNo = 1;

            double GrandTotalAmount = 0, GroupTotalAmount = 0;


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "Account From" ? "AccountFrom" :
                                          GroupBy == "Account To" ? "AccountTo" :
                                          "";


                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {

                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 10).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }


                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalAmount = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountFrom"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DebitCredit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CashTransactionMode"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountTo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["InstrumentNo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Amount"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));

                        GroupTotalAmount += Convert.ToDouble(sqlReader["Amount"]);
                        GrandTotalAmount += Convert.ToDouble(sqlReader["Amount"]);
                    }


                }
            }

            pdftableMain.AddCell(new Cell(1, 10).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));


            pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 10).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion
    }
    public class JournalDocumentRepository :IJournalDocument
    {
        private readonly OreasDbContext db;
        public JournalDocumentRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetJournalDocumentMaster(int id)
        {
            var qry = from o in await db.tbl_Ac_V_JournalDocumentMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          VoucherDate = o.VoucherDate.ToString("dd-MMM-yyyy") ?? "",
                          o.VoucherNo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit1_Credit0,
                          o.IsSupervisedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""                          
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLJournalDocumentMaster()
        {
            return new[]
            {
                new { n = "by Account From", v = "byAccountFrom" }, new { n = "by Account To", v = "byAccountTo" }, new { n = "by Voucher No", v = "byVoucherNo" }
            }.ToList();
        }
        public object GetWCLBJournalDocumentMaster()
        {
            return new[]
            {
                new { n = "by Not Supervised", v = "byNotSupervised" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadJournalDocumentMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_V_JournalDocumentMasters
                                               .Where(w =>
                                                        string.IsNullOrEmpty(FilterByLoad)
                                                        ||
                                                        FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                     )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountFrom" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byAccountTo" && w.tbl_Ac_V_JournalDocumentDetails.Any(a => a.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byVoucherNo" && w.VoucherNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_V_JournalDocumentMasters
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                            )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountFrom" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byAccountTo" && w.tbl_Ac_V_JournalDocumentDetails.Any(a => a.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byVoucherNo" && w.VoucherNo.ToString() == FilterValueByText
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          VoucherDate = o.VoucherDate.ToString("dd-MMM-yyyy") ?? "",
                          o.VoucherNo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit1_Credit0,
                          o.IsSupervisedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          Total = (double?)o.tbl_Ac_V_JournalDocumentDetails.Sum(s => s.Amount) ?? 0
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostJournalDocumentMaster(tbl_Ac_V_JournalDocumentMaster tbl_Ac_V_JournalDocumentMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_V_JournalDocumentMaster.CreatedBy = userName;
                tbl_Ac_V_JournalDocumentMaster.CreatedDate = DateTime.Now;
                //db.tbl_Ac_V_JournalDocumentMasters.Add(tbl_Ac_V_JournalDocumentMaster);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_V_JournalDocumentMaster.ModifiedBy = userName;
                tbl_Ac_V_JournalDocumentMaster.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Ac_V_JournalDocumentMaster).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Ac_V_JournalDocumentMasters.Remove(db.tbl_Ac_V_JournalDocumentMasters.Find(tbl_Ac_V_JournalDocumentMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_JournalDocumentMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
                @ID={3},@VoucherNo={4},@VoucherDate={5},@FK_tbl_Ac_ChartOfAccounts_ID={6},@Debit1_Credit0={7},
                @CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_V_JournalDocumentMaster.ID, tbl_Ac_V_JournalDocumentMaster.VoucherNo, tbl_Ac_V_JournalDocumentMaster.VoucherDate, tbl_Ac_V_JournalDocumentMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_V_JournalDocumentMaster.Debit1_Credit0,
                tbl_Ac_V_JournalDocumentMaster.CreatedBy, tbl_Ac_V_JournalDocumentMaster.CreatedDate, tbl_Ac_V_JournalDocumentMaster.ModifiedBy, tbl_Ac_V_JournalDocumentMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetJournalDocumentDetail(int id)
        {
            var qry = from o in await db.tbl_Ac_V_JournalDocumentDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_V_JournalDocumentMaster_ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          PostingDate = o.PostingDate.ToString() ?? "",
                          o.Narration,
                          o.Amount,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_For,
                          FK_tbl_Ac_ChartOfAccounts_ID_ForName = o?.tbl_Ac_ChartOfAccounts_For?.AccountName ?? "",
                          o.IsSupervised,
                          o.SupervisedBy,
                          SupervisedDate = o.SupervisedDate.HasValue ? o.SupervisedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLJournalDocumentDetail()
        {
            return new[]
            {
                new { n = "by Account To", v = "byAccountTo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadJournalDocumentDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_V_JournalDocumentDetails
                                               .Where(w => w.FK_tbl_Ac_V_JournalDocumentMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountTo" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_V_JournalDocumentDetails
                                  .Where(w => w.FK_tbl_Ac_V_JournalDocumentMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountTo" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_V_JournalDocumentMaster_ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          PostingDate = o.PostingDate.ToString("dd-MMM-yyyy hh:mm tt") ?? "",
                          o.Narration,
                          o.Amount,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_For,
                          FK_tbl_Ac_ChartOfAccounts_ID_ForName = o?.tbl_Ac_ChartOfAccounts_For?.AccountName ?? "",
                          o.IsSupervised,
                          o.SupervisedBy,
                          SupervisedDate = o.SupervisedDate.HasValue ? o.SupervisedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostJournalDocumentDetail(tbl_Ac_V_JournalDocumentDetail tbl_Ac_V_JournalDocumentDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_V_JournalDocumentDetail.CreatedBy = userName;
                tbl_Ac_V_JournalDocumentDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_V_JournalDocumentDetail.ModifiedBy = userName;
                tbl_Ac_V_JournalDocumentDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_JournalDocumentDetail] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Ac_V_JournalDocumentMaster_ID={4}
                  ,@FK_tbl_Ac_ChartOfAccounts_ID={5},@PostingDate={6},@Narration={7},@Amount={8},@FK_tbl_Ac_ChartOfAccounts_ID_For={9} 
                  ,@IsSupervised={10},@SupervisedBy={11},@SupervisedDate={12}
                  ,@CreatedBy={13},@CreatedDate={14},@ModifiedBy={15},@ModifiedDate={16}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_V_JournalDocumentDetail.ID, tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_V_JournalDocumentMaster_ID,
                tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_V_JournalDocumentDetail.PostingDate, tbl_Ac_V_JournalDocumentDetail.Narration, tbl_Ac_V_JournalDocumentDetail.Amount, tbl_Ac_V_JournalDocumentDetail.FK_tbl_Ac_ChartOfAccounts_ID_For,
                tbl_Ac_V_JournalDocumentDetail.IsSupervised, tbl_Ac_V_JournalDocumentDetail.SupervisedBy, tbl_Ac_V_JournalDocumentDetail.SupervisedDate,
                tbl_Ac_V_JournalDocumentDetail.CreatedBy, tbl_Ac_V_JournalDocumentDetail.CreatedDate, tbl_Ac_V_JournalDocumentDetail.ModifiedBy, tbl_Ac_V_JournalDocumentDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        public async Task<string> JournalDocumentDetailUploadExcelFile(List<JournalDocExcelData> JournalDocExcelDataList, string operation, string userName)
        {
            if (operation == "Save New")
            {
                List<tbl_Ac_V_JournalDocumentDetail> JournalDocDetails = new List<tbl_Ac_V_JournalDocumentDetail>();

                foreach (var item in JournalDocExcelDataList)
                {
                    var temp = await db.tbl_Ac_ChartOfAccountss.Where(w => w.AccountCode == item.AcCode).FirstOrDefaultAsync();

                    if (!(item.MasterID > 0) || string.IsNullOrEmpty(item.AcCode) || 
                         string.IsNullOrEmpty(item.Narration) || item.PostingDate == null || 
                         !(item.Amount>0) || temp == null)
                    {
                        continue;
                    }  

                    JournalDocDetails.Add(
                        new tbl_Ac_V_JournalDocumentDetail()
                        {
                            ID = 0,
                            FK_tbl_Ac_V_JournalDocumentMaster_ID = item.MasterID,
                            FK_tbl_Ac_ChartOfAccounts_ID = temp.ID,
                            Narration = item.Narration,
                            PostingDate = item.PostingDate,
                            IsSupervised = false,
                            Amount = item.Amount,
                            CreatedBy = userName,
                            CreatedDate = DateTime.Now
                        });

                }

                //------------Add compiled record to database--------------------//
                SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
                SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
                SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                CRUD_Type.Value = "Insert";

                foreach (var jd in JournalDocDetails)
                {
                    await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_JournalDocumentDetail] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Ac_V_JournalDocumentMaster_ID={4}
                  ,@FK_tbl_Ac_ChartOfAccounts_ID={5},@PostingDate={6},@Narration={7},@Amount={8},@FK_tbl_Ac_ChartOfAccounts_ID_For={9} 
                  ,@IsSupervised={10},@SupervisedBy={11},@SupervisedDate={12}
                  ,@CreatedBy={13},@CreatedDate={14},@ModifiedBy={15},@ModifiedDate={16}",
                  CRUD_Type, CRUD_Msg, CRUD_ID,
                  jd.ID, jd.FK_tbl_Ac_V_JournalDocumentMaster_ID,
                  jd.FK_tbl_Ac_ChartOfAccounts_ID, jd.PostingDate, jd.Narration, jd.Amount, jd.FK_tbl_Ac_ChartOfAccounts_ID_For,
                  jd.IsSupervised, jd.SupervisedBy, jd.SupervisedDate,
                  jd.CreatedBy, jd.CreatedDate, jd.ModifiedBy, jd.ModifiedDate);

                }


            }
            else
            {
                return "Wrong Operation";
            }

            return "OK";
        }

        #endregion

        #region Report     
        public List<ReportCallingModel> GetRLJournalMasterDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Journal Doc",
                    GroupBy = new List<string>(){ "Account From", "Account To" },
                    OrderBy = new List<string>(){ "Voucher Date", "Voucher No" },
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLJournalDocument()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Journal Doc",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Journal Doc")
            {
                return await Task.Run(() => CurrentJournalDoc(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Journal Doc")
            {
                return await Task.Run(() => RegisterJournalDoc(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentJournalDoc(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Journal Document Voucher -----", true);
            var ColorSteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorGray = new MyDeviceRgb(MyColor.Gray).color;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //Voucher No
                        (float)(PageSize.A4.GetWidth() * 0.15), //Voucher Date
                        (float)(PageSize.A4.GetWidth() * 0.60),  //Account From 
                        (float)(PageSize.A4.GetWidth() * 0.10)  //DebitCredit
                }
                ).SetFontSize(10).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Account From")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Flow")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = ""; string DebitCredit = "";
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DebitCredit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                        DebitCredit = sqlReader["DebitCredit"].ToString();
                    }
                }
                pdftableMaster.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 5------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.08), //S No
                        (float)(PageSize.A4.GetWidth() * 0.35), //AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.45),  //Narration
                        (float)(PageSize.A4.GetWidth() * 0.12)  //Amount
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

               
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Account To")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Narration")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(DebitCredit == "Debit" ? "Credit" : "Debit")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalAmount = 0;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Amount"])).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        TotalAmount = TotalAmount + (double)sqlReader["Amount"];
                        SNo++;

                    }
                }

                pdftableDetail.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add(AmountIntoWords.ConvertToWords((long)TotalAmount))).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", TotalAmount))).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterJournalDoc(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Journal Document Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 9------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//VoucherNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//VoucherDate 
                        (float)(PageSize.A4.GetWidth() * 0.20),//AccountFrom 
                        (float)(PageSize.A4.GetWidth() * 0.05),//DebitCredit 
                        (float)(PageSize.A4.GetWidth() * 0.20),//AccountTo
                        (float)(PageSize.A4.GetWidth() * 0.05),//PostingDate 
                        (float)(PageSize.A4.GetWidth() * 0.27),//Narration
                        (float)(PageSize.A4.GetWidth() * 0.07)//Amount
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account From")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Flow")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account To")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Amount")).SetBold());

            int SNo = 1;
            double GrandTotalAmount = 0, GroupTotalAmount = 0;


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "Account From" ? "AccountFrom" :
                                          GroupBy == "Account To" ? "AccountTo" :
                                          "";


                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {
                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalAmount = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountFrom"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DebitCredit"].ToString())).SetKeepTogether(true));                        
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountTo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Amount"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));

                        GroupTotalAmount += Convert.ToDouble(sqlReader["Amount"]);
                        GrandTotalAmount += Convert.ToDouble(sqlReader["Amount"]);
                    }
                }
            }

            pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
           
            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));
           
            pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion
    }
    public class JournalDocument2Repository : IJournalDocument2
    {
        private readonly OreasDbContext db;
        public JournalDocument2Repository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetJournalDocument2Master(int id)
        {
            var qry = from o in await db.tbl_Ac_V_JournalDocument2Masters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          VoucherDate = o.VoucherDate.ToString("dd-MMM-yyyy hh:mm tt") ?? "",
                          o.VoucherNo,
                          o.Remarks,
                          o.TotalDebit,
                          o.TotalCredit,
                          o.IsPosted,
                          o.IsSupervisedAll,
                          o.SupervisedBy,
                          SupervisedDate = o.SupervisedDate.HasValue ? o.SupervisedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLJournalDocument2Master()
        {
            return new[]
            {
                new { n = "by Account", v = "byAccount" }, new { n = "by Voucher No", v = "byVoucherNo" }
            }.ToList();
        }
        public object GetWCLBJournalDocument2Master()
        {
            return new[]
            {
                new { n = "by Not Supervised", v = "byNotSupervised" }, new { n = "by Not Posted", v = "byNotPosted" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadJournalDocument2Master(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_V_JournalDocument2Masters
                                               .Where(w =>
                                                        string.IsNullOrEmpty(FilterByLoad)
                                                        ||
                                                        FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                        ||
                                                        FilterByLoad == "byNotPosted" && !w.IsPosted
                                                     )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccount" && w.tbl_Ac_V_JournalDocument2Details.Any(a => a.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byVoucherNo" && w.VoucherNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_V_JournalDocument2Masters
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                            ||
                                            FilterByLoad == "byNotPosted" && !w.IsPosted
                                            )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccount" && w.tbl_Ac_V_JournalDocument2Details.Any(a => a.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byVoucherNo" && w.VoucherNo.ToString() == FilterValueByText
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          VoucherDate = o.VoucherDate.ToString("dd-MMM-yyyy hh:mm tt") ?? "",
                          o.VoucherNo,
                          o.Remarks,
                          o.TotalDebit,
                          o.TotalCredit,
                          o.IsPosted,
                          o.IsSupervisedAll,
                          o.SupervisedBy,
                          SupervisedDate = o.SupervisedDate.HasValue ? o.SupervisedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostJournalDocument2Master(tbl_Ac_V_JournalDocument2Master tbl_Ac_V_JournalDocument2Master, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                //-----------------new record--------------//
                if (tbl_Ac_V_JournalDocument2Master.ID == 0)
                {
                    tbl_Ac_V_JournalDocument2Master.CreatedBy = userName;
                    tbl_Ac_V_JournalDocument2Master.CreatedDate = DateTime.Now;
                    CRUD_Type.Value = "Insert";
                }
                else if (tbl_Ac_V_JournalDocument2Master.ID > 0)
                {
                    tbl_Ac_V_JournalDocument2Master.ModifiedBy = userName;
                    tbl_Ac_V_JournalDocument2Master.ModifiedDate = DateTime.Now;
                    CRUD_Type.Value = "Posting";
                }

            }
            else if (operation == "Save Update")
            {
                tbl_Ac_V_JournalDocument2Master.ModifiedBy = userName;
                tbl_Ac_V_JournalDocument2Master.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_JournalDocument2Master] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@VoucherNo={4},@VoucherDate={5}
                ,@Remarks={6},@TotalDebit={7},@TotalCredit={8},@IsPosted={9}
                ,@IsSupervisedAll={10},@SupervisedBy={11},@SupervisedDate={12}
                ,@CreatedBy={13},@CreatedDate={14},@ModifiedBy={15},@ModifiedDate={16}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_V_JournalDocument2Master.ID, tbl_Ac_V_JournalDocument2Master.VoucherNo, tbl_Ac_V_JournalDocument2Master.VoucherDate,
                tbl_Ac_V_JournalDocument2Master.Remarks, tbl_Ac_V_JournalDocument2Master.TotalDebit, tbl_Ac_V_JournalDocument2Master.TotalCredit, tbl_Ac_V_JournalDocument2Master.IsPosted,
                tbl_Ac_V_JournalDocument2Master.IsSupervisedAll, tbl_Ac_V_JournalDocument2Master.SupervisedBy, tbl_Ac_V_JournalDocument2Master.SupervisedDate,
                tbl_Ac_V_JournalDocument2Master.CreatedBy, tbl_Ac_V_JournalDocument2Master.CreatedDate, tbl_Ac_V_JournalDocument2Master.ModifiedBy, tbl_Ac_V_JournalDocument2Master.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetJournalDocument2Detail(int id)
        {
            var qry = from o in await db.tbl_Ac_V_JournalDocument2Details.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_V_JournalDocument2Master_ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit,
                          o.Credit,
                          o.Narration,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLJournalDocument2Detail()
        {
            return new[]
            {
                new { n = "by Account", v = "byAccount" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadJournalDocument2Detail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_V_JournalDocument2Details
                                               .Where(w => w.FK_tbl_Ac_V_JournalDocument2Master_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccount" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_V_JournalDocument2Details
                                  .Where(w => w.FK_tbl_Ac_V_JournalDocument2Master_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccount" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_V_JournalDocument2Master_ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Debit,
                          o.Credit,
                          o.Narration,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;


            double TotalDebit = await db.tbl_Ac_V_JournalDocument2Details.Where(w => w.FK_tbl_Ac_V_JournalDocument2Master_ID == MasterID).SumAsync(s => s.Debit);
            double TotalCredit = await db.tbl_Ac_V_JournalDocument2Details.Where(w => w.FK_tbl_Ac_V_JournalDocument2Master_ID == MasterID).SumAsync(s => s.Credit);
            bool IsPosted = await db.tbl_Ac_V_JournalDocument2Masters.Where(w => w.ID == MasterID).Select(o=> o.IsPosted).FirstOrDefaultAsync();
            
            pageddata.otherdata = new { TotalDebit= TotalDebit, TotalCredit=TotalCredit, IsPosted= IsPosted };

            return pageddata;
        }
        public async Task<string> PostJournalDocument2Detail(tbl_Ac_V_JournalDocument2Detail tbl_Ac_V_JournalDocument2Detail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_V_JournalDocument2Detail.CreatedBy = userName;
                tbl_Ac_V_JournalDocument2Detail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_V_JournalDocument2Detail.ModifiedBy = userName;
                tbl_Ac_V_JournalDocument2Detail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_V_JournalDocument2Detail] 
                 @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Ac_V_JournalDocument2Master_ID={4}
                ,@FK_tbl_Ac_ChartOfAccounts_ID={5},@Debit={6},@Credit={7},@Narration={8}
                ,@CreatedBy={9},@CreatedDate={10},@ModifiedBy={11},@ModifiedDate={12}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_V_JournalDocument2Detail.ID, tbl_Ac_V_JournalDocument2Detail.FK_tbl_Ac_V_JournalDocument2Master_ID,
                tbl_Ac_V_JournalDocument2Detail.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_V_JournalDocument2Detail.Debit, tbl_Ac_V_JournalDocument2Detail.Credit, tbl_Ac_V_JournalDocument2Detail.Narration,
                tbl_Ac_V_JournalDocument2Detail.CreatedBy, tbl_Ac_V_JournalDocument2Detail.CreatedDate, tbl_Ac_V_JournalDocument2Detail.ModifiedBy, tbl_Ac_V_JournalDocument2Detail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Report     
        public List<ReportCallingModel> GetRLJournalDocument2Master()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Journal2 Doc",
                    GroupBy = new List<string>(){ "Account" },
                    OrderBy = new List<string>(){ "Voucher Date", "Voucher No" },
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLJournalDocument2Detail()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Journal2 Doc",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Journal2 Doc")
            {
                return await Task.Run(() => CurrentJournalDoc(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Journal2 Doc")
            {
                return await Task.Run(() => RegisterJournalDoc(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentJournalDoc(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Journal Document2 Voucher -----", true);
            var ColorSteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorGray = new MyDeviceRgb(MyColor.Gray).color;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 5------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //Voucher No
                        (float)(PageSize.A4.GetWidth() * 0.15), //Voucher Date
                        (float)(PageSize.A4.GetWidth() * 0.40),  //Remarks 
                        (float)(PageSize.A4.GetWidth() * 0.15),  //TotalDebit 
                        (float)(PageSize.A4.GetWidth() * 0.15)  //TotalCredit
                }
                ).SetFontSize(10).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Remarks")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Total Debit")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Total Credit")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                

                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = ""; 
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Remarks"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TotalDebit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TotalCredit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                pdftableMaster.AddCell(new Cell(1, 54).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 5------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.08), //S No
                        (float)(PageSize.A4.GetWidth() * 0.36), //AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.12), //Debit 
                        (float)(PageSize.A4.GetWidth() * 0.12), //Credit 
                        (float)(PageSize.A4.GetWidth() * 0.32)  //Narration
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);


                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Account")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Debit")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Credit")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Narration")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                

                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalDebit = 0, TotalCredit=0;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Debit"])).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Credit"])).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        

                        TotalDebit += Convert.ToDouble(sqlReader["Debit"]);
                        TotalCredit += Convert.ToDouble(sqlReader["Credit"]);
                        SNo++;

                    }
                }

                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", TotalDebit.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", TotalCredit.ToString())).SetBold().SetTextAlignment(TextAlignment.RIGHT)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
   
                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterJournalDoc(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Journal Document2 Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 7------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//VoucherNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//VoucherDate 
                        (float)(PageSize.A4.GetWidth() * 0.34),//Account 
                        (float)(PageSize.A4.GetWidth() * 0.33), //Narration 
                        (float)(PageSize.A4.GetWidth() * 0.09),//Debit
                        (float)(PageSize.A4.GetWidth() * 0.09) //Credit
                        
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Voucher Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Debit")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Credit")).SetBold());
            

            int SNo = 1;
            double GrandTotalDebit = 0, GrandTotalCredit = 0, GroupTotalDebit = 0, GroupTotalCredit = 0;


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_Voucher] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "Account" ? "AccountName" :
                                          "";

                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {
                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalDebit))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalCredit))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalDebit = 0; GroupTotalCredit = 0; SNo = 1;

                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["VoucherNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["VoucherDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Debit"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Credit"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        
                        

                        GroupTotalDebit += Convert.ToDouble(sqlReader["Debit"]);
                        GroupTotalCredit += Convert.ToDouble(sqlReader["Credit"]);

                        GrandTotalDebit += Convert.ToDouble(sqlReader["Debit"]);
                        GrandTotalCredit += Convert.ToDouble(sqlReader["Credit"]);

                        SNo++;
                    }
                }
            }

            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalDebit))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalCredit))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalDebit))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalCredit))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion
    }
    public class PurchaseNoteInvoiceRepository : IPurchaseNoteInvoice
    {
        private readonly OreasDbContext db;
        public PurchaseNoteInvoiceRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetPurchaseNoteInvoiceMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.SupplierChallanNo,
                          o.SupplierInvoiceNo,
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.IsQCAll,
                          o.IsQCSampleAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseNoteInvoiceMaster()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBPurchaseNoteInvoiceMaster()
        {
            return new[]
            {
                new { n = "by Not Processed", v = "byNotProcessed" }, new { n = "by Not Supervised", v = "byNotSupervised" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseNoteInvoiceMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseNoteMasters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_PurchaseNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .Where(w=>
                                                         string.IsNullOrEmpty(FilterByLoad)
                                                         ||
                                                         FilterByLoad == "byNotProcessed" && !w.IsProcessedAll
                                                         ||
                                                         FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseNoteMasters
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_PurchaseNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byNotProcessed" && !w.IsProcessedAll
                                            ||
                                            FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                        )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.SupplierChallanNo,
                          o.SupplierInvoiceNo,
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.IsQCAll,
                          o.IsQCSampleAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseNoteInvoiceMaster(tbl_Inv_PurchaseNoteMaster tbl_Inv_PurchaseNoteMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {

                //db.Entry(tbl_Inv_PurchaseNoteMaster).Property(x => x.SupplierInvoiceNo).IsModified = true;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "InsertByAc";
            }
            else if (operation == "Save Update")
            {

                //db.Entry(tbl_Inv_PurchaseNoteMaster).Property(x => x.SupplierInvoiceNo).IsModified = true;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "UpdateByAc";

            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Inv_PurchaseNoteMasters.Remove(db.tbl_Inv_PurchaseNoteMasters.Find(tbl_Inv_PurchaseNoteMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "DeleteByAc";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseNoteMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
                @ID={3},@DocNo={4},@DocDate={5},@FK_tbl_Inv_WareHouseMaster_ID={6},@FK_tbl_Ac_ChartOfAccounts_ID={7},
                @SupplierChallanNo={8},@SupplierInvoiceNo={9},@Remarks={10},
                @CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseNoteMaster.ID, tbl_Inv_PurchaseNoteMaster.DocNo, tbl_Inv_PurchaseNoteMaster.DocDate,
                tbl_Inv_PurchaseNoteMaster.FK_tbl_Inv_WareHouseMaster_ID, tbl_Inv_PurchaseNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID,
                tbl_Inv_PurchaseNoteMaster.SupplierChallanNo, tbl_Inv_PurchaseNoteMaster.SupplierInvoiceNo, tbl_Inv_PurchaseNoteMaster.Remarks,
                tbl_Inv_PurchaseNoteMaster.CreatedBy, tbl_Inv_PurchaseNoteMaster.CreatedDate, tbl_Inv_PurchaseNoteMaster.ModifiedBy, tbl_Inv_PurchaseNoteMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetPurchaseNoteInvoiceDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseNoteDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Rate,
                          o.GrossAmount,
                          o.GSTPercentage,
                          o.GSTAmount,
                          o.FreightIn,
                          o.DiscountAmount,
                          o.CostAmount,
                          o.WHTPercentage,
                          o.WHTAmount,
                          o.NetAmount,
                          o.MfgBatchNo,
                          o.ExpiryDate,
                          o.Remarks,
                          o.ReferenceNo,
                          o.FK_tbl_Inv_PurchaseOrderDetail_ID,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.QuantitySample,
                          DefaultWHTPer = o.tbl_Inv_PurchaseNoteMaster.tbl_Ac_ChartOfAccounts.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID.HasValue ? o.tbl_Inv_PurchaseNoteMaster.tbl_Ac_ChartOfAccounts.tbl_Ac_PolicyWHTaxOnPurchase.WHTaxPer : 0
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseNoteInvoiceDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseNoteInvoiceDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseNoteDetails
                                               .Where(w => w.FK_tbl_Inv_PurchaseNoteMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseNoteDetails
                                  .Where(w => w.FK_tbl_Inv_PurchaseNoteMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Rate,
                          o.GrossAmount,
                          o.GSTPercentage,
                          o.GSTAmount,
                          o.FreightIn,
                          o.DiscountAmount,
                          o.CostAmount,
                          o.WHTPercentage,
                          o.WHTAmount,
                          o.NetAmount,
                          o.MfgBatchNo,
                          o.ExpiryDate,
                          o.Remarks,
                          o.ReferenceNo,
                          o.FK_tbl_Inv_PurchaseOrderDetail_ID,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.QuantitySample
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseNoteInvoiceDetail(tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                //db.Entry(tbl_Inv_PurchaseNoteDetail).Property(x => x.Rate).IsModified = true;
                CRUD_Type.Value = "InsertByAc";
            }
            else if (operation == "Save Update")
            {
                //db.Entry(tbl_Inv_PurchaseNoteDetail).Property(x => x.Rate).IsModified = true;
                CRUD_Type.Value = "UpdateByAc";
            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Inv_PurchaseNoteDetails.Remove(db.tbl_Inv_PurchaseNoteDetails.Find(tbl_Inv_PurchaseNoteDetail.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "DeleteByAc";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseNoteDetail] 
                 @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Inv_PurchaseNoteMaster_ID={4},@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
                ,@Quantity={6},@Rate={7},@GrossAmount={8}
                ,@GSTPercentage={9},@GSTAmount={10},@FreightIn={11},@DiscountAmount={12},@CostAmount={13}
                ,@WHTPercentage={14},@WHTAmount={15},@NetAmount={16}
                ,@MfgBatchNo={17},@MfgDate={18},@ExpiryDate={19},@Remarks={20},@ReferenceNo={21},@FK_tbl_Inv_PurchaseOrderDetail_ID={22}
                ,@NoOfContainers={23},@PotencyPercentage={24}
                ,@CreatedBy={25},@CreatedDate={26},@ModifiedBy={27},@ModifiedDate={28}
                ,@FK_tbl_Qc_ActionType_ID={29},@QuantitySample={30},@RetestDate={31}
                ,@CreatedByQcQa={32},@CreatedDateQcQa={33},@ModifiedByQcQa={34},@ModifiedDateQcQa={35}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseNoteDetail.ID, tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseNoteMaster_ID, tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Inv_PurchaseNoteDetail.Quantity, tbl_Inv_PurchaseNoteDetail.Rate, tbl_Inv_PurchaseNoteDetail.GrossAmount,
                tbl_Inv_PurchaseNoteDetail.GSTPercentage, tbl_Inv_PurchaseNoteDetail.GSTAmount, tbl_Inv_PurchaseNoteDetail.FreightIn, tbl_Inv_PurchaseNoteDetail.DiscountAmount, tbl_Inv_PurchaseNoteDetail.CostAmount,
                tbl_Inv_PurchaseNoteDetail.WHTPercentage, tbl_Inv_PurchaseNoteDetail.WHTAmount, tbl_Inv_PurchaseNoteDetail.NetAmount,
                tbl_Inv_PurchaseNoteDetail.MfgBatchNo, tbl_Inv_PurchaseNoteDetail.MfgDate, tbl_Inv_PurchaseNoteDetail.ExpiryDate, tbl_Inv_PurchaseNoteDetail.Remarks, tbl_Inv_PurchaseNoteDetail.ReferenceNo, tbl_Inv_PurchaseNoteDetail.FK_tbl_Inv_PurchaseOrderDetail_ID,
                tbl_Inv_PurchaseNoteDetail.NoOfContainers, tbl_Inv_PurchaseNoteDetail.PotencyPercentage,
                tbl_Inv_PurchaseNoteDetail.CreatedBy, tbl_Inv_PurchaseNoteDetail.CreatedDate, tbl_Inv_PurchaseNoteDetail.ModifiedBy, tbl_Inv_PurchaseNoteDetail.ModifiedDate,
                tbl_Inv_PurchaseNoteDetail.FK_tbl_Qc_ActionType_ID, tbl_Inv_PurchaseNoteDetail.QuantitySample,tbl_Inv_PurchaseNoteDetail.RetestDate,
                tbl_Inv_PurchaseNoteDetail.CreatedByQcQa, tbl_Inv_PurchaseNoteDetail.CreatedDateQcQa, tbl_Inv_PurchaseNoteDetail.ModifiedByQcQa, tbl_Inv_PurchaseNoteDetail.ModifiedDateQcQa
                );

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region DetailOfDetail
        public async Task<PagedData<object>> LoadPurchaseNoteDetailOfDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_Ledgers
                                               .Where(w => w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo == MasterID)
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_Ledgers
                                  .Where(w => w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo == MasterID)
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          PostingDate = o.PostingDate.ToString("dd-MMM-yy hh:mm tt"),
                          o.Narration,
                          o.QuantityIn,
                          o.QuantityOut,
                          Ref = o.TrackingNo ?? ""

                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLPurchaseNote()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Purchase Note",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Purchase Invoice",
                    GroupBy = new List<string>(){ "WareHouse", "Supplier", "Product Name" },
                    OrderBy = new List<string>(){ "Doc Date", "Product Name" },
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Purchase Note")
            {
                return await Task.Run(() => CurrentPurchaseNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Purchase Invoice")
            {
                return await Task.Run(() => RegisterPurchaseNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentPurchaseNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Purchase Invoice -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 5------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocNo
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocDate
                        (float)(PageSize.A4.GetWidth() * 0.25),  //WareHouseName
                        (float)(PageSize.A4.GetWidth() * 0.30),  //AccountName                                                                 
                        (float)(PageSize.A4.GetWidth() * 0.15)  //SupplierChallanNo
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Challan #")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = "";
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["SupplierChallanNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 11------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.09), //S No
                        (float)(PageSize.A4.GetWidth() * 0.23), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),  //MeasurementUnit
                        (float)(PageSize.A4.GetWidth() * 0.09),  //ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.08),  //Rate
                        (float)(PageSize.A4.GetWidth() * 0.05),  //GSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.05),  //FreightIn
                        (float)(PageSize.A4.GetWidth() * 0.07),  //DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.05),  //WHTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.14)  //NetAmount
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 11).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("GST%")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Freight")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("WHT%")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalNetAmount = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["GSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FreightIn"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WHTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                        TotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);

                    }
                }
                pdftableDetail.AddCell(new Cell(1, 10).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 1).Add(new Paragraph().Add(string.Format("{0:n0}", TotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterPurchaseNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Purchase Invoice Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 14------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.14),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.15),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.20),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.05),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//MeasurementUnit 
                        (float)(PageSize.A4.GetWidth() * 0.05),//ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.04),// Rate
                        (float)(PageSize.A4.GetWidth() * 0.03),// GSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.04),// DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.03),// WHTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.07) // NetAmount
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Ref #")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("GST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WHT %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0, GrandTotalNetAmount = 0, GroupTotalNetAmount = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "WareHouse" ? "WareHouseName" :
                                          GroupBy == "Supplier" ? "AccountName" :
                                          GroupBy == "Product Name" ? "ProductName" :
                                          "";

                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {
                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0; GroupTotalNetAmount= 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["GSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WHTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));


                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        GroupTotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                        GrandTotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));


            pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion
    }
    public class PurchaseReturnNoteInvoiceRepository : IPurchaseReturnNoteInvoice
    {
        private readonly OreasDbContext db;
        public PurchaseReturnNoteInvoiceRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetPurchaseReturnNoteInvoiceMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseReturnNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseReturnNoteInvoiceMaster()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBPurchaseReturnNoteInvoiceMaster()
        {
            return new[]
            {
                new { n = "by Not Processed", v = "byNotProcessed" }, new { n = "by Not Supervised", v = "byNotSupervised" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseReturnNoteInvoiceMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseReturnNoteMasters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_PurchaseReturnNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .Where(w =>
                                                         string.IsNullOrEmpty(FilterByLoad)
                                                         ||
                                                         FilterByLoad == "byNotProcessed" && !w.IsProcessedAll
                                                         ||
                                                         FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseReturnNoteMasters
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_PurchaseReturnNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byNotProcessed" && !w.IsProcessedAll
                                            ||
                                            FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                         )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseReturnNoteInvoiceMaster(tbl_Inv_PurchaseReturnNoteMaster tbl_Inv_PurchaseReturnNoteMaster, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {

                //db.Entry(tbl_Inv_PurchaseReturnNoteMaster).Property(x => x.SupplierInvoiceNo).IsModified = true;
                //await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {

                //db.Entry(tbl_Inv_PurchaseReturnNoteMaster).Property(x => x.SupplierInvoiceNo).IsModified = true;
                //await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_PurchaseReturnNoteMasters.Remove(db.tbl_Inv_PurchaseReturnNoteMasters.Find(tbl_Inv_PurchaseReturnNoteMaster.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        #endregion

        #region Detail
        public async Task<object> GetPurchaseReturnNoteInvoiceDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseReturnNoteDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseReturnNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          ReferenceNo = o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo,
                          o.Quantity,
                          o.Rate,
                          o.GrossAmount,
                          o.GSTPercentage,
                          o.GSTAmount,
                          o.FreightIn,
                          o.WHTPercentage,                          
                          o.DiscountAmount,
                          o.CostAmount,
                          o.WHTAmount,
                          o.NetAmount,
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseReturnNoteInvoiceDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseReturnNoteInvoiceDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseReturnNoteDetails
                                               .Where(w => w.FK_tbl_Inv_PurchaseReturnNoteMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseReturnNoteDetails
                                  .Where(w => w.FK_tbl_Inv_PurchaseReturnNoteMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseReturnNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          ReferenceNo = o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo,
                          o.Quantity,
                          o.Rate,
                          o.GrossAmount,
                          o.GSTPercentage,
                          o.GSTAmount,
                          o.FreightIn,
                          o.WHTPercentage,
                          o.DiscountAmount,
                          o.CostAmount,
                          o.WHTAmount,
                          o.NetAmount,
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""                          
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseReturnNoteInvoiceDetail(tbl_Inv_PurchaseReturnNoteDetail tbl_Inv_PurchaseReturnNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                CRUD_Type.Value = "InsertByAc";
            }
            else if (operation == "Save Update")
            {
                CRUD_Type.Value = "UpdateByAc";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "DeleteByAc";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseReturnNoteDetail] 
                 @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Inv_PurchaseReturnNoteMaster_ID={4},@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
                ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={6},@Quantity={7}
                ,@Rate={8},@GrossAmount={9},@GSTPercentage={10},@GSTAmount={11},@FreightIn={12}
                ,@DiscountAmount={13},@CostAmount={14},@WHTPercentage={15},@WHTAmount={16}
                ,@NetAmount={17},@Remarks={18},@CreatedBy={19},@CreatedDate={20},@ModifiedBy={21},@ModifiedDate={22}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseReturnNoteDetail.ID, tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseReturnNoteMaster_ID,
                tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Inv_PurchaseReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo, tbl_Inv_PurchaseReturnNoteDetail.Quantity,
                tbl_Inv_PurchaseReturnNoteDetail.Rate, tbl_Inv_PurchaseReturnNoteDetail.GrossAmount, tbl_Inv_PurchaseReturnNoteDetail.GSTPercentage,
                tbl_Inv_PurchaseReturnNoteDetail.GSTAmount, tbl_Inv_PurchaseReturnNoteDetail.FreightIn, tbl_Inv_PurchaseReturnNoteDetail.DiscountAmount, tbl_Inv_PurchaseReturnNoteDetail.CostAmount,
                tbl_Inv_PurchaseReturnNoteDetail.WHTPercentage, tbl_Inv_PurchaseReturnNoteDetail.WHTAmount, tbl_Inv_PurchaseReturnNoteDetail.NetAmount,
                tbl_Inv_PurchaseReturnNoteDetail.Remarks, tbl_Inv_PurchaseReturnNoteDetail.CreatedBy, tbl_Inv_PurchaseReturnNoteDetail.CreatedDate,
                tbl_Inv_PurchaseReturnNoteDetail.ModifiedBy, tbl_Inv_PurchaseReturnNoteDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLPurchaseReturnNote()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Purchase Return Note",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Purchase Return Invoice",
                    GroupBy = new List<string>(){ "WareHouse", "Supplier", "Product Name" },
                    OrderBy = new List<string>(){ "Doc Date", "Product Name" },
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Purchase Return Note")
            {
                return await Task.Run(() => CurrentPurchaseReturnNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Purchase Return Invoice")
            {
                return await Task.Run(() => RegisterPurchaseReturnNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentPurchaseReturnNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Purchase Return Invoice -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 5------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocNo
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocDate
                        (float)(PageSize.A4.GetWidth() * 0.30),  //WareHouseName
                        (float)(PageSize.A4.GetWidth() * 0.40)  //AccountName    
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = "";
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 11------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.09), //S No
                        (float)(PageSize.A4.GetWidth() * 0.23), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),  //MeasurementUnit
                        (float)(PageSize.A4.GetWidth() * 0.09),  //ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.08),  //Rate
                        (float)(PageSize.A4.GetWidth() * 0.05),  //GSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.05),  //FreightIn
                        (float)(PageSize.A4.GetWidth() * 0.07),  //DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.05),  //WHTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.14)  //NetAmount
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 11).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("GST%")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Freight")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("WHT%")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalNetAmount = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["GSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FreightIn"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WHTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                        TotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);

                    }
                }
                pdftableDetail.AddCell(new Cell(1, 10).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 1).Add(new Paragraph().Add(string.Format("{0:n0}", TotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterPurchaseReturnNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Purchase Return Invoice Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);
            /////////////------------------------------table for Detail 14------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.14),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.15),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.20),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.05),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//MeasurementUnit 
                        (float)(PageSize.A4.GetWidth() * 0.05),//ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.04),// Rate
                        (float)(PageSize.A4.GetWidth() * 0.03),// GSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.04),// DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.03),// WHTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.07) // NetAmount
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Ref #")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("GST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WHT %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0, GrandTotalNetAmount = 0, GroupTotalNetAmount = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "WareHouse" ? "WareHouseName" :
                                          GroupBy == "Supplier" ? "AccountName" :
                                          GroupBy == "Product Name" ? "ProductName" :
                                          "";

                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {
                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0; GroupTotalNetAmount = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["GSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WHTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));


                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        GroupTotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                        GrandTotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));


            pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();

        }
        #endregion
    }
    public class SalesNoteInvoiceRepository : ISalesNoteInvoice
    {
        private readonly OreasDbContext db;
        public SalesNoteInvoiceRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetSalesNoteInvoiceMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_SalesNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.FK_tbl_Ac_CustomerSubDistributorList_ID,
                          FK_tbl_Ac_CustomerSubDistributorList_IDName = o?.tbl_Ac_CustomerSubDistributorList?.Name ?? "",
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter,
                          FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter.HasValue ? o.tbl_Ac_ChartOfAccounts_Transporter.AccountName : "",
                          o.TransportCharges,
                          o.TransporterBiltyNo,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSalesNoteInvoiceMaster()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBSalesNoteInvoiceMaster()
        {
            return new[]
            {
                new { n = "by Not Processed", v = "byNotProcessed" }, new { n = "by Not Supervised", v = "byNotSupervised" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesNoteInvoiceMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesNoteMasters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .Where(w =>
                                                         string.IsNullOrEmpty(FilterByLoad)
                                                         ||
                                                         FilterByLoad == "byNotProcessed" && !w.IsProcessedAll
                                                         ||
                                                         FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesNoteMasters
                                      .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                            )
                                      .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byNotProcessed" && !w.IsProcessedAll
                                                ||
                                                FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.FK_tbl_Ac_CustomerSubDistributorList_ID,
                          FK_tbl_Ac_CustomerSubDistributorList_IDName = o?.tbl_Ac_CustomerSubDistributorList?.Name ?? "",
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter,
                          FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter.HasValue ? o.tbl_Ac_ChartOfAccounts_Transporter.AccountName : "",
                          o.TransportCharges,
                          o.TransporterBiltyNo,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostSalesNoteInvoiceMaster(tbl_Inv_SalesNoteMaster tbl_Inv_SalesNoteMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                CRUD_Type.Value = "InsertByAc";
            }
            else if (operation == "Save Update")
            {
                CRUD_Type.Value = "UpdateByAc";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "DeleteByAc";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_SalesNoteMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
                @ID={3},@DocNo={4},@DocDate={5},
                @FK_tbl_Inv_WareHouseMaster_ID={6},@FK_tbl_Ac_ChartOfAccounts_ID={7},@FK_tbl_Ac_CustomerSubDistributorList_ID={8},@Remarks={9},
                @FK_tbl_Ac_ChartOfAccounts_ID_Transporter={10},@TransportCharges={11},@TransporterBiltyNo={12},
                @CreatedBy={13},@CreatedDate={14},@ModifiedBy={15},@ModifiedDate={16}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Inv_SalesNoteMaster.ID, tbl_Inv_SalesNoteMaster.DocNo, tbl_Inv_SalesNoteMaster.DocDate,
               tbl_Inv_SalesNoteMaster.FK_tbl_Inv_WareHouseMaster_ID, tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Inv_SalesNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_ID, tbl_Inv_SalesNoteMaster.Remarks,
               tbl_Inv_SalesNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_Transporter, tbl_Inv_SalesNoteMaster.TransportCharges, tbl_Inv_SalesNoteMaster.TransporterBiltyNo,
               tbl_Inv_SalesNoteMaster.CreatedBy, tbl_Inv_SalesNoteMaster.CreatedDate, tbl_Inv_SalesNoteMaster.ModifiedBy, tbl_Inv_SalesNoteMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetSalesNoteInvoiceDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_SalesNoteDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_SalesNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          o.Rate,
                          o.GrossAmount,
                          o.STPercentage,
                          o.STAmount,
                          o.FSTPercentage,
                          o.FSTAmount,
                          o.WHTPercentage,
                          o.WHTAmount,
                          o.DiscountAmount,
                          o.NetAmount,
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.NoOfPackages,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Inv_OrderNoteDetail_ID,
                          ONRate = o.FK_tbl_Inv_OrderNoteDetail_ID > 0 ?  o.tbl_Inv_OrderNoteDetail.Rate : 0,
                          DefaultWHTPer = o.tbl_Inv_SalesNoteMaster.tbl_Ac_ChartOfAccounts.FK_tbl_Ac_PolicyWHTaxOnSales_ID.HasValue ? o.tbl_Inv_SalesNoteMaster.tbl_Ac_ChartOfAccounts.tbl_Ac_PolicyWHTaxOnSales.WHTaxPer : 0
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSalesNoteInvoiceDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesNoteInvoiceDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesNoteDetails
                                               .Where(w => w.FK_tbl_Inv_SalesNoteMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesNoteDetails
                                  .Where(w => w.FK_tbl_Inv_SalesNoteMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_SalesNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          o.Rate,
                          o.GrossAmount,
                          o.STPercentage,
                          o.STAmount,
                          o.FSTPercentage,
                          o.FSTAmount,
                          o.WHTPercentage,
                          o.WHTAmount,
                          o.DiscountAmount,
                          o.NetAmount,
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.NoOfPackages,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Inv_OrderNoteDetail_ID
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostSalesNoteInvoiceDetail(tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                CRUD_Type.Value = "InsertByAc";
            }
            else if (operation == "Save Update")
            {
                CRUD_Type.Value = "UpdateByAc";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "DeleteByAc";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_SalesNoteDetail] 
             @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
            ,@ID={3},@FK_tbl_Inv_SalesNoteMaster_ID={4}
            ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
            ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={6}
            ,@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={7}
            ,@Quantity={8},@Rate={9},@GrossAmount={10},@STPercentage={11},@STAmount={12}
            ,@FSTPercentage={13},@FSTAmount={14},@WHTPercentage={15},@WHTAmount={16}
            ,@DiscountAmount={17},@NetAmount={18},@Remarks={19},@NoOfPackages={20}
            ,@CreatedBy={21},@CreatedDate={22},@ModifiedBy={23},@ModifiedDate={24},@FK_tbl_Inv_OrderNoteDetail_ID={25}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Inv_SalesNoteDetail.ID, tbl_Inv_SalesNoteDetail.FK_tbl_Inv_SalesNoteMaster_ID,
            tbl_Inv_SalesNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
            tbl_Inv_SalesNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
            tbl_Inv_SalesNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
            tbl_Inv_SalesNoteDetail.Quantity, tbl_Inv_SalesNoteDetail.Rate, tbl_Inv_SalesNoteDetail.GrossAmount,
            tbl_Inv_SalesNoteDetail.STPercentage, tbl_Inv_SalesNoteDetail.STAmount,
            tbl_Inv_SalesNoteDetail.FSTPercentage, tbl_Inv_SalesNoteDetail.FSTAmount, tbl_Inv_SalesNoteDetail.WHTPercentage, tbl_Inv_SalesNoteDetail.WHTAmount,
            tbl_Inv_SalesNoteDetail.DiscountAmount, tbl_Inv_SalesNoteDetail.NetAmount, tbl_Inv_SalesNoteDetail.Remarks, tbl_Inv_SalesNoteDetail.NoOfPackages,
            tbl_Inv_SalesNoteDetail.CreatedBy, tbl_Inv_SalesNoteDetail.CreatedDate, tbl_Inv_SalesNoteDetail.ModifiedBy, tbl_Inv_SalesNoteDetail.ModifiedDate, tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_ID);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region DetailReturn
        public async Task<PagedData<object>> LoadSalesNoteInvoiceDetailReturn(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesReturnNoteDetails
                                               .Where(w => w.FK_tbl_Inv_SalesNoteDetail_ID == MasterID)
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesReturnNoteDetails
                                  .Where(w => w.FK_tbl_Inv_SalesNoteDetail_ID == MasterID)
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          o.Rate,
                          o.DiscountAmount,
                          o.STPercentage,
                          o.FSTPercentage,
                          o.WHTPercentage,
                          o.NetAmount
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLSalesNote()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Sales Invoice",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = new List<string>(){ "Normal A4", "Letter Head", "Normal A4 With Outstanding", "Letter Head With Outstanding" }
                },
                 new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Sales Note",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = new List<string>(){ "Normal A4", "Letter Head" }
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Sales Invoice",
                    GroupBy = new List<string>(){ "WareHouse", "Supplier", "Product Name" },
                    OrderBy = new List<string>(){ "Doc Date", "Product Name" },
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Sales Note")
            {
                return await Task.Run(() => CurrentSalesNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Current Sales Invoice")
            {
                return await Task.Run(() => CurrentSalesNote2(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, "Current Sales Note", GroupID, userName));
            }
            else if (rn == "Register Sales Invoice")
            {
                return await Task.Run(() => RegisterSalesNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentSalesNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, (SeekBy == "Letter Head" ? 100f : 20f), (SeekBy == "Letter Head" ? 70f : 30f), "----- Sales Invoice -----", true, (SeekBy == "Letter Head" ? false : true), (SeekBy == "Letter Head" ? false : true), (SeekBy == "Letter Head" ? true : false));

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 6------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //col1
                        (float)(PageSize.A4.GetWidth() * 0.20), //col2
                        (float)(PageSize.A4.GetWidth() * 0.10),  //col3
                        (float)(PageSize.A4.GetWidth() * 0.20),  //col4
                        (float)(PageSize.A4.GetWidth() * 0.10),  //col5
                        (float)(PageSize.A4.GetWidth() * 0.30)  //col6    
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = "";
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Invoice#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Inv Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Ware House")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Customer")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Contact")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ContactPersonNumber"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Address")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["Address"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("NTN")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["NTN"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("STR")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["STR"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Email")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Email"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell(1, 6).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        CreatedBy = sqlReader["CreatedBy"].ToString();

                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 11------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.08), // SNo
                        (float)(PageSize.A4.GetWidth() * 0.09), // ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.05), // Expiry Date
                        (float)(PageSize.A4.GetWidth() * 0.35), // ProductName
                        (float)(PageSize.A4.GetWidth() * 0.09), // Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05), // MeasurementUnit                        
                        (float)(PageSize.A4.GetWidth() * 0.05), // Rate
                        (float)(PageSize.A4.GetWidth() * 0.05), // STPercentage
                        (float)(PageSize.A4.GetWidth() * 0.05), // FSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.05), // DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.09)  // NetAmount
                        
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 12).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference #")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Expiry")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("ST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("FST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(176, 196, 222)));

                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalNetAmount = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ExpiryDate"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["STPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        
                        SNo++;
                        TotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                    }
                }

                pdftableDetail.AddCell(new Cell(1, 10).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 1).Add(new Paragraph().Add(string.Format("{0:n0}", TotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> CurrentSalesNote2(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, (SeekBy.Contains("Letter Head") ? 100f : 20f), (SeekBy.Contains("Letter Head") ? 70f : 30f), "----- Sales Invoice -----", true, (SeekBy.Contains("Letter Head") ? false : true), (SeekBy.Contains("Letter Head") ? false : true), (SeekBy.Contains("Letter Head") ? true : false));
            var ColorSteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorLightSteelBlue = new MyDeviceRgb(MyColor.LightSteelBlue).color;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.14), //
                        (float)(PageSize.A4.GetWidth() * 0.36), //
                        (float)(PageSize.A4.GetWidth() * 0.14), //
                        (float)(PageSize.A4.GetWidth() * 0.36)  //
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = ""; double OutStanding = 0; DateTime InvoiceDate = DateTime.Now;
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBorderTop(new SolidBorder(0.5f)).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["LicenseTo"].ToString())).SetBorderTop(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["Address"].ToString())).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["LicenseToAddress"].ToString())).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Tel #")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Telephone"].ToString())).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Invoice No")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Email")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Email"].ToString())).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Invoice Date")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Transporter")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TransporterName"].ToString())).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Tran Bilty#")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TransporterBiltyNo"].ToString())).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("NTN")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["NTN"].ToString())).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("NTN")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["LicenseToNTN"].ToString())).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("STR")).SetBorderBottom(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["STR"].ToString())).SetBorderBottom(new SolidBorder(0.5f)).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("STR")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["LicenseToSTN"].ToString())).SetBorderBottom(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));


                        pdftableMaster.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        CreatedBy = sqlReader["CreatedBy"].ToString();
                        OutStanding = Convert.ToDouble(sqlReader["OutStanding"]);
                        InvoiceDate = Convert.ToDateTime(sqlReader["DocDate"]);
                    }
                }
                page.InsertContent(pdftableMaster);
           
                /////////////------------------------------table for detail 10------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {                      
                        (float)(PageSize.A4.GetWidth() * 0.38), // ProductName
                        (float)(PageSize.A4.GetWidth() * 0.09), // ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.12), // Quantity
                        (float)(PageSize.A4.GetWidth() * 0.04), // Unit
                        (float)(PageSize.A4.GetWidth() * 0.09), // Rate
                        (float)(PageSize.A4.GetWidth() * 0.04), // STTax
                        (float)(PageSize.A4.GetWidth() * 0.04), // FSTTax
                        (float)(PageSize.A4.GetWidth() * 0.04), // WHTax
                        (float)(PageSize.A4.GetWidth() * 0.05), // DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.11)  // NetAmount
                        
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Batch #")).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("ST")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("FST")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("WHT")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Gross Value")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalGrossAmount = 0, TotalDiscount = 0, TotalNetAmount = 0, TotalWHT = 0, TotalST = 0, TotalFST = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {

                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["STPercentage"].ToString() + "%")).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FSTPercentage"].ToString() + "%")).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WHTPercentage"].ToString() + "%")).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                        TotalDiscount += Convert.ToDouble(sqlReader["DiscountAmount"]);
                        TotalGrossAmount += Convert.ToDouble(sqlReader["GrossAmount"]);
                        TotalST += Convert.ToDouble(sqlReader["STAmount"]);
                        TotalFST += Convert.ToDouble(sqlReader["FSTAmount"]);
                        TotalWHT += Convert.ToDouble(sqlReader["WHTAmount"]);
                        TotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                    }
                }

                pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Total Gross")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", TotalGrossAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Total Disc")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", TotalDiscount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Total ST")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", TotalST))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                if (SeekBy.Contains("With Outstanding"))
                {
                    pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Previous Balance")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", OutStanding - TotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                }
                else
                    pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Total FST")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", TotalFST))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                if (SeekBy.Contains("With Outstanding"))
                {
                    pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("This Invoice Balance")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", TotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                }
                else
                    pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Total WHT")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", TotalWHT))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                if (SeekBy.Contains("With Outstanding"))
                {
                    pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Current Balance as On " + InvoiceDate.ToString("dd-MMM-yyyy hh:mm tt"))).SetBold().SetBackgroundColor(ColorLightSteelBlue).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", OutStanding))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBackgroundColor(ColorLightSteelBlue).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                }
                else
                    pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Total Net")).SetBold().SetBackgroundColor(ColorLightSteelBlue).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", TotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBackgroundColor(ColorLightSteelBlue).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Issued By: " + userName)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterSalesNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Sales Invoice Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 14------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.13),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.16),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.18),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.05),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//MeasurementUnit 
                        (float)(PageSize.A4.GetWidth() * 0.05),//ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//Rate
                        (float)(PageSize.A4.GetWidth() * 0.03),//STPercentage
                        (float)(PageSize.A4.GetWidth() * 0.03),//FSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.05),//DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.07),//NetAmount
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference #")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("ST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("FST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0, GrandTotalNetAmount = 0, GroupTotalNetAmount = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "WareHouse" ? "WareHouseName" :
                                          GroupBy == "Supplier" ? "AccountName" :
                                          GroupBy == "Product Name" ? "ProductName" :
                                          "";

                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {
                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0; GroupTotalNetAmount = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["STPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        GroupTotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                        GrandTotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion
    }
    public class SalesReturnNoteInvoiceRepository : ISalesReturnNoteInvoice
    {
        private readonly OreasDbContext db;
        public SalesReturnNoteInvoiceRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetSalesReturnNoteInvoiceMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_SalesReturnNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.FK_tbl_Ac_CustomerSubDistributorList_ID,
                          FK_tbl_Ac_CustomerSubDistributorList_IDName = o?.tbl_Ac_CustomerSubDistributorList?.Name ?? "",
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter,
                          FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter.HasValue ? o.tbl_Ac_ChartOfAccounts_Transporter.AccountName : "",
                          o.TransportCharges,
                          o.TransporterBiltyNo,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSalesReturnNoteInvoiceMaster()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBSalesReturnNoteInvoiceMaster()
        {
            return new[]
            {
                new { n = "by Not Processed", v = "byNotProcessed" }, new { n = "by Not Supervised", v = "byNotSupervised" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesReturnNoteInvoiceMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesReturnNoteMasters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_SalesReturnNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .Where(w =>
                                                         string.IsNullOrEmpty(FilterByLoad)
                                                         ||
                                                         FilterByLoad == "byNotProcessed" && !w.IsProcessedAll
                                                         ||
                                                         FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesReturnNoteMasters
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_SalesReturnNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byNotProcessed" && !w.IsProcessedAll
                                            ||
                                            FilterByLoad == "byNotSupervised" && !w.IsSupervisedAll
                                        )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.FK_tbl_Ac_CustomerSubDistributorList_ID,
                          FK_tbl_Ac_CustomerSubDistributorList_IDName = o?.tbl_Ac_CustomerSubDistributorList?.Name ?? "",
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter,
                          FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter.HasValue ? o.tbl_Ac_ChartOfAccounts_Transporter.AccountName : "",
                          o.TransportCharges,
                          o.TransporterBiltyNo,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostSalesReturnNoteInvoiceMaster(tbl_Inv_SalesReturnNoteMaster tbl_Inv_SalesReturnNoteMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                CRUD_Type.Value = "InsertByAc";
            }
            else if (operation == "Save Update")
            {
                CRUD_Type.Value = "UpdateByAc";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "DeleteByAc";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_SalesReturnNoteMaster] 
            @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
            @ID={3},@DocNo={4},@DocDate={5},
            @FK_tbl_Inv_WareHouseMaster_ID={6},@FK_tbl_Ac_ChartOfAccounts_ID={7},@FK_tbl_Ac_CustomerSubDistributorList_ID={8},@Remarks={9},
            @FK_tbl_Ac_ChartOfAccounts_ID_Transporter={10},@TransportCharges={11},@TransporterBiltyNo={12},
            @CreatedBy={13},@CreatedDate={14},@ModifiedBy={15},@ModifiedDate={16}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Inv_SalesReturnNoteMaster.ID, tbl_Inv_SalesReturnNoteMaster.DocNo, tbl_Inv_SalesReturnNoteMaster.DocDate,
            tbl_Inv_SalesReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_ID, tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_CustomerSubDistributorList_ID, tbl_Inv_SalesReturnNoteMaster.Remarks,
            tbl_Inv_SalesReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID_Transporter, tbl_Inv_SalesReturnNoteMaster.TransportCharges, tbl_Inv_SalesReturnNoteMaster.TransporterBiltyNo,
            tbl_Inv_SalesReturnNoteMaster.CreatedBy, tbl_Inv_SalesReturnNoteMaster.CreatedDate, tbl_Inv_SalesReturnNoteMaster.ModifiedBy, tbl_Inv_SalesReturnNoteMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetSalesReturnNoteInvoiceDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_SalesReturnNoteDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_SalesReturnNoteMaster_ID,
                          o.FK_tbl_Inv_SalesNoteDetail_ID,
                          FK_tbl_Inv_SalesNoteDetail_IDName = o.tbl_Inv_SalesNoteDetail.tbl_Inv_SalesNoteMaster.DocNo,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          o.Rate,
                          o.GrossAmount,
                          o.STPercentage,
                          o.STAmount,
                          o.FSTPercentage,
                          o.FSTAmount,
                          o.WHTPercentage,
                          o.WHTAmount,
                          o.DiscountAmount,
                          o.NetAmount,
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSalesReturnNoteInvoiceDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesReturnNoteInvoiceDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesReturnNoteDetails
                                               .Where(w => w.FK_tbl_Inv_SalesReturnNoteMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesReturnNoteDetails
                                  .Where(w => w.FK_tbl_Inv_SalesReturnNoteMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_SalesReturnNoteMaster_ID,
                          o.FK_tbl_Inv_SalesNoteDetail_ID,
                          FK_tbl_Inv_SalesNoteDetail_IDName = o.tbl_Inv_SalesNoteDetail.tbl_Inv_SalesNoteMaster.DocNo,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          o.Rate,
                          o.GrossAmount,
                          o.STPercentage,
                          o.STAmount,
                          o.FSTPercentage,
                          o.FSTAmount,
                          o.WHTPercentage,
                          o.WHTAmount,
                          o.DiscountAmount,
                          o.NetAmount,
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostSalesReturnNoteInvoiceDetail(tbl_Inv_SalesReturnNoteDetail tbl_Inv_SalesReturnNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                CRUD_Type.Value = "InsertByAc";
            }
            else if (operation == "Save Update")
            {
                CRUD_Type.Value = "UpdateByAc";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "DeleteByAc";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_SalesReturnNoteDetail] 
             @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
            ,@ID={3},@FK_tbl_Inv_SalesReturnNoteMaster_ID={4},@FK_tbl_Inv_SalesNoteDetail_ID={5}
            ,@FK_tbl_Inv_ProductRegistrationDetail_ID={6}
            ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={7}
            ,@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={8}
            ,@Quantity={9},@Rate={10},@GrossAmount={11},@STPercentage={12},@STAmount={13}
            ,@FSTPercentage={14},@FSTAmount={15},@WHTPercentage={16},@WHTAmount={17}
            ,@DiscountAmount={18},@NetAmount={19},@Remarks={20}
            ,@CreatedBy={21},@CreatedDate={22},@ModifiedBy={23},@ModifiedDate={24}",
             CRUD_Type, CRUD_Msg, CRUD_ID,
             tbl_Inv_SalesReturnNoteDetail.ID, tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesReturnNoteMaster_ID, tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_SalesNoteDetail_ID,
             tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
             tbl_Inv_SalesReturnNoteDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
             tbl_Inv_SalesReturnNoteDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
             tbl_Inv_SalesReturnNoteDetail.Quantity, tbl_Inv_SalesReturnNoteDetail.Rate, tbl_Inv_SalesReturnNoteDetail.GrossAmount,
             tbl_Inv_SalesReturnNoteDetail.STPercentage, tbl_Inv_SalesReturnNoteDetail.STAmount,
             tbl_Inv_SalesReturnNoteDetail.FSTPercentage, tbl_Inv_SalesReturnNoteDetail.FSTAmount, tbl_Inv_SalesReturnNoteDetail.WHTPercentage, tbl_Inv_SalesReturnNoteDetail.WHTAmount,
             tbl_Inv_SalesReturnNoteDetail.DiscountAmount, tbl_Inv_SalesReturnNoteDetail.NetAmount, tbl_Inv_SalesReturnNoteDetail.Remarks,
             tbl_Inv_SalesReturnNoteDetail.CreatedBy, tbl_Inv_SalesReturnNoteDetail.CreatedDate, tbl_Inv_SalesReturnNoteDetail.ModifiedBy, tbl_Inv_SalesReturnNoteDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLSalesReturnNote()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Sales Return Note",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Sales Return Invoice",
                    GroupBy = new List<string>(){ "WareHouse", "Supplier", "Product Name" },
                    OrderBy = new List<string>(){ "Doc Date", "Product Name" },
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Sales Return Note")
            {
                return await Task.Run(() => CurrentSalesReturnNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Sales Return Invoice")
            {
                return await Task.Run(() => RegisterSalesReturnNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentSalesReturnNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Sales Return Invoice -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocNo
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocDate
                        (float)(PageSize.A4.GetWidth() * 0.30),  //WareHouseName
                        (float)(PageSize.A4.GetWidth() * 0.40)  //AccountName    
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Customer")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string CreatedBy = "";
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 10------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), // SNo
                        (float)(PageSize.A4.GetWidth() * 0.10), // ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.35), // ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10), // Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05), // MeasurementUnit                        
                        (float)(PageSize.A4.GetWidth() * 0.05), // Rate
                        (float)(PageSize.A4.GetWidth() * 0.05), // GSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.05), // FSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.05), // DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.10)  // NetAmount
                        
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 11).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference #")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("ST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("FST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;
                double TotalNetAmount = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["STPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                        TotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                    }
                }

                pdftableDetail.AddCell(new Cell(1, 9).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 1).Add(new Paragraph().Add(string.Format("{0:n0}", TotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterSalesReturnNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Sales Return Invoice Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 14------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.13),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.16),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.18),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.05),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//MeasurementUnit 
                        (float)(PageSize.A4.GetWidth() * 0.05),//ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//Rate
                        (float)(PageSize.A4.GetWidth() * 0.03),//STPercentage
                        (float)(PageSize.A4.GetWidth() * 0.03),//FSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.05),//DiscountAmount
                        (float)(PageSize.A4.GetWidth() * 0.07),//NetAmount
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference #")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("ST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("FST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Disc")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0, GrandTotalNetAmount = 0, GroupTotalNetAmount = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Challan_Ac_Invoice] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                string GroupbyValue = string.Empty;
                string GroupbyFieldName = GroupBy == "WareHouse" ? "WareHouseName" :
                                          GroupBy == "Supplier" ? "AccountName" :
                                          GroupBy == "Product Name" ? "ProductName" :
                                          "";

                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {
                            if (!string.IsNullOrEmpty(GroupbyValue))
                            {
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0; GroupTotalNetAmount = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["STPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DiscountAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["NetAmount"]))).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        GroupTotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                        GrandTotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GroupTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 14).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalNetAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();

        }
        #endregion
    }
    public class AcLedgerRepository : IAcLedger
    {
        private readonly OreasDbContext db;
        public AcLedgerRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region  AcLedger
        public async Task<PagedData<object>> LoadAcLedger(int CurrentPage = 1, int MasterID = 0, bool? TStatus = null, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
      
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_Ledgers
                                               .Where(w => w.FK_tbl_Ac_ChartOfAccounts_ID == MasterID && (TStatus == null || w.Posted == TStatus))
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                     )
                                               .Where(w => w.PostingDate >= FilterValueByDateRangeFrom && w.PostingDate <= FilterValueByDateRangeTill)
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_Ledgers
                                        .Where(w => w.FK_tbl_Ac_ChartOfAccounts_ID == MasterID && (TStatus == null || w.Posted == TStatus))
                                        .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                          )
                                        .Where(w => w.PostingDate >= FilterValueByDateRangeFrom && w.PostingDate <= FilterValueByDateRangeTill)
                                        .OrderBy(i => i.PostingDate).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          o.Debit,
                          o.Credit,
                          o.Narration,
                          o.Posted,
                          PostingDate = o.PostingDate.ToString("dd-MMM-yy hh:mm tt"),
                          o.PostingNo,
                          o.FK_tbl_Ac_V_BankDocumentDetail_ID,
                          o.FK_tbl_Ac_V_CashDocumentDetail_ID,
                          o.FK_tbl_Ac_V_JournalDocumentDetail_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID,
                          o.FK_tbl_Inv_PurchaseReturnNoteDetail_ID,
                          o.FK_tbl_Inv_SalesNoteDetail_ID,
                          o.FK_tbl_Inv_SalesReturnNoteDetail_ID,
                          Ref = o.TrackingNo ?? ""
                      };

            pageddata.Data = qry;
            pageddata.otherdata = 
                new{ 
                    Opening = Convert.ToDecimal(db.tbl_Ac_Ledgers.Where(w=> w.FK_tbl_Ac_ChartOfAccounts_ID == MasterID && (TStatus == null || w.Posted == TStatus) && w.PostingDate<FilterValueByDateRangeFrom).Sum(s=> s.Debit-s.Credit)),
                    Closing = Convert.ToDecimal(db.tbl_Ac_Ledgers.Where(w => w.FK_tbl_Ac_ChartOfAccounts_ID == MasterID && (TStatus == null || w.Posted == TStatus) && w.PostingDate <= FilterValueByDateRangeTill).Sum(s => s.Debit - s.Credit))
                };

            return pageddata;
        }

        #endregion

        #region Report     

        public List<ReportCallingModel> GetRLAcLedger()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Ledger",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }

        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Ledger")
            {
                return await Task.Run(() => LedgerAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Trial3")
            {
                return await Task.Run(() => Trial3Async(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "TrialDetail")
            {
                return await Task.Run(() => TrialDetailAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> TrialDetailAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + "Trial Balance Detail Period Date From: " + datefrom.Value.ToString("dd-MMM-yyyy") + " To " + datetill.Value.ToString("dd-MMM-yyyy") + "-----", true);
            var ColorSteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorMaroon = new MyDeviceRgb(MyColor.Maroon).color;
            //--------------------------------5 column table ------------------------------//
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.60),//AccountName3
                        (float)(PageSize.A4.GetWidth()*0.10),//Opening
                        (float)(PageSize.A4.GetWidth()*0.10),//Debit
                        (float)(PageSize.A4.GetWidth()*0.10),//Credit
                        (float)(PageSize.A4.GetWidth()*0.10) //Closing
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();

                 

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    { 
                        pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["AllParentAccountNames"].ToString())).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    }
                }

                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Account Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Opening")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Debit")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Credit")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Closing")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                double GrandOpening = 0, GrandDebit = 0, GrandCredit = 0,  GrandClosing = 0, EvaluateClosing = 0;
                ReportName.Value = rn + "2";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {

                        EvaluateClosing = 0;
                        EvaluateClosing = Convert.ToDouble(sqlReader["Opening"]) + Convert.ToDouble(sqlReader["TotalDebit"]) - Convert.ToDouble(sqlReader["TotalCredit"]);

                        if((bool)sqlReader["IsTransactional"])
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(new Link(sqlReader["AccountName"].ToString(), PdfAction.CreateURI(uri + "?rn=Ledger&id=" + sqlReader["ID"].ToString() + "&datefrom=" + datefrom + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=")))).SetFontColor(ColorMaroon).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        else
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(new Link(sqlReader["AccountName"].ToString(), PdfAction.CreateURI(uri + "?rn=TrialDetail&id=" + sqlReader["ID"].ToString() + "&datefrom=" + datefrom + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=")))).SetFontColor(ColorSteelBlue).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Opening"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["TotalDebit"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["TotalCredit"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", EvaluateClosing))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        GrandOpening += Convert.ToDouble(sqlReader["Opening"]);
                        GrandDebit += Convert.ToDouble(sqlReader["TotalDebit"]);
                        GrandCredit += Convert.ToDouble(sqlReader["TotalCredit"]);
                        GrandClosing += EvaluateClosing;
                    }
                }

                pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add("\n")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandOpening))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandDebit))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandCredit))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandClosing))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);


            }

            page.InsertContent(pdftableMain);

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> Trial3Async(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + "Trial Balance Level 3 Period Date From: " + datefrom.Value.ToString("dd-MMM-yyyy") + " To " + datetill.Value.ToString("dd-MMM-yyyy") + "-----", true);
            
            //--------------------------------5 column table ------------------------------//
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.60),//AccountName3
                        (float)(PageSize.A4.GetWidth()*0.10),//Opening
                        (float)(PageSize.A4.GetWidth()*0.10),//Debit
                        (float)(PageSize.A4.GetWidth()*0.10),//Credit
                        (float)(PageSize.A4.GetWidth()*0.10) //Closing
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();

                string AcL1 = "", AcL2 = "";

                double GrandOpening = 0, SubOpening = 0, GrandDebit = 0, SubDebit = 0, GrandCredit = 0, SubCredit = 0, GrandClosing = 0, SubClosing = 0;
                double EvaluateClosing = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (AcL1 != sqlReader["AccountName"].ToString())
                        {
                            if (!string.IsNullOrEmpty(AcL2))
                            {
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", SubOpening))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", SubDebit))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", SubCredit))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", SubClosing))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);

                                SubOpening = 0; SubDebit = 0; SubCredit = 0; SubClosing = 0;
                            }

                            AcL1 = sqlReader["AccountName"].ToString();
                            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(AcL1)).SetFontSize(8).SetBackgroundColor(new DeviceRgb(77, 148, 255)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            if (AcL2 != sqlReader["AccountName2"].ToString())
                            {                                

                                AcL2 = sqlReader["AccountName2"].ToString();
                                pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(AcL2)).SetFontSize(7).SetBackgroundColor(new DeviceRgb(153, 194, 255)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("")).SetFontSize(7).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Account Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Opening")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Debit")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Credit")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Closing")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            }

                        }
                        EvaluateClosing = 0;
                        EvaluateClosing = Convert.ToDouble(sqlReader["Opening"]) + Convert.ToDouble(sqlReader["TotalDebit"]) - Convert.ToDouble(sqlReader["TotalCredit"]);

                        //pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName3"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(new Link(sqlReader["AccountName3"].ToString(), PdfAction.CreateURI(uri + "?rn=TrialDetail&id=" + sqlReader["ID"].ToString() + "&datefrom=" + datefrom + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=")))).SetFontColor(new DeviceRgb(0, 102, 204)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Opening"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["TotalDebit"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["TotalCredit"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", EvaluateClosing))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SubOpening += Convert.ToDouble(sqlReader["Opening"]);
                        GrandOpening += Convert.ToDouble(sqlReader["Opening"]);

                        SubDebit += Convert.ToDouble(sqlReader["TotalDebit"]);
                        GrandDebit += Convert.ToDouble(sqlReader["TotalDebit"]);

                        SubCredit += Convert.ToDouble(sqlReader["TotalCredit"]);
                        GrandCredit += Convert.ToDouble(sqlReader["TotalCredit"]);

                        SubClosing += EvaluateClosing;
                        GrandClosing += EvaluateClosing;


                    }
                }

                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", SubOpening))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", SubDebit))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", SubCredit))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", SubClosing))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);

                pdftableMain.AddCell(new Cell(1,5).Add(new Paragraph().Add("\n")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandOpening))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandDebit))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandCredit))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", GrandClosing))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);


            }

            page.InsertContent(pdftableMain);

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> LedgerAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + "Financial Ledger Period Date From: " + datefrom.Value.ToString("dd-MMM-yyyy") + " To " + datetill.Value.ToString("dd-MMM-yyyy") + "-----", true);
                       

            //--------------------------------7 column table ------------------------------//
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.15),//PostingDate
                        (float)(PageSize.A4.GetWidth()*0.40),//Narration
                        (float)(PageSize.A4.GetWidth()*0.05),//Posted
                        (float)(PageSize.A4.GetWidth()*0.10),//Reference
                        (float)(PageSize.A4.GetWidth()*0.10),//Debit
                        (float)(PageSize.A4.GetWidth()*0.10),//Credit
                        (float)(PageSize.A4.GetWidth()*0.10) //Balance
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();

                decimal Balance = 0;

                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Account Name:")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true)).SetFontSize(7);
                        pdftableMain.AddCell(new Cell(1,4).Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true)).SetFontSize(7);

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Opening:")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Opening"]))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        
                        Balance += Convert.ToDecimal(sqlReader["Opening"]);
                    }
                }
                
                ReportName.Value = rn + "2";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Posted")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Reference")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Debit")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Credit")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Balance")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                    while (sqlReader.Read())
                    {
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MM-yy hh:mm tt"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Posted"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Reference"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Debit"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Credit"]))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
             
                        Balance += Convert.ToDecimal(sqlReader["Debit"]) - Convert.ToDecimal(sqlReader["Credit"]);

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", Balance))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                    }
                    
                }

                pdftableMain.AddCell(new Cell(1,5).Add(new Paragraph().Add(" ")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Closing:")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", Balance))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true)).SetFontSize(7);

            }

            page.InsertContent(pdftableMain);

            return page.FinishToGetBytes();
        }

        #endregion
    }
    public class PolicyPaymentTermRepository : IPolicyPaymentTerm
    {
        private readonly OreasDbContext db;
        public PolicyPaymentTermRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        public async Task<object> GetPolicyPaymentTerm(int id)
        {
            var qry = from o in await db.tbl_Ac_PolicyPaymentTerms.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.Name,
                          o.DaysLimit,
                          o.AdvancePercentage,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPolicyPaymentTerm()
        {
            return new[]
            {
                new { n = "by Term Name", v = "byTermName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPolicyPaymentTerm(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_PolicyPaymentTerms
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTermName" && w.Name.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_PolicyPaymentTerms
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byTermName" && w.Name.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.Name,
                          o.DaysLimit,
                          o.AdvancePercentage,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalAc = o.tbl_Ac_ChartOfAccountss.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPolicyPaymentTerm(tbl_Ac_PolicyPaymentTerm tbl_Ac_PolicyPaymentTerm, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Ac_PolicyPaymentTerm.CreatedBy = userName;
                tbl_Ac_PolicyPaymentTerm.CreatedDate = DateTime.Now;
                db.tbl_Ac_PolicyPaymentTerms.Add(tbl_Ac_PolicyPaymentTerm);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_PolicyPaymentTerm.ModifiedBy = userName;
                tbl_Ac_PolicyPaymentTerm.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Ac_PolicyPaymentTerm).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Ac_PolicyPaymentTerms.Remove(db.tbl_Ac_PolicyPaymentTerms.Find(tbl_Ac_PolicyPaymentTerm.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #region COA Linking
        public object GetWCLPolicyPaymentTermCOA()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPolicyPaymentTermCOA(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_ChartOfAccountss
                                               .Where(w => w.FK_tbl_Ac_PolicyPaymentTerm_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_ChartOfAccountss
                                  .Where(w => w.FK_tbl_Ac_PolicyPaymentTerm_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.AccountName).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_PolicyPaymentTerm_ID,
                          o.ParentID,
                          ParentName = o.ParentID.HasValue ? db.tbl_Ac_ChartOfAccountss.Where(i => i.ID == o.ParentID).FirstOrDefault().AccountName : "",
                          o.FK_tbl_Ac_ChartOfAccounts_Type_ID,
                          FK_tbl_Ac_ChartOfAccounts_Type_IDName = o.tbl_Ac_ChartOfAccounts_Type.AccountType,
                          o.AccountCode,
                          o.AccountName,
                          o.IsTransactional

                      };


            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

    }

    //---------------------Production------------------------//
    public class CompositionCostingRepository : ICompositionCosting
    {
        private readonly OreasDbContext db;
        public CompositionCostingRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Composition Master
        public object GetWCLCompositionMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));
            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.tbl_Pro_CompositionDetail_Coupling.BatchSize,
                          SemiProduct = o.tbl_Pro_CompositionDetail_Coupling.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          SemiProductUnit = o.tbl_Pro_CompositionDetail_Coupling.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          PrimaryProduct = o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "]",
                          SecondaryProduct = o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary > 0 ? o.tbl_Inv_ProductRegistrationDetail_Secondary.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail_Secondary.tbl_Inv_MeasurementUnit.MeasurementUnit + "]" : "",
                          o.PackagingName,
                          o.tbl_Pro_CompositionDetail_Coupling.FK_tbl_Pro_CompositionMaster_ID

                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Composition Detail Raw
        public async Task<object> GetCompositionDetailRaw(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_RawDetail_Itemss.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_RawMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CustomeRate,
                          o.PercentageOnRate
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCompositionDetailRaw()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionDetailRaw(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_RawDetail_Itemss
                                      .Where(w => w.tbl_Pro_CompositionDetail_RawMaster.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));
            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_RawDetail_Itemss
                                        .Where(w => w.tbl_Pro_CompositionDetail_RawMaster.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                               )
                                        .OrderByDescending(i => i.FK_tbl_Pro_CompositionDetail_RawMaster_ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_RawMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CustomeRate,
                          o.PercentageOnRate
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCompositionDetailRaw(tbl_Pro_CompositionDetail_RawDetail_Items tbl_Pro_CompositionDetail_RawDetail_Items, string operation = "", string userName = "")
        {
            if (operation == "Save Update")
            {
                db.Entry(tbl_Pro_CompositionDetail_RawDetail_Items).State = EntityState.Unchanged;

                db.Entry(tbl_Pro_CompositionDetail_RawDetail_Items).Property(x => x.CustomeRate).IsModified = true;
                db.Entry(tbl_Pro_CompositionDetail_RawDetail_Items).Property(x => x.PercentageOnRate).IsModified = true;

                await db.SaveChangesAsync();

            }
            return "OK";
        }
        #endregion

        #region Composition Detail Packaging
        public async Task<object> GetCompositionDetailPackaging(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CustomeRate,
                          o.PercentageOnRate
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCompositionDetailPackaging()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionDetailPackaging(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss
                                      .Where(w => w.tbl_Pro_CompositionDetail_Coupling_PackagingDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));
            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss
                                        .Where(w => w.tbl_Pro_CompositionDetail_Coupling_PackagingDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                               )
                                        .OrderByDescending(i => i.FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CustomeRate,
                          o.PercentageOnRate
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCompositionDetailPackaging(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items, string operation = "", string userName = "")
        {
            if (operation == "Save Update")
            {

                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items).State = EntityState.Unchanged;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items).Property(x => x.CustomeRate).IsModified = true;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items).Property(x => x.PercentageOnRate).IsModified = true;
                await db.SaveChangesAsync();

            }
            return "OK";
        }
        #endregion

        #region Report     
        public List<ReportCallingModel> GetRLCompositionDetail()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Composition Complete",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Composition Complete")
            {
                return await Task.Run(() => CompositionCompleteAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CompositionCompleteAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + rn + " -----", true);

            //--------------------------------8 column table ------------------------------//
            Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.10),//
                        (float)(PageSize.A4.GetWidth()*0.15),//
                        (float)(PageSize.A4.GetWidth()*0.10),//
                        (float)(PageSize.A4.GetWidth()*0.15),//
                        (float)(PageSize.A4.GetWidth()*0.10),//
                        (float)(PageSize.A4.GetWidth()*0.15),//
                        (float)(PageSize.A4.GetWidth()*0.10),//
                        (float)(PageSize.A4.GetWidth()*0.15) //
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);
            //--------------------------------10 column Raw table ------------------------------//
            Table pdftableDetailRaw = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05),//SNo
                        (float)(PageSize.A4.GetWidth()*0.10),//FilterPolicy
                        (float)(PageSize.A4.GetWidth()*0.35),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.10),//Quantity
                        (float)(PageSize.A4.GetWidth()*0.07),//LastRate
                        (float)(PageSize.A4.GetWidth()*0.02),//LastFrom
                        (float)(PageSize.A4.GetWidth()*0.07),//LastDate                                     
                        (float)(PageSize.A4.GetWidth()*0.10),//CustomRate 
                        (float)(PageSize.A4.GetWidth()*0.04), //PerOnRate 
                        (float)(PageSize.A4.GetWidth()*0.10) //Amount           
            }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            
            
            pdftableDetailRaw.AddCell(new Cell(1, 10).Add(new Paragraph().Add("Raw Detail")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Formula Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(106, 127, 196)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("From")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(106, 127, 196)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Date")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(106, 127, 196)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Cust Rate")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(237, 226, 123)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("%")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
           
           
            //--------------------------------10 column Packaging table ------------------------------//
            Table pdftableDetailPackaging = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05),//SNo
                        (float)(PageSize.A4.GetWidth()*0.10),//FilterPolicy
                        (float)(PageSize.A4.GetWidth()*0.35),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.10),//Quantity
                        (float)(PageSize.A4.GetWidth()*0.07),//LastRate
                        (float)(PageSize.A4.GetWidth()*0.02),//LastFrom
                        (float)(PageSize.A4.GetWidth()*0.07),//LastDate                                     
                        (float)(PageSize.A4.GetWidth()*0.10),//CustomRate 
                        (float)(PageSize.A4.GetWidth()*0.04), //PerOnRate 
                        (float)(PageSize.A4.GetWidth()*0.10) //Amount           
            }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableDetailPackaging.AddCell(new Cell(1, 10).Add(new Paragraph().Add("Packaging Detail")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Formula Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Last Rate")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(106, 127, 196)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("From")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(106, 127, 196)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Date")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(106, 127, 196)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Cust Rate")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(237, 226, 123)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("%")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_Costing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                int FK_tbl_Pro_CompositionMaster_ID = 0; double BatchSize = 1; double SplitInto = 1;
                //----------master
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        //----------Row 1
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No:")).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date:")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yyyy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Shelf Life:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ShelfLifeInMonths"].ToString() + " Month(s)")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch Size:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + sqlReader["SemiMeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        //----------Row 2
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Composition:")).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["CompositionName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Product:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["SemiProductName"].ToString()).SetFontSize(6)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        //----------Row 3
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Packaging:")).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["PackagingName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Primary:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PrimarySplitInto"].ToString() + " x " + sqlReader["PrimaryMeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //------------setting parameter for second query of Raw ingredients

                        FK_tbl_Pro_CompositionMaster_ID = (int)sqlReader["FK_tbl_Pro_CompositionMaster_ID"];
                        BatchSize = (double)sqlReader["BatchSize"];
                        SplitInto = (double)sqlReader["PrimarySplitInto"];
                    }                    
                }

                //----------raw detail
                ReportName.Value = rn + "2"; MasterID.Value = FK_tbl_Pro_CompositionMaster_ID;
                int SNo = 1; double RawAmountTotal = 0;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FilterName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RateFrom"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader.IsDBNull("RateDate") ? "" : ((DateTime)sqlReader["RateDate"]).ToString("MMM-yy"))).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CustomeRate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PercentageOnRate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Amount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        RawAmountTotal += (double)sqlReader["Amount"];
                        SNo++;
                    }

                    pdftableDetailRaw.AddCell(new Cell(1,8).Add(new Paragraph().Add("Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableDetailRaw.AddCell(new Cell(1,2).Add(new Paragraph().Add(RawAmountTotal.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                }

                //----------Packaging detail
                ReportName.Value = rn + "3"; MasterID.Value = id;
                SNo = 1; double PackagingAmountTotal = 0;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FilterName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RateFrom"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader.IsDBNull("RateDate") ? "": ((DateTime)sqlReader["RateDate"]).ToString("MMM-yy"))).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CustomeRate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PercentageOnRate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Amount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        PackagingAmountTotal += (double)sqlReader["Amount"];
                        SNo++;
                    }

                    pdftableDetailPackaging.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableDetailPackaging.AddCell(new Cell(1, 2).Add(new Paragraph().Add(PackagingAmountTotal.ToString()).SetTextAlignment(TextAlignment.RIGHT)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                }


                //----------Factors detail
                ReportName.Value = rn + "4"; double TotalPerUnit = 0; string FormulaExpresion = ""; double FormulaValue = 0;

                //----------Row 3 coninute last cell-------//
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Total Amount")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add((RawAmountTotal + PackagingAmountTotal).ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                double MaterialPerUnit = Math.Round(((RawAmountTotal + PackagingAmountTotal) / BatchSize * SplitInto), 3);
                double ValueFactor = (1 / BatchSize) * SplitInto;

                pdftableMaster.AddCell(new Cell(1, 4).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Material value Per Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Rs: " + MaterialPerUnit.ToString() + "/-")).SetBold().SetBackgroundColor(new DeviceRgb(131, 207, 103)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                TotalPerUnit = MaterialPerUnit;
                double FormulaFactorValue = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        FormulaExpresion= sqlReader["FormulaExpression"].ToString();

                        if (string.IsNullOrEmpty(FormulaExpresion)==false)
                        {
                            FormulaFactorValue = 0;

                            if (double.TryParse(FormulaExpresion, out FormulaFactorValue))
                            {
                                FormulaValue = Math.Round((FormulaFactorValue * ValueFactor) + TotalPerUnit, 3);
                            }
                            else
                            {
                                FormulaExpresion = FormulaExpresion.Replace("c", TotalPerUnit.ToString(), StringComparison.OrdinalIgnoreCase);
                                FormulaValue = Math.Round(AmountIntoWords.ExecuteMathExpression(FormulaExpresion), 3);
                            }  
                            pdftableMaster.AddCell(new Cell(1, 4).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["FormulaName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Rs: " + FormulaValue.ToString() + "/-")).SetBold().SetBackgroundColor(new DeviceRgb(237, 226, 123)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            TotalPerUnit = FormulaValue;
                        }
               
                    }

                }

                pdftableMaster.AddCell(new Cell(1, 8).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            }

            page.InsertContent(pdftableMaster);
            page.InsertContent(pdftableDetailRaw);
            page.InsertContent(pdftableDetailPackaging);

            return page.FinishToGetBytes();
        }

        #endregion

    }
    public class BMRCostingRepository : IBMRCosting
    {
        private readonly OreasDbContext db;
        public BMRCostingRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region BMRMaster
        public object GetWCLBMRMaster()
        {
            return new[]
           {
                new { n = "by DocNo", v = "byDocNo" }, new { n = "by BatchNo", v = "byBatchNo" }, new { n = "by SemiFinished ProductName", v = "bySemiFinishedProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionMasters
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "bySemiFinishedProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byBatchNo" && w.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));
            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMasters
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "bySemiFinishedProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                   ||
                                                   FilterByText == "byBatchNo" && w.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                   ||
                                                   FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy"),
                          o.BatchNo,
                          BatchMfgDate = o.BatchMfgDate.ToString("dd-MMM-yyyy"),
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          BatchSizeUnit = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.BatchSize,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_ID,
                          o.TotalProd,
                          o.Cost,
                          o.IsCompleted,
                          FinishedDate = o.FinishedDate.HasValue ? o.FinishedDate.Value.ToString("dd-MMM-yyyy hh:mm tt") : "",
                          o.IsDispensedR,
                          o.IsDispensedP,
                          o.IsQAClearanceBMRPending,
                          o.IsQAClearanceBPRPending,
                          o.IsQCSampleBMRPending,
                          o.IsQCSampleBPRPending,
                          TotalPackageBatchSize = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters?.Sum(s => s.BatchSize) ?? 0
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region BMRDetailPackagingMaster

        public object GetWCLBMRDetailPackagingMaster()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDetailPackagingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                              )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          o.PackagingName,
                          o.BatchSize,
                          BatchSizeUnit = o.tbl_Pro_BatchMaterialRequisitionMaster.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "'s " + o.tbl_Inv_ProductRegistrationDetail_Primary.Description,
                          o.Cost_Primary,
                          o.TotalProd_Primary,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          o.FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID,
                          OrderDetail = o.FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID > 0 ? "[O#: " + o.tbl_Inv_OrderNoteDetail_ProductionOrder.tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster.DocNo.ToString() + "] Customer: " + o.tbl_Inv_OrderNoteDetail_ProductionOrder.tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster.tbl_Ac_ChartOfAccounts.AccountName : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Report

        public List<ReportCallingModel> GetRLBMRDetail()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Complete",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Financial",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "BMR Complete")
            {
                return await Task.Run(() => BMRCompleteAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "BMR Financial")
            {
                return await Task.Run(() => BMRFinancialAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> BMRCompleteAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + rn + "-----", true);

            //--------------------------------8 column table ------------------------------//
            Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.10),//
                        (float)(PageSize.A4.GetWidth()*0.15),//
                        (float)(PageSize.A4.GetWidth()*0.10),//
                        (float)(PageSize.A4.GetWidth()*0.15),//
                        (float)(PageSize.A4.GetWidth()*0.10),//
                        (float)(PageSize.A4.GetWidth()*0.15),//
                        (float)(PageSize.A4.GetWidth()*0.10),//
                        (float)(PageSize.A4.GetWidth()*0.15) //
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);
            //--------------------------------7 column Raw table ------------------------------//
            Table pdftableDetailRaw = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05), //SNo
                        (float)(PageSize.A4.GetWidth()*0.35), //ProductName
                        (float)(PageSize.A4.GetWidth()*0.125),//Quantity
                        (float)(PageSize.A4.GetWidth()*0.10), //RefNo
                        (float)(PageSize.A4.GetWidth()*0.125),//DispensedQty
                        (float)(PageSize.A4.GetWidth()*0.10), //Rate
                        (float)(PageSize.A4.GetWidth()*0.15), //Amount
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            //--------------------------------7 column Packaging table ------------------------------//
            Table pdftableDetailPackaging = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05),//SNo
                        (float)(PageSize.A4.GetWidth()*0.35),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.125),//Quantity
                        (float)(PageSize.A4.GetWidth()*0.10), //RefNo
                        (float)(PageSize.A4.GetWidth()*0.125),//DispensedQty
                        (float)(PageSize.A4.GetWidth()*0.10),//Rate
                        (float)(PageSize.A4.GetWidth()*0.15), //Amount   
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            //--------------------------------7 column Additional table ------------------------------//
            Table pdftableAdditional = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05),//SNo
                        (float)(PageSize.A4.GetWidth()*0.30),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.05),//Issued Or Return
                        (float)(PageSize.A4.GetWidth()*0.15),//DispensedQty
                        (float)(PageSize.A4.GetWidth()*0.15), //RefNo
                        (float)(PageSize.A4.GetWidth()*0.15),//Rate
                        (float)(PageSize.A4.GetWidth()*0.15) //Amount 
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            string CreatedBy = "";
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_Costing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn + "1";
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();
                string BatchSizeUnit = ""; double BatchSize = 1; double TotalProd = 0;
                //----------master
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        //----------Row 1

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date:")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yyyy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Mfg Date:")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["BatchMfgDate"]).ToString("dd-MMM-yyyy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch No:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString()).SetBold()).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch Size:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + sqlReader["MeasurementUnit"].ToString()).SetBold()).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //----------Row 2
                        
                        pdftableMaster.AddCell(new Cell(1, 4).Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetBold().SetBackgroundColor(new DeviceRgb(141, 227, 118)).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Total Prod:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TotalProd"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        BatchSizeUnit = sqlReader["MeasurementUnit"].ToString();
                        BatchSize = Convert.ToDouble(sqlReader["BatchSize"]);
                        TotalProd = Convert.ToDouble(sqlReader["TotalProd"]);
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }

                int SNo = 1; string FilterName = ""; double TotalRawAmount = 0; double SubTotalPackagingAmount = 0; double TotalPackagingAmount = 0; double TotalAdditionalAmount = 0; int LastDetailID = 0;

                //-----List of BPR i.e to add in last to master record--------------//
                var BPRs = new List<(string PackagingName,string Unit, double split, double PrimaryCost)>();

                //----------raw detail
                if (string.IsNullOrEmpty(SeekBy) || SeekBy == "Raw")
                {
                    pdftableDetailRaw.AddCell(new Cell(1, 7).Add(new Paragraph().Add("\n")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                    pdftableDetailRaw.AddCell(new Cell(1, 7).Add(new Paragraph().Add(new Link("Raw Detail", PdfAction.CreateURI(uri + "?rn=" + rn + "&id=" + id + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=Raw" + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + GroupID)))).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    ReportName.Value = rn + "2"; LastDetailID = 0;

                    using (DbDataReader sqlReader = command.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            if (FilterName != sqlReader["FilterName"].ToString())
                            {
                                FilterName = sqlReader["FilterName"].ToString();

                                pdftableDetailRaw.AddCell(new Cell(1, 4).Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["FilterName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Dispensed Qty")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Amount")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                SNo = 1;
                            }

                            if (LastDetailID != (int)sqlReader["BMRRDID"])
                            {
                                LastDetailID = (int)sqlReader["BMRRDID"];
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                SNo++;
                            }
                            else
                                pdftableDetailRaw.AddCell(new Cell(1,3).Add(new Paragraph().Add("")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RefNo"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DispensedQty"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Amount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            TotalRawAmount += Convert.ToDouble(sqlReader["Amount"]);

                            
                        }

                        pdftableDetailRaw.AddCell(new Cell(1, 5).Add(new Paragraph().Add("")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Total Amount")).SetBackgroundColor(new DeviceRgb(156,166,181)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(TotalRawAmount.ToString())).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    }
                }

                //----------Additional detail                
                if (string.IsNullOrEmpty(SeekBy) || SeekBy == "Additional")
                {
                    pdftableAdditional.AddCell(new Cell(1, 7).Add(new Paragraph().Add("\n")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                    pdftableAdditional.AddCell(new Cell(1, 7).Add(new Paragraph().Add(new Link("Additional Detail", PdfAction.CreateURI(uri + "?rn=" + rn + "&id=" + id + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=Additional" + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + GroupID)))).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                    pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add("I/R")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add("Dispensed Qty")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add("Amount")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                    ReportName.Value = rn + "4"; LastDetailID = 0;

                    using (DbDataReader sqlReader = command.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            if (FilterName != sqlReader["WareHouseName"].ToString())
                            {
                                FilterName = sqlReader["WareHouseName"].ToString();
                                pdftableAdditional.AddCell(new Cell(1, 7).Add(new Paragraph().Add(FilterName.ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                SNo = 1;
                            }

                            pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["IR"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RefNo"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DispensedQty"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Amount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            TotalAdditionalAmount += Convert.ToDouble(sqlReader["Amount"]);
                            SNo++;

                        }

                        pdftableAdditional.AddCell(new Cell(1, 5).Add(new Paragraph().Add("")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableAdditional.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Total Amount")).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableAdditional.AddCell(new Cell().Add(new Paragraph().Add(TotalAdditionalAmount.ToString())).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    }
                }

                //----------Packaging detail                
                if (string.IsNullOrEmpty(SeekBy) || SeekBy == "Packaging") 
                {
                    ReportName.Value = rn + "3";
                    SNo = 1; FilterName = ""; int BMRDPM = 0; double SplitInto = 1; LastDetailID = 0;

                    pdftableDetailPackaging.AddCell(new Cell(1, 7).Add(new Paragraph().Add("\n")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                    pdftableDetailPackaging.AddCell(new Cell(1, 7).Add(new Paragraph().Add(new Link("Packaging Detail", PdfAction.CreateURI(uri + "?rn=" + rn + "&id=" + id + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=Packaging" + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + GroupID)))).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                    using (DbDataReader sqlReader = command.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            if (BMRDPM != (int)sqlReader["ID"])
                            {
                               
                                if (BMRDPM > 0) 
                                {
                                    pdftableDetailPackaging.AddCell(new Cell(1, 5).Add(new Paragraph().Add("")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                    pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Total Amount")).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                    pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(SubTotalPackagingAmount.ToString())).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                    SubTotalPackagingAmount = 0;
                                    SplitInto = 1;  

                                }                                
                                BMRDPM = (int)sqlReader["ID"];
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Packaging").SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell(1,3).Add(new Paragraph().Add(sqlReader["PackagingName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PrimarySplit_Into"].ToString() + " x " + sqlReader["PrimaryMeasurementUnit"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell(1,2).Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + BatchSizeUnit).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                               
                                double Yield = Math.Round( (Convert.ToDouble(sqlReader["TotalProduction"]) / Convert.ToDouble(sqlReader["BatchSize"]))*100,1);
                                

                                BPRs.Add((
                                    sqlReader["PackagingName"].ToString(), 
                                    sqlReader["PrimaryMeasurementUnit"].ToString(), 
                                    Convert.ToDouble(sqlReader["PrimarySplit_Into"]),0
                                    ));

                                pdftableDetailPackaging.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetFontSize(8).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Yield")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell(1, 1).Add(new Paragraph().Add(Yield.ToString() + "%")).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(131, 219, 72)).SetBold().SetFontSize(8).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell(1, 1).Add(new Paragraph().Add("Production")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["TotalProduction"].ToString() + " " + BatchSizeUnit)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(131, 219, 72)).SetBold().SetFontSize(8).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            }

                            if (FilterName != sqlReader["FilterName"].ToString())
                            {
                                FilterName = sqlReader["FilterName"].ToString();

                                pdftableDetailPackaging.AddCell(new Cell(1, 4).Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["FilterName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Dispensed Qty")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Amount")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                SNo = 1;
                            }

                            if (LastDetailID != (int)sqlReader["BMRDPDIID"])
                            {
                                LastDetailID = (int)sqlReader["BMRDPDIID"];
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                SNo++;
                                SubTotalPackagingAmount += Convert.ToDouble(sqlReader["Amount"]);
                                TotalPackagingAmount += Convert.ToDouble(sqlReader["Amount"]);
                            }
                            else
                            {
                                pdftableDetailPackaging.AddCell(new Cell(1, 3).Add(new Paragraph().Add("")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                SubTotalPackagingAmount += Convert.ToDouble(sqlReader["Amount"]);
                                TotalPackagingAmount += Convert.ToDouble(sqlReader["Amount"]);
                            }
                                

                            
                            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RefNo"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DispensedQty"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Amount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            SplitInto = Convert.ToDouble(sqlReader["PrimarySplit_Into"]);
                            
                        }

                        pdftableDetailPackaging.AddCell(new Cell(1, 5).Add(new Paragraph().Add("")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Total Amount")).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(SubTotalPackagingAmount.ToString())).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
       
                    }
                }

                //----------Adding BPR in master table-------------------//
                //----------Row 2 continue
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Total Cost")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add((TotalRawAmount+TotalPackagingAmount+TotalAdditionalAmount).ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                foreach (var item in BPRs)
                {
                    pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("BPR")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(item.PackagingName)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(item.Unit + " x " + item.split.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("PrimaryCost")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(
                        TotalProd > 0 ?
                        (
                        Math.Round((TotalRawAmount + TotalPackagingAmount + TotalAdditionalAmount) / TotalProd * item.split, 2)
                        ).ToString()
                        : "0"
                        )).SetBackgroundColor(new DeviceRgb(141, 227, 118)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                }

            }

            page.InsertContent(pdftableMaster);
            page.InsertContent(pdftableDetailRaw);
            page.InsertContent(pdftableDetailPackaging);
            page.InsertContent(pdftableAdditional);

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> BMRFinancialAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- " + rn + "-----", true);

            /////////////------------------------------table for Detail 6------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10),//S No
                        (float)(PageSize.A4.GetWidth() * 0.15),//Debit
                        (float)(PageSize.A4.GetWidth() * 0.15),//Credit 
                        (float)(PageSize.A4.GetWidth() * 0.40),//Narration 
                        (float)(PageSize.A4.GetWidth() * 0.15),//Posting Date 
                        (float)(PageSize.A4.GetWidth() * 0.05) //ExeCode
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Debit")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Credit")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("ExeCode")).SetBold());


            int SNo = 1;

            double GrandTotalAmount = 0; string RecordFrom = "";


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Ac_Costing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
                command.CommandType = CommandType.Text;

                var ReportName = command.CreateParameter();
                ReportName.ParameterName = "@ReportName"; ReportName.DbType = DbType.String; ReportName.Value = rn;
                command.Parameters.Add(ReportName);

                var DateFrom = command.CreateParameter();
                DateFrom.ParameterName = "@DateFrom"; DateFrom.DbType = DbType.DateTime; DateFrom.Value = datefrom.HasValue ? datefrom.Value : DateTime.Now;
                command.Parameters.Add(DateFrom);

                var DateTill = command.CreateParameter();
                DateTill.ParameterName = "@DateTill"; DateTill.DbType = DbType.DateTime; DateTill.Value = datetill.HasValue ? datetill.Value : DateTime.Now;
                command.Parameters.Add(DateTill);

                var MasterID = command.CreateParameter();
                MasterID.ParameterName = "@MasterID"; MasterID.DbType = DbType.Int32; MasterID.Value = id;
                command.Parameters.Add(MasterID);

                var seekBy = command.CreateParameter();
                seekBy.ParameterName = "@SeekBy"; seekBy.DbType = DbType.String; seekBy.Value = SeekBy; seekBy.Value = SeekBy ?? "";
                command.Parameters.Add(seekBy);

                var groupBy = command.CreateParameter();
                groupBy.ParameterName = "@GroupBy"; groupBy.DbType = DbType.String; groupBy.Value = GroupBy ?? "";
                command.Parameters.Add(groupBy);

                var orderBy = command.CreateParameter();
                orderBy.ParameterName = "@OrderBy"; orderBy.DbType = DbType.String; orderBy.Value = Orderby ?? "";
                command.Parameters.Add(orderBy);

                var groupID = command.CreateParameter();
                groupID.ParameterName = "@GroupID"; groupID.DbType = DbType.Int32; groupID.Value = GroupID;
                command.Parameters.Add(groupID);

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);


                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (RecordFrom != sqlReader["RecordFrom"].ToString())
                        {
                            RecordFrom = sqlReader["RecordFrom"].ToString();
                            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(RecordFrom)).SetKeepTogether(true));

                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Debit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Credit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy hh:mm tt"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ExeCode"].ToString())).SetKeepTogether(true));



                        GrandTotalAmount += Convert.ToDouble(sqlReader["Debit"]) - Convert.ToDouble(sqlReader["Credit"]);

                        SNo++;
                    }


                }
            }

            pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add(string.Format("{0:n0}", GrandTotalAmount))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }

        #endregion

    }
    public class AccountsDashboardRepository : IAccountsDashboard
    {
        private readonly OreasDbContext db;

        public AccountsDashboardRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        public async Task<object> GetDashBoardData(string userName = "")
        {
            double COA_Assets = 0; double COA_Capital = 0; double COA_Liabilities = 0; double COA_Expense = 0; double COA_Revenue = 0;
            double Sales_L6M = 0; double SalesReturn_L6M = 0; double Purchase_L6M = 0; double PurchaseReturn_L6M = 0;
            int BD_Pending = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[USP_Ac_DashBoard] @UserName ";
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 0;

                var UserName = command.CreateParameter();
                UserName.ParameterName = "@UserName"; UserName.DbType = DbType.String; UserName.Value = userName;
                command.Parameters.Add(UserName);

                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        COA_Assets = (double)sqlReader["COA_Assets"];
                        COA_Capital = (double)sqlReader["COA_Capital"];
                        COA_Liabilities = (double)sqlReader["COA_Liabilities"];
                        COA_Expense = (double)sqlReader["COA_Expense"];
                        COA_Revenue = (double)sqlReader["COA_Revenue"];

                        Sales_L6M = (double)sqlReader["Sales_L6M"];
                        SalesReturn_L6M = (double)sqlReader["SalesReturn_L6M"];
                        Purchase_L6M = (double)sqlReader["Purchase_L6M"];
                        PurchaseReturn_L6M = (double)sqlReader["PurchaseReturn_L6M"];

                        BD_Pending = (int)sqlReader["BD_Pending"];
                    }
                }
            }
            return new
            {
                COA = new { COA_Assets, COA_Capital, COA_Liabilities, COA_Expense, COA_Revenue },
                SP = new { Sales_L6M, SalesReturn_L6M, Purchase_L6M, PurchaseReturn_L6M },
                Other = new { BD_Pending }
            };
        }


    }

}
