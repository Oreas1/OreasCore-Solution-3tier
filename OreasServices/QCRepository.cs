using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Action;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.StyledXmlParser.Jsoup.Nodes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OreasModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OreasServices
{
    public class QcListRepository : IQcList
    {
        private readonly OreasDbContext db;
        public QcListRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetActionTypeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Qc_ActionTypes
                              select new
                              {
                                  a.ID,
                                  a.ActionName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Qc_ActionTypes
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.ActionName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.ActionName
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetQcLabListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Qc_Labs
                              select new
                              {
                                  a.ID,
                                  a.LabName,
                                  a.Prefix
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Qc_Labs
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.LabName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.LabName,
                                  a.Prefix
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetQcTestListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Qc_Tests
                              select new
                              {
                                  a.ID,
                                  a.TestName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Qc_Tests
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.TestName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.TestName
                              }).Take(5).ToListAsync();
        }
    }
    public class ProductRegistrationQcTestForPNRepository : IProductRegistrationQcTestForPN
    {
        private readonly OreasDbContext db;

        public ProductRegistrationQcTestForPNRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region ProductRegistration
        public object GetWCLProductRegistration()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }, new { n = "by Product Code", v = "byProductCode" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductRegistration(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_ProductRegistrationDetails
                                      .Where(w => w.tbl_Inv_ProductType_Category.tbl_Inv_WareHouseDetails
                                                   .Any(x =>
                                                        x.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                        .Any(y => y.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                        .Count(c => c.UserName == userName) > 0))
                                            )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductCode" && w.ProductCode.ToLower() == FilterValueByText.ToLower()
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_ProductRegistrationDetails
                                        .Where(w => w.tbl_Inv_ProductType_Category.tbl_Inv_WareHouseDetails
                                                   .Any(x =>
                                                        x.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                        .Any(y => y.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                        .Count(c => c.UserName == userName) > 0))
                                            )
                                      .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byProductCode" && w.ProductCode.ToLower() == FilterValueByText.ToLower()
                                          )
                                      .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType,
                          o.tbl_Inv_ProductType_Category.CategoryName,
                          o.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.ProductCode,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalTests = o.tbl_Inv_ProductRegistrationDetail_PNQcTests.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region ProductRegistrationDetail QcTest For Purchase Note
        public async Task<object> GetProductRegistrationPNQcTest(int id)
        {
            var qry = from o in await db.tbl_Inv_ProductRegistrationDetail_PNQcTests.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLProductRegistrationPNQcTest()
        {
            return new[]
            {
                new { n = "by Test Name", v = "byTestName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductRegistrationPNQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_ProductRegistrationDetail_PNQcTests
                                               .Where(w => w.FK_tbl_Inv_ProductRegistrationDetail_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_ProductRegistrationDetail_PNQcTests
                                  .Where(w => w.FK_tbl_Inv_ProductRegistrationDetail_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostProductRegistrationPNQcTest(tbl_Inv_ProductRegistrationDetail_PNQcTest tbl_Inv_ProductRegistrationDetail_PNQcTest, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_ProductRegistrationDetail_PNQcTest.CreatedBy = userName;
                tbl_Inv_ProductRegistrationDetail_PNQcTest.CreatedDate = DateTime.Now;
                db.tbl_Inv_ProductRegistrationDetail_PNQcTests.Add(tbl_Inv_ProductRegistrationDetail_PNQcTest);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_ProductRegistrationDetail_PNQcTest.ModifiedBy = userName;
                tbl_Inv_ProductRegistrationDetail_PNQcTest.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_ProductRegistrationDetail_PNQcTest).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_ProductRegistrationDetail_PNQcTests.Remove(db.tbl_Inv_ProductRegistrationDetail_PNQcTests.Find(tbl_Inv_ProductRegistrationDetail_PNQcTest.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion      

    }
    public class QcPurchaseNoteRepository : IQcPurchaseNote
    {
        private readonly OreasDbContext db;
        public QcPurchaseNoteRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region PurchaseNote
        public async Task<object> GetPurchaseNote(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseNoteDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseNoteMaster_ID,
                          o.tbl_Inv_PurchaseNoteMaster.DocNo,
                          DocDate = o.tbl_Inv_PurchaseNoteMaster.DocDate.ToString("dd-MMM-yy"),
                          o.tbl_Inv_PurchaseNoteMaster.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.tbl_Inv_PurchaseNoteMaster.tbl_Ac_ChartOfAccounts.AccountName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.MfgBatchNo,
                          MfgDate = o.MfgDate.HasValue ? o.MfgDate.Value.ToString("dd-MMM-yyyy") : null,
                          ExpiryDate = o.ExpiryDate.HasValue ? o.ExpiryDate.Value.ToString("dd-MMM-yyyy") : null,
                          o.ReferenceNo,
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.QuantitySample,
                          o.RetestDate,
                          o.NoOfContainers,
                          o.PotencyPercentage,
                          o.CreatedByQcQa,
                          CreatedDateQcQa = o.CreatedDateQcQa.HasValue ? o.CreatedDateQcQa.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedByQcQa,
                          ModifiedDateQcQa = o.ModifiedDateQcQa.HasValue ? o.ModifiedDateQcQa.Value.ToString("dd-MMM-yyyy") : "",
                          o.IsProcessed,
                          o.IsSupervised,
                          o.Remarks,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.IsDecimal
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLQcPurchaseNote()
        {
            return new[]
            {
                new { n = "by PND No", v = "byPNDNo" }, new { n = "by Reference No", v = "byReferenceNo" }, new { n = "by ProductName", v = "byProductName" }, new { n = "by DocNo", v = "byDocNo" }
            }.ToList();
        }
        public object GetWCLBQcPurchaseNote()
        {
            return new[]
            {
                new { n = "by Pending", v = "byPending" }, new { n = "by Sample Pending", v = "bySamplePending" },
                new { n = "by Rejected", v = "byRejected" }, new { n = "by Approved", v = "byApproved" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseNote(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseNoteDetails
                                        .Where(w => w.tbl_Inv_PurchaseNoteMaster.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                        .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                        .Count(w => w.UserName == userName) > 0)
                                        )
                                        .Where(w =>
                                                    string.IsNullOrEmpty(FilterByLoad)
                                                    ||
                                                    FilterByLoad == "byPending" && w.FK_tbl_Qc_ActionType_ID == 1
                                                    ||
                                                    FilterByLoad == "byApproved" && w.FK_tbl_Qc_ActionType_ID == 2
                                                    ||
                                                    FilterByLoad == "byRejected" && w.FK_tbl_Qc_ActionType_ID == 3
                                                    ||
                                                    FilterByLoad == "bySamplePending" && w.FK_tbl_Qc_ActionType_ID == 1 && w.QuantitySample == 0
                                                )
                                        .Where(w =>
                                                string.IsNullOrEmpty(FilterValueByText)
                                                ||
                                                FilterByText == "byReferenceNo" && w.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower())
                                                ||
                                                FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                ||
                                                FilterByText == "byDocNo" && w.tbl_Inv_PurchaseNoteMaster.DocNo.ToString() == FilterValueByText
                                                ||
                                                FilterByText == "byPNDNo" && w.ID.ToString() == FilterValueByText
                                                )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseNoteDetails
                                    .Where(w => w.tbl_Inv_PurchaseNoteMaster.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                        .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                        .Count(w => w.UserName == userName) > 0)
                                        )
                                    .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byPending" && w.FK_tbl_Qc_ActionType_ID == 1
                                                ||
                                                FilterByLoad == "byApproved" && w.FK_tbl_Qc_ActionType_ID == 2
                                                ||
                                                FilterByLoad == "byRejected" && w.FK_tbl_Qc_ActionType_ID == 3
                                                ||
                                                FilterByLoad == "bySamplePending" && w.FK_tbl_Qc_ActionType_ID == 1 && w.QuantitySample == 0
                                            )
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byReferenceNo" && w.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byDocNo" && w.tbl_Inv_PurchaseNoteMaster.DocNo.ToString() == FilterValueByText
                                            ||
                                            FilterByText == "byPNDNo" && w.ID.ToString() == FilterValueByText
                                            )
                                    .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseNoteMaster_ID,
                          o.tbl_Inv_PurchaseNoteMaster.DocNo,
                          DocDate = o.tbl_Inv_PurchaseNoteMaster.DocDate.ToString("dd-MMM-yy"),
                          o.tbl_Inv_PurchaseNoteMaster.tbl_Ac_ChartOfAccounts.AccountName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.ReferenceNo,
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.QuantitySample,
                          RetestDate = o.RetestDate.HasValue ? o.RetestDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedByQcQa,
                          CreatedDateQcQa = o.CreatedDateQcQa.HasValue ? o.CreatedDateQcQa.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedByQcQa,
                          ModifiedDateQcQa = o.ModifiedDateQcQa.HasValue ? o.ModifiedDateQcQa.Value.ToString("dd-MMM-yyyy") : "",
                          o.IsProcessed,
                          o.IsSupervised,
                          o.Remarks,
                          TotalTests = o.tbl_Qc_PurchaseNoteDetail_QcTests.Count()
                      };


            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseNote(tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "", string userName = "")
        {

            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PurchaseNoteDetail.CreatedByQcQa = userName;
                tbl_Inv_PurchaseNoteDetail.CreatedDateQcQa = DateTime.Now;
                CRUD_Type.Value = "UpdateQc";
            }
            else if (operation == "Save Update")
            {
                if (tbl_Inv_PurchaseNoteDetail.CreatedByQcQa == null)
                    tbl_Inv_PurchaseNoteDetail.CreatedByQcQa = userName;

                if (tbl_Inv_PurchaseNoteDetail.CreatedDateQcQa == null)
                    tbl_Inv_PurchaseNoteDetail.CreatedDateQcQa = DateTime.Now;

                tbl_Inv_PurchaseNoteDetail.ModifiedByQcQa = userName;
                tbl_Inv_PurchaseNoteDetail.ModifiedDateQcQa = DateTime.Now;
                CRUD_Type.Value = "UpdateQc";
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
                tbl_Inv_PurchaseNoteDetail.FK_tbl_Qc_ActionType_ID, tbl_Inv_PurchaseNoteDetail.QuantitySample, tbl_Inv_PurchaseNoteDetail.RetestDate,
                tbl_Inv_PurchaseNoteDetail.CreatedByQcQa, tbl_Inv_PurchaseNoteDetail.CreatedDateQcQa, tbl_Inv_PurchaseNoteDetail.ModifiedByQcQa, tbl_Inv_PurchaseNoteDetail.ModifiedDateQcQa
                );

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region PurchaseNote QcTest
        public async Task<object> GetPurchaseNoteQcTest(int id)
        {
            var qry = from o in await db.tbl_Qc_PurchaseNoteDetail_QcTests.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.ResultValue,
                          o.ResultRemarks,
                          o.IsPrintOnCOA,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseNoteQcTest()
        {
            return new[]
            {
                new { n = "by Test Name", v = "byTestName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseNoteQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Qc_PurchaseNoteDetail_QcTests
                                      .Where(w => w.FK_tbl_Inv_PurchaseNoteDetail_ID == MasterID)
                                      .Where(w =>
                                                    string.IsNullOrEmpty(FilterValueByText)
                                                    ||
                                                    FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                    )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_PurchaseNoteDetail_QcTests
                                        .Where(w => w.FK_tbl_Inv_PurchaseNoteDetail_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.ResultValue,
                          o.ResultRemarks,
                          o.IsPrintOnCOA,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseNoteQcTest(tbl_Qc_PurchaseNoteDetail_QcTest tbl_Qc_PurchaseNoteDetail_QcTest, string operation = "", string userName = "")
        {
            if (operation == "Save New" && tbl_Qc_PurchaseNoteDetail_QcTest.ID == 0)
            {
                tbl_Qc_PurchaseNoteDetail_QcTest.CreatedBy = userName;
                tbl_Qc_PurchaseNoteDetail_QcTest.CreatedDate = DateTime.Now;
                db.tbl_Qc_PurchaseNoteDetail_QcTests.Add(tbl_Qc_PurchaseNoteDetail_QcTest);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update" || (operation == "Save New" && tbl_Qc_PurchaseNoteDetail_QcTest.ID > 0))
            {
                tbl_Qc_PurchaseNoteDetail_QcTest.ModifiedBy = userName;
                tbl_Qc_PurchaseNoteDetail_QcTest.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Qc_PurchaseNoteDetail_QcTest).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Qc_PurchaseNoteDetail_QcTests.Remove(db.tbl_Qc_PurchaseNoteDetail_QcTests.Find(tbl_Qc_PurchaseNoteDetail_QcTest.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        public async Task<string> PostPurchaseNoteQcTestReplicationFromStandard(int MasterID, string userName = "")
        {
            var PurchaseNoteDetail = await db.tbl_Inv_PurchaseNoteDetails.Where(w=> w.ID == MasterID).FirstOrDefaultAsync();

            if(db.tbl_Qc_PurchaseNoteDetail_QcTests.Count(c=> c.FK_tbl_Inv_PurchaseNoteDetail_ID == MasterID) > 0)
                return "Replication Aborted! because the test list has already been entered";

            if (PurchaseNoteDetail != null)
            {
                var StandardQcTestList = await db.tbl_Inv_ProductRegistrationDetail_PNQcTests
                                           .Where(w => w.FK_tbl_Inv_ProductRegistrationDetail_ID == PurchaseNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID)
                                           .ToListAsync();

                DateTime currentDateTime = DateTime.Now;

                var qcTestList = StandardQcTestList.Select(s => new tbl_Qc_PurchaseNoteDetail_QcTest
                {
                    ID = 0,
                    FK_tbl_Inv_PurchaseNoteDetail_ID = MasterID,
                    FK_tbl_Qc_Test_ID = s.FK_tbl_Qc_Test_ID,
                    TestDescription = s.TestDescription,
                    Specification = s.Specification,
                    RangeFrom = s.RangeFrom,
                    RangeTill = s.RangeTill,
                    FK_tbl_Inv_MeasurementUnit_ID = s.FK_tbl_Inv_MeasurementUnit_ID,
                    ResultValue = 0,
                    ResultRemarks = null,
                    IsPrintOnCOA = true,
                    CreatedBy = userName,
                    CreatedDate = currentDateTime,
                    ModifiedBy = null,
                    ModifiedDate = null                    
                }).ToList();

                if(qcTestList.Count == 0)
                    return "No Test Found in Product Registration for Replication";

                await db.tbl_Qc_PurchaseNoteDetail_QcTests.AddRangeAsync(qcTestList);
                await db.SaveChangesAsync();

                return "OK";
            }
            else
            {
                return "Purchae Note Record Not Found";
            }            
            
        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLQcQaPurchaseNote()
        {
            return new List<ReportCallingModel>()
            {
                //new ReportCallingModel()
                //{
                //    ReportType= EnumReportType.Periodic,
                //    ReportName ="Register QcQa Purchase Note",
                //    GroupBy = new List<string>(){ "WareHouse", "Product" },
                //    OrderBy = new List<string>(){ "Doc Date", "Doc No" },
                //    SeekBy = null,
                //}
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Purchase Note Label")
            {
                return await Task.Run(() => PurchaseNoteLabel(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }

            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> PurchaseNoteLabel(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A5, 25f, 25f, 50f, 50f, null, false, false, false);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                Table pdftable = new Table(new float[] {
                        (float)(PageSize.A5.GetWidth() * 0.48),(float)(PageSize.A5.GetWidth() * 0.04),(float)(PageSize.A5.GetWidth() * 0.48)
                }
               ).UseAllAvailableWidth().SetFixedLayout().SetBorder(Border.NO_BORDER);

                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        25f, //
                        50f, //
                        25f, //
                        150f //
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);



                command.CommandText = "EXECUTE [dbo].[Report_Qc_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell(1, 4).Add(new Paragraph().Add(sqlReader["ActionName"].ToString())).SetBold().SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Product Name \n\n")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Date")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("By")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("\n\n" + "____________________\n" + sqlReader["CreatedBy"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftable.AddCell(new Cell().Add(pdftableMaster).SetBorder(Border.NO_BORDER));
                        pdftable.AddCell(new Cell().Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER));
                        pdftable.AddCell(new Cell().Add(pdftableMaster).SetBorder(Border.NO_BORDER));

                        pdftable.AddCell(new Cell(1, 3).Add(new Paragraph().Add("\n\n")).SetBorder(Border.NO_BORDER));

                        pdftable.AddCell(new Cell().Add(pdftableMaster).SetBorder(Border.NO_BORDER));
                        pdftable.AddCell(new Cell().Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER));
                        pdftable.AddCell(new Cell().Add(pdftableMaster).SetBorder(Border.NO_BORDER));
                    }
                }

                page.InsertContent(new Cell().Add(pdftable).SetBorder(Border.NO_BORDER));

            }

            return page.FinishToGetBytes();
        }


        #endregion
    }
    public class CompositionQcTestRepository : ICompositionQcTest
    {
        private readonly OreasDbContext db;
        public CompositionQcTestRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region CompositionMaster
        public async Task<object> GetCompositionMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy"),
                          o.CompositionName,
                          o.ShelfLifeInMonths,
                          o.DimensionValue,
                          o.FK_tbl_Inv_MeasurementUnit_ID_Dimension,
                          FK_tbl_Inv_MeasurementUnit_ID_DimensionName = o.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.RevisionNo,
                          RevisionDate = o.RevisionDate.HasValue ? o.RevisionDate.Value.ToString("dd-MMM-yyyy") : null,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCompositionMaster()
        {
            return new[]
            {
                new { n = "by Composition Name", v = "byCompositionName" }, new { n = "by Raw Product Name", v = "byRawProductName" }, new { n = "by Semi Finished Product Name", v = "bySemiFinishedProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionMasters
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byCompositionName" && w.CompositionName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byRawProductName" && w.tbl_Pro_CompositionDetail_RawMasters.Any(a => a.tbl_Pro_CompositionDetail_RawDetail_Itemss.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
                                                       ||
                                                       FilterByText == "bySemiFinishedProductName" && w.tbl_Pro_CompositionDetail_Couplings.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionMasters
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byCompositionName" && w.CompositionName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byRawProductName" && w.tbl_Pro_CompositionDetail_RawMasters.Any(a => a.tbl_Pro_CompositionDetail_RawDetail_Itemss.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
                                                       ||
                                                       FilterByText == "bySemiFinishedProductName" && w.tbl_Pro_CompositionDetail_Couplings.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy"),
                          o.CompositionName,
                          o.ShelfLifeInMonths,
                          o.DimensionValue,
                          o.FK_tbl_Inv_MeasurementUnit_ID_Dimension,
                          FK_tbl_Inv_MeasurementUnit_ID_DimensionName = o.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.RevisionNo,
                          RevisionDate = o.RevisionDate.HasValue ? o.RevisionDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          NoOfCouplings = o.tbl_Pro_CompositionDetail_Couplings.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region BMRProcess
        public async Task<object> GetBMRProcess(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionMaster_ProcessBMRs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionMaster_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_ProductRegistrationMaster.ProductName ?? "",
                          MeasurementUnit = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.IsQAClearanceBeforeStart,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRProcess()
        {
            return new[]
            {
                new { n = "by Procedure Name", v = "byProcedureName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionMaster_ProcessBMRs
                                      .Where(w => w.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionMaster_ProcessBMRs
                                        .Where(w => w.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionMaster_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_ProductRegistrationMaster.ProductName ?? "",
                          MeasurementUnit = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.IsQAClearanceBeforeStart,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalTests = o.tbl_Pro_CompositionMaster_ProcessBMR_QcTests.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRProcess(tbl_Pro_CompositionMaster_ProcessBMR tbl_Pro_CompositionMaster_ProcessBMR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_CompositionMaster_ProcessBMR.CreatedBy = userName;
                tbl_Pro_CompositionMaster_ProcessBMR.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionMaster_ProcessBMR.ModifiedBy = userName;
                tbl_Pro_CompositionMaster_ProcessBMR.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }
            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_CompositionMaster_ProcessBMR] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Pro_CompositionMaster_ID={4},@FK_tbl_Pro_Procedure_ID={5}
              ,@FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample={6},@IsQAClearanceBeforeStart={7}
              ,@CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
              CRUD_Type, CRUD_Msg, CRUD_ID,
              tbl_Pro_CompositionMaster_ProcessBMR.ID, tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Pro_CompositionMaster_ID, tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Pro_Procedure_ID,
              tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample, tbl_Pro_CompositionMaster_ProcessBMR.IsQAClearanceBeforeStart,
              tbl_Pro_CompositionMaster_ProcessBMR.CreatedBy, tbl_Pro_CompositionMaster_ProcessBMR.CreatedDate, tbl_Pro_CompositionMaster_ProcessBMR.ModifiedBy, tbl_Pro_CompositionMaster_ProcessBMR.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region BMRProcess QcTest
        public async Task<object> GetBMRProcessQcTest(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionMaster_ProcessBMR_QcTests.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionMaster_ProcessBMR_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRProcessQcTest()
        {
            return new[]
            {
                new { n = "by Test Name", v = "byTestName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRProcessQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionMaster_ProcessBMR_QcTests
                                      .Where(w => w.FK_tbl_Pro_CompositionMaster_ProcessBMR_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionMaster_ProcessBMR_QcTests
                                        .Where(w => w.FK_tbl_Pro_CompositionMaster_ProcessBMR_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionMaster_ProcessBMR_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRProcessQcTest(tbl_Pro_CompositionMaster_ProcessBMR_QcTest tbl_Pro_CompositionMaster_ProcessBMR_QcTest, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionMaster_ProcessBMR_QcTest.CreatedBy = userName;
                tbl_Pro_CompositionMaster_ProcessBMR_QcTest.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionMaster_ProcessBMR_QcTests.Add(tbl_Pro_CompositionMaster_ProcessBMR_QcTest);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionMaster_ProcessBMR_QcTest.ModifiedBy = userName;
                tbl_Pro_CompositionMaster_ProcessBMR_QcTest.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionMaster_ProcessBMR_QcTest).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionMaster_ProcessBMR_QcTests.Remove(db.tbl_Pro_CompositionMaster_ProcessBMR_QcTests.Find(tbl_Pro_CompositionMaster_ProcessBMR_QcTest.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

        #region CompositionPackagingMaster
        public async Task<object> GetCompositionPackagingMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "'s " + o.tbl_Inv_ProductRegistrationDetail_Primary.Description,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary.HasValue ? " [" + o.tbl_Inv_ProductRegistrationDetail_Secondary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Secondary.Split_Into.ToString() + " " + o.tbl_Inv_ProductRegistrationDetail_Secondary.Description : "",
                          o.PackagingName,
                          o.IsDiscontinue,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          NoOfItems = o.tbl_Pro_CompositionDetail_Coupling_PackagingDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCompositionPackagingMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionPackagingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters
                                      .Where(w => w.tbl_Pro_CompositionDetail_Coupling.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters
                                        .Where(w => w.tbl_Pro_CompositionDetail_Coupling.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "'s " + o.tbl_Inv_ProductRegistrationDetail_Primary.Description,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary.HasValue ? " [" + o.tbl_Inv_ProductRegistrationDetail_Secondary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Secondary.Split_Into.ToString() + " " + o.tbl_Inv_ProductRegistrationDetail_Secondary.Description : "",
                          o.PackagingName,
                          o.IsDiscontinue,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          NoOfItems = o.tbl_Pro_CompositionDetail_Coupling_PackagingDetails.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        #endregion

        #region BPRProcess
        public async Task<object> GetBPRProcess(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_ProductRegistrationMaster.ProductName ?? "",
                          MeasurementUnit = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.IsQAClearanceBeforeStart,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBPRProcess()
        {
            return new[]
            {
                new { n = "by Procedure Name", v = "byProcedureName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBPRProcess(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs
                                      .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs
                                        .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_ProductRegistrationMaster.ProductName ?? "",
                          MeasurementUnit = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.IsQAClearanceBeforeStart,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalTests = o.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBPRProcess(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.CreatedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }
            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID={4},@FK_tbl_Pro_Procedure_ID={5}
              ,@FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample={6},@IsQAClearanceBeforeStart={7}
              ,@CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
              CRUD_Type, CRUD_Msg, CRUD_ID,
              tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.ID, tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID, tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Pro_Procedure_ID,
              tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample, tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.IsQAClearanceBeforeStart,
              tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.CreatedBy, tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.CreatedDate, tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.ModifiedBy, tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion      

        #region BPRProcess QcTest
        public async Task<object> GetBPRProcessQcTest(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBPRProcessQcTest()
        {
            return new[]
            {
                new { n = "by Test Name", v = "byTestName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBPRProcessQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests
                                      .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests
                                        .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBPRProcessQcTest(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest.CreatedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests.Add(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests.Remove(db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests.Find(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion
    }
    public class QCBatchRepository : IQCBatch
    {
        private readonly OreasDbContext db;
        public QCBatchRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public object GetWCLBatchRecordMaster()
        {
            return new[]
            {
                new { n = "by Batch No", v = "byBatchNo" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBBatchRecordMaster()
        {
            return new[]
            {
                new { n = "by QC Sample BMR Pending", v = "byQCSampleBMRPending" }, new { n = "by QC Sample BPR Pending", v = "byQCSampleBPRPending" }, new { n = "by Finished", v = "byFinished" }, new { n = "by Partial Finished", v = "byPartialFinished" }, new { n = "by InProcess", v = "byInProcess" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBatchRecordMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionMasters
                                               .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byQCSampleBMRPending" && w.IsQCSampleBMRPending == true
                                                 ||
                                                 FilterByLoad == "byQCSampleBPRPending" && w.IsQCSampleBPRPending == true
                                                 ||
                                                 FilterByLoad == "byFinished" && w.IsCompleted == true
                                                 ||
                                                 FilterByLoad == "byPartialFinished" && w.IsCompleted == null
                                                 ||
                                                 FilterByLoad == "byInProcess" && w.IsCompleted == false
                                                 )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && w.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMasters
                                  .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byQCSampleBMRPending" && w.IsQCSampleBMRPending == true
                                                 ||
                                                 FilterByLoad == "byQCSampleBPRPending" && w.IsQCSampleBPRPending == true
                                                 ||
                                                 FilterByLoad == "byFinished" && w.IsCompleted == true
                                                 ||
                                                 FilterByLoad == "byPartialFinished" && w.IsCompleted == null
                                                 ||
                                                 FilterByLoad == "byInProcess" && w.IsCompleted == false
                                                 )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byBatchNo" && w.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy"),
                          o.BatchNo,
                          BatchMfgDate = o.BatchMfgDate.ToString("dd-MMM-yyyy"),
                          BatchExpiryDate = o.BatchExpiryDate.ToString("dd-MMM-yyyy"),
                          o.DimensionValue,
                          o.FK_tbl_Inv_MeasurementUnit_ID_Dimension,
                          FK_tbl_Inv_MeasurementUnit_ID_DimensionName = o.tbl_Inv_MeasurementUnit.MeasurementUnit,
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
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalPackageBatchSize = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters?.Sum(s => s.BatchSize) ?? 0
                      };




            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region BMRSample
        public async Task<object> GetBMRSample(int id)
        {
            var qry = from o in await db.tbl_Qc_SampleProcessBMRs.Where(w => w.ID == id).ToListAsync()
                      join p in db.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs on o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID equals p.ID
                      join d in db.tbl_Pro_Procedures on p.FK_tbl_Pro_Procedure_ID equals d.ID
                      join pd in db.tbl_Inv_ProductRegistrationDetails on p.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample equals pd.ID
                      join pm in db.tbl_Inv_ProductRegistrationMasters on pd.FK_tbl_Inv_ProductRegistrationMaster_ID equals pm.ID
                      join u in db.tbl_Inv_MeasurementUnits on pd.FK_tbl_Inv_MeasurementUnit_ID equals u.ID
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID,
                          d.ProcedureName,
                          pm.ProductName,
                          u.MeasurementUnit,
                          SampleDate = o.SampleDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.SampleQty,
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.ActionBy,
                          ActionDate = o.ActionDate.HasValue ? o.ActionDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRSample()
        {
            return new[]
            {
                new { n = "by Action", v = "byAction" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRSample(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await (from o in db.tbl_Qc_SampleProcessBMRs
                                     join p in db.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs on o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID equals p.ID
                                     where p.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID && p.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample != null
                                     select o).CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_SampleProcessBMRs.OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      join p in db.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs on o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID equals p.ID
                      join d in db.tbl_Pro_Procedures on p.FK_tbl_Pro_Procedure_ID equals d.ID
                      join pd in db.tbl_Inv_ProductRegistrationDetails on p.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample equals pd.ID
                      join pm in db.tbl_Inv_ProductRegistrationMasters on pd.FK_tbl_Inv_ProductRegistrationMaster_ID equals pm.ID
                      join u in db.tbl_Inv_MeasurementUnits on pd.FK_tbl_Inv_MeasurementUnit_ID equals u.ID
                      where p.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID && p.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample != null
                      orderby o.ID descending
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID,
                          d.ProcedureName,
                          pm.ProductName,
                          u.MeasurementUnit,
                          SampleDate = o.SampleDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.SampleQty,
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.ActionBy,
                          ActionDate = o.ActionDate.HasValue ? o.ActionDate.Value.ToString("dd-MMM-yyyy") : "",
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          p.IsCompleted,
                          TotalTests = o.tbl_Qc_SampleProcessBMR_QcTests.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRSample(tbl_Qc_SampleProcessBMR tbl_Qc_SampleProcessBMR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            CRUD_Type.Value = "UpdateByQC";
            tbl_Qc_SampleProcessBMR.ActionBy = userName;
            tbl_Qc_SampleProcessBMR.ActionDate = DateTime.Now;

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Qc_SampleProcessBMR] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3} ,@FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID={4},@SampleDate={5}
              ,@SampleQty={6},@FK_tbl_Qc_ActionType_ID={7},@ActionBy={8},@ActionDate={9}
              ,@CreatedBy={10},@CreatedDate={11},@ModifiedBy={12},@ModifiedDate={13}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Qc_SampleProcessBMR.ID, tbl_Qc_SampleProcessBMR.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID, tbl_Qc_SampleProcessBMR.SampleDate,
               tbl_Qc_SampleProcessBMR.SampleQty, tbl_Qc_SampleProcessBMR.FK_tbl_Qc_ActionType_ID, tbl_Qc_SampleProcessBMR.ActionBy, tbl_Qc_SampleProcessBMR.ActionDate,
               tbl_Qc_SampleProcessBMR.CreatedBy, tbl_Qc_SampleProcessBMR.CreatedDate,
               tbl_Qc_SampleProcessBMR.ModifiedBy, tbl_Qc_SampleProcessBMR.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region BMRSample
        public async Task<object> GetBPRSample(int id)
        {
            var qry = from o in await db.tbl_Qc_SampleProcessBPRs.Where(w => w.ID == id).ToListAsync()
                      join p in db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs on o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID equals p.ID
                      join m in db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters on p.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID equals m.ID
                      join d in db.tbl_Pro_Procedures on p.FK_tbl_Pro_Procedure_ID equals d.ID
                      join pd in db.tbl_Inv_ProductRegistrationDetails on p.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample equals pd.ID
                      join pm in db.tbl_Inv_ProductRegistrationMasters on pd.FK_tbl_Inv_ProductRegistrationMaster_ID equals pm.ID
                      join u in db.tbl_Inv_MeasurementUnits on pd.FK_tbl_Inv_MeasurementUnit_ID equals u.ID
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID,
                          d.ProcedureName,
                          pm.ProductName,
                          u.MeasurementUnit,
                          SampleDate = o.SampleDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.SampleQty,
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.ActionBy,
                          ActionDate = o.ActionDate.HasValue ? o.ActionDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBPRSample()
        {
            return new[]
            {
                new { n = "by Action", v = "byAction" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBPRSample(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {

            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await (from o in db.tbl_Qc_SampleProcessBPRs
                                     join p in db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs on o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID equals p.ID
                                     join m in db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters on p.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID equals m.ID
                                     where m.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID && p.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample != null
                                     select o).CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_SampleProcessBPRs.OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      join p in db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs on o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID equals p.ID
                      join m in db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters on p.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID equals m.ID
                      join d in db.tbl_Pro_Procedures on p.FK_tbl_Pro_Procedure_ID equals d.ID
                      join pd in db.tbl_Inv_ProductRegistrationDetails on p.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample equals pd.ID
                      join pm in db.tbl_Inv_ProductRegistrationMasters on pd.FK_tbl_Inv_ProductRegistrationMaster_ID equals pm.ID
                      join u in db.tbl_Inv_MeasurementUnits on pd.FK_tbl_Inv_MeasurementUnit_ID equals u.ID
                      where m.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID && p.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample != null
                      orderby o.ID descending
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID,
                          d.ProcedureName,
                          pm.ProductName,
                          u.MeasurementUnit,
                          SampleDate = o.SampleDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.SampleQty,
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.ActionBy,
                          ActionDate = o.ActionDate.HasValue ? o.ActionDate.Value.ToString("dd-MMM-yyyy") : "",
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          p.IsCompleted,
                          TotalTests = o.tbl_Qc_SampleProcessBPR_QcTests.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBPRSample(tbl_Qc_SampleProcessBPR tbl_Qc_SampleProcessBPR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            CRUD_Type.Value = "UpdateByQC";
            tbl_Qc_SampleProcessBPR.ActionBy = userName;
            tbl_Qc_SampleProcessBPR.ActionDate = DateTime.Now;

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Qc_SampleProcessBPR] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3} ,@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID={4},@SampleDate={5}
              ,@SampleQty={6},@FK_tbl_Qc_ActionType_ID={7},@ActionBy={8},@ActionDate={9}
              ,@CreatedBy={10},@CreatedDate={11},@ModifiedBy={12},@ModifiedDate={13}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Qc_SampleProcessBPR.ID, tbl_Qc_SampleProcessBPR.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID, tbl_Qc_SampleProcessBPR.SampleDate,
               tbl_Qc_SampleProcessBPR.SampleQty, tbl_Qc_SampleProcessBPR.FK_tbl_Qc_ActionType_ID, tbl_Qc_SampleProcessBPR.ActionBy, tbl_Qc_SampleProcessBPR.ActionDate,
               tbl_Qc_SampleProcessBPR.CreatedBy, tbl_Qc_SampleProcessBPR.CreatedDate,
               tbl_Qc_SampleProcessBPR.ModifiedBy, tbl_Qc_SampleProcessBPR.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region BMRSample QcTest
        public async Task<object> GetBMRSampleQcTest(int id)
        {
            var qry = from o in await db.tbl_Qc_SampleProcessBMR_QcTests.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Qc_SampleProcessBMR_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.ResultValue,
                          o.ResultRemarks,
                          o.IsPrintOnCOA,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRSampleQcTest()
        {
            return new[]
            {
                new { n = "by Test Name", v = "byTestName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRSampleQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Qc_SampleProcessBMR_QcTests
                                      .Where(w => w.FK_tbl_Qc_SampleProcessBMR_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_SampleProcessBMR_QcTests
                                        .Where(w => w.FK_tbl_Qc_SampleProcessBMR_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Qc_SampleProcessBMR_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.ResultValue,
                          o.ResultRemarks,
                          o.IsPrintOnCOA,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRSampleQcTest(tbl_Qc_SampleProcessBMR_QcTest tbl_Qc_SampleProcessBMR_QcTest, string operation = "", string userName = "")
        {
            if (operation == "Save New" && tbl_Qc_SampleProcessBMR_QcTest.ID == 0)
            {
                tbl_Qc_SampleProcessBMR_QcTest.CreatedBy = userName;
                tbl_Qc_SampleProcessBMR_QcTest.CreatedDate = DateTime.Now;
                db.tbl_Qc_SampleProcessBMR_QcTests.Add(tbl_Qc_SampleProcessBMR_QcTest);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update" || (operation == "Save New" && tbl_Qc_SampleProcessBMR_QcTest.ID > 0))
            {
                tbl_Qc_SampleProcessBMR_QcTest.ModifiedBy = userName;
                tbl_Qc_SampleProcessBMR_QcTest.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Qc_SampleProcessBMR_QcTest).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Qc_SampleProcessBMR_QcTests.Remove(db.tbl_Qc_SampleProcessBMR_QcTests.Find(tbl_Qc_SampleProcessBMR_QcTest.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        public async Task<string> PostBMRSampleQcTestReplicationFromStandard(int MasterID, string userName = "")
        {
            var SampleProcessBMR = await db.tbl_Qc_SampleProcessBMRs.Where(w => w.ID == MasterID).FirstOrDefaultAsync();
            
            if (db.tbl_Qc_SampleProcessBMR_QcTests.Count(c => c.FK_tbl_Qc_SampleProcessBMR_ID == MasterID) > 0)
                return "Replication Aborted! because the test list has already been entered";

            if (SampleProcessBMR != null)
            {
                var CompositionID = SampleProcessBMR.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.tbl_Pro_BatchMaterialRequisitionMaster?.tbl_Pro_CompositionDetail_Coupling?.FK_tbl_Pro_CompositionMaster_ID ?? 0;
                if (CompositionID == 0)
                    return "No Link Found From Batch to Composition";

                var ProcedureID = SampleProcessBMR.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.FK_tbl_Pro_Procedure_ID;

                var StandardQcTestList = await db.tbl_Pro_CompositionMaster_ProcessBMR_QcTests
                    .Where(w => w.tbl_Pro_CompositionMaster_ProcessBMR.tbl_Pro_CompositionMaster.ID == CompositionID
                                 &&
                                 w.tbl_Pro_CompositionMaster_ProcessBMR.FK_tbl_Pro_Procedure_ID == ProcedureID
                          )
                    .ToListAsync();

                DateTime currentDateTime = DateTime.Now;

                var qcTestList = StandardQcTestList.Select(s => new tbl_Qc_SampleProcessBMR_QcTest
                {
                    ID = 0,
                    FK_tbl_Qc_SampleProcessBMR_ID = MasterID,
                    FK_tbl_Qc_Test_ID = s.FK_tbl_Qc_Test_ID,
                    TestDescription = s.TestDescription,
                    Specification = s.Specification,
                    RangeFrom = s.RangeFrom,
                    RangeTill = s.RangeTill,
                    FK_tbl_Inv_MeasurementUnit_ID = s.FK_tbl_Inv_MeasurementUnit_ID,
                    ResultValue = 0,
                    ResultRemarks = null,
                    IsPrintOnCOA = true,
                    CreatedBy = userName,
                    CreatedDate = currentDateTime,
                    ModifiedBy = null,
                    ModifiedDate = null
                }).ToList();

                if (qcTestList.Count == 0)
                    return "No Test Found in Composition for Replication";

                await db.tbl_Qc_SampleProcessBMR_QcTests.AddRangeAsync(qcTestList);
                await db.SaveChangesAsync();

                return "OK";
            }
            else
            {
                return "QC Sample Record Not Found";
            }

        }

        #endregion

        #region BPRSample QcTest
        public async Task<object> GetBPRSampleQcTest(int id)
        {
            var qry = from o in await db.tbl_Qc_SampleProcessBPR_QcTests.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Qc_SampleProcessBPR_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.ResultValue,
                          o.ResultRemarks,
                          o.IsPrintOnCOA,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBPRSampleQcTest()
        {
            return new[]
            {
                new { n = "by Test Name", v = "byTestName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBPRSampleQcTest(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Qc_SampleProcessBPR_QcTests
                                      .Where(w => w.FK_tbl_Qc_SampleProcessBPR_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_SampleProcessBPR_QcTests
                                        .Where(w => w.FK_tbl_Qc_SampleProcessBPR_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.tbl_Qc_Test.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Qc_SampleProcessBPR_ID,
                          o.FK_tbl_Qc_Test_ID,
                          FK_tbl_Qc_Test_IDName = o.tbl_Qc_Test.TestName,
                          o.TestDescription,
                          o.Specification,
                          o.RangeFrom,
                          o.RangeTill,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.FK_tbl_Inv_MeasurementUnit_ID.HasValue ? o.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.ResultValue,
                          o.ResultRemarks,
                          o.IsPrintOnCOA,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBPRSampleQcTest(tbl_Qc_SampleProcessBPR_QcTest tbl_Qc_SampleProcessBPR_QcTest, string operation = "", string userName = "")
        {
            if (operation == "Save New" && tbl_Qc_SampleProcessBPR_QcTest.ID == 0)
            {
                tbl_Qc_SampleProcessBPR_QcTest.CreatedBy = userName;
                tbl_Qc_SampleProcessBPR_QcTest.CreatedDate = DateTime.Now;
                db.tbl_Qc_SampleProcessBPR_QcTests.Add(tbl_Qc_SampleProcessBPR_QcTest);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update" || (operation == "Save New" && tbl_Qc_SampleProcessBPR_QcTest.ID > 0))
            {
                tbl_Qc_SampleProcessBPR_QcTest.ModifiedBy = userName;
                tbl_Qc_SampleProcessBPR_QcTest.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Qc_SampleProcessBPR_QcTest).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Qc_SampleProcessBPR_QcTests.Remove(db.tbl_Qc_SampleProcessBPR_QcTests.Find(tbl_Qc_SampleProcessBPR_QcTest.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        public async Task<string> PostBPRSampleQcTestReplicationFromStandard(int MasterID, string userName = "")
        {
            var SampleProcessBMR = await db.tbl_Qc_SampleProcessBPRs.Where(w => w.ID == MasterID).FirstOrDefaultAsync();

            if (db.tbl_Qc_SampleProcessBPR_QcTests.Count(c => c.FK_tbl_Qc_SampleProcessBPR_ID == MasterID) > 0)
                return "Replication Aborted! because the test list has already been entered";

            if (SampleProcessBMR != null)
            {
                var CompositionCouplingID = SampleProcessBMR.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster?.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID ?? 0;
                if (CompositionCouplingID == 0)
                    return "No Link Found From Batch to Composition Coupling";

                var ProcedureID = SampleProcessBMR.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.FK_tbl_Pro_Procedure_ID;

                var StandardQcTestList = await db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests
                    .Where(w => w.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID == CompositionCouplingID
                                &&
                                w.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.FK_tbl_Pro_Procedure_ID == ProcedureID
                          )
                    .ToListAsync();

                DateTime currentDateTime = DateTime.Now;

                var qcTestList = StandardQcTestList.Select(s => new tbl_Qc_SampleProcessBPR_QcTest
                {
                    ID = 0,
                    FK_tbl_Qc_SampleProcessBPR_ID = MasterID,
                    FK_tbl_Qc_Test_ID = s.FK_tbl_Qc_Test_ID,
                    TestDescription = s.TestDescription,
                    Specification = s.Specification,
                    RangeFrom = s.RangeFrom,
                    RangeTill = s.RangeTill,
                    FK_tbl_Inv_MeasurementUnit_ID = s.FK_tbl_Inv_MeasurementUnit_ID,
                    ResultValue = 0,
                    ResultRemarks = null,
                    IsPrintOnCOA = true,
                    CreatedBy = userName,
                    CreatedDate = currentDateTime,
                    ModifiedBy = null,
                    ModifiedDate = null
                }).ToList();

                if (qcTestList.Count == 0)
                    return "No Test Found in Composition for Replication";

                await db.tbl_Qc_SampleProcessBPR_QcTests.AddRangeAsync(qcTestList);
                await db.SaveChangesAsync();

                return "OK";
            }
            else
            {
                return "QC Sample Record Not Found";
            }

        }
        #endregion

    }
    public class QcLabRepository : IQcLab
    {
        private readonly OreasDbContext db;
        public QcLabRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Qc_Labs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.LabName,
                          o.Prefix,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };
            return qry.FirstOrDefault();
        }
        public object GetWCLQcLab()
        {
            return new[]
            {
                new { n = "by Lab Name", v = "byLabName" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Qc_Labs
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byLabName" && w.LabName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_Labs
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byLabName" && w.LabName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.LabName,
                          o.Prefix,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Qc_Lab tbl_Qc_Lab, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Qc_Lab.CreatedBy = userName;
                tbl_Qc_Lab.CreatedDate = DateTime.Now;
                db.tbl_Qc_Labs.Add(tbl_Qc_Lab);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Qc_Lab.ModifiedBy = userName;
                tbl_Qc_Lab.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Qc_Lab).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Qc_Labs.Remove(db.tbl_Qc_Labs.Find(tbl_Qc_Lab.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class QcTestRepository : IQcTest
    {
        private readonly OreasDbContext db;
        public QcTestRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Qc_Tests.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.TestName,
                          o.FK_tbl_Qc_Lab_ID,
                          FK_tbl_Qc_Lab_IDName = o.tbl_Qc_Lab.LabName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };
            return qry.FirstOrDefault();
        }
        public object GetWCLQcTest()
        {
            return new[]
            {
                new { n = "by Test Name", v = "byTestName" }, new { n = "by Lab Name", v = "byLabName" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Qc_Tests
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTestName" && w.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byLabName" && w.tbl_Qc_Lab.LabName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_Tests
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byTestName" && w.TestName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byLabName" && w.tbl_Qc_Lab.LabName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.TestName,
                          o.FK_tbl_Qc_Lab_ID,
                          FK_tbl_Qc_Lab_IDName = o.tbl_Qc_Lab.LabName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Qc_Test tbl_Qc_Test, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Qc_Test.CreatedBy = userName;
                tbl_Qc_Test.CreatedDate = DateTime.Now;
                db.tbl_Qc_Tests.Add(tbl_Qc_Test);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Qc_Test.ModifiedBy = userName;
                tbl_Qc_Test.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Qc_Test).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Qc_Tests.Remove(db.tbl_Qc_Tests.Find(tbl_Qc_Test.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class QcDashboardRepository : IQcDashboard
    {
        private readonly OreasDbContext db;
        public QcDashboardRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetDashBoardData(string userName = "")
        {
            int PN_PendingActionAfterSample = 0; int PN_PendingSamples = 0;
            int BMR_PendingAction = 0; int BPR_PendingAction = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[USP_Qc_DashBoard] @UserName ";
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
                        PN_PendingActionAfterSample = (int)sqlReader["PN_PendingActionAfterSample"];
                        PN_PendingSamples = (int)sqlReader["PN_PendingSamples"];

                        BMR_PendingAction = (int)sqlReader["BMR_PendingAction"];
                        BPR_PendingAction = (int)sqlReader["BPR_PendingAction"];

                    }
                }
            }
            return new
            {
                PN_PendingActionAfterSample,
                PN_PendingSamples,
                BMR_PendingAction,
                BPR_PendingAction

            };
        }

    }

}
