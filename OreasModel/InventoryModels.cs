using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace OreasModel
{
    [Table("tbl_Inv_MeasurementUnit")]
    public class tbl_Inv_MeasurementUnit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Measurement Unit")]
        public string MeasurementUnit { get; set; }

        [Required]
        [Display(Name = "Is Decimal/Whole No")]
        public bool IsDecimal { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_ProductRegistrationDetail.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Inv_ProductRegistrationDetail> tbl_Inv_ProductRegistrationDetails { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionMaster.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Pro_CompositionMaster> tbl_Pro_CompositionMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionMaster.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionMaster> tbl_Pro_BatchMaterialRequisitionMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderDetail.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Inv_PurchaseOrderDetail> tbl_Inv_PurchaseOrderDetails { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionMaster_ProcessBMR_QcTest.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Pro_CompositionMaster_ProcessBMR_QcTest> tbl_Pro_CompositionMaster_ProcessBMR_QcTests { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest> tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBMR_QcTest.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Qc_SampleProcessBMR_QcTest> tbl_Qc_SampleProcessBMR_QcTests { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBPR_QcTest.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Qc_SampleProcessBPR_QcTest> tbl_Qc_SampleProcessBPR_QcTests { get; set; }

        [InverseProperty(nameof(tbl_Inv_ProductRegistrationDetail_PNQcTest.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Inv_ProductRegistrationDetail_PNQcTest> tbl_Inv_ProductRegistrationDetail_PNQcTests { get; set; }

        [InverseProperty(nameof(tbl_Qc_PurchaseNoteDetail_QcTest.tbl_Inv_MeasurementUnit))]
        public virtual ICollection<tbl_Qc_PurchaseNoteDetail_QcTest> tbl_Qc_PurchaseNoteDetail_QcTests { get; set; }

    }

    [Table("tbl_Inv_ProductClassification")]
    public class tbl_Inv_ProductClassification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Measurement Unit")]
        public string ClassName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_ProductRegistrationMaster.tbl_Inv_ProductClassification))]
        public virtual ICollection<tbl_Inv_ProductRegistrationMaster> tbl_Inv_ProductRegistrationMasters { get; set; }

    }

    [Table("tbl_Inv_ProductType", Schema = "dbo")]
    public class tbl_Inv_ProductType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Product Type")]
        public string ProductType { get; set; }

        [Required]
        [MaxLength(5)]
        public string Prefix { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Qc_ActionType))]
        public int FK_tbl_Qc_ActionType_ID_PurchaseNote { get; set; }
        public virtual tbl_Qc_ActionType tbl_Qc_ActionType { get; set; }

        [Required]
        public bool PurchaseNoteDetailRateAutoInsertFromPO { get; set; }

        [Required]
        public bool PurchaseNoteDetailWithOutPOAllowed { get; set; }


        [Required]
        public bool SalesNoteDetailRateAutoInsertFromON { get; set; }

        [Required]
        public bool SalesNoteDetailWithOutONAllowed { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_ProductType_Category.tbl_Inv_ProductType))]
        public virtual ICollection<tbl_Inv_ProductType_Category> tbl_Inv_ProductType_Categorys { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionFilterPolicyMaster.tbl_Inv_ProductType_Coupling))]
        public virtual ICollection<tbl_Pro_CompositionFilterPolicyMaster> tbl_Pro_CompositionFilterPolicyMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionFilterPolicyMaster.tbl_Inv_ProductType_QCSample))]
        public virtual ICollection<tbl_Pro_CompositionFilterPolicyMaster> tbl_Pro_CompositionFilterPolicyMasters_QCSample { get; set; }


        [InverseProperty(nameof(tbl_Ac_PolicyInventory.tbl_Inv_ProductType))]
        public virtual ICollection<tbl_Ac_PolicyInventory> tbl_Ac_PolicyInventorys { get; set; }
    }

    [Table("tbl_Inv_ProductType_Category")]
    public class tbl_Inv_ProductType_Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductType))]
        public int FK_tbl_Inv_ProductType_ID { get; set; }
        public virtual tbl_Inv_ProductType tbl_Inv_ProductType { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_WareHouseDetail.tbl_Inv_ProductType_Category))]
        public virtual ICollection<tbl_Inv_WareHouseDetail> tbl_Inv_WareHouseDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductType_Category))]
        public virtual ICollection<tbl_Inv_ProductRegistrationDetail> tbl_Inv_ProductRegistrationDetails { get; set; }

    }

    [Table("tbl_Inv_WareHouseMaster")]
    public class tbl_Inv_WareHouseMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "WareHouse Name")]
        public string WareHouseName { get; set; }

        [Required]
        [MaxLength(5)]
        public string Prefix { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_WareHouseDetail.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Inv_WareHouseDetail> tbl_Inv_WareHouseDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

        //-------------------Inventory----------------------------------//

        [InverseProperty(nameof(tbl_Inv_PurchaseNoteMaster.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Inv_PurchaseNoteMaster> tbl_Inv_PurchaseNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseReturnNoteMaster.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Inv_PurchaseReturnNoteMaster> tbl_Inv_PurchaseReturnNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesNoteMaster.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Inv_SalesNoteMaster> tbl_Inv_SalesNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteMaster.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteMaster> tbl_Inv_SalesReturnNoteMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseRequestMaster.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Inv_PurchaseRequestMaster> tbl_Inv_PurchaseRequestMasters { get; set; }

        //---------------Production--------------------------//
        [InverseProperty(nameof(tbl_Pro_CompositionFilterPolicyMaster.tbl_Inv_WareHouseMaster_By))]
        public virtual ICollection<tbl_Pro_CompositionFilterPolicyMaster> tbl_Pro_CompositionFilterPolicyMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionFilterPolicyDetail.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Pro_CompositionFilterPolicyDetail> tbl_Pro_CompositionFilterPolicyDetails { get; set; }

        //------------------------BMR----------------------------------//

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail> tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails { get; set; }

        [InverseProperty(nameof(tbl_Pro_BMRAdditionalMaster.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Pro_BMRAdditionalMaster> tbl_Pro_BMRAdditionalMasters { get; set; }

        //-----------------------------------Ordinary Requisition----------------------//
        [InverseProperty(nameof(tbl_Inv_OrdinaryRequisitionMaster.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Inv_OrdinaryRequisitionMaster> tbl_Inv_OrdinaryRequisitionMasters { get; set; }

        //------------------------Production Transfer----------------------------------//

        [InverseProperty(nameof(tbl_Pro_ProductionTransferMaster.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_Pro_ProductionTransferMaster> tbl_Pro_ProductionTransferMasters { get; set; }

        //------------------------Stock Transfer----------------------------------//

        [InverseProperty(nameof(tbl_Inv_StockTransferMaster.tbl_Inv_WareHouseMaster_From))]
        public virtual ICollection<tbl_Inv_StockTransferMaster> tbl_Inv_StockTransferMaster_From { get; set; }


        [InverseProperty(nameof(tbl_Inv_StockTransferMaster.tbl_Inv_WareHouseMaster_To))]
        public virtual ICollection<tbl_Inv_StockTransferMaster> tbl_Inv_StockTransferMaster_To { get; set; }

        //------------------------Authorization Scheme----------------------------------//

        [InverseProperty(nameof(AspNetOreasAuthorizationScheme_WareHouse.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<AspNetOreasAuthorizationScheme_WareHouse> AspNetOreasAuthorizationScheme_WareHouses { get; set; }

        //-------------------------------//
        [InverseProperty(nameof(tbl_PD_RequestDetailTR_CFP.tbl_Inv_WareHouseMaster))]
        public virtual ICollection<tbl_PD_RequestDetailTR_CFP> tbl_PD_RequestDetailTR_CFPs { get; set; }


    }

    [Table("tbl_Inv_WareHouseDetail")]
    public class tbl_Inv_WareHouseDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductType_Category))]
        public int FK_tbl_Inv_ProductType_Category_ID { get; set; }
        public virtual tbl_Inv_ProductType_Category tbl_Inv_ProductType_Category { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


    }

    [Table("tbl_Inv_ProductRegistrationMaster")]
    public class tbl_Inv_ProductRegistrationMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductClassification))]
        public int FK_tbl_Inv_ProductClassification_ID { get; set; }
        public virtual tbl_Inv_ProductClassification tbl_Inv_ProductClassification { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [MaxLength(50)]
        [Display(Name = "Control Procedure No")]
        public string ControlProcedureNo { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationMaster))]
        public virtual ICollection<tbl_Inv_ProductRegistrationDetail> tbl_Inv_ProductRegistrationDetails { get; set; }

    }

    [Table("tbl_Inv_ProductRegistrationDetail")]
    public class tbl_Inv_ProductRegistrationDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationMaster))]
        public int FK_tbl_Inv_ProductRegistrationMaster_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationMaster tbl_Inv_ProductRegistrationMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductType_Category))]
        public int FK_tbl_Inv_ProductType_Category_ID { get; set; }
        public virtual tbl_Inv_ProductType_Category tbl_Inv_ProductType_Category { get; set; }

        [MaxLength(20)]
        public string Description { get; set; }

        [MaxLength(15)]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_MeasurementUnit))]
        public int FK_tbl_Inv_MeasurementUnit_ID { get; set; }
        public virtual tbl_Inv_MeasurementUnit tbl_Inv_MeasurementUnit { get; set; }

        [Required]
        [Display(Name = "Reorder Level")]
        public double ReorderLevel { get; set; }

        [Required]
        [Display(Name = "Reorder Alert")]
        public bool ReorderAlert { get; set; }

        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_Parent))]
        public int? FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_Parent { get; set; }

        [Required]
        [Display(Name = "Split Into")]
        public double Split_Into { get; set; }

        [Required]
        [Display(Name = "Is Inventory")]
        public bool IsInventory { get; set; }

        [Required]
        public bool IsDiscontinue { get; set; }

        [MaxLength(50)]
        [Display(Name = "Harmonized Code")]
        public string HarmonizedCode { get; set; }

        [MaxLength(50)]
        [Display(Name = "GTIN Code")]
        public string GTINCode { get; set; }

        [Display(Name = "Standard MRP")]
        public double? StandardMRP { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------self relation---------------------//

        [InverseProperty(nameof(tbl_Inv_ProductRegistrationDetail.tbl_Inv_ProductRegistrationDetail_Parent))]
        public virtual ICollection<tbl_Inv_ProductRegistrationDetail> tbl_Inv_ProductRegistrationDetails_Parents { get; set; }

        [InverseProperty(nameof(tbl_Inv_ProductRegistrationDetail_PNQcTest.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_ProductRegistrationDetail_PNQcTest> tbl_Inv_ProductRegistrationDetail_PNQcTests { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

        //-----------------------------------Inventory---------------------------------//
        [InverseProperty(nameof(tbl_Inv_PurchaseNoteDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_PurchaseNoteDetail> tbl_Inv_PurchaseNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseReturnNoteDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_PurchaseReturnNoteDetail> tbl_Inv_PurchaseReturnNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesNoteDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_SalesNoteDetail> tbl_Inv_SalesNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteDetail> tbl_Inv_SalesReturnNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseRequestDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_PurchaseRequestDetail> tbl_Inv_PurchaseRequestDetails { get; set; }

        //-----------------------------------BMR---------------------------------//
        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.tbl_Inv_ProductRegistrationDetail_QCSample))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR> tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.tbl_Inv_ProductRegistrationDetail_QCSample))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs { get; set; }

        //-----------------------------------Composition---------------------------------//
        [InverseProperty(nameof(tbl_Pro_CompositionDetail_RawDetail_Items.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Pro_CompositionDetail_RawDetail_Items> tbl_Pro_CompositionDetail_RawDetail_Itemss { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling> tbl_Pro_CompositionDetail_Couplings { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Primary))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingMaster> tbl_Pro_CompositionDetail_Coupling_PackagingMasters_Primary { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Secondary))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingMaster> tbl_Pro_CompositionDetail_Coupling_PackagingMasters_Secondary { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items> tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss { get; set; }

        //----------------------------------------BMR----------------------------------//
        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionMaster.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionMaster> tbl_Pro_BatchMaterialRequisitionMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_RawDetail> tbl_Pro_BatchMaterialRequisitionDetail_RawDetails { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Primary))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters_Primary { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.tbl_Inv_ProductRegistrationDetail_Secondary))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters_Secondary { get; set; }


        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items> tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss { get; set; }

        //-------------------------------------BMR Additional-------------------------------------//
        [InverseProperty(nameof(tbl_Pro_BMRAdditionalDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Pro_BMRAdditionalDetail> tbl_Pro_BMRAdditionalDetails { get; set; }

        //-----------------------------------Supply chain---------------------------------//
        [InverseProperty(nameof(tbl_Inv_PurchaseOrderDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_PurchaseOrderDetail> tbl_Inv_PurchaseOrderDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrderNoteDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_OrderNoteDetail> tbl_Inv_OrderNoteDetails { get; set; }

        //-------------------------------------Ordinary Requisition-------------------------------------//
        [InverseProperty(nameof(tbl_Inv_OrdinaryRequisitionDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_OrdinaryRequisitionDetail> tbl_Inv_OrdinaryRequisitionDetails { get; set; }

        //--------------------------------------------------------------------------------//
        [InverseProperty(nameof(tbl_PD_RequestMaster.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_PD_RequestMaster> tbl_PD_RequestMasters { get; set; }

        [InverseProperty(nameof(tbl_PD_RequestMaster.tbl_Inv_ProductRegistrationDetail_Primary))]
        public virtual ICollection<tbl_PD_RequestMaster> tbl_PD_RequestMasters_Primary { get; set; }

        [InverseProperty(nameof(tbl_PD_RequestDetailTR_CFP_Item.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_PD_RequestDetailTR_CFP_Item> tbl_PD_RequestDetailTR_CFP_Items { get; set; }

        //-------------------------------------Production Transfer-------------------------------------//
        [InverseProperty(nameof(tbl_Pro_ProductionTransferDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Pro_ProductionTransferDetail> tbl_Pro_ProductionTransferDetails { get; set; }

        //-------------------------------------Stock Transfer-------------------------------------//
        [InverseProperty(nameof(tbl_Inv_StockTransferDetail.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Inv_StockTransferDetail> tbl_Inv_StockTransferDetails { get; set; }

        //----------------------------------------------------------------------------------//
        [InverseProperty(nameof(tbl_Ac_CompositionCostingIndirectExpense.tbl_Inv_ProductRegistrationDetail))]
        public virtual ICollection<tbl_Ac_CompositionCostingIndirectExpense> tbl_Ac_CompositionCostingIndirectExpenses { get; set; }


    }

    [Table("tbl_Inv_ProductRegistrationDetail_PNQcTest")]
    public class tbl_Inv_ProductRegistrationDetail_PNQcTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Qc_Test))]
        public int FK_tbl_Qc_Test_ID { get; set; }
        public virtual tbl_Qc_Test tbl_Qc_Test { get; set; }

        [MaxLength(100)]
        [Display(Name = "Test Description")]
        public string TestDescription { get; set; }

        [MaxLength(250)]
        public string Specification { get; set; }

        [Display(Name = "Range From")]
        public double? RangeFrom { get; set; }

        [Display(Name = "Range Till")]
        public double? RangeTill { get; set; }

        [ForeignKey(nameof(tbl_Inv_MeasurementUnit))]
        public int? FK_tbl_Inv_MeasurementUnit_ID { get; set; }
        public virtual tbl_Inv_MeasurementUnit tbl_Inv_MeasurementUnit { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_Inv_Ledger")]
    public class tbl_Inv_Ledger
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [Required]
        [Display(Name = "Posting Date")]
        public DateTime PostingDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal QuantityIn { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal QuantityOut { get; set; }

        [MaxLength(100)]
        public string Narration { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        public int? PostingNo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal BalanceByWareHouse { get; set; }

        [MaxLength(15)]
        public string TrackingNo { get; set; }

        public int? FK_tbl_Ac_Ledger_ID { get; set; }

        //------------------------challan---------------------------//

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseReturnNoteDetail))]
        public int? FK_tbl_Inv_PurchaseReturnNoteDetail_ID { get; set; }
        public virtual tbl_Inv_PurchaseReturnNoteDetail tbl_Inv_PurchaseReturnNoteDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_SalesNoteDetail))]
        public int? FK_tbl_Inv_SalesNoteDetail_ID { get; set; }
        public virtual tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_SalesReturnNoteDetail))]
        public int? FK_tbl_Inv_SalesReturnNoteDetail_ID { get; set; }
        public virtual tbl_Inv_SalesReturnNoteDetail tbl_Inv_SalesReturnNoteDetail { get; set; }

        //------------------------Dispensing---------------------------//

        [ForeignKey(nameof(tbl_Inv_OrdinaryRequisitionDispensing))]
        public int? FK_tbl_Inv_OrdinaryRequisitionDispensing_ID { get; set; }
        public virtual tbl_Inv_OrdinaryRequisitionDispensing tbl_Inv_OrdinaryRequisitionDispensing { get; set; }

        [ForeignKey(nameof(tbl_Inv_BMRDispensingRaw))]
        public int? FK_tbl_Inv_BMRDispensingRaw_ID { get; set; }
        public virtual tbl_Inv_BMRDispensingRaw tbl_Inv_BMRDispensingRaw { get; set; }

        [ForeignKey(nameof(tbl_Inv_BMRDispensingPackaging))]
        public int? FK_tbl_Inv_BMRDispensingPackaging_ID { get; set; }
        public virtual tbl_Inv_BMRDispensingPackaging tbl_Inv_BMRDispensingPackaging { get; set; }

        [ForeignKey(nameof(tbl_Inv_BMRAdditionalDispensing))]
        public int? FK_tbl_Inv_BMRAdditionalDispensing_ID { get; set; }
        public virtual tbl_Inv_BMRAdditionalDispensing tbl_Inv_BMRAdditionalDispensing { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionMaster))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster { get; set; }

        //------------------------Production Transfer---------------------------//
        [ForeignKey(nameof(tbl_Pro_ProductionTransferDetail))]
        public int? FK_tbl_Pro_ProductionTransferDetail_ID { get; set; }
        public virtual tbl_Pro_ProductionTransferDetail tbl_Pro_ProductionTransferDetail { get; set; }


        //------------------------Stock Transfer---------------------------//
        [ForeignKey(nameof(tbl_Inv_StockTransferDetail))]
        public int? FK_tbl_Inv_StockTransferDetail_ID { get; set; }
        public virtual tbl_Inv_StockTransferDetail tbl_Inv_StockTransferDetail { get; set; }

        //------------------------Product Development---------------------------//
        [ForeignKey(nameof(tbl_Inv_PDRequestDispensing))]
        public int? FK_tbl_Inv_PDRequestDispensing_ID { get; set; }
        public virtual tbl_Inv_PDRequestDispensing tbl_Inv_PDRequestDispensing { get; set; }
    }

    //--------------------------------Purchase challan-------------------------------------//

    [Table("tbl_Inv_PurchaseNoteMaster")]
    public class tbl_Inv_PurchaseNoteMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [MaxLength(15)]
        [Display(Name = "Supp Challan #")]
        public string SupplierChallanNo { get; set; }

        [MaxLength(15)]
        [Display(Name = "Supp Invoice #")]
        public string SupplierInvoiceNo { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Total Net Amount")]
        public double TotalNetAmount { get; set; }

        [Required]
        public bool IsProcessedAll { get; set; }

        [Required]
        public bool IsSupervisedAll { get; set; }

        [Required]
        public bool IsQCAll { get; set; }

        [Required]
        public bool IsQCSampleAll { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseNoteDetail.tbl_Inv_PurchaseNoteMaster))]
        public virtual ICollection<tbl_Inv_PurchaseNoteDetail> tbl_Inv_PurchaseNoteDetails { get; set; }

    }

    [Table("tbl_Inv_PurchaseNoteDetail")]
    public class tbl_Inv_PurchaseNoteDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_PurchaseNoteMaster))]
        public int FK_tbl_Inv_PurchaseNoteMaster_ID { get; set; }
        public virtual tbl_Inv_PurchaseNoteMaster tbl_Inv_PurchaseNoteMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        [Display(Name = "Gross Amount")]
        public double GrossAmount { get; set; }

        [Required]
        [Display(Name = "GST %")]
        public double GSTPercentage { get; set; }

        [Required]
        [Display(Name = "GST Amount")]
        public double GSTAmount { get; set; }

        [Required]
        [Display(Name = "Freight In Charges")]
        public double FreightIn { get; set; }

        [Required]
        [Display(Name = "Disc. Amount")]
        public double DiscountAmount { get; set; }

        [Required]
        [Display(Name = "Cost Amount")]
        public double CostAmount { get; set; }

        [Required]
        [Display(Name = "WHT %")]
        public double WHTPercentage { get; set; }

        [Required]
        [Display(Name = "WHT Amount")]
        public double WHTAmount { get; set; }

        [Required]
        [Display(Name = "Net Amount")]
        public double NetAmount { get; set; }

        [MaxLength(20)]
        [Display(Name = "Mfg Batch#")]
        public string MfgBatchNo { get; set; }

        [Display(Name = "Mfg Date")]
        public DateTime? MfgDate { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Display(Name = "Reference No")]
        [MaxLength(15)]
        public string ReferenceNo { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseOrderDetail))]
        public int? FK_tbl_Inv_PurchaseOrderDetail_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrderDetail tbl_Inv_PurchaseOrderDetail { get; set; }

        [Display(Name = "No Of Containers")]
        [MaxLength(10)]
        public string NoOfContainers { get; set; }

        [Required]
        [Display(Name = "Potency %")]
        public double PotencyPercentage { get; set; }

        [Required]
        public bool IsProcessed { get; set; }

        [Required]
        public bool IsSupervised { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //---------------------------qc-----------------------------//
        [Required]
        [ForeignKey(nameof(tbl_Qc_ActionType))]
        public int FK_tbl_Qc_ActionType_ID { get; set; }
        public virtual tbl_Qc_ActionType tbl_Qc_ActionType { get; set; }

        [Required]
        [Display(Name = "Quantity Sample")]
        public double QuantitySample { get; set; }

        [Display(Name = "Retest Date")]
        public DateTime? RetestDate { get; set; }

        [Display(Name = "QC Comments")]
        [MaxLength(150)]
        public string QCComments { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By QcQa")]
        public string CreatedByQcQa { get; set; }

        [Display(Name = "Created Date QcQa")]
        public DateTime? CreatedDateQcQa { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By QcQa")]
        public string ModifiedByQcQa { get; set; }

        [Display(Name = "Modified Date QcQa")]
        public DateTime? ModifiedDateQcQa { get; set; }
        //-----------------------xxxxxxxxxxxxxxxxxxxxxxxxxxxx-----------------------//

        [InverseProperty(nameof(tbl_Inv_PurchaseReturnNoteDetail.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_PurchaseReturnNoteDetail> tbl_Inv_PurchaseReturnNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesNoteDetail.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_SalesNoteDetail> tbl_Inv_SalesNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteDetail.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteDetail> tbl_Inv_SalesReturnNoteDetails { get; set; }

        //----------------------dispensing-----------------------------//
        [InverseProperty(nameof(tbl_Inv_OrdinaryRequisitionDispensing.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_OrdinaryRequisitionDispensing> tbl_Inv_OrdinaryRequisitionDispensings { get; set; }

        [InverseProperty(nameof(tbl_Inv_BMRAdditionalDispensing.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_BMRAdditionalDispensing> tbl_Inv_BMRAdditionalDispensings { get; set; }

        [InverseProperty(nameof(tbl_Inv_BMRDispensingRaw.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_BMRDispensingRaw> tbl_Inv_BMRDispensingRaws { get; set; }

        [InverseProperty(nameof(tbl_Inv_BMRDispensingPackaging.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_BMRDispensingPackaging> tbl_Inv_BMRDispensingPackagings { get; set; }

        [InverseProperty(nameof(tbl_Inv_PDRequestDispensing.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_PDRequestDispensing> tbl_Inv_PDRequestDispensings { get; set; }

        //----------------------stock transfer-----------------------------//
        [InverseProperty(nameof(tbl_Inv_StockTransferDetail.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_StockTransferDetail> tbl_Inv_StockTransferDetails { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_PurchaseNoteDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        //----------------------Item-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_PurchaseNoteDetail_RefNo))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers_RefNo { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_PurchaseNoteDetail))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

        //----------------------Qc Test-----------------------------//
        [InverseProperty(nameof(tbl_Qc_PurchaseNoteDetail_QcTest.tbl_Inv_PurchaseNoteDetail))]
        public virtual ICollection<tbl_Qc_PurchaseNoteDetail_QcTest> tbl_Qc_PurchaseNoteDetail_QcTests { get; set; }

    }

    [Table("tbl_Inv_PurchaseReturnNoteMaster")]
    public class tbl_Inv_PurchaseReturnNoteMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Total Net Amount")]
        public double TotalNetAmount { get; set; }

        [Required]
        public bool IsProcessedAll { get; set; }

        [Required]
        public bool IsSupervisedAll { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseReturnNoteDetail.tbl_Inv_PurchaseReturnNoteMaster))]
        public virtual ICollection<tbl_Inv_PurchaseReturnNoteDetail> tbl_Inv_PurchaseReturnNoteDetails { get; set; }

    }

    [Table("tbl_Inv_PurchaseReturnNoteDetail")]
    public class tbl_Inv_PurchaseReturnNoteDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_PurchaseReturnNoteMaster))]
        public int FK_tbl_Inv_PurchaseReturnNoteMaster_ID { get; set; }
        public virtual tbl_Inv_PurchaseReturnNoteMaster tbl_Inv_PurchaseReturnNoteMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        [Display(Name = "Gross Amount")]
        public double GrossAmount { get; set; }

        [Required]
        [Display(Name = "GST %")]
        public double GSTPercentage { get; set; }

        [Required]
        [Display(Name = "GST Amount")]
        public double GSTAmount { get; set; }

        [Required]
        [Display(Name = "Freight In Charges")]
        public double FreightIn { get; set; }

        [Required]
        [Display(Name = "Disc. Amount")]
        public double DiscountAmount { get; set; }

        [Required]
        [Display(Name = "Cost Amount")]
        public double CostAmount { get; set; }

        [Required]
        [Display(Name = "WHT %")]
        public double WHTPercentage { get; set; }

        [Required]
        [Display(Name = "WHT Amount")]
        public double WHTAmount { get; set; }

        [Required]
        [Display(Name = "Net Amount")]
        public double NetAmount { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        public bool IsProcessed { get; set; }

        [Required]
        public bool IsSupervised { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_PurchaseReturnNoteDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        //----------------------Item-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_PurchaseReturnNoteDetail))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

    }

    //--------------------------------sales Challan-------------------------------//
    [Table("tbl_Inv_SalesNoteMaster")]
    public class tbl_Inv_SalesNoteMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [ForeignKey(nameof(tbl_Ac_CustomerSubDistributorList))]
        public int? FK_tbl_Ac_CustomerSubDistributorList_ID { get; set; }
        public virtual tbl_Ac_CustomerSubDistributorList tbl_Ac_CustomerSubDistributorList { get; set; }


        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Total Net Amount")]
        public double TotalNetAmount { get; set; }

        [Required]
        public bool IsProcessedAll { get; set; }

        [Required]
        public bool IsSupervisedAll { get; set; }

        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_Transporter))]
        public int? FK_tbl_Ac_ChartOfAccounts_ID_Transporter { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_Transporter { get; set; }

        [Required]
        [Display(Name = "Transport Charges")]
        public double TransportCharges { get; set; }

        [MaxLength(50)]
        [Display(Name = "Transporter Bilty No")]
        public string TransporterBiltyNo { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesNoteDetail.tbl_Inv_SalesNoteMaster))]
        public virtual ICollection<tbl_Inv_SalesNoteDetail> tbl_Inv_SalesNoteDetails { get; set; }
    }

    [Table("tbl_Inv_SalesNoteDetail")]
    public class tbl_Inv_SalesNoteDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_SalesNoteMaster))]
        public int FK_tbl_Inv_SalesNoteMaster_ID { get; set; }
        public virtual tbl_Inv_SalesNoteMaster tbl_Inv_SalesNoteMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        [Display(Name = "Gross Amount")]
        public double GrossAmount { get; set; }

        [Required]
        [Display(Name = "ST %")]
        public double STPercentage { get; set; }

        [Required]
        [Display(Name = "ST Amount")]
        public double STAmount { get; set; }

        [Required]
        [Display(Name = "FST %")]
        public double FSTPercentage { get; set; }

        [Required]
        [Display(Name = "FST Amount")]
        public double FSTAmount { get; set; }

        [Required]
        [Display(Name = "WHT %")]
        public double WHTPercentage { get; set; }

        [Required]
        [Display(Name = "WHT Amount")]
        public double WHTAmount { get; set; }

        [Required]
        [Display(Name = "Disc. Amount")]
        public double DiscountAmount { get; set; }

        [Required]
        [Display(Name = "Net Amount")]
        public double NetAmount { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        public bool IsProcessed { get; set; }

        [Required]
        public bool IsSupervised { get; set; }

        [Required]
        [Display(Name = "No Of Packages")]
        public double NoOfPackages { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [ForeignKey(nameof(tbl_Inv_OrderNoteDetail))]
        public int? FK_tbl_Inv_OrderNoteDetail_ID { get; set; }
        public virtual tbl_Inv_OrderNoteDetail tbl_Inv_OrderNoteDetail { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_SalesNoteDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        //----------------------Item-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_SalesNoteDetail))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

        //----------------------Sales Return-----------------------------//
        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteDetail.tbl_Inv_SalesNoteDetail))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteDetail> tbl_Inv_SalesReturnNoteDetails { get; set; }
    }

    [Table("tbl_Inv_SalesReturnNoteMaster")]
    public class tbl_Inv_SalesReturnNoteMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [ForeignKey(nameof(tbl_Ac_CustomerSubDistributorList))]
        public int? FK_tbl_Ac_CustomerSubDistributorList_ID { get; set; }
        public virtual tbl_Ac_CustomerSubDistributorList tbl_Ac_CustomerSubDistributorList { get; set; }


        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Total Net Amount")]
        public double TotalNetAmount { get; set; }

        [Required]
        public bool IsProcessedAll { get; set; }

        [Required]
        public bool IsSupervisedAll { get; set; }

        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts_Transporter))]
        public int? FK_tbl_Ac_ChartOfAccounts_ID_Transporter { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts_Transporter { get; set; }

        [Required]
        [Display(Name = "Transport Charges")]
        public double TransportCharges { get; set; }

        [MaxLength(50)]
        [Display(Name = "Transporter Bilty No")]
        public string TransporterBiltyNo { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteDetail.tbl_Inv_SalesReturnNoteMaster))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteDetail> tbl_Inv_SalesReturnNoteDetails { get; set; }
    }

    [Table("tbl_Inv_SalesReturnNoteDetail")]
    public class tbl_Inv_SalesReturnNoteDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_SalesReturnNoteMaster))]
        public int FK_tbl_Inv_SalesReturnNoteMaster_ID { get; set; }
        public virtual tbl_Inv_SalesReturnNoteMaster tbl_Inv_SalesReturnNoteMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_SalesNoteDetail))]
        public int FK_tbl_Inv_SalesNoteDetail_ID { get; set; }
        public virtual tbl_Inv_SalesNoteDetail tbl_Inv_SalesNoteDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        [Display(Name = "Gross Amount")]
        public double GrossAmount { get; set; }

        [Required]
        [Display(Name = "ST %")]
        public double STPercentage { get; set; }

        [Required]
        [Display(Name = "ST Amount")]
        public double STAmount { get; set; }

        [Required]
        [Display(Name = "FST %")]
        public double FSTPercentage { get; set; }

        [Required]
        [Display(Name = "FST Amount")]
        public double FSTAmount { get; set; }

        [Required]
        [Display(Name = "WHT %")]
        public double WHTPercentage { get; set; }

        [Required]
        [Display(Name = "WHT Amount")]
        public double WHTAmount { get; set; }

        [Required]
        [Display(Name = "Disc. Amount")]
        public double DiscountAmount { get; set; }

        [Required]
        [Display(Name = "Net Amount")]
        public double NetAmount { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        public bool IsProcessed { get; set; }

        [Required]
        public bool IsSupervised { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //----------------------Ac-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_SalesReturnNoteDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }


        //----------------------Item-Ledger-----------------------------//
        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_SalesReturnNoteDetail))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }
    }

    //--------------------------------Ordinary Requisition-------------------------------//
    [Table("tbl_Inv_OrdinaryRequisitionType")]
    public class tbl_Inv_OrdinaryRequisitionType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrdinaryRequisitionDetail.tbl_Inv_OrdinaryRequisitionType))]
        public virtual ICollection<tbl_Inv_OrdinaryRequisitionDetail> tbl_Inv_OrdinaryRequisitionDetails { get; set; }
    }

    [Table("tbl_Inv_OrdinaryRequisitionMaster")]
    public class tbl_Inv_OrdinaryRequisitionMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_DepartmentDetail_Section))]
        public int FK_tbl_WPT_DepartmentDetail_Section_ID { get; set; }
        public virtual tbl_WPT_DepartmentDetail_Section tbl_WPT_DepartmentDetail_Section { get; set; }

        [Required]
        public bool IsDispensedAll { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrdinaryRequisitionDetail.tbl_Inv_OrdinaryRequisitionMaster))]
        public virtual ICollection<tbl_Inv_OrdinaryRequisitionDetail> tbl_Inv_OrdinaryRequisitionDetails { get; set; }

    }

    [Table("tbl_Inv_OrdinaryRequisitionDetail")]
    public class tbl_Inv_OrdinaryRequisitionDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_OrdinaryRequisitionMaster))]
        public int FK_tbl_Inv_OrdinaryRequisitionMaster_ID { get; set; }
        public virtual tbl_Inv_OrdinaryRequisitionMaster tbl_Inv_OrdinaryRequisitionMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_OrdinaryRequisitionType))]
        public int FK_tbl_Inv_OrdinaryRequisitionType_ID { get; set; }
        public virtual tbl_Inv_OrdinaryRequisitionType tbl_Inv_OrdinaryRequisitionType { get; set; }

        [Required]
        [Display(Name = "Required OR Return")]
        public bool RequiredTrue_ReturnFalse { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        public double Quantity { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        public bool IsDispensed { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrdinaryRequisitionDispensing.tbl_Inv_OrdinaryRequisitionDetail))]
        public virtual ICollection<tbl_Inv_OrdinaryRequisitionDispensing> tbl_Inv_OrdinaryRequisitionDispensings { get; set; }
    }

    //--------------------------------Dispensing-------------------------------//
    [Table("tbl_Inv_OrdinaryRequisitionDispensing")]
    public class tbl_Inv_OrdinaryRequisitionDispensing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_OrdinaryRequisitionDetail))]
        public int FK_tbl_Inv_OrdinaryRequisitionDetail_ID { get; set; }
        public virtual tbl_Inv_OrdinaryRequisitionDetail tbl_Inv_OrdinaryRequisitionDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }
        
        [Display(Name = "Dispensing Date")]
        public DateTime? DispensingDate { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------Ledger--------------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_OrdinaryRequisitionDispensing))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_OrdinaryRequisitionDispensing))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

    }

    [Table("tbl_Inv_BMRAdditionalDispensing")]
    public class tbl_Inv_BMRAdditionalDispensing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BMRAdditionalDetail))]
        public int FK_tbl_Pro_BMRAdditionalDetail_ID { get; set; }
        public virtual tbl_Pro_BMRAdditionalDetail tbl_Pro_BMRAdditionalDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }
        
        [Display(Name = "Dispensing Date")]
        public DateTime? DispensingDate { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------Ledger--------------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_BMRAdditionalDispensing))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_BMRAdditionalDispensing))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }
    }


    [Table("tbl_Inv_BMRDispensingRaw")]
    public class tbl_Inv_BMRDispensingRaw
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_RawDetail))]
        public int FK_tbl_Pro_BatchMaterialRequisitionDetail_RawDetail_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_RawDetail tbl_Pro_BatchMaterialRequisitionDetail_RawDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Display(Name = "Dispensing Date")]
        public DateTime? DispensingDate { get; set; }

        [Display(Name = "Reservation Date")]
        public DateTime? ReservationDate { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------Ledger--------------------------------//
        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_BMRDispensingRaw))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_BMRDispensingRaw))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

    }

    [Table("tbl_Inv_BMRDispensingPackaging")]
    public class tbl_Inv_BMRDispensingPackaging
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items))]
        public int FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Display(Name = "Dispensing Date")]
        public DateTime? DispensingDate { get; set; }

        [Display(Name = "Reservation Date")]
        public DateTime? ReservationDate { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------Ledger--------------------------------//
        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_BMRDispensingPackaging))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_BMRDispensingPackaging))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }


    }

    [Table("tbl_Inv_PDRequestDispensing")]
    public class tbl_Inv_PDRequestDispensing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_PD_RequestDetailTR_CFP_Item))]
        public int FK_tbl_PD_RequestDetailTR_CFP_Item_ID { get; set; }
        public virtual tbl_PD_RequestDetailTR_CFP_Item tbl_PD_RequestDetailTR_CFP_Item { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Display(Name = "Dispensing Date")]
        public DateTime? DispensingDate { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------Ledger--------------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_PDRequestDispensing))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_PDRequestDispensing))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

    }

    //------------------------Production Transfer----------------------------//

    [Table("tbl_Inv_StockTransferMaster")]
    public class tbl_Inv_StockTransferMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster_From))]
        public int FK_tbl_Inv_WareHouseMaster_ID_From { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster_From { get; set; }
        
        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster_To))]
        public int FK_tbl_Inv_WareHouseMaster_ID_To { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster_To { get; set; }

        [Required]
        [Display(Name = "Received All")]
        public bool IsReceivedAll { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_StockTransferDetail.tbl_Inv_StockTransferMaster))]
        public virtual ICollection<tbl_Inv_StockTransferDetail> tbl_Inv_StockTransferDetails { get; set; }

    }

    [Table("tbl_Inv_StockTransferDetail")]
    public class tbl_Inv_StockTransferDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_StockTransferMaster))]
        public int FK_tbl_Inv_StockTransferMaster_ID { get; set; }
        public virtual tbl_Inv_StockTransferMaster tbl_Inv_StockTransferMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseNoteDetail_RefNo))]
        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }
        public virtual tbl_Inv_PurchaseNoteDetail tbl_Inv_PurchaseNoteDetail_RefNo { get; set; }

        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        [Display(Name = "Received")]
        public bool IsReceived { get; set; }

        [Display(Name = "Received By")]
        public string ReceivedBy { get; set; }

        [Display(Name = "Received Date")]
        public DateTime? ReceivedDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //-------------------Ledger--------------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Inv_StockTransferDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Inv_StockTransferDetail))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

    }

    /// <summary>
    /// Supply Chain 
    /// </summary>

    //---------------------------Order Note--------------------------------------//
    [Table("tbl_Inv_InternationalCommercialTerm")]
    public class tbl_Inv_InternationalCommercialTerm
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Incoterm Name")] 
        public string IncotermName { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Abbreviation")]
        public string Abbreviation { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Inv_InternationalCommercialTerm))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }
    }

    [Table("tbl_Inv_TransportType")]
    public class tbl_Inv_TransportType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Inv_TransportType))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }

    }

    [Table("tbl_Inv_OrderNoteMaster")]
    public class tbl_Inv_OrderNoteMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [Display(Name = "Target Date")]
        public DateTime TargetDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrderNoteDetail.tbl_Inv_OrderNoteMaster))]
        public virtual ICollection<tbl_Inv_OrderNoteDetail> tbl_Inv_OrderNoteDetails { get; set; }

    }

    [Table("tbl_Inv_OrderNoteDetail")]
    public class tbl_Inv_OrderNoteDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_OrderNoteMaster))]
        public int FK_tbl_Inv_OrderNoteMaster_ID { get; set; }
        public virtual tbl_Inv_OrderNoteMaster tbl_Inv_OrderNoteMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [ForeignKey(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster))]
        public int? FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_Coupling_PackagingMaster tbl_Pro_CompositionDetail_Coupling_PackagingMaster { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasPriority))]
        public int FK_AspNetOreasPriority_ID { get; set; }
        public virtual AspNetOreasPriority AspNetOreasPriority { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        public double MfgQtyLimit { get; set; }

        [Required]
        [Display(Name = "Requested Qty By Production")]
        public double RequestedQtyByProduction { get; set; }

        [Required]
        [Display(Name = "Manufacturing Qty")]
        public double ManufacturingQty { get; set; }

        [Required]
        [Display(Name = "SoldQty")]
        public double SoldQty { get; set; }

        [Required]
        [Display(Name = "Open OR Closed")]
        public bool ClosedTrue_OpenFalse { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        public double CustomMRP { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrderNoteDetail_ProductionOrder.tbl_Inv_OrderNoteDetail))]
        public virtual ICollection<tbl_Inv_OrderNoteDetail_ProductionOrder> tbl_Inv_OrderNoteDetail_ProductionOrders { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrderNoteDetail_SubDistributor.tbl_Inv_OrderNoteDetail))]
        public virtual ICollection<tbl_Inv_OrderNoteDetail_SubDistributor> tbl_Inv_OrderNoteDetail_SubDistributors { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesNoteDetail.tbl_Inv_OrderNoteDetail))]
        public virtual ICollection<tbl_Inv_SalesNoteDetail> tbl_Inv_SalesNoteDetails { get; set; }
    }

    [Table("tbl_Inv_OrderNoteDetail_ProductionOrder")]
    public class tbl_Inv_OrderNoteDetail_ProductionOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_OrderNoteDetail))]
        public int FK_tbl_Inv_OrderNoteDetail_ID { get; set; }
        public virtual tbl_Inv_OrderNoteDetail tbl_Inv_OrderNoteDetail { get; set; }

        [Required]
        public double RequestedBatchSize { get; set; }

        [Required]
        [MaxLength(25)]
        public string RequestedBatchNo { get; set; }

        [Required]
        public DateTime RequestedMfgDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }

        [Display(Name = "Requested Date")]
        public DateTime? RequestedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Processed By")]
        public string ProcessedBy { get; set; }

        [Display(Name = "Processed Date")]
        public DateTime? ProcessedDate { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.tbl_Inv_OrderNoteDetail_ProductionOrder))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters { get; set; }

    }

    [Table("tbl_Inv_OrderNoteDetail_SubDistributor")]
    public class tbl_Inv_OrderNoteDetail_SubDistributor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_OrderNoteDetail))]
        public int FK_tbl_Inv_OrderNoteDetail_ID { get; set; }
        public virtual tbl_Inv_OrderNoteDetail tbl_Inv_OrderNoteDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_CustomerSubDistributorList))]
        public int FK_tbl_Ac_CustomerSubDistributorList_ID { get; set; }
        public virtual tbl_Ac_CustomerSubDistributorList tbl_Ac_CustomerSubDistributorList { get; set; }

        [Required]
        public double Quantity { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
    }

    //---------------------------Purchase Order--------------------------------------//

    [Table("tbl_Inv_PurchaseOrderTermsConditions")]
    public class tbl_Inv_PurchaseOrderTermsConditions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "T&C Name")]
        public string TCName { get; set; }

        [Required]
        [MaxLength(2000)]
        [Display(Name = "Terms & Condition")]
        public string TermsCondition { get; set; }

        [MaxLength(500)]
        [Display(Name = "Note")]
        public string Note { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Inv_PurchaseOrderTermsConditions))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }

    }

    [Table("tbl_Inv_PurchaseOrderMaster")]
    public class tbl_Inv_PurchaseOrderMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "PO No")]
        public int? PONo { get; set; }

        [Required]
        [Display(Name = "PO Date")]
        public DateTime PODate { get; set; }

        [Required]
        [Display(Name = "Target Date")]
        public DateTime TargetDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseOrderTermsConditions))]
        public int? FK_tbl_Inv_PurchaseOrderTermsConditions_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrderTermsConditions tbl_Inv_PurchaseOrderTermsConditions { get; set; }

        //-----------------------------Import----------------------//
        [Required]
        public bool LocalTrue_ImportFalse { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseOrder_Supplier))]
        public int? FK_tbl_Inv_PurchaseOrder_Supplier_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrder_Supplier tbl_Inv_PurchaseOrder_Supplier { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseOrder_Manufacturer))]
        public int? FK_tbl_Inv_PurchaseOrder_Manufacturer_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrder_Manufacturer tbl_Inv_PurchaseOrder_Manufacturer { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseOrder_Indenter))]
        public int? FK_tbl_Inv_PurchaseOrder_Indenter_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrder_Indenter tbl_Inv_PurchaseOrder_Indenter { get; set; }

        [Display(Name = "Indent Date")]
        public DateTime? IndentDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Indent No")]
        public string IndentNo { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseOrder_ImportTerms))]
        public int? FK_tbl_Inv_PurchaseOrder_ImportTerms_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrder_ImportTerms tbl_Inv_PurchaseOrder_ImportTerms { get; set; }

        [ForeignKey(nameof(tbl_Ac_CurrencyAndCountry_Currency))]
        public int? FK_tbl_Ac_CurrencyAndCountry_ID_Currency { get; set; }
        public virtual tbl_Ac_CurrencyAndCountry tbl_Ac_CurrencyAndCountry_Currency { get; set; }

        [ForeignKey(nameof(tbl_Ac_CurrencyAndCountry_CountryOfOrigin))]
        public int? FK_tbl_Ac_CurrencyAndCountry_ID_CountryOfOrigin { get; set; }
        public virtual tbl_Ac_CurrencyAndCountry tbl_Ac_CurrencyAndCountry_CountryOfOrigin { get; set; }

        [Display(Name = "Shipment Date")]
        public DateTime? ShipmentDate { get; set; }

        [Display(Name = "Negotiation Date")]
        public DateTime? NegotiationDate { get; set; }

        [ForeignKey(nameof(tbl_Inv_TransportType))]
        public int? FK_tbl_Inv_TransportType_ID { get; set; }
        public virtual tbl_Inv_TransportType tbl_Inv_TransportType { get; set; }

        [ForeignKey(nameof(tbl_Inv_InternationalCommercialTerm))]
        public int? FK_tbl_Inv_InternationalCommercialTerm_ID { get; set; }
        public virtual tbl_Inv_InternationalCommercialTerm tbl_Inv_InternationalCommercialTerm { get; set; }

        //------------------xxxxxxxxxxx--------------------//
        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderDetail.tbl_Inv_PurchaseOrderMaster))]
        public virtual ICollection<tbl_Inv_PurchaseOrderDetail> tbl_Inv_PurchaseOrderDetails { get; set; }

    }

    [Table("tbl_Inv_PurchaseOrderDetail")]
    public class tbl_Inv_PurchaseOrderDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_PurchaseOrderMaster))]
        public int FK_tbl_Inv_PurchaseOrderMaster_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrderMaster tbl_Inv_PurchaseOrderMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasPriority))]
        public int FK_AspNetOreasPriority_ID { get; set; }
        public virtual AspNetOreasPriority AspNetOreasPriority { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        [Display(Name = "GST %")]
        public double GSTPercentage { get; set; }

        [Required]
        [Display(Name = "Discount Amount")]
        public double DiscountAmount { get; set; }

        [Required]
        [Display(Name = "WHT %")]
        public double WHTPercentage { get; set; }

        [Required]
        [Display(Name = "Net Amount")]
        public double NetAmount { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseOrder_Manufacturer))]
        public int? FK_tbl_Inv_PurchaseOrder_Manufacturer_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrder_Manufacturer tbl_Inv_PurchaseOrder_Manufacturer { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Received Qty")]
        public double ReceivedQty { get; set; }

        [Required]
        [Display(Name = "Open OR Closed")]
        public bool ClosedTrue_OpenFalse { get; set; }

        [Required]
        [Display(Name = "Performance OnTime")]
        public bool Performance_Time { get; set; }

        [Required]
        [Display(Name = "Performance Quantity")]
        public bool Performance_Quantity { get; set; }

        [Required]
        [Display(Name = "Performance Quality")]
        public bool Performance_Quality { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseRequestDetail))]
        public int? FK_tbl_Inv_PurchaseRequestDetail_ID { get; set; }
        public virtual tbl_Inv_PurchaseRequestDetail tbl_Inv_PurchaseRequestDetail { get; set; }

        //-----------------------------Import----------------------//

        [ForeignKey(nameof(tbl_Inv_MeasurementUnit))]
        public int? FK_tbl_Inv_MeasurementUnit_ID_Supplier { get; set; }
        public virtual tbl_Inv_MeasurementUnit tbl_Inv_MeasurementUnit { get; set; }

        [Display(Name = "Qty As Per Suppliers Unit")]
        public double? QuantityAsPerSupplierUnit { get; set; }

        [Display(Name = "Unit Factor To Convert In Local Unit")]
        public double? UnitFactorToConvertInLocalUnit { get; set; }

        public string Packaging { get; set; }
        public string BatchNo { get; set; }

        [Display(Name = "Mfg Date")]
        public DateTime? MfgDate { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        //-----------------------------xxxxxx----------------------//

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseNoteDetail.tbl_Inv_PurchaseOrderDetail))]
        public virtual ICollection<tbl_Inv_PurchaseNoteDetail> tbl_Inv_PurchaseNoteDetails { get; set; }

    }
    
    [Table("tbl_Inv_PurchaseOrder_ImportTerms")]
    public class tbl_Inv_PurchaseOrder_ImportTerms
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Term Name")]
        public string TermName { get; set; }

        [Required]
        [Display(Name = "At Sight")]
        public bool AtSight { get; set; }

        [Required]
        [Display(Name = "At Usance")]
        public bool AtUsance { get; set; }

        [Required]
        [Display(Name = "At Usance Days")]
        public int AtUsanceDays { get; set; }

        [Required]
        [MaxLength(2000)]
        [Display(Name = "Documents For DIL")]
        public string DocumentsForDIL { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Inv_PurchaseOrder_ImportTerms))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }
    }

    [Table("tbl_Inv_PurchaseOrder_Manufacturer")]
    public class tbl_Inv_PurchaseOrder_Manufacturer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Manufacturer Name")]
        public string ManufacturerName { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Manufacturer Address")]
        public string ManufacturerAddress { get; set; }

        [MaxLength(50)]
        public string ContactNo { get; set; }

        [MaxLength(50)]
        public string ContactPerson { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Inv_PurchaseOrder_Manufacturer))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseRequestDetail_Bids.tbl_Inv_PurchaseOrder_Manufacturer))]
        public virtual ICollection<tbl_Inv_PurchaseRequestDetail_Bids> tbl_Inv_PurchaseRequestDetail_Bidss { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderDetail.tbl_Inv_PurchaseOrder_Manufacturer))]
        public virtual ICollection<tbl_Inv_PurchaseOrderDetail> tbl_Inv_PurchaseOrderDetails { get; set; }

    }

    [Table("tbl_Inv_PurchaseOrder_Supplier")]
    public class tbl_Inv_PurchaseOrder_Supplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Supplier Address")]
        public string SupplierAddress { get; set; }

        [MaxLength(50)]
        public string ContactNo { get; set; }

        [MaxLength(50)]
        public string ContactPerson { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Inv_PurchaseOrder_Supplier))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }


    }

    [Table("tbl_Inv_PurchaseOrder_Indenter")]
    public class tbl_Inv_PurchaseOrder_Indenter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Indenter Name")]
        public string IndenterName { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Indenter Address")]
        public string IndenterAddress { get; set; }

        [MaxLength(50)]
        public string ContactNo { get; set; }

        [MaxLength(50)]
        public string ContactPerson { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderMaster.tbl_Inv_PurchaseOrder_Indenter))]
        public virtual ICollection<tbl_Inv_PurchaseOrderMaster> tbl_Inv_PurchaseOrderMasters { get; set; }
    }

    //---------------------------Purchase Order--------------------------------------//

    [Table("tbl_Inv_PurchaseRequestMaster")]
    public class tbl_Inv_PurchaseRequestMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseRequestDetail.tbl_Inv_PurchaseRequestMaster))]
        public virtual ICollection<tbl_Inv_PurchaseRequestDetail> tbl_Inv_PurchaseRequestDetails { get; set; }

    }

    [Table("tbl_Inv_PurchaseRequestDetail")]
    public class tbl_Inv_PurchaseRequestDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_PurchaseRequestMaster))]
        public int FK_tbl_Inv_PurchaseRequestMaster_ID { get; set; }
        public virtual tbl_Inv_PurchaseRequestMaster tbl_Inv_PurchaseRequestMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasPriority))]
        public int FK_AspNetOreasPriority_ID { get; set; }
        public virtual AspNetOreasPriority AspNetOreasPriority { get; set; }

        [Required]
        public double Quantity { get; set; }
      
        [MaxLength(50)]
        public string Remarks { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public bool IsRejected { get; set; }

        [Required]
        public bool IsPending { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseRequestDetail_Bids.tbl_Inv_PurchaseRequestDetail))]
        public virtual ICollection<tbl_Inv_PurchaseRequestDetail_Bids> tbl_Inv_PurchaseRequestDetail_Bidss { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderDetail.tbl_Inv_PurchaseRequestDetail))]
        public virtual ICollection<tbl_Inv_PurchaseOrderDetail> tbl_Inv_PurchaseOrderDetails { get; set; }
    }

    [Table("tbl_Inv_PurchaseRequestDetail_Bids")]
    public class tbl_Inv_PurchaseRequestDetail_Bids
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_PurchaseRequestDetail))]
        public int FK_tbl_Inv_PurchaseRequestDetail_ID { get; set; }
        public virtual tbl_Inv_PurchaseRequestDetail tbl_Inv_PurchaseRequestDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Ac_ChartOfAccounts))]
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public virtual tbl_Ac_ChartOfAccounts tbl_Ac_ChartOfAccounts { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        [Display(Name = "GST %")]
        public double GSTPercentage { get; set; }

        [Required]
        [Display(Name = "Target Date")]
        public DateTime TargetDate { get; set; }

        [ForeignKey(nameof(tbl_Inv_PurchaseOrder_Manufacturer))]
        public int? FK_tbl_Inv_PurchaseOrder_Manufacturer_ID { get; set; }
        public virtual tbl_Inv_PurchaseOrder_Manufacturer tbl_Inv_PurchaseOrder_Manufacturer { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        public bool? Approved { get; set; }

        [MaxLength(50)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

    }

}
