using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Action;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Drawing.Controls;
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
    public class PDRequestRepository : IPDRequest
    {
        private readonly OreasDbContext db;
        public PDRequestRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Request Master
        public async Task<object> GetRequestMaster(int id)
        {
            var qry = from o in await db.tbl_PD_RequestMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "s]",
                          o.SampleProductExpiryMonths,
                          o.SampleProductMRP,
                          SampleProductPhoto = o?.SampleProductPhoto ?? "",
                          o.Remarks,
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLRequestMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }, new { n = "by Doc No", v = "byDocNo" }
            }.ToList();
        }
        public object GetWCLBRequestMaster()
        {
            return new[]
            {
                new { n = "by Sucessfull", v = "bySucessfull" }, new { n = "by Failed", v = "byFailed" }, new { n = "by Pending", v = "byPending" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadRequestMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_PD_RequestMasters
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                            )
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "bySucessfull" && w.FinalStatus == true
                                                ||
                                                FilterByLoad == "byFailed" && w.FinalStatus == false
                                                ||
                                                FilterByLoad == "byPending" && w.FinalStatus == null
                                                )
                                    .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_PD_RequestMasters
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                            )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "bySucessfull" && w.FinalStatus == true
                                                ||
                                                FilterByLoad == "byFailed" && w.FinalStatus == false
                                                ||
                                                FilterByLoad == "byPending" && w.FinalStatus == null
                                                )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "s]",
                          o.SampleProductExpiryMonths,
                          o.SampleProductMRP,
                          SampleProductPhoto = string.IsNullOrEmpty(o.SampleProductPhoto) ? "No" : "Yes",
                          o.Remarks,
                          o.FinalStatus,
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostRequestMaster(tbl_PD_RequestMaster tbl_PD_RequestMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_PD_RequestMaster.CreatedBy = userName;
                tbl_PD_RequestMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_PD_RequestMaster.ModifiedBy = userName;
                tbl_PD_RequestMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_PD_RequestMaster] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@DocNo={4},@DocDate={5},@FK_tbl_Inv_ProductRegistrationDetail_ID={6}
                  ,@FK_tbl_Inv_ProductRegistrationDetail_ID_Primary={7},@SampleProductExpiryMonths={8}
                  ,@SampleProductMRP={9},@SampleProductPhoto={10},@Remarks={11}
                  ,@CreatedBy={12},@CreatedDate={13},@ModifiedBy={14},@ModifiedDate={15}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_PD_RequestMaster.ID, tbl_PD_RequestMaster.DocNo, tbl_PD_RequestMaster.DocDate, tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID,
            tbl_PD_RequestMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary, tbl_PD_RequestMaster.SampleProductExpiryMonths,
            tbl_PD_RequestMaster.SampleProductMRP, tbl_PD_RequestMaster.SampleProductPhoto, tbl_PD_RequestMaster.Remarks,
            tbl_PD_RequestMaster.CreatedBy, tbl_PD_RequestMaster.CreatedDate, tbl_PD_RequestMaster.ModifiedBy, tbl_PD_RequestMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Request Detail TR
        public async Task<object> GetRequestDetailTR(int id)
        {
            var qry = from o in await db.tbl_PD_RequestDetailTRs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestMaster_ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          MfgDate = o.MfgDate.ToString("dd-MMM-yyyy") ?? "",
                          o.TrialBatchNo,
                          o.TrialBatchSizeInSemiUnits,
                          o.TrialStatus,
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLRequestDetailTR()
        {
            return new[]
            {
                new { n = "by BatchNo", v = "byBatchNo" }, new { n = "by Doc No", v = "byDocNo" }
            }.ToList();
        }
        public object GetWCLBRequestDetailTR()
        {
            return new[]
            {
                new { n = "by Sucessfull", v = "bySucessfull" }, new { n = "by Failed", v = "byFailed" }, new { n = "by Pending", v = "byPending" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadRequestDetailTR(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_PD_RequestDetailTRs
                                    .Where(w => w.FK_tbl_PD_RequestMaster_ID == MasterID)
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byBatchNo" && w.TrialBatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                            )
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "bySucessfull" && w.TrialStatus == true
                                                ||
                                                FilterByLoad == "byFailed" && w.TrialStatus == false
                                                ||
                                                FilterByLoad == "byPending" && w.TrialStatus == null
                                                )
                                    .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_PD_RequestDetailTRs
                                  .Where(w => w.FK_tbl_PD_RequestMaster_ID == MasterID)
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byBatchNo" && w.TrialBatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                            )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "bySucessfull" && w.TrialStatus == true
                                                ||
                                                FilterByLoad == "byFailed" && w.TrialStatus == false
                                                ||
                                                FilterByLoad == "byPending" && w.TrialStatus == null
                                                )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestMaster_ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          MfgDate = o.MfgDate.ToString("dd-MMM-yyyy") ?? "",
                          o.TrialBatchNo,
                          o.TrialBatchSizeInSemiUnits,
                          o.TrialStatus,
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostRequestDetailTR(tbl_PD_RequestDetailTR tbl_PD_RequestDetailTR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New" && tbl_PD_RequestDetailTR.ID == 0)
            {
                tbl_PD_RequestDetailTR.CreatedBy = userName;
                tbl_PD_RequestDetailTR.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if ((operation == "Save Update") || (operation == "Save New" && tbl_PD_RequestDetailTR.ID > 0))
            {
                tbl_PD_RequestDetailTR.ModifiedBy = userName;
                tbl_PD_RequestDetailTR.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_PD_RequestDetailTR] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_PD_RequestMaster_ID={4},@DocNo={5}
              ,@DocDate={6},@MfgDate={7},@TrialBatchNo={8},@TrialBatchSizeInSemiUnits={9},@TrialStatus={10}
              ,@CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_PD_RequestDetailTR.ID, tbl_PD_RequestDetailTR.FK_tbl_PD_RequestMaster_ID, tbl_PD_RequestDetailTR.DocNo,
            tbl_PD_RequestDetailTR.DocDate, tbl_PD_RequestDetailTR.MfgDate, tbl_PD_RequestDetailTR.TrialBatchNo, tbl_PD_RequestDetailTR.TrialBatchSizeInSemiUnits, tbl_PD_RequestDetailTR.TrialStatus,
            tbl_PD_RequestDetailTR.CreatedBy, tbl_PD_RequestDetailTR.CreatedDate, tbl_PD_RequestDetailTR.ModifiedBy, tbl_PD_RequestDetailTR.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Request Detail TR Procedure
        public async Task<object> GetRequestDetailTRProcedure(int id)
        {
            var qry = from o in await db.tbl_PD_RequestDetailTR_Procedures.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName + " [" + (o.tbl_Pro_Procedure.ForRaw1_Packaging0 ? "BMR" : "BPR") + "]",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLRequestDetailTRProcedure()
        {
            return new[]
            {
                new { n = "by Procedure Name", v = "byProcedureName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadRequestDetailTRProcedure(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_PD_RequestDetailTR_Procedures
                                    .Where(w => w.FK_tbl_PD_RequestDetailTR_ID == MasterID)
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                            )
                                    .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_PD_RequestDetailTR_Procedures
                                  .Where(w => w.FK_tbl_PD_RequestDetailTR_ID == MasterID)
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                            )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName + " [" + (o.tbl_Pro_Procedure.ForRaw1_Packaging0 ? "BMR" : "BPR") + "]",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostRequestDetailTRProcedure(tbl_PD_RequestDetailTR_Procedure tbl_PD_RequestDetailTR_Procedure, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_PD_RequestDetailTR_Procedure.CreatedBy = userName;
                tbl_PD_RequestDetailTR_Procedure.CreatedDate = DateTime.Now;
                db.tbl_PD_RequestDetailTR_Procedures.Add(tbl_PD_RequestDetailTR_Procedure);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_PD_RequestDetailTR_Procedure.ModifiedBy = userName;
                tbl_PD_RequestDetailTR_Procedure.ModifiedDate = DateTime.Now;
                db.Entry(tbl_PD_RequestDetailTR_Procedure).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_PD_RequestDetailTR_Procedures.Remove(db.tbl_PD_RequestDetailTR_Procedures.Find(tbl_PD_RequestDetailTR_Procedure.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

        #region Request Detail TR CFP
        public async Task<object> GetRequestDetailTRCFP(int id)
        {
            var qry = from o in await db.tbl_PD_RequestDetailTR_CFPs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLRequestDetailTRCFP()
        {
            return new[]
            {
                new { n = "by Filer Policy Name", v = "byFilerPolicyName" }, new { n = "by WareHouse Name", v = "byWareHouseName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadRequestDetailTRCFP(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_PD_RequestDetailTR_CFPs
                                    .Where(w => w.FK_tbl_PD_RequestDetailTR_ID == MasterID)
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byFilerPolicyName" && w.tbl_Pro_CompositionFilterPolicyDetail.FilterName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byWareHouseName" && w.tbl_Inv_WareHouseMaster.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                            )
                                    .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_PD_RequestDetailTR_CFPs
                                  .Where(w => w.FK_tbl_PD_RequestDetailTR_ID == MasterID)
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byFilerPolicyName" && w.tbl_Pro_CompositionFilterPolicyDetail.FilterName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byWareHouseName" && w.tbl_Inv_WareHouseMaster.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                            )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostRequestDetailTRCFP(tbl_PD_RequestDetailTR_CFP tbl_PD_RequestDetailTR_CFP, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_PD_RequestDetailTR_CFP.CreatedBy = userName;
                tbl_PD_RequestDetailTR_CFP.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_PD_RequestDetailTR_CFP.ModifiedBy = userName;
                tbl_PD_RequestDetailTR_CFP.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_PD_RequestDetailTR_CFP] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_PD_RequestDetailTR_ID={4}
              ,@FK_tbl_Pro_CompositionFilterPolicyDetail_ID={5},@FK_tbl_Inv_WareHouseMaster_ID={6}
              ,@CreatedBy={7},@CreatedDate={8},@ModifiedBy={8},@ModifiedDate={10}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_PD_RequestDetailTR_CFP.ID, tbl_PD_RequestDetailTR_CFP.FK_tbl_PD_RequestDetailTR_ID,
            tbl_PD_RequestDetailTR_CFP.FK_tbl_Pro_CompositionFilterPolicyDetail_ID, tbl_PD_RequestDetailTR_CFP.FK_tbl_Inv_WareHouseMaster_ID,
            tbl_PD_RequestDetailTR_CFP.CreatedBy, tbl_PD_RequestDetailTR_CFP.CreatedDate, tbl_PD_RequestDetailTR_CFP.ModifiedBy, tbl_PD_RequestDetailTR_CFP.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Request Detail TR Item
        public async Task<object> GetRequestDetailTRCFPItem(int id)
        {
            var qry = from o in await db.tbl_PD_RequestDetailTR_CFP_Items.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_CFP_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.RequiredTrue_ReturnFalse,
                          o.Remarks,
                          o.IsDispensed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLRequestDetailTRCFPItem()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadRequestDetailTRCFPItem(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_PD_RequestDetailTR_CFP_Items
                                    .Where(w => w.FK_tbl_PD_RequestDetailTR_CFP_ID == MasterID)
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                          )
                                    .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_PD_RequestDetailTR_CFP_Items
                                  .Where(w => w.FK_tbl_PD_RequestDetailTR_CFP_ID == MasterID)
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                          )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_CFP_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.RequiredTrue_ReturnFalse,
                          o.Remarks,
                          o.IsDispensed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostRequestDetailTRCFPItem(tbl_PD_RequestDetailTR_CFP_Item tbl_PD_RequestDetailTR_CFP_Item, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_PD_RequestDetailTR_CFP_Item.CreatedBy = userName;
                tbl_PD_RequestDetailTR_CFP_Item.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_PD_RequestDetailTR_CFP_Item.ModifiedBy = userName;
                tbl_PD_RequestDetailTR_CFP_Item.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_PD_RequestDetailTR_CFP_Item] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_PD_RequestDetailTR_CFP_ID={4}
              ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
              ,@Quantity={6},@RequiredTrue_ReturnFalse={7},@Remarks={8},@IsDispensed={9}
              ,@CreatedBy={10},@CreatedDate={11},@ModifiedBy={12},@ModifiedDate={13}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_PD_RequestDetailTR_CFP_Item.ID, tbl_PD_RequestDetailTR_CFP_Item.FK_tbl_PD_RequestDetailTR_CFP_ID,
            tbl_PD_RequestDetailTR_CFP_Item.FK_tbl_Inv_ProductRegistrationDetail_ID,
            tbl_PD_RequestDetailTR_CFP_Item.Quantity, tbl_PD_RequestDetailTR_CFP_Item.RequiredTrue_ReturnFalse, tbl_PD_RequestDetailTR_CFP_Item.Remarks, tbl_PD_RequestDetailTR_CFP_Item.IsDispensed,
            tbl_PD_RequestDetailTR_CFP_Item.CreatedBy, tbl_PD_RequestDetailTR_CFP_Item.CreatedDate, tbl_PD_RequestDetailTR_CFP_Item.ModifiedBy, tbl_PD_RequestDetailTR_CFP_Item.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLTR()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Trial BMR",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Trial BMR")
            {
                return await Task.Run(() => TRCompleteAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Single Item Request")
            {
                return await Task.Run(() => SingleItemRequestAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> SingleItemRequestAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
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
            string CreatedBy = ""; string CreatedDate = "";
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_PD_Request] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                        //----------Row 1

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yyyy"))).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Mfg Date:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["MfgDate"]).ToString("dd-MMM-yyyy"))).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Exp Months:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["SampleProductExpiryMonths"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //----------Row 2
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("For Product:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["PDProductName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch No:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TrialBatchNo"].ToString()).SetBold()).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch Size:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TrialBatchSizeInSemiUnits"].ToString() + " " + sqlReader["PDMeasurementUnit"].ToString()).SetBold()).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //----------Row 3
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Composition:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["FilterName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //----------Row 4
                        pdftableMaster.AddCell(new Cell(1, 8).Add(new Paragraph().Add("\n")).SetBold().SetMaxWidth(0.05f).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Detail")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //----------Row 5
                        pdftableMaster.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Ingredient")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("+ / -")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Dispensed")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequiredTrue_ReturnFalse"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["IsDispensed"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                       
                        CreatedDate = !sqlReader.IsDBNull("CreatedDate") ? ((DateTime)sqlReader["CreatedDate"]).ToString("dd-MMM-yy hh:mm tt") : "";
                        CreatedBy = sqlReader["CreatedBy"].ToString();

                        pdftableMaster.AddCell(new Cell(1, 8).Add(new Paragraph().Add("This is created by " + CreatedBy + " on " + CreatedDate)).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                    }
                }


            }



            /////////////------------------------------Signature Footer table------------------------------////////////////
            Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Created By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Checked By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Approved By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Issued By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


            page.InsertContent(pdftableMaster);
            page.InsertContent(pdftableSignature);


            return page.FinishToGetBytes();
        }

        private async Task<byte[]> TRCompleteAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
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
            //--------------------------------5 column Detail table ------------------------------//
            Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.10),//SNo
                        (float)(PageSize.A4.GetWidth()*0.50),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.12),//Quantity
                        (float)(PageSize.A4.GetWidth()*0.05),//Req / Ret
                        (float)(PageSize.A4.GetWidth()*0.23) //ReferenceNo     
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            //--------------------------------3 column Procedures ------------------------------//
            Table pdftableDetailProcedure = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.10),//SNo                        
                        (float)(PageSize.A4.GetWidth()*0.75),//Procedure Name
                        (float)(PageSize.A4.GetWidth()*0.15),//For
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            string CreatedBy = "";
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_PD_Request] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string BatchSizeUnit = "";
                //----------master
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        //----------Row 1

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yyyy"))).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Mfg Date:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["MfgDate"]).ToString("dd-MMM-yyyy"))).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Exp Months:")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["SampleProductExpiryMonths"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //----------Row 2
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Product:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch No:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TrialBatchNo"].ToString()).SetBold()).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //----------Row 3
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Description:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["Description"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch Size:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TrialBatchSizeInSemiUnits"].ToString() + " " + sqlReader["MeasurementUnit"].ToString()).SetBold()).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                        BatchSizeUnit = sqlReader["MeasurementUnit"].ToString();
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }

                int SNo = 1;
                string FilterName = "";

                //---------detail
                pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add("\n")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Material Detail")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                
                ReportName.Value = rn + "2";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (FilterName != sqlReader["FilterName"].ToString())
                        {
                            FilterName = sqlReader["FilterName"].ToString();

                            pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["FilterName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("+ / -")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            SNo = 1;
                        }
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequiredTrue_ReturnFalse"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceList"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                        SNo++;
                    }
                }

                //---------Procedure
                pdftableDetailProcedure.AddCell(new Cell(1, 3).Add(new Paragraph().Add("\n")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetailProcedure.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Procedure Detail")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                ReportName.Value = rn + "3"; SNo = 1;

                pdftableDetailProcedure.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetailProcedure.AddCell(new Cell().Add(new Paragraph().Add("Procedure Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetailProcedure.AddCell(new Cell().Add(new Paragraph().Add("For")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                       
                        pdftableDetailProcedure.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailProcedure.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProcedureName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailProcedure.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ForRaw1_Packaging0"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                    }
                }

            }

            

            /////////////------------------------------Signature Footer table------------------------------////////////////
            Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Issued By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Received By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


            pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Officer QC")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Manager QC")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Officer RND")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Manager RND")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            page.InsertContent(pdftableMaster);
            page.InsertContent(pdftableDetail);
            page.InsertContent(pdftableSignature);

            page.InsertNewPage();
            page.InsertContent(pdftableDetailProcedure);


            return page.FinishToGetBytes();
        }

        #endregion

    }
}
