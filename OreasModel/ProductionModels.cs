using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace OreasModel
{
    //-------------------Procedure--------------------------//
    [Table("tbl_Pro_Procedure")]
    public class tbl_Pro_Procedure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Procedure Name")]
        public string ProcedureName { get; set; }

        [Required]
        [Display(Name = "For Raw1_Packaging0")]
        public bool ForRaw1_Packaging0 { get; set; }

        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_QCSample))]
        public int? FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_QCSample { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Pro_ProcessDetail.tbl_Pro_Procedure))]
        public virtual ICollection<tbl_Pro_ProcessDetail> tbl_Pro_ProcessDetails { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.tbl_Pro_Procedure))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR> tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.tbl_Pro_Procedure))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs { get; set; }

        [InverseProperty(nameof(tbl_PD_RequestDetailTR_Procedure.tbl_Pro_Procedure))]
        public virtual ICollection<tbl_PD_RequestDetailTR_Procedure> tbl_PD_RequestDetailTR_Procedures { get; set; }

    }

    //-------------------Process--------------------------//
    [Table("tbl_Pro_ProcessMaster")]
    public class tbl_Pro_ProcessMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Process Name")]
        public string ProcessName { get; set; }

        [Required]
        [Display(Name = "For Raw1_Packaging0")]
        public bool ForRaw1_Packaging0 { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Pro_ProcessDetail.tbl_Pro_ProcessMaster))]
        public virtual ICollection<tbl_Pro_ProcessDetail> tbl_Pro_ProcessDetails { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionMaster.tbl_Pro_ProcessMaster))]
        public virtual ICollection<tbl_Pro_CompositionMaster> tbl_Pro_CompositionMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Pro_ProcessMaster))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingMaster> tbl_Pro_CompositionDetail_Coupling_PackagingMasters { get; set; }

    }

    [Table("tbl_Pro_ProcessDetail")]
    public class tbl_Pro_ProcessDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_ProcessMaster))]
        public int FK_tbl_Pro_ProcessMaster_ID { get; set; }
        public virtual tbl_Pro_ProcessMaster tbl_Pro_ProcessMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_Procedure))]
        public int FK_tbl_Pro_Procedure_ID { get; set; }
        public virtual tbl_Pro_Procedure tbl_Pro_Procedure { get; set; }

        [Required]
        [Display(Name = "QA Clearance Req before Start")]
        public bool IsQAClearanceBeforeStart { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    //-------------------Composition--------------------------//
    [Table("tbl_Pro_CompositionFilterPolicyMaster")]
    public class tbl_Pro_CompositionFilterPolicyMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductType_Coupling))]
        public int FK_tbl_Inv_ProductType_ID_For_Coupling { get; set; }
        public virtual tbl_Inv_ProductType tbl_Inv_ProductType_Coupling { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster_By))]
        public int FK_tbl_Inv_WareHouseMaster_ID_By { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster_By { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductType_QCSample))]
        public int FK_tbl_Inv_ProductType_ID_QCSample { get; set; }
        public virtual tbl_Inv_ProductType tbl_Inv_ProductType_QCSample { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Qc_ActionType))]
        public int FK_tbl_Qc_ActionType_ID_BMRProcessTestingSample { get; set; }
        public virtual tbl_Qc_ActionType tbl_Qc_ActionType { get; set; }

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


        [InverseProperty(nameof(tbl_Pro_CompositionFilterPolicyDetail.tbl_Pro_CompositionFilterPolicyMaster))]
        public virtual ICollection<tbl_Pro_CompositionFilterPolicyDetail> tbl_Pro_CompositionFilterPolicyDetails { get; set; }
    }

    [Table("tbl_Pro_CompositionFilterPolicyDetail")]
    public class tbl_Pro_CompositionFilterPolicyDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionFilterPolicyMaster))]
        public int FK_tbl_Pro_CompositionFilterPolicyMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionFilterPolicyMaster tbl_Pro_CompositionFilterPolicyMaster { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Filter Name")]
        public string FilterName { get; set; }

        [Required]
        [Display(Name = "For Raw1_Packaging0")]
        public bool ForRaw1_Packaging0 { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

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

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_RawMaster.tbl_Pro_CompositionFilterPolicyDetail))]
        public virtual ICollection<tbl_Pro_CompositionDetail_RawMaster> tbl_Pro_CompositionDetail_RawMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingDetail.tbl_Pro_CompositionFilterPolicyDetail))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingDetail> tbl_Pro_CompositionDetail_Coupling_PackagingDetails { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.tbl_Pro_CompositionFilterPolicyDetail))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_RawMaster> tbl_Pro_BatchMaterialRequisitionDetail_RawMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_CompositionFilterPolicyDetail))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail> tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails { get; set; }

        [InverseProperty(nameof(tbl_PD_RequestDetailTR_CFP.tbl_Pro_CompositionFilterPolicyDetail))]
        public virtual ICollection<tbl_PD_RequestDetailTR_CFP> tbl_PD_RequestDetailTR_CFPs { get; set; }

    }


    //------------------------Composition----------------------------//

    [Table("tbl_Pro_CompositionMaster")]
    public class tbl_Pro_CompositionMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        [DataType(DataType.Date)]
        public DateTime DocDate { get; set; }

        [Required]
        [MaxLength(250)]
        public string CompositionName { get; set; }

        [Required]
        public int ShelfLifeInMonths { get; set; }

        [Required]
        [Display(Name = "Dimension Value")]
        public double DimensionValue { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_MeasurementUnit))]
        public int FK_tbl_Inv_MeasurementUnit_ID_Dimension { get; set; }
        public virtual tbl_Inv_MeasurementUnit tbl_Inv_MeasurementUnit { get; set; }

        [Display(Name = "Revision No")]
        public int? RevisionNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? RevisionDate { get; set; }

        [ForeignKey(nameof(tbl_Pro_ProcessMaster))]
        public int? FK_tbl_Pro_ProcessMaster_ID { get; set; }
        public virtual tbl_Pro_ProcessMaster tbl_Pro_ProcessMaster { get; set; }

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

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_RawMaster.tbl_Pro_CompositionMaster))]
        public virtual ICollection<tbl_Pro_CompositionDetail_RawMaster> tbl_Pro_CompositionDetail_RawMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling.tbl_Pro_CompositionMaster))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling> tbl_Pro_CompositionDetail_Couplings { get; set; }

    }

    [Table("tbl_Pro_CompositionDetail_RawMaster")]
    public class tbl_Pro_CompositionDetail_RawMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionMaster))]
        public int FK_tbl_Pro_CompositionMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionMaster tbl_Pro_CompositionMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionFilterPolicyDetail))]
        public int FK_tbl_Pro_CompositionFilterPolicyDetail_ID { get; set; }
        public virtual tbl_Pro_CompositionFilterPolicyDetail tbl_Pro_CompositionFilterPolicyDetail { get; set; }

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

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_RawDetail_Items.tbl_Pro_CompositionDetail_RawMaster))]
        public virtual ICollection<tbl_Pro_CompositionDetail_RawDetail_Items> tbl_Pro_CompositionDetail_RawDetail_Itemss { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.tbl_Pro_CompositionDetail_RawMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_RawMaster> tbl_Pro_BatchMaterialRequisitionDetail_RawMasters { get; set; }

    }

    [Table("tbl_Pro_CompositionDetail_RawDetail_Items")]
    public class tbl_Pro_CompositionDetail_RawDetail_Items
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionDetail_RawMaster))]
        public int FK_tbl_Pro_CompositionDetail_RawMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_RawMaster tbl_Pro_CompositionDetail_RawMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        public double Quantity { get; set; }

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

        [Required]
        [Display(Name = "Custome Rate")]
        public double CustomeRate { get; set; }

        [Required]
        [Display(Name = "Percentage On Rate")]
        public double PercentageOnRate { get; set; }

    }

    [Table("tbl_Pro_CompositionDetail_Coupling")]
    public class tbl_Pro_CompositionDetail_Coupling
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionMaster))]
        public int FK_tbl_Pro_CompositionMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionMaster tbl_Pro_CompositionMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [Display(Name = "Batch Size")]
        public double BatchSize { get; set; }

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

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster.tbl_Pro_CompositionDetail_Coupling))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingMaster> tbl_Pro_CompositionDetail_Coupling_PackagingMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionMaster.tbl_Pro_CompositionDetail_Coupling))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionMaster> tbl_Pro_BatchMaterialRequisitionMasters { get; set; }


    }

    [Table("tbl_Pro_CompositionDetail_Coupling_PackagingMaster")]
    public class tbl_Pro_CompositionDetail_Coupling_PackagingMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionDetail_Coupling))]
        public int FK_tbl_Pro_CompositionDetail_Coupling_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_Coupling tbl_Pro_CompositionDetail_Coupling { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_Primary))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID_Primary { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_Primary { get; set; }

        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_Secondary))]
        public int? FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_Secondary { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Packaging Name")]
        public string PackagingName { get; set; }

        [Required]
        public bool IsDiscontinue { get; set; }

        [ForeignKey(nameof(tbl_Pro_ProcessMaster))]
        public int? FK_tbl_Pro_ProcessMaster_ID { get; set; }
        public virtual tbl_Pro_ProcessMaster tbl_Pro_ProcessMaster { get; set; }

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

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingDetail.tbl_Pro_CompositionDetail_Coupling_PackagingMaster))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingDetail> tbl_Pro_CompositionDetail_Coupling_PackagingDetails { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.tbl_Pro_CompositionDetail_Coupling_PackagingMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrderNoteDetail.tbl_Pro_CompositionDetail_Coupling_PackagingMaster))]
        public virtual ICollection<tbl_Inv_OrderNoteDetail> tbl_Inv_OrderNoteDetails { get; set; }
    }

    [Table("tbl_Pro_CompositionDetail_Coupling_PackagingDetail")]
    public class tbl_Pro_CompositionDetail_Coupling_PackagingDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster))]
        public int FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_Coupling_PackagingMaster tbl_Pro_CompositionDetail_Coupling_PackagingMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionFilterPolicyDetail))]
        public int FK_tbl_Pro_CompositionFilterPolicyDetail_ID { get; set; }
        public virtual tbl_Pro_CompositionFilterPolicyDetail tbl_Pro_CompositionFilterPolicyDetail { get; set; }

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

        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items.tbl_Pro_CompositionDetail_Coupling_PackagingDetail))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items> tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Itemss { get; set; }


        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_CompositionDetail_Coupling_PackagingDetail))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail> tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails { get; set; }

    }

    [Table("tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items")]
    public class tbl_Pro_CompositionDetail_Coupling_PackagingDetail_Items
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingDetail))]
        public int FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_Coupling_PackagingDetail tbl_Pro_CompositionDetail_Coupling_PackagingDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

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

        [Required]
        [Display(Name = "Custome Rate")]
        public double CustomeRate { get; set; }

        [Required]
        [Display(Name = "Percentage On Rate")]
        public double PercentageOnRate { get; set; }

    }

    //------------------------BMR----------------------------//
    [Table("tbl_Pro_BatchMaterialRequisitionMaster")]
    public class tbl_Pro_BatchMaterialRequisitionMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Batch No")]
        public string BatchNo { get; set; }

        [Required]
        [Display(Name = "Batch Mfg Date")]
        public DateTime BatchMfgDate { get; set; }

        [Required]
        [Display(Name = "Batch Expiry Date")]
        public DateTime BatchExpiryDate { get; set; }

        [Required]
        [Display(Name = "Dimension Value")]
        public double DimensionValue { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_MeasurementUnit))]
        public int FK_tbl_Inv_MeasurementUnit_ID_Dimension { get; set; }
        public virtual tbl_Inv_MeasurementUnit tbl_Inv_MeasurementUnit { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [Display(Name = "Batch Size")]
        public double BatchSize { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionDetail_Coupling))]
        public int FK_tbl_Pro_CompositionDetail_Coupling_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_Coupling tbl_Pro_CompositionDetail_Coupling { get; set; }

        [Required]
        [Display(Name = "Total Production")]
        public double TotalProd { get; set; }        
        public bool? IsCompleted { get; set; }

        [Required]
        public double Cost { get; set; }

        [Display(Name = "Finished Date")]
        public DateTime? FinishedDate { get; set; }

        [Required]
        public bool IsDispensedR { get; set; }

        [Required]
        public bool IsDispensedP { get; set; }

        [Required]
        public bool IsQAClearanceBMRPending { get; set; }

        [Required]
        public bool IsQAClearanceBPRPending { get; set; }

        [Required]
        public bool IsQCSampleBMRPending { get; set; }

        [Required]
        public bool IsQCSampleBPRPending { get; set; }

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

        //---------------------------Ledger--------------------------------//
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Pro_BatchMaterialRequisitionMaster))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        //---------------------------others--------------------------------//
        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR.tbl_Pro_BatchMaterialRequisitionMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR> tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMRs { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_RawMaster.tbl_Pro_BatchMaterialRequisitionMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_RawMaster> tbl_Pro_BatchMaterialRequisitionDetail_RawMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster.tbl_Pro_BatchMaterialRequisitionMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMasters { get; set; }

        [InverseProperty(nameof(tbl_Pro_BMRAdditionalMaster.tbl_Pro_BatchMaterialRequisitionMaster))]
        public virtual ICollection<tbl_Pro_BMRAdditionalMaster> tbl_Pro_BMRAdditionalMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Pro_BatchMaterialRequisitionMaster))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

    }

    [Table("tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR")]
    public class tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionMaster))]
        public int FK_tbl_Pro_BatchMaterialRequisitionMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_Procedure))]
        public int FK_tbl_Pro_Procedure_ID { get; set; }
        public virtual tbl_Pro_Procedure tbl_Pro_Procedure { get; set; }

        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_QCSample))]
        public int? FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_QCSample { get; set; }

        [Required]
        [Display(Name = "QA Clearance Req before Start")]
        public bool IsQAClearanceBeforeStart { get; set; }

        [Display(Name = "QA Cleared")]
        public bool? QACleared { get; set; }

        [Display(Name = "QA Cleared By")]
        public string QAClearedBy { get; set; }

        [Display(Name = "QA Cleared Date")]
        public DateTime? QAClearedDate { get; set; }

        [Required]
        public double Yield { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBMR.tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR))]
        public virtual ICollection<tbl_Qc_SampleProcessBMR> tbl_Qc_SampleProcessBMRs { get; set; }


    }

    [Table("tbl_Pro_BatchMaterialRequisitionDetail_RawMaster")]
    public class tbl_Pro_BatchMaterialRequisitionDetail_RawMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionMaster))]
        public int FK_tbl_Pro_BatchMaterialRequisitionMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionFilterPolicyDetail))]
        public int FK_tbl_Pro_CompositionFilterPolicyDetail_ID { get; set; }
        public virtual tbl_Pro_CompositionFilterPolicyDetail tbl_Pro_CompositionFilterPolicyDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

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

        [ForeignKey(nameof(tbl_Pro_CompositionDetail_RawMaster))]
        public int? FK_tbl_Pro_CompositionDetail_RawMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_RawMaster tbl_Pro_CompositionDetail_RawMaster { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_RawDetail.tbl_Pro_BatchMaterialRequisitionDetail_RawMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_RawDetail> tbl_Pro_BatchMaterialRequisitionDetail_RawDetails { get; set; }

    }

    [Table("tbl_Pro_BatchMaterialRequisitionDetail_RawDetail")]
    public class tbl_Pro_BatchMaterialRequisitionDetail_RawDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_RawMaster))]
        public int FK_tbl_Pro_BatchMaterialRequisitionDetail_RawMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_RawMaster tbl_Pro_BatchMaterialRequisitionDetail_RawMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        public double Quantity { get; set; }

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

        [InverseProperty(nameof(tbl_Inv_BMRDispensingRaw.tbl_Pro_BatchMaterialRequisitionDetail_RawDetail))]
        public virtual ICollection<tbl_Inv_BMRDispensingRaw> tbl_Inv_BMRDispensingRaws { get; set; }


    }

    [Table("tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster")]
    public class tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionMaster))]
        public int FK_tbl_Pro_BatchMaterialRequisitionMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Packaging Name")]
        public string PackagingName { get; set; }

        [Required]
        [Display(Name = "Batch Size")]
        public double BatchSize { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_Primary))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID_Primary { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_Primary { get; set; }

        [Required]
        [Display(Name = "Cost Primary")]
        public double Cost_Primary { get; set; }

        [Required]
        [Display(Name = "Production Primary")]
        public double TotalProd_Primary { get; set; }

        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_Secondary))]
        public int? FK_tbl_Inv_ProductRegistrationDetail_ID_Secondary { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_Secondary { get; set; }

        [Required]
        [Display(Name = "Cost Secondary")]
        public double Cost_Secondary { get; set; }

        [Required]
        [Display(Name = "Production Secondary")]
        public double TotalProd_Secondary { get; set; }

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

        [ForeignKey(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster))]
        public int? FK_tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_Coupling_PackagingMaster tbl_Pro_CompositionDetail_Coupling_PackagingMaster { get; set; }

        [ForeignKey(nameof(tbl_Inv_OrderNoteDetail_ProductionOrder))]
        public int? FK_tbl_Inv_OrderNoteDetail_ProductionOrder_ID { get; set; }
        public virtual tbl_Inv_OrderNoteDetail_ProductionOrder tbl_Inv_OrderNoteDetail_ProductionOrder { get; set; }

        //----------------------------------------------------------------------------------------//

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR> tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPRs { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail> tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesNoteDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_SalesNoteDetail> tbl_Inv_SalesNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_SalesReturnNoteDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_SalesReturnNoteDetail> tbl_Inv_SalesReturnNoteDetails { get; set; }

        //------------------------dispensing------------------------------//
        [InverseProperty(nameof(tbl_Inv_OrdinaryRequisitionDispensing.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_OrdinaryRequisitionDispensing> tbl_Inv_OrdinaryRequisitionDispensings { get; set; }

        [InverseProperty(nameof(tbl_Inv_BMRAdditionalDispensing.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_BMRAdditionalDispensing> tbl_Inv_BMRAdditionalDispensings { get; set; }

        [InverseProperty(nameof(tbl_Inv_BMRDispensingRaw.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_BMRDispensingRaw> tbl_Inv_BMRDispensingRaws { get; set; }

        [InverseProperty(nameof(tbl_Inv_BMRDispensingPackaging.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_BMRDispensingPackaging> tbl_Inv_BMRDispensingPackagings { get; set; }

        [InverseProperty(nameof(tbl_Inv_PDRequestDispensing.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_PDRequestDispensing> tbl_Inv_PDRequestDispensings { get; set; }


        //------------------------Item-Ledger------------------------------//
        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers_RefNo { get; set; }

        //------------------------Production Transfer------------------------------//
        [InverseProperty(nameof(tbl_Pro_ProductionTransferDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Pro_ProductionTransferDetail> tbl_Pro_ProductionTransferDetails_RefNo { get; set; }

        //----------------------stock transfer-----------------------------//
        [InverseProperty(nameof(tbl_Inv_StockTransferDetail.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public virtual ICollection<tbl_Inv_StockTransferDetail> tbl_Inv_StockTransferDetails { get; set; }


    }

    [Table("tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR")]
    public class tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster))]
        public int FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_Procedure))]
        public int FK_tbl_Pro_Procedure_ID { get; set; }
        public virtual tbl_Pro_Procedure tbl_Pro_Procedure { get; set; }

        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_QCSample))]
        public int? FK_tbl_Inv_ProductRegistrationDetail_ID_QCSample { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_QCSample { get; set; }

        [Required]
        [Display(Name = "QA Clearance Req before Start")]
        public bool IsQAClearanceBeforeStart { get; set; }

        [Display(Name = "QA Cleared")]
        public bool? QACleared { get; set; }

        [Display(Name = "QA Cleared By")]
        public string QAClearedBy { get; set; }

        [Display(Name = "QA Cleared Date")]
        public DateTime? QAClearedDate { get; set; }

        [Required]
        public double Yield { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [Display(Name = "Completed Date")]
        public DateTime? CompletedDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBPR.tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR))]
        public virtual ICollection<tbl_Qc_SampleProcessBPR> tbl_Qc_SampleProcessBPRs { get; set; }


    }

    [Table("tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail")]
    public class tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster))]
        public int FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionFilterPolicyDetail))]
        public int FK_tbl_Pro_CompositionFilterPolicyDetail_ID { get; set; }
        public virtual tbl_Pro_CompositionFilterPolicyDetail tbl_Pro_CompositionFilterPolicyDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

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

        [ForeignKey(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingDetail))]
        public int? FK_tbl_Pro_CompositionDetail_Coupling_PackagingDetail_ID { get; set; }
        public virtual tbl_Pro_CompositionDetail_Coupling_PackagingDetail tbl_Pro_CompositionDetail_Coupling_PackagingDetail { get; set; }

        [InverseProperty(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail))]
        public virtual ICollection<tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items> tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Itemss { get; set; }

    }

    [Table("tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items")]
    public class tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail))]
        public int FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        public double Quantity { get; set; }

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

        [InverseProperty(nameof(tbl_Inv_BMRDispensingPackaging.tbl_Pro_BatchMaterialRequisitionDetail_PackagingDetail_Items))]
        public virtual ICollection<tbl_Inv_BMRDispensingPackaging> tbl_Inv_BMRDispensingPackagings { get; set; }

    }

    //-------------------------BMR Additional-----------------------------//
    [Table("tbl_Pro_BMRAdditionalType")]
    public class tbl_Pro_BMRAdditionalType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; }

        [InverseProperty(nameof(tbl_Pro_BMRAdditionalDetail.tbl_Pro_BMRAdditionalType))]
        public virtual ICollection<tbl_Pro_BMRAdditionalDetail> tbl_Pro_BMRAdditionalDetails { get; set; }

    }

    [Table("tbl_Pro_BMRAdditionalMaster")]
    public class tbl_Pro_BMRAdditionalMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionMaster))]
        public int FK_tbl_Pro_BatchMaterialRequisitionMaster_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionMaster tbl_Pro_BatchMaterialRequisitionMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int? FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

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

        [InverseProperty(nameof(tbl_Pro_BMRAdditionalDetail.tbl_Pro_BMRAdditionalMaster))]
        public virtual ICollection<tbl_Pro_BMRAdditionalDetail> tbl_Pro_BMRAdditionalDetails { get; set; }


    }

    [Table("tbl_Pro_BMRAdditionalDetail")]
    public class tbl_Pro_BMRAdditionalDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BMRAdditionalMaster))]
        public int FK_tbl_Pro_BMRAdditionalMaster_ID { get; set; }
        public virtual tbl_Pro_BMRAdditionalMaster tbl_Pro_BMRAdditionalMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BMRAdditionalType))]
        public int FK_tbl_Pro_BMRAdditionalType_ID { get; set; }
        public virtual tbl_Pro_BMRAdditionalType tbl_Pro_BMRAdditionalType { get; set; }

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

        [InverseProperty(nameof(tbl_Inv_BMRAdditionalDispensing.tbl_Pro_BMRAdditionalDetail))]
        public virtual ICollection<tbl_Inv_BMRAdditionalDispensing> tbl_Inv_BMRAdditionalDispensings { get; set; }

    }

    //------------------------Production Transfer----------------------------//

    [Table("tbl_Pro_ProductionTransferMaster")]
    public class tbl_Pro_ProductionTransferMaster
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

        [Display(Name = "QA Cleared All")]
        public bool QAClearedAll { get; set; }

        [Display(Name = "Received All")]
        public bool ReceivedAll { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Pro_ProductionTransferDetail.tbl_Pro_ProductionTransferMaster))]
        public virtual ICollection<tbl_Pro_ProductionTransferDetail> tbl_Pro_ProductionTransferDetails { get; set; }

    }

    [Table("tbl_Pro_ProductionTransferDetail")]
    public class tbl_Pro_ProductionTransferDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_ProductionTransferMaster))]
        public int FK_tbl_Pro_ProductionTransferMaster_ID { get; set; }
        public virtual tbl_Pro_ProductionTransferMaster tbl_Pro_ProductionTransferMaster { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo))]
        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_RefNo { get; set; }

        [Required]
        public double Quantity { get; set; }     

        [MaxLength(50)]
        public string Remarks { get; set; }

        [Display(Name = "QA Cleared")]
        public bool? QACleared { get; set; }

        [Display(Name = "QA Cleared By")]
        public string QAClearedBy { get; set; }

        [Display(Name = "QA Cleared Date")]
        public DateTime? QAClearedDate { get; set; }

        [Display(Name = "Received")]
        public bool Received { get; set; }

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
        [InverseProperty(nameof(tbl_Ac_Ledger.tbl_Pro_ProductionTransferDetail))]
        public virtual ICollection<tbl_Ac_Ledger> tbl_Ac_Ledgers { get; set; }

        [InverseProperty(nameof(tbl_Inv_Ledger.tbl_Pro_ProductionTransferDetail))]
        public virtual ICollection<tbl_Inv_Ledger> tbl_Inv_Ledgers { get; set; }

    }



}
