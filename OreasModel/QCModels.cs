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


    }

}
