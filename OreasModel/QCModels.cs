using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace OreasModel
{

    [Table("tbl_Qc_ActionType")]
    public class tbl_Qc_ActionType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Action Name")]
        public string ActionName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Action For")]
        public string ActionFor { get; set; }

        [InverseProperty(nameof(tbl_Inv_ProductType.tbl_Qc_ActionType))]
        public virtual ICollection<tbl_Inv_ProductType> tbl_Inv_ProductTypes { get; set; }

        [InverseProperty(nameof(tbl_Pro_CompositionFilterPolicyMaster.tbl_Qc_ActionType))]
        public virtual ICollection<tbl_Pro_CompositionFilterPolicyMaster> tbl_Pro_CompositionFilterPolicyMasters { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseNoteDetail.tbl_Qc_ActionType))]
        public virtual ICollection<tbl_Inv_PurchaseNoteDetail> tbl_Inv_PurchaseNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBMR.tbl_Qc_ActionType))]
        public virtual ICollection<tbl_Qc_SampleProcessBMR> tbl_Qc_SampleProcessBMRs { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBPR.tbl_Qc_ActionType))]
        public virtual ICollection<tbl_Qc_SampleProcessBPR> tbl_Qc_SampleProcessBPRs { get; set; }

    }

    [Table("tbl_Qc_SampleProcessBMR")]
    public class tbl_Qc_SampleProcessBMR
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR))]
        public int FK_tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR tbl_Pro_BatchMaterialRequisitionMaster_ProcessBMR { get; set; }

        [Required]
        public DateTime SampleDate { get; set; }

        [Required]
        public double SampleQty { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Qc_ActionType))]
        public int FK_tbl_Qc_ActionType_ID { get; set; }
        public virtual tbl_Qc_ActionType tbl_Qc_ActionType { get; set; }

        [MaxLength(50)]
        public string ActionBy { get; set; }

        public DateTime? ActionDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBMR_QcTest.tbl_Qc_SampleProcessBMR))]
        public virtual ICollection<tbl_Qc_SampleProcessBMR_QcTest> tbl_Qc_SampleProcessBMR_QcTests { get; set; }

    }

    [Table("tbl_Qc_SampleProcessBPR")]
    public class tbl_Qc_SampleProcessBPR
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR))]
        public int FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR_ID { get; set; }
        public virtual tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ProcessBPR { get; set; }

        [Required]
        public DateTime SampleDate { get; set; }

        [Required]
        public double SampleQty { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Qc_ActionType))]
        public int FK_tbl_Qc_ActionType_ID { get; set; }
        public virtual tbl_Qc_ActionType tbl_Qc_ActionType { get; set; }

        [MaxLength(50)]
        public string ActionBy { get; set; }

        public DateTime? ActionDate { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBPR_QcTest.tbl_Qc_SampleProcessBPR))]
        public virtual ICollection<tbl_Qc_SampleProcessBPR_QcTest> tbl_Qc_SampleProcessBPR_QcTests { get; set; }

    }

    [Table("tbl_Qc_SampleProcessBMR_QcTest")]
    public class tbl_Qc_SampleProcessBMR_QcTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Qc_SampleProcessBMR))]
        public int FK_tbl_Qc_SampleProcessBMR_ID { get; set; }
        public virtual tbl_Qc_SampleProcessBMR tbl_Qc_SampleProcessBMR { get; set; }

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

        [Display(Name = "Result Value")]
        public double? ResultValue { get; set; }

        [MaxLength(100)]
        [Display(Name = "Result Remarks")]
        public string ResultRemarks { get; set; }

        [Required]
        public bool IsPrintOnCOA { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_Qc_SampleProcessBPR_QcTest")]
    public class tbl_Qc_SampleProcessBPR_QcTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Qc_SampleProcessBPR))]
        public int FK_tbl_Qc_SampleProcessBPR_ID { get; set; }
        public virtual tbl_Qc_SampleProcessBPR tbl_Qc_SampleProcessBPR { get; set; }

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

        [Display(Name = "Result Value")]
        public double? ResultValue { get; set; }

        [MaxLength(100)]
        [Display(Name = "Result Remarks")]
        public string ResultRemarks { get; set; }

        [Required]
        public bool IsPrintOnCOA { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("tbl_Qc_Lab")]
    public class tbl_Qc_Lab
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Lab Name")]
        public string LabName { get; set; }

        [Required]
        [MaxLength(5)]
        public string Prefix { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(tbl_Qc_Test.tbl_Qc_Lab))]
        public virtual ICollection<tbl_Qc_Test> tbl_Qc_Tests { get; set; }
    }

    [Table("tbl_Qc_Test")]
    public class tbl_Qc_Test
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Test Name")]
        public string TestName { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Qc_Lab))]
        public int FK_tbl_Qc_Lab_ID { get; set; }
        public virtual tbl_Qc_Lab tbl_Qc_Lab { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }


        [InverseProperty(nameof(tbl_Pro_CompositionMaster_ProcessBMR_QcTest.tbl_Qc_Test))]
        public virtual ICollection<tbl_Pro_CompositionMaster_ProcessBMR_QcTest> tbl_Pro_CompositionMaster_ProcessBMR_QcTests { get; set; }
        
        [InverseProperty(nameof(tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest.tbl_Qc_Test))]
        public virtual ICollection<tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTest> tbl_Pro_CompositionDetail_Coupling_PackagingMaster_ProcessBPR_QcTests { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBMR_QcTest.tbl_Qc_Test))]
        public virtual ICollection<tbl_Qc_SampleProcessBMR_QcTest> tbl_Qc_SampleProcessBMR_QcTests { get; set; }

        [InverseProperty(nameof(tbl_Qc_SampleProcessBPR_QcTest.tbl_Qc_Test))]
        public virtual ICollection<tbl_Qc_SampleProcessBPR_QcTest> tbl_Qc_SampleProcessBPR_QcTests { get; set; }

    }

}
