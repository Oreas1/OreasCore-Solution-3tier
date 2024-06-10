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
using iText.Commons.Actions.Contexts;
using OfficeOpenXml.Drawing.Controls;

namespace OreasServices
{
    public class QAProcessRepository : IQAProcess
    {
        private readonly OreasDbContext db;
        public QAProcessRepository(OreasDbContext oreasDbContext)
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
                new { n = "by QA BMR Pending", v = "byQABMRPending" }, new { n = "by QA BPR Pending", v = "byQABPRPending" }, new { n = "by Finished", v = "byFinished" }, new { n = "by Partial Finished", v = "byPartialFinished" }, new { n = "by InProcess", v = "byInProcess" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBatchRecordMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionMasters
                                               .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byQABMRPending" && w.IsQAClearanceBMRPending == true
                                                 ||
                                                 FilterByLoad == "byQABPRPending" && w.IsQAClearanceBPRPending == true
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
                                                 FilterByLoad == "byQABMRPending" && w.IsQAClearanceBMRPending == true
                                                 ||
                                                 FilterByLoad == "byQABPRPending" && w.IsQAClearanceBPRPending == true
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

        #region BMRProcess   
        public async Task<object> GetBMRProcess(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_ProductRegistrationMaster.ProductName ?? "",
                          MeasurementUnit = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.IsQAClearanceBeforeStart,
                          o.QACleared,
                          o.QAClearedBy,
                          QAClearedDate = o.QAClearedDate.HasValue ? o.QAClearedDate.Value.ToString("dd-MM-yyyy") : "",
                          o.Yield,
                          o.IsCompleted,
                          CompletedDate = o.CompletedDate.HasValue ? o.CompletedDate.Value.ToString("dd-MM-yyyy") : "",
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

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID && w.IsQAClearanceBeforeStart)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_ProductRegistrationMaster.ProductName ?? "",
                          MeasurementUnit = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.IsQAClearanceBeforeStart,
                          o.QACleared,
                          o.QAClearedBy,
                          QAClearedDate = o.QAClearedDate.HasValue ? o.QAClearedDate.Value.ToString("dd-MM-yyyy") : "",
                          o.Yield,
                          o.IsCompleted,
                          CompletedDate = o.CompletedDate.HasValue ? o.CompletedDate.Value.ToString("dd-MM-yyyy hh:mm tt") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRProcess(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            CRUD_Type.Value = "UpdateByQA";
            tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.QAClearedBy = userName;
            tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.QAClearedDate = DateTime.Now;

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionMaster_ProcessBMR] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionMaster_ID={4}
              ,@FK_tbl_Pro_Procedure_ID={5},@FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample={6}
              ,@IsQAClearanceBeforeStart={7},@QACleared={8},@QAClearedBy={9},@QAClearedDate={10}
              ,@Yield={11},@IsCompleted={12},@CompletedDate={13}
              ,@CreatedBy={14},@CreatedDate={15},@ModifiedBy={16},@ModifiedDate={17}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.ID, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
               tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.FK_tbl_Pro_Procedure_ID, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
               tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.IsQAClearanceBeforeStart, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.QACleared, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.QAClearedBy, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.QAClearedDate,
               tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.Yield, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.IsCompleted, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.CompletedDate,
               tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.CreatedBy, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.CreatedDate,
               tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.ModifiedBy, tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region BPRProcess
        public async Task<object> GetBPRProcess(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_ProductRegistrationMaster.ProductName ?? "",
                          MeasurementUnit = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.IsQAClearanceBeforeStart,
                          o.QACleared,
                          o.QAClearedBy,
                          QAClearedDate = o.QAClearedDate.HasValue ? o.QAClearedDate.Value.ToString("dd-MM-yyyy") : "",
                          o.Yield,
                          o.IsCompleted,
                          CompletedDate = o.CompletedDate.HasValue ? o.CompletedDate.Value.ToString("dd-MM-yyyy") : "",
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

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs
                                      .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID && w.IsQAClearanceBeforeStart)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs
                                        .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID,
                          o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.PackagingName,
                          o.FK_tbl_Pro_Procedure_ID,
                          FK_tbl_Pro_Procedure_IDName = o.tbl_Pro_Procedure.ProcedureName,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_QCSampleName = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_ProductRegistrationMaster.ProductName ?? "",
                          MeasurementUnit = o?.tbl_Inv_ProductRegistrationDetail_QCSample?.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.IsQAClearanceBeforeStart,
                          o.QACleared,
                          o.QAClearedBy,
                          QAClearedDate = o.QAClearedDate.HasValue ? o.QAClearedDate.Value.ToString("dd-MM-yyyy") : "",
                          o.Yield,
                          o.IsCompleted,
                          CompletedDate = o.CompletedDate.HasValue ? o.CompletedDate.Value.ToString("dd-MM-yyyy hh:mm tt") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBPRProcess(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            CRUD_Type.Value = "UpdateByQA";
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.QAClearedBy = userName;
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.QAClearedDate = DateTime.Now;

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID={4}
              ,@FK_tbl_Pro_Procedure_ID={5},@FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample={6}
              ,@IsQAClearanceBeforeStart={7},@QACleared={8},@QAClearedBy={9},@QAClearedDate={10}
              ,@Yield={11},@IsCompleted={12},@CompletedDate={13}
              ,@CreatedBy={14},@CreatedDate={15},@ModifiedBy={16},@ModifiedDate={17}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.ID, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID,
               tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.FK_tbl_Pro_Procedure_ID, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample,
               tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.IsQAClearanceBeforeStart, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.QACleared, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.QAClearedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.QAClearedDate,
               tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.Yield, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.IsCompleted, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.CompletedDate,
               tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.CreatedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.CreatedDate,
               tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.ModifiedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

    }
    public class QAProductionOrder : IQAProductionOrder
    {
        private readonly OreasDbContext db;
        public QAProductionOrder(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Production Order

        public object GetWCLProductionOrder()
        {
            return new[]
            {
                new { n = "by Batch No", v = "byBatchNo" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBProductionOrder()
        {
            return new[]
            {
                new { n = "by Processed", v = "byProcessed" }, new { n = "by UnProcessed", v = "byUnProcessed" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductionOrder(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrderNoteDetail_ProductionOrders
                                               .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byProcessed" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Count() > 0
                                                 ||
                                                 FilterByLoad == "byUnProcessed" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Count() == 0
                                                 )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && w.RequestedBatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_OrderNoteDetail.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrderNoteDetail_ProductionOrders
                                  .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byProcessed" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Count() > 0
                                                 ||
                                                 FilterByLoad == "byUnProcessed" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Count() == 0
                                                 )
                                  .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && w.RequestedBatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_OrderNoteDetail.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster.tbl_Ac_ChartOfAccounts.AccountName,
                          OrderNo = o.tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster.DocNo,
                          OrderDate = o.tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster.DocDate.ToString("dd-MMM-yy"),
                          TargetDate = o.tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster.TargetDate.ToString("dd-MMM-yy"),
                          o.tbl_Inv_OrderNoteDetail.AspNetOreasPriority.Priority,
                          o.tbl_Inv_OrderNoteDetail.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_OrderNoteDetail.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.tbl_Inv_OrderNoteDetail.Quantity,
                          o.tbl_Inv_OrderNoteDetail.MfgQtyLimit,
                          o.tbl_Inv_OrderNoteDetail.ManufacturingQty,
                          o.tbl_Inv_OrderNoteDetail.SoldQty,
                          o.RequestedBatchSize,
                          o.RequestedBatchNo,
                          RequestedMfgDate = o.RequestedMfgDate.ToString("dd-MMM-yy"),
                          o.RequestedBy,
                          RequestedDate = o.RequestedDate.HasValue ? o.RequestedDate.Value.ToString("dd-MMM-yy") : "",
                          o.ProcessedBy,
                          ProcessedDate = o.ProcessedDate.HasValue ? o.ProcessedDate.Value.ToString("dd-MMM-yy") : "",
                          ProcessedBatchNo = o?.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters?.FirstOrDefault()?.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo ?? "",
                          o.tbl_Inv_OrderNoteDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<object> GetBMRAvailability(int id)
        {
            var qry = from o in await db.USP_Pro_Composition_GetMaterialAvailabilitys.FromSqlRaw("EXECUTE [dbo].[USP_Pro_Composition_GetMaterialAvailability] {0} ", id).ToListAsync()
                      select new
                      {
                          o.FilterName,
                          o.WareHouseName,
                          o.ProductName,
                          o.CategoryName,
                          o.FormulaQty,
                          o.FormulaBatchSize,
                          o.FormulaPackSize,
                          o.UnitPrimary,
                          o.AvailableQty,
                          o.UnitItem,
                          o.FormulaFor
                      };

            return qry;
        }
        public async Task<string> GenerateBMR(int FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID, string SPOperation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            CRUD_Type.Value = SPOperation;            

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_OrderNoteDetail_ProductionOrder_GenerateBatch] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID={3},@ProcessedBy={4},@ProcessedDate={5}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID, userName, DateTime.Now);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Production Order Batch
        public async Task<PagedData<object>> LoadProductionOrderBatch(int CurrentPage = 1, string BatchNo = "", string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionMasters.Where(w => w.BatchNo.ToLower().Equals(BatchNo)).CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));
            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMasters.Where(w => w.BatchNo.ToLower().Equals(BatchNo))
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy"),
                          o.BatchNo,
                          o.BatchSize,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        #endregion
    }
    public class QAProductionTransferRepository : IQAProductionTransfer
    {
        private readonly OreasDbContext db;
        public QAProductionTransferRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master

        public object GetWCLProductionTransferMaster()
        {
            return new[]
            {
                new { n = "by Batch No", v = "byBatchNo" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBProductionTransferMaster()
        {
            return new[]
            {
                new { n = "by QA Cleared", v = "byQACleared" }, new { n = "by QA Not Cleared", v = "byQANotCleared" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductionTransferMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_ProductionTransferMasters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterByLoad)
                                                       ||
                                                       FilterByLoad == "byQACleared" && w.QAClearedAll == true
                                                       ||
                                                       FilterByLoad == "byQANotCleared" && w.QAClearedAll == false
                                                     )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && w.tbl_Pro_ProductionTransferDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_ProductionTransferDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_ProductionTransferMasters
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byQACleared" && w.QAClearedAll == true
                                            ||
                                            FilterByLoad == "byQANotCleared" && w.QAClearedAll == false
                                            )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byBatchNo" && w.tbl_Pro_ProductionTransferDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Pro_ProductionTransferDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.Remarks,
                          o.QAClearedAll,
                          o.ReceivedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalItems = o.tbl_Pro_ProductionTransferDetails.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Detail
        public async Task<object> GetProductionTransferDetail(int id)
        {
            var qry = from o in await db.tbl_Pro_ProductionTransferDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_ProductionTransferMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " x " + o.tbl_Inv_ProductRegistrationDetail.Split_Into.ToString(),
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo + " [" + o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.PackagingName + "]",
                          o.Quantity,
                          o.Remarks,
                          o.QACleared,
                          o.QAClearedBy,
                          QAClearedDate = o.QAClearedDate.HasValue ? o.QAClearedDate.Value.ToString("dd-MMM-yy hh:mm tt") : "",
                          o.Received,
                          o.ReceivedBy,
                          ReceivedDate = o.ReceivedDate.HasValue ? o.ReceivedDate.Value.ToString("dd-MMM-yy hh:mm tt") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLProductionTransferDetail()
        {
            return new[]
            {
                new { n = "by Batch No", v = "byBatchNo" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBProductionTransferDetail()
        {
            return new[]
            {
                 new { n = "by QA Cleared", v = "byQACleared" }, new { n = "by QA Pending", v = "byQAPending" }, new { n = "by QA Rejected", v = "byQARejected" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductionTransferDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_ProductionTransferDetails
                                               .Where(w => w.FK_tbl_Pro_ProductionTransferMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterByLoad)
                                                       ||
                                                       FilterByLoad == "byQACleared" && w.QACleared == true
                                                       ||
                                                       FilterByLoad == "byQAPending" && w.QACleared == null
                                                       ||
                                                       FilterByLoad == "byQARejected" && w.QACleared == false
                                                     )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_ProductionTransferDetails
                                  .Where(w => w.FK_tbl_Pro_ProductionTransferMaster_ID == MasterID)
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byQACleared" && w.QACleared == true
                                            ||
                                            FilterByLoad == "byQAPending" && w.QACleared == null
                                            ||
                                            FilterByLoad == "byQARejected" && w.QACleared == false
                                            )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byBatchNo" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_ProductionTransferMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " x " + o.tbl_Inv_ProductRegistrationDetail.Split_Into.ToString(),
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo + " [" + o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.PackagingName + "]",
                          o.Quantity,
                          o.Remarks,
                          o.QACleared,
                          o.QAClearedBy,
                          QAClearedDate = o.QAClearedDate.HasValue ? o.QAClearedDate.Value.ToString("dd-MM-yy hh:mm tt") : "",
                          o.Received,
                          o.ReceivedBy,
                          ReceivedDate = o.ReceivedDate.HasValue ? o.ReceivedDate.Value.ToString("dd-MM-yy hh:mm tt") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostProductionTransferDetail(tbl_Pro_ProductionTransferDetail tbl_Pro_ProductionTransferDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            
            CRUD_Type.Value = "QAUpdate";

            tbl_Pro_ProductionTransferDetail.QAClearedBy = userName;
            tbl_Pro_ProductionTransferDetail.QAClearedDate = DateTime.Now;

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_ProductionTransferDetail] 
                    @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                    ,@ID={3},@FK_tbl_Pro_ProductionTransferMaster_ID={4}
                    ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={6}
                    ,@Quantity={7},@Remarks={8}
                    ,@QACleared={9},@QAClearedBy={10},@QAClearedDate={11}
                    ,@Received={12},@ReceivedBy={13},@ReceivedDate={14}
                    ,@CreatedBy={15},@CreatedDate={16},@ModifiedBy={17},@ModifiedDate={18}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Pro_ProductionTransferDetail.ID, tbl_Pro_ProductionTransferDetail.FK_tbl_Pro_ProductionTransferMaster_ID,
                tbl_Pro_ProductionTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_ID, tbl_Pro_ProductionTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                tbl_Pro_ProductionTransferDetail.Quantity, tbl_Pro_ProductionTransferDetail.Remarks,
                tbl_Pro_ProductionTransferDetail.QACleared, tbl_Pro_ProductionTransferDetail.QAClearedBy, tbl_Pro_ProductionTransferDetail.QAClearedDate,
                tbl_Pro_ProductionTransferDetail.Received, tbl_Pro_ProductionTransferDetail.ReceivedBy, tbl_Pro_ProductionTransferDetail.ReceivedDate,
                tbl_Pro_ProductionTransferDetail.CreatedBy, tbl_Pro_ProductionTransferDetail.CreatedDate, tbl_Pro_ProductionTransferDetail.ModifiedBy, tbl_Pro_ProductionTransferDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

    }
}
