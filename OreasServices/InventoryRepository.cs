﻿using iText.Kernel.Colors;
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
using Org.BouncyCastle.Asn1.X509;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Text.RegularExpressions;
using Table = iText.Layout.Element.Table;
using System.Dynamic;
using iText.IO.Image;

namespace OreasServices
{
    public class InventoryListRepository : IInventoryList
    {
        private readonly OreasDbContext db;
        public InventoryListRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetMeasurementUnitListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_MeasurementUnits
                              select new
                              {
                                  a.ID,
                                  a.MeasurementUnit
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Inv_MeasurementUnits
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.MeasurementUnit.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.MeasurementUnit
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetProductClassificationListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_ProductClassifications
                              select new
                              {
                                  a.ID,
                                  a.ClassName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Inv_ProductClassifications
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.ClassName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.ClassName
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetProductTypeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_ProductTypes
                              select new
                              {
                                  a.ID,
                                  a.ProductType
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Inv_ProductTypes
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.ProductType.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.ProductType
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetProductTypeCategoryListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_ProductType_Categorys.OrderBy(o=> o.tbl_Inv_ProductType.ProductType).ThenBy(o=> o.CategoryName)
                              select new
                              {
                                  a.ID,
                                  CategoryName = a.CategoryName + " [" + a.tbl_Inv_ProductType.ProductType + "]"
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Inv_ProductType_Categorys.OrderBy(o => o.tbl_Inv_ProductType.ProductType).ThenBy(o => o.CategoryName)
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.CategoryName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  CategoryName = a.CategoryName + " [" + a.tbl_Inv_ProductType.ProductType + "]"
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetWareHouseListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_WareHouseMasters
                              select new
                              {
                                  a.ID,
                                  a.WareHouseName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Inv_WareHouseMasters
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.WareHouseName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.WareHouseName
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetWHMListAsync(string QueryName = "", string WHMFilterBy = "", string WHMFilterValue = "", int FormID = 0, string userName = "")
        {
            var qry = from o in await db.VM_Inv_WHMSearchModals.FromSqlRaw("EXECUTE [dbo].[VM_Inv_WHMSearchModal] @QueryName={0}, @FilterBy={1}, @FilterValue={2}, @FormID={3}, @UserName={4}", QueryName, WHMFilterBy, WHMFilterValue, FormID, userName).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.WareHouseName,
                          o.Prefix
                      };
            return qry;
        }
        public async Task<object> GetProductListAsync(string QueryName = "", string ProductFilterBy = "", string ProductFilterValue = "", int? masterid = null, DateTime? tillDate = null, int? detailid = null, string userid = "")
        {
            var qry = from o in await db.VM_Inv_ProductSearchModals.FromSqlRaw("EXECUTE [dbo].[VM_Inv_ProductSearchModal] @QueryName={0}, @FilterBy={1}, @FilterValue={2}, @FormID={3}, @tillDate={4}, @detailid={5}, @userid={6}", QueryName, ProductFilterBy, ProductFilterValue, masterid, tillDate, detailid, userid).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.MasterProdID,
                          o.ProductName,
                          o.CategoryName,
                          o.MeasurementUnit,
                          o.Description,
                          o.IsDecimal,
                          o.Split_Into,
                          o.OtherDetail
                      };
            return qry;
        }
        public async Task<object> GetReferenceListAsync(string QueryName = "", string ReferenceFilterBy = "", string ReferenceFilterValue = "", int? masterid = null, DateTime? tillDate = null, int? detailid = null)
        {
            var qry = from o in await db.VM_Inv_ReferenceSearchModals.FromSqlRaw("EXECUTE [dbo].[VM_Inv_ReferenceSearchModal] @QueryName={0}, @FilterBy={1}, @FilterValue={2}, @FormID={3}, @tillDate={4}, @detailid={5} ", QueryName, ReferenceFilterBy, ReferenceFilterValue, masterid, tillDate, detailid).ToListAsync()
                      select new
                      {
                          o.ReferenceNo,
                          o.Balance,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          o.OtherDetail
                      };
            return qry;
        }
        public async Task<object> GetOrdinaryRequisitionTypeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_OrdinaryRequisitionTypes
                              select new
                              {
                                  a.ID,
                                  a.TypeName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Inv_OrdinaryRequisitionTypes
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.TypeName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.TypeName
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetPurchaseOrderListAsync(string QueryName = "", string POFilterBy = "", string POFilterValue = "", int SupplierID = 0, int ProductID = 0)
        {

            return await (from a in db.tbl_Inv_PurchaseOrderDetails
                                .Where(w =>
                                        w.tbl_Inv_PurchaseOrderMaster.FK_tbl_Ac_ChartOfAccounts_ID == SupplierID
                                        &&
                                        w.FK_tbl_Inv_ProductRegistrationDetail_ID == ProductID
                                        &&
                                        w.ClosedTrue_OpenFalse == false
                                        )
                                 .Where(w => 
                                        string.IsNullOrEmpty(POFilterValue)
                                        ||
                                        POFilterBy == "byPONo" && w.tbl_Inv_PurchaseOrderMaster.PONo.ToString() == POFilterValue
                                        ||
                                        POFilterBy == "byQuantity" && w.Quantity == Convert.ToDouble(POFilterValue)                                        
                                       )
                                 .OrderByDescending(o=> o.tbl_Inv_PurchaseOrderMaster.PODate)
                          select new
                          {
                              a.ID,
                              a.FK_tbl_Inv_ProductRegistrationDetail_ID,
                              FK_tbl_Inv_ProductRegistrationDetail_IDName = a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                              a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                              a.tbl_Inv_PurchaseOrderMaster.PONo,
                              PODate = a.tbl_Inv_PurchaseOrderMaster.PODate.ToString("dd-MMM-yyyy"),
                              a.Quantity,
                              Bal = a.Quantity - a.ReceivedQty
                          }).Take(10).ToListAsync();
        }
        public async Task<object> GetSubDistributorListAsync(string QueryName = "", string SDFilterBy = "", string SDFilterValue = "", int CustomerID = 0)
        {

            return await (from a in db.tbl_Ac_CustomerSubDistributorLists
                                .Where(w =>
                                        w.FK_tbl_Ac_ChartOfAccounts_ID == CustomerID
                                        )
                                 .Where(w =>
                                        string.IsNullOrEmpty(SDFilterValue)
                                        ||
                                        SDFilterBy == "byName" && w.Name.ToLower().Contains(SDFilterValue.ToLower())
                                       )
                                 .OrderByDescending(o => o.Name)
                          select new
                          {
                              a.ID,
                              a.Name,
                              a.Address,
                              a.ContactNo,
                              a.ContactPerson
                          }).Take(10).ToListAsync();
        }
        public async Task<object> GetOrderNoteListAsync(string QueryName = "", string ONFilterBy = "", string ONFilterValue = "", int CustomerID = 0, int ProductID = 0)
        {
            var qry = from o in await db.VM_Inv_OrderNoteSearchModals.FromSqlRaw("EXECUTE [dbo].[VM_Inv_OrderNoteSearchModal] @QueryName={0}, @FilterBy={1}, @FilterValue={2}, @CustomerID={3}, @ProductID={4}", QueryName, ONFilterBy, ONFilterValue, CustomerID, ProductID).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yy"),
                          TargetDate = o.TargetDate.ToString("dd-MMM-yy"),
                          o.ProductName,
                          o.MeasurementUnit,
                          o.Quantity,
                          o.SoldQty
                      };
            return qry;
        }
        public async Task<object> GetPOTermsConditionsListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_PurchaseOrderTermsConditionss
                              select new
                              {
                                  a.ID,
                                  a.TCName
                              }).ToListAsync();
            else
                return await (from a in db.tbl_Inv_PurchaseOrderTermsConditionss
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.TCName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.TCName
                              }).Take(5).ToListAsync();
        }
        public async Task<object> GetSalesNoteForReturnAsync(int CustomerID = 0, int? PurchaseRefID = null, int? BMRRefID = null, string SalesNoteFilterBy = null, string SalesNoteFilterValue = null)
        {
            return await (from a in db.tbl_Inv_SalesNoteDetails
                            join m in db.tbl_Inv_SalesNoteMasters on a.FK_tbl_Inv_SalesNoteMaster_ID equals m.ID
                            join p in db.tbl_Inv_ProductRegistrationDetails on a.FK_tbl_Inv_ProductRegistrationDetail_ID equals p.ID
                            join u in db.tbl_Inv_MeasurementUnits on p.FK_tbl_Inv_MeasurementUnit_ID equals u.ID
                            where m.FK_tbl_Ac_ChartOfAccounts_ID == CustomerID && 
                                  a.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo == PurchaseRefID &&
                                  a.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo == BMRRefID &&
                                  (
                                    string.IsNullOrEmpty(SalesNoteFilterValue)
                                    ||
                                    SalesNoteFilterBy == "InvoiceNo" && m.DocNo == Convert.ToInt32(SalesNoteFilterValue)
                                    ||
                                    SalesNoteFilterBy == "Quantity" && a.Quantity == Convert.ToDouble(SalesNoteFilterValue)
                                  )
                          select new
                          {
                              a.ID,
                              InvoiceNo = m.DocNo,
                              InvoiceDate = m.DocDate.ToString("dd-MMM-yy"),                              
                              a.Quantity,
                              u.MeasurementUnit
                          }).ToListAsync();

        }
        public async Task<object> GetPOSupplierListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_PurchaseOrder_Suppliers
                              select new
                              {
                                  a.ID,
                                  a.SupplierName
                              }).OrderBy(o=> o.SupplierName).ToListAsync();
            else
                return await (from a in db.tbl_Inv_PurchaseOrder_Suppliers
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.SupplierName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.SupplierName
                              }).OrderBy(o => o.SupplierName).Take(5).ToListAsync();
        }
        public async Task<object> GetPOManufacturerListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_PurchaseOrder_Manufacturers
                              select new
                              {
                                  a.ID,
                                  a.ManufacturerName
                              }).OrderBy(o => o.ManufacturerName).ToListAsync();
            else
                return await (from a in db.tbl_Inv_PurchaseOrder_Manufacturers
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.ManufacturerName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.ManufacturerName
                              }).OrderBy(o => o.ManufacturerName).Take(5).ToListAsync();
        }
        public async Task<object> GetPOIndenterListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_PurchaseOrder_Indenters
                              select new
                              {
                                  a.ID,
                                  a.IndenterName
                              }).OrderBy(o => o.IndenterName).ToListAsync();
            else
                return await (from a in db.tbl_Inv_PurchaseOrder_Indenters
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.IndenterName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.IndenterName
                              }).OrderBy(o => o.IndenterName).Take(5).ToListAsync();
        }
        public async Task<object> GetPOImportTermListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_PurchaseOrder_ImportTermss
                              select new
                              {
                                  a.ID,
                                  TermName = a.TermName + " [" + (a.AtSight ? "At Sight" : "At Usance" + " Days" + a.AtUsanceDays.ToString()) + "]"
                              }).OrderBy(o => o.TermName).ToListAsync();
            else
                return await (from a in db.tbl_Inv_PurchaseOrder_ImportTermss
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.TermName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  TermName = a.TermName + " [" + (a.AtSight ? "At Sight" : "At Usance" + " Days" + a.AtUsanceDays.ToString()) + "]"
                              }).OrderBy(o => o.TermName).Take(5).ToListAsync();
        }
        public async Task<object> GetCurrencyCodeListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_CurrencyAndCountrys
                              select new
                              {
                                  a.ID,
                                  a.CurrencyCode
                              }).OrderBy(o => o.CurrencyCode).ToListAsync();
            else
                return await (from a in db.tbl_Ac_CurrencyAndCountrys
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.CurrencyCode.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.CurrencyCode
                              }).OrderBy(o => o.CurrencyCode).Take(5).ToListAsync();
        }
        public async Task<object> GetCountryListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Ac_CurrencyAndCountrys
                              select new
                              {
                                  a.ID,
                                  a.CountryName
                              }).OrderBy(o => o.CountryName).ToListAsync();
            else
                return await (from a in db.tbl_Ac_CurrencyAndCountrys
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.CountryName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.CountryName
                              }).OrderBy(o => o.CountryName).Take(5).ToListAsync();
        }
        public async Task<object> GetInternationalCommercialTermListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_InternationalCommercialTerms
                              select new
                              {
                                  a.ID,
                                  IncotermName = a.IncotermName + " [" + a.Abbreviation + "]"
                              }).OrderBy(o => o.IncotermName).ToListAsync();
            else
                return await (from a in db.tbl_Inv_InternationalCommercialTerms
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.IncotermName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  IncotermName = a.IncotermName + " [" + a.Abbreviation + "]"
                              }).OrderBy(o => o.IncotermName).Take(5).ToListAsync();
        }
        public async Task<object> GetTransportListAsync(string FilterByText = null, string FilterValueByText = null)
        {
            if (string.IsNullOrEmpty(FilterByText))
                return await (from a in db.tbl_Inv_TransportTypes
                              select new
                              {
                                  a.ID,
                                  a.TypeName
                              }).OrderBy(o => o.TypeName).ToListAsync();
            else
                return await (from a in db.tbl_Inv_TransportTypes
                                        .Where(w => string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.TypeName.ToLower().Contains(FilterValueByText.ToLower()))
                              select new
                              {
                                  a.ID,
                                  a.TypeName
                              }).OrderBy(o => o.TypeName).Take(5).ToListAsync();
        }
    }
    public class MeasurementUnitRepository : IMeasurementUnit
    {
        private readonly OreasDbContext db;
        public MeasurementUnitRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Inv_MeasurementUnits.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.MeasurementUnit,
                          o.IsDecimal,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }

        public object GetWCLMeasurementUnit()
        {
            return new[]
            {
                new { n = "by Measurement Unit", v = "byMeasurementUnit" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_MeasurementUnits
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byMeasurementUnit" && w.MeasurementUnit.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_MeasurementUnits
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byMeasurementUnit" && w.MeasurementUnit.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.MeasurementUnit,
                          o.IsDecimal,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }

        public async Task<string> Post(tbl_Inv_MeasurementUnit tbl_Inv_MeasurementUnit, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_MeasurementUnit.CreatedBy = userName;
                tbl_Inv_MeasurementUnit.CreatedDate = DateTime.Now;
                db.tbl_Inv_MeasurementUnits.Add(tbl_Inv_MeasurementUnit);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_MeasurementUnit.ModifiedBy = userName;
                tbl_Inv_MeasurementUnit.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_MeasurementUnit).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_MeasurementUnits.Remove(db.tbl_Inv_MeasurementUnits.Find(tbl_Inv_MeasurementUnit.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class ProductClassificationRepository : IProductClassification
    {
        private readonly OreasDbContext db;
        public ProductClassificationRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Inv_ProductClassifications.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.ClassName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }

        public object GetWCLProductClassification()
        {
            return new[]
            {
                new { n = "by Class Name", v = "byClassName" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_ProductClassifications
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byClassName" && w.ClassName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_ProductClassifications
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byClassName" && w.ClassName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.ClassName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }

        public async Task<string> Post(tbl_Inv_ProductClassification tbl_Inv_ProductClassification, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_ProductClassification.CreatedBy = userName;
                tbl_Inv_ProductClassification.CreatedDate = DateTime.Now;
                db.tbl_Inv_ProductClassifications.Add(tbl_Inv_ProductClassification);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_ProductClassification.ModifiedBy = userName;
                tbl_Inv_ProductClassification.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_ProductClassification).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_ProductClassifications.Remove(db.tbl_Inv_ProductClassifications.Find(tbl_Inv_ProductClassification.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class ProductTypeRepository : IProductType
    {
        private readonly OreasDbContext db;
        public ProductTypeRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetProductTypeMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_ProductTypes.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.ProductType,
                          o.Prefix,
                          o.FK_tbl_Qc_ActionType_ID_PurchaseNote,
                          FK_tbl_Qc_ActionType_ID_PurchaseNoteName = o.tbl_Qc_ActionType.ActionName,
                          o.PurchaseNoteDetailRateAutoInsertFromPO,
                          o.PurchaseNoteDetailWithOutPOAllowed,
                          o.SalesNoteDetailRateAutoInsertFromON,
                          o.SalesNoteDetailWithOutONAllowed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLProductTypeMaster()
        {
            return new[]
            {
                new { n = "by Product Type", v = "byProductType" }, new { n = "by Category Name", v = "byCategoryName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductTypeMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_ProductTypes
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductType" && w.ProductType.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byCategoryName" && w.tbl_Inv_ProductType_Categorys.Any(a => a.CategoryName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_ProductTypes
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductType" && w.ProductType.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byCategoryName" && w.tbl_Inv_ProductType_Categorys.Any(a => a.CategoryName.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.ProductType,
                          o.Prefix,
                          o.FK_tbl_Qc_ActionType_ID_PurchaseNote,
                          FK_tbl_Qc_ActionType_ID_PurchaseNoteName = o.tbl_Qc_ActionType.ActionName,
                          o.PurchaseNoteDetailRateAutoInsertFromPO,
                          o.PurchaseNoteDetailWithOutPOAllowed,
                          o.SalesNoteDetailRateAutoInsertFromON,
                          o.SalesNoteDetailWithOutONAllowed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          NoOfCategories = o.tbl_Inv_ProductType_Categorys.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostProductTypeMaster(tbl_Inv_ProductType tbl_Inv_ProductType, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_ProductType.CreatedBy = userName;
                tbl_Inv_ProductType.CreatedDate = DateTime.Now;
                tbl_Inv_ProductType.PurchaseNoteDetailWithOutPOAllowed = true;
                tbl_Inv_ProductType.PurchaseNoteDetailRateAutoInsertFromPO = false;
                tbl_Inv_ProductType.SalesNoteDetailWithOutONAllowed = true;
                tbl_Inv_ProductType.SalesNoteDetailRateAutoInsertFromON = false;

                db.tbl_Inv_ProductTypes.Add(tbl_Inv_ProductType);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_ProductType.ModifiedBy = userName;
                tbl_Inv_ProductType.ModifiedDate = DateTime.Now;

                db.Entry(tbl_Inv_ProductType).Property(x => x.Prefix).IsModified = true;
                db.Entry(tbl_Inv_ProductType).Property(x => x.ProductType).IsModified = true;
                db.Entry(tbl_Inv_ProductType).Property(x => x.FK_tbl_Qc_ActionType_ID_PurchaseNote).IsModified = true;
                db.Entry(tbl_Inv_ProductType).Property(x => x.PurchaseNoteDetailWithOutPOAllowed).IsModified = false;
                db.Entry(tbl_Inv_ProductType).Property(x => x.PurchaseNoteDetailRateAutoInsertFromPO).IsModified = false;
                db.Entry(tbl_Inv_ProductType).Property(x => x.SalesNoteDetailWithOutONAllowed).IsModified = false;
                db.Entry(tbl_Inv_ProductType).Property(x => x.SalesNoteDetailRateAutoInsertFromON).IsModified = false;
                await db.SaveChangesAsync();


                //db.Entry(tbl_Inv_ProductType).State = EntityState.Modified;
                //await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_ProductTypes.Remove(db.tbl_Inv_ProductTypes.Find(tbl_Inv_ProductType.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        #endregion

        #region Detail
        public async Task<object> GetProductTypeDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_ProductType_Categorys.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductType_ID,
                          o.CategoryName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLProductTypeDetail()
        {
            return new[]
            {
                new { n = "by Category Name", v = "byCategoryName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductTypeDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_ProductType_Categorys
                                               .Where(w=> w.FK_tbl_Inv_ProductType_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byCategoryName" && w.CategoryName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_ProductType_Categorys
                                  .Where(w => w.FK_tbl_Inv_ProductType_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byCategoryName" && w.CategoryName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductType_ID,
                          o.CategoryName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostProductTypeDetail(tbl_Inv_ProductType_Category tbl_Inv_ProductType_Category, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_ProductType_Category.CreatedBy = userName;
                tbl_Inv_ProductType_Category.CreatedDate = DateTime.Now;
                db.tbl_Inv_ProductType_Categorys.Add(tbl_Inv_ProductType_Category);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_ProductType_Category.ModifiedBy = userName;
                tbl_Inv_ProductType_Category.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_ProductType_Category).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_ProductType_Categorys.Remove(db.tbl_Inv_ProductType_Categorys.Find(tbl_Inv_ProductType_Category.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion

        #region Report     
        public List<ReportCallingModel> GetRLProductTypeDetail()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Product List by Type Excel",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }

        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Product List by Type Excel")
            {
                return await Task.Run(() => ProductListbyTypeExcel(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }

        private async Task<byte[]> ProductListbyTypeExcel(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("ProductList");

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                Sheet.Cells[1, 2].Value = "Product Code";
                Sheet.Cells[1, 3].Value = "Product Name";
                Sheet.Cells[1, 4].Value = "Unit";
                Sheet.Cells[1, 5].Value = "Description";
                Sheet.Cells[1, 6].Value = "ReorderLevel";
                Sheet.Cells[1, 7].Value = "Split Into";
                Sheet.Cells[1, 8].Value = "Harmonized#";
                Sheet.Cells[1, 9].Value = "GTIN Code";
                Sheet.Cells[1, 10].Value = "Standard MRP";
                Sheet.Cells[1, 11].Value = "Category";

                for (int c = 1; c <= 11; c++)
                {
                    Sheet.Cells[1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Sheet.Cells[1, c].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSteelBlue);
                }

                int row = 2; int Sno = 1;
                string CategoryName = "";
                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        Sheet.Row(row).Height = 22;
                        Sheet.Row(row).Style.Font.Size = 12;
                            
                        if (sqlReader["CategoryName"].ToString() != CategoryName)
                        {
                            if (string.IsNullOrEmpty(CategoryName))
                            {
                                Sheet.Workbook.Worksheets[0].Name = sqlReader["ProductType"].ToString();
                            }
                           
                            CategoryName = sqlReader["CategoryName"].ToString();

                            //-------------this is when category shows in seperate full row---------------//
                            //Sheet.Row(row).Height = 22;
                            //Sheet.Row(row).Style.Font.Size = 14;
                            //Sheet.Cells[row, 1, row, 10].Merge = true; Sheet.Cells[row, 1, row, 10].Value = CategoryName;
                            //row++;
                        }

                        Sheet.Cells[row, 1].Value = Sno;
                        Sheet.Cells[row, 2].Value = sqlReader["ProductCode"].ToString();
                        Sheet.Cells[row, 3].Value = sqlReader["ProductName"].ToString();
                        Sheet.Cells[row, 4].Value = sqlReader["MeasurementUnit"].ToString();
                        Sheet.Cells[row, 5].Value = sqlReader["Description"].ToString();
                        Sheet.Cells[row, 6].Value = sqlReader["ReorderLevel"].ToString();
                        Sheet.Cells[row, 7].Value = sqlReader["Split_Into"].ToString();
                        Sheet.Cells[row, 8].Value = sqlReader["HarmonizedCode"].ToString();
                        Sheet.Cells[row, 9].Value = sqlReader["GTINCode"].ToString();
                        Sheet.Cells[row, 10].Value = sqlReader["StandardMRP"].ToString();
                        Sheet.Cells[row, 11].Value = sqlReader["CategoryName"].ToString();

                        row++;
                        Sno++;
                    }
                }

                Sheet.Cells[1, 1, row - 1, 11].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[1, 1, row - 1, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[1, 1, row - 1, 11].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                Sheet.Cells[1, 1, row - 1, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                for (int i = 1; i <= 11; i++)
                {
                    Sheet.Column(i).BestFit = true;
                };


                Sheet.Cells["A:AZ"].AutoFitColumns();
            }

            return Ep.GetAsByteArray();
        }

        #endregion
    }
    public class WareHouseRepository : IWareHouse
    {
        private readonly OreasDbContext db;
        public WareHouseRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetWareHouseMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_WareHouseMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.WareHouseName,
                          o.Prefix,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLWareHouseMaster()
        {
            return new[]
            {
                new { n = "by WareHouse Name", v = "byWareHouseName" }, new { n = "by Category Name", v = "byCategoryName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadWareHouseMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_WareHouseMasters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byWareHouseName" && w.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byCategoryName" && w.tbl_Inv_WareHouseDetails.Any(a => a.tbl_Inv_ProductType_Category.CategoryName.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_WareHouseMasters
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byWareHouseName" && w.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byCategoryName" && w.tbl_Inv_WareHouseDetails.Any(a => a.tbl_Inv_ProductType_Category.CategoryName.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.WareHouseName,
                          o.Prefix,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          NoOfCategories = o.tbl_Inv_WareHouseDetails.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostWareHouseMaster(tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_WareHouseMaster.CreatedBy = userName;
                tbl_Inv_WareHouseMaster.CreatedDate = DateTime.Now;
                db.tbl_Inv_WareHouseMasters.Add(tbl_Inv_WareHouseMaster);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_WareHouseMaster.ModifiedBy = userName;
                tbl_Inv_WareHouseMaster.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_WareHouseMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_WareHouseMasters.Remove(db.tbl_Inv_WareHouseMasters.Find(tbl_Inv_WareHouseMaster.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        #endregion

        #region Detail
        public async Task<object> GetWareHouseDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_WareHouseDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          o.FK_tbl_Inv_ProductType_Category_ID,
                          FK_tbl_Inv_ProductType_Category_IDName = o.tbl_Inv_ProductType_Category.CategoryName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLWareHouseDetail()
        {
            return new[]
            {
                new { n = "by Category Name", v = "byCategoryName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadWareHouseDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_WareHouseDetails
                                               .Where(w => w.FK_tbl_Inv_WareHouseMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byCategoryName" && w.tbl_Inv_ProductType_Category.CategoryName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_WareHouseDetails
                                  .Where(w => w.FK_tbl_Inv_WareHouseMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byCategoryName" && w.tbl_Inv_ProductType_Category.CategoryName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          o.FK_tbl_Inv_ProductType_Category_ID,
                          FK_tbl_Inv_ProductType_Category_IDName = o.tbl_Inv_ProductType_Category.CategoryName + " [" + o.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostWareHouseDetail(tbl_Inv_WareHouseDetail tbl_Inv_WareHouseDetail, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_WareHouseDetail.CreatedBy = userName;
                tbl_Inv_WareHouseDetail.CreatedDate = DateTime.Now;
                db.tbl_Inv_WareHouseDetails.Add(tbl_Inv_WareHouseDetail);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_WareHouseDetail.ModifiedBy = userName;
                tbl_Inv_WareHouseDetail.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_WareHouseDetail).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_WareHouseDetails.Remove(db.tbl_Inv_WareHouseDetails.Find(tbl_Inv_WareHouseDetail.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

        #endregion
    }
    public class ProductRegistrationRepository : IProductRegistration
    {
        private readonly OreasDbContext db;
        public ProductRegistrationRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetProductRegistrationMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_ProductRegistrationMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductClassification_ID,
                          FK_tbl_Inv_ProductClassification_IDName = o.tbl_Inv_ProductClassification.ClassName,
                          o.ProductName,
                          o.Description,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLProductRegistrationMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }, new { n = "by Product Code", v = "byProductCode" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductRegistrationMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_ProductRegistrationMasters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductCode" && w.tbl_Inv_ProductRegistrationDetails.Any(a => a.ProductCode.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_ProductRegistrationMasters
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductCode" && w.tbl_Inv_ProductRegistrationDetails.Any(a => a.ProductCode.ToLower().Contains(FilterValueByText.ToLower()))
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductClassification_ID,
                          FK_tbl_Inv_ProductClassification_IDName = o.tbl_Inv_ProductClassification.ClassName,
                          o.ProductName,
                          o.Description,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          NoOfUnits = o.tbl_Inv_ProductRegistrationDetails.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostProductRegistrationMaster(tbl_Inv_ProductRegistrationMaster tbl_Inv_ProductRegistrationMaster, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_ProductRegistrationMaster.CreatedBy = userName;
                tbl_Inv_ProductRegistrationMaster.CreatedDate = DateTime.Now;
                db.tbl_Inv_ProductRegistrationMasters.Add(tbl_Inv_ProductRegistrationMaster);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_ProductRegistrationMaster.ModifiedBy = userName;
                tbl_Inv_ProductRegistrationMaster.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_ProductRegistrationMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_ProductRegistrationMasters.Remove(db.tbl_Inv_ProductRegistrationMasters.Find(tbl_Inv_ProductRegistrationMaster.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
        public async Task<string> ProductRegistrationUploadExcelFile(List<ProdRegExcelData> ProdRegExcelDataList, string operation, string userName)
        {
            if (operation == "Save New")
            {
                //------------Add compiled record to database--------------------//
                SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
                SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
                SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                CRUD_Type.Value = "Insert";

                foreach (var item in ProdRegExcelDataList)
                {
                    if (!(item.ClassificationID > 0) || string.IsNullOrEmpty(item.ProductName))
                    {
                        continue;
                    }
                    //-------------Master Entry----------//
                    tbl_Inv_ProductRegistrationMaster master =
                        new tbl_Inv_ProductRegistrationMaster() 
                        { 
                            ID=0,
                            FK_tbl_Inv_ProductClassification_ID = item.ClassificationID,
                            ProductName = item.ProductName,
                            Description = null,
                            CreatedBy = userName,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = null,
                            ModifiedDate = null
                        };

                    db.tbl_Inv_ProductRegistrationMasters.Add(master);
                    await db.SaveChangesAsync();

                    //---------------------Detail Entries-------------//
                    if (master.ID>0 && item.CategoryID > 0 && item.UnitID > 0 && 
                        item.Split_Into > 0 && item.ReorderLevel >= 0)
                    {                        
                        //-----------Root Unit-------------//
                        tbl_Inv_ProductRegistrationDetail detail =
                        new tbl_Inv_ProductRegistrationDetail()
                        {
                            ID = 0,
                            FK_tbl_Inv_ProductRegistrationMaster_ID = master.ID,
                            FK_tbl_Inv_ProductType_Category_ID = item.CategoryID,
                            FK_tbl_Inv_MeasurementUnit_ID = item.UnitID,
                            ProductCode = item.ProductCode,
                            IsInventory = true,
                            IsDiscontinue = false,
                            ReorderLevel = item.ReorderLevel,
                            ReorderAlert = item.ReorderLevel > 0 ? true : false,
                            Split_Into = item.Split_Into,                            
                            FK_tbl_Inv_ProductRegistrationDetail_ID = null,
                            Description = null,
                            CreatedBy = userName,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = null,
                            ModifiedDate = null
                        };

                        CRUD_ID.Value = 0;
                        await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_ProductRegistrationDetail] 
                       @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                      ,@ID={3},@FK_tbl_Inv_ProductRegistrationMaster_ID={4},@FK_tbl_Inv_ProductType_Category_ID={5}
                      ,@Description={6},@ProductCode={7},@FK_tbl_Inv_MeasurementUnit_ID={8}
                      ,@ReorderLevel={9},@ReorderAlert={10},@FK_tbl_Inv_ProductRegistrationDetail_ID={11},@Split_Into={12},@IsInventory={13},@IsDiscontinue={14}
                      ,@HarmonizedCode={15},@GTINCode={16},@StandardMRP={17}
                      ,@CreatedBy={18},@CreatedDate={19},@ModifiedBy={20},@ModifiedDate={21}",
                        CRUD_Type, CRUD_Msg, CRUD_ID,
                        detail.ID, detail.FK_tbl_Inv_ProductRegistrationMaster_ID, detail.FK_tbl_Inv_ProductType_Category_ID,
                        detail.Description, detail.ProductCode, detail.FK_tbl_Inv_MeasurementUnit_ID,
                        detail.ReorderLevel, detail.ReorderAlert, detail.FK_tbl_Inv_ProductRegistrationDetail_ID, detail.Split_Into, detail.IsInventory, detail.IsDiscontinue,
                        detail.HarmonizedCode, detail.GTINCode, detail.StandardMRP,
                        detail.CreatedBy, detail.CreatedDate, detail.ModifiedBy, detail.ModifiedDate);


                        if ((int)CRUD_ID.Value > 0 && master.ID > 0 && item.CategoryID2 > 0 && 
                            item.UnitID2 > 0 && item.Split_Into2 > 0 && item.ReorderLevel2 >= 0)
                        {
                            //-----------Child of Root Unit-------------//
                            tbl_Inv_ProductRegistrationDetail detail2 =
                                new tbl_Inv_ProductRegistrationDetail()
                                {
                                    ID = 0,
                                    FK_tbl_Inv_ProductRegistrationMaster_ID = master.ID,
                                    FK_tbl_Inv_ProductType_Category_ID = item.CategoryID2,
                                    FK_tbl_Inv_MeasurementUnit_ID = item.UnitID2,
                                    ProductCode = item.ProductCode2,
                                    IsInventory = true,
                                    IsDiscontinue = false,
                                    ReorderLevel = item.ReorderLevel2,
                                    ReorderAlert = item.ReorderLevel2 > 0 ? true : false,
                                    Split_Into = item.Split_Into2,
                                    FK_tbl_Inv_ProductRegistrationDetail_ID = (int)CRUD_ID.Value,
                                    Description = null,
                                    CreatedBy = userName,
                                    CreatedDate = DateTime.Now,
                                    ModifiedBy = null,
                                    ModifiedDate = null
                                };

                            CRUD_ID.Value = 0;
                            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_ProductRegistrationDetail] 
                               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                              ,@ID={3},@FK_tbl_Inv_ProductRegistrationMaster_ID={4},@FK_tbl_Inv_ProductType_Category_ID={5}
                              ,@Description={6},@ProductCode={7},@FK_tbl_Inv_MeasurementUnit_ID={8}
                              ,@ReorderLevel={9},@ReorderAlert={10},@FK_tbl_Inv_ProductRegistrationDetail_ID={11},@Split_Into={12},@IsInventory={13},@IsDiscontinue={14}
                              ,@HarmonizedCode={15},@GTINCode={16},@StandardMRP={17}
                              ,@CreatedBy={18},@CreatedDate={19},@ModifiedBy={20},@ModifiedDate={21}",
                            CRUD_Type, CRUD_Msg, CRUD_ID,
                            detail.ID, detail2.FK_tbl_Inv_ProductRegistrationMaster_ID, detail2.FK_tbl_Inv_ProductType_Category_ID,
                            detail2.Description, detail2.ProductCode, detail2.FK_tbl_Inv_MeasurementUnit_ID,
                            detail2.ReorderLevel, detail2.ReorderAlert, detail2.FK_tbl_Inv_ProductRegistrationDetail_ID, detail2.Split_Into, detail2.IsInventory, detail2.IsDiscontinue,
                            detail2.HarmonizedCode, detail2.GTINCode, detail2.StandardMRP,
                            detail2.CreatedBy, detail2.CreatedDate, detail2.ModifiedBy, detail2.ModifiedDate);

                        }

                    }

                }       
            }
            else
            {
                return "Wrong Operation";
            }

            return "OK";
        }
        #endregion

        #region Detail
        public async Task<object> GetProductRegistrationDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_ProductRegistrationDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductRegistrationMaster_ID,
                          o.FK_tbl_Inv_ProductType_Category_ID,
                          FK_tbl_Inv_ProductType_Category_IDName = o.tbl_Inv_ProductType_Category.CategoryName + " [" + o.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.Description,
                          o.ProductCode,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.ReorderLevel,
                          o.ReorderAlert,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.FK_tbl_Inv_ProductRegistrationDetail_ID > 0 ? "[" + o.tbl_Inv_ProductRegistrationDetail_Parent.Description + "] " + o.tbl_Inv_ProductRegistrationDetail_Parent.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.Split_Into,
                          o.IsInventory,
                          o.IsDiscontinue,
                          o.HarmonizedCode,
                          o.StandardMRP,
                          o.GTINCode,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          ChildCount = o.tbl_Inv_ProductRegistrationDetails_Parents.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLProductRegistrationDetail()
        {
            return new[]
            {
                new { n = "by Product Code", v = "byProductCode" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductRegistrationDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_ProductRegistrationDetails
                                               .Where(w => w.FK_tbl_Inv_ProductRegistrationMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductCode" && w.ProductCode.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_ProductRegistrationDetails
                                  .Where(w => w.FK_tbl_Inv_ProductRegistrationMaster_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductCode" && w.ProductCode.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductRegistrationMaster_ID,
                          o.FK_tbl_Inv_ProductType_Category_ID,
                          FK_tbl_Inv_ProductType_Category_IDName = o.tbl_Inv_ProductType_Category.CategoryName + " [" + o.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.Description,
                          o.ProductCode,
                          o.FK_tbl_Inv_MeasurementUnit_ID,
                          FK_tbl_Inv_MeasurementUnit_IDName = o.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.ReorderLevel,
                          o.ReorderAlert,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.FK_tbl_Inv_ProductRegistrationDetail_ID > 0 ? "[" + o.tbl_Inv_ProductRegistrationDetail_Parent.Description + "] " + o.tbl_Inv_ProductRegistrationDetail_Parent.tbl_Inv_MeasurementUnit.MeasurementUnit : "",
                          o.Split_Into,
                          o.IsInventory,
                          o.IsDiscontinue,
                          o.HarmonizedCode,
                          o.GTINCode,
                          o.StandardMRP,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          ChildCount = o.tbl_Inv_ProductRegistrationDetails_Parents.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostProductRegistrationDetail(tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_ProductRegistrationDetail.CreatedBy = userName;
                tbl_Inv_ProductRegistrationDetail.CreatedDate = DateTime.Now;
                //db.tbl_Inv_ProductRegistrationDetails.Add(tbl_Inv_ProductRegistrationDetail);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_ProductRegistrationDetail.ModifiedBy = userName;
                tbl_Inv_ProductRegistrationDetail.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Inv_ProductRegistrationDetail).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Inv_ProductRegistrationDetails.Remove(db.tbl_Inv_ProductRegistrationDetails.Find(tbl_Inv_ProductRegistrationDetail.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_ProductRegistrationDetail] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Inv_ProductRegistrationMaster_ID={4},@FK_tbl_Inv_ProductType_Category_ID={5}
              ,@Description={6},@ProductCode={7},@FK_tbl_Inv_MeasurementUnit_ID={8}
              ,@ReorderLevel={9},@ReorderAlert={10},@FK_tbl_Inv_ProductRegistrationDetail_ID={11},@Split_Into={12},@IsInventory={13},@IsDiscontinue={14}
              ,@HarmonizedCode={15},@GTINCode={16},@StandardMRP={17}
              ,@CreatedBy={18},@CreatedDate={19},@ModifiedBy={20},@ModifiedDate={21}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_ProductRegistrationDetail.ID, tbl_Inv_ProductRegistrationDetail.FK_tbl_Inv_ProductRegistrationMaster_ID, tbl_Inv_ProductRegistrationDetail.FK_tbl_Inv_ProductType_Category_ID,
                tbl_Inv_ProductRegistrationDetail.Description, tbl_Inv_ProductRegistrationDetail.ProductCode, tbl_Inv_ProductRegistrationDetail.FK_tbl_Inv_MeasurementUnit_ID,
                tbl_Inv_ProductRegistrationDetail.ReorderLevel, tbl_Inv_ProductRegistrationDetail.ReorderAlert, tbl_Inv_ProductRegistrationDetail.FK_tbl_Inv_ProductRegistrationDetail_ID, tbl_Inv_ProductRegistrationDetail.Split_Into, tbl_Inv_ProductRegistrationDetail.IsInventory, tbl_Inv_ProductRegistrationDetail.IsDiscontinue,
                tbl_Inv_ProductRegistrationDetail.HarmonizedCode, tbl_Inv_ProductRegistrationDetail.GTINCode, tbl_Inv_ProductRegistrationDetail.StandardMRP,
                tbl_Inv_ProductRegistrationDetail.CreatedBy, tbl_Inv_ProductRegistrationDetail.CreatedDate, tbl_Inv_ProductRegistrationDetail.ModifiedBy, tbl_Inv_ProductRegistrationDetail.ModifiedDate);


            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        public async Task<object> GetNodesAsync(int PID = 0, int MasterID = 0)
        {
            List<TreeView_ProductUnit> treenodes = new List<TreeView_ProductUnit>();
            if (PID >= 0)
            {
                var rootNodes = await db.tbl_Inv_ProductRegistrationDetails.Where(i => i.FK_tbl_Inv_ProductRegistrationMaster_ID == MasterID && (i.FK_tbl_Inv_ProductRegistrationDetail_ID ?? 0) == PID).ToListAsync();
                int _ChildCount = 0;
                foreach (var nodes in rootNodes)
                {
                    _ChildCount = await db.tbl_Inv_ProductRegistrationDetails.Where(i => i.FK_tbl_Inv_ProductRegistrationMaster_ID == MasterID && i.FK_tbl_Inv_ProductRegistrationDetail_ID == nodes.ID).CountAsync();

                    treenodes.Add(new TreeView_ProductUnit()
                    {
                        ID = nodes.ID,
                        Description = nodes.Description,
                        Unit = nodes.tbl_Inv_MeasurementUnit.MeasurementUnit,
                        Split_Into = nodes.Split_Into,
                        ParentID = nodes.FK_tbl_Inv_ProductRegistrationDetail_ID ?? 0,
                        ChildCount = _ChildCount,
                        spacing = "",
                        sign = (_ChildCount > 0) ? "+" : "",
                        IsParent = (_ChildCount > 0)
                    });
                }

            }
            return treenodes;
        }
        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLProductRegistration()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Product List",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Product List")
            {
                return await Task.Run(() => ProductList(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> ProductList(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Product List by -----", true);

            /////////////------------------------------table for Detail 5------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.50), //
                        (float)(PageSize.A4.GetWidth() * 0.10), //
                        (float)(PageSize.A4.GetWidth() * 0.10), //
                        (float)(PageSize.A4.GetWidth() * 0.10), //
                        (float)(PageSize.A4.GetWidth() * 0.10) // 
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string CategoryName = "";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {

                        if (CategoryName != sqlReader["CategoryName"].ToString())
                        {
                            CategoryName = sqlReader["CategoryName"].ToString();

                            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["ProductType"].ToString())).SetBold().SetFontSize(8).SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["CategoryName"].ToString())).SetBold().SetKeepTogether(true));

                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Code")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Reorder Qty")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Split")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        }
                        
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductCode"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReorderLevel"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Split_Into"].ToString())).SetKeepTogether(true));

                    }

                }

            }

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion
    }
    public class PurchaseNoteRepository : IPurchaseNote
    {
        private readonly OreasDbContext db;
        public PurchaseNoteRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetPurchaseNoteMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          o.DocDate,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.SupplierChallanNo,
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.IsQCAll,
                          o.IsQCSampleAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_PurchaseNoteDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseNoteMaster()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }, new { n = "by DocNo", v = "byDocNo" }
            }.ToList();
        }
        public object GetWCLBPurchaseNoteMaster()
        {
            return new[]
            {
                new { n = "by Any QC Pending", v = "byAnyQCPending" }, new { n = "by Any Sample Pending", v = "byAnySamplePending" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseNoteMasters
                                    .Where(w=> w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a=> a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w=> w.UserName == userName) > 0)
                                                )
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_PurchaseNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                       
                                            )
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byAnyQCPending" && w.IsQCAll == false
                                            ||
                                            FilterByLoad == "byAnySamplePending" && w.IsQCSampleAll == false
                                            )
                                    .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseNoteMasters
                                      .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                   .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                   .Where(w => w.UserName == userName).Count() > 0))
                                      .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_PurchaseNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                          )
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byAnyQCPending" && w.IsQCAll == false
                                                 ||
                                                 FilterByLoad == "byAnySamplePending" && w.IsQCSampleAll == false
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
                          o.Remarks,
                          o.TotalNetAmount,
                          o.IsProcessedAll,
                          o.IsSupervisedAll,
                          o.IsQCAll,
                          o.IsQCSampleAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_PurchaseNoteDetails.Count(),
                          TotalPending = o.tbl_Inv_PurchaseNoteDetails.Count(c => c.FK_tbl_Qc_ActionType_ID == 1),
                          TotalAproved = o.tbl_Inv_PurchaseNoteDetails.Count(c => c.FK_tbl_Qc_ActionType_ID == 2),
                          TotalRejected = o.tbl_Inv_PurchaseNoteDetails.Count(c => c.FK_tbl_Qc_ActionType_ID == 3)
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseNoteMaster(tbl_Inv_PurchaseNoteMaster tbl_Inv_PurchaseNoteMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            
            if (operation == "Save New")
            {
                tbl_Inv_PurchaseNoteMaster.CreatedBy = userName;
                tbl_Inv_PurchaseNoteMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseNoteMaster.ModifiedBy = userName;
                tbl_Inv_PurchaseNoteMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
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
        public async Task<object> GetPurchaseNoteDetail(int id)
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
                          o.MfgBatchNo,
                          MfgDate = o.MfgDate.HasValue ? o.MfgDate.Value.ToString("dd-MMM-yyyy") : null,
                          ExpiryDate = o.ExpiryDate.HasValue ? o.ExpiryDate.Value.ToString("dd-MMM-yyyy") : null,
                          o.Remarks,
                          o.ReferenceNo,
                          o.FK_tbl_Inv_PurchaseOrderDetail_ID,
                          FK_tbl_Inv_PurchaseOrderDetail_IDName = o?.tbl_Inv_PurchaseOrderDetail?.tbl_Inv_PurchaseOrderMaster.PONo.ToString() ?? "",
                          o.NoOfContainers,
                          o.PotencyPercentage,
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

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseNoteDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBPurchaseNoteDetail()
        {
            return new[]
            {
                new { n = "by QC Rejected", v = "byQCRejected" }, new { n = "by QC Pending", v = "byQCPending" }, new { n = "by QC Approved", v = "byQCApproved" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseNoteDetails
                                               .Where(w => w.FK_tbl_Inv_PurchaseNoteMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                                .Where(w =>
                                                        string.IsNullOrEmpty(FilterByLoad)
                                                        ||
                                                        FilterByLoad == "byQCRejected" && w.FK_tbl_Qc_ActionType_ID == 3
                                                        ||
                                                        FilterByLoad == "byQCPending" && w.FK_tbl_Qc_ActionType_ID == 1
                                                        ||
                                                        FilterByLoad == "byQCApproved" && w.FK_tbl_Qc_ActionType_ID == 2
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
                                   .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byQCRejected" && w.FK_tbl_Qc_ActionType_ID == 3
                                            ||
                                            FilterByLoad == "byQCPending" && w.FK_tbl_Qc_ActionType_ID == 1
                                            ||
                                            FilterByLoad == "byQCApproved" && w.FK_tbl_Qc_ActionType_ID == 2
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
                          o.MfgBatchNo,
                          MfgDate = o.MfgDate.HasValue ? o.MfgDate.Value.ToString("dd-MMM-yyyy") : null,
                          ExpiryDate = o.ExpiryDate.HasValue ? o.ExpiryDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.Remarks,
                          o.ReferenceNo,
                          o.FK_tbl_Inv_PurchaseOrderDetail_ID,
                          FK_tbl_Inv_PurchaseOrderDetail_IDName = o?.tbl_Inv_PurchaseOrderDetail?.tbl_Inv_PurchaseOrderMaster.PONo.ToString() ?? "",
                          o.NoOfContainers,
                          o.PotencyPercentage,
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
        public async Task<string> PostPurchaseNoteDetail(tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {  
                tbl_Inv_PurchaseNoteDetail.CreatedBy = userName;
                tbl_Inv_PurchaseNoteDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseNoteDetail.ModifiedBy = userName;
                tbl_Inv_PurchaseNoteDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
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
        public async Task<string> PurchaseNoteDetailUploadExcelFile(List<PurchaseNoteExcelData> PurchaseNoteExcelDataList, int MasterID, string operation, string userName)
        {
            string RespondMsg = "";
            if (operation == "Save New")
            {
                
                int? PODID = null;
                //------------Add compiled record to database--------------------//
                SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
                SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
                SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                int InsertedDetailID = 0;

                foreach (var item in PurchaseNoteExcelDataList)
                {
                    var temp = await db.tbl_Inv_ProductRegistrationDetails.Where(w => w.ProductCode == item.ProductCode).FirstOrDefaultAsync();
                    
                    if (string.IsNullOrEmpty(item.ProductCode) || !(item.Quantity > 0))
                    {
                        continue;
                    }

                    var POD = await db.tbl_Inv_PurchaseOrderDetails.Where(w => w.tbl_Inv_PurchaseOrderMaster.PONo == item.PONo && w.FK_tbl_Inv_ProductRegistrationDetail_ID == temp.ID).FirstOrDefaultAsync();
                    if (POD != null)
                        PODID = POD.ID;
                    else
                        PODID = null;

                    if (item.Quantity > 0 && temp.ID > 0)
                    {
                        CRUD_Type.Value = "Insert";
                        CRUD_ID.Value = 0;
                        CRUD_Msg.Value = "";
                        InsertedDetailID = 0;
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
                        0, MasterID, temp.ID,
                        item.Quantity, item.Rate, 0,
                        item.GSTPercentage, 0, 0, 0, 0,
                        item.WHTPercentage, 0, 0,
                        item.MfgBatchNo, null, item.ExpiryDate, "excel", null, PODID,
                        null,0,
                        userName, DateTime.Now, null, null,
                        1, 0,null,
                        null, null, null, null
                        );

                        if ((string)CRUD_Msg.Value != "Successful")
                            RespondMsg = RespondMsg + "\n" + "On Adding Record For: " + item.ProductCode + ", Exception: " + (string)CRUD_Msg.Value;
                        else
                        {
                            InsertedDetailID = (int)CRUD_ID.Value;

                            CRUD_Type.Value = "InsertByAc";
                            CRUD_Msg.Value = "";
                            var pnd = await db.tbl_Inv_PurchaseNoteDetails.Where(w => w.ID == InsertedDetailID).FirstOrDefaultAsync();

                            if (pnd != null)
                            {
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
                                pnd.ID, pnd.FK_tbl_Inv_PurchaseNoteMaster_ID, pnd.FK_tbl_Inv_ProductRegistrationDetail_ID,
                                pnd.Quantity, pnd.Rate, pnd.GrossAmount,
                                pnd.GSTPercentage, pnd.GSTAmount, pnd.FreightIn, pnd.DiscountAmount, pnd.CostAmount,
                                pnd.WHTPercentage, pnd.WHTAmount, pnd.NetAmount,
                                pnd.MfgBatchNo, pnd.MfgDate, pnd.ExpiryDate, pnd.Remarks, pnd.ReferenceNo, pnd.FK_tbl_Inv_PurchaseOrderDetail_ID,
                                pnd.NoOfContainers, pnd.PotencyPercentage,
                                pnd.CreatedBy, pnd.CreatedDate, pnd.ModifiedBy, pnd.ModifiedDate,
                                pnd.FK_tbl_Qc_ActionType_ID, pnd.QuantitySample,pnd.RetestDate,
                                pnd.CreatedByQcQa, pnd.CreatedDateQcQa, pnd.ModifiedByQcQa, pnd.ModifiedDateQcQa
                                );

                                if ((string)CRUD_Msg.Value != "Successful")
                                    RespondMsg = RespondMsg + "\n" + "On Applying Rate For: " + item.ProductCode + ", Exception: " + (string)CRUD_Msg.Value;
                            }
                            

                            


                        }

                    }
         

                }             

            }
            else
            {
                RespondMsg = "Wrong Operation";
            }
            if (string.IsNullOrEmpty(RespondMsg))
                return "OK";
            else
                return RespondMsg;
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
        
        public List<ReportCallingModel> GetRLPurchaseNoteMaster()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Purchase Note",
                    GroupBy = new List<string>(){ "WareHouse", "Supplier", "Product Name" },
                    OrderBy = new List<string>(){ "Doc Date", "Product Name" },
                    SeekBy = null
                },
                 new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Pending Purchase Order",
                    GroupBy = new List<string>(){ "Supplier", "Product" },
                    OrderBy = new List<string>(){ "Doc Date", "Doc No" },
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLPurchaseNoteDetail()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="QC Intimation Slip",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="QC Intimation Slip Singular",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Purchase Note",
                    GroupBy = null,
                    OrderBy = null,
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
            else if (rn == "QC Intimation Slip")
            {
                return await Task.Run(() => QCIntimationSlip(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "QC Intimation Slip Singular")
            {
                return await Task.Run(() => QCIntimationSlipSingular(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Purchase Note")
            {
                return await Task.Run(() => RegisterPurchaseNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Pending Purchase Order")
            {
                return await Task.Run(() => PendingPurchaseOrder(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> PendingPurchaseOrder(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Pending Purchase Order Register " + "-----", false);
            /////////////------------------------------table for Detail 9------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//PONo
                        (float)(PageSize.A4.GetWidth() * 0.05),//PODate 
                        (float)(PageSize.A4.GetWidth() * 0.05),//TargetDate 
                        (float)(PageSize.A4.GetWidth() * 0.25),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.25),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.10),//Receive Qty 
                        (float)(PageSize.A4.GetWidth() * 0.10) //Balance
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("PO No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("PO Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Target Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Order Qty")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Received Qty")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Balance Qty")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalBalQty = 0, GroupTotalBalQty = 0, BalQty = 0;

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
                string GroupbyFieldName = GroupBy == "Supplier" ? "AccountName" :
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
                                pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalBalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalBalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PONo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PODate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["TargetDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReceivedQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        BalQty = Math.Round(Convert.ToDouble(sqlReader["Quantity"]) - Convert.ToDouble(sqlReader["ReceivedQty"]),3);
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(BalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));;

                        GroupTotalBalQty += BalQty;
                        GrandTotalBalQty += BalQty;

                        SNo++;
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalBalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalBalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> QCIntimationSlip(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- QC Intimation Slip Of Purchase Note -----", true);

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
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold().SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Challan #")).SetBold().SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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

                /////////////------------------------------table for detail 7------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //Detail ID
                        (float)(PageSize.A4.GetWidth() * 0.30), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity                        
                        (float)(PageSize.A4.GetWidth() * 0.10),  //MfgBatchNo
                        (float)(PageSize.A4.GetWidth() * 0.10),  //ExpiryDate
                        (float)(PageSize.A4.GetWidth() * 0.15),  //ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.10)  //Action
                }
                ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Intimated Material Information")).SetFontSize(10).SetBold().SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("PND No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Supplier Batch#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Expiry Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("QC Ref#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("QC Decision")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ID"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBackgroundColor(new DeviceRgb(144, 180, 210)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MfgBatchNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(!sqlReader.IsDBNull("ExpiryDate") ? ((DateTime)sqlReader["ExpiryDate"]).ToString("dd-MMM-yy") : "")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ActionName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Note: " + sqlReader["Remarks"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Received By QC:")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> QCIntimationSlipSingular(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "", true,false,false);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
               
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

                string CreatedBy = ""; string DocNo = ""; string DocDate = ""; string WareHouse = ""; string Supplier = ""; string ChallanNo = "";

                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        DocNo = sqlReader["DocNo"].ToString();
                        DocDate = ((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy");
                        WareHouse = sqlReader["WareHouseName"].ToString();
                        Supplier = sqlReader["AccountName"].ToString();
                        ChallanNo = sqlReader["SupplierChallanNo"].ToString();
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }

                /////////////------------------------------table for detail 8------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.11), //Detail ID
                        (float)(PageSize.A4.GetWidth() * 0.15), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity                        
                        (float)(PageSize.A4.GetWidth() * 0.11),  //MfgBatchNo
                        (float)(PageSize.A4.GetWidth() * 0.11),  //ExpiryDate
                        (float)(PageSize.A4.GetWidth() * 0.11),  //ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.11)  //Action
                }
                ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);
                              
                ReportName.Value = rn + "2";
                int SNo = 1;

                Table t = page.GetHearderTable();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell(1, 8).Add(t).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Good Receive Intimation Slip for QC")).SetFontSize(10).SetBold().SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("PN#")).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(DocNo)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Supplier Name")).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add(Supplier)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Sup Doc#")).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(ChallanNo)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell(1, 8).Add(new Paragraph().Add("")).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("PND No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Product Name")).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Supplier Batch#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Expiry Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("QC Ref#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("QC Decision")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ID"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MfgBatchNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(!sqlReader.IsDBNull("ExpiryDate") ? ((DateTime)sqlReader["ExpiryDate"]).ToString("dd-MMM-yy") : "")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Remarks:")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 7).Add(new Paragraph().Add(sqlReader["Remarks"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell(1, 8).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        
                        pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 4).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Received By QC:")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));

                        pdftableDetail.AddCell(new Cell(1, 8).Add(new Paragraph().Add("\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 8).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorderBottom(new DottedBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell(1, 8).Add(new Paragraph().Add("\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        SNo++;

                    }
                }

                page.InsertContent(pdftableDetail);

            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> CurrentPurchaseNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Purchase Note Challan -----", true);

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

                /////////////------------------------------table for detail 7------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //S No
                        (float)(PageSize.A4.GetWidth() * 0.35), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),  //MeasurementUnit
                        (float)(PageSize.A4.GetWidth() * 0.10),  //MfgBatchNo
                        (float)(PageSize.A4.GetWidth() * 0.10),  //ExpiryDate
                        (float)(PageSize.A4.GetWidth() * 0.15)  //ReferenceNo
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Mfg Batch#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Expiry Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;
 
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MfgBatchNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(!sqlReader.IsDBNull("ExpiryDate") ? ((DateTime)sqlReader["ExpiryDate"]).ToString("dd-MMM-yy") : "")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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
        private async Task<byte[]> RegisterPurchaseNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Purchase Note Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 11------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.15),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.15),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.20),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//MeasurementUnit 
                        (float)(PageSize.A4.GetWidth() * 0.05),//MfgBatchNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//ExpiryDate
                        (float)(PageSize.A4.GetWidth() * 0.10)//ReferenceNo
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
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Mfg Batch#")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Expiry Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("ReferenceNo")).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

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
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 13).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 13).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MfgBatchNo"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(!sqlReader.IsDBNull("ExpiryDate") ? ((DateTime)sqlReader["ExpiryDate"]).ToString("dd-MMM-yy") : "")).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        
        #endregion
    }
    public class PurchaseReturnNoteRepository : IPurchaseReturnNote
    {
        private readonly OreasDbContext db;
        public PurchaseReturnNoteRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetPurchaseReturnNoteMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseReturnNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          o.DocDate,
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_PurchaseReturnNoteDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseReturnNoteMaster()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }, new { n = "by DocNo", v = "byDocNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseReturnNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseReturnNoteMasters
                                      .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_PurchaseReturnNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseReturnNoteMasters
                                  .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_PurchaseReturnNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_PurchaseReturnNoteDetails.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseReturnNoteMaster(tbl_Inv_PurchaseReturnNoteMaster tbl_Inv_PurchaseReturnNoteMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PurchaseReturnNoteMaster.CreatedBy = userName;
                tbl_Inv_PurchaseReturnNoteMaster.CreatedDate = DateTime.Now;
                //db.tbl_Inv_PurchaseReturnNoteMasters.Add(tbl_Inv_PurchaseReturnNoteMaster);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseReturnNoteMaster.ModifiedBy = userName;
                tbl_Inv_PurchaseReturnNoteMaster.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Inv_PurchaseReturnNoteMaster).Property(x => x.DocDate).IsModified = true;
                //db.Entry(tbl_Inv_PurchaseReturnNoteMaster).Property(x => x.FK_tbl_Inv_WareHouseMaster_ID).IsModified = true;
                //db.Entry(tbl_Inv_PurchaseReturnNoteMaster).Property(x => x.FK_tbl_Ac_ChartOfAccounts_ID).IsModified = true;
                //db.Entry(tbl_Inv_PurchaseReturnNoteMaster).Property(x => x.Remarks).IsModified = true;
                //db.Entry(tbl_Inv_PurchaseReturnNoteMaster).Property(x => x.ModifiedBy).IsModified = true;
                //db.Entry(tbl_Inv_PurchaseReturnNoteMaster).Property(x => x.ModifiedDate).IsModified = true;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Inv_PurchaseReturnNoteMasters.Remove(db.tbl_Inv_PurchaseReturnNoteMasters.Find(tbl_Inv_PurchaseReturnNoteMaster.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseReturnNoteMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
                @ID={3},@DocNo={4},@DocDate={5},
                @FK_tbl_Inv_WareHouseMaster_ID={6},@FK_tbl_Ac_ChartOfAccounts_ID={7},@Remarks={8},
                @CreatedBy={9},@CreatedDate={10},@ModifiedBy={11},@ModifiedDate={12}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseReturnNoteMaster.ID, tbl_Inv_PurchaseReturnNoteMaster.DocNo, tbl_Inv_PurchaseReturnNoteMaster.DocDate,
                tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Inv_WareHouseMaster_ID, tbl_Inv_PurchaseReturnNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Inv_PurchaseReturnNoteMaster.Remarks,
                tbl_Inv_PurchaseReturnNoteMaster.CreatedBy, tbl_Inv_PurchaseReturnNoteMaster.CreatedDate, tbl_Inv_PurchaseReturnNoteMaster.ModifiedBy, tbl_Inv_PurchaseReturnNoteMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetPurchaseReturnNoteDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseReturnNoteDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseReturnNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          MeasurementUnit = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNoName = o.tbl_Inv_PurchaseNoteDetail_RefNo?.ReferenceNo ?? "",
                          o.Quantity,
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          FK_tbl_Inv_PurchaseNoteDetail_IDName = o.tbl_Inv_PurchaseNoteDetail_RefNo.tbl_Inv_PurchaseNoteMaster.DocNo
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseReturnNoteDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseReturnNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
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
                          MeasurementUnit = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNoName = o.tbl_Inv_PurchaseNoteDetail_RefNo?.ReferenceNo ?? "",
                          o.Quantity,
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          FK_tbl_Inv_PurchaseNoteDetail_IDName = o.tbl_Inv_PurchaseNoteDetail_RefNo.tbl_Inv_PurchaseNoteMaster.DocNo
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseReturnNoteDetail(tbl_Inv_PurchaseReturnNoteDetail tbl_Inv_PurchaseReturnNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PurchaseReturnNoteDetail.CreatedBy = userName;
                tbl_Inv_PurchaseReturnNoteDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseReturnNoteDetail.ModifiedBy = userName;
                tbl_Inv_PurchaseReturnNoteDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
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
                    ReportName ="Register Purchase Return Note",
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
            else if (rn == "Register Purchase Return Note")
            {
                return await Task.Run(() => RegisterPurchaseReturnNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentPurchaseReturnNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Purchase Return Note Challan -----", true);

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

                /////////////------------------------------table for detail 5------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //S No
                        (float)(PageSize.A4.GetWidth() * 0.10),  //ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.60), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.10)  //MeasurementUnit
                        
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference #")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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
        private async Task<byte[]> RegisterPurchaseReturnNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Purchase Return Note Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 9------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.15),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.20),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.25),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//MeasurementUnit 
                        (float)(PageSize.A4.GetWidth() * 0.10)//ReferenceNo
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
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("ReferenceNo")).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

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
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
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
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion
    }
    public class SalesNoteRepository : ISalesNote
    {
        private readonly OreasDbContext db;
        private readonly IOrderNote iOrderNote;

        public SalesNoteRepository(OreasDbContext oreasDbContext, IOrderNote _iOrderNote)
        {
            this.db = oreasDbContext;
            this.iOrderNote = _iOrderNote;
        }

        #region Master
        public async Task<object> GetSalesNoteMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_SalesNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          o.DocDate,
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
                          o.TransporterBiltyNo,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_SalesNoteDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSalesNoteMaster()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }, new { n = "by Account Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }, 
                new { n = "by Mfg ReferenceNo", v = "byMfgReferenceNo" }, new { n = "by PN ReferenceNo", v = "byPNReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesNoteMasters
                                      .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byMfgReferenceNo" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byPNReferenceNo" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesNoteMasters
                                        .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                      .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                            ||
                                            FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                            ||
                                            FilterByText == "byReferenceNo" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                            ||
                                            FilterByText == "byMfgReferenceNo" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                            ||
                                            FilterByText == "byPNReferenceNo" && w.tbl_Inv_SalesNoteDetails.Any(a => a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()))
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
                          FK_tbl_Ac_ChartOfAccounts_ID_TransporterName = o.FK_tbl_Ac_ChartOfAccounts_ID_Transporter.HasValue ? o.tbl_Ac_ChartOfAccounts_Transporter.AccountName : "",
                          o.TransporterBiltyNo,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_SalesNoteDetails.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostSalesNoteMaster(tbl_Inv_SalesNoteMaster tbl_Inv_SalesNoteMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_SalesNoteMaster.CreatedBy = userName;
                tbl_Inv_SalesNoteMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_SalesNoteMaster.ModifiedBy = userName;
                tbl_Inv_SalesNoteMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
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
        public async Task<object> GetSalesNoteDetail(int id)
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
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.NoOfPackages,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Inv_OrderNoteDetail_ID,
                          FK_tbl_Inv_OrderNoteDetail_IDName = o?.tbl_Inv_OrderNoteDetail?.tbl_Inv_OrderNoteMaster.DocNo.ToString() ?? ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSalesNoteDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" },  new { n = "by Mfg ReferenceNo", v = "byMfgReferenceNo" }, new { n = "by PN ReferenceNo", v = "byPNReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesNoteDetails
                                               .Where(w => w.FK_tbl_Inv_SalesNoteMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byMfgReferenceNo" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byPNReferenceNo" && w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower())
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
                                        ||
                                        FilterByText == "byMfgReferenceNo" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byPNReferenceNo" && w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower())
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
                          o.Remarks,
                          o.IsProcessed,
                          o.IsSupervised,
                          o.NoOfPackages,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Inv_OrderNoteDetail_ID,
                          FK_tbl_Inv_OrderNoteDetail_IDName = o?.tbl_Inv_OrderNoteDetail?.tbl_Inv_OrderNoteMaster.DocNo.ToString() ?? ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostSalesNoteDetail(tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_SalesNoteDetail.CreatedBy = userName;
                tbl_Inv_SalesNoteDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_SalesNoteDetail.ModifiedBy = userName;
                tbl_Inv_SalesNoteDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
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
            tbl_Inv_SalesNoteDetail.STPercentage, tbl_Inv_SalesNoteDetail.STAmount, tbl_Inv_SalesNoteDetail.FSTPercentage, tbl_Inv_SalesNoteDetail.FSTAmount,
            tbl_Inv_SalesNoteDetail.WHTPercentage, tbl_Inv_SalesNoteDetail.WHTAmount, 
            tbl_Inv_SalesNoteDetail.DiscountAmount, tbl_Inv_SalesNoteDetail.NetAmount, tbl_Inv_SalesNoteDetail.Remarks, tbl_Inv_SalesNoteDetail.NoOfPackages,
            tbl_Inv_SalesNoteDetail.CreatedBy, tbl_Inv_SalesNoteDetail.CreatedDate, tbl_Inv_SalesNoteDetail.ModifiedBy, tbl_Inv_SalesNoteDetail.ModifiedDate, tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_ID);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region DetailReturn
        public async Task<PagedData<object>> LoadSalesNoteDetailReturn(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
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
                          o.Remarks
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Report  
        public List<ReportCallingModel> GetRLSalesNoteMaster()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Sales Note",
                    GroupBy = new List<string>(){ "WareHouse", "Supplier", "Product Name" },
                    OrderBy = new List<string>(){ "Doc Date", "Product Name" },
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Mfg Stock On Order Note",
                    GroupBy = new List<string>(){ "Customer", "Product" },
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLSalesNoteDetail()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Sales Note",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Gate Pass",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Sales Note",
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
            else if (rn == "Gate Pass")
            {
                return await Task.Run(() => SalesNoteGatePass(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, "Current Sales Note", GroupID, userName));
            }
            else if (rn == "Register Sales Note")
            {
                return await Task.Run(() => RegisterSalesNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Mfg Stock On Order Note")
            {
                return await iOrderNote.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, GroupID, userName);
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> SalesNoteGatePass(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Gate Pass Of Sales -----", true);

            var ColorSteelBlue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorLightSteelBlue = new MyDeviceRgb(MyColor.LightSteelBlue).color;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 6------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //
                        (float)(PageSize.A4.GetWidth() * 0.50), //
                        (float)(PageSize.A4.GetWidth() * 0.10), //
                        (float)(PageSize.A4.GetWidth() * 0.10), //
                        (float)(PageSize.A4.GetWidth() * 0.10), //
                        (float)(PageSize.A4.GetWidth() * 0.10)  //
                }
                ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

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
                pdftableMaster.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Please allow the following Materials")).SetBorder(Border.NO_BORDER).SetFontSize(10).SetBold().SetTextAlignment(TextAlignment.CENTER).SetKeepTogether(true));
                 using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("To")).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Address")).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell(1,5).Add(new Paragraph().Add(sqlReader["Address"].ToString())).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Transporter")).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TransporterName"].ToString())).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Tran Bilty#")).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TransporterBiltyNo"].ToString())).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Tran Tel #")).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TransporterTelephone"].ToString())).SetKeepTogether(true));


                        pdftableMaster.AddCell(new Cell(1, 6).Add(new Paragraph().Add("\n")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
               
                page.InsertContent(pdftableMaster);
                double TotalNoOfPackages = 0; int SNo = 1;
                /////////////------------------------------table for detail 5------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), // Sno
                        (float)(PageSize.A4.GetWidth() * 0.50), // ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15), // ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.15), // Quantity
                        (float)(PageSize.A4.GetWidth() * 0.10) // NoOfPackages
                        
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Batch #")).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("# Packages")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBackgroundColor(ColorSteelBlue).SetFontColor(ColorWhite).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

 
                ReportName.Value = rn + "2";

   
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["NoOfPackages"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                        SNo++;
                        TotalNoOfPackages += Convert.ToDouble(sqlReader["NoOfPackages"]);
                    }
                }

                pdftableDetail.AddCell(new Cell(1, 3).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Total Packages")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", TotalNoOfPackages))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Issued By: " + userName + "\n" +DateTime.Now.ToString("dd-MM-yy hh:mm tt"))).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> CurrentSalesNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Sales Note Challan -----", true);

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

                /////////////------------------------------table for detail 5------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //S No
                        (float)(PageSize.A4.GetWidth() * 0.10),  //ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.60), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.10)  //MeasurementUnit
                        
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference #")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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
        private async Task<byte[]> RegisterSalesNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Sales Note Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 9------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.15),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.20),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.25),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//MeasurementUnit 
                        (float)(PageSize.A4.GetWidth() * 0.10)//ReferenceNo
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
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("ReferenceNo")).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

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
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
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
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion

    }
    public class SalesReturnNoteRepository : ISalesReturnNote
    {
        private readonly OreasDbContext db;
        public SalesReturnNoteRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetSalesReturnNoteMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_SalesReturnNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          o.DocDate,
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
                          o.TransporterBiltyNo,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_SalesReturnNoteDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSalesReturnNoteMaster()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }, new { n = "by Account Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesReturnNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesReturnNoteMasters
                                      .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_SalesReturnNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesReturnNoteMasters
                                  .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_SalesReturnNoteDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
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
                          o.TransporterBiltyNo,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_SalesReturnNoteDetails.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostSalesReturnNoteMaster(tbl_Inv_SalesReturnNoteMaster tbl_Inv_SalesReturnNoteMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_SalesReturnNoteMaster.CreatedBy = userName;
                tbl_Inv_SalesReturnNoteMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_SalesReturnNoteMaster.ModifiedBy = userName;
                tbl_Inv_SalesReturnNoteMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
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
        }
        #endregion

        #region Detail
        public async Task<object> GetSalesReturnNoteDetail(int id)
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
        public object GetWCLSalesReturnNoteDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesReturnNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
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
        public async Task<string> PostSalesReturnNoteDetail(tbl_Inv_SalesReturnNoteDetail tbl_Inv_SalesReturnNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            { 
                tbl_Inv_SalesReturnNoteDetail.CreatedBy = userName;
                tbl_Inv_SalesReturnNoteDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_SalesReturnNoteDetail.ModifiedBy = userName;
                tbl_Inv_SalesReturnNoteDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
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
                    ReportName ="Register Sales Return Note",
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
            else if (rn == "Register Sales Return Note")
            {
                return await Task.Run(() => RegisterSalesReturnNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentSalesReturnNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Sales Return Note Challan -----", true);

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

                /////////////------------------------------table for detail 5------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //S No
                        (float)(PageSize.A4.GetWidth() * 0.10),  //ReferenceNo
                        (float)(PageSize.A4.GetWidth() * 0.60), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.10)  //MeasurementUnit
                        
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference #")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Unit")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";

                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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
        private async Task<byte[]> RegisterSalesReturnNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Sales Return Note Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 9------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.15),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.20),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.25),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//MeasurementUnit 
                        (float)(PageSize.A4.GetWidth() * 0.10)//ReferenceNo
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
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("ReferenceNo")).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

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
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
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
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion
    }
    public class OrdinaryRequisitionRepository : IOrdinaryRequisition
    {
        private readonly OreasDbContext db;
        public OrdinaryRequisitionRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region OrdinaryRequisitionMaster
        public async Task<object> GetOrdinaryRequisitionMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_WPT_DepartmentDetail_Section_ID,
                          FK_tbl_WPT_DepartmentDetail_Section_IDName = o.tbl_WPT_DepartmentDetail_Section.SectionName + " [" + o.tbl_WPT_DepartmentDetail_Section.tbl_WPT_Department.DepartmentName + "]",
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_OrdinaryRequisitionDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLOrdinaryRequisitionMaster()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }, new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBOrdinaryRequisitionMaster()
        {
            return new[]
            {
                new { n = "by Dispensing Pending", v = "byDispensingPending" }, new { n = "by Dispensing Completed", v = "byDispensingCompleted" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrdinaryRequisitionMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "", bool IsCanViewOnlyOwnData = false)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrdinaryRequisitionMasters
                                      .Where(w=> 
                                                w.tbl_WPT_DepartmentDetail_Section.AspNetOreasAuthorizationScheme_Sections
                                                 .Any(w=> w.AspNetOreasAuthorizationScheme.ApplicationUsers.Any(a=> a.UserName == userName)))
                                      .Where(w =>
                                                string.IsNullOrEmpty(FilterValueByText)
                                                ||
                                                FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                ||
                                                FilterByText == "byProductName" && w.tbl_Inv_OrdinaryRequisitionDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                )
                                      .Where(w=>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byDispensingPending" && w.IsDispensedAll == false
                                                ||
                                                FilterByLoad == "byDispensingCompleted" && w.IsDispensedAll == true
                                                )
                                      .Where(w =>
                                                IsCanViewOnlyOwnData == true && w.CreatedBy.Equals(userName)
                                                ||
                                                IsCanViewOnlyOwnData == false
                                                )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionMasters
                                        .Where(w =>
                                                w.tbl_WPT_DepartmentDetail_Section.AspNetOreasAuthorizationScheme_Sections
                                                 .Any(w => w.AspNetOreasAuthorizationScheme.ApplicationUsers.Any(a => a.UserName == userName)))
                                        .Where(w =>
                                                    string.IsNullOrEmpty(FilterValueByText)
                                                    ||
                                                    FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                    ||
                                                    FilterByText == "byProductName" && w.tbl_Inv_OrdinaryRequisitionDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                    )
                                        .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byDispensingPending" && w.IsDispensedAll == false
                                                ||
                                                FilterByLoad == "byDispensingCompleted" && w.IsDispensedAll == true
                                                )
                                        .Where(w =>
                                                    IsCanViewOnlyOwnData == true && w.CreatedBy.Equals(userName)
                                                    ||
                                                    IsCanViewOnlyOwnData == false
                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString("dd-MMM-yy"),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_WPT_DepartmentDetail_Section_ID,
                          FK_tbl_WPT_DepartmentDetail_Section_IDName = o.tbl_WPT_DepartmentDetail_Section.SectionName + " [" + o.tbl_WPT_DepartmentDetail_Section.tbl_WPT_Department.DepartmentName + "]",
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""

                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostOrdinaryRequisitionMaster(tbl_Inv_OrdinaryRequisitionMaster tbl_Inv_OrdinaryRequisitionMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_OrdinaryRequisitionMaster.CreatedBy = userName;
                tbl_Inv_OrdinaryRequisitionMaster.CreatedDate = DateTime.Now;
                tbl_Inv_OrdinaryRequisitionMaster.IsDispensedAll = false;
                CRUD_Type.Value = "Insert";

            }
            else if (operation == "Save Update")
            {
                tbl_Inv_OrdinaryRequisitionMaster.ModifiedBy = userName;
                tbl_Inv_OrdinaryRequisitionMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_OrdinaryRequisitionMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT,
                @ID={3},@DocNo={4},@DocDate={5},
                @FK_tbl_Inv_WareHouseMaster_ID={6},@FK_tbl_WPT_DepartmentDetail_Section_ID={7},
                @CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_OrdinaryRequisitionMaster.ID, tbl_Inv_OrdinaryRequisitionMaster.DocNo, tbl_Inv_OrdinaryRequisitionMaster.DocDate,
                tbl_Inv_OrdinaryRequisitionMaster.FK_tbl_Inv_WareHouseMaster_ID, tbl_Inv_OrdinaryRequisitionMaster.FK_tbl_WPT_DepartmentDetail_Section_ID,
                tbl_Inv_OrdinaryRequisitionMaster.CreatedBy, tbl_Inv_OrdinaryRequisitionMaster.CreatedDate, tbl_Inv_OrdinaryRequisitionMaster.ModifiedBy, tbl_Inv_OrdinaryRequisitionMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }
        #endregion

        #region OrdinaryRequisitionDetail
        public async Task<object> GetOrdinaryRequisitionDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionMaster_ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionType_ID,
                          FK_tbl_Inv_OrdinaryRequisitionType_IDName = o.tbl_Inv_OrdinaryRequisitionType.TypeName,
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          DispenseQty = o.tbl_Inv_OrdinaryRequisitionDispensings?.Sum(s=> s.Quantity) ?? 0

                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLOrdinaryRequisitionDetail()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrdinaryRequisitionDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrdinaryRequisitionDetails
                                      .Where(w => w.FK_tbl_Inv_OrdinaryRequisitionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionDetails
                                        .Where(w => w.FK_tbl_Inv_OrdinaryRequisitionMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionMaster_ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionType_ID,
                          FK_tbl_Inv_OrdinaryRequisitionType_IDName = o.tbl_Inv_OrdinaryRequisitionType.TypeName,
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
        public async Task<string> PostOrdinaryRequisitionDetail(tbl_Inv_OrdinaryRequisitionDetail tbl_Inv_OrdinaryRequisitionDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_OrdinaryRequisitionDetail.CreatedBy = userName;
                tbl_Inv_OrdinaryRequisitionDetail.CreatedDate = DateTime.Now;
                tbl_Inv_OrdinaryRequisitionDetail.IsDispensed = false;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_OrdinaryRequisitionDetail.ModifiedBy = userName;
                tbl_Inv_OrdinaryRequisitionDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE[dbo].[OP_Inv_OrdinaryRequisitionDetail] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Inv_OrdinaryRequisitionMaster_ID={4}
                  ,@FK_tbl_Inv_OrdinaryRequisitionType_ID={5} ,@RequiredTrue_ReturnFalse={6}
                  ,@FK_tbl_Inv_ProductRegistrationDetail_ID={7}
                  ,@Quantity={8},@Remarks={9},@IsDispensed={10}
                  ,@CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_OrdinaryRequisitionDetail.ID, tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_OrdinaryRequisitionMaster_ID,
                tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_OrdinaryRequisitionType_ID, tbl_Inv_OrdinaryRequisitionDetail.RequiredTrue_ReturnFalse,
                tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Inv_OrdinaryRequisitionDetail.Quantity, tbl_Inv_OrdinaryRequisitionDetail.Remarks, tbl_Inv_OrdinaryRequisitionDetail.IsDispensed,
                tbl_Inv_OrdinaryRequisitionDetail.CreatedBy, tbl_Inv_OrdinaryRequisitionDetail.CreatedDate, tbl_Inv_OrdinaryRequisitionDetail.ModifiedBy, tbl_Inv_OrdinaryRequisitionDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        
        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLOrdinaryRequisition()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Ordinary Requisition",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Ordinary Requisition",
                    GroupBy = new List<string>(){ "WareHouse", "Product" },
                    OrderBy = new List<string>(){ "Doc Date", "Doc No" },
                    SeekBy = null,
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Ordinary Requisition")
            {
                return await Task.Run(() => CurrentOrdinaryRequisition(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Ordinary Requisition")
            {
                return await Task.Run(() => RegisterOrdinaryRequisition(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentOrdinaryRequisition(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Ordinary Requisition -----", true);

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

                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                /////////////------------------------------table for detail 5------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //S No
                        (float)(PageSize.A4.GetWidth() * 0.15),  //TypeName
                        (float)(PageSize.A4.GetWidth() * 0.10),  //RequiredTrue_ReturnFalse
                        (float)(PageSize.A4.GetWidth() * 0.50), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15)  //Quantity
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Req / Ret")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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
        private async Task<byte[]> RegisterOrdinaryRequisition(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Ordinary Requisition Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);

            /////////////------------------------------table for Detail 8------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.08),//S No
                        (float)(PageSize.A4.GetWidth() * 0.08),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.08),//DocDate  
                        (float)(PageSize.A4.GetWidth() * 0.10),//Type
                        (float)(PageSize.A4.GetWidth() * 0.20),//WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.08),//Req Ret
                        (float)(PageSize.A4.GetWidth() * 0.28),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10) //Quantity
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Type")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Req / Ret")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                                pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["TypeName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequiredTrue_ReturnFalse"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        SNo++;
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }

        #endregion

    }
    public class OrdinaryRequisitionDispensingRepository : IOrdinaryRequisitionDispensing
    {
        private readonly OreasDbContext db;
        public OrdinaryRequisitionDispensingRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region OrdinaryRequisitionDispensingMaster
        public async Task<object> GetOrdinaryRequisitionDispensingMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_WPT_DepartmentDetail_Section_ID,
                          FK_tbl_WPT_DepartmentDetail_Section_IDName = o.tbl_WPT_DepartmentDetail_Section.SectionName + " [" + o.tbl_WPT_DepartmentDetail_Section.tbl_WPT_Department.DepartmentName + "]",
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLOrdinaryRequisitionDispensingMaster()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }, new { n = "by ProductName", v = "byProductName" }, new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public object GetWCLBOrdinaryRequisitionDispensingMaster()
        {
            return new[]
            {
                new { n = "by Dispensing Pending", v = "byDispensingPending" }, new { n = "by Dispensing Completed", v = "byDispensingCompleted" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrdinaryRequisitionDispensingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrdinaryRequisitionMasters
                                      .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                      .Where(w =>
                                                string.IsNullOrEmpty(FilterValueByText)
                                                ||
                                                FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                ||
                                                FilterByText == "byProductName" && w.tbl_Inv_OrdinaryRequisitionDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                ||
                                                FilterByText == "byReferenceNo" && w.tbl_Inv_OrdinaryRequisitionDetails
                                                                                    .Any(a => 
                                                                                            a.tbl_Inv_OrdinaryRequisitionDispensings
                                                                                                .Any(b=> b.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower())
                                                                                                ||
                                                                                                a.tbl_Inv_OrdinaryRequisitionDispensings
                                                                                                .Any(c=> c.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower())
                                                                                        )
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

            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionMasters
                                        .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                        .Where(w =>
                                                string.IsNullOrEmpty(FilterValueByText)
                                                ||
                                                FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                ||
                                                FilterByText == "byProductName" && w.tbl_Inv_OrdinaryRequisitionDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                ||
                                                FilterByText == "byReferenceNo" && w.tbl_Inv_OrdinaryRequisitionDetails
                                                                                    .Any(a =>
                                                                                            a.tbl_Inv_OrdinaryRequisitionDispensings
                                                                                                .Any(b => b.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower())
                                                                                                ||
                                                                                                a.tbl_Inv_OrdinaryRequisitionDispensings
                                                                                                .Any(c => c.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower())
                                                                                        )
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
                          DocDate = o.DocDate.ToString("dd-MMM-yy"),
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_WPT_DepartmentDetail_Section_ID,
                          FK_tbl_WPT_DepartmentDetail_Section_IDName = o.tbl_WPT_DepartmentDetail_Section.SectionName + " [" + o.tbl_WPT_DepartmentDetail_Section.tbl_WPT_Department.DepartmentName + "]",
                          o.IsDispensedAll,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalItems = o.tbl_Inv_OrdinaryRequisitionDetails.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        #endregion

        #region OrdinaryRequisitionDispensingDetail
        public async Task<object> GetOrdinaryRequisitionDispensingDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionMaster_ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionType_ID,
                          FK_tbl_Inv_OrdinaryRequisitionType_IDName = o.tbl_Inv_OrdinaryRequisitionType.TypeName,
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
        public object GetWCLOrdinaryRequisitionDispensingDetail()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }, new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrdinaryRequisitionDispensingDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrdinaryRequisitionDetails
                                      .Where(w => w.FK_tbl_Inv_OrdinaryRequisitionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byReferenceNo" && w.tbl_Inv_OrdinaryRequisitionDispensings
                                                                                      .Any(a=> 
                                                                                                a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower()
                                                                                                ||
                                                                                                a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower()
                                                                                           )
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionDetails
                                        .Where(w => w.FK_tbl_Inv_OrdinaryRequisitionMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byReferenceNo" && w.tbl_Inv_OrdinaryRequisitionDispensings
                                                                                             .Any(a =>
                                                                                                      a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower()
                                                                                                      ||
                                                                                                      a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower()
                                                                                                   )
                                                               )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionMaster_ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionType_ID,
                          FK_tbl_Inv_OrdinaryRequisitionType_IDName = o.tbl_Inv_OrdinaryRequisitionType.TypeName,
                          o.RequiredTrue_ReturnFalse,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          o.IsDispensed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.IsDecimal,
                          DispensedQty = o.tbl_Inv_OrdinaryRequisitionDispensings.Sum(s=> s.Quantity)
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostOrdinaryRequisitionDispensingDetail(tbl_Inv_OrdinaryRequisitionDetail tbl_Inv_OrdinaryRequisitionDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New" && tbl_Inv_OrdinaryRequisitionDetail.ID > 0)
            {
                tbl_Inv_OrdinaryRequisitionDetail.ModifiedBy = userName;
                tbl_Inv_OrdinaryRequisitionDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "UpdateByDispensing";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE[dbo].[OP_Inv_OrdinaryRequisitionDetail] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Inv_OrdinaryRequisitionMaster_ID={4}
                  ,@FK_tbl_Inv_OrdinaryRequisitionType_ID={5} ,@RequiredTrue_ReturnFalse={6}
                  ,@FK_tbl_Inv_ProductRegistrationDetail_ID={7}
                  ,@Quantity={8},@Remarks={9},@IsDispensed={10}
                  ,@CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_OrdinaryRequisitionDetail.ID, tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_OrdinaryRequisitionMaster_ID,
                tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_OrdinaryRequisitionType_ID, tbl_Inv_OrdinaryRequisitionDetail.RequiredTrue_ReturnFalse,
                tbl_Inv_OrdinaryRequisitionDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Inv_OrdinaryRequisitionDetail.Quantity, tbl_Inv_OrdinaryRequisitionDetail.Remarks, tbl_Inv_OrdinaryRequisitionDetail.IsDispensed,
                tbl_Inv_OrdinaryRequisitionDetail.CreatedBy, tbl_Inv_OrdinaryRequisitionDetail.CreatedDate, tbl_Inv_OrdinaryRequisitionDetail.ModifiedBy, tbl_Inv_OrdinaryRequisitionDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }

        #endregion

        #region OrdinaryRequisitionDispensingDetailDispensing
        public async Task<object> GetOrdinaryRequisitionDispensingDetailDispensing(int id)
        {
            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionDispensings.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionDetail_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString() : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLOrdinaryRequisitionDispensingDetailDispensing()
        {
            return new[]
            {
                new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrdinaryRequisitionDispensingDetailDispensing(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrdinaryRequisitionDispensings
                                      .Where(w => w.FK_tbl_Inv_OrdinaryRequisitionDetail_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byReferenceNo" && (w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()) : w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrdinaryRequisitionDispensings
                                        .Where(w => w.FK_tbl_Inv_OrdinaryRequisitionDetail_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byReferenceNo" && (w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()) : w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrdinaryRequisitionDetail_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString("dd-MMM-yy hh: mm tt") : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostOrdinaryRequisitionDispensingDetailDispensing(tbl_Inv_OrdinaryRequisitionDispensing tbl_Inv_OrdinaryRequisitionDispensing, string operation = "", string userName = "")
        {
            // tbl_Inv_OrdinaryRequisitionDispensing.DispensingDate = DateTime.Now;
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_OrdinaryRequisitionDispensing.CreatedBy = userName;
                tbl_Inv_OrdinaryRequisitionDispensing.CreatedDate = DateTime.Now;
                //db.tbl_Inv_OrdinaryRequisitionDispensings.Add(tbl_Inv_OrdinaryRequisitionDispensing);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_OrdinaryRequisitionDispensing.ModifiedBy = userName;
                tbl_Inv_OrdinaryRequisitionDispensing.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Inv_OrdinaryRequisitionDispensing).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Inv_OrdinaryRequisitionDispensings.Remove(db.tbl_Inv_OrdinaryRequisitionDispensings.Find(tbl_Inv_OrdinaryRequisitionDispensing.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_OrdinaryRequisitionDispensing] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Inv_OrdinaryRequisitionDetail_ID={4}
                  ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={5},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={6}
                  ,@Quantity={7},@DispensingDate={8},@Remarks={9}
                  ,@CreatedBy={10},@CreatedDate={11},@ModifiedBy={12},@ModifiedDate={13}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Inv_OrdinaryRequisitionDispensing.ID, tbl_Inv_OrdinaryRequisitionDispensing.FK_tbl_Inv_OrdinaryRequisitionDetail_ID,
            tbl_Inv_OrdinaryRequisitionDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo, tbl_Inv_OrdinaryRequisitionDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
            tbl_Inv_OrdinaryRequisitionDispensing.Quantity, tbl_Inv_OrdinaryRequisitionDispensing.DispensingDate, tbl_Inv_OrdinaryRequisitionDispensing.Remarks,
            tbl_Inv_OrdinaryRequisitionDispensing.CreatedBy, tbl_Inv_OrdinaryRequisitionDispensing.CreatedDate, tbl_Inv_OrdinaryRequisitionDispensing.ModifiedBy, tbl_Inv_OrdinaryRequisitionDispensing.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region AutoDispensing
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

        #region Report   
        public List<ReportCallingModel> GetRLOrdinaryRequisitionDispensing()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Ordinary Dispensing Item History",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Ordinary Dispensing Item Availability",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null,
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Ordinary Dispensing Item History")
            {
                return await Task.Run(() => OrdinaryDispensingItemHistory(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Ordinary Dispensing Item Availability")
            {
                return await Task.Run(() => OrdinaryDispensingItemAvailability(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> OrdinaryDispensingItemHistory(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Ordinary Dispensing Item History During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);

            /////////////------------------------------table for Detail 6------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.20), //WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.15),//Posting Date
                        (float)(PageSize.A4.GetWidth() * 0.30),//Narration
                        (float)(PageSize.A4.GetWidth() * 0.12),//Qty In  
                        (float)(PageSize.A4.GetWidth() * 0.12),//Qty Out
                        (float)(PageSize.A4.GetWidth() * 0.11),//Reference No                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);



            double GrandTotalQtyIn = 0, GrandTotalQtyOut = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string ProductName = "";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Product")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add(ProductName.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Department")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["DepartmentName"].ToString())).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));


                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Return")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Issued")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy hh:mm tt"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityIn"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityOut"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetBold());

                        GrandTotalQtyIn += Convert.ToDouble(sqlReader["QuantityIn"]);
                        GrandTotalQtyOut += Convert.ToDouble(sqlReader["QuantityOut"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQtyIn.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQtyOut.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> OrdinaryDispensingItemAvailability(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Ordinary Dispensing Item Available to Issuance -----", true);

            /////////////------------------------------table for Detail 4------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //SNo
                        (float)(PageSize.A4.GetWidth() * 0.40), //WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.25),//Balance
                        (float)(PageSize.A4.GetWidth() * 0.25),//Reference No                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);


            double GrandTotalBalance = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string ProductName = "";
                int SNo = 1;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Product")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 3).Add(new Paragraph().Add(ProductName.ToString())).SetTextAlignment(TextAlignment.CENTER).SetBold().SetKeepTogether(true));

                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Balance")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BalanceByWareHouse"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetBold());

                        SNo++;

                        GrandTotalBalance += Convert.ToDouble(sqlReader["BalanceByWareHouse"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalBalance.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }

        #endregion

    }
    public class BMRAdditionalDispensingRepository : IBMRAdditionalDispensing
    {
        private readonly OreasDbContext db;
        public BMRAdditionalDispensingRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region BMRAdditionalDispensingMaster
        public async Task<object> GetBMRAdditionalDispensingMaster(int id)
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRAdditionalDispensingMaster()
        {
            return new[]
            {
                new { n = "by BatchNo", v = "byBatchNo" }, new { n = "by ProductName", v = "byProductName" }, new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public object GetWCLBBMRAdditionalDispensingMaster()
        {
            return new[]
            {
                new { n = "by Dispensing Pending", v = "byDispensingPending" }, new { n = "by Dispensing Completed", v = "byDispensingCompleted" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRAdditionalDispensingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BMRAdditionalMasters
                                      .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                      .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && (w.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_BMRAdditionalDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byReferenceNo" && w.tbl_Pro_BMRAdditionalDetails
                                                                                           .Any(a =>
                                                                                                    a.tbl_Inv_BMRAdditionalDispensings
                                                                                                     .Any(b => b.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower())
                                                                                                     ||
                                                                                                     a.tbl_Inv_BMRAdditionalDispensings
                                                                                                     .Any(c => c.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower())
                                                                                                )
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
                                        .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && (w.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_BMRAdditionalDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byReferenceNo" && w.tbl_Pro_BMRAdditionalDetails
                                                                                           .Any(a =>
                                                                                                    a.tbl_Inv_BMRAdditionalDispensings
                                                                                                     .Any(b => b.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower())
                                                                                                     ||
                                                                                                     a.tbl_Inv_BMRAdditionalDispensings
                                                                                                     .Any(c => c.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower())
                                                                                                )
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
        #endregion

        #region BMRAdditionalDispensingDetail
        public async Task<object> GetBMRAdditionalDispensingDetail(int id)
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
        public object GetWCLBMRAdditionalDispensingDetail()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }, new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRAdditionalDispensingDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BMRAdditionalDetails
                                      .Where(w => w.FK_tbl_Pro_BMRAdditionalMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byReferenceNo" && w.tbl_Inv_BMRAdditionalDispensings
                                                                                      .Any(a =>
                                                                                                a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower()
                                                                                                ||
                                                                                                a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower()
                                                                                           )
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
                                                       ||
                                                       FilterByText == "byReferenceNo" && w.tbl_Inv_BMRAdditionalDispensings
                                                                                           .Any(a =>
                                                                                                      a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower()
                                                                                                      ||
                                                                                                      a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower()
                                                                                                 )
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
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          o.IsDispensed,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.IsDecimal,
                          DispensedQty = o.tbl_Inv_BMRAdditionalDispensings.Sum(s=> s.Quantity)
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRAdditionalDispensingDetail(tbl_Pro_BMRAdditionalDetail tbl_Pro_BMRAdditionalDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New" && tbl_Pro_BMRAdditionalDetail.ID > 0)
            {
                tbl_Pro_BMRAdditionalDetail.ModifiedBy = userName;
                tbl_Pro_BMRAdditionalDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "UpdateByDispensing";
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

        #region BMRAdditionalDispensingDetailDispensing
        public async Task<object> GetBMRAdditionalDispensingDetailDispensing(int id)
        {
            var qry = from o in await db.tbl_Inv_BMRAdditionalDispensings.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BMRAdditionalDetail_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString() : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRAdditionalDispensingDetailDispensing()
        {
            return new[]
            {
                new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRAdditionalDispensingDetailDispensing(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_BMRAdditionalDispensings
                                      .Where(w => w.FK_tbl_Pro_BMRAdditionalDetail_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byReferenceNo" && (w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()) : w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_BMRAdditionalDispensings
                                        .Where(w => w.FK_tbl_Pro_BMRAdditionalDetail_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byReferenceNo" && (w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()) : w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BMRAdditionalDetail_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString("dd-MMM-yy hh: mm tt") : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRAdditionalDispensingDetailDispensing(tbl_Inv_BMRAdditionalDispensing tbl_Inv_BMRAdditionalDispensing, string operation = "", string userName = "")
        {
            //tbl_Inv_BMRAdditionalDispensing.DispensingDate = DateTime.Now;
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_BMRAdditionalDispensing.CreatedBy = userName;
                tbl_Inv_BMRAdditionalDispensing.CreatedDate = DateTime.Now;
                //db.tbl_Inv_BMRAdditionalDispensings.Add(tbl_Inv_BMRAdditionalDispensing);
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_BMRAdditionalDispensing.ModifiedBy = userName;
                tbl_Inv_BMRAdditionalDispensing.ModifiedDate = DateTime.Now;
                //db.Entry(tbl_Inv_BMRAdditionalDispensing).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                //db.tbl_Inv_BMRAdditionalDispensings.Remove(db.tbl_Inv_BMRAdditionalDispensings.Find(tbl_Inv_BMRAdditionalDispensing.ID));
                //await db.SaveChangesAsync();
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_BMRAdditionalDispensing] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Pro_BMRAdditionalDetail_ID={4}
                  ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={5},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={6}
                  ,@Quantity={7},@DispensingDate={8},@Remarks={9}
                  ,@CreatedBy={10},@CreatedDate={11},@ModifiedBy={12},@ModifiedDate={13}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_BMRAdditionalDispensing.ID, tbl_Inv_BMRAdditionalDispensing.FK_tbl_Pro_BMRAdditionalDetail_ID,
                tbl_Inv_BMRAdditionalDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo, tbl_Inv_BMRAdditionalDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                tbl_Inv_BMRAdditionalDispensing.Quantity, tbl_Inv_BMRAdditionalDispensing.DispensingDate, tbl_Inv_BMRAdditionalDispensing.Remarks,
                tbl_Inv_BMRAdditionalDispensing.CreatedBy, tbl_Inv_BMRAdditionalDispensing.CreatedDate, tbl_Inv_BMRAdditionalDispensing.ModifiedBy, tbl_Inv_BMRAdditionalDispensing.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region AutoDispensing
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

        #region Report   
        public List<ReportCallingModel> GetRLBMRAdditionalRequisitionDetailDispensing()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Addition Dispensing Item History",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Addition Dispensing Item Availability",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null,
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "BMR Addition Dispensing Item History")
            {
                return await Task.Run(() => BMRAdditionDispensingItemHistory(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "BMR Addition Dispensing Item Availability")
            {
                return await Task.Run(() => BMRAdditionDispensingItemAvailability(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> BMRAdditionDispensingItemHistory(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- BMR Additional Dispensing Item History During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);

            /////////////------------------------------table for Detail 6------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.20), //WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.15),//Posting Date
                        (float)(PageSize.A4.GetWidth() * 0.30),//Narration
                        (float)(PageSize.A4.GetWidth() * 0.12),//Qty In  
                        (float)(PageSize.A4.GetWidth() * 0.12),//Qty Out
                        (float)(PageSize.A4.GetWidth() * 0.11),//Reference No                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);



            double GrandTotalQtyIn = 0, GrandTotalQtyOut = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string ProductName = "";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Product")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add(ProductName.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Batch No")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));


                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Return")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Issued")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy hh:mm tt"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityIn"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityOut"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetBold());

                        GrandTotalQtyIn += Convert.ToDouble(sqlReader["QuantityIn"]);
                        GrandTotalQtyOut += Convert.ToDouble(sqlReader["QuantityOut"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQtyIn.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQtyOut.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> BMRAdditionDispensingItemAvailability(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- BMR Additional Dispensing Item Available to Issuance -----", true);

            /////////////------------------------------table for Detail 4------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //SNo
                        (float)(PageSize.A4.GetWidth() * 0.40), //WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.25),//Balance
                        (float)(PageSize.A4.GetWidth() * 0.25),//Reference No                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);


            double GrandTotalBalance = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string ProductName = "";
                int SNo = 1;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Product")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 3).Add(new Paragraph().Add(ProductName.ToString())).SetTextAlignment(TextAlignment.CENTER).SetBold().SetKeepTogether(true));

                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Balance")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BalanceByWareHouse"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetBold());

                        SNo++;

                        GrandTotalBalance += Convert.ToDouble(sqlReader["BalanceByWareHouse"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalBalance.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }

        #endregion

    }
    public class BMRDispensingRepository : IBMRDispensing
    {
        private readonly OreasDbContext db;
        public BMRDispensingRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region BMRDispensingMaster
        public async Task<object> GetBMRDispensingMaster(int id)
        {
            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionMasters.Where(w => w.ID == id).ToListAsync()
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
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDispensingMaster()
        {
            return new[]
            {
                new { n = "by BatchNo", v = "byBatchNo" }, new { n = "by Raw ProductName", v = "byRawProductName" }, new { n = "by SemiFinished ProductName", v = "bySemiFinishedProductName" }
            }.ToList();
        }
        public object GetWCLBBMRDispensingMaster()
        {
            return new[]
            {
                new { n = "by Dispensing Pending R", v = "byDispensingPendingR" }, new { n = "by Dispensing Completed R", v = "byDispensingCompletedR" },
                new { n = "by Dispensing Pending P", v = "byDispensingPendingP" }, new { n = "by Dispensing Completed P", v = "byDispensingCompletedP" },
                new { n = "by Dispensing Pending", v = "byDispensingPending" }, new { n = "by Dispensing Completed", v = "byDispensingCompleted" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDispensingMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
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

        #region BMRDispensingDetailRawItems
        public async Task<object> GetBMRDispensingDetailRawItems(int id)
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDispensingDetailRawItems()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }, new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDispensingDetailRawItems(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();
            pageddata.PageSize = 10;

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails
                                      .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                      .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byReferenceNo" && w.tbl_Inv_BMRDispensingRaws
                                                                                      .Any(a =>
                                                                                                a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower()
                                                                                                ||
                                                                                                a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower()
                                                                                           )
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_RawDetails
                                        .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                        .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                        .Where(w =>
                                                   string.IsNullOrEmpty(FilterValueByText)
                                                   ||
                                                   FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                   ||
                                                   FilterByText == "byReferenceNo" && w.tbl_Inv_BMRDispensingRaws
                                                                                        .Any(a =>
                                                                                                  a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower()
                                                                                                  ||
                                                                                                  a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower()
                                                                                             )
                                                  )
                                        .OrderBy(i => i.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.tbl_Inv_WareHouseMaster.WareHouseName,
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
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.IsDecimal,
                          DispensedQty = o.tbl_Inv_BMRDispensingRaws.Where(w => w.DispensingDate.HasValue).Sum(s => s.Quantity),
                          ReservedQty = o.tbl_Inv_BMRDispensingRaws.Where(w => !w.DispensingDate.HasValue).Sum(s => s.Quantity)
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        #endregion

        #region DispensingRaw
        public async Task<object> GetBMRDispensingRaw(int id)
        {
            var qry = from o in await db.tbl_Inv_BMRDispensingRaws.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString() : "",
                          ReservationDate = o.ReservationDate.HasValue ? o.ReservationDate.Value.ToString() : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          IsDispensed = o.DispensingDate.HasValue
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDispensingRaw()
        {
            return new[]
            {
                new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDispensingRaw(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_BMRDispensingRaws
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byReferenceNo" && (w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()) : w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_BMRDispensingRaws
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byReferenceNo" && (w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()) : w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString("dd-MMM-yy hh: mm tt") : "",
                          ReservationDate = o.ReservationDate.HasValue ? o.ReservationDate.Value.ToString("dd-MMM-yy hh: mm tt") : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          IsDispensed = o.DispensingDate.HasValue
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRDispensingRaw(tbl_Inv_BMRDispensingRaw tbl_Inv_BMRDispensingRaw, string operation = "", string userName = "")
        {

            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_BMRDispensingRaw.CreatedBy = userName;
                tbl_Inv_BMRDispensingRaw.CreatedDate = DateTime.Now;

                //-----if ID=0 means new direct dispensing else if ID>0 with operation="Save New" means reserverdate stock is dispensing
                if (tbl_Inv_BMRDispensingRaw.ID == 0)
                    CRUD_Type.Value = "Insert";
                else if (tbl_Inv_BMRDispensingRaw.ID > 0)
                    CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_BMRDispensingRaw.ModifiedBy = userName;
                tbl_Inv_BMRDispensingRaw.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_BMRDispensingRaw] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID={4}
                  ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={5},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={6}
                  ,@Quantity={7},@DispensingDate={8},@ReservationDate={9},@Remarks={10}
                  ,@CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Inv_BMRDispensingRaw.ID, tbl_Inv_BMRDispensingRaw.FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID,
            tbl_Inv_BMRDispensingRaw.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo, tbl_Inv_BMRDispensingRaw.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
            tbl_Inv_BMRDispensingRaw.Quantity, tbl_Inv_BMRDispensingRaw.DispensingDate, tbl_Inv_BMRDispensingRaw.ReservationDate, tbl_Inv_BMRDispensingRaw.Remarks,
            tbl_Inv_BMRDispensingRaw.CreatedBy, tbl_Inv_BMRDispensingRaw.CreatedDate, tbl_Inv_BMRDispensingRaw.ModifiedBy, tbl_Inv_BMRDispensingRaw.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region BMRDispensingDetailPackagingDetailItems
        public async Task<object> GetBMRDispensingDetailPackagingDetailItems(int id)
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
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDispensingDetailPackagingDetailItems()
        {
            return new[]
            {
                new { n = "by ProductName", v = "byProductName" }, new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDispensingDetailPackagingDetailItems(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();
            pageddata.PageSize = 10;

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss
                                      .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                      .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byReferenceNo" && w.tbl_Inv_BMRDispensingPackagings
                                                                                      .Any(a =>
                                                                                                a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower()
                                                                                                ||
                                                                                                a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower()
                                                                                           )
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss
                                        .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                        .Where(w => w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byReferenceNo" && w.tbl_Inv_BMRDispensingPackagings
                                                                                            .Any(a =>
                                                                                                      a.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower()
                                                                                                      ||
                                                                                                      a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower()
                                                                                                 )
                                                       )
                                        .OrderBy(i => i.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.ID)
                                        .ThenBy(i => i.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.BatchSize,
                          BatchSizeUnit = o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.tbl_Pro_BatchMaterialRequisitionMaster.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductType_Category.tbl_Inv_ProductType.ProductType + "]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.IsDecimal,
                          DispensedQty = o.tbl_Inv_BMRDispensingPackagings.Where(w => w.DispensingDate.HasValue).Sum(s => s.Quantity),
                          ReservedQty = o.tbl_Inv_BMRDispensingPackagings.Where(w => !w.DispensingDate.HasValue).Sum(s => s.Quantity)
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        #endregion

        #region DispensingPackaging
        public async Task<object> GetBMRDispensingPackaging(int id)
        {
            var qry = from o in await db.tbl_Inv_BMRDispensingPackagings.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString() : "",
                          ReservationDate = o.ReservationDate.HasValue ? o.ReservationDate.Value.ToString() : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          IsDispensed = o.DispensingDate.HasValue
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLBMRDispensingPackaging()
        {
            return new[]
            {
                new { n = "by ReferenceNo", v = "byReferenceNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDispensingPackaging(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_BMRDispensingPackagings
                                      .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byReferenceNo" && (w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()) : w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_BMRDispensingPackagings
                                        .Where(w => w.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byReferenceNo" && (w.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower()) : w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower()))
                                                       )
                                        .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString("dd-MMM-yy hh: mm tt") : "",
                          ReservationDate = o.ReservationDate.HasValue ? o.ReservationDate.Value.ToString("dd-MMM-yy hh: mm tt") : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          IsDispensed = o.DispensingDate.HasValue
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostBMRDispensingPackaging(tbl_Inv_BMRDispensingPackaging tbl_Inv_BMRDispensingPackaging, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_BMRDispensingPackaging.CreatedBy = userName;
                tbl_Inv_BMRDispensingPackaging.CreatedDate = DateTime.Now;

                //-----if ID=0 means new direct dispensing else if ID>0 with operation="Save New" means reserverdate stock is dispensing
                if (tbl_Inv_BMRDispensingPackaging.ID == 0)
                    CRUD_Type.Value = "Insert";
                else if (tbl_Inv_BMRDispensingPackaging.ID > 0)
                    CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_BMRDispensingPackaging.ModifiedBy = userName;
                tbl_Inv_BMRDispensingPackaging.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_BMRDispensingPackaging] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID={4}
                  ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={5},@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={6}
                  ,@Quantity={7},@DispensingDate={8},@ReservationDate={9},@Remarks={10}
                  ,@CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Inv_BMRDispensingPackaging.ID,
            tbl_Inv_BMRDispensingPackaging.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID,
            tbl_Inv_BMRDispensingPackaging.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo, tbl_Inv_BMRDispensingPackaging.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
            tbl_Inv_BMRDispensingPackaging.Quantity, tbl_Inv_BMRDispensingPackaging.DispensingDate, tbl_Inv_BMRDispensingPackaging.ReservationDate, tbl_Inv_BMRDispensingPackaging.Remarks,
            tbl_Inv_BMRDispensingPackaging.CreatedBy, tbl_Inv_BMRDispensingPackaging.CreatedDate, tbl_Inv_BMRDispensingPackaging.ModifiedBy, tbl_Inv_BMRDispensingPackaging.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region AutoDispensing

        public async Task<string> PostStockIssuanceReservation(int BMR_RawItemID = 0, int BMR_PackagingItemID = 0, int BMR_AdditionalItemID = 0, int OR_ItemID = 0, bool IssueTrue_ReserveFalse = true, string userName = "")
        {
            SqlParameter ResponseMsg = new SqlParameter("@ResponseMsg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = -1, Value = "" };

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[USP_Pro_StockIssuedOrReservation] 
              @ResponseMsg={0} OUTPUT,@BMR_RawItemID={1},@BMR_PackagingItemID={2}
             ,@BMR_AdditionalItemID={3},@OR_ItemID={4}
             ,@IssueTrue_ReserveFalse={5},@UserName={6},@EntryTime={7}",
             ResponseMsg, BMR_RawItemID, BMR_PackagingItemID,
             BMR_AdditionalItemID, OR_ItemID,
             IssueTrue_ReserveFalse,userName,null);

            return (string)ResponseMsg.Value;

        }

        #endregion

        #region Report
        public List<ReportCallingModel> GetRLBMRDispensingRaw()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Raw Dispensing Item History",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Raw Dispensing Item Availability",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null,
                }
            };
        }
        public List<ReportCallingModel> GetRLBMRDispensingPackaging()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Packaging Dispensing Item History",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="BMR Packaging Dispensing Item Availability",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null,
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "BMR Raw Dispensing Item History")
            {
                return await Task.Run(() => BMRRawDispensingItemHistory(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "BMR Raw Dispensing Item Availability")
            {
                return await Task.Run(() => BMRRawDispensingItemAvailability(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "BMR Packaging Dispensing Item History")
            {
                return await Task.Run(() => BMRPackagingDispensingItemHistory(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "BMR Packaging Dispensing Item Availability")
            {
                return await Task.Run(() => BMRPackagingDispensingItemAvailability(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "BMR Dispensing Detail")
            {
                return await Task.Run(() => BMRDispensingDetail(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> BMRDispensingDetail(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Batch Dispensing Detail " + "-----", true);

            /////////////------------------------------table for Detail 6------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //SNo
                        (float)(PageSize.A4.GetWidth() * 0.40),//Product
                        (float)(PageSize.A4.GetWidth() * 0.12),//ReqQty
                        (float)(PageSize.A4.GetWidth() * 0.12),//DispensedQty 
                        (float)(PageSize.A4.GetWidth() * 0.10),//RefNo
                        (float)(PageSize.A4.GetWidth() * 0.16),//Date                       
                }
            ).SetFontSize(8).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);

            var ColorSteelblue = new MyDeviceRgb(MyColor.SteelBlue).color;
            var ColorWhite = new MyDeviceRgb(MyColor.White).color;
            var ColorGray = new MyDeviceRgb(MyColor.Gray).color;
            var ColorLightSteelBlue = new MyDeviceRgb(MyColor.LightSteelBlue).color;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                               
                //----------------------Report 1-----------------------------//
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {                        
                        pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Product Name")).SetBackgroundColor(ColorSteelblue).SetFontColor(ColorWhite).SetBold());
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Batch No")).SetBackgroundColor(ColorSteelblue).SetFontColor(ColorWhite).SetBold());
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Batch Size")).SetBackgroundColor(ColorSteelblue).SetFontColor(ColorWhite).SetBold());
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBackgroundColor(ColorSteelblue).SetFontColor(ColorWhite).SetBold());
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Status")).SetBackgroundColor(ColorSteelblue).SetFontColor(ColorWhite).SetBold());

                        pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchSize"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchStatus"].ToString())).SetKeepTogether(true).SetBold());
                    }
                }

                //----------------------Report 2-----------------------------//
                ReportName.Value = rn + "2"; int SNo = 1; int ProdID = 0; string dt = null;
                string FilterName = "";
                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("\n")).SetBold().SetBorder(Border.NO_BORDER));
                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Raw Material Detail")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(10));
   
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (FilterName != sqlReader["FilterName"].ToString())
                        {
                            FilterName = sqlReader["FilterName"].ToString();

                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(FilterName)).SetBold());
                            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBold());

                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Req Qty")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetTextAlignment(TextAlignment.RIGHT).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Dispensed Qty")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetTextAlignment(TextAlignment.RIGHT).SetTextAlignment(TextAlignment.RIGHT).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Ref No")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Date")).SetBackgroundColor(ColorGray).SetFontColor(ColorWhite).SetBold());

                        }
                        if (ProdID != Convert.ToInt32(sqlReader["ID"]))
                        {
                            ProdID = Convert.ToInt32(sqlReader["ID"]);
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReqQty"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));

                            SNo++;
                        }
                        else
                            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("")).SetKeepTogether(true));

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DispensedQty"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true));
                        dt = (
                                "D: " + (sqlReader.IsDBNull("DispensingDate") ? "" : Convert.ToDateTime(sqlReader["DispensingDate"]).ToString("dd-MMM-yy hh:mm tt")) + "\n" +
                                "R: " + (sqlReader.IsDBNull("ReservationDate") ? "" : Convert.ToDateTime(sqlReader["ReservationDate"]).ToString("dd-MMM-yy hh:mm tt"))
                            );
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(dt)).SetFontSize(6).SetKeepTogether(true));

                    }
                }

                //----------------------Report 3-----------------------------//
                ReportName.Value = rn + "3";  FilterName = ""; ProdID = 0;
                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("\n")).SetBold().SetBorder(Border.NO_BORDER));
                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Packaging Material Detail")).SetBackgroundColor(ColorLightSteelBlue).SetTextAlignment(TextAlignment.CENTER).SetBold().SetFontSize(10));

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (FilterName != sqlReader["FilterName"].ToString())
                        {
                            FilterName = sqlReader["FilterName"].ToString();

                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(FilterName)).SetBold());
                            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBold());

                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBackgroundColor(ColorLightSteelBlue).SetFontColor(ColorWhite).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBackgroundColor(ColorLightSteelBlue).SetFontColor(ColorWhite).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Req Qty")).SetBackgroundColor(ColorLightSteelBlue).SetFontColor(ColorWhite).SetTextAlignment(TextAlignment.RIGHT).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Dispensed Qty")).SetBackgroundColor(ColorLightSteelBlue).SetFontColor(ColorWhite).SetTextAlignment(TextAlignment.RIGHT).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Ref No")).SetBackgroundColor(ColorLightSteelBlue).SetFontColor(ColorWhite).SetBold());
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Date")).SetBackgroundColor(ColorLightSteelBlue).SetFontColor(ColorWhite).SetBold());

                        }
                        if (ProdID != Convert.ToInt32(sqlReader["ID"]))
                        {
                            ProdID = Convert.ToInt32(sqlReader["ID"]);
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReqQty"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));

                            SNo++;
                        }
                        else
                            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("")).SetKeepTogether(true));

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DispensedQty"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        dt = (
                                "D: " + (sqlReader.IsDBNull("DispensingDate") ? "" : Convert.ToDateTime(sqlReader["DispensingDate"]).ToString("dd-MMM-yy hh:mm tt")) + "\n" +
                                "R: " + (sqlReader.IsDBNull("ReservationDate") ? "" : Convert.ToDateTime(sqlReader["ReservationDate"]).ToString("dd-MMM-yy hh:mm tt"))
                            );
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(dt)).SetFontSize(6).SetKeepTogether(true));

                    }
                }


            }

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> BMRRawDispensingItemHistory(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- BMR Raw Dispensing Item History During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);

            /////////////------------------------------table for Detail 6------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.20), //WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.15),//Posting Date
                        (float)(PageSize.A4.GetWidth() * 0.30),//Narration
                        (float)(PageSize.A4.GetWidth() * 0.12),//Qty In  
                        (float)(PageSize.A4.GetWidth() * 0.12),//Qty Out
                        (float)(PageSize.A4.GetWidth() * 0.11),//Reference No                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);



            double GrandTotalQtyIn = 0, GrandTotalQtyOut = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string ProductName = "";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Product")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add(ProductName.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Batch No")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));


                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Return")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Issued")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy hh:mm tt"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityIn"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityOut"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetBold());

                        GrandTotalQtyIn += Convert.ToDouble(sqlReader["QuantityIn"]);
                        GrandTotalQtyOut += Convert.ToDouble(sqlReader["QuantityOut"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQtyIn.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQtyOut.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> BMRRawDispensingItemAvailability(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- BMR Raw Dispensing Item Available to Issuance -----", true);

            /////////////------------------------------table for Detail 4------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //SNo
                        (float)(PageSize.A4.GetWidth() * 0.40), //WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.25),//Balance
                        (float)(PageSize.A4.GetWidth() * 0.25),//Reference No                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);


            double GrandTotalBalance = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string ProductName = "";
                int SNo = 1;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Product")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 3).Add(new Paragraph().Add(ProductName.ToString())).SetTextAlignment(TextAlignment.CENTER).SetBold().SetKeepTogether(true));

                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Balance")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BalanceByWareHouse"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetBold());

                        SNo++;

                        GrandTotalBalance += Convert.ToDouble(sqlReader["BalanceByWareHouse"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalBalance.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> BMRPackagingDispensingItemHistory(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- BMR Packaging Dispensing Item History During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);

            /////////////------------------------------table for Detail 6------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.20), //WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.15),//Posting Date
                        (float)(PageSize.A4.GetWidth() * 0.30),//Narration
                        (float)(PageSize.A4.GetWidth() * 0.12),//Qty In  
                        (float)(PageSize.A4.GetWidth() * 0.12),//Qty Out
                        (float)(PageSize.A4.GetWidth() * 0.11),//Reference No                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);



            double GrandTotalQtyIn = 0, GrandTotalQtyOut = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string ProductName = "";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Product")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add(ProductName.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Batch No")).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetTextAlignment(TextAlignment.LEFT).SetBold().SetKeepTogether(true));


                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Return")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Issued")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MMM-yy hh:mm tt"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityIn"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityOut"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetBold());

                        GrandTotalQtyIn += Convert.ToDouble(sqlReader["QuantityIn"]);
                        GrandTotalQtyOut += Convert.ToDouble(sqlReader["QuantityOut"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQtyIn.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQtyOut.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> BMRPackagingDispensingItemAvailability(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- BMR Packaging Dispensing Item Available to Issuance -----", true);

            /////////////------------------------------table for Detail 4------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //SNo
                        (float)(PageSize.A4.GetWidth() * 0.40), //WareHouse
                        (float)(PageSize.A4.GetWidth() * 0.25),//Balance
                        (float)(PageSize.A4.GetWidth() * 0.25),//Reference No                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);


            double GrandTotalBalance = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Dispensing] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string ProductName = "";
                int SNo = 1;
                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(ProductName))
                        {
                            ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 1).Add(new Paragraph().Add("Product")).SetTextAlignment(TextAlignment.CENTER).SetBold().SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 3).Add(new Paragraph().Add(ProductName.ToString())).SetTextAlignment(TextAlignment.CENTER).SetBold().SetKeepTogether(true));

                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Balance")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reference#")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BalanceByWareHouse"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetKeepTogether(true).SetBold());

                        SNo++;

                        GrandTotalBalance += Convert.ToDouble(sqlReader["BalanceByWareHouse"]);
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalBalance.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 1).Add(new Paragraph().Add(" ")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }

        #endregion

    }
    public class InvProductionTransferRepository : IInvProductionTransfer
    {
        private readonly OreasDbContext db;
        public InvProductionTransferRepository(OreasDbContext oreasDbContext)
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
                new { n = "by Received", v = "byReceived" }, new { n = "by Not Received", v = "byNotReceived" }, new { n = "by QA Cleared Not Received", v = "byQAClearedNotReceived" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadProductionTransferMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_ProductionTransferMasters
                                    .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                    .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                    .Count(w => w.UserName == userName) > 0)
                                    )
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byReceived" && w.ReceivedAll == true
                                            ||
                                            FilterByLoad == "byNotReceived" && w.ReceivedAll == false
                                            ||
                                            FilterByLoad == "byQAClearedNotReceived" && w.ReceivedAll == false && w.QAClearedAll == true
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
                                  .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                    .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                    .Count(w => w.UserName == userName) > 0)
                                    )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byReceived" && w.ReceivedAll == true
                                            ||
                                            FilterByLoad == "byNotReceived" && w.ReceivedAll == false
                                            ||
                                            FilterByLoad == "byQAClearedNotReceived" && w.ReceivedAll == false && w.QAClearedAll == true
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

            CRUD_Type.Value = "ReceivedUpdate";

            tbl_Pro_ProductionTransferDetail.ReceivedBy = userName;
            tbl_Pro_ProductionTransferDetail.ReceivedDate = DateTime.Now;

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
    public class PDRequestDispensingRepository : IPDRequestDispensing
    {
        private readonly OreasDbContext db;
        public PDRequestDispensingRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region CFP Master

        public object GetWCLRequestCFPMaster()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }, new { n = "by ProductName", v = "byProductName" }, new { n = "by BatchNo", v = "byBatchNo" }
            }.ToList();
        }
        public object GetWCLBRequestCFPMaster()
        {
            return new[]
            {
                new { n = "by Dispensing Pending", v = "byDispensingPending" }, new { n = "by Dispensing Completed", v = "byDispensingCompleted" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadRequestCFPMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "", List<int> AuthStoreList = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_PD_RequestDetailTR_CFPs
                                      .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses.Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers.Count(c => c.UserName == userName) > 0))
                                      //.Where(w=> AuthStoreList.Contains(w.FK_tbl_Inv_WareHouseMaster_ID))
                                      .Where(w =>
                                                string.IsNullOrEmpty(FilterValueByText)
                                                ||
                                                FilterByText == "byDocNo" && w.tbl_PD_RequestDetailTR.DocNo.ToString() == FilterValueByText
                                                ||
                                                FilterByText == "byBatchNo" && w.tbl_PD_RequestDetailTR.TrialBatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                ||
                                                FilterByText == "byProductName" && w.tbl_PD_RequestDetailTR_CFP_Items.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                ||
                                                FilterByText == "byReferenceNo" && w.tbl_PD_RequestDetailTR_CFP_Items
                                                                                    .Any(a =>
                                                                                            a.tbl_Inv_PDRequestDispensings
                                                                                                .Any(b => b.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower())
                                                                                                ||
                                                                                            a.tbl_Inv_PDRequestDispensings
                                                                                            .Any(c => c.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower())
                                                                                        )
                                                       )
                                      .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byDispensingPending" && w.IsDispensedAll == false
                                                ||
                                                FilterByLoad == "byDispensingCompleted" && w.IsDispensedAll == true
                                                )
                                       .OrderBy(o => o.FK_tbl_PD_RequestDetailTR_ID).CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_PD_RequestDetailTR_CFPs
                                        .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses.Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers.Count(c => c.UserName == userName) > 0))
                                       // .Where(w => AuthStoreList.Contains(w.FK_tbl_Inv_WareHouseMaster_ID))
                                        .Where(w =>
                                                string.IsNullOrEmpty(FilterValueByText)
                                                ||
                                                FilterByText == "byDocNo" && w.tbl_PD_RequestDetailTR.DocNo.ToString() == FilterValueByText
                                                ||
                                                FilterByText == "byBatchNo" && w.tbl_PD_RequestDetailTR.TrialBatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                ||
                                                FilterByText == "byProductName" && w.tbl_PD_RequestDetailTR_CFP_Items.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                ||
                                                FilterByText == "byReferenceNo" && w.tbl_PD_RequestDetailTR_CFP_Items
                                                                                    .Any(a =>
                                                                                            a.tbl_Inv_PDRequestDispensings
                                                                                                .Any(b => b.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower() == FilterValueByText.ToLower())
                                                                                                ||
                                                                                            a.tbl_Inv_PDRequestDispensings
                                                                                            .Any(c => c.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower() == FilterValueByText.ToLower())
                                                                                        )
                                                       )
                                      .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byDispensingPending" && w.IsDispensedAll == false
                                                ||
                                                FilterByLoad == "byDispensingCompleted" && w.IsDispensedAll == true
                                                )
                                        .OrderByDescending(i => i.FK_tbl_PD_RequestDetailTR_ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_ID,
                          o.tbl_PD_RequestDetailTR.tbl_PD_RequestMaster.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.FK_tbl_Pro_CompositionFilterPolicyDetail_ID,
                          FK_tbl_Pro_CompositionFilterPolicyDetail_IDName = o.tbl_Pro_CompositionFilterPolicyDetail.FilterName,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.IsDispensedAll,
                          o.tbl_PD_RequestDetailTR.TrialBatchNo,
                          o.tbl_PD_RequestDetailTR.TrialBatchSizeInSemiUnits,
                          o.tbl_PD_RequestDetailTR.tbl_PD_RequestMaster.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.tbl_PD_RequestDetailTR.TrialStatus,
                          o.tbl_PD_RequestDetailTR.DocNo,
                          DocDate = o.tbl_PD_RequestDetailTR.DocDate.ToString("dd-MMM-yy"),
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region CFP Detail Item
        public async Task<object> GetRequestCFPDetail(int id)
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
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yy hh:mm tt") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yy hh:mm tt") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLRequestCFPDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBRequestCFPDetail()
        {
            return new[]
            {
                new { n = "by Dispensing Pending", v = "byDispensingPending" }, new { n = "by Dispensing Completed", v = "byDispensingCompleted" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadRequestCFPDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_PD_RequestDetailTR_CFP_Items
                                    .Where(w => w.FK_tbl_PD_RequestDetailTR_CFP_ID == MasterID)
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                          )
                                    .Where(w =>
                                               string.IsNullOrEmpty(FilterByLoad)
                                               ||
                                               FilterByLoad == "byDispensingPending" && w.IsDispensed == false
                                               ||
                                               FilterByLoad == "byDispensingCompleted" && w.IsDispensed == true
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
                                  .Where(w =>
                                               string.IsNullOrEmpty(FilterByLoad)
                                               ||
                                               FilterByLoad == "byDispensingPending" && w.IsDispensed == false
                                               ||
                                               FilterByLoad == "byDispensingCompleted" && w.IsDispensed == true
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
        public async Task<string> PostRequestCFPDetail(tbl_PD_RequestDetailTR_CFP_Item tbl_PD_RequestDetailTR_CFP_Item, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New" && tbl_PD_RequestDetailTR_CFP_Item.ID > 0)
            {
                tbl_PD_RequestDetailTR_CFP_Item.ModifiedBy = userName;
                tbl_PD_RequestDetailTR_CFP_Item.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "UpdateByDispensing";
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

        #region CFP Detail Item Dispensing
        public async Task<object> GetRequestCFPDetailDispensing(int id)
        {
            var qry = from o in await db.tbl_Inv_PDRequestDispensings.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_CFP_Item_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString() : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          IsDispensed = o.DispensingDate.HasValue
                      };

            return qry.FirstOrDefault();
        }
        public async Task<PagedData<object>> LoadRequestCFPDetailDispensing(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PDRequestDispensings
                                    .Where(w => w.FK_tbl_PD_RequestDetailTR_CFP_Item_ID == MasterID)
                                    .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PDRequestDispensings
                                  .Where(w => w.FK_tbl_PD_RequestDetailTR_CFP_Item_ID == MasterID)
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_PD_RequestDetailTR_CFP_Item_ID,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          DispensingDate = o.DispensingDate.HasValue ? o.DispensingDate.Value.ToString() : "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          IsDispensed = o.DispensingDate.HasValue
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostRequestCFPDetailDispensing(tbl_Inv_PDRequestDispensing tbl_Inv_PDRequestDispensing, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PDRequestDispensing.CreatedBy = userName;
                tbl_Inv_PDRequestDispensing.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";

            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PDRequestDispensing.ModifiedBy = userName;
                tbl_Inv_PDRequestDispensing.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PDRequestDispensing] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_PD_RequestDetailTR_CFP_Item_ID={4}
              ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={5}
              ,@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={6}
              ,@Quantity={7},@DispensingDate={8},@Remarks={9}
              ,@CreatedBy={10},@CreatedDate={11},@ModifiedBy={12},@ModifiedDate={13}",
            CRUD_Type, CRUD_Msg, CRUD_ID,
            tbl_Inv_PDRequestDispensing.ID, tbl_Inv_PDRequestDispensing.FK_tbl_PD_RequestDetailTR_CFP_Item_ID,
            tbl_Inv_PDRequestDispensing.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
            tbl_Inv_PDRequestDispensing.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
            tbl_Inv_PDRequestDispensing.Quantity, tbl_Inv_PDRequestDispensing.DispensingDate, tbl_Inv_PDRequestDispensing.Remarks,
            tbl_Inv_PDRequestDispensing.CreatedBy, tbl_Inv_PDRequestDispensing.CreatedDate, tbl_Inv_PDRequestDispensing.ModifiedBy, tbl_Inv_PDRequestDispensing.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

    }
    public class InvLedgerRepository : IInvLedger
    {
        private readonly OreasDbContext db;
        public InvLedgerRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region  InvLedger
        public async Task<PagedData<object>> LoadInvLedger(int CurrentPage = 1, int MasterID = 0, int WareHouseID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {

            PagedData<object> pageddata = new PagedData<object>();
            pageddata.PageSize = 20;

            int NoOfRecords = await db.tbl_Inv_Ledgers
                                               .Where(w => w.FK_tbl_Inv_ProductRegistrationDetail_ID == MasterID)
                                               .Where(w => WareHouseID == 0 || w.FK_tbl_Inv_WareHouseMaster_ID == WareHouseID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                     )
                                               .Where(w => w.PostingDate >= FilterValueByDateRangeFrom && w.PostingDate <= FilterValueByDateRangeTill)
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_Ledgers
                                        .Where(w => w.FK_tbl_Inv_ProductRegistrationDetail_ID == MasterID)
                                        .Where(w => WareHouseID == 0 || w.FK_tbl_Inv_WareHouseMaster_ID == WareHouseID)
                                        .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                          )
                                        .Where(w => w.PostingDate >= FilterValueByDateRangeFrom && w.PostingDate <= FilterValueByDateRangeTill)
                                        .OrderByDescending(i => i.FK_tbl_Inv_WareHouseMaster_ID)
                                        .ThenByDescending(i => i.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo)
                                        .ThenByDescending(i => i.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo)
                                        .ThenByDescending(i => i.PostingDate)
                                        .Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o?.tbl_Inv_WareHouseMaster?.WareHouseName ?? "",
                          o.QuantityIn,
                          o.QuantityOut,
                          o.Narration,
                          PostingDate = o.PostingDate.ToString("dd-MMM-yy hh:mm tt"),
                          o.PostingNo,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID,
                          o.FK_tbl_Inv_PurchaseReturnNoteDetail_ID,
                          o.FK_tbl_Inv_SalesNoteDetail_ID,
                          o.FK_tbl_Inv_SalesReturnNoteDetail_ID,
                          ReferenceNo =  o?.tbl_Inv_PurchaseNoteDetail_RefNo?.ReferenceNo ?? "" + 
                                         o?.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo?.tbl_Pro_BatchMaterialRequisitionMaster?.BatchNo ?? "",
                          Ref = o.TrackingNo ?? ""
                      };

            pageddata.Data = qry;

            pageddata.otherdata =
                new
                {
                    Opening = Convert.ToDecimal(
                                    db.tbl_Inv_Ledgers.Where(w => 
                                    w.FK_tbl_Inv_ProductRegistrationDetail_ID == MasterID 
                                    && 
                                    w.PostingDate < FilterValueByDateRangeFrom
                                    &&
                                    (WareHouseID == 0 || w.FK_tbl_Inv_WareHouseMaster_ID == WareHouseID)
                                    ).Sum(s => s.QuantityIn - s.QuantityOut)
                                    ),
                    Closing = Convert.ToDecimal(
                                    db.tbl_Inv_Ledgers.Where(w => 
                                    w.FK_tbl_Inv_ProductRegistrationDetail_ID == MasterID 
                                    && 
                                    w.PostingDate <= FilterValueByDateRangeTill
                                    &&
                                    (WareHouseID == 0 || w.FK_tbl_Inv_WareHouseMaster_ID == WareHouseID)
                                    ).Sum(s => s.QuantityIn - s.QuantityOut)
                                    )
                };

            return pageddata;
        }

        #endregion

        #region Report     

        public List<ReportCallingModel> GetRLInvLedger()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Ledger",
                    GroupBy = null,
                    OrderBy = new List<string>(){ "WareHouse", "Reference No" },
                    SeekBy = null
                },
                  new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Reorder Level Alert",
                    GroupBy = new List<string>(){ "Product Type", "Product Category" },
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }

        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName ="")
        {
            if (rn == "Ledger")
            {
                return await Task.Run(() => LedgerAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID));
            }
            else if (rn == "Stock")
            {
                return await Task.Run(() => StockAsync(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID));
            }
            else if (rn == "Reorder Level Alert")
            {
                return await Task.Run(() => ReorderLevelAlert(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> ReorderLevelAlert(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Reorder Level Alert on " + DateTime.Now.ToString("dd-MMM-yyyy") + " -----", true);

            /////////////------------------------------table for Detail 6------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10),//S No
                        (float)(PageSize.A4.GetWidth() * 0.30),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10),//ReorderLevel
                        (float)(PageSize.A4.GetWidth() * 0.10),//Balance 
                        (float)(PageSize.A4.GetWidth() * 0.20),//Type 
                        (float)(PageSize.A4.GetWidth() * 0.20)//Category                        
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            int SNo = 1;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string GroupbyFieldName = GroupBy == "Product Type" ? "ProductType" :
                                          GroupBy == "Product Category" ? "CategoryName" :
                                          "";

                await command.Connection.OpenAsync();

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (!string.IsNullOrEmpty(GroupbyFieldName) && GroupbyValue != sqlReader[GroupbyFieldName].ToString())
                        {

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Reorder Level")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Balance")).SetBold());
                        }


                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReorderLevel"].ToString() + sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Balance"].ToString() + sqlReader["MeasurementUnit"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductType"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CategoryName"].ToString())).SetKeepTogether(true));

                        SNo++;
                    }

                }
            }

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> StockAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + "Stock Position Period Date From: " + datefrom.Value.ToString("dd-MMM-yyyy") + " To " + datetill.Value.ToString("dd-MMM-yyyy") + "-----", true);

            //--------------------------------5 column table ------------------------------//
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.50),//ProductName
                        (float)(PageSize.A4.GetWidth()*0.13),//Opening
                        (float)(PageSize.A4.GetWidth()*0.12),//In
                        (float)(PageSize.A4.GetWidth()*0.12),//Out
                        (float)(PageSize.A4.GetWidth()*0.13)//Closing
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);


            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string WareHouse = "", Category = "";

                decimal Opening = 0, TotalIn = 0, TotalOut = 0, Closing = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {

                    while (sqlReader.Read())
                    {
                        if (WareHouse != sqlReader["WareHouseName"].ToString())
                        {
                            WareHouse = sqlReader["WareHouseName"].ToString();                            
                            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(WareHouse)).SetBackgroundColor(new DeviceRgb(102, 140, 255)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                            if (Category != sqlReader["CategoryName"].ToString())
                            {
                                Category = sqlReader["CategoryName"].ToString();
                                pdftableMain.AddCell(new Cell(1,5).Add(new Paragraph().Add(Category)).SetBackgroundColor(new DeviceRgb(179, 198, 255)).SetTextAlignment(TextAlignment.CENTER).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Product")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Opening")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Total In")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Total Out")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Closing")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                            }
                        }

                        Opening = Convert.ToDecimal(sqlReader["Opening"]);
                        TotalIn = Convert.ToDecimal(sqlReader["TotalIn"]);
                        TotalOut = Convert.ToDecimal(sqlReader["TotalOut"]);
                        Closing = Convert.ToDecimal(sqlReader["Closing"]);

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Opening.ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(TotalIn.ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(TotalOut.ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Closing.ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                    }

                }

            }

            page.InsertContent(pdftableMain);

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> LedgerAsync(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- " + "Inventory Ledger Period Date From: " + datefrom.Value.ToString("dd-MMM-yyyy") + " To " + datetill.Value.ToString("dd-MMM-yyyy") + "-----", true);

            //--------------------------------7 column table ------------------------------//
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth()*0.15),//PostingDate
                        (float)(PageSize.A4.GetWidth()*0.18),//WareHouseName
                        (float)(PageSize.A4.GetWidth()*0.27),//Narration
                        (float)(PageSize.A4.GetWidth()*0.10),//QuantityIn
                        (float)(PageSize.A4.GetWidth()*0.10),//QuantityOut
                        (float)(PageSize.A4.GetWidth()*0.10),//ReferenceNo
                        (float)(PageSize.A4.GetWidth()*0.10) //Balance
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Product Name:")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Opening:")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(string.Format("{0:n0}", sqlReader["Opening"]))).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        Balance += Convert.ToDecimal(sqlReader["Opening"]);
                    }
                }
                
                ReportName.Value = rn + "2";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Posting Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Narration")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Qty In")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Qty Out")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Reference #")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Balance")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                    while (sqlReader.Read())
                    {
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PostingDate"]).ToString("dd-MM-yy hh:mm tt"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Narration"].ToString()).SetFontSize(5)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityIn"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["QuantityOut"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        Balance += Convert.ToDecimal(sqlReader["QuantityIn"]) - Convert.ToDecimal(sqlReader["QuantityOut"]);

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Balance.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                    }

                }

                pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add(" ")).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true)).SetFontSize(7);
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add("Closing:")).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(Balance.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

            }

            page.InsertContent(pdftableMain);

            return page.FinishToGetBytes();
        }

        #endregion
    }
    public class StockTransferRepository : IStockTransfer
    {
        private readonly OreasDbContext db;
        public StockTransferRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetStockTransferMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_StockTransferMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID_From,
                          FK_tbl_Inv_WareHouseMaster_ID_FromName = o.tbl_Inv_WareHouseMaster_From.WareHouseName,
                          o.FK_tbl_Inv_WareHouseMaster_ID_To,
                          FK_tbl_Inv_WareHouseMaster_ID_ToName = o.tbl_Inv_WareHouseMaster_To.WareHouseName,
                          o.IsReceivedAll,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_StockTransferDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLStockTransferMaster()
        {
            return new[]
            {
                new { n = "by Doc No", v = "byDocNo" }, new { n = "by Product Name", v = "byProductName" },
                new { n = "by WareHouse From", v = "byWareHouseFrom" }, new { n = "by WareHouse To", v = "byWareHouseTo" }
            }.ToList();
        }
        public object GetWCLBStockTransferMaster()
        {
            return new[]
            {
                new { n = "by Received", v = "byReceived" }, new { n = "by Not Received", v = "byNotReceived" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadStockTransferMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_StockTransferMasters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterByLoad)
                                                       ||
                                                       FilterByLoad == "byReceived" && w.IsReceivedAll == true
                                                       ||
                                                       FilterByLoad == "byNotReceived" && w.IsReceivedAll == false
                                                     )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_StockTransferDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byWareHouseFrom" && w.tbl_Inv_WareHouseMaster_From.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byWareHouseTo" && w.tbl_Inv_WareHouseMaster_To.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_StockTransferMasters
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byReceived" && w.IsReceivedAll == true
                                            ||
                                            FilterByLoad == "byNotReceived" && w.IsReceivedAll == false
                                            )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_StockTransferDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                            ||
                                            FilterByText == "byWareHouseFrom" && w.tbl_Inv_WareHouseMaster_From.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byWareHouseTo" && w.tbl_Inv_WareHouseMaster_To.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                            )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.DocNo,
                          DocDate = o.DocDate.ToString(),
                          o.FK_tbl_Inv_WareHouseMaster_ID_From,
                          FK_tbl_Inv_WareHouseMaster_ID_FromName = o.tbl_Inv_WareHouseMaster_From.WareHouseName,
                          o.FK_tbl_Inv_WareHouseMaster_ID_To,
                          FK_tbl_Inv_WareHouseMaster_ID_ToName = o.tbl_Inv_WareHouseMaster_To.WareHouseName,
                          o.IsReceivedAll,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostStockTransferMaster(tbl_Inv_StockTransferMaster tbl_Inv_StockTransferMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_StockTransferMaster.CreatedBy = userName;
                tbl_Inv_StockTransferMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_StockTransferMaster.ModifiedBy = userName;
                tbl_Inv_StockTransferMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";

            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_StockTransferMaster] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@DocNo={4},@DocDate={5}
                  ,@FK_tbl_Inv_WareHouseMaster_ID_From={6},@FK_tbl_Inv_WareHouseMaster_ID_To={7},@Remarks={8}
                  ,@CreatedBy={9},@CreatedDate={10},@ModifiedBy={11},@ModifiedDate={12}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_StockTransferMaster.ID, tbl_Inv_StockTransferMaster.DocNo, tbl_Inv_StockTransferMaster.DocDate,
                tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_From, tbl_Inv_StockTransferMaster.FK_tbl_Inv_WareHouseMaster_ID_To, tbl_Inv_StockTransferMaster.Remarks,
                tbl_Inv_StockTransferMaster.CreatedBy, tbl_Inv_StockTransferMaster.CreatedDate, tbl_Inv_StockTransferMaster.ModifiedBy, tbl_Inv_StockTransferMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetStockTransferDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_StockTransferDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_StockTransferMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " x " + o.tbl_Inv_ProductRegistrationDetail.Split_Into.ToString(),
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          o.IsReceived,
                          o.ReceivedBy,
                          ReceivedDate = o.ReceivedDate.HasValue ? o.ReceivedDate.Value.ToString("dd-MMM-yy hh:mm tt") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLStockTransferDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }, new { n = "by Mfg ReferenceNo", v = "byMfgReferenceNo" }, new { n = "by PN ReferenceNo", v = "byPNReferenceNo" }
            }.ToList();
        }
        public object GetWCLBStockTransferDetail()
        {
            return new[]
            {
                new { n = "by Received", v = "byReceived" }, new { n = "by Not Received", v = "byNotReceived" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadStockTransferDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_StockTransferDetails
                                               .Where(w => w.FK_tbl_Inv_StockTransferMaster_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterByLoad)
                                                       ||
                                                       FilterByLoad == "byReceived" && w.IsReceived == true
                                                       ||
                                                       FilterByLoad == "byNotReceived" && w.IsReceived == false
                                                     )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byMfgReferenceNo" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byPNReferenceNo" && w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_StockTransferDetails
                                  .Where(w => w.FK_tbl_Inv_StockTransferMaster_ID == MasterID)
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byReceived" && w.IsReceived == true
                                            ||
                                            FilterByLoad == "byNotReceived" && w.IsReceived == false
                                            )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byMfgReferenceNo" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byPNReferenceNo" && w.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_StockTransferMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + " x " + o.tbl_Inv_ProductRegistrationDetail.Split_Into.ToString(),
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                          o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                          ReferenceNo = o.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo > 0 ? o.tbl_Inv_PurchaseNoteDetail_RefNo.ReferenceNo : o.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo > 0 ? o.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo : "",
                          o.Quantity,
                          o.IsReceived,
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
        public async Task<string> PostStockTransferDetail(tbl_Inv_StockTransferDetail tbl_Inv_StockTransferDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_StockTransferDetail.CreatedBy = userName;
                tbl_Inv_StockTransferDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_StockTransferDetail.ModifiedBy = userName;
                tbl_Inv_StockTransferDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_StockTransferDetail] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Inv_StockTransferMaster_ID={4}
                ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
                ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={6}
                ,@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={7}
                ,@Quantity={8},@IsReceived={9},@ReceivedBy={10},@ReceivedDate={11}
                ,@CreatedBy={12},@CreatedDate={13},@ModifiedBy={14},@ModifiedDate={15}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_StockTransferDetail.ID, tbl_Inv_StockTransferDetail.FK_tbl_Inv_StockTransferMaster_ID,
                tbl_Inv_StockTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Inv_StockTransferDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                tbl_Inv_StockTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                tbl_Inv_StockTransferDetail.Quantity, tbl_Inv_StockTransferDetail.IsReceived, tbl_Inv_StockTransferDetail.ReceivedBy, tbl_Inv_StockTransferDetail.ReceivedDate,
                tbl_Inv_StockTransferDetail.CreatedBy, tbl_Inv_StockTransferDetail.CreatedDate, tbl_Inv_StockTransferDetail.ModifiedBy, tbl_Inv_StockTransferDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        public async Task<string> PostReceviedStockTransferDetail(tbl_Inv_StockTransferDetail tbl_Inv_StockTransferDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            CRUD_Type.Value = "ReceivedUpdate";

            tbl_Inv_StockTransferDetail.ReceivedBy = userName;
            tbl_Inv_StockTransferDetail.ReceivedDate = DateTime.Now;

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_StockTransferDetail] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Inv_StockTransferMaster_ID={4}
                ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
                ,@FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo={6}
                ,@FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo={7}
                ,@Quantity={8},@IsReceived={9},@ReceivedBy={10},@ReceivedDate={11}
                ,@CreatedBy={12},@CreatedDate={13},@ModifiedBy={14},@ModifiedDate={15}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_StockTransferDetail.ID, tbl_Inv_StockTransferDetail.FK_tbl_Inv_StockTransferMaster_ID,
                tbl_Inv_StockTransferDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Inv_StockTransferDetail.FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo,
                tbl_Inv_StockTransferDetail.FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo,
                tbl_Inv_StockTransferDetail.Quantity, tbl_Inv_StockTransferDetail.IsReceived, tbl_Inv_StockTransferDetail.ReceivedBy, tbl_Inv_StockTransferDetail.ReceivedDate,
                tbl_Inv_StockTransferDetail.CreatedBy, tbl_Inv_StockTransferDetail.CreatedDate, tbl_Inv_StockTransferDetail.ModifiedBy, tbl_Inv_StockTransferDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLStockTransfer()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Stock Transfer",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Stock Transfer",
                    GroupBy = new List<string>(){ "WareHouseFrom", "WareHouseTo", "Product" },
                    OrderBy = new List<string>(){ "Doc Date", "Doc No" },
                    SeekBy = new List<string>(){ "All" ,"Received", "Not Received" }
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Stock Transfer")
            {
                return await Task.Run(() => CurrentStockTransfer(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Stock Transfer")
            {
                return await Task.Run(() => RegisterStockTransfer(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentStockTransfer(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Stock Transfer -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //DocNo
                        (float)(PageSize.A4.GetWidth() * 0.10), //DocDate
                        (float)(PageSize.A4.GetWidth() * 0.40), //WareHouseFrom
                        (float)(PageSize.A4.GetWidth() * 0.40) //WareHouseFrom
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse From")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse To")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WHMF"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WHMT"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 4------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //S No
                        (float)(PageSize.A4.GetWidth() * 0.60),  //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15), //Ref No
                        (float)(PageSize.A4.GetWidth() * 0.15)  //Quantity
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Reference No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";
                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));                        
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ReferenceNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
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
        private async Task<byte[]> RegisterStockTransfer(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Stock Transfer Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);

            /////////////------------------------------table for Detail 8------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.08),//S No
                        (float)(PageSize.A4.GetWidth() * 0.08),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.08),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.16),//WareHouseFrom
                        (float)(PageSize.A4.GetWidth() * 0.16),//WareHouseTo
                        (float)(PageSize.A4.GetWidth() * 0.30),//ProductName                      
                        (float)(PageSize.A4.GetWidth() * 0.10), //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05) //IsReceived
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse From")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("WareHouse To")).SetBold());            
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Received")).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_General] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string GroupbyFieldName = GroupBy == "WareHouseFrom" ? "WareHouseFrom" :
                                          GroupBy == "WareHouseTo" ? "WareHouseTo" :
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
                                pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseFrom"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseTo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["IsReceived"].ToString())).SetKeepTogether(true));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        SNo++;
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 8).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }

        #endregion
    }

    /// <summary>
    /// Supply Chain
    /// </summary>
    /// 
    public class InternationalCommercialTermRepository : IInternationalCommercialTerm
    {
        private readonly OreasDbContext db;
        public InternationalCommercialTermRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetInternationalCommercialTerm(int id)
        {
            var qry = from o in await db.tbl_Inv_InternationalCommercialTerms.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.IncotermName,
                          o.Abbreviation,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLInternationalCommercialTerm()
        {
            return new[]
            {
                new { n = "by Name", v = "byName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadInternationalCommercialTerm(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_InternationalCommercialTerms
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byName" && w.IncotermName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_InternationalCommercialTerms
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.IncotermName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.IncotermName,
                          o.Abbreviation,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostInternationalCommercialTerm(tbl_Inv_InternationalCommercialTerm tbl_Inv_InternationalCommercialTerm, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_InternationalCommercialTerm.CreatedBy = userName;
                tbl_Inv_InternationalCommercialTerm.CreatedDate = DateTime.Now;
                db.tbl_Inv_InternationalCommercialTerms.Add(tbl_Inv_InternationalCommercialTerm);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_InternationalCommercialTerm.ModifiedBy = userName;
                tbl_Inv_InternationalCommercialTerm.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_InternationalCommercialTerm).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_InternationalCommercialTerms.Remove(db.tbl_Inv_InternationalCommercialTerms.Find(tbl_Inv_InternationalCommercialTerm.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }
    }
    public class TransportTypeRepository : ITransportType
    {
        private readonly OreasDbContext db;
        public TransportTypeRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetTransportType(int id)
        {
            var qry = from o in await db.tbl_Inv_TransportTypes.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.TypeName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLTransportType()
        {
            return new[]
            {
                new { n = "by Type", v = "byType" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadTransportType(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_TransportTypes
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byType" && w.TypeName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_TransportTypes
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byType" && w.TypeName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.TypeName,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostTransportType(tbl_Inv_TransportType tbl_Inv_TransportType, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_TransportType.CreatedBy = userName;
                tbl_Inv_TransportType.CreatedDate = DateTime.Now;
                db.tbl_Inv_TransportTypes.Add(tbl_Inv_TransportType);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_TransportType.ModifiedBy = userName;
                tbl_Inv_TransportType.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_TransportType).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_TransportTypes.Remove(db.tbl_Inv_TransportTypes.Find(tbl_Inv_TransportType.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class SupplierRepository : ISupplier
    {
        private readonly OreasDbContext db;
        public SupplierRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> GetSupplier(int id)
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
                          Supplier_EvaluatedOn = o.Supplier_EvaluatedOn.HasValue ? o.Supplier_EvaluatedOn.Value.ToString("dd-MMM-yy") : "",
                          Supplier_EvaluationScore = o?.Supplier_EvaluationScore ?? 0,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          ChildCount = o.tbl_Ac_ChartOfAccounts_Parents.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSupplier()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSupplier(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_ChartOfAccountss
                                               .Where(w => w.tbl_Ac_ChartOfAccounts_Type.AccountType.ToLower() == "supplier" && w.IsTransactional)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_ChartOfAccountss
                                  .Where(w => w.tbl_Ac_ChartOfAccounts_Type.AccountType.ToLower() == "supplier" && w.IsTransactional)
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
                          FK_tbl_Ac_PolicyWHTaxOnPurchase_IDName = o.FK_tbl_Ac_PolicyWHTaxOnPurchase_ID.HasValue ? o.tbl_Ac_PolicyWHTaxOnPurchase.WHTaxName : "",
                          o.FK_tbl_Ac_PolicyWHTaxOnSales_ID,
                          FK_tbl_Ac_PolicyWHTaxOnSales_IDName = o.FK_tbl_Ac_PolicyWHTaxOnSales_ID.HasValue ? o.tbl_Ac_PolicyWHTaxOnSales.WHTaxName : "",
                          o.FK_tbl_Ac_PolicyPaymentTerm_ID,
                          FK_tbl_Ac_PolicyPaymentTerm_IDName = o.FK_tbl_Ac_PolicyPaymentTerm_ID.HasValue ? o.tbl_Ac_PolicyPaymentTerm.Name + "[DL:" + o.tbl_Ac_PolicyPaymentTerm.DaysLimit.ToString() + "][Ad%:" + o.tbl_Ac_PolicyPaymentTerm.AdvancePercentage.ToString() + "]" : "",
                          o.Supplier_Approved,
                          Supplier_EvaluatedOn = o.Supplier_EvaluatedOn.HasValue ? o.Supplier_EvaluatedOn.Value.ToString("dd-MMM-yy") : "",
                          o.Supplier_EvaluationScore,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostSupplier(tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_ChartOfAccounts.ModifiedBy = userName;
                tbl_Ac_ChartOfAccounts.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Evaluation";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_ChartOfAccounts.ModifiedBy = userName;
                tbl_Ac_ChartOfAccounts.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "EvaluationUpdate";
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
        public List<ReportCallingModel> GetRLSupplierMaster()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Supplier List",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = new List<string>(){ "Approved", "Rejected", "Pending", "All" }
                }
            };
        }
        public List<ReportCallingModel> GetRLSupplierDetail()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Supplier Performance",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Supplier List")
            {
                return await Task.Run(() => SupplierList(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Supplier Performance")
            {
                return await Task.Run(() => SupplierPerformance(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> SupplierPerformance(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Supplier Performance From " + datefrom.Value.ToString("dd-MMM-yy") + " To " + datetill.Value.ToString("dd-MMM-yy") + " -----", true);

            /////////////------------------------------table for Detail 8------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10),//SNo 
                        (float)(PageSize.A4.GetWidth() * 0.10),//PONo 
                        (float)(PageSize.A4.GetWidth() * 0.10),//PODate 
                        (float)(PageSize.A4.GetWidth() * 0.40),//Product 
                        (float)(PageSize.A4.GetWidth() * 0.10),//Time 
                        (float)(PageSize.A4.GetWidth() * 0.10),//Qty 
                        (float)(PageSize.A4.GetWidth() * 0.10) //QC 
                }
            ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);



            int SNo = 1;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                double TotalTime = 0, TotalQuantity = 0, TotalQuality = 0; string AccountName = "";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(AccountName))
                        {
                            AccountName = sqlReader["AccountName"].ToString();

                            pdftableMain.AddHeaderCell(new Cell(1, 7).Add(new Paragraph().Add(AccountName)).SetBold().SetBorder(Border.NO_BORDER));

                            pdftableMain.AddHeaderCell(new Cell(1, 4).Add(new Paragraph().Add(" ")).SetBold().SetBorder(Border.NO_BORDER));
                            pdftableMain.AddHeaderCell(new Cell(1, 3).Add(new Paragraph().Add(" Performance ")).SetBold().SetTextAlignment(TextAlignment.CENTER));

                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S.No")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("PO#")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("PO Date")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Time")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quality")).SetBold().SetTextAlignment(TextAlignment.CENTER));
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PONo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PODate"]).ToString("dd-MM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((bool)sqlReader["Performance_Time"] ? "Yes" : "No").ToString())).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(77, 184, 255)).SetTextAlignment(TextAlignment.CENTER));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((bool)sqlReader["Performance_Quantity"] ? "Yes" : "No").ToString())).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(128, 255, 128)).SetTextAlignment(TextAlignment.CENTER));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((bool)sqlReader["Performance_Quality"] ? "Yes" : "No").ToString())).SetKeepTogether(true).SetBackgroundColor(new DeviceRgb(255, 170, 128)).SetTextAlignment(TextAlignment.CENTER));

                        if ((bool)sqlReader["Performance_Time"])
                            TotalTime++;
                        if ((bool)sqlReader["Performance_Quantity"])
                            TotalQuantity++;
                        if ((bool)sqlReader["Performance_Quality"])
                            TotalQuality++;

                        SNo++;
                    }

                }

                SNo--;
                double TotalTime_weightagePer = Math.Round((TotalTime / SNo) * 100),
                       TotalQuantity_weightagePer = Math.Round((TotalQuantity / SNo) * 100), 
                       TotalQuality_weightagePer = Math.Round((TotalQuality / SNo) * 100);

                pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Performance Summary")).SetKeepTogether(true).SetTextAlignment(TextAlignment.RIGHT));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(TotalTime_weightagePer.ToString() + "%")).SetKeepTogether(true).SetTextAlignment(TextAlignment.CENTER));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(TotalQuantity_weightagePer.ToString() + "%")).SetKeepTogether(true).SetTextAlignment(TextAlignment.CENTER));
                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(TotalQuality_weightagePer.ToString() + "%")).SetKeepTogether(true).SetTextAlignment(TextAlignment.CENTER));

                pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Performance Overall")).SetKeepTogether(true).SetTextAlignment(TextAlignment.RIGHT));
                pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add(Math.Round((TotalTime_weightagePer + TotalQuantity_weightagePer + TotalQuality_weightagePer)/3).ToString() + "%")).SetKeepTogether(true).SetTextAlignment(TextAlignment.CENTER).SetBackgroundColor(new DeviceRgb(255, 255, 153)));
            }



            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> SupplierList(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Supplier List By " + SeekBy + " -----", false);

            /////////////------------------------------table for Detail 8------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.10),//AccountCode
                        (float)(PageSize.A4.GetWidth() * 0.15),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.15),//CompanyName 
                        (float)(PageSize.A4.GetWidth() * 0.30),//Address 
                        (float)(PageSize.A4.GetWidth() * 0.10),//NTN/STR 
                        (float)(PageSize.A4.GetWidth() * 0.10),//Supplier_EvaluatedOn
                        (float)(PageSize.A4.GetWidth() * 0.05) //Supplier_EvaluationScore
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Code")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Account Name")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Company Name")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Address")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("NTN/STR")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Evaluated On")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Evaluation Score")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountCode"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CompanyName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Address"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["NTN"].ToString() + "/" + sqlReader["STR"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(
                            sqlReader.IsDBNull("Supplier_EvaluatedOn") ? "" :
                            ((DateTime)sqlReader["Supplier_EvaluatedOn"]).ToString("dd-MMM-yy")
                            )).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Supplier_EvaluationScore"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        
                        SNo++;
                    }

                }
            }

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
    }
    public class CustomerSubDistributorListRepository : ICustomerSubDistributorList
    {
        private readonly OreasDbContext db;
        public CustomerSubDistributorListRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master

        public object GetWCLCustomerSubDistributorListMaster()
        {
            return new[]
            {
                new { n = "by Account Name", v = "byAccountName" }
            }.ToList();
        }

        public async Task<PagedData<object>> LoadCustomerSubDistributorListMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
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
                          TotalSubDistributors = o.tbl_Ac_CustomerSubDistributorLists.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Detail
        public async Task<object> GetCustomerSubDistributorListDetail(int id)
        {
            var qry = from o in await db.tbl_Ac_CustomerSubDistributorLists.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          o.Name,
                          o.Address,
                          o.ContactNo,
                          o.ContactPerson,
                          o.Email,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLCustomerSubDistributorListDetail()
        {
            return new[]
            {
                new { n = "by Name", v = "byName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadCustomerSubDistributorListDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Ac_CustomerSubDistributorLists
                                               .Where(w => w.FK_tbl_Ac_ChartOfAccounts_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byName" && w.Name.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Ac_CustomerSubDistributorLists
                                  .Where(w => w.FK_tbl_Ac_ChartOfAccounts_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.Name.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          o.Name,
                          o.Address,
                          o.ContactNo,
                          o.ContactPerson,
                          o.Email,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostCustomerSubDistributorListDetail(tbl_Ac_CustomerSubDistributorList tbl_Ac_CustomerSubDistributorList, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Ac_CustomerSubDistributorList.CreatedBy = userName;
                tbl_Ac_CustomerSubDistributorList.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Ac_CustomerSubDistributorList.ModifiedBy = userName;
                tbl_Ac_CustomerSubDistributorList.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Ac_CustomerSubDistributorList] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Ac_ChartOfAccounts_ID={4},@Name={5},@Address={6}
                  ,@ContactNo={7},@ContactPerson={8},@Email={9},@Remarks={10}
                  ,@CreatedBy={11},@CreatedDate={12},@ModifiedBy={13},@ModifiedDate={14}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Ac_CustomerSubDistributorList.ID, tbl_Ac_CustomerSubDistributorList.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Ac_CustomerSubDistributorList.Name, tbl_Ac_CustomerSubDistributorList.Address,
                tbl_Ac_CustomerSubDistributorList.ContactNo, tbl_Ac_CustomerSubDistributorList.ContactPerson, tbl_Ac_CustomerSubDistributorList.Email, tbl_Ac_CustomerSubDistributorList.Remarks,
                tbl_Ac_CustomerSubDistributorList.CreatedBy, tbl_Ac_CustomerSubDistributorList.CreatedDate, tbl_Ac_CustomerSubDistributorList.ModifiedBy, tbl_Ac_CustomerSubDistributorList.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }

        #endregion

    }
    public class OrderNoteRepository : IOrderNote
    {
        private readonly OreasDbContext db;
        public OrderNoteRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetOrderNoteMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_OrderNoteMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          o.DocNo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          TargetDate = o.TargetDate.ToString("dd-MMM-yyyy") ?? "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLOrderNoteMaster()
        {
            return new[]
            {
                new { n = "by Customer Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }, new { n = "by Doc No", v = "byDocNo" }
            }.ToList();
        }
        public object GetWCLBOrderNoteMaster()
        {
            return new[]
            {
                new { n = "by Closed", v = "byClosed" }, new { n = "by Open", v = "byOpen" }, new { n = "by Complete Manufacturing", v = "byCompleteManufacturing" }, new { n = "by In-Complete Manufacturing", v = "byInCompleteManufacturing" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrderNoteMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrderNoteMasters
                                               .Where(w =>
                                                        string.IsNullOrEmpty(FilterByLoad)
                                                        ||
                                                        FilterByLoad == "byClosed" && w.tbl_Inv_OrderNoteDetails.Any(a => a.ClosedTrue_OpenFalse != false)
                                                        ||
                                                        FilterByLoad == "byOpen" && w.tbl_Inv_OrderNoteDetails.Any(a => a.ClosedTrue_OpenFalse == false)
                                                        ||
                                                        FilterByLoad == "byCompleteManufacturing" && w.tbl_Inv_OrderNoteDetails.Sum(s => s.Quantity) <= w.tbl_Inv_OrderNoteDetails.Sum(s => s.ManufacturingQty)
                                                        ||
                                                        FilterByLoad == "byInCompleteManufacturing" && w.tbl_Inv_OrderNoteDetails.Sum(s => s.Quantity) > w.tbl_Inv_OrderNoteDetails.Sum(s => s.ManufacturingQty)
                                                         )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_OrderNoteDetails.Any(a => a.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrderNoteMasters
                                  .Where(w =>
                                             string.IsNullOrEmpty(FilterByLoad)
                                             ||
                                             FilterByLoad == "byClosed" && w.tbl_Inv_OrderNoteDetails.Any(a => a.ClosedTrue_OpenFalse != false)
                                             ||
                                             FilterByLoad == "byOpen" && w.tbl_Inv_OrderNoteDetails.Any(a => a.ClosedTrue_OpenFalse == false)
                                             ||
                                             FilterByLoad == "byCompleteManufacturing" && w.tbl_Inv_OrderNoteDetails.Sum(s => s.Quantity) <= w.tbl_Inv_OrderNoteDetails.Sum(s => s.ManufacturingQty)
                                             ||
                                             FilterByLoad == "byInCompleteManufacturing" && w.tbl_Inv_OrderNoteDetails.Sum(s => s.Quantity) > w.tbl_Inv_OrderNoteDetails.Sum(s => s.ManufacturingQty)
                                          )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_OrderNoteDetails.Any(a => a.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          o.DocNo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          TargetDate = o.TargetDate.ToString("dd-MMM-yyyy") ?? "",
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalOrder = o.tbl_Inv_OrderNoteDetails.Count(),
                          TotalOpen = o.tbl_Inv_OrderNoteDetails.Count(a => a.ClosedTrue_OpenFalse == false),
                          TotalClosed = o.tbl_Inv_OrderNoteDetails.Count(a => a.ClosedTrue_OpenFalse == true)
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostOrderNoteMaster(tbl_Inv_OrderNoteMaster tbl_Inv_OrderNoteMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_OrderNoteMaster.CreatedBy = userName;
                tbl_Inv_OrderNoteMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_OrderNoteMaster.ModifiedBy = userName;
                tbl_Inv_OrderNoteMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_OrderNoteMaster] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@DocNo={4},@DocDate={5}
              ,@TargetDate={6},@FK_tbl_Ac_ChartOfAccounts_ID={7} ,@Remarks={8}
              ,@CreatedBy={9},@CreatedDate={10},@ModifiedBy={11},@ModifiedDate={12}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_OrderNoteMaster.ID, tbl_Inv_OrderNoteMaster.DocNo, tbl_Inv_OrderNoteMaster.DocDate,
                tbl_Inv_OrderNoteMaster.TargetDate, tbl_Inv_OrderNoteMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Inv_OrderNoteMaster.Remarks,
                tbl_Inv_OrderNoteMaster.CreatedBy, tbl_Inv_OrderNoteMaster.CreatedDate, tbl_Inv_OrderNoteMaster.ModifiedBy, tbl_Inv_OrderNoteMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetOrderNoteDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_OrderNoteDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrderNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName + "[" + o.tbl_Inv_ProductRegistrationDetail.Split_Into + "'s]",
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                          FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_IDName = o?.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.PackagingName ?? "",
                          o.Quantity,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Remarks,
                          o.MfgQtyLimit,
                          o.ManufacturingQty,
                          o.SoldQty,
                          o.ClosedTrue_OpenFalse,
                          o.Rate,
                          o.CustomMRP,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          UsedInSN_BMR = (o.tbl_Inv_SalesNoteDetails.Count() > 0 || o.tbl_Inv_OrderNoteDetail_ProductionOrders.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters.Count() > 0))
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLOrderNoteDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBOrderNoteDetail()
        {
            return new[]
            {
                new { n = "by Closed", v = "byClosed" },new { n = "by Open", v = "byOpen" }, new { n = "by Complete Manufacturing", v = "byCompleteManufacturing" }, new { n = "by In-Complete Manufacturing", v = "byInCompleteManufacturing" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrderNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrderNoteDetails
                                               .Where(w => w.FK_tbl_Inv_OrderNoteMaster_ID == MasterID)
                                               .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byClosed" && w.ClosedTrue_OpenFalse == true
                                                 ||
                                                 FilterByLoad == "byOpen" && w.ClosedTrue_OpenFalse == false
                                                 ||
                                                 FilterByLoad == "byCompleteManufacturing" && w.Quantity <= w.ManufacturingQty
                                                 ||
                                                 FilterByLoad == "byInCompleteManufacturing" && w.Quantity > w.ManufacturingQty
                                                 )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrderNoteDetails
                                  .Where(w => w.FK_tbl_Inv_OrderNoteMaster_ID == MasterID)
                                  .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byClosed" && w.ClosedTrue_OpenFalse == true
                                                 ||
                                                 FilterByLoad == "byOpen" && w.ClosedTrue_OpenFalse == false
                                                 ||
                                                 FilterByLoad == "byCompleteManufacturing" && w.Quantity <= w.ManufacturingQty
                                                 ||
                                                 FilterByLoad == "byInCompleteManufacturing" && w.Quantity > w.ManufacturingQty
                                                 )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
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
                          o.Quantity,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Remarks,
                          o.MfgQtyLimit,
                          o.ManufacturingQty,
                          o.SoldQty,
                          o.ClosedTrue_OpenFalse,
                          o.Rate,
                          o.CustomMRP,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          SubDistributors = o.tbl_Inv_OrderNoteDetail_SubDistributors.Count()
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostOrderNoteDetail(tbl_Inv_OrderNoteDetail tbl_Inv_OrderNoteDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_OrderNoteDetail.CreatedBy = userName;
                tbl_Inv_OrderNoteDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_OrderNoteDetail.ModifiedBy = userName;
                tbl_Inv_OrderNoteDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_OrderNoteDetail] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Inv_OrderNoteMaster_ID={4}
              ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5},@FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID={6}
              ,@Quantity={7},@FK_AspNetOreasPriority_ID={8},@Remarks={9},@MfgQtyLimit={10},@ManufacturingQty={11}
              ,@SoldQty={12},@ClosedTrue_OpenFalse={13},@Rate={14}, @CustomMRP={15}
              ,@CreatedBy={16},@CreatedDate={17},@ModifiedBy={18},@ModifiedDate={19}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_OrderNoteDetail.ID, tbl_Inv_OrderNoteDetail.FK_tbl_Inv_OrderNoteMaster_ID,
                tbl_Inv_OrderNoteDetail.FK_tbl_Inv_ProductRegistrationDetail_ID, tbl_Inv_OrderNoteDetail.FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID,
                tbl_Inv_OrderNoteDetail.Quantity, tbl_Inv_OrderNoteDetail.FK_AspNetOreasPriority_ID, tbl_Inv_OrderNoteDetail.Remarks, tbl_Inv_OrderNoteDetail.MfgQtyLimit, tbl_Inv_OrderNoteDetail.ManufacturingQty,
                tbl_Inv_OrderNoteDetail.SoldQty, tbl_Inv_OrderNoteDetail.ClosedTrue_OpenFalse, tbl_Inv_OrderNoteDetail.Rate, tbl_Inv_OrderNoteDetail.CustomMRP,
                tbl_Inv_OrderNoteDetail.CreatedBy, tbl_Inv_OrderNoteDetail.CreatedDate, tbl_Inv_OrderNoteDetail.ModifiedBy, tbl_Inv_OrderNoteDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }

        #endregion

        #region Detail SubDistributor
        public async Task<object> GetOrderNoteDetailSubDistributor(int id)
        {
            var qry = from o in await db.tbl_Inv_OrderNoteDetail_SubDistributors.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrderNoteDetail_ID,
                          o.FK_tbl_Ac_CustomerSubDistributorList_ID,
                          FK_tbl_Ac_CustomerSubDistributorList_IDName = o.tbl_Ac_CustomerSubDistributorList.Name,
                          o.tbl_Ac_CustomerSubDistributorList.Address,
                          o.Quantity,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLOrderNoteDetailSubDistributor()
        {
            return new[]
            {
                new { n = "by SubDistributor Name", v = "bySubDistributorName" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadOrderNoteDetailSubDistributor(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_OrderNoteDetail_SubDistributors
                                               .Where(w => w.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "bySubDistributorName" && w.tbl_Ac_CustomerSubDistributorList.Name.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_OrderNoteDetail_SubDistributors
                                  .Where(w => w.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "bySubDistributorName" && w.tbl_Ac_CustomerSubDistributorList.Name.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

           select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_OrderNoteDetail_ID,
                          o.FK_tbl_Ac_CustomerSubDistributorList_ID,
                          FK_tbl_Ac_CustomerSubDistributorList_IDName = o.tbl_Ac_CustomerSubDistributorList.Name,
                          o.tbl_Ac_CustomerSubDistributorList.Address,
                          o.Quantity,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostOrderNoteDetailSubDistributor(tbl_Inv_OrderNoteDetail_SubDistributor tbl_Inv_OrderNoteDetail_SubDistributor, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_OrderNoteDetail_SubDistributor.CreatedBy = userName;
                tbl_Inv_OrderNoteDetail_SubDistributor.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_OrderNoteDetail_SubDistributor.ModifiedBy = userName;
                tbl_Inv_OrderNoteDetail_SubDistributor.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_OrderNoteDetail_SubDistributor] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Inv_OrderNoteDetail_ID={4}
                ,@FK_tbl_Ac_CustomerSubDistributorList_ID={5},@Quantity={6}
                ,@CreatedBy={7},@CreatedDate={8},@ModifiedBy={9},@ModifiedDate={10}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_OrderNoteDetail_SubDistributor.ID, tbl_Inv_OrderNoteDetail_SubDistributor.FK_tbl_Inv_OrderNoteDetail_ID,
                tbl_Inv_OrderNoteDetail_SubDistributor.FK_tbl_Ac_CustomerSubDistributorList_ID, tbl_Inv_OrderNoteDetail_SubDistributor.Quantity,
                tbl_Inv_OrderNoteDetail_SubDistributor.CreatedBy, tbl_Inv_OrderNoteDetail_SubDistributor.CreatedDate, tbl_Inv_OrderNoteDetail_SubDistributor.ModifiedBy, tbl_Inv_OrderNoteDetail_SubDistributor.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }

        #endregion

        #region BMRDetail
        public object GetWCLBMRDetail()
        {
            return new[]
            {
                new { n = "by BatchNo", v = "byBatchNo" }, new { n = "by ProductName", v = "byProductName" }, new { n = "by Ingredient ProductName", v = "byProductNameIngredient" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadBMRDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters
                                      .Where(w => w.tbl_Inv_OrderNoteDetail_ProductionOrder.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byBatchNo" && w.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                 ||
                                                 FilterByText == "byProductNameIngredient" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters
                                        .Where(w => w.tbl_Inv_OrderNoteDetail_ProductionOrder.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byBatchNo" && w.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductNameIngredient" && w.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails.Any(a => a.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss.Any(b => b.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())))
                                                       )
                                        .OrderByDescending(i => i.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.FK_tbl_Pro_BatchMaterialRequisitionMaster_ID,
                          o.tbl_Pro_BatchMaterialRequisitionMaster.BatchNo,
                          BatchMfgDate = o.tbl_Pro_BatchMaterialRequisitionMaster.BatchMfgDate.ToString("dd-MMM-yyyy"),
                          o.BatchSize,
                          PrimaryProduct = o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail_Primary.tbl_Inv_MeasurementUnit.MeasurementUnit + "]",
                          PrimarySecondary = o.FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary > 0 ? o.tbl_Inv_ProductRegistrationDetail_Secondary.tbl_Inv_ProductRegistrationMaster.ProductName + " [" + o.tbl_Inv_ProductRegistrationDetail_Secondary.tbl_Inv_MeasurementUnit.MeasurementUnit + "]" : "",
                          o.PackagingName
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region SalesNoteDetail
        public object GetWCLSalesNoteDetail()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesNoteDetails
                                      .Where(w => w.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byDocNo" && w.tbl_Inv_SalesNoteMaster.DocNo.ToString() == FilterValueByText
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesNoteDetails
                                        .Where(w => w.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byDocNo" && w.tbl_Inv_SalesNoteMaster.DocNo.ToString() == FilterValueByText
                                                       )
                                        .OrderByDescending(i => i.tbl_Inv_SalesNoteMaster.DocNo).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.tbl_Inv_SalesNoteMaster.DocNo,
                          DocDate = o.tbl_Inv_SalesNoteMaster.DocDate.ToString("dd-MMM-yy"),
                          o.FK_tbl_Inv_SalesNoteMaster_ID,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region SalesReturnNoteDetail
        public object GetWCLSalesReturnNoteDetail()
        {
            return new[]
            {
                new { n = "by DocNo", v = "byDocNo" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadSalesReturnNoteDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_SalesReturnNoteDetails
                                      .Where(w => w.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                      .Where(w =>
                                                 string.IsNullOrEmpty(FilterValueByText)
                                                 ||
                                                 FilterByText == "byDocNo" && w.tbl_Inv_SalesReturnNoteMaster.DocNo.ToString() == FilterValueByText
                                             )
                                       .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_SalesReturnNoteDetails
                                        .Where(w => w.tbl_Inv_SalesNoteDetail.FK_tbl_Inv_OrderNoteDetail_ID == MasterID)
                                        .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byDocNo" && w.tbl_Inv_SalesReturnNoteMaster.DocNo.ToString() == FilterValueByText
                                                       )
                                        .OrderByDescending(i => i.tbl_Inv_SalesReturnNoteMaster.DocNo).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.tbl_Inv_SalesReturnNoteMaster.DocNo,
                          DocDate = o.tbl_Inv_SalesReturnNoteMaster.DocDate.ToString("dd-MMM-yy"),
                          o.FK_tbl_Inv_SalesReturnNoteMaster_ID,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.Quantity
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Report  
        public List<ReportCallingModel> GetRLOrderNoteMaster()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Material Status Order Note",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.NonPeriodicNonSerial,
                    ReportName ="Mfg Stock On Order Note",
                    GroupBy = new List<string>(){ "Customer", "Product" },
                    OrderBy = null,
                    SeekBy = null
                }
            };
        }
        public List<ReportCallingModel> GetRLOrderNote()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Order Note",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Order Note",
                    GroupBy = new List<string>(){ "Customer", "Product" },
                    OrderBy = new List<string>(){ "Doc Date", "Doc No" },
                    SeekBy = new List<string>(){ "Closed", "Open", "All" },
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Order Note")
            {
                return await Task.Run(() => CurrentOrderNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Order Note")
            {
                return await Task.Run(() => RegisterOrderNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Mfg Stock On Order Note")
            {
                return await Task.Run(() => MfgStockOnOrderNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Material Status Order Note")
            {
                return await Task.Run(() => MaterialStatusOrderNote(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Material Status Order Note Detail")
            {
                return await Task.Run(() => MaterialStatusOrderNoteDetail(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> MaterialStatusOrderNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Order Note Material Status Of Open Orders as on " + DateTime.Now.ToString("dd-MMM-yy") + " -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 8------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.09), //DocNo
                        (float)(PageSize.A4.GetWidth() * 0.08), //DocDate
                        (float)(PageSize.A4.GetWidth() * 0.08), //TargetDate
                        (float)(PageSize.A4.GetWidth() * 0.24), //AccountName
                        (float)(PageSize.A4.GetWidth() * 0.24), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.09), //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.09), //ManufacturingQuantity
                        (float)(PageSize.A4.GetWidth() * 0.09) //SoldQuantity
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Target Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Customer")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Product")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Order Qty")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Mfg Qty")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Sold Qty")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string ProductName = "";


                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["TargetDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "] [" + sqlReader["Split_Into"].ToString() + "'s]")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ManufacturingQty"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["SoldQty"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 7------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.07), //S No
                        (float)(PageSize.A4.GetWidth() * 0.08),  //Type
                        (float)(PageSize.A4.GetWidth() * 0.18), //CategoryName
                        (float)(PageSize.A4.GetWidth() * 0.31),  //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.09),  //RequiredQty
                        (float)(PageSize.A4.GetWidth() * 0.09),  //InHand
                        (float)(PageSize.A4.GetWidth() * 0.09),  //PO Balance
                        (float)(PageSize.A4.GetWidth() * 0.09)  //Req Balance
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 8).Add(new Paragraph().Add("---- Material Detail ----")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Formula Type")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Category")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Ingredient Name")).SetBold().SetTextAlignment(TextAlignment.LEFT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Req Qty")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("InHand")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("PO Bal")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Req Bal")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";
                int SNo = 1; double ReqQty = 0; double InHand = 0; double POBalance = 0; decimal ReqBalance = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        ReqQty = 0; InHand = 0; POBalance = 0; ReqBalance = 0; ProductName = "";

                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RawORPackaging"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CategoryName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        //pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]")).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                        ProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";

                        pdftableDetail.AddCell(new Cell(1, 1).Add(new Paragraph().Add(new Link(ProductName, PdfAction.CreateURI("?rn=Material Status Order Note Detail" + "&id=" + sqlReader["ProdID"].ToString() + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + GroupID)))).SetFontColor(new DeviceRgb(0, 102, 204)).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequiredQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["InHand"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["POBalance"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                        ReqQty = sqlReader.GetDouble("RequiredQty");
                        InHand = sqlReader.GetDouble("InHand");
                        POBalance = sqlReader.GetDouble("POBalance");
                        ReqBalance = (decimal)(ReqQty - InHand - POBalance);

                        if (ReqBalance > 0)
                            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(ReqBalance.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBackgroundColor(new DeviceRgb(229, 126, 159)).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        else
                            pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("0")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));




                        SNo++;
                    }
                }

                page.InsertContent(pdftableDetail);



                /////////////------------------------------table for detail 4------------------------------////////////////
                Table pdftableDetail2 = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10), //S No
                        (float)(PageSize.A4.GetWidth() * 0.20),  //Category
                        (float)(PageSize.A4.GetWidth() * 0.60),  //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.10)  //RequiredQty
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail2.AddCell(new Cell(1, 6).Add(new Paragraph().Add("---- BMR Pending Material Detail ----")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail2.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail2.AddCell(new Cell().Add(new Paragraph().Add("Category")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail2.AddCell(new Cell().Add(new Paragraph().Add("Ingredient Name")).SetBold().SetTextAlignment(TextAlignment.LEFT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail2.AddCell(new Cell().Add(new Paragraph().Add("Req Qty")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                ReportName.Value = rn + "3";
                SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail2.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail2.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["CategoryName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail2.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail2.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequiredQty"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                        SNo++;
                    }
                }

                page.InsertContent(pdftableDetail2);



            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> MaterialStatusOrderNoteDetail(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Order Note Ingredient Detail -----", true);

            /////////////------------------------------table for Detail 7------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.09),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.09),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.09),//OrderQty 
                        (float)(PageSize.A4.GetWidth() * 0.29),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.29),//Product Name 
                        (float)(PageSize.A4.GetWidth() * 0.10) //RequiredQty
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);



            int SNo = 1;
            double GrandTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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

                string MainProductName = "";

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        if (string.IsNullOrEmpty(MainProductName))
                        {
                            MainProductName = sqlReader["ProductName"].ToString() + " [" + sqlReader["MeasurementUnit"].ToString() + "]";
                            pdftableMain.AddHeaderCell(new Cell(1, 2).Add(new Paragraph().Add("Ingredient:")).SetBold().SetFontSize(10).SetKeepTogether(true));
                            pdftableMain.AddHeaderCell(new Cell(1, 5).Add(new Paragraph().Add(MainProductName)).SetBold().SetFontSize(10).SetKeepTogether(true));

                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Order No")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Order Date")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Order Qty")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Customer")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold());
                            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Req Qty")).SetBold());
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["OrderQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductNameOrder"].ToString() + " [" + sqlReader["MeasurementUnitOrder"].ToString() + "]")).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["RequiredQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));

                        GrandTotalQty += Convert.ToDouble(sqlReader["RequiredQty"]);
                        SNo++;
                    }

                }
            }


            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> CurrentOrderNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Order Note -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocNo
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocDate
                        (float)(PageSize.A4.GetWidth() * 0.65), //AccountName
                        (float)(PageSize.A4.GetWidth() * 0.15)  //TargetDate
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Customer")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Target Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["TargetDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 8------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05), //S No
                        (float)(PageSize.A4.GetWidth() * 0.10),  //Priority
                        (float)(PageSize.A4.GetWidth() * 0.40), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.10),  //ManufacturingQty
                        (float)(PageSize.A4.GetWidth() * 0.10),  //SoldQty
                        (float)(PageSize.A4.GetWidth() * 0.05),  //Status
                        (float)(PageSize.A4.GetWidth() * 0.05)  //Rate
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 8).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Priority")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Mfg Qty")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Sold Qty")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Status")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";
                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Priority"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ManufacturingQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["SoldQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Status"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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
        private async Task<byte[]> RegisterOrderNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Order Note Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 12------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.05),//DocDate 
                        (float)(PageSize.A4.GetWidth() * 0.05),//TargetDate 
                        (float)(PageSize.A4.GetWidth() * 0.19),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.10),//Priority 
                        (float)(PageSize.A4.GetWidth() * 0.23),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.08),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//ManufacturingQty 
                        (float)(PageSize.A4.GetWidth() * 0.05),//SoldQty
                        (float)(PageSize.A4.GetWidth() * 0.05),//Status
                        (float)(PageSize.A4.GetWidth() * 0.05)//Rate
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Target Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Customer")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Priority")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Mfg Qty")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Sold Qty")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Status")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string GroupbyFieldName = GroupBy == "Customer" ? "AccountName" :
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
                                pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 12).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 12).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["TargetDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Priority"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ManufacturingQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["SoldQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Status"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        SNo++;
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 12).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 4).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        private async Task<byte[]> MfgStockOnOrderNote(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {


            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Mfg Stock for Order " + "-----", false);

            /////////////------------------------------table for Detail 9------------------------------////////////////
            Table pdftableMain = new Table(new float[]
                {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.25),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.08),//BalanceByWareHouse 
                        (float)(PageSize.A4.GetWidth() * 0.08),//BatchNo 
                        (float)(PageSize.A4.GetWidth() * 0.20),//WareHouseName 
                        (float)(PageSize.A4.GetWidth() * 0.20),//OCustomerName 
                        (float)(PageSize.A4.GetWidth() * 0.08),//OTargetDate 
                        (float)(PageSize.A4.GetWidth() * 0.08),//OQty
                        (float)(PageSize.A4.GetWidth() * 0.08) //OSoldQty
                }
            ).UseAllAvailableWidth().SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Balance")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Batch No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Warehouse")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Customer")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Target")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("O Qty")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Sold")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string GroupbyFieldName = GroupBy == "Customer" ? "AccountName" :
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
                                pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + (datefrom.HasValue ? datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") : "") + "&datetill=" + (datetill.HasValue ? datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") : "") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BalanceByWareHouse"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["BatchNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["WareHouseName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["OTargetDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["OQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["OSoldQty"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true));

                        GroupTotalQty += Convert.ToDouble(sqlReader["BalanceByWareHouse"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["BalanceByWareHouse"]);

                        SNo++;
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 9).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 2).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
        #endregion

    }
    public class PurchaseOrderRepository : IPurchaseOrder
    {
        private readonly OreasDbContext db;
        public PurchaseOrderRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetPurchaseOrderMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrderMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          PODate = o.PODate.ToString("dd-MMM-yyyy") ?? "",
                          o.PONo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          TargetDate = o.TargetDate.ToString("dd-MMM-yyyy") ?? "",
                          o.Remarks,
                          o.FK_tbl_Inv_PurchaseOrderTermsConditions_ID,
                          FK_tbl_Inv_PurchaseOrderTermsConditions_IDName = o?.tbl_Inv_PurchaseOrderTermsConditions?.TCName ?? "",
                          o.LocalTrue_ImportFalse,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public async Task<object> GetPurchaseOrderImportMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrderMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          PODate = o.PODate.ToString("dd-MMM-yyyy") ?? "",
                          o.PONo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          TargetDate = o.TargetDate.ToString("dd-MMM-yyyy") ?? "",
                          o.Remarks,
                          o.FK_tbl_Inv_PurchaseOrderTermsConditions_ID,
                          FK_tbl_Inv_PurchaseOrderTermsConditions_IDName = o?.tbl_Inv_PurchaseOrderTermsConditions?.TCName ?? "",
                          o.LocalTrue_ImportFalse,
                          o.FK_tbl_Inv_PurchaseOrder_Supplier_ID,
                          FK_tbl_Inv_PurchaseOrder_Supplier_IDName = o?.tbl_Inv_PurchaseOrder_Supplier?.SupplierName ?? "",
                          o.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                          FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName = o?.tbl_Inv_PurchaseOrder_Manufacturer?.ManufacturerName ?? "",
                          o.FK_tbl_Inv_PurchaseOrder_Indenter_ID,
                          FK_tbl_Inv_PurchaseOrder_Indenter_IDName = o?.tbl_Inv_PurchaseOrder_Indenter?.IndenterName ?? "",
                          IndentDate = o.IndentDate.HasValue ? o.IndentDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.IndentNo,
                          o.FK_tbl_Inv_PurchaseOrder_ImportTerms_ID,
                          FK_tbl_Inv_PurchaseOrder_ImportTerms_IDName = o?.tbl_Inv_PurchaseOrder_ImportTerms?.TermName ?? "",
                          o.FK_tbl_Ac_CurrencyAndCountry_ID_Currency,
                          FK_tbl_Ac_CurrencyAndCountry_ID_CurrencyName = o?.tbl_Ac_CurrencyAndCountry_Currency?.CurrencyCode ?? "",
                          o.FK_tbl_Ac_CurrencyAndCountry_ID_CountryOfOrigin,
                          FK_tbl_Ac_CurrencyAndCountry_ID_CountryOfOriginName = o?.tbl_Ac_CurrencyAndCountry_CountryOfOrigin?.CountryName ?? "",
                          ShipmentDate = o.ShipmentDate.HasValue ? o.ShipmentDate.Value.ToString("dd-MMM-yyyy") : "",
                          NegotiationDate = o.NegotiationDate.HasValue ? o.NegotiationDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Inv_TransportType_ID,
                          FK_tbl_Inv_TransportType_IDName = o?.tbl_Inv_TransportType?.TypeName ?? "",
                          o.FK_tbl_Inv_InternationalCommercialTerm_ID,
                          FK_tbl_Inv_InternationalCommercialTerm_IDName = o?.tbl_Inv_InternationalCommercialTerm?.IncotermName ?? "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseOrderMaster()
        {
            return new[]
            {
                new { n = "by Supplier Name", v = "byAccountName" }, new { n = "by Product Name", v = "byProductName" }, new { n = "by Doc No", v = "byPONo" }
            }.ToList();
        }
        public object GetWCLPurchaseOrderImportMaster()
        {
            return new[]
            {
                new { n = "by Supplier Name", v = "bySupplierName" }, new { n = "by Indenter Name", v = "byIndenterName" },
                new { n = "by Product Name", v = "byProductName" }, new { n = "by Doc No", v = "byPONo" }
            }.ToList();
        }
        public object GetWCLBPurchaseOrderMaster()
        {
            return new[]
            {
                new { n = "by Closed", v = "byClosed" },new { n = "by Open", v = "byOpen" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseOrderMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "", bool IsCanViewOnlyOwnData = false)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrderMasters
                                               .Where(w=> w.LocalTrue_ImportFalse)
                                               .Where(w =>
                                                        string.IsNullOrEmpty(FilterByLoad)
                                                        ||
                                                        FilterByLoad == "byClosed" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.ClosedTrue_OpenFalse != false)
                                                        ||
                                                        FilterByLoad == "byOpen" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.ClosedTrue_OpenFalse == false)
                                                         )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byPONo" && w.PONo.ToString() == FilterValueByText
                                                     )
                                               .Where(w=>
                                                         IsCanViewOnlyOwnData == true && w.CreatedBy.Equals(userName)
                                                         ||
                                                         IsCanViewOnlyOwnData == false
                                                    )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrderMasters
                                  .Where(w => w.LocalTrue_ImportFalse)
                                  .Where(w =>
                                             string.IsNullOrEmpty(FilterByLoad)
                                             ||
                                             FilterByLoad == "byClosed" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.ClosedTrue_OpenFalse != false)
                                             ||
                                             FilterByLoad == "byOpen" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.ClosedTrue_OpenFalse == false)
                                          )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byAccountName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byPONo" && w.PONo.ToString() == FilterValueByText
                                      )
                                  .Where(w =>
                                            IsCanViewOnlyOwnData == true && w.CreatedBy.Equals(userName)
                                            ||
                                            IsCanViewOnlyOwnData == false
                                        )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          PODate = o.PODate.ToString("dd-MMM-yyyy") ?? "",
                          o.PONo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          TargetDate = o.TargetDate.ToString("dd-MMM-yyyy") ?? "",
                          o.Remarks,
                          o.FK_tbl_Inv_PurchaseOrderTermsConditions_ID,
                          FK_tbl_Inv_PurchaseOrderTermsConditions_IDName = o?.tbl_Inv_PurchaseOrderTermsConditions?.TCName ?? "",
                          o.LocalTrue_ImportFalse,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalOrder = o.tbl_Inv_PurchaseOrderDetails.Count(),
                          TotalOpen = o.tbl_Inv_PurchaseOrderDetails.Count(a => a.ClosedTrue_OpenFalse == false),
                          TotalClosed = o.tbl_Inv_PurchaseOrderDetails.Count(a => a.ClosedTrue_OpenFalse == true),
                          DefaultWHTPer = o.tbl_Ac_ChartOfAccounts?.tbl_Ac_PolicyWHTaxOnPurchase?.WHTaxPer ?? 0,
                          o.tbl_Ac_ChartOfAccounts.Email,
                          o.tbl_Ac_ChartOfAccounts.ContactPersonName
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<PagedData<object>> LoadPurchaseOrderImportMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "", bool IsCanViewOnlyOwnData = false)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrderMasters
                                               .Where(w => w.LocalTrue_ImportFalse == false)
                                               .Where(w =>
                                                        string.IsNullOrEmpty(FilterByLoad)
                                                        ||
                                                        FilterByLoad == "byClosed" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.ClosedTrue_OpenFalse != false)
                                                        ||
                                                        FilterByLoad == "byOpen" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.ClosedTrue_OpenFalse == false)
                                                         )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "bySupplierName" && w.tbl_Inv_PurchaseOrder_Supplier.SupplierName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byIndenterName" && w.tbl_Inv_PurchaseOrder_Indenter.IndenterName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                                       ||
                                                       FilterByText == "byPONo" && w.PONo.ToString() == FilterValueByText
                                                     )
                                               .Where(w =>
                                                         IsCanViewOnlyOwnData == true && w.CreatedBy.Equals(userName)
                                                         ||
                                                         IsCanViewOnlyOwnData == false
                                                    )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrderMasters
                                  .Where(w => w.LocalTrue_ImportFalse == false)
                                  .Where(w =>
                                             string.IsNullOrEmpty(FilterByLoad)
                                             ||
                                             FilterByLoad == "byClosed" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.ClosedTrue_OpenFalse != false)
                                             ||
                                             FilterByLoad == "byOpen" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.ClosedTrue_OpenFalse == false)
                                          )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "bySupplierName" && w.tbl_Inv_PurchaseOrder_Supplier.SupplierName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byIndenterName" && w.tbl_Inv_PurchaseOrder_Indenter.IndenterName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_PurchaseOrderDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byPONo" && w.PONo.ToString() == FilterValueByText
                                      )
                                  .Where(w =>
                                            IsCanViewOnlyOwnData == true && w.CreatedBy.Equals(userName)
                                            ||
                                            IsCanViewOnlyOwnData == false
                                        )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          PODate = o.PODate.ToString("dd-MMM-yyyy") ?? "",
                          o.PONo,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          TargetDate = o.TargetDate.ToString("dd-MMM-yyyy") ?? "",
                          o.Remarks,
                          o.FK_tbl_Inv_PurchaseOrderTermsConditions_ID,
                          FK_tbl_Inv_PurchaseOrderTermsConditions_IDName = o?.tbl_Inv_PurchaseOrderTermsConditions?.TCName ?? "",
                          o.LocalTrue_ImportFalse,
                          o.FK_tbl_Inv_PurchaseOrder_Supplier_ID,
                          FK_tbl_Inv_PurchaseOrder_Supplier_IDName = o?.tbl_Inv_PurchaseOrder_Supplier?.SupplierName ?? "",
                          o.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                          FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName = o?.tbl_Inv_PurchaseOrder_Manufacturer?.ManufacturerName ?? "",
                          o.FK_tbl_Inv_PurchaseOrder_Indenter_ID,
                          FK_tbl_Inv_PurchaseOrder_Indenter_IDName = o?.tbl_Inv_PurchaseOrder_Indenter?.IndenterName ?? "",
                          IndentDate = o.IndentDate.HasValue ? o.IndentDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.IndentNo,
                          o.FK_tbl_Inv_PurchaseOrder_ImportTerms_ID,
                          FK_tbl_Inv_PurchaseOrder_ImportTerms_IDName = o?.tbl_Inv_PurchaseOrder_ImportTerms?.TermName ?? "",
                          o.FK_tbl_Ac_CurrencyAndCountry_ID_Currency,
                          FK_tbl_Ac_CurrencyAndCountry_ID_CurrencyName = o?.tbl_Ac_CurrencyAndCountry_Currency?.CurrencyCode ?? "",
                          o.FK_tbl_Ac_CurrencyAndCountry_ID_CountryOfOrigin,
                          FK_tbl_Ac_CurrencyAndCountry_ID_CountryOfOriginName = o?.tbl_Ac_CurrencyAndCountry_CountryOfOrigin?.CountryName ?? "",
                          ShipmentDate = o.ShipmentDate.HasValue ? o.ShipmentDate.Value.ToString("dd-MMM-yyyy") : "",
                          NegotiationDate = o.NegotiationDate.HasValue ? o.NegotiationDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.FK_tbl_Inv_TransportType_ID,
                          FK_tbl_Inv_TransportType_IDName = o?.tbl_Inv_TransportType?.TypeName ?? "",
                          o.FK_tbl_Inv_InternationalCommercialTerm_ID,
                          FK_tbl_Inv_InternationalCommercialTerm_IDName = o.FK_tbl_Inv_InternationalCommercialTerm_ID.HasValue ?  o.tbl_Inv_InternationalCommercialTerm.IncotermName + " ["+ o.tbl_Inv_InternationalCommercialTerm.Abbreviation + "]" : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalOrder = o.tbl_Inv_PurchaseOrderDetails.Count(),
                          TotalOpen = o.tbl_Inv_PurchaseOrderDetails.Count(a => a.ClosedTrue_OpenFalse == false),
                          TotalClosed = o.tbl_Inv_PurchaseOrderDetails.Count(a => a.ClosedTrue_OpenFalse == true),
                          DefaultWHTPer = 0,//o.tbl_Ac_ChartOfAccounts?.tbl_Ac_PolicyWHTaxOnPurchase?.WHTaxPer ?? 0,
                          Email = "",//o.tbl_Ac_ChartOfAccounts.Email,
                          ContactPersonName = "" //o.tbl_Ac_ChartOfAccounts.ContactPersonName
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseOrderMaster(tbl_Inv_PurchaseOrderMaster tbl_Inv_PurchaseOrderMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PurchaseOrderMaster.CreatedBy = userName;
                tbl_Inv_PurchaseOrderMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseOrderMaster.ModifiedBy = userName;
                tbl_Inv_PurchaseOrderMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseOrderMaster] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@PONo={4},@PODate={5},@TargetDate={6}
                ,@FK_tbl_Ac_ChartOfAccounts_ID={7},@Remarks={8}
                ,@FK_tbl_Inv_PurchaseOrderTermsConditions_ID={9},@LocalTrue_ImportFalse={10}
                ,@FK_tbl_Inv_PurchaseOrder_Supplier_ID={11},@FK_tbl_Inv_PurchaseOrder_Manufacturer_ID={12}
                ,@FK_tbl_Inv_PurchaseOrder_Indenter_ID={13},@IndentDate={14},@IndentNo={15}
                ,@FK_tbl_Inv_PurchaseOrder_ImportTerms_ID={16},@FK_tbl_Ac_CurrencyAndCountry_ID_Currency={17}
                ,@FK_tbl_Ac_CurrencyAndCountry_ID_CountryOfOrigin={18},@ShipmentDate={19},@NegotiationDate={20}
                ,@FK_tbl_Inv_TransportType_ID={21},@FK_tbl_Inv_InternationalCommercialTerm_ID={22}
                ,@CreatedBy={23},@CreatedDate={24},@ModifiedBy={25},@ModifiedDate={26}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseOrderMaster.ID, tbl_Inv_PurchaseOrderMaster.PONo, tbl_Inv_PurchaseOrderMaster.PODate,
                tbl_Inv_PurchaseOrderMaster.TargetDate, tbl_Inv_PurchaseOrderMaster.FK_tbl_Ac_ChartOfAccounts_ID, tbl_Inv_PurchaseOrderMaster.Remarks, tbl_Inv_PurchaseOrderMaster.FK_tbl_Inv_PurchaseOrderTermsConditions_ID, tbl_Inv_PurchaseOrderMaster.LocalTrue_ImportFalse,
                tbl_Inv_PurchaseOrderMaster.FK_tbl_Inv_PurchaseOrder_Supplier_ID, tbl_Inv_PurchaseOrderMaster.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                tbl_Inv_PurchaseOrderMaster.FK_tbl_Inv_PurchaseOrder_Indenter_ID, tbl_Inv_PurchaseOrderMaster.IndentDate, tbl_Inv_PurchaseOrderMaster.IndentNo,
                tbl_Inv_PurchaseOrderMaster.FK_tbl_Inv_PurchaseOrder_ImportTerms_ID, tbl_Inv_PurchaseOrderMaster.FK_tbl_Ac_CurrencyAndCountry_ID_Currency,
                tbl_Inv_PurchaseOrderMaster.FK_tbl_Ac_CurrencyAndCountry_ID_CountryOfOrigin, tbl_Inv_PurchaseOrderMaster.ShipmentDate, tbl_Inv_PurchaseOrderMaster.NegotiationDate,
                tbl_Inv_PurchaseOrderMaster.FK_tbl_Inv_TransportType_ID, tbl_Inv_PurchaseOrderMaster.FK_tbl_Inv_InternationalCommercialTerm_ID,
                tbl_Inv_PurchaseOrderMaster.CreatedBy, tbl_Inv_PurchaseOrderMaster.CreatedDate, tbl_Inv_PurchaseOrderMaster.ModifiedBy, tbl_Inv_PurchaseOrderMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetPurchaseOrderDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrderDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseOrderMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Quantity,
                          o.Rate,
                          o.GSTPercentage,
                          o.DiscountAmount,
                          o.WHTPercentage,
                          o.NetAmount,
                          o.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                          FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName = o?.tbl_Inv_PurchaseOrder_Manufacturer?.ManufacturerName ?? "",
                          o.Remarks,
                          o.ReceivedQty,
                          o.ClosedTrue_OpenFalse,
                          o.Performance_Time,
                          o.Performance_Quantity,
                          o.Performance_Quality,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public async Task<object> GetPurchaseOrderImportDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrderDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseOrderMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Quantity,
                          o.Rate,
                          o.GSTPercentage,
                          o.DiscountAmount,
                          o.WHTPercentage,
                          o.NetAmount,
                          o.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                          FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName = o?.tbl_Inv_PurchaseOrder_Manufacturer?.ManufacturerName ?? "",
                          o.Remarks,
                          o.ReceivedQty,
                          o.ClosedTrue_OpenFalse,
                          o.Performance_Time,
                          o.Performance_Quantity,
                          o.Performance_Quality,
                          o.FK_tbl_Inv_MeasurementUnit_ID_Supplier,
                          FK_tbl_Inv_MeasurementUnit_ID_SupplierName = o.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.QuantityAsPerSupplierUnit,
                          o.UnitFactorToConvertInLocalUnit,
                          o.Packaging,
                          o.BatchNo,
                          MfgDate = o.MfgDate.HasValue ? o.MfgDate.Value.ToString("dd-MMM-yyyy") : "",
                          ExpiryDate = o.ExpiryDate.HasValue ? o.ExpiryDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseOrderDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBPurchaseOrderDetail()
        {
            return new[]
            {
                new { n = "by Closed", v = "byClosed" },new { n = "by Open", v = "byOpen" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseOrderDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrderDetails
                                               .Where(w => w.FK_tbl_Inv_PurchaseOrderMaster_ID == MasterID)
                                               .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byClosed" && w.ClosedTrue_OpenFalse == true
                                                 ||
                                                 FilterByLoad == "byOpen" && w.ClosedTrue_OpenFalse == false
                                                 )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrderDetails
                                  .Where(w => w.FK_tbl_Inv_PurchaseOrderMaster_ID == MasterID)
                                  .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byClosed" && w.ClosedTrue_OpenFalse == true
                                                 ||
                                                 FilterByLoad == "byOpen" && w.ClosedTrue_OpenFalse == false
                                                 )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseOrderMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Quantity,
                          o.Rate,
                          o.GSTPercentage,
                          o.DiscountAmount,
                          o.WHTPercentage,
                          o.NetAmount,
                          o.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                          FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName = o?.tbl_Inv_PurchaseOrder_Manufacturer?.ManufacturerName ?? "",
                          o.Remarks,
                          o.ReceivedQty,
                          o.ClosedTrue_OpenFalse,
                          o.Performance_Time,
                          o.Performance_Quantity,
                          o.Performance_Quality,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalReceivings = o.tbl_Inv_PurchaseNoteDetails.Count()
                      };




            pageddata.Data = qry;            

            return pageddata;
        }
        public async Task<PagedData<object>> LoadPurchaseOrderImportDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrderDetails
                                               .Where(w => w.FK_tbl_Inv_PurchaseOrderMaster_ID == MasterID)
                                               .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byClosed" && w.ClosedTrue_OpenFalse == true
                                                 ||
                                                 FilterByLoad == "byOpen" && w.ClosedTrue_OpenFalse == false
                                                 )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrderDetails
                                  .Where(w => w.FK_tbl_Inv_PurchaseOrderMaster_ID == MasterID)
                                  .Where(w =>
                                                string.IsNullOrEmpty(FilterByLoad)
                                                 ||
                                                 FilterByLoad == "byClosed" && w.ClosedTrue_OpenFalse == true
                                                 ||
                                                 FilterByLoad == "byOpen" && w.ClosedTrue_OpenFalse == false
                                                 )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseOrderMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Quantity,
                          o.Rate,
                          o.GSTPercentage,
                          o.DiscountAmount,
                          o.WHTPercentage,
                          o.NetAmount,
                          o.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                          FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName = o?.tbl_Inv_PurchaseOrder_Manufacturer?.ManufacturerName ?? "",
                          o.Remarks,
                          o.ReceivedQty,
                          o.ClosedTrue_OpenFalse,
                          o.Performance_Time,
                          o.Performance_Quantity,
                          o.Performance_Quality,
                          o.FK_tbl_Inv_MeasurementUnit_ID_Supplier,
                          FK_tbl_Inv_MeasurementUnit_ID_SupplierName = o.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.QuantityAsPerSupplierUnit,
                          o.UnitFactorToConvertInLocalUnit,
                          o.Packaging,
                          o.BatchNo,
                          MfgDate = o.MfgDate.HasValue ? o.MfgDate.Value.ToString("dd-MMM-yyyy") : "",
                          ExpiryDate = o.ExpiryDate.HasValue ? o.ExpiryDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalReceivings = o.tbl_Inv_PurchaseNoteDetails.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseOrderDetail(tbl_Inv_PurchaseOrderDetail tbl_Inv_PurchaseOrderDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PurchaseOrderDetail.CreatedBy = userName;
                tbl_Inv_PurchaseOrderDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseOrderDetail.ModifiedBy = userName;
                tbl_Inv_PurchaseOrderDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseOrderDetail] 
                @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                ,@ID={3},@FK_tbl_Inv_PurchaseOrderMaster_ID={4}
                ,@FK_tbl_Inv_ProductRegistrationDetail_ID={5},@FK_AspNetOreasPriority_ID={6}
                ,@Quantity={7},@Rate={8},@GSTPercentage={9},@DiscountAmount={10}
                ,@WHTPercentage={11},@NetAmount={12},@FK_tbl_Inv_PurchaseOrder_Manufacturer_ID={13}
                ,@Remarks={14},@ReceivedQty={15},@ClosedTrue_OpenFalse={16}
                ,@Performance_Time={17},@Performance_Quantity={18},@Performance_Quality={19}
                ,@FK_tbl_Inv_PurchaseRequestDetail_ID={20},@FK_tbl_Inv_MeasurementUnit_ID_Supplier={21}
                ,@QuantityAsPerSupplierUnit={22},@UnitFactorToConvertInLocalUnit={23}
                ,@Packaging={24},@BatchNo={25},@MfgDate={26},@ExpiryDate={27}
                ,@CreatedBy={28},@CreatedDate={29},@ModifiedBy={30},@ModifiedDate={31}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseOrderDetail.ID, tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_PurchaseOrderMaster_ID, tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Inv_PurchaseOrderDetail.FK_AspNetOreasPriority_ID, tbl_Inv_PurchaseOrderDetail.Quantity, tbl_Inv_PurchaseOrderDetail.Rate, tbl_Inv_PurchaseOrderDetail.GSTPercentage, tbl_Inv_PurchaseOrderDetail.DiscountAmount, tbl_Inv_PurchaseOrderDetail.WHTPercentage,
                tbl_Inv_PurchaseOrderDetail.NetAmount, tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID, tbl_Inv_PurchaseOrderDetail.Remarks, tbl_Inv_PurchaseOrderDetail.ReceivedQty, tbl_Inv_PurchaseOrderDetail.ClosedTrue_OpenFalse,
                tbl_Inv_PurchaseOrderDetail.Performance_Time, tbl_Inv_PurchaseOrderDetail.Performance_Quantity, tbl_Inv_PurchaseOrderDetail.Performance_Quality, 
                tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_PurchaseRequestDetail_ID, tbl_Inv_PurchaseOrderDetail.FK_tbl_Inv_MeasurementUnit_ID_Supplier,
                tbl_Inv_PurchaseOrderDetail.QuantityAsPerSupplierUnit, tbl_Inv_PurchaseOrderDetail.UnitFactorToConvertInLocalUnit,
                tbl_Inv_PurchaseOrderDetail.Packaging, tbl_Inv_PurchaseOrderDetail.BatchNo, tbl_Inv_PurchaseOrderDetail.MfgDate, tbl_Inv_PurchaseOrderDetail.ExpiryDate,
                tbl_Inv_PurchaseOrderDetail.CreatedBy, tbl_Inv_PurchaseOrderDetail.CreatedDate, tbl_Inv_PurchaseOrderDetail.ModifiedBy, tbl_Inv_PurchaseOrderDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }

        #endregion

        #region Detail PurchaseNote
        public async Task<PagedData<object>> LoadPurchaseOrderDetailPN(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseNoteDetails
                                               .Where(w => w.FK_tbl_Inv_PurchaseOrderDetail_ID == MasterID)
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseNoteDetails
                                  .Where(w => w.FK_tbl_Inv_PurchaseOrderDetail_ID == MasterID)
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          o.Quantity,
                          o.ReferenceNo,
                          o.tbl_Inv_PurchaseNoteMaster.DocNo,
                          DocDate = o.tbl_Inv_PurchaseNoteMaster.DocDate.ToString("dd-MMM-yyyy"),
                          Action = o.tbl_Qc_ActionType?.ActionName
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Detail PurchaseReturnNote
        public async Task<PagedData<object>> LoadPurchaseOrderDetailPRN(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseReturnNoteDetails
                                               .Where(w => w.tbl_Inv_PurchaseNoteDetail_RefNo.FK_tbl_Inv_PurchaseOrderDetail_ID == MasterID)
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseReturnNoteDetails
                                  .Where(w => w.tbl_Inv_PurchaseNoteDetail_RefNo.FK_tbl_Inv_PurchaseOrderDetail_ID == MasterID)
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseReturnNoteMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          o.Quantity,
                          o.tbl_Inv_PurchaseReturnNoteMaster.DocNo,
                          DocDate = o.tbl_Inv_PurchaseReturnNoteMaster.DocDate.ToString("dd-MMM-yyyy")
                      };

            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLPurchaseOrder()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Purchase Order",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current PO Vendor Copy",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Purchase Order",
                    GroupBy = new List<string>(){ "Supplier", "Product" },
                    OrderBy = new List<string>(){ "PO Date", "PO No" },
                    SeekBy = new List<string>(){ "Closed", "Open", "All" },
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Purchase Order")
            {
                return await Task.Run(() => CurrentPurchaseOrder(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Current PO Vendor Copy")
            {
                return await Task.Run(() => CurrentPurchaseOrderVendorCopy(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Purchase Order")
            {
                return await Task.Run(() => RegisterPurchaseOrder(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentPurchaseOrder(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Purchase Order -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //PONo
                        (float)(PageSize.A4.GetWidth() * 0.15), //PODate
                        (float)(PageSize.A4.GetWidth() * 0.55), //AccountName
                        (float)(PageSize.A4.GetWidth() * 0.15)  //TargetDate
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("PO No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("PO Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Target Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string CreatedBy = "", TermsCondition = "", Note = "";
                using (DbDataReader sqlReader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    while (sqlReader.Read())
                    {
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PONo"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PODate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["TargetDate"]).ToString("dd-MMM-yy"))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        CreatedBy = sqlReader["CreatedBy"].ToString();
                        TermsCondition = sqlReader["TermsCondition"].ToString();
                        Note = sqlReader["Note"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 7------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05), //S No
                        (float)(PageSize.A4.GetWidth() * 0.10),  //Priority
                        (float)(PageSize.A4.GetWidth() * 0.40), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.10),  //Rate
                        (float)(PageSize.A4.GetWidth() * 0.10),  //GSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.10)  //NetAmount
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Priority")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("GST %")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                

                ReportName.Value = rn + "2";
                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Priority"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + (!sqlReader.IsDBNull("ManufacturerName") ? "\n" +sqlReader["ManufacturerName"].ToString() : ""))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["GSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["NetAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

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

                if (!string.IsNullOrEmpty(TermsCondition))
                {
                    pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                    pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add(TermsCondition)).SetBold().SetTextAlignment(TextAlignment.JUSTIFIED).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add(Note)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                }

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> CurrentPurchaseOrderVendorCopy(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Purchase Order (Vendor Copy) -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 4------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.14), //
                        (float)(PageSize.A4.GetWidth() * 0.36), //
                        (float)(PageSize.A4.GetWidth() * 0.14), //
                        (float)(PageSize.A4.GetWidth() * 0.36)  //
                }
                ).SetFontSize(7).SetFixedLayout().SetBorder(Border.NO_BORDER);

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string CreatedBy = "", TermsCondition = "", Note = "";
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
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("PO No")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PONo"].ToString())).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Pay Terms")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PaymentTerms"].ToString())).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("PO Date")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PODate"]).ToString("dd-MMM-yy"))).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Contact Person")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ContactPersonName"].ToString())).SetBorderRight(new SolidBorder(0.5f)).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Target Date")).SetBorder(Border.NO_BORDER).SetBold().SetKeepTogether(true));
                        pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["TargetDate"]).ToString("dd-MMM-yy"))).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

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
                        TermsCondition = sqlReader["TermsCondition"].ToString();
                        Note = sqlReader["Note"].ToString();
                    }
                }
                page.InsertContent(pdftableMaster);

                /////////////------------------------------table for detail 7------------------------------////////////////
                Table pdftableDetail = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05), //S No
                        (float)(PageSize.A4.GetWidth() * 0.10),  //Priority
                        (float)(PageSize.A4.GetWidth() * 0.40), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Quantity
                        (float)(PageSize.A4.GetWidth() * 0.10),  //Rate
                        (float)(PageSize.A4.GetWidth() * 0.10),  //GSTPercentage
                        (float)(PageSize.A4.GetWidth() * 0.10)  //NetAmount
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Detail")).SetBackgroundColor(new DeviceRgb(176, 196, 222)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Priority")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Rate")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("GST %")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));


                ReportName.Value = rn + "2";
                int SNo = 1; double TotalNetAmount = 0;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Priority"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString() + (!sqlReader.IsDBNull("ManufacturerName") ? "\n" + sqlReader["ManufacturerName"].ToString() : ""))).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["GSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["NetAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                        SNo++;
                        TotalNetAmount += Convert.ToDouble(sqlReader["NetAmount"]);
                    }
                }
                pdftableDetail.AddCell(new Cell(1, 5).Add(new Paragraph().Add("")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Total Amount")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(TotalNetAmount.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                page.InsertContent(pdftableDetail);

                /////////////------------------------------Signature Footer table------------------------------////////////////
                Table pdftableSignature = new Table(new float[] {
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25),
                (float)(PageSize.A4.GetWidth() * 0.25), (float)(PageSize.A4.GetWidth() * 0.25)
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                if (!string.IsNullOrEmpty(TermsCondition))
                {
                    pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                    pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add(TermsCondition)).SetBold().SetTextAlignment(TextAlignment.JUSTIFIED).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                    pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add(Note)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                }

                pdftableSignature.AddCell(new Cell(1, 4).Add(new Paragraph().Add("\n\n\n")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Created By: " + CreatedBy)).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell(1, 2).Add(new Paragraph().Add(" ")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                pdftableSignature.AddCell(new Cell().Add(new Paragraph().Add("Authorized By")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER).SetBorderTop(new SolidBorder(0.5f)).SetKeepTogether(true));
                page.InsertContent(pdftableSignature);
            }

            return page.FinishToGetBytes();
        }
        private async Task<byte[]> RegisterPurchaseOrder(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Purchase Order Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", false);

            /////////////------------------------------table for Detail 11------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.05),//S No
                        (float)(PageSize.A4.GetWidth() * 0.05),//PONo
                        (float)(PageSize.A4.GetWidth() * 0.05),//PODate 
                        (float)(PageSize.A4.GetWidth() * 0.05),//TargetDate 
                        (float)(PageSize.A4.GetWidth() * 0.19),//AccountName 
                        (float)(PageSize.A4.GetWidth() * 0.10),//Priority 
                        (float)(PageSize.A4.GetWidth() * 0.23),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.08),//Quantity
                        (float)(PageSize.A4.GetWidth() * 0.05),//Rate 
                        (float)(PageSize.A4.GetWidth() * 0.05),//GST%
                        (float)(PageSize.A4.GetWidth() * 0.10)//NetAmount
                }
            ).UseAllAvailableWidth().SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("PO No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("PO Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Target Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Supplier")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Priority")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Rate")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("GST %")).SetTextAlignment(TextAlignment.RIGHT).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Net Amount")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                string GroupbyFieldName = GroupBy == "Supplier" ? "AccountName" :
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
                                pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["PONo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["PODate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["TargetDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["AccountName"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Priority"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Rate"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["GSTPercentage"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["NetAmount"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        SNo++;
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 11).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 7).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell(1, 3).Add(new Paragraph().Add("")).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }
       
        #endregion

    }
    public class PurchaseOrderTermsConditionsRepository : IPurchaseOrderTermsConditions
    {
        private readonly OreasDbContext db;
        public PurchaseOrderTermsConditionsRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrderTermsConditionss.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.TCName,
                          o.TermsCondition,
                          o.Note,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseOrderTermsConditions()
        {
            return new[]
            {
                new { n = "by Term Name", v = "byTermName" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrderTermsConditionss
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTermName" && w.TCName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));

            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrderTermsConditionss
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byTermName" && w.TCName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.TCName,
                          o.TermsCondition,
                          o.Note,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Inv_PurchaseOrderTermsConditions tbl_Inv_PurchaseOrderTermsConditions, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_PurchaseOrderTermsConditions.CreatedBy = userName;
                tbl_Inv_PurchaseOrderTermsConditions.CreatedDate = DateTime.Now;
                db.tbl_Inv_PurchaseOrderTermsConditionss.Add(tbl_Inv_PurchaseOrderTermsConditions);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseOrderTermsConditions.ModifiedBy = userName;
                tbl_Inv_PurchaseOrderTermsConditions.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_PurchaseOrderTermsConditions).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                if (db.tbl_Inv_PurchaseOrderMasters.Any(a => a.FK_tbl_Inv_PurchaseOrderTermsConditions_ID == tbl_Inv_PurchaseOrderTermsConditions.ID))
                {
                    return "Aborted! Deleting Term is utilized";
                }
                db.tbl_Inv_PurchaseOrderTermsConditionss.Remove(db.tbl_Inv_PurchaseOrderTermsConditionss.Find(tbl_Inv_PurchaseOrderTermsConditions.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class PurchaseOrderImportTermsRepository : IPurchaseOrderImportTerms
    {
        private readonly OreasDbContext db;
        public PurchaseOrderImportTermsRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrder_ImportTermss.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.TermName,
                          o.AtSight,
                          o.AtUsance,
                          o.AtUsanceDays,
                          o.DocumentsForDIL,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseOrderImportTerm()
        {
            return new[]
            {
                new { n = "by Term Name", v = "byTermName" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrder_ImportTermss
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byTermName" && w.TermName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrder_ImportTermss
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byTermName" && w.TermName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.TermName,
                          o.AtSight,
                          o.AtUsance,
                          o.AtUsanceDays,
                          o.DocumentsForDIL,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Inv_PurchaseOrder_ImportTerms tbl_Inv_PurchaseOrder_ImportTerms, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_PurchaseOrder_ImportTerms.CreatedBy = userName;
                tbl_Inv_PurchaseOrder_ImportTerms.CreatedDate = DateTime.Now;
                db.tbl_Inv_PurchaseOrder_ImportTermss.Add(tbl_Inv_PurchaseOrder_ImportTerms);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseOrder_ImportTerms.ModifiedBy = userName;
                tbl_Inv_PurchaseOrder_ImportTerms.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_PurchaseOrder_ImportTerms).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_PurchaseOrder_ImportTermss.Remove(db.tbl_Inv_PurchaseOrder_ImportTermss.Find(tbl_Inv_PurchaseOrder_ImportTerms.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class POManufacturerRepository : IPOManufacturer
    {
        private readonly OreasDbContext db;
        public POManufacturerRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrder_Manufacturers.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.ManufacturerName,
                          o.ManufacturerAddress,
                          o.ContactNo,
                          o.ContactPerson,
                          o.Email,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLManufacturer()
        {
            return new[]
            {
                new { n = "by Name", v = "byName" }, new { n = "by Address", v = "byAddress" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrder_Manufacturers
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byName" && w.ManufacturerName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byAddress" && w.ManufacturerAddress.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrder_Manufacturers
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.ManufacturerName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byAddress" && w.ManufacturerAddress.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.ManufacturerName,
                          o.ManufacturerAddress,
                          o.ContactNo,
                          o.ContactPerson,
                          o.Email,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Inv_PurchaseOrder_Manufacturer tbl_Inv_PurchaseOrder_Manufacturer, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_PurchaseOrder_Manufacturer.CreatedBy = userName;
                tbl_Inv_PurchaseOrder_Manufacturer.CreatedDate = DateTime.Now;
                db.tbl_Inv_PurchaseOrder_Manufacturers.Add(tbl_Inv_PurchaseOrder_Manufacturer);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseOrder_Manufacturer.ModifiedBy = userName;
                tbl_Inv_PurchaseOrder_Manufacturer.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_PurchaseOrder_Manufacturer).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_PurchaseOrder_Manufacturers.Remove(db.tbl_Inv_PurchaseOrder_Manufacturers.Find(tbl_Inv_PurchaseOrder_Manufacturer.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class POSupplierRepository : IPOSupplier
    {
        private readonly OreasDbContext db;
        public POSupplierRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrder_Suppliers.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.SupplierName,
                          o.SupplierAddress,
                          o.ContactNo,
                          o.ContactPerson,
                          o.Email,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLSupplier()
        {
            return new[]
            {
                new { n = "by Name", v = "byName" }, new { n = "by Address", v = "byAddress" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrder_Suppliers
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byName" && w.SupplierName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byAddress" && w.SupplierAddress.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrder_Suppliers
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.SupplierName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byAddress" && w.SupplierAddress.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.SupplierName,
                          o.SupplierAddress,
                          o.ContactNo,
                          o.ContactPerson,
                          o.Email,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Inv_PurchaseOrder_Supplier tbl_Inv_PurchaseOrder_Supplier, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_PurchaseOrder_Supplier.CreatedBy = userName;
                tbl_Inv_PurchaseOrder_Supplier.CreatedDate = DateTime.Now;
                db.tbl_Inv_PurchaseOrder_Suppliers.Add(tbl_Inv_PurchaseOrder_Supplier);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseOrder_Supplier.ModifiedBy = userName;
                tbl_Inv_PurchaseOrder_Supplier.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_PurchaseOrder_Supplier).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_PurchaseOrder_Suppliers.Remove(db.tbl_Inv_PurchaseOrder_Suppliers.Find(tbl_Inv_PurchaseOrder_Supplier.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class POIndenterRepository : IPOIndenter
    {
        private readonly OreasDbContext db;
        public POIndenterRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }
        public async Task<object> Get(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseOrder_Indenters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.IndenterName,
                          o.IndenterAddress,
                          o.ContactNo,
                          o.ContactPerson,
                          o.Email,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLIndenter()
        {
            return new[]
            {
                new { n = "by Name", v = "byName" }, new { n = "by Address", v = "byAddress" }
            }.ToList();
        }
        public async Task<PagedData<object>> Load(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseOrder_Indenters
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byName" && w.IndenterName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byAddress" && w.IndenterAddress.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseOrder_Indenters
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byName" && w.IndenterName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byAddress" && w.IndenterAddress.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.IndenterName,
                          o.IndenterAddress,
                          o.ContactNo,
                          o.ContactPerson,
                          o.Email,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> Post(tbl_Inv_PurchaseOrder_Indenter tbl_Inv_PurchaseOrder_Indenter, string operation = "", string userName = "")
        {
            if (operation == "Save New")
            {
                tbl_Inv_PurchaseOrder_Indenter.CreatedBy = userName;
                tbl_Inv_PurchaseOrder_Indenter.CreatedDate = DateTime.Now;
                db.tbl_Inv_PurchaseOrder_Indenters.Add(tbl_Inv_PurchaseOrder_Indenter);
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseOrder_Indenter.ModifiedBy = userName;
                tbl_Inv_PurchaseOrder_Indenter.ModifiedDate = DateTime.Now;
                db.Entry(tbl_Inv_PurchaseOrder_Indenter).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            else if (operation == "Save Delete")
            {
                db.tbl_Inv_PurchaseOrder_Indenters.Remove(db.tbl_Inv_PurchaseOrder_Indenters.Find(tbl_Inv_PurchaseOrder_Indenter.ID));
                await db.SaveChangesAsync();
            }
            return "OK";
        }

    }
    public class PurchaseRequestRepository : IPurchaseRequest
    {
        private readonly OreasDbContext db;
        public PurchaseRequestRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetPurchaseRequestMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseRequestMasters.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          o.DocNo,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.Remarks,                         
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalDetail = o.tbl_Inv_PurchaseRequestDetails.Count()
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseRequestMaster()
        {
            return new[]
            {
                new { n = "by WareHouse Name", v = "byWareHouseName" }, new { n = "by Product Name", v = "byProductName" }, new { n = "by Doc No", v = "byDocNo" }
            }.ToList();
        }
        public object GetWCLBPurchaseRequestMaster()
        {
            return new[]
            {
                new { n = "by Approved", v = "byApproved" }, new { n = "by Rejected", v = "byRejected" }, new { n = "by Pending", v = "byPending" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseRequestMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseRequestMasters
                                    .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                                ||
                                                FilterByLoad == "byApproved" && w.tbl_Inv_PurchaseRequestDetails.Any(a => a.IsApproved)
                                                ||
                                                FilterByLoad == "byRejected" && w.tbl_Inv_PurchaseRequestDetails.Any(a => a.IsRejected)
                                                ||
                                                FilterByLoad == "byPending" && w.tbl_Inv_PurchaseRequestDetails.Any(a => a.IsPending)
                                                )
                                    .Where(w =>
                                            string.IsNullOrEmpty(FilterValueByText)
                                            ||
                                            FilterByText == "byWareHouseName" && w.tbl_Inv_WareHouseMaster.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                            ||
                                            FilterByText == "byProductName" && w.tbl_Inv_PurchaseRequestDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                            ||
                                            FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                            )
                                    .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseRequestMasters
                                  .Where(w => w.tbl_Inv_WareHouseMaster.AspNetOreasAuthorizationScheme_WareHouses
                                                .Any(a => a.AspNetOreasAuthorizationScheme.ApplicationUsers
                                                .Count(w => w.UserName == userName) > 0)
                                                )
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byApproved" && w.tbl_Inv_PurchaseRequestDetails.Any(a => a.IsApproved)
                                            ||
                                            FilterByLoad == "byRejected" && w.tbl_Inv_PurchaseRequestDetails.Any(a => a.IsRejected)
                                            ||
                                            FilterByLoad == "byPending" && w.tbl_Inv_PurchaseRequestDetails.Any(a => a.IsPending)
                                          )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byWareHouseName" && w.tbl_Inv_WareHouseMaster.WareHouseName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_PurchaseRequestDetails.Any(a => a.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower()))
                                        ||
                                        FilterByText == "byDocNo" && w.DocNo.ToString() == FilterValueByText
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          DocDate = o.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          o.DocNo,
                          o.FK_tbl_Inv_WareHouseMaster_ID,
                          FK_tbl_Inv_WareHouseMaster_IDName = o.tbl_Inv_WareHouseMaster.WareHouseName,
                          o.Remarks,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          TotalItems = o.tbl_Inv_PurchaseRequestDetails.Count(),
                          TotalApproved = o.tbl_Inv_PurchaseRequestDetails.Count(c => c.IsApproved),
                          TotalRejected = o.tbl_Inv_PurchaseRequestDetails.Count(c => c.IsRejected),
                          TotalPending = o.tbl_Inv_PurchaseRequestDetails.Count(c => c.IsPending)
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseRequestMaster(tbl_Inv_PurchaseRequestMaster tbl_Inv_PurchaseRequestMaster, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PurchaseRequestMaster.CreatedBy = userName;
                tbl_Inv_PurchaseRequestMaster.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseRequestMaster.ModifiedBy = userName;
                tbl_Inv_PurchaseRequestMaster.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseRequestMaster] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@DocNo={4},@DocDate={5}
              ,@FK_tbl_Inv_WareHouseMaster_ID={6} ,@Remarks={7}
              ,@CreatedBy={8},@CreatedDate={9},@ModifiedBy={10},@ModifiedDate={11}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseRequestMaster.ID, tbl_Inv_PurchaseRequestMaster.DocNo, tbl_Inv_PurchaseRequestMaster.DocDate,
                tbl_Inv_PurchaseRequestMaster.FK_tbl_Inv_WareHouseMaster_ID, tbl_Inv_PurchaseRequestMaster.Remarks,
                tbl_Inv_PurchaseRequestMaster.CreatedBy, tbl_Inv_PurchaseRequestMaster.CreatedDate, tbl_Inv_PurchaseRequestMaster.ModifiedBy, tbl_Inv_PurchaseRequestMaster.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;
        }
        #endregion

        #region Detail
        public async Task<object> GetPurchaseRequestDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseRequestDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseRequestMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Quantity,
                          o.Remarks,
                          o.IsApproved,
                          o.IsRejected,
                          o.IsPending,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseRequestDetail()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }
            }.ToList();
        }
        public object GetWCLBPurchaseRequestDetail()
        {
            return new[]
            {
                new { n = "by Approved", v = "byApproved" }, new { n = "by Rejected", v = "byRejected" }, new { n = "by Pending", v = "byPending" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseRequestDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseRequestDetails
                                               .Where(w => w.FK_tbl_Inv_PurchaseRequestMaster_ID == MasterID)
                                               .Where(w =>
                                                   string.IsNullOrEmpty(FilterByLoad)
                                                   ||
                                                   FilterByLoad == "byApproved" && w.IsApproved
                                                   ||
                                                   FilterByLoad == "byRejected" && w.IsRejected
                                                   ||
                                                   FilterByLoad == "byPending" && w.IsPending
                                                   )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseRequestDetails
                                  .Where(w => w.FK_tbl_Inv_PurchaseRequestMaster_ID == MasterID)
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byApproved" && w.IsApproved
                                            ||
                                            FilterByLoad == "byRejected" && w.IsRejected
                                            ||
                                            FilterByLoad == "byPending" && w.IsPending
                                        )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseRequestMaster_ID,
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Quantity,
                          o.Remarks,
                          o.IsApproved,
                          o.IsRejected,
                          o.IsPending,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          PONo = o?.tbl_Inv_PurchaseOrderDetails?.FirstOrDefault()?.tbl_Inv_PurchaseOrderMaster.PONo ?? null
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseRequestDetail(tbl_Inv_PurchaseRequestDetail tbl_Inv_PurchaseRequestDetail, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PurchaseRequestDetail.CreatedBy = userName;
                tbl_Inv_PurchaseRequestDetail.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseRequestDetail.ModifiedBy = userName;
                tbl_Inv_PurchaseRequestDetail.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseRequestDetail] 
               @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
              ,@ID={3},@FK_tbl_Inv_PurchaseRequestMaster_ID={4},@FK_tbl_Inv_ProductRegistrationDetail_ID={5}
              ,@FK_AspNetOreasPriority_ID={6},@Quantity={7},@Remarks={8}
              ,@CreatedBy={9},@CreatedDate={10},@ModifiedBy={11},@ModifiedDate={12}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseRequestDetail.ID, tbl_Inv_PurchaseRequestDetail.FK_tbl_Inv_PurchaseRequestMaster_ID, tbl_Inv_PurchaseRequestDetail.FK_tbl_Inv_ProductRegistrationDetail_ID,
                tbl_Inv_PurchaseRequestDetail.FK_AspNetOreasPriority_ID, tbl_Inv_PurchaseRequestDetail.Quantity, tbl_Inv_PurchaseRequestDetail.Remarks,
                tbl_Inv_PurchaseRequestDetail.CreatedBy, tbl_Inv_PurchaseRequestDetail.CreatedDate, tbl_Inv_PurchaseRequestDetail.ModifiedBy, tbl_Inv_PurchaseRequestDetail.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }

        #endregion

        #region Report   
        public List<ReportCallingModel> GetRLPurchaseRequest()
        {
            return new List<ReportCallingModel>() {
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.OnlyID,
                    ReportName ="Current Purchase Request",
                    GroupBy = null,
                    OrderBy = null,
                    SeekBy = null
                },
                new ReportCallingModel()
                {
                    ReportType= EnumReportType.Periodic,
                    ReportName ="Register Purchase Request",
                    GroupBy = new List<string>(){ "WareHouse", "Product" },
                    OrderBy = new List<string>(){ "Doc Date", "Doc No" },
                    SeekBy = null,
                }
            };
        }
        public async Task<byte[]> GetPDFFileAsync(string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", int GroupID = 0, string userName = "")
        {
            if (rn == "Current Purchase Request")
            {
                return await Task.Run(() => CurrentPurchaseRequest(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            else if (rn == "Register Purchase Request")
            {
                return await Task.Run(() => RegisterPurchaseRequest(id, datefrom, datetill, SeekBy, GroupBy, Orderby, uri, rn, GroupID, userName));
            }
            return Encoding.ASCII.GetBytes("Wrong Parameters");
        }
        private async Task<byte[]> CurrentPurchaseRequest(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {
            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 15f, 35f, "----- Purchase Request -----", true);

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                /////////////------------------------------table for master 3------------------------------////////////////
                Table pdftableMaster = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocNo
                        (float)(PageSize.A4.GetWidth() * 0.15), //DocDate
                        (float)(PageSize.A4.GetWidth() * 0.70), //WareHouse
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableMaster.AddCell(new Cell().Add(new Paragraph().Add("WareHouse")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                        (float)(PageSize.A4.GetWidth() * 0.15),  //Priority
                        (float)(PageSize.A4.GetWidth() * 0.60), //ProductName
                        (float)(PageSize.A4.GetWidth() * 0.15)  //Quantity
                }
                ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER);

                pdftableDetail.AddCell(new Cell(1, 4).Add(new Paragraph().Add("Detail")).SetBold().SetTextAlignment(TextAlignment.CENTER).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("S No")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Priority")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Product Name")).SetBold().SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add("Quantity")).SetBold().SetTextAlignment(TextAlignment.RIGHT).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));

                ReportName.Value = rn + "2";
                int SNo = 1;

                using (DbDataReader sqlReader = command.ExecuteReader())
                {
                    while (sqlReader.Read())
                    {
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
                        pdftableDetail.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Priority"].ToString())).SetBorder(new SolidBorder(0.5f)).SetKeepTogether(true));
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
        private async Task<byte[]> RegisterPurchaseRequest(int id = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string Orderby = "", string uri = "", string rn = "", int GroupID = 0, string userName = "")
        {

            ITPage page = new ITPage(PageSize.A4, 20f, 20f, 20f, 30f, "----- Purchase Request Register During Period " + "From: " + datefrom.Value.ToString("dd-MMM-yy") + " TO " + datetill.Value.ToString("dd-MMM-yy") + "-----", true);
           
            /////////////------------------------------table for Detail 6------------------------------////////////////
            Table pdftableMain = new Table(new float[] {
                        (float)(PageSize.A4.GetWidth() * 0.10),//S No
                        (float)(PageSize.A4.GetWidth() * 0.10),//DocNo
                        (float)(PageSize.A4.GetWidth() * 0.10),//DocDate  
                        (float)(PageSize.A4.GetWidth() * 0.10),//Priority 
                        (float)(PageSize.A4.GetWidth() * 0.40),//ProductName
                        (float)(PageSize.A4.GetWidth() * 0.20),//Quantity
                }
            ).SetFontSize(6).SetFixedLayout().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);

            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("S. No.")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc No")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Doc Date")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Priority")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Product")).SetBold());
            pdftableMain.AddHeaderCell(new Cell().Add(new Paragraph().Add("Quantity")).SetTextAlignment(TextAlignment.RIGHT).SetBold());

            int SNo = 1;
            double GrandTotalQty = 0, GroupTotalQty = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[Report_Inv_Orders] @ReportName,@DateFrom,@DateTill,@MasterID,@SeekBy,@GroupBy,@OrderBy,@GroupID,@UserName ";
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
                                pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                                pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            }

                            GroupbyValue = sqlReader[GroupbyFieldName].ToString();

                            if (GroupID > 0)
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(GroupbyValue)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));
                            else
                                pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(new Link(GroupbyValue, PdfAction.CreateURI(uri + "?rn=" + rn + "&datefrom=" + datefrom.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&datetill=" + datetill.Value.ToString("MM/dd/yyyy hh:mm:ss tt") + "&SeekBy=" + SeekBy + "&GroupBy=" + GroupBy + "&OrderBy=" + Orderby + "&GroupID=" + sqlReader[GroupbyFieldName + "ID"].ToString())))).SetFontColor(new DeviceRgb(0, 102, 204)).SetFontSize(10).SetBold().SetBorder(Border.NO_BORDER).SetKeepTogether(true));

                            GroupTotalQty = 0;
                        }

                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(SNo.ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["DocNo"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(((DateTime)sqlReader["DocDate"]).ToString("dd-MMM-yy"))).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Priority"].ToString())).SetKeepTogether(true));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["ProductName"].ToString())).SetKeepTogether(true).SetFontSize(5));
                        pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(sqlReader["Quantity"].ToString() + " " + sqlReader["MeasurementUnit"].ToString())).SetTextAlignment(TextAlignment.RIGHT).SetKeepTogether(true).SetFontSize(5));

                        GroupTotalQty += Convert.ToDouble(sqlReader["Quantity"]);
                        GrandTotalQty += Convert.ToDouble(sqlReader["Quantity"]);

                        SNo++;
                    }

                }
            }

            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Sub Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GroupTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            pdftableMain.AddCell(new Cell(1, 6).Add(new Paragraph().Add(" ")).SetBorder(Border.NO_BORDER).SetBorderBottom(new SolidBorder(0.5f)));

            pdftableMain.AddCell(new Cell(1, 5).Add(new Paragraph().Add("Grand Total")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));
            pdftableMain.AddCell(new Cell().Add(new Paragraph().Add(GrandTotalQty.ToString())).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetKeepTogether(true));

            page.InsertContent(new Cell().Add(pdftableMain).SetBorder(Border.NO_BORDER));
            return page.FinishToGetBytes();
        }

        #endregion
    }
    public class PurchaseRequestBidsRepository : IPurchaseRequestBids
    {
        private readonly OreasDbContext db;
        public PurchaseRequestBidsRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        #region Master
        public async Task<object> GetPurchaseRequestBidsMaster(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseRequestDetails.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.tbl_Inv_PurchaseRequestMaster.DocNo,
                          DocDate = o.tbl_Inv_PurchaseRequestMaster.DocDate.ToString("dd-MMM-yyyy") ?? "",                          
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Quantity,
                          o.Remarks,
                          o.IsApproved,
                          o.IsRejected,
                          o.IsPending,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseRequestBidsMaster()
        {
            return new[]
            {
                new { n = "by Product Name", v = "byProductName" }, new { n = "by Doc No", v = "byDocNo" }
            }.ToList();
        }
        public object GetWCLBPurchaseRequestBidsMaster()
        {
            return new[]
            {
                new { n = "by Approved", v = "byApproved" }, new { n = "by Rejected", v = "byRejected" }, new { n = "by Pending", v = "byPending" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseRequestBidsMaster(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null)
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseRequestDetails
                                               .Where(w =>
                                                     string.IsNullOrEmpty(FilterByLoad)
                                                     ||
                                                     FilterByLoad == "byApproved" && w.IsApproved
                                                     ||
                                                     FilterByLoad == "byRejected" && w.IsRejected
                                                     ||
                                                     FilterByLoad == "byPending" && w.IsPending
                                                   )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                                       ||
                                                       FilterByText == "byDocNo" && w.tbl_Inv_PurchaseRequestMaster.DocNo.ToString() == FilterValueByText
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseRequestDetails
                                  .Where(w =>
                                            string.IsNullOrEmpty(FilterByLoad)
                                            ||
                                            FilterByLoad == "byApproved" && w.IsApproved
                                            ||
                                            FilterByLoad == "byRejected" && w.IsRejected
                                            ||
                                            FilterByLoad == "byPending" && w.IsPending
                                         )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "byProductName" && w.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName.ToLower().Contains(FilterValueByText.ToLower())
                                        ||
                                        FilterByText == "byDocNo" && w.tbl_Inv_PurchaseRequestMaster.DocNo.ToString() == FilterValueByText
                                      )
                                  .OrderByDescending(i => i.FK_tbl_Inv_PurchaseRequestMaster_ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.tbl_Inv_PurchaseRequestMaster.DocNo,
                          DocDate = o.tbl_Inv_PurchaseRequestMaster.DocDate.ToString("dd-MMM-yyyy") ?? "",
                          o.FK_tbl_Inv_ProductRegistrationDetail_ID,
                          FK_tbl_Inv_ProductRegistrationDetail_IDName = o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster.ProductName,
                          o.tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit.MeasurementUnit,
                          o.FK_AspNetOreasPriority_ID,
                          FK_AspNetOreasPriority_IDName = o.AspNetOreasPriority.Priority,
                          o.Quantity,
                          o.Remarks,
                          o.IsApproved,
                          o.IsRejected,
                          o.IsPending,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : "",
                          PONo = o?.tbl_Inv_PurchaseOrderDetails?.FirstOrDefault()?.tbl_Inv_PurchaseOrderMaster.PONo ?? null,
                          NoOfBids = o.tbl_Inv_PurchaseRequestDetail_Bidss.Count()
                      };




            pageddata.Data = qry;

            return pageddata;
        }

        #endregion

        #region Detail
        public async Task<object> GetPurchaseRequestBidsDetail(int id)
        {
            var qry = from o in await db.tbl_Inv_PurchaseRequestDetail_Bidss.Where(w => w.ID == id).ToListAsync()
                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseRequestDetail_ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Quantity,
                          o.Rate,
                          o.GSTPercentage,
                          TargetDate = o.TargetDate.ToString("dd-MMM-yyyy") ?? "",
                          o.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                          FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName = o?.tbl_Inv_PurchaseOrder_Manufacturer?.ManufacturerName ?? "",
                          o.Remarks,
                          o.Approved,
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };

            return qry.FirstOrDefault();
        }
        public object GetWCLPurchaseRequestBidsDetail()
        {
            return new[]
            {
                new { n = "by Supplier Name", v = "bySupplierName" }
            }.ToList();
        }
        public object GetWCLBPurchaseRequestBidsDetail()
        {
            return new[]
            {
                new { n = "by Pending", v = "byPending" },new { n = "by Rejected", v = "byRejected" },new { n = "by Approved", v = "byApproved" }
            }.ToList();
        }
        public async Task<PagedData<object>> LoadPurchaseRequestBidsDetail(int CurrentPage = 1, int MasterID = 0, string FilterByText = null, string FilterValueByText = null, string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0, string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null, string FilterByLoad = null, string userName = "")
        {
            PagedData<object> pageddata = new PagedData<object>();

            int NoOfRecords = await db.tbl_Inv_PurchaseRequestDetail_Bidss
                                               .Where(w => w.FK_tbl_Inv_PurchaseRequestDetail_ID == MasterID)
                                               .Where(w =>
                                                   string.IsNullOrEmpty(FilterByLoad)
                                                   ||
                                                   FilterByLoad == "byPending" && w.Approved == null
                                                   ||
                                                   FilterByLoad == "byRejected" && w.Approved == false
                                                   ||
                                                   FilterByLoad == "byApproved" && w.Approved == true
                                                   )
                                               .Where(w =>
                                                       string.IsNullOrEmpty(FilterValueByText)
                                                       ||
                                                       FilterByText == "bySupplierName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                                     )
                                               .CountAsync();

            pageddata.TotalPages = Convert.ToInt32(Math.Ceiling((double)NoOfRecords / pageddata.PageSize));


            pageddata.CurrentPage = CurrentPage;

            var qry = from o in await db.tbl_Inv_PurchaseRequestDetail_Bidss
                                  .Where(w => w.FK_tbl_Inv_PurchaseRequestDetail_ID == MasterID)
                                  .Where(w =>
                                             string.IsNullOrEmpty(FilterByLoad)
                                             ||
                                             FilterByLoad == "byPending" && w.Approved == null
                                             ||
                                             FilterByLoad == "byRejected" && w.Approved == false
                                             ||
                                             FilterByLoad == "byApproved" && w.Approved == true
                                             )
                                  .Where(w =>
                                        string.IsNullOrEmpty(FilterValueByText)
                                        ||
                                        FilterByText == "bySupplierName" && w.tbl_Ac_ChartOfAccounts.AccountName.ToLower().Contains(FilterValueByText.ToLower())
                                      )
                                  .OrderByDescending(i => i.ID).Skip(pageddata.PageSize * (CurrentPage - 1)).Take(pageddata.PageSize).ToListAsync()

                      select new
                      {
                          o.ID,
                          o.FK_tbl_Inv_PurchaseRequestDetail_ID,
                          o.FK_tbl_Ac_ChartOfAccounts_ID,
                          FK_tbl_Ac_ChartOfAccounts_IDName = o.tbl_Ac_ChartOfAccounts.AccountName,
                          o.Quantity,
                          o.Rate,
                          o.GSTPercentage,
                          TargetDate = o.TargetDate.ToString("dd-MMM-yyyy") ?? "",
                          o.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID,
                          FK_tbl_Inv_PurchaseOrder_Manufacturer_IDName = o?.tbl_Inv_PurchaseOrder_Manufacturer?.ManufacturerName ?? "",
                          o.Remarks,
                          Approved = o.Approved.HasValue ? (o.Approved.Value ? "Approved" : "Rejected") : "Pending",
                          o.CreatedBy,
                          CreatedDate = o.CreatedDate.HasValue ? o.CreatedDate.Value.ToString("dd-MMM-yyyy") : "",
                          o.ModifiedBy,
                          ModifiedDate = o.ModifiedDate.HasValue ? o.ModifiedDate.Value.ToString("dd-MMM-yyyy") : ""
                      };




            pageddata.Data = qry;

            return pageddata;
        }
        public async Task<string> PostPurchaseRequestBidsDetail(tbl_Inv_PurchaseRequestDetail_Bids tbl_Inv_PurchaseRequestDetail_Bids, string operation = "", string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            if (operation == "Save New")
            {
                tbl_Inv_PurchaseRequestDetail_Bids.CreatedBy = userName;
                tbl_Inv_PurchaseRequestDetail_Bids.CreatedDate = DateTime.Now;
                CRUD_Type.Value = "Insert";
            }
            else if (operation == "Save Update")
            {
                tbl_Inv_PurchaseRequestDetail_Bids.ModifiedBy = userName;
                tbl_Inv_PurchaseRequestDetail_Bids.ModifiedDate = DateTime.Now;
                CRUD_Type.Value = "Update";
            }
            else if (operation == "Save Delete")
            {
                CRUD_Type.Value = "Delete";
            }

            await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseRequestDetail_Bids] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Inv_PurchaseRequestDetail_ID={4},@FK_tbl_Ac_ChartOfAccounts_ID={5}
                  ,@Quantity={6},@Rate={7},@GSTPercentage={8}
                  ,@TargetDate={9},@FK_tbl_Inv_PurchaseOrder_Manufacturer_ID={10},@Remarks={11},@Approved={12}
                  ,@CreatedBy={13},@CreatedDate={14},@ModifiedBy={15},@ModifiedDate={16}",
                CRUD_Type, CRUD_Msg, CRUD_ID,
                tbl_Inv_PurchaseRequestDetail_Bids.ID, tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Inv_PurchaseRequestDetail_ID, tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Ac_ChartOfAccounts_ID,
                tbl_Inv_PurchaseRequestDetail_Bids.Quantity, tbl_Inv_PurchaseRequestDetail_Bids.Rate, tbl_Inv_PurchaseRequestDetail_Bids.GSTPercentage,
                tbl_Inv_PurchaseRequestDetail_Bids.TargetDate, tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID, tbl_Inv_PurchaseRequestDetail_Bids.Remarks, tbl_Inv_PurchaseRequestDetail_Bids.Approved,
                tbl_Inv_PurchaseRequestDetail_Bids.CreatedBy, tbl_Inv_PurchaseRequestDetail_Bids.CreatedDate, tbl_Inv_PurchaseRequestDetail_Bids.ModifiedBy, tbl_Inv_PurchaseRequestDetail_Bids.ModifiedDate);

            if ((string)CRUD_Msg.Value == "Successful")
                return "OK";
            else
                return (string)CRUD_Msg.Value;

        }
        public async Task<string> PostPurchaseRequestBidsDecision(tbl_Inv_PurchaseRequestDetail_Bids tbl_Inv_PurchaseRequestDetail_Bids, int tbl_Inv_PurchaseRequestDetail_BidsID = 0, bool? Decision = null, string userName = "")
        {
            SqlParameter CRUD_Type = new SqlParameter("@CRUD_Type", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Size = 50 };
            SqlParameter CRUD_Msg = new SqlParameter("@CRUD_Msg", SqlDbType.VarChar) { Direction = ParameterDirection.Output, Size = 100, Value = "Failed" };
            SqlParameter CRUD_ID = new SqlParameter("@CRUD_ID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            CRUD_Type.Value = "UpdateByDecision";

            var bit = await db.tbl_Inv_PurchaseRequestDetail_Bidss.Where(w => w.ID == tbl_Inv_PurchaseRequestDetail_BidsID).FirstOrDefaultAsync();
            if (bit != null)
            {
                await db.Database.ExecuteSqlRawAsync(@"EXECUTE [dbo].[OP_Inv_PurchaseRequestDetail_Bids] 
                   @CRUD_Type={0},@CRUD_Msg={1} OUTPUT,@CRUD_ID={2} OUTPUT
                  ,@ID={3},@FK_tbl_Inv_PurchaseRequestDetail_ID={4},@FK_tbl_Ac_ChartOfAccounts_ID={5}
                  ,@Quantity={6},@Rate={7},@GSTPercentage={8}
                  ,@TargetDate={9},@FK_tbl_Inv_PurchaseOrder_Manufacturer_ID={10},@Remarks={11},@Approved={12}
                  ,@CreatedBy={13},@CreatedDate={14},@ModifiedBy={15},@ModifiedDate={16}",
                    CRUD_Type, CRUD_Msg, CRUD_ID,
                    tbl_Inv_PurchaseRequestDetail_Bids.ID, tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Inv_PurchaseRequestDetail_ID, tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Ac_ChartOfAccounts_ID,
                    tbl_Inv_PurchaseRequestDetail_Bids.Quantity, tbl_Inv_PurchaseRequestDetail_Bids.Rate, tbl_Inv_PurchaseRequestDetail_Bids.GSTPercentage,
                    tbl_Inv_PurchaseRequestDetail_Bids.TargetDate, tbl_Inv_PurchaseRequestDetail_Bids.FK_tbl_Inv_PurchaseOrder_Manufacturer_ID, tbl_Inv_PurchaseRequestDetail_Bids.Remarks, tbl_Inv_PurchaseRequestDetail_Bids.Approved,
                    tbl_Inv_PurchaseRequestDetail_Bids.CreatedBy, tbl_Inv_PurchaseRequestDetail_Bids.CreatedDate, tbl_Inv_PurchaseRequestDetail_Bids.ModifiedBy, tbl_Inv_PurchaseRequestDetail_Bids.ModifiedDate);

                if ((string)CRUD_Msg.Value == "Successful")
                    return "OK";
                else
                    return (string)CRUD_Msg.Value;

            }
            else
                return "Not Found";


            

        }
        public async Task<object> GetPOSuggestions(int ProductID = 0)
        {
            var qry = await db.UDTVF_Inv_GetLastPOWithBestSuppliers
                              .FromSqlRaw("SELECT * FROM [dbo].[UDTVF_Inv_GetLastPOWithBestSupplier] (" + ProductID.ToString() + ")")
                              .OrderBy(o=> o.PONo)
                              .FirstOrDefaultAsync();
            return qry;
        }
        public async Task<object> IsPurchaseRequestAprrover(string userName = "")
        {
            return (await db.Users.Where(w => w.UserName.ToLower() == userName.ToLower()).FirstOrDefaultAsync()).PurchaseRequestApprover;
        }

        #endregion

    }

    //---------------Dashboard-------------//
    public class InventoryDashboardRepository : IInventoryDashboard
    {
        private readonly OreasDbContext db;
        
        public InventoryDashboardRepository(OreasDbContext oreasDbContext)
        {
            this.db = oreasDbContext;
        }

        public async Task<object> GetDashBoardData(string userName = "")
        {
            int ON_NoOfProd = 0; double ON_TotalOrderQty = 0; double ON_TotalMfgQty = 0; double ON_TotalSoldQty = 0;
            int PO_NoOfProd = 0; double PO_Qty = 0; double PO_RecQty = 0; int PR_PendingApprovalBits = 0; int PR_PendingReqForBits = 0; int ReorderAlerts = 0;
            int BMR_R_DispPending = 0; int BMR_R_DispReserved = 0; int BMR_P_DispPending = 0; int BMR_P_DispReserved = 0;
            int BMRA_DispPending = 0; int O_DisPending = 0;

            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXECUTE [dbo].[USP_Inv_DashBoard] @UserName ";
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
                        ON_TotalSoldQty = (double)sqlReader["ON_TotalSoldQty"];

                        PO_NoOfProd = (int)sqlReader["PO_NoOfProd"];
                        PO_Qty = (double)sqlReader["PO_Qty"];
                        PO_RecQty = (double)sqlReader["PO_RecQty"];

                        PR_PendingApprovalBits = (int)sqlReader["PR_PendingApprovalBits"];
                        PR_PendingReqForBits = (int)sqlReader["PR_PendingReqForBits"];

                        ReorderAlerts = (int)sqlReader["ReorderAlerts"];

                        BMR_R_DispPending = (int)sqlReader["BMR_R_DispPending"];
                        BMR_R_DispReserved = (int)sqlReader["BMR_R_DispReserved"];
                        BMR_P_DispPending = (int)sqlReader["BMR_P_DispPending"];
                        BMR_P_DispReserved = (int)sqlReader["BMR_P_DispReserved"];

                        BMRA_DispPending = (int)sqlReader["BMRA_DispPending"];
                        O_DisPending = (int)sqlReader["O_DisPending"];

                    }

                }
            }
            return new
            {
                OrderNote = new
                {                    
                    ON_NoOfProd,
                    ON_TotalOrderQty,
                    ON_TotalMfgQty,
                    ON_TotalSoldQty
                },
                PurchaseOrder = new
                {
                    PO_NoOfProd,
                    PO_Qty,
                    PO_RecQty
                },
                PurchaseRequestBit = new
                {
                    PR_PendingApprovalBits,
                    PR_PendingReqForBits
                },
                Dispensing = new
                {
                    BMR_R_DispPending,
                    BMR_R_DispReserved,
                    BMR_P_DispPending,
                    BMR_P_DispReserved,
                    BMRA_DispPending,
                    O_DisPending
                },
                Other = new 
                {
                    ReorderAlerts
                }

            };
        }


    }

}
