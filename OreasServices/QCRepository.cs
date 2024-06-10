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

    }
    public class QcPurchaseNoteRepository : IQcPurchaseNote
    {
        private readonly OreasDbContext db;
        public QcPurchaseNoteRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
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
                          o.Remarks
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
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
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
                          o.Remarks
                      };


            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "", string userName = "")
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
                tbl_Inv_PurchaseNoteDetail.GSTPercentage, tbl_Inv_PurchaseNoteDetail.GSTAmount,tbl_Inv_PurchaseNoteDetail.FreightIn, tbl_Inv_PurchaseNoteDetail.DiscountAmount, tbl_Inv_PurchaseNoteDetail.CostAmount,
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

        #region Report   
        public List<ReportCallingModel> GetRLQcQaPurchaseNote()
        {
            return new List<ReportCallingModel>() {                
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
                        pdftableMaster.AddCell(new Cell(1,4).Add(new Paragraph().Add(sqlReader["ActionName"].ToString())).SetBold().SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Product Name \n\n")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Date")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("By")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("\n\n" + "____________________\n" + sqlReader["CreatedBy"].ToString()  )).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftable.AddCell(new Cell().Add(pdftableMaster).SetBorder(Border.NO_BORDER));
                        pdftable.AddCell(new Cell().Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER));
                        pdftable.AddCell(new Cell().Add(pdftableMaster).SetBorder(Border.NO_BORDER));

                        pdftable.AddCell(new Cell(1,3).Add(new Paragraph().Add("\n\n")).SetBorder(Border.NO_BORDER));

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
    public class QCProcessRepository : IQCProcess
    {
        private readonly OreasDbContext db;
        public QCProcessRepository(OreasDbContext oreasDbContext)
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
                          p.IsCompleted
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
                          p.IsCompleted
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

    }
}
