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
    public class ProductionListRepository : IProductionList
    {
        private readonly OreasDbContext db;
        public ProductionListRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetCompositionFilterPolicyListAsync(bool? ForRaw1_Packaging0 = null)
        {
            if (ForRaw1_Packaging0 == null)
                return await (from a in db.tbl_Pro_CompositionFilterPolicyDetails
                              select new
                              {
                                  a.ID,
                                  FilterName = a.FilterName + " [" + a.tbl_Inv_WareHouseMaster.WareHouseName + "]"
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Pro_CompositionFilterPolicyDetails
                                        .Where(w => w.ForRaw1_Packaging0 == ForRaw1_Packaging0)
                              select new
                              {
                                  a.ID,
                                  FilterName = a.FilterName + " [" + a.tbl_Inv_WareHouseMaster.WareHouseName + "]"
                              }).ToListAsync();
        }
        public async Task<object> GetProProcedureListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Pro_Procedures
                              select new
                              {
                                  a.ID,
                                  ProcedureName = a.ProcedureName + " [" + (a.ForRaw1_Packaging0 ? "BMR" : "BPR") + "]"
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Pro_Procedures
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byBMRBPR" && FilterValueByText.ToUpper() == "BMR" && w.ForRaw1_Packaging0 == true
                                        ||
                                        FilterByText == "byBMRBPR" && FilterValueByText.ToUpper() == "BPR" && w.ForRaw1_Packaging0 == false
                                        )
                              select new
                              {
                                  a.ID,
                                  a.ProcedureName
                              }).ToListAsync();
        }
        public async Task<object> GetBMRAdditionalTypeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Pro_BMRAdditionalTypes
                              select new
                              {
                                  a.ID,
                                  a.TypeName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Pro_BMRAdditionalTypes
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.TypeName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.TypeName
                              }).Take(5).ToListAsync();
        }
    }
    public class CompositionFilterPolicyRepository : ICompositionFilterPolicy
    {
        private readonly OreasDbContext db;
        public CompositionFilterPolicyRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetCompositionFilterPolicyMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionFilterPolicyMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductType_ID_For_Coupling,
                          FK_tbl_Inv_ProductType_ID_For_CouplingName = o.tbl_Inv_ProductType_Coupling.ProductType,
                          o.FK_tbl_Inv_WareHouseMaster_ID_By,
                          FK_tbl_Inv_WareHouseMaster_ID_ByName = o.tbl_Inv_WareHouseMaster_By.WareHouseName,
                          o.FK_tbl_Inv_ProductType_ID_QCSample,
                          FK_tbl_Inv_ProductType_ID_QCSampleName = o.tbl_Inv_ProductType_QCSample.ProductType,
                          o.FK_tbl_Qc_ActionType_ID_BMRProcessTestingSample,
                          FK_tbl_Qc_ActionType_ID_BMRProcessTestingSampleName = o.tbl_Qc_ActionType.ActionName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public async Task<PagedData<object>> LoadCompositionFilterPolicyMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionFilterPolicyMasters
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionFilterPolicyMasters
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductType_ID_For_Coupling,
                          FK_tbl_Inv_ProductType_ID_For_CouplingName = o.tbl_Inv_ProductType_Coupling.ProductType,
                          o.FK_tbl_Inv_WareHouseMaster_ID_By,
                          FK_tbl_Inv_WareHouseMaster_ID_ByName = o.tbl_Inv_WareHouseMaster_By.WareHouseName,
                          o.FK_tbl_Inv_ProductType_ID_QCSample,
                          FK_tbl_Inv_ProductType_ID_QCSampleName = o.tbl_Inv_ProductType_QCSample.ProductType,
                          o.FK_tbl_Qc_ActionType_ID_BMRProcessTestingSample,
                          FK_tbl_Qc_ActionType_ID_BMRProcessTestingSampleName = o.tbl_Qc_ActionType.ActionName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCompositionFilterPolicyMaster(tbl_Pro_CompositionFilterPolicyMaster tbl_Pro_CompositionFilterPolicyMaster, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                //tbl_Pro_CompositionFilterPolicyMaster.CreatedBy = userName;
                //tbl_Pro_CompositionFilterPolicyMaster.CreatedDate = DateTime.Now;
                //db.tbl_Pro_CompositionFilterPolicyMasters.Add(tbl_Pro_CompositionFilterPolicyMaster);
                //await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionFilterPolicyMaster.ModifiedBy = userName;
                tbl_Pro_CompositionFilterPolicyMaster.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionFilterPolicyMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Pro_CompositionFilterPolicyMasters.Remove(db.tbl_Pro_CompositionFilterPolicyMasters.Find(tbl_Pro_CompositionFilterPolicyMaster.ID));
                //await db.SaveChangesAsync();
            }
            return "OK";
        }
   
        #endregion

        #region Detail
        public async Task<object> GetCompositionFilterPolicyDetail(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionFilterPolicyDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyMaster_ID,
                          o.FilterName,
                          o.ForRaw1_Packaging0,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCompositionFilterPolicyDetail()
        {
            return new[]
            {
                new { n = "by Filter Name", v = "byFilterName" }, new { n = "by WareHouse", v = "byWareHouse" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionFilterPolicyDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionFilterPolicyDetails
                                               .Where(w => w.FK_tbl_Pro_CompositionFilterPolicyMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byFilterName" && w.FilterName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byWareHouse" && w.tbl_Inv_WareHouseMaster.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionFilterPolicyDetails
                                  .Where(w => w.FK_tbl_Pro_CompositionFilterPolicyMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byFilterName" && w.FilterName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byWareHouse" && w.tbl_Inv_WareHouseMaster.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyMaster_ID,
                          o.FilterName,
                          o.ForRaw1_Packaging0,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCompositionFilterPolicyDetail(tbl_Pro_CompositionFilterPolicyDetail tbl_Pro_CompositionFilterPolicyDetail, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionFilterPolicyDetail.FilterName = tbl_Pro_CompositionFilterPolicyDetail.FilterName.ToUpper();
                tbl_Pro_CompositionFilterPolicyDetail.CreatedBy = userName;
                tbl_Pro_CompositionFilterPolicyDetail.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionFilterPolicyDetails.Add(tbl_Pro_CompositionFilterPolicyDetail);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionFilterPolicyDetail.FilterName = tbl_Pro_CompositionFilterPolicyDetail.FilterName.ToUpper();
                tbl_Pro_CompositionFilterPolicyDetail.ModifiedBy = userName;
                tbl_Pro_CompositionFilterPolicyDetail.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionFilterPolicyDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionFilterPolicyDetails.Remove(db.tbl_Pro_CompositionFilterPolicyDetails.Find(tbl_Pro_CompositionFilterPolicyDetail.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

    }
    public class ProProcedureRepository : IProProcedure
    {
        private readonly OreasDbContext db;
        public ProProcedureRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Pro_Procedures.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.ProcedureName,
                          o.ForRaw1_Packaging0,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLProProcedure()
        {
            return new[]
            {
                new { n = "by Process Name", v = "byName" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(string caller = "",int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_Procedures
                                               .Where(w=> 
                                                        caller.ToUpper() == "BMR" && w.ForRaw1_Packaging0 == true
                                                        ||
                                                        caller.ToUpper() == "BPR" && w.ForRaw1_Packaging0 == false
                                                        )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byName" && w.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_Procedures
                                  .Where(w =>
                                            caller.ToUpper() == "BMR" && w.ForRaw1_Packaging0 == true
                                            ||
                                            caller.ToUpper() == "BPR" && w.ForRaw1_Packaging0 == false
                                         )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.ProcedureName,
                          o.ForRaw1_Packaging0,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Pro_Procedure tbl_Pro_Procedure, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_Procedure.CreatedBy = userName;
                tbl_Pro_Procedure.CreatedDate = DateTime.Now;
                db.tbl_Pro_Procedures.Add(tbl_Pro_Procedure);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_Procedure.ModifiedBy = userName;
                tbl_Pro_Procedure.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_Procedure).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_Procedures.Remove(db.tbl_Pro_Procedures.Find(tbl_Pro_Procedure.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class CompositionRepository : IComposition
    {
        private readonly OreasDbContext db;
        public CompositionRepository(OreasDbContext oreasDbContext)
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
        public async Task<string> PostCompositionMaster(tbl_Pro_CompositionMaster tbl_Pro_CompositionMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_CompositionMaster.CreatedBy = userName;
                tbl_Pro_CompositionMaster.CreatedDate = DateTime.Now;
                //db.tbl_Pro_CompositionMasters.Add(tbl_Pro_CompositionMaster);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionMaster.ModifiedBy = userName;
                tbl_Pro_CompositionMaster.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Pro_CompositionMaster).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Pro_CompositionMasters.Remove(db.tbl_Pro_CompositionMasters.Find(tbl_Pro_CompositionMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }
            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_CompositionMaster] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@DocNo={4},@DocDate={5},@CompositionName={6},@ShelfLifeInMonths={7}
              ,@DimensionValue={8},@FK_tbl_Inv_MeasurementUnit_ID_Dimension={9}
              ,@RevisionNo={10},@RevisionDate={11},@CreatedBy={12},@CreatedDate={13},@ModifiedBy={14},@ModifiedDate={15}",
              CRUD_Type, CRUD_Msg, CRUD_ID,
              tbl_Pro_CompositionMaster.ID, tbl_Pro_CompositionMaster.DocNo, tbl_Pro_CompositionMaster.DocDate, tbl_Pro_CompositionMaster.CompositionName, tbl_Pro_CompositionMaster.ShelfLifeInMonths,
              tbl_Pro_CompositionMaster.DimensionValue, tbl_Pro_CompositionMaster.FK_tbl_Inv_MeasurementUnit_ID_Dimension,
              tbl_Pro_CompositionMaster.RevisionNo, tbl_Pro_CompositionMaster.RevisionDate, 
              tbl_Pro_CompositionMaster.CreatedBy, tbl_Pro_CompositionMaster.CreatedDate, tbl_Pro_CompositionMaster.ModifiedBy, tbl_Pro_CompositionMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region CompositionDetailRawMaster
        public async Task<object> GetCompositionDetailRawMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_RawMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionMaster_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Pro_CompositionDetail_RawDetail_Itemss.Count
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCompositionDetailRawMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionDetailRawMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_RawMasters
                                      .Where(w => w.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Pro_CompositionDetail_RawDetail_Itemss.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_RawMasters
                                        .Where(w => w.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Pro_CompositionDetail_RawDetail_Itemss.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionMaster_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName + " [" + o.tbl_Pro_CompositionFilterPolicyDetail.tbl_Inv_WareHouseMaster.WareHouseName + "]",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalItems = o.tbl_Pro_CompositionDetail_RawDetail_Itemss.Count()                          
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCompositionDetailRawMaster(tbl_Pro_CompositionDetail_RawMaster tbl_Pro_CompositionDetail_RawMaster, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_RawMaster.CreatedBy = userName;
                tbl_Pro_CompositionDetail_RawMaster.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionDetail_RawMasters.Add(tbl_Pro_CompositionDetail_RawMaster);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_RawMaster.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_RawMaster.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionDetail_RawMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionDetail_RawMasters.Remove(db.tbl_Pro_CompositionDetail_RawMasters.Find(tbl_Pro_CompositionDetail_RawMaster.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        

        #endregion

        #region CompositionDetailRawDetail
        public async Task<object> GetCompositionDetailRawDetail(int id)
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
        public object GetWCLCompositionDetailRawDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionDetailRawDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();
            pageddata.PageSize = 15;

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_RawDetail_Itemss
                                      .Where(w => w.FK_tbl_Pro_CompositionDetail_RawMaster_ID == MasterID)
                                      .Where(w =>
                                                  string.IsNullOrEmpty(FilterValueByText)
                                                  ||
                                                  FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                              )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_RawDetail_Itemss
                                        .Where(w => w.FK_tbl_Pro_CompositionDetail_RawMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

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
        public async Task<string> PostCompositionDetailRawDetail(tbl_Pro_CompositionDetail_RawDetail_Items tbl_Pro_CompositionDetail_RawDetail_Items, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_RawDetail_Items.CreatedBy = userName;
                tbl_Pro_CompositionDetail_RawDetail_Items.CreatedDate = DateTime.Now;
                tbl_Pro_CompositionDetail_RawDetail_Items.CustomeRate = 0;
                tbl_Pro_CompositionDetail_RawDetail_Items.PercentageOnRate = 0;
                db.tbl_Pro_CompositionDetail_RawDetail_Itemss.Add(tbl_Pro_CompositionDetail_RawDetail_Items);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_RawDetail_Items.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_RawDetail_Items.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionDetail_RawDetail_Items).State = EntityState.Modified;

                db.Entry(tbl_Pro_CompositionDetail_RawDetail_Items).Property(x => x.CustomeRate).IsModified = false;
                db.Entry(tbl_Pro_CompositionDetail_RawDetail_Items).Property(x => x.PercentageOnRate).IsModified = false;

                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionDetail_RawDetail_Itemss.Remove(db.tbl_Pro_CompositionDetail_RawDetail_Itemss.Find(tbl_Pro_CompositionDetail_RawDetail_Items.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

        #region CompositionDetailCouplingMaster
        public async Task<object> GetCompositionDetailCouplingMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_Couplings.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.BatchSize,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Pro_CompositionDetail_Coupling_PackagingMasters.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCompositionDetailCouplingMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionDetailCouplingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Couplings
                                      .Where(w => w.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Couplings
                                        .Where(w => w.FK_tbl_Pro_CompositionMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.BatchSize,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          NoOfPackagings = o.tbl_Pro_CompositionDetail_Coupling_PackagingMasters.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCompositionDetailCouplingMaster(tbl_Pro_CompositionDetail_Coupling tbl_Pro_CompositionDetail_Coupling, string operation = "", string userName = "")
        {
            if (tbl_Pro_CompositionDetail_Coupling.BatchSize <= 0)
                tbl_Pro_CompositionDetail_Coupling.BatchSize = 1;

            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_Coupling.CreatedBy = userName;
                tbl_Pro_CompositionDetail_Coupling.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionDetail_Couplings.Add(tbl_Pro_CompositionDetail_Coupling);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_Coupling.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_Coupling.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionDetail_Coupling).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionDetail_Couplings.Remove(db.tbl_Pro_CompositionDetail_Couplings.Find(tbl_Pro_CompositionDetail_Coupling.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

        #region CompositionDetailCouplingDetailPackagingMaster
        public async Task<object> GetCompositionDetailCouplingDetailPackagingMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "'s " + o.tbl_Inv_ProductRegistrationDetail_Primary.Description,
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
        public object GetWCLCompositionDetailCouplingDetailPackagingMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }, new { n = "by Ingredient Product Name", v = "byProductNameIngredient" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionDetailCouplingDetailPackagingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters
                                      .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byProductNameIngredient" && w.tbl_Pro_CompositionDetail_Coupling_PackagingDetails.Any(a => a.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters
                                        .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                   ||
                                                   FilterByText == "byProductNameIngredient" && w.tbl_Pro_CompositionDetail_Coupling_PackagingDetails.Any(a => a.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "'s " + o.tbl_Inv_ProductRegistrationDetail_Primary.Description,
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
        public async Task<string> PostCompositionDetailCouplingDetailPackagingMaster(tbl_Pro_CompositionDetail_Coupling_PackagingMaster tbl_Pro_CompositionDetail_Coupling_PackagingMaster, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster.CreatedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters.Add(tbl_Pro_CompositionDetail_Coupling_PackagingMaster);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters.Remove(db.tbl_Pro_CompositionDetail_Coupling_PackagingMasters.Find(tbl_Pro_CompositionDetail_Coupling_PackagingMaster.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

        #region CompositionDetailCouplingDetailPackagingDetailMaster
        public async Task<object> GetCompositionDetailCouplingDetailPackagingDetailMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCompositionDetailCouplingDetailPackagingDetailMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionDetailCouplingDetailPackagingDetailMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Coupling_PackagingDetails
                                      .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingDetails
                                        .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName + " [" + o.tbl_Pro_CompositionFilterPolicyDetail.tbl_Inv_WareHouseMaster.WareHouseName + "]",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCompositionDetailCouplingDetailPackagingDetailMaster(tbl_Pro_CompositionDetail_Coupling_PackagingDetail tbl_Pro_CompositionDetail_Coupling_PackagingDetail, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail.CreatedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionDetail_Coupling_PackagingDetails.Add(tbl_Pro_CompositionDetail_Coupling_PackagingDetail);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionDetail_Coupling_PackagingDetails.Remove(db.tbl_Pro_CompositionDetail_Coupling_PackagingDetails.Find(tbl_Pro_CompositionDetail_Coupling_PackagingDetail.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

        #region CompositionDetailCouplingDetailPackagingDetailDetail
        public async Task<object> GetCompositionDetailCouplingDetailPackagingDetailDetail(int id)
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
        public object GetWCLCompositionDetailCouplingDetailPackagingDetailDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCompositionDetailCouplingDetailPackagingDetailDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();
            pageddata.PageSize = 15;
            int NoOfRecords = await db.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss
                                      .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss
                                        .Where(w => w.FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

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
        public async Task<string> PostCompositionDetailCouplingDetailPackagingDetailDetail(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.CreatedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.CreatedDate = DateTime.Now;
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.CustomeRate = 0;
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.PercentageOnRate = 0;
                db.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Add(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items).State = EntityState.Modified;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items).Property(x => x.CustomeRate).IsModified = false;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items).Property(x => x.PercentageOnRate).IsModified = false;
                await db.SaveChangesAsync();

            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Remove(db.tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss.Find(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRProcess(tbl_Pro_CompositionMaster_ProcessBMR tbl_Pro_CompositionMaster_ProcessBMR, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionMaster_ProcessBMR.CreatedBy = userName;
                tbl_Pro_CompositionMaster_ProcessBMR.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionMaster_ProcessBMRs.Add(tbl_Pro_CompositionMaster_ProcessBMR);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionMaster_ProcessBMR.ModifiedBy = userName;
                tbl_Pro_CompositionMaster_ProcessBMR.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionMaster_ProcessBMR).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionMaster_ProcessBMRs.Remove(db.tbl_Pro_CompositionMaster_ProcessBMRs.Find(tbl_Pro_CompositionMaster_ProcessBMR.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBPRProcess(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.CreatedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.CreatedDate = DateTime.Now;
                db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs.Add(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.ModifiedBy = userName;
                tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs.Remove(db.tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPRs.Find(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

        #region Report     

        public List<ReportCallingModel> GetRLCompositionRawMaster()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Composition Raw",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLCouplingPackagingMaster()
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
            if (rn == "Composition Raw")
            {
                return await Task.Run(() => CompositionRawAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Composition Complete")
            {
                return await Task.Run(() => CompositionCompleteAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CompositionCompleteAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
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
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);
            //--------------------------------5 column Raw table ------------------------------//
            Table pdftableDetailRaw = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05),//SNo
                        (float)(PageSize.A4.GetWidth()*0.10),//FilterPolicy
                        (float)(PageSize.A4.GetWidth()*0.20),//WareHouse
                        (float)(PageSize.A4.GetWidth()*0.50),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.15)//Quantity      
                        }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableDetailRaw.AddCell(new Cell(1,5).Add(new Paragraph().Add("Raw Detail")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            
            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Formula Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            //--------------------------------5 column Packaging table ------------------------------//
            Table pdftableDetailPackaging = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05),//SNo
                        (float)(PageSize.A4.GetWidth()*0.10),//FilterPolicy
                        (float)(PageSize.A4.GetWidth()*0.20),//WareHouse
                        (float)(PageSize.A4.GetWidth()*0.50),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.15)//Quantity      
                        }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableDetailPackaging.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Packaging Detail")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Formula Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            //--------------------------------3 column Process table ------------------------------//
            Table pdftableDetailProcess = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05),//SNo
                        (float)(PageSize.A4.GetWidth()*0.40),//ProcedureName
                        (float)(PageSize.A4.GetWidth()*0.55)//ProductName    
                        }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableDetailProcess.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Production Process")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            pdftableDetailProcess.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailProcess.AddCell(new Cell().Add(new Paragraph().Add("Process Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetailProcess.AddCell(new Cell().Add(new Paragraph().Add("QC Sample Product")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Pro_Composition] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                int FK_tbl_Pro_CompositionMaster_ID = 0;
                //----------master
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        //----------Row 1
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No:").SetFontSize(5)).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date:").SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yyyy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Shelf Life:").SetFontSize(5)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ShelfLifeInMonths"].ToString() + " Month(s)" )).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch Size:").SetFontSize(5)).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + sqlReader["SemiMeasurementUnit"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        //----------Row 2
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Composition:").SetFontSize(5)).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1,3).Add(new Paragraph().Add(sqlReader["CompositionName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Product Name:").SetFontSize(5)).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["SemiProductName"].ToString()).SetFontSize(5)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        //----------Row 3
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Packaging:").SetFontSize(5)).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["PackagingName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Primary:").SetFontSize(5)).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PrimarySplitInto"].ToString() + " x " + sqlReader["PrimaryMeasurementUnit"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Secondary:").SetFontSize(5)).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["SecondarySplitInto"].ToString() + " x " +sqlReader["SecondaryMeasurementUnit"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //------------setting parameter for second query of Raw ingredients

                        FK_tbl_Pro_CompositionMaster_ID = (int)sqlReader["FK_tbl_Pro_CompositionMaster_ID"];
                        
                    }
                }

                //----------raw detail
                ReportName.Value = rn + "2";  MasterID.Value = FK_tbl_Pro_CompositionMaster_ID;
                int SNo = 1;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {   
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FilterName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString()).SetFontSize(5)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                    }
                }

                //----------Packaging detail
                ReportName.Value = rn + "3"; MasterID.Value = id;
                SNo = 1;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FilterName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString()).SetFontSize(5)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                    }
                }

                //----------Process detail
                ReportName.Value = rn + "4"; MasterID.Value = FK_tbl_Pro_CompositionMaster_ID;
                SNo = 1;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetailProcess.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailProcess.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProcedureName"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetailProcess.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() +"]").SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        SNo++;
                    }
                }

            }

            page.InsertContent(pdftableMaster);
            page.InsertContent(pdftableDetailRaw);
            page.InsertContent(pdftableDetailPackaging);
            page.InsertContent(pdftableDetailProcess);

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> CompositionRawAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + rn + "-----", true);

            //--------------------------------5 column table ------------------------------//
            Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.10),//DocNo
                        (float)(PageSize.A4.GetWidth()*0.10),//DocDate
                        (float)(PageSize.A4.GetWidth()*0.55),//CompositionName
                        (float)(PageSize.A4.GetWidth()*0.15),//Dimension
                        (float)(PageSize.A4.GetWidth()*0.10) //ShelfLifeInMonths
                        }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Composition Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Dimension")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Shelf Life")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            //--------------------------------5 column table ------------------------------//
            Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.05),//SNo
                        (float)(PageSize.A4.GetWidth()*0.10),//FilterPolicy
                        (float)(PageSize.A4.GetWidth()*0.20),//WareHouse
                        (float)(PageSize.A4.GetWidth()*0.50),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.15)//Quantity     
                        }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

     
            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Formula Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Pro_Composition] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                bool firstrow = true; int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {                   

                    while (sqlReader.Read())
                    {
                        if (firstrow)
                        {
                            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yyyy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CompositionName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DimensionValue"].ToString() + " " + sqlReader["DimensionUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ShelfLifeInMonths"].ToString() + " Month(s)")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            firstrow = false;

                            pdftableMaster.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Raw Material Ingredients")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        }

                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FilterName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                    }

                }

            }

            page.InsertContent(pdftableMaster);
            page.InsertContent(pdftableDetail);

            return page.FinishToGetBytes();
        }

        #endregion

    }
    public class BMRRepository : IBMR
    {
        private readonly OreasDbContext db;
        public BMRRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region BMRMaster
        public async Task<object> GetBMRMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
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
                          IsRawDispened =    o.tbl_Pro_BatchMaterialRequisitionDetail_RawMasters
                                              .Any(a=> a.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails
                                              .Any(b=> b.tbl_Inv_BMRDispensingRaws.Any(c=> c.ID > 0)))
                                            
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRMaster()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }, new { n = "by BatchNo", v = "byBatchNo" }, new { n = "by Raw ProductName", v = "byRawProductName" }, new { n = "by SemiFinished ProductName", v = "bySemiFinishedProductName" }
            }.ToList();
        }
        public object GetWCLBBMRMaster()
        {
            return new[]
          {
                new { n = "by Dispensing Pending", v = "byDispensingPending" }, new { n = "by Dispensing Completed", v = "byDispensingCompleted" },
                new { n = "by Finished", v = "byFinished" }, new { n = "by Partial Finished", v = "byPartialFinished" }, new { n = "by InProcess", v = "byInProcess" },
                new { n = "by Dispensing Pending R", v = "byDispensingPendingR" }, new { n = "by Dispensing Completed R", v = "byDispensingCompletedR" },
                new { n = "by Dispensing Pending P", v = "byDispensingPendingP" }, new { n = "by Dispensing Completed P", v = "byDispensingCompletedP" }

            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {


            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionMasters
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && w.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byRawProductName" && w.tbl_Pro_BatchMaterialRequisitionDetail_RawMasters.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
                                                       ||
                                                       FilterByText == "bySemiFinishedProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                       )
                                      .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byDispensingPending" && w.IsDispensedR == false && w.IsDispensedP == false
                                                ||
                                                FilterByLoad == "byDispensingCompleted" && w.IsDispensedR == true && w.IsDispensedP == true
                                                ||
                                                FilterByLoad == "byDispensingPendingR" && w.IsDispensedR == false 
                                                ||
                                                FilterByLoad == "byDispensingCompletedR" && w.IsDispensedR == true 
                                                ||
                                                FilterByLoad == "byDispensingPendingP" && w.IsDispensedP == false
                                                ||
                                                FilterByLoad == "byDispensingCompletedP" && w.IsDispensedP == true
                                                ||
                                                FilterByLoad == "byFinished" && w.IsCompleted == true
                                                ||
                                                FilterByLoad == "byPartialFinished" && w.IsCompleted == null
                                                ||
                                                FilterByLoad == "byInProcess" && w.IsCompleted == false
                                                )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMasters
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && w.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byRawProductName" && w.tbl_Pro_BatchMaterialRequisitionDetail_RawMasters.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
                                                       ||
                                                       FilterByText == "bySemiFinishedProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                       )
                                        .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byDispensingPending" && w.IsDispensedR == false && w.IsDispensedP == false
                                                ||
                                                FilterByLoad == "byDispensingCompleted" && w.IsDispensedR == true && w.IsDispensedP == true
                                                ||
                                                FilterByLoad == "byDispensingPendingR" && w.IsDispensedR == false
                                                ||
                                                FilterByLoad == "byDispensingCompletedR" && w.IsDispensedR == true
                                                ||
                                                FilterByLoad == "byDispensingPendingP" && w.IsDispensedP == false
                                                ||
                                                FilterByLoad == "byDispensingCompletedP" && w.IsDispensedP == true
                                                ||
                                                FilterByLoad == "byFinished" && w.IsCompleted == true
                                                ||
                                                FilterByLoad == "byPartialFinished" && w.IsCompleted == null
                                                ||
                                                FilterByLoad == "byInProcess" && w.IsCompleted == false
                                                )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
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
                          TotalPackageBatchSize = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters?.Sum(s => s.BatchSize) ?? 0,
                          BMRProcesses = o.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs.Count(),
                          BPRProcesses = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Sum(s => s.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs.Count())
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRMaster(tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster, string operation = "", string userName = "")
        {
            if (tbl_Pro_BatchMaterialRequisitionMaster.BatchSize <= 0)
                tbl_Pro_BatchMaterialRequisitionMaster.BatchSize = 1;

            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionMaster.CreatedBy = userName;
                tbl_Pro_BatchMaterialRequisitionMaster.CreatedDate = DateTime.Now;
                tbl_Pro_BatchMaterialRequisitionMaster.IsDispensedR = false;
                tbl_Pro_BatchMaterialRequisitionMaster.IsDispensedP = false;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionMaster.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionMaster.ModifiedDate = DateTime.Now;

                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Pro_BatchMaterialRequisitionMasters.Remove(db.tbl_Pro_BatchMaterialRequisitionMasters.Find(tbl_Pro_BatchMaterialRequisitionMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionMaster] 
                               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                              ,@ID={3},@DocNo={4},@DocDate={5},@BatchNo={6}
                              ,@BatchMfgDate={7},@BatchExpiryDate={8}
                              ,@DimensionValue={9},@FK_tbl_Inv_MeasurementUnit_ID_Dimension={10}
                              ,@FK_tbl_Inv_ProductRegistrationDetail_ID={11},@BatchSize={12}
                              ,@FK_tbl_Pro_CompositionDetail_Coupling_ID={13},@IsCompleted={14},@FinishedDate={15}
                              ,@CreatedBy={16},@CreatedDate={17},@ModifiedBy={18},@ModifiedDate={19}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Pro_BatchMaterialRequisitionMaster.ID, tbl_Pro_BatchMaterialRequisitionMaster.DocNo, tbl_Pro_BatchMaterialRequisitionMaster.DocDate, tbl_Pro_BatchMaterialRequisitionMaster.BatchNo,
               tbl_Pro_BatchMaterialRequisitionMaster.BatchMfgDate, tbl_Pro_BatchMaterialRequisitionMaster.BatchExpiryDate,
               tbl_Pro_BatchMaterialRequisitionMaster.DimensionValue, tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Inv_MeasurementUnit_ID_Dimension,
               tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Inv_ProductRegistrationDetail_ID, tbl_Pro_BatchMaterialRequisitionMaster.BatchSize,
               tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Pro_CompositionDetail_Coupling_ID, tbl_Pro_BatchMaterialRequisitionMaster.IsCompleted, tbl_Pro_BatchMaterialRequisitionMaster.FinishedDate,
               tbl_Pro_BatchMaterialRequisitionMaster.CreatedBy, tbl_Pro_BatchMaterialRequisitionMaster.CreatedDate,
               tbl_Pro_BatchMaterialRequisitionMaster.ModifiedBy, tbl_Pro_BatchMaterialRequisitionMaster.ModifiedDate);


            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region BMRDetailRawMaster
        public async Task<object> GetBMRDetailRawMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_RawMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Pro_CompositionDetail_RawMaster_ID,
                          TotalDetail = o.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails.Count
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDetailRawMaster()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDetailRawMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_RawMasters
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_RawMasters
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalItems = o.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRDetailRawMaster(tbl_Pro_BatchMaterialRequisitionDetail_RawMaster tbl_Pro_BatchMaterialRequisitionDetail_RawMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.CreatedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionDetail_RawMaster] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionMaster_ID={4}
              ,@FK_tbl_Pro_CompositionFilterPolicyDetail_ID={5},@FK_tbl_Inv_WareHouseMaster_ID={6}
              ,@CreatedBy={7},@CreatedDate={8},@ModifiedBy={8},@ModifiedDate={10},@FK_tbl_Pro_CompositionDetail_RawMaster_ID={11}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.ID, tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Pro_CompositionFilterPolicyDetail_ID, tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Inv_WareHouseMaster_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.CreatedBy, tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.CreatedDate,
            tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.ModifiedBy, tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.ModifiedDate, tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Pro_CompositionDetail_RawMaster_ID);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region BMRDetailRawDetailItem
        public async Task<object> GetBMRDetailRawDetailItem(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDispensed = o?.tbl_Inv_BMRDispensingRaws?.Sum(s=> s.Quantity) ?? 0
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDetailRawDetailItem()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDetailRawDetailItem(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                  )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          DispensingQty = o.tbl_Inv_BMRDispensingRaws.Where(w => w.DispensingDate.HasValue).Sum(s => s.Quantity),
                          ReservedQty = o.tbl_Inv_BMRDispensingRaws.Where(w => !w.DispensingDate.HasValue).Sum(s => s.Quantity),
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRDetailRawDetailItem(tbl_Pro_BatchMaterialRequisitionDetail_RawDetail tbl_Pro_BatchMaterialRequisitionDetail_RawDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.CreatedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionDetail_RawDetail] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID={4}
              ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
              ,@Quantity={6},@Remarks={7}
              ,@CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.ID, tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.Quantity, tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.Remarks, 
            tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.CreatedBy, tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.CreatedDate, tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.ModifiedBy, tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region BMRDetailPackagingMaster
        public async Task<object> GetBMRDetailPackagingMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          o.PackagingName,
                          o.BatchSize,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "'s " + o.tbl_Inv_ProductRegistrationDetail_Primary.Description,
                          o.Cost_Primary,
                          o.TotalProd_Primary,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary.HasValue ? " [" + o.tbl_Inv_ProductRegistrationDetail_Secondary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Secondary.Split_Into.ToString() + " " + o.tbl_Inv_ProductRegistrationDetail_Secondary.Description : "",
                          o.Cost_Secondary,
                          o.TotalProd_Secondary,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          o.FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID,
                          SecondaryUsedINPT= o.tbl_Pro_ProductionTransferDetails_RefNo.Any(a=> a.FK_tbl_Inv_ProductRegistrationDetail_ID == o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary),
                          IsPackagingDispened = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails
                                                .Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss
                                                .Any(b => b.tbl_Inv_BMRDispensingPackagings.Any(d => d.ID > 0)))
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDetailPackagingMaster()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }, new { n = "by Ingredient ProductName", v = "byProductNameIngredient" }
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
                                                 ||
                                                 FilterByText == "byProductNameIngredient" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
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
                                                       ||
                                                       FilterByText == "byProductNameIngredient" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
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
                          FK_tbl_Inv_ProductRegistrationDetail_ID_PrimaryName = " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Primary.Split_Into.ToString() + "'s {SMRP: " + o.tbl_Inv_ProductRegistrationDetail_Primary.StandardMRP + "}",
                          o.tbl_Inv_ProductRegistrationDetail_Primary.GTINCode,
                          o.Cost_Primary,
                          o.TotalProd_Primary,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary,
                          FK_tbl_Inv_ProductRegistrationDetail_ID_SecondaryName = o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary.HasValue ? " [" + o.tbl_Inv_ProductRegistrationDetail_Secondary.tbl_Inv_MeasurementUnit.MeasurementUnit + "] x " + o.tbl_Inv_ProductRegistrationDetail_Secondary.Split_Into.ToString() + " " + o.tbl_Inv_ProductRegistrationDetail_Secondary.Description : "",
                          o.Cost_Secondary,
                          o.TotalProd_Secondary,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          o.FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID,
                          NoOfItems = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails.Count(),
                          OrderDetail = o.FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID > 0 ? "[O#: " + o.tbl_Inv_OrderNoteDetail_ProductionOrder.tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster.DocNo.ToString() + "] [Customer: " + o.tbl_Inv_OrderNoteDetail_ProductionOrder.tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster.tbl_Ac_ChartOfAccounts.AccountName + "] [MRP:" + o.tbl_Inv_OrderNoteDetail_ProductionOrder.tbl_Inv_OrderNoteDetail.CustomMRP.ToString() + "]" : "",
                          BPRProcesses = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRDetailPackagingMaster(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster, string operation = "", string userName = "")
        {
            if (tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.BatchSize <= 0)
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.BatchSize = 1;

            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 250, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.CreatedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.CreatedDate = DateTime.Now;
                //db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Add(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Remove(db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Find(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }


            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionDetail_PackagingMaster] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionMaster_ID={4}
                  ,@PackagingName={5},@BatchSize={6}
                  ,@FK_tbl_Inv_ProductRegistrationDetail_ID_Primary={7}
                  ,@FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary={8}
                  ,@CreatedBy={9},@CreatedDate={10},@ModifiedBy={11},@ModifiedDate={12}
                  ,@FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID={13},@FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID={14}",
                CRUD_Type.Value, CRUD_Msg, CRUD_ID,
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.ID, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.PackagingName, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.BatchSize,
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Primary, 
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary,
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.CreatedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.CreatedDate, 
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.ModifiedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.ModifiedDate,
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID);


            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region BMRDetailPackagingDetailFilter
        public async Task<object> GetBMRDetailPackagingDetailFilter(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID,
                          TotalDetail = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDetailPackagingDetailFilter()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDetailPackagingDetailFilter(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""                          
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRDetailPackagingDetailFilter(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.CreatedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionDetail_PackagingDetail] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID={4}
              ,@FK_tbl_Pro_CompositionFilterPolicyDetail_ID={5},@FK_tbl_Inv_WareHouseMaster_ID={6}
              ,@CreatedBy={7},@CreatedDate={8},@ModifiedBy={8},@ModifiedDate={10},@FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID={11}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.ID, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.FK_tbl_Pro_CompositionFilterPolicyDetail_ID, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.FK_tbl_Inv_WareHouseMaster_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.CreatedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.CreatedDate,
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.ModifiedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.ModifiedDate, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region BMRDetailPackagingDetailFilterDetailItem
        public async Task<object> GetBMRDetailPackagingDetailFilterDetailItem(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDispensed = o?.tbl_Inv_BMRDispensingPackagings?.Sum(s => s.Quantity) ?? 0
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDetailPackagingDetailFilterDetailItem()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDetailPackagingDetailFilterDetailItem(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          DispensingQty = o.tbl_Inv_BMRDispensingPackagings.Where(w => w.DispensingDate.HasValue).Sum(s => s.Quantity),
                          ReservedQty = o.tbl_Inv_BMRDispensingPackagings.Where(w => !w.DispensingDate.HasValue).Sum(s => s.Quantity),
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""

                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRDetailPackagingDetailFilterDetailItem(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.CreatedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID={4}
              ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
              ,@Quantity={6},@Remarks={7}
              ,@CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.ID, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.FK_tbl_Inv_ProductRegistrationDetail_ID,
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.Quantity, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.Remarks,
            tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.CreatedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.CreatedDate, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.ModifiedBy, tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Auto Reservation

        public async Task<string> PostStockIssuanceReservation(int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, bool IssueTrue_ReserveFalse = true, string userName = "")
        {
            SqlParameter ResponseMsg = new SqlParameter("@ResponseMsg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = -1, Value = "" };

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[USP_Pro_StockIssuedOrReservation] 
              @ResponseMsg={0} OUTPUT,@BMR_RawItemID={1},@BMR_PackagingItemID={2}
             ,@BMR_AdditionalItemID={3},@OR_ItemID={4}
             ,@IssueTrue_ReserveFalse={5},@UserName={6},@EntryTime={7}",
             ResponseMsg, BMR_RawItemID, BMR_PackagingItemID,
             BMR_AdditionalItemID, OR_ItemID,
             IssueTrue_ReserveFalse, userName, null);

            return (string)ResponseMsg.Value;

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
                                      .Where(w=> w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
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
                          CompletedDate = o.CompletedDate.HasValue ? o.CompletedDate.Value.ToString("dd-MM-yyyy") : "",
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

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.CreatedBy = userName;
                tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

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
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProcedureName" && w.tbl_Pro_Procedure.ProcedureName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

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

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBPRProcess(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.CreatedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

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
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "BMR Complete")
            {
                return await Task.Run(() => BMRCompleteAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
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
            //--------------------------------4 column Raw table ------------------------------//
            Table pdftableDetailRaw = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.10),//SNo
                        (float)(PageSize.A4.GetWidth()*0.50),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.15),//Quantity
                        (float)(PageSize.A4.GetWidth()*0.25) //ReferenceNo     
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            //--------------------------------5 column Packaging table ------------------------------//
            Table pdftableDetailPackaging = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.10),//SNo
                        (float)(PageSize.A4.GetWidth()*0.50),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.15),//Quantity
                        (float)(PageSize.A4.GetWidth()*0.25) //ReferenceNo     
                        }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);
            string CreatedBy = "";
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Pro_BMR] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
       
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date:")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yyyy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                        
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Mfg Date:")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["BatchMfgDate"]).ToString("dd-MMM-yyyy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Expiry Date:")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["BatchExpiryDate"]).ToString("dd-MMM-yyyy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Dimension:")).SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DimensionValue"].ToString() + " " + sqlReader["DimensionMeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        //----------Row 2
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Product Name:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch No:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString()).SetBold()).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch Size:")).SetBold().SetMaxWidth(0.05f).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + sqlReader["MeasurementUnit"].ToString()).SetBold()).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        BatchSizeUnit = sqlReader["MeasurementUnit"].ToString();
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }

                int SNo = 1; string FilterName = "";
                //----------raw detail
                if (string.IsNullOrEmpty(SeekBy) || SeekBy == "Raw")
                {
                    pdftableDetailRaw.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                    pdftableDetailRaw.AddCell(new Cell(1, 4).Add(new Paragraph().Add(new Link("Raw Detail", PdfAction.CreateURI(uri + "?rn=" + rn + "&id=" + id + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=Raw" + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + GroupID)))).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    ReportName.Value = rn + "2";

                    using (DbDataReader sqlReader = command.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            if (FilterName != sqlReader["FilterName"].ToString())
                            {
                                FilterName = sqlReader["FilterName"].ToString();

                                pdftableDetailRaw.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["FilterName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                SNo = 1;
                            }
                            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailRaw.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceList"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                            SNo++;
                        }
                    }
                }                              

                //----------Packaging detail                
                if (string.IsNullOrEmpty(SeekBy) || SeekBy == "Packaging")
                {
                    ReportName.Value = rn + "3";
                    SNo = 1; FilterName = ""; int BMRDPM = 0;

                    pdftableDetailPackaging.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                    pdftableDetailPackaging.AddCell(new Cell(1, 4).Add(new Paragraph().Add(new Link("Packaging Detail", PdfAction.CreateURI(uri + "?rn=" + rn + "&id=" + id + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=Packaging" + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + GroupID)))).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(8).SetBackgroundColor(new DeviceRgb(156, 166, 181)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                    using (DbDataReader sqlReader = command.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            if (BMRDPM != (int)sqlReader["ID"])
                            {
                                BMRDPM = (int)sqlReader["ID"];
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Packaging").SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PackagingName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PrimarySplit_Into"].ToString() + " x " + sqlReader["PrimaryMeasurementUnit"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + BatchSizeUnit).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("GTIN #").SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["GTINCode"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Custom MRP").SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CustomMRP"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(130, 120, 100)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            }

                            if (FilterName != sqlReader["FilterName"].ToString())
                            {
                                FilterName = sqlReader["FilterName"].ToString();

                                pdftableDetailPackaging.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["FilterName"].ToString()).SetBold().SetFontSize(7)).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(218, 226, 240)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Ingredients Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                SNo = 1;
                            }
                            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableDetailPackaging.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceList"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                            SNo++;
                        }
                    }
                }
               
            }

            page.InsertContent(pdftableMaster);           
            page.InsertContent(pdftableDetailRaw);
            page.InsertContent(pdftableDetailPackaging);

            /////////////------------------------------Signature Footer table------------------------------////////////////
            Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Created By: "+ CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Issued By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Received By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


            pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Officer QC")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Manager QC")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Officer Production")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("\n\n\n_______________________\n" + "Manager Production")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


            page.InsertContent(pdftableSignature);

            return page.FinishToGetBytes();
        }

        #endregion

    }
    public class BMRBPRProcessRepository : IBMRBPRProcess
    {
        private readonly OreasDbContext db;
        public BMRBPRProcessRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetBMRBPRMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""

                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRBPRMaster()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }, new { n = "by Batch No", v = "byBatchNo" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBBMRBPRMaster()
        {
            return new[]
            {
                new { n = "by Finished", v = "byFinished" }, new { n = "by Partial Finished", v = "byPartialFinished" }, new { n = "by InProcess", v = "byInProcess" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRBPRMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionMasters
                                               .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
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
                                                       ||
                                                       FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMasters
                                  .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
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
                          TotalPackageBatchSize = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters?.Sum(s => s.BatchSize) ?? 0,
                          BMRProcesses = o.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs.Count(),
                          BPRProcesses = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Sum(s => s.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs.Count())
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRBPRMaster(tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BatchMaterialRequisitionMaster.FinishedDate = DateTime.Now;

                CRUD_Type.Value = "UpdateByProcess";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BatchMaterialRequisitionMaster.FinishedDate = null;

                tbl_Pro_BatchMaterialRequisitionMaster.ModifiedBy = userName;
                tbl_Pro_BatchMaterialRequisitionMaster.ModifiedDate = DateTime.Now;

                CRUD_Type.Value = "UpdateByProcess";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BatchMaterialRequisitionMaster] 
                               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                              ,@ID={3},@DocNo={4},@DocDate={5},@BatchNo={6}
                              ,@BatchMfgDate={7},@BatchExpiryDate={8}
                              ,@DimensionValue={9},@FK_tbl_Inv_MeasurementUnit_ID_Dimension={10}
                              ,@FK_tbl_Inv_ProductRegistrationDetail_ID={11},@BatchSize={12}
                              ,@FK_tbl_Pro_CompositionDetail_Coupling_ID={13},@IsCompleted={14},@FinishedDate={15}
                              ,@CreatedBy={16},@CreatedDate={17},@ModifiedBy={18},@ModifiedDate={19}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Pro_BatchMaterialRequisitionMaster.ID, tbl_Pro_BatchMaterialRequisitionMaster.DocNo, tbl_Pro_BatchMaterialRequisitionMaster.DocDate, tbl_Pro_BatchMaterialRequisitionMaster.BatchNo,
               tbl_Pro_BatchMaterialRequisitionMaster.BatchMfgDate, tbl_Pro_BatchMaterialRequisitionMaster.BatchExpiryDate,
               tbl_Pro_BatchMaterialRequisitionMaster.DimensionValue, tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Inv_MeasurementUnit_ID_Dimension,
               tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Inv_ProductRegistrationDetail_ID, tbl_Pro_BatchMaterialRequisitionMaster.BatchSize,
               tbl_Pro_BatchMaterialRequisitionMaster.FK_tbl_Pro_CompositionDetail_Coupling_ID, tbl_Pro_BatchMaterialRequisitionMaster.IsCompleted, tbl_Pro_BatchMaterialRequisitionMaster.FinishedDate,
               tbl_Pro_BatchMaterialRequisitionMaster.CreatedBy, tbl_Pro_BatchMaterialRequisitionMaster.CreatedDate,
               tbl_Pro_BatchMaterialRequisitionMaster.ModifiedBy, tbl_Pro_BatchMaterialRequisitionMaster.ModifiedDate);


            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
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
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
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
                          SampleStatus = o?.tbl_Qc_SampleProcessBMRs?.LastOrDefault()?.FK_tbl_Qc_ActionType_ID ?? 0,
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

            if (operation == "Save New")
            {
                CRUD_Type.Value = "UpdateByProcess";
            }
            else if (operation == "Save Update")
            {

                CRUD_Type.Value = "UpdateByProcess";
            }

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

        #region BMRSample
        public async Task<object> GetBMRSample(int id)
        {
            var qry = from o in await db.tbl_Qc_SampleProcessBMRs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID,
                          SampleDate = o.SampleDate.ToString("dd-MMM-yyyy"),
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

            int NoOfRecords = await db.tbl_Qc_SampleProcessBMRs
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAction" && w.tbl_Qc_ActionType.ActionName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_SampleProcessBMRs
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAction" && w.tbl_Qc_ActionType.ActionName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID,
                          SampleDate = o.SampleDate.ToString("dd-MMM-yyyy"),
                          o.SampleQty,
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.ActionBy,
                          ActionDate = o.ActionDate.HasValue ? o.ActionDate.Value.ToString("dd-MMM-yyyy") : "",
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRSample(tbl_Qc_SampleProcessBMR tbl_Qc_SampleProcessBMR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Qc_SampleProcessBMR.CreatedBy = userName;
                tbl_Qc_SampleProcessBMR.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Qc_SampleProcessBMR.ModifiedBy = userName;
                tbl_Qc_SampleProcessBMR.ModifiedDate = DateTime.Now;

                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

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
                                      .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
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
                          SampleStatus = o?.tbl_Qc_SampleProcessBPRs?.LastOrDefault()?.FK_tbl_Qc_ActionType_ID ?? 0,
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

            if (operation == "Save New")
            {
                CRUD_Type.Value = "UpdateByProcess";
            }
            else if (operation == "Save Update")
            {

                CRUD_Type.Value = "UpdateByProcess";
            }

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

        #region BMRSample
        public async Task<object> GetBPRSample(int id)
        {
            var qry = from o in await db.tbl_Qc_SampleProcessBPRs.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID,
                          SampleDate = o.SampleDate.ToString("dd-MMM-yyyy"),
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

            int NoOfRecords = await db.tbl_Qc_SampleProcessBPRs
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID == MasterID)
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAction" && w.tbl_Qc_ActionType.ActionName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Qc_SampleProcessBPRs
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAction" && w.tbl_Qc_ActionType.ActionName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID,
                          SampleDate = o.SampleDate.ToString("dd-MMM-yyyy"),
                          o.SampleQty,
                          o.FK_tbl_Qc_ActionType_ID,
                          FK_tbl_Qc_ActionType_IDName = o.tbl_Qc_ActionType.ActionName,
                          o.ActionBy,
                          ActionDate = o.ActionDate.HasValue ? o.ActionDate.Value.ToString("dd-MMM-yyyy") : "",
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBPRSample(tbl_Qc_SampleProcessBPR tbl_Qc_SampleProcessBPR, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Qc_SampleProcessBPR.CreatedBy = userName;
                tbl_Qc_SampleProcessBPR.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Qc_SampleProcessBPR.ModifiedBy = userName;
                tbl_Qc_SampleProcessBPR.ModifiedDate = DateTime.Now;

                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

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

        #region Report     
        public List<ReportCallingModel> GetRLMaster()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register BMR",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = new List<string>(){ "In-process", "Partial-Finished", "Finished" }
                }
            };
        }
        public List<ReportCallingModel> GetRLDetail()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Status",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Register BMR")
            {
                return await Task.Run(() => RegisterBMR(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "BMR Status")
            {
                return await Task.Run(() => BMRStatus(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }     
        private async Task<byte[]> RegisterBMR(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- " + rn + " " + SeekBy + " From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            var LightGreen = new MyDeviceRgb(MyColor.LightGreen).color;
            var LightPink = new MyDeviceRgb(MyColor.LightPink).color;
            var LightSteelBlue = new MyDeviceRgb(MyColor.LightSteelBlue).color;
            var SteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;

            /////////////------------------------------table for Detail 11------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.06),//S No
                        (float)(PageSize.A4.GetWidth() * 0.07),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.06),//DocDate
                        (float)(PageSize.A4.GetWidth() * 0.07),//Batch No 
                        (float)(PageSize.A4.GetWidth() * 0.30),//ProductName 
                        (float)(PageSize.A4.GetWidth() * 0.05),//Unit 
                        (float)(PageSize.A4.GetWidth() * 0.10),//Batch Size 
                        (float)(PageSize.A4.GetWidth() * 0.10),//Total Production
                        (float)(PageSize.A4.GetWidth() * 0.09),//RawDisp / Packaging Disp 
                        (float)(PageSize.A4.GetWidth() * 0.09),// BMR Process / BPR Process
                        (float)(PageSize.A4.GetWidth() * 0.05) // Yeild
                }
            ).SetFontSize(7).UseAllAvailableWidth().SetFixedLayout().SetBorder(Border.NO_BORDER);


            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Batch#")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Batch Size")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Production")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Full Dispensing"+ "\n" +"[BMR/BPR]")).SetTextAlignment(TextAlignment.CENTER).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Incomplete Process" + "\n" + "[BMR/BPR]")).SetTextAlignment(TextAlignment.CENTER).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Yeild")).SetBold());


            int SNo = 1;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Pro_BMR] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                double yeild = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        yeild = 0;
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(new Link(sqlReader["BatchNo"].ToString(), PdfAction.CreateURI(uri + "?rn=BMR Status" + "&id=" + sqlReader["ID"].ToString() + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=Raw" + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + GroupID)))).SetBold().SetFontSize(8).SetFontColor(SteelBlue).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(new Link(sqlReader["ProductName"].ToString(), PdfAction.CreateURI(uri + "?rn=BMR Status" + "&id=" + sqlReader["ID"].ToString() + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=Raw" + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + GroupID)))).SetBold().SetFontSize(8).SetFontColor(SteelBlue).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProdUnit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchSize"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TotalProd"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["IsDispensedR"].ToString() + " / " + sqlReader["IsDispensedP"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["InCompleteBMRProcesses"].ToString() + " / " + sqlReader["InCompleteBPRProcesses"].ToString())).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                       
                        yeild = Math.Round(((Convert.ToDouble(sqlReader["TotalProd"]) / Convert.ToDouble(sqlReader["BatchSize"]))*100),2);
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(yeild.ToString()+"%")).SetBackgroundColor(yeild >= 80 ? LightGreen : yeild >= 50 ? LightSteelBlue : LightPink).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                        SNo++;
                    }
                }
            }

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> BMRStatus(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- " + rn + "-----", true);
            var ColorSteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorGray = new MyDeviceRgb(MyColor.Gray).color;
            var ColorLightSteelBlue = new MyDeviceRgb(MyColor.LightSteelBlue).color;
            var ColorLightPink = new MyDeviceRgb(MyColor.LightPink).color;


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Pro_BMR] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                /////////////------------------------------Main Table------------------------------////////////////
                Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.10),//DocDate
                        (float)(PageSize.A4.GetWidth() * 0.10),//BatchNo 
                        (float)(PageSize.A4.GetWidth() * 0.40),//ProductName 
                        (float)(PageSize.A4.GetWidth() * 0.20),//BatchSize 
                        (float)(PageSize.A4.GetWidth() * 0.10) //IsCompleted
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("BMR Detail")).SetBold().SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Batch No")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Product")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Batch Size")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Status")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());

                string BatchSizeUnit = "";
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchStatus"].ToString())).SetKeepTogether(true));
                        BatchSizeUnit = sqlReader["MeasurementUnit"].ToString();
                    }
                }

                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("BPR Detail")).SetBold().SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Packaging Name")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Primary Unit Detail")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());
                pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Packaging BatchSize")).SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBold());

                ReportName.Value = rn + "2";
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {                        
                        pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add(sqlReader["PackagingName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Unit: " + sqlReader["MeasurementUnit"].ToString() + " PackSize: " + sqlReader["Split_Into"].ToString() + "'s")).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + BatchSizeUnit)).SetKeepTogether(true));
                    }
                }
                page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));

                /////////////------------------------------Dispening Table------------------------------////////////////
                Table pdftableDispensing = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10),//FilterName
                        (float)(PageSize.A4.GetWidth() * 0.20),//WareHouseName
                        (float)(PageSize.A4.GetWidth() * 0.40),//ProductName MeasurementUnit
                        (float)(PageSize.A4.GetWidth() * 0.15),//RequestedQty  
                        (float)(PageSize.A4.GetWidth() * 0.15) //DispensedQty  
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDispensing.AddCell(new Cell(1, 5).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDispensing.AddCell(new Cell(1, 5).Add(new Paragraph().Add("BMR Material Dispensing Status")).SetBold().SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("Type")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("Ingredient Name With Unit")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("Requested Qty")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("Dispensed Qty")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());

                ReportName.Value = rn + "3";
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FilterName"].ToString())).SetKeepTogether(true));
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]")).SetKeepTogether(true));
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequestedQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DispensedQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(Convert.ToDouble(sqlReader["RequestedQty"]) > Convert.ToDouble(sqlReader["DispensedQty"]) ? ColorLightPink : ColorWhite).SetKeepTogether(true));
                    }
                    
                }
                pdftableDispensing.AddCell(new Cell(1, 5).Add(new Paragraph().Add("BPR Material Dispensing Status")).SetBold().SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("Type")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("Ingredient Name With Unit")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("Requested Qty")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add("Dispensed Qty")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());

                ReportName.Value = rn + "4";
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["FilterName"].ToString())).SetKeepTogether(true));
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]")).SetKeepTogether(true));
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequestedQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableDispensing.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DispensedQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(Convert.ToDouble(sqlReader["RequestedQty"]) > Convert.ToDouble(sqlReader["DispensedQty"]) ? ColorLightPink : ColorWhite).SetKeepTogether(true));
                    }
                }
                page.InsertContent(new Cell().Add(pdftableDispensing).SetBorder(Border.NO_BORDER));

                /////////////------------------------------Dispening Table------------------------------////////////////
                Table pdftableProd = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.10),//DocDate
                        (float)(PageSize.A4.GetWidth() * 0.20),//WareHouseName
                        (float)(PageSize.A4.GetWidth() * 0.30),//ProductName  
                        (float)(PageSize.A4.GetWidth() * 0.10), //Quantity  MeasurementUnit
                        (float)(PageSize.A4.GetWidth() * 0.09),//QACleared   
                        (float)(PageSize.A4.GetWidth() * 0.11) //Received    
                }
                ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableProd.AddCell(new Cell(1, 7).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableProd.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Production Transfer Status")).SetBold().SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                pdftableProd.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBackgroundColor(ColorLightSteelBlue).SetKeepTogether(true));
                pdftableProd.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBackgroundColor(ColorLightSteelBlue).SetBold());
                pdftableProd.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBackgroundColor(ColorLightSteelBlue).SetBold());
                pdftableProd.AddCell(new Cell().Add(new Paragraph().Add("Product Detail")).SetBackgroundColor(ColorLightSteelBlue).SetBold());
                pdftableProd.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBackgroundColor(ColorLightSteelBlue).SetBold());
                pdftableProd.AddCell(new Cell().Add(new Paragraph().Add("QA Cleared")).SetBackgroundColor(ColorLightSteelBlue).SetBold());
                pdftableProd.AddCell(new Cell().Add(new Paragraph().Add("Store Received")).SetBackgroundColor(ColorLightSteelBlue).SetBold());

                ReportName.Value = rn + "5";
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableProd.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableProd.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableProd.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableProd.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                        pdftableProd.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + "\n[" + sqlReader["MeasurementUnit"].ToString() + "]")).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableProd.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QACleared"].ToString() + "\n" + (sqlReader.IsDBNull("QAClearedDate") ? "" : ((DateTime)sqlReader["QAClearedDate"]).ToString("dd-MM-yy")))).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(sqlReader["QACleared"].ToString() == "NO" ? ColorLightPink : ColorWhite).SetKeepTogether(true));
                        pdftableProd.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Received"].ToString() + "\n" + (sqlReader.IsDBNull("ReceivedDate") ? "" : ((DateTime)sqlReader["ReceivedDate"]).ToString("dd-MM-yy")))).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(sqlReader["Received"].ToString() == "NO" ? ColorLightPink : ColorWhite).SetKeepTogether(true));
                    }
                }
                page.InsertContent(new Cell().Add(pdftableProd).SetBorder(Border.NO_BORDER));
            }


            return page.FinishToGetBytes();
        }
        #endregion

    }
    public class BMRAdditionalRepository : IBMRAdditional
    {
        private readonly OreasDbContext db;
        public BMRAdditionalRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        public async Task<object> GetBatchNoList(string FilterBy = "BatchNo", string FilterValue = "")
        {
            
            return await (from a in db.tbl_Pro_BatchMaterialRequisitionMasters
                                       .Where(w =>
                                                    string.IsNullOrEmpty(FilterValue)
                                                    ||
                                                    w.BatchNo.ToLower().Contains(FilterValue.ToLower())
                                                    ).OrderByDescending(o => o.BatchMfgDate)
                          select new
                          {
                              a.ID,
                              a.BatchNo,
                              a.BatchSize,
                              BatchMfgDate = a.BatchMfgDate.ToString("dd-MMM-yy")
                          }).Take(10).ToListAsync();
        }

        #region BMRAdditionalMaster
        public async Task<object> GetBMRAdditionalMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_BMRAdditionalMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy"),
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          FK_tbl_Pro_BatchMaterialRequisitionMaster_IDName = o.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Pro_BMRAdditionalDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRAdditionalMaster()
        {
            return new[]
            {
                new { n = "by BatchNo", v = "byBatchNo" }, new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBBMRAdditionalMaster()
        {
            return new[]
            {
                new { n = "by Dispensing Pending", v = "byDispensingPending" }, new { n = "by Dispensing Completed", v = "byDispensingCompleted" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRAdditionalMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BMRAdditionalMasters
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && (w.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_BMRAdditionalDetails.Any(a=> a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                      .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byDispensingPending" && w.IsDispensedAll == false
                                                ||
                                                FilterByLoad == "byDispensingCompleted" && w.IsDispensedAll == true
                                                )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BMRAdditionalMasters
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && (w.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_BMRAdditionalDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                        .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byDispensingPending" && w.IsDispensedAll == false
                                                ||
                                                FilterByLoad == "byDispensingCompleted" && w.IsDispensedAll == true
                                                )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy"),
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          FK_tbl_Pro_BatchMaterialRequisitionMaster_IDName = o.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalItems = o.tbl_Pro_BMRAdditionalDetails.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRAdditionalMaster(tbl_Pro_BMRAdditionalMaster tbl_Pro_BMRAdditionalMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BMRAdditionalMaster.CreatedBy = userName;
                tbl_Pro_BMRAdditionalMaster.CreatedDate = DateTime.Now;
                tbl_Pro_BMRAdditionalMaster.IsDispensedAll = false;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BMRAdditionalMaster.ModifiedBy = userName;
                tbl_Pro_BMRAdditionalMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BMRAdditionalMaster] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@DocNo={4},@DocDate={5}
                  ,@FK_tbl_Pro_BatchMaterialRequisitionMaster_ID={6},@FK_tbl_Inv_WareHouseMaster_ID={7}
                  ,@CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Pro_BMRAdditionalMaster.ID, tbl_Pro_BMRAdditionalMaster.DocNo, tbl_Pro_BMRAdditionalMaster.DocDate,
               tbl_Pro_BMRAdditionalMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID, tbl_Pro_BMRAdditionalMaster.FK_tbl_Inv_WareHouseMaster_ID,
               tbl_Pro_BMRAdditionalMaster.CreatedBy, tbl_Pro_BMRAdditionalMaster.CreatedDate,
               tbl_Pro_BMRAdditionalMaster.ModifiedBy, tbl_Pro_BMRAdditionalMaster.ModifiedDate);


            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }
        #endregion

        #region BMRAdditionalDetail
        public async Task<object> GetBMRAdditionalDetail(int id)
        {
            var qry = from o in await db.tbl_Pro_BMRAdditionalDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BMRAdditionalMaster_ID,
                          o.FK_tbl_Pro_BMRAdditionalType_ID,
                          FK_tbl_Pro_BMRAdditionalType_IDName = o.tbl_Pro_BMRAdditionalType.TypeName,
                          o.RequiredTrue_ReturnFalse,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          o.IsDispensed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRAdditionalDetail()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRAdditionalDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BMRAdditionalDetails
                                      .Where(w => w.FK_tbl_Pro_BMRAdditionalMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BMRAdditionalDetails
                                        .Where(w => w.FK_tbl_Pro_BMRAdditionalMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BMRAdditionalMaster_ID,
                          o.FK_tbl_Pro_BMRAdditionalType_ID,
                          FK_tbl_Pro_BMRAdditionalType_IDName = o.tbl_Pro_BMRAdditionalType.TypeName,
                          o.RequiredTrue_ReturnFalse,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
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
        public async Task<string> PostBMRAdditionalDetail(tbl_Pro_BMRAdditionalDetail tbl_Pro_BMRAdditionalDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_BMRAdditionalDetail.CreatedBy = userName;
                tbl_Pro_BMRAdditionalDetail.CreatedDate = DateTime.Now;
                tbl_Pro_BMRAdditionalDetail.IsDispensed = false;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_BMRAdditionalDetail.ModifiedBy = userName;
                tbl_Pro_BMRAdditionalDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_BMRAdditionalDetail] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Pro_BMRAdditionalMaster_ID={4},@FK_tbl_Pro_BMRAdditionalType_ID={5}
                  ,@RequiredTrue_ReturnFalse={6},@FK_tbl_Inv_ProductRegistrationDetail_ID={7}
                  ,@Quantity={8},@Remarks={9},@IsDispensed={10}
                  ,@CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
               CRUD_Type, CRUD_Msg, CRUD_ID,
               tbl_Pro_BMRAdditionalDetail.ID, tbl_Pro_BMRAdditionalDetail.FK_tbl_Pro_BMRAdditionalMaster_ID, tbl_Pro_BMRAdditionalDetail.FK_tbl_Pro_BMRAdditionalType_ID,
               tbl_Pro_BMRAdditionalDetail.RequiredTrue_ReturnFalse, tbl_Pro_BMRAdditionalDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
               tbl_Pro_BMRAdditionalDetail.Quantity, tbl_Pro_BMRAdditionalDetail.Remarks, tbl_Pro_BMRAdditionalDetail.IsDispensed,
               tbl_Pro_BMRAdditionalDetail.CreatedBy, tbl_Pro_BMRAdditionalDetail.CreatedDate,
               tbl_Pro_BMRAdditionalDetail.ModifiedBy, tbl_Pro_BMRAdditionalDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLBMRAdditional()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current BMR Additional",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register BMR Additional",
                    GroupBy = new List<string>(){ "WareHouse", "Product" },
                    OrderBy = new List<string>(){ "Doc Date", "Doc No" },
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current BMR Additional")
            {
                return await Task.Run(() => CurrentBMRAdditional(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register BMR Additional")
            {
                return await Task.Run(() => RegisterBMRAdditional(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentBMRAdditional(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- BMR Additional -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //Doc No
                        (float)(PageSize.A4.GetWidth() * 0.15), //Doc Date
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Batch No 
                        (float)(PageSize.A4.GetWidth() * 0.55)  //Ware House
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Batch No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                command.CommandText = "EXECUTE [dbo].[Report_Pro_BMR] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 6------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05), //S No
                        (float)(PageSize.A4.GetWidth() * 0.15), //Type
                        (float)(PageSize.A4.GetWidth() * 0.06), //RequiredTrue_ReturnFalse
                        (float)(PageSize.A4.GetWidth() * 0.36),  //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.12),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.26)  //Remarks
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Req / Return")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetHorizontalAlignment(HorizontalAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Remarks")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
          
                ReportName.Value = rn + "2";  

                int SNo = 1;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TypeName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequiredTrue_ReturnFalse"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Remarks"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                   
                        SNo++;

                    }
                }

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
        private async Task<byte[]> RegisterBMRAdditional(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- BMR Additional Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);

            /////////////------------------------------table for Detail 9------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.08),//S No
                        (float)(PageSize.A4.GetWidth() * 0.08),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.08),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.15),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.08),//BatchNo 
                        (float)(PageSize.A4.GetWidth() * 0.12),//Type
                        (float)(PageSize.A4.GetWidth() * 0.06),//RequiredTrue_ReturnFalse
                        (float)(PageSize.A4.GetWidth() * 0.25),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10) //Quantity 
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);


            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Batch #")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Req / Ret")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Pro_BMR] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                                          GroupBy == "Product" ? "ProductName" :
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
                                pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }


                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TypeName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequiredTrue_ReturnFalse"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                    }
                }
            }

            pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
         
            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion

    }
    public class ProductionTransferRepository : IProductionTransfer
    {
        private readonly OreasDbContext db;
        public ProductionTransferRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetProductionTransferMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_ProductionTransferMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.Remarks,
                          o.QAClearedAll,
                          o.ReceivedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Pro_ProductionTransferDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
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
                new { n = "by QA Cleared", v = "byQACleared" }, new { n = "by QA Not Cleared", v = "byQANotCleared" },
                new { n = "by Received", v = "byReceived" }, new { n = "by Not Received", v = "byNotReceived" }
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
                                                       ||
                                                       FilterByLoad == "byReceived" && w.ReceivedAll == true
                                                       ||
                                                       FilterByLoad == "byNotReceived" && w.ReceivedAll == false
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
                                            ||
                                            FilterByLoad == "byReceived" && w.ReceivedAll == true
                                            ||
                                            FilterByLoad == "byNotReceived" && w.ReceivedAll == false
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
        public async Task<string> PostProductionTransferMaster(tbl_Pro_ProductionTransferMaster tbl_Pro_ProductionTransferMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Pro_ProductionTransferMaster.CreatedBy = userName;
                tbl_Pro_ProductionTransferMaster.CreatedDate = DateTime.Now;
                //db.tbl_Pro_ProductionTransferMasters.Add(tbl_Pro_ProductionTransferMaster);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_ProductionTransferMaster.ModifiedBy = userName;
                tbl_Pro_ProductionTransferMaster.ModifiedDate = DateTime.Now;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Pro_ProductionTransferMasters.Remove(db.tbl_Pro_ProductionTransferMasters.Find(tbl_Pro_ProductionTransferMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Pro_ProductionTransferMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
                @ID={3},@DocNo={4},@DocDate={5},@FK_tbl_Inv_WareHouseMaster_ID={6},@Remarks={7},
                @CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Pro_ProductionTransferMaster.ID, tbl_Pro_ProductionTransferMaster.DocNo, tbl_Pro_ProductionTransferMaster.DocDate,
                tbl_Pro_ProductionTransferMaster.FK_tbl_Inv_WareHouseMaster_ID, tbl_Pro_ProductionTransferMaster.Remarks,
                tbl_Pro_ProductionTransferMaster.CreatedBy, tbl_Pro_ProductionTransferMaster.CreatedDate, tbl_Pro_ProductionTransferMaster.ModifiedBy, tbl_Pro_ProductionTransferMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
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
                 new { n = "by QA Cleared", v = "byQACleared" }, new { n = "by QA Pending", v = "byQAPending" }, new { n = "by QA Rejected", v = "byQARejected" },
                 new { n = "by Received", v = "byReceived" }, new { n = "by Not Received", v = "byNotReceived" }
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
                                                       ||
                                                       FilterByLoad == "byReceived" && w.Received == true
                                                       ||
                                                       FilterByLoad == "byNotReceived" && w.Received == false
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
                                            ||
                                            FilterByLoad == "byReceived" && w.Received == true
                                            ||
                                            FilterByLoad == "byNotReceived" && w.Received == false
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
                          QAClearedDate = o.QAClearedDate.HasValue ? o.QAClearedDate.Value.ToString("dd-MMM-yy hh:mm tt") : "",
                          o.Received,
                          o.ReceivedBy,
                          ReceivedDate = o.ReceivedDate.HasValue ? o.ReceivedDate.Value.ToString("dd-MMM-yy hh:mm tt") : "",
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

            if (operation == "Save New")
            {
                tbl_Pro_ProductionTransferDetail.CreatedBy = userName;
                tbl_Pro_ProductionTransferDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Pro_ProductionTransferDetail.ModifiedBy = userName;
                tbl_Pro_ProductionTransferDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Pro_ProductionTransferDetails.Remove(db.tbl_Pro_ProductionTransferDetails.Find(tbl_Pro_ProductionTransferDetail.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

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

        #region Report   
        public List<ReportCallingModel> GetRLProductionTransfer()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Production Transfer",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Production Transfer",
                    GroupBy = new List<string>(){ "WareHouse", "Product" },
                    OrderBy = new List<string>(){ "Doc Date", "Doc No" },
                    SeekBy = null,
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Production Transfer")
            {
                return await Task.Run(() => CurrentProductionTransfer(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Production Transfer")
            {
                return await Task.Run(() => RegisterProductionTransfer(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentProductionTransfer(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Production Transfer -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 3------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocNo
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocDate
                        (float)(PageSize.A4.GetWidth() * 0.70) //WareHouse
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Pro_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 4------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //S No
                        (float)(PageSize.A4.GetWidth() * 0.15),  //BatchNo
                        (float)(PageSize.A4.GetWidth() * 0.60), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15)  //Quantity
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("BatchNo")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";
                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));                        
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                    }
                }

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
        private async Task<byte[]> RegisterProductionTransfer(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Production Transfer Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);

            /////////////------------------------------table for Detail 7------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10),//S No
                        (float)(PageSize.A4.GetWidth() * 0.10),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.10),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.20),//WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.10),//BatchNo
                        (float)(PageSize.A4.GetWidth() * 0.30),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10) //Quantity
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Batch No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Pro_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                                          GroupBy == "Product" ? "ProductName" :
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
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        SNo++;
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }

        #endregion
    }
    public class OrderNoteProductionRepository : IOrderNoteProduction
    {
        private readonly OreasDbContext db;

        public OrderNoteProductionRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region OrderNote
        public object GetWCLOrderNoteDetail()
        {
            return new[]
            {
                new { n = "by OrderNo", v = "byDocNo" }, new { n = "by Product Name", v = "byProductName" }, new { n = "by Priority", v = "byPriority" }
            }.ToList();
        }
        public object GetWCLBOrderNoteDetail()
        {
            return new[]
            {                
                new { n = "by Manufacturing Pending", v = "byManufacturingPending" }, new { n = "by Manufacturing Taken", v = "byManufacturingTaken" },
                new { n = "by Closed", v = "byClosed" }, new { n = "by Open", v = "byOpen" },
                new { n = "by Not QA Processed", v = "byNotQAProcessed" }


            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrderNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrderNoteDetails
                                               .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byClosed" && w.ClosedTrue_OpenFalse == true
                                                 ||
                                                 FilterByLoad == "byOpen" && w.ClosedTrue_OpenFalse == false
                                                 ||
                                                 FilterByLoad == "byManufacturingTaken" && w.Quantity <= w.ManufacturingQty
                                                 ||
                                                 FilterByLoad == "byManufacturingPending" && w.Quantity > w.ManufacturingQty
                                                 ||
                                                 FilterByLoad == "byNotQAProcessed" && w.ManufacturingQty < w.RequestedQtyByProduction
                                                 )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byDocNo" && w.tbl_Inv_OrderNoteMaster.DocNo.ToString() == FilterValueByText
                                                       ||
                                                       FilterByText == "byPriority" && w.AspNetOreasPriority.Priority.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrderNoteDetails
                                  .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byClosed" && w.ClosedTrue_OpenFalse == true
                                                 ||
                                                 FilterByLoad == "byOpen" && w.ClosedTrue_OpenFalse == false
                                                 ||
                                                 FilterByLoad == "byManufacturingTaken" && w.Quantity <= w.ManufacturingQty
                                                 ||
                                                 FilterByLoad == "byManufacturingPending" && w.Quantity > w.ManufacturingQty
                                                 ||
                                                 FilterByLoad == "byNotQAProcessed" && w.ManufacturingQty < w.RequestedQtyByProduction
                                                 )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byDocNo" && w.tbl_Inv_OrderNoteMaster.DocNo.ToString() == FilterValueByText
                                        ||
                                        FilterByText == "byPriority" && w.AspNetOreasPriority.Priority.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrderNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + "[" + o.tbl_Inv_ProductRegistrationDetail.Split_Into + "'s]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_IDName = o?.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.PackagingName ?? "",
                          CouplingUnit = o?.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Pro_CompositionDetail_Coupling.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit ?? "",
                          o.Quantity,
                          o.CustomMRP,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Remarks,
                          o.MfgQtyLimit,
                          o.RequestedQtyByProduction,
                          o.ManufacturingQty,
                          o.SoldQty,
                          o.ClosedTrue_OpenFalse,
                          CustomerName = o.tbl_Inv_OrderNoteMaster.tbl_Ac_ChartOfAccounts.AccountName,
                          o.tbl_Inv_OrderNoteMaster.DocNo,
                          DocDate = o.tbl_Inv_OrderNoteMaster.DocDate.ToString("dd-MMM-yyyy"),
                          TargetDate = o.tbl_Inv_OrderNoteMaster.TargetDate.ToString("dd-MMM-yyyy")

                      };




            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region OrderNote_ProductionOrder
        public async Task<object> GetProductionOrder(int id)
        {
            var qry = from o in await db.tbl_Inv_OrderNoteDetail_ProductionOrders.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrderNoteDetail_ID,
                          o.RequestedBatchSize,
                          o.RequestedBatchNo,
                          RequestedMfgDate = o.RequestedMfgDate.ToString("dd-MMM-yyyy"),
                          o.RequestedBy,
                          RequestedDate = o.RequestedDate.HasValue ? o.RequestedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ProcessedBy,
                          ProcessedDate = o.RequestedDate.HasValue ? o.RequestedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public async Task<PagedData<object>> LoadProductionOrder(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrderNoteDetail_ProductionOrders
                                               .Where(w => w.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrderNoteDetail_ProductionOrders
                                  .Where(w => w.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrderNoteDetail_ID,
                          o.RequestedBatchSize,
                          o.RequestedBatchNo,
                          RequestedMfgDate = o.RequestedMfgDate.ToString("dd-MMM-yyyy hh:mm tt"),
                          o.RequestedBy,
                          RequestedDate = o.RequestedDate.HasValue ? o.RequestedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ProcessedBy,
                          ProcessedDate = o.ProcessedDate.HasValue ? o.ProcessedDate.Value.ToString("dd-MMM-yyyy") : "",
                          ProcessedBatchNo = o?.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters?.FirstOrDefault()?.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo ?? ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostProductionOrder(tbl_Inv_OrderNoteDetail_ProductionOrder tbl_Inv_OrderNoteDetail_ProductionOrder, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            tbl_Inv_OrderNoteDetail_ProductionOrder.RequestedBy = userName;
            tbl_Inv_OrderNoteDetail_ProductionOrder.RequestedDate = DateTime.Now;

            if (operation == "Save New")
            {
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_OrderNoteDetail_ProductionOrder] 
                 @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Inv_OrderNoteDetail_ID={4}
                ,@RequestedBatchSize={5},@RequestedBatchNo={6},@RequestedMfgDate={7}
                ,@RequestedBy={8},@RequestedDate={9},@ProcessedBy={10},@ProcessedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_OrderNoteDetail_ProductionOrder.ID, tbl_Inv_OrderNoteDetail_ProductionOrder.FK_tbl_Inv_OrderNoteDetail_ID,
                tbl_Inv_OrderNoteDetail_ProductionOrder.RequestedBatchSize, tbl_Inv_OrderNoteDetail_ProductionOrder.RequestedBatchNo, tbl_Inv_OrderNoteDetail_ProductionOrder.RequestedMfgDate,
                tbl_Inv_OrderNoteDetail_ProductionOrder.RequestedBy, tbl_Inv_OrderNoteDetail_ProductionOrder.RequestedDate,
                tbl_Inv_OrderNoteDetail_ProductionOrder.ProcessedBy, tbl_Inv_OrderNoteDetail_ProductionOrder.ProcessedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
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

        #endregion

    }   
    public class ProductionDashboardRepository : IProductionDashboard
    {
        private readonly OreasDbContext db;

        public ProductionDashboardRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        public async Task<object> GetDashBoardData(string userName = "")
        {
            int ON_NoOfProd = 0; double ON_TotalOrderQty = 0; double ON_TotalMfgQty = 0;
            int BMR_NoOfOpen = 0; int BMR_NoOfClosed = 0; int BMR_R_DisPending = 0; int BMR_P_DisPending = 0; int BMR_ProcessPending = 0;
            int BMR_R_NoOfProd = 0; int BMR_R_DispPending = 0; int BMR_R_DispReserved = 0; int BMR_P_NoOfProd = 0; int BMR_P_DispPending = 0; int BMR_P_DispReserved = 0;
            int BMR_A_DisPending = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[USP_Pro_DashBoard] @UserName ";
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
                        ON_NoOfProd = (int)sqlReader["ON_NoOfProd"];
                        ON_TotalOrderQty = (double)sqlReader["ON_TotalOrderQty"];
                        ON_TotalMfgQty = (double)sqlReader["ON_TotalMfgQty"];

                        BMR_NoOfOpen = (int)sqlReader["BMR_NoOfOpen"];
                        BMR_NoOfClosed = (int)sqlReader["BMR_NoOfClosed"];
                        BMR_R_DisPending = (int)sqlReader["BMR_R_DisPending"];
                        BMR_P_DisPending = (int)sqlReader["BMR_P_DisPending"];

                        BMR_R_NoOfProd = (int)sqlReader["BMR_R_NoOfProd"];
                        BMR_R_DispPending = (int)sqlReader["BMR_R_DispPending"];
                        BMR_R_DispReserved = (int)sqlReader["BMR_R_DispReserved"];
                        BMR_P_NoOfProd = (int)sqlReader["BMR_P_NoOfProd"];
                        BMR_P_DispPending = (int)sqlReader["BMR_P_DispPending"];
                        BMR_P_DispReserved = (int)sqlReader["BMR_P_DispReserved"];

                        BMR_ProcessPending = (int)sqlReader["BMR_ProcessPending"];
                        BMR_A_DisPending = (int)sqlReader["BMR_A_DisPending"];

                    }
                }
            }
            return new
            {
                OrderNote = new
                {
                    ON_NoOfProd, ON_TotalOrderQty, ON_TotalMfgQty
                },
                BMR = new
                {
                    BMR_NoOfOpen, BMR_NoOfClosed,BMR_R_DisPending,BMR_P_DisPending
                },
                BMRDispensing = new
                {
                    BMR_R_NoOfProd,
                    BMR_R_DispPending,
                    BMR_R_DispReserved,
                    BMR_P_NoOfProd,
                    BMR_P_DispPending,
                    BMR_P_DispReserved,
                },
                BMROther = new {
                    BMR_ProcessPending,
                    BMR_A_DisPending
                }

            };
        }


    }
   
}
