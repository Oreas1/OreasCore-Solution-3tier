using Microsoft.AspNetCore.Mvc;
using OreasCore.Custom_Classes.OreasCore.Custom_Classes;
using OreasCore.Custom_Classes;
using OreasModel;
using OreasServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using System.Linq;

namespace OreasCore.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class SetUpController : Controller
    {
        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetWHMListAsync([FromServices] IInventoryList IList, string QueryName = "", string WHMFilterBy = "", string WHMFilterValue = "", int FormID = 0)
        {
            return Json(
                await IList.GetWHMListAsync(QueryName, WHMFilterBy, WHMFilterValue, FormID, User.Identity.Name)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        #region Measurement Unit

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedMeasurementUnitAsync([FromServices] IAuthorizationScheme db, [FromServices] IMeasurementUnit db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "MeasurementUnitIndexCtlr",
                        WildCard = db2.GetWCLMeasurementUnit(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Measurement Unit"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Measurement Unit", Operation = "CanView")]
        public IActionResult MeasurementUnitIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> MeasurementUnitLoad([FromServices] IMeasurementUnit db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Measurement Unit", Operation = "CanPost")]
        public async Task<string> MeasurementUnitPost([FromServices] IMeasurementUnit db, [FromBody] tbl_Inv_MeasurementUnit tbl_Inv_MeasurementUnit, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Inv_MeasurementUnit, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Measurement Unit", Operation = "CanView")]
        public async Task<IActionResult> MeasurementUnitGet([FromServices] IMeasurementUnit db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Product Classification

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedProductClassificationAsync([FromServices] IAuthorizationScheme db, [FromServices] IProductClassification db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductClassificationIndexCtlr",
                        WildCard = db2.GetWCLProductClassification(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Product Classification"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Product Classification", Operation = "CanView")]
        public IActionResult ProductClassificationIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> ProductClassificationLoad([FromServices] IProductClassification db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Product Classification", Operation = "CanPost")]
        public async Task<string> ProductClassificationPost([FromServices] IProductClassification db, [FromBody] tbl_Inv_ProductClassification tbl_Inv_ProductClassification, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Inv_ProductClassification, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Product Classification", Operation = "CanView")]
        public async Task<IActionResult> ProductClassificationGet([FromServices] IProductClassification db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Product Type

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedProductTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] IProductType db2, [FromServices] IQcList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductTypeMasterCtlr",
                        WildCard = db2.GetWCLProductTypeMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Product Type"),
                        Otherdata = new { ActionList = await db3.GetActionTypeListAsync(null,null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductTypeDetailCtlr",
                        WildCard = db2.GetWCLProductTypeDetail(),
                        Reports = db2.GetRLProductTypeDetail(),
                        Privilege = null,
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Product Type", Operation = "CanView")]
        public IActionResult ProductTypeIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Product Type", Operation = "CanView")]
        public async Task<IActionResult> ProductTypeMasterLoad([FromServices] IProductType db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductTypeMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Product Type", Operation = "CanPost")]
        public async Task<string> ProductTypeMasterPost([FromServices] IProductType db, [FromBody] tbl_Inv_ProductType tbl_Inv_ProductType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostProductTypeMaster(tbl_Inv_ProductType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Product Type", Operation = "CanView")]
        public async Task<IActionResult> ProductTypeMasterGet([FromServices] IProductType db, int ID)
        {
            return Json(await db.GetProductTypeMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Product Type", Operation = "CanView")]
        public async Task<IActionResult> ProductTypeDetailLoad([FromServices] IProductType db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductTypeDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Product Type", Operation = "CanPost")]
        public async Task<string> ProductTypeDetailPost([FromServices] IProductType db, [FromBody] tbl_Inv_ProductType_Category tbl_Inv_ProductType_Category, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostProductTypeDetail(tbl_Inv_ProductType_Category, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Product Type", Operation = "CanView")]
        public async Task<IActionResult> ProductTypeDetailGet([FromServices] IProductType db, int ID)
        {
            return Json(await db.GetProductTypeDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Product Type", Operation = "CanViewReport")]
        public async Task<IActionResult> GetProductTypeReport([FromServices] IProductType db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region WareHouse

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedWareHouseAsync([FromServices] IAuthorizationScheme db, [FromServices] IWareHouse db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "WareHouseMasterCtlr",
                        WildCard = db2.GetWCLWareHouseMaster(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "WareHouse"),
                        Otherdata = null
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "WareHouseDetailCtlr",
                        WildCard = db2.GetWCLWareHouseDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new { ProductTypeCategoryList = await db3.GetProductTypeCategoryListAsync(null,null) }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "WareHouse", Operation = "CanView")]
        public IActionResult WareHouseIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "WareHouse", Operation = "CanView")]
        public async Task<IActionResult> WareHouseMasterLoad([FromServices] IWareHouse db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadWareHouseMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "WareHouse", Operation = "CanPost")]
        public async Task<string> WareHouseMasterPost([FromServices] IWareHouse db, [FromBody] tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostWareHouseMaster(tbl_Inv_WareHouseMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "WareHouse", Operation = "CanView")]
        public async Task<IActionResult> WareHouseMasterGet([FromServices] IWareHouse db, int ID)
        {
            return Json(await db.GetWareHouseMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "WareHouse", Operation = "CanView")]
        public async Task<IActionResult> WareHouseDetailLoad([FromServices] IWareHouse db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadWareHouseDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "WareHouse", Operation = "CanPost")]
        public async Task<string> WareHouseDetailPost([FromServices] IWareHouse db, [FromBody] tbl_Inv_WareHouseDetail tbl_Inv_WareHouseDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostWareHouseDetail(tbl_Inv_WareHouseDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "WareHouse", Operation = "CanView")]
        public async Task<IActionResult> WareHouseDetailGet([FromServices] IWareHouse db, int ID)
        {
            return Json(await db.GetWareHouseDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #endregion

        #region ProductRegistration

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetProductListAsync([FromServices] IInventoryList IList, [FromServices] IUser user, string QueryName = "", string ProductFilterBy = "", string ProductFilterValue = "", int? masterid = null, DateTime? tillDate = null, int? detailid = null)
        {
            string UserID = await user.GetUserIDAsync(User.Identity.Name);
            return Json(
                await IList.GetProductListAsync(QueryName,ProductFilterBy,ProductFilterValue,masterid,tillDate,detailid, UserID)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [AjaxOnly]
        [MyAuthorization]
        public async Task<IActionResult> GetReferenceListAsync([FromServices] IInventoryList IList, [FromServices] IUser user, string QueryName = "", string ReferenceFilterBy = "", string ReferenceFilterValue = "", int? masterid = null, DateTime? tillDate = null, int? detailid = null)
        {
            return Json(
                await IList.GetReferenceListAsync(QueryName, ReferenceFilterBy, ReferenceFilterValue, masterid, tillDate, detailid)
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }


        [MyAuthorization]
        public async Task<IActionResult> GetInitializedProductRegistrationAsync([FromServices] IAuthorizationScheme db, [FromServices] IProductRegistration db2, [FromServices] IInventoryList db3)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductRegistrationMasterCtlr",
                        WildCard = db2.GetWCLProductRegistrationMaster(),
                        Reports = db2.GetRLProductRegistration(),
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Product Registration"),
                        Otherdata = new { ProductClassificationList = await db3.GetProductClassificationListAsync(null,null) }
                    },
                    new Init_ViewSetupStructure()
                    {
                        Controller = "ProductRegistrationDetailCtlr",
                        WildCard = db2.GetWCLProductRegistrationDetail(),
                        Reports = null,
                        Privilege = null,
                        Otherdata = new {
                            ProductTypeCategoryList = await db3.GetProductTypeCategoryListAsync(null,null),
                            MeasurementUnitList = await db3.GetMeasurementUnitListAsync(null,null)
                        }
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Product Registration", Operation = "CanView")]
        public IActionResult ProductRegistrationIndex()
        {
            return View();
        }

        #region Master

        [AjaxOnly]
        [MyAuthorization(FormName = "Product Registration", Operation = "CanView")]
        public async Task<IActionResult> ProductRegistrationMasterLoad([FromServices] IProductRegistration db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductRegistrationMaster(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Product Registration", Operation = "CanPost")]
        public async Task<string> ProductRegistrationMasterPost([FromServices] IProductRegistration db, [FromBody] tbl_Inv_ProductRegistrationMaster tbl_Inv_ProductRegistrationMaster, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostProductRegistrationMaster(tbl_Inv_ProductRegistrationMaster, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Product Registration", Operation = "CanView")]
        public async Task<IActionResult> ProductRegistrationMasterGet([FromServices] IProductRegistration db, int ID)
        {
            return Json(await db.GetProductRegistrationMaster(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Product Registration", Operation = "CanPost")]
        public async Task<string> ProductRegistrationUploadExcelFile([FromServices] IProductRegistration db, IFormFile PDExcelFile, string operation = "")
        {
            if (PDExcelFile.Length > 0 && Path.GetExtension(PDExcelFile.FileName) == ".xlsx")
            {
                using (var ms = new MemoryStream())
                {
                    await PDExcelFile.CopyToAsync(ms);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(ms);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    List<ProdRegExcelData> ProdRegExcelDataList = new List<ProdRegExcelData>();

                    try
                    {

                        await Task.Factory.StartNew(() =>
                        {
                            
                            for (var rowNo = 2; rowNo <= worksheet.Dimension.End.Row; rowNo++)
                            {
                                ProdRegExcelDataList.Add(new ProdRegExcelData()
                                {
                                    ProductName = worksheet.Cells[rowNo, 1].Value == null ? "" : worksheet.Cells[rowNo, 1].Value.ToString(),
                                    ClassificationID = worksheet.Cells[rowNo, 2].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 2].Value.ToString(), out int aa0) ? Convert.ToInt32(worksheet.Cells[rowNo, 2].Value) : 0,

                                    CategoryID = worksheet.Cells[rowNo, 3].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 3].Value.ToString(), out int a0) ? Convert.ToInt32(worksheet.Cells[rowNo, 3].Value) : 0,
                                    UnitID = worksheet.Cells[rowNo, 4].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 4].Value.ToString(), out int a1) ? Convert.ToInt32(worksheet.Cells[rowNo, 4].Value) : 0,
                                    Split_Into = worksheet.Cells[rowNo, 5].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 5].Value.ToString(), out double a2) ? Convert.ToDouble(worksheet.Cells[rowNo, 5].Value) : 0,
                                    ReorderLevel = worksheet.Cells[rowNo, 6].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 6].Value.ToString(), out double a3) ? Convert.ToDouble(worksheet.Cells[rowNo, 6].Value) : 0,
                                    ProductCode = worksheet.Cells[rowNo, 7].Value == null ? "" : worksheet.Cells[rowNo, 7].Value.ToString(),

                                    CategoryID2 = worksheet.Cells[rowNo, 8].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 8].Value.ToString(), out int a02) ? Convert.ToInt32(worksheet.Cells[rowNo, 8].Value) : 0,
                                    UnitID2 = worksheet.Cells[rowNo, 9].Value == null ? 0 :
                                           int.TryParse(worksheet.Cells[rowNo, 9].Value.ToString(), out int a12) ? Convert.ToInt32(worksheet.Cells[rowNo, 9].Value) : 0,
                                    Split_Into2 = worksheet.Cells[rowNo, 10].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 10].Value.ToString(), out double a22) ? Convert.ToDouble(worksheet.Cells[rowNo, 10].Value) : 0,
                                    ReorderLevel2 = worksheet.Cells[rowNo, 11].Value == null ? 0 :
                                               double.TryParse(worksheet.Cells[rowNo, 11].Value.ToString(), out double a32) ? Convert.ToDouble(worksheet.Cells[rowNo, 11].Value) : 0,
                                    ProductCode2 = worksheet.Cells[rowNo, 12].Value == null ? "" : worksheet.Cells[rowNo, 12].Value.ToString(),

                                });
                            }
                        });

                        await db.ProductRegistrationUploadExcelFile(ProdRegExcelDataList, operation, User.Identity.Name);

                    }
                    catch (Exception ex)
                    {
                        return ex.InnerException.Message;
                    }
                }
            }
            else
            {
                return "File not Supported";
            }

            return "OK";
        }

        #endregion

        #region Detail

        [AjaxOnly]
        [MyAuthorization(FormName = "Product Registration", Operation = "CanView")]
        public async Task<IActionResult> ProductRegistrationDetailLoad([FromServices] IProductRegistration db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadProductRegistrationDetail(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Product Registration", Operation = "CanPost")]
        public async Task<string> ProductRegistrationDetailPost([FromServices] IProductRegistration db, [FromBody] tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostProductRegistrationDetail(tbl_Inv_ProductRegistrationDetail, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Product Registration", Operation = "CanView")]
        public async Task<IActionResult> ProductRegistrationDetailGet([FromServices] IProductRegistration db, int ID)
        {
            return Json(await db.GetProductRegistrationDetail(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpGet]
        [MyAuthorization(FormName = "Product Registration", Operation = "CanView")]
        public async Task<IActionResult> GetNodes([FromServices] IProductRegistration db, int PID, int MasterID)
        {
            return Json(await db.GetNodesAsync(PID,MasterID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region Report

        [MyAuthorization(FormName = "Product Registration", Operation = "CanViewReport")]
        public async Task<IActionResult> GetProductRegistrationReport([FromServices] IProductRegistration db, string rn = null, int id = 0, int SerialNoFrom = 0, int SerialNoTill = 0, DateTime? datefrom = null, DateTime? datetill = null, string SeekBy = "", string GroupBy = "", string OrderBy = "", int GroupID = 0)
        {
            return File(await db.GetPDFFileAsync(rn, id, SerialNoFrom, SerialNoTill, datefrom, datetill, SeekBy, GroupBy, OrderBy, "", GroupID, User.Identity.Name), rn.ToLower().Contains("excel") ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf");
        }

        #endregion

        #endregion

        #region Purchase Order Terms Conditions

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPOTermsConditionsAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseOrderTermsConditions db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "POTermsConditionsIndexCtlr",
                        WildCard = db2.GetWCLPurchaseOrderTermsConditions(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Purchase Order Terms Conditions"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Purchase Order Terms Conditions", Operation = "CanView")]
        public IActionResult POTermsConditionsIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> POTermsConditionsLoad([FromServices] IPurchaseOrderTermsConditions db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Purchase Order Terms Conditions", Operation = "CanPost")]
        public async Task<string> POTermsConditionsPost([FromServices] IPurchaseOrderTermsConditions db, [FromBody] tbl_Inv_PurchaseOrderTermsConditions tbl_Inv_PurchaseOrderTermsConditions, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Inv_PurchaseOrderTermsConditions, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Purchase Order Terms Conditions", Operation = "CanView")]
        public async Task<IActionResult> POTermsConditionsGet([FromServices] IPurchaseOrderTermsConditions db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PurchaseOrder Import Terms

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPOImportTermAsync([FromServices] IAuthorizationScheme db, [FromServices] IPurchaseOrderImportTerms db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "POImportTermIndexCtlr",
                        WildCard = db2.GetWCLPurchaseOrderImportTerm(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "PO Import Term"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "PO Import Term", Operation = "CanView")]
        public IActionResult POImportTermIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> POImportTermLoad([FromServices] IPurchaseOrderImportTerms db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PO Import Term", Operation = "CanPost")]
        public async Task<string> POImportTermPost([FromServices] IPurchaseOrderImportTerms db, [FromBody] tbl_Inv_PurchaseOrder_ImportTerms tbl_Inv_PurchaseOrder_ImportTerms, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Inv_PurchaseOrder_ImportTerms, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PO Import Term", Operation = "CanView")]
        public async Task<IActionResult> POImportTermGet([FromServices] IPurchaseOrderImportTerms db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PurchaseOrder Supplier

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPOSupplierAsync([FromServices] IAuthorizationScheme db, [FromServices] IPOSupplier db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "POSupplierIndexCtlr",
                        WildCard = db2.GetWCLSupplier(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "PO Supplier"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "PO Supplier", Operation = "CanView")]
        public IActionResult POSupplierIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> POSupplierLoad([FromServices] IPOSupplier db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PO Supplier", Operation = "CanPost")]
        public async Task<string> POSupplierPost([FromServices] IPOSupplier db, [FromBody] tbl_Inv_PurchaseOrder_Supplier tbl_Inv_PurchaseOrder_Supplier, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Inv_PurchaseOrder_Supplier, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PO Supplier", Operation = "CanView")]
        public async Task<IActionResult> POSupplierGet([FromServices] IPOSupplier db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PO Manufacturer

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPOManufacturerAsync([FromServices] IAuthorizationScheme db, [FromServices] IPOManufacturer db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "POManufacturerIndexCtlr",
                        WildCard = db2.GetWCLManufacturer(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "PO Manufacturer"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "PO Manufacturer", Operation = "CanView")]
        public IActionResult POManufacturerIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> POManufacturerLoad([FromServices] IPOManufacturer db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PO Manufacturer", Operation = "CanPost")]
        public async Task<string> POManufacturerPost([FromServices] IPOManufacturer db, [FromBody] tbl_Inv_PurchaseOrder_Manufacturer tbl_Inv_PurchaseOrder_Manufacturer, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Inv_PurchaseOrder_Manufacturer, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PO Manufacturer", Operation = "CanView")]
        public async Task<IActionResult> POManufacturerGet([FromServices] IPOManufacturer db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region PurchaseOrder Indenter

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedPOIndenterAsync([FromServices] IAuthorizationScheme db, [FromServices] IPOIndenter db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "POIndenterIndexCtlr",
                        WildCard = db2.GetWCLIndenter(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "PO Indenter"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "PO Indenter", Operation = "CanView")]
        public IActionResult POIndenterIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> POIndenterLoad([FromServices] IPOIndenter db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.Load(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "PO Indenter", Operation = "CanPost")]
        public async Task<string> POIndenterPost([FromServices] IPOIndenter db, [FromBody] tbl_Inv_PurchaseOrder_Indenter tbl_Inv_PurchaseOrder_Indenter, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.Post(tbl_Inv_PurchaseOrder_Indenter, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "PO Indenter", Operation = "CanView")]
        public async Task<IActionResult> POIndenterGet([FromServices] IPOIndenter db, int ID)
        {
            return Json(await db.Get(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region TransportType

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedTransportTypeAsync([FromServices] IAuthorizationScheme db, [FromServices] ITransportType db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "TransportTypeIndexCtlr",
                        WildCard = db2.GetWCLTransportType(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "Transport Type"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "Transport Type", Operation = "CanView")]
        public IActionResult TransportTypeIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> TransportTypeLoad([FromServices] ITransportType db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadTransportType(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "Transport Type", Operation = "CanPost")]
        public async Task<string> TransportTypePost([FromServices] ITransportType db, [FromBody] tbl_Inv_TransportType tbl_Inv_TransportType, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostTransportType(tbl_Inv_TransportType, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "Transport Type", Operation = "CanView")]
        public async Task<IActionResult> TransportTypeGet([FromServices] ITransportType db, int ID)
        {
            return Json(await db.GetTransportType(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion

        #region InternationalCommercialTerm

        [MyAuthorization]
        public async Task<IActionResult> GetInitializedInternationalCommercialTermAsync([FromServices] IAuthorizationScheme db, [FromServices] IInternationalCommercialTerm db2)
        {
            return Json(
                new List<Init_ViewSetupStructure>()
                {
                    new Init_ViewSetupStructure()
                    {
                        Controller = "InternationalCommercialTermIndexCtlr",
                        WildCard = db2.GetWCLInternationalCommercialTerm(),
                        Reports = null,
                        Privilege = await db.GetUserAuthorizatedOnOperationAsync("Inventory", User.Identity.Name, "International Commercial Term"),
                        Otherdata = null
                    }
                }
                , new Newtonsoft.Json.JsonSerializerSettings()
                );
        }

        [MyAuthorization(FormName = "International Commercial Term", Operation = "CanView")]
        public IActionResult InternationalCommercialTermIndex()
        {
            return View();
        }

        [AjaxOnly]
        public async Task<IActionResult> InternationalCommercialTermLoad([FromServices] IInternationalCommercialTerm db,
            int CurrentPage = 1, int MasterID = 0,
            string FilterByText = null, string FilterValueByText = null,
            string FilterByNumberRange = null, int FilterValueByNumberRangeFrom = 0, int FilterValueByNumberRangeTill = 0,
            string FilterByDateRange = null, DateTime? FilterValueByDateRangeFrom = null, DateTime? FilterValueByDateRangeTill = null,
            string FilterByLoad = null)
        {
            PagedData<object> pageddata =
                await db.LoadInternationalCommercialTerm(CurrentPage, MasterID, FilterByText, FilterValueByText,
                FilterByNumberRange, FilterValueByNumberRangeFrom, FilterValueByNumberRangeTill,
                FilterByDateRange, FilterValueByDateRangeFrom, FilterValueByDateRangeTill,
                FilterByLoad);

            return Json(new { pageddata }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorization(FormName = "International Commercial Term", Operation = "CanPost")]
        public async Task<string> InternationalCommercialTermPost([FromServices] IInternationalCommercialTerm db, [FromBody] tbl_Inv_InternationalCommercialTerm tbl_Inv_InternationalCommercialTerm, string operation = "")
        {
            if (ModelState.IsValid)
                return await db.PostInternationalCommercialTerm(tbl_Inv_InternationalCommercialTerm, operation, User.Identity.Name);
            else
                return CustomMessage.ModelValidationFailedMessage(ModelState);
        }

        [MyAuthorization(FormName = "International Commercial Term", Operation = "CanView")]
        public async Task<IActionResult> InternationalCommercialTermGet([FromServices] IInternationalCommercialTerm db, int ID)
        {
            return Json(await db.GetInternationalCommercialTerm(ID), new Newtonsoft.Json.JsonSerializerSettings());
        }

        #endregion
    }
}
