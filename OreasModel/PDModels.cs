using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OreasModel
{

    [Table("tbl_PD_RequestMaster")]
    public class tbl_PD_RequestMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail_Primary))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID_Primary { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail_Primary { get; set; }

        [Required]
        public int SampleProductExpiryMonths { get; set; }

        [Required]
        public double SampleProductMRP { get; set; }

        public string SampleProductPhoto { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

        public bool? FinalStatus { get; set; }

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

        [InverseProperty(nameof(tbl_PD_RequestDetailTR.tbl_PD_RequestMaster))]
        public virtual ICollection<tbl_PD_RequestDetailTR> tbl_PD_RequestDetailTRs { get; set; }

    }

    [Table("tbl_PD_RequestDetailTR")]
    public class tbl_PD_RequestDetailTR
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required]
        [ForeignKey(nameof(tbl_PD_RequestMaster))]
        public int FK_tbl_PD_RequestMaster_ID { get; set; }
        public virtual tbl_PD_RequestMaster tbl_PD_RequestMaster { get; set; }

        [Display(Name = "Doc No")]
        public int? DocNo { get; set; }

        [Required]
        [Display(Name = "Doc Date")]
        public DateTime DocDate { get; set; }

        [Required]
        [Display(Name = "MfgDate")]
        public DateTime MfgDate { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Trial Batch No")]
        public string TrialBatchNo { get; set; }

        [Display(Name = "Batch Size")]
        public double TrialBatchSizeInSemiUnits { get; set; }

        public bool? TrialStatus { get; set; }

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

        [InverseProperty(nameof(tbl_PD_RequestDetailTR_Procedure.tbl_PD_RequestDetailTR))]
        public virtual ICollection<tbl_PD_RequestDetailTR_Procedure> tbl_PD_RequestDetailTR_Procedures { get; set; }

        [InverseProperty(nameof(tbl_PD_RequestDetailTR_CFP.tbl_PD_RequestDetailTR))]
        public virtual ICollection<tbl_PD_RequestDetailTR_CFP> tbl_PD_RequestDetailTR_CFPs { get; set; }

    }

    [Table("tbl_PD_RequestDetailTR_Procedure")]
    public class tbl_PD_RequestDetailTR_Procedure
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_PD_RequestDetailTR))]
        public int FK_tbl_PD_RequestDetailTR_ID { get; set; }
        public virtual tbl_PD_RequestDetailTR tbl_PD_RequestDetailTR { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_Procedure))]
        public int FK_tbl_Pro_Procedure_ID { get; set; }
        public virtual tbl_Pro_Procedure tbl_Pro_Procedure { get; set; }

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

    [Table("tbl_PD_RequestDetailTR_CFP")]
    public class tbl_PD_RequestDetailTR_CFP
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_PD_RequestDetailTR))]
        public int FK_tbl_PD_RequestDetailTR_ID { get; set; }
        public virtual tbl_PD_RequestDetailTR tbl_PD_RequestDetailTR { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Pro_CompositionFilterPolicyDetail))]
        public int FK_tbl_Pro_CompositionFilterPolicyDetail_ID { get; set; }
        public virtual tbl_Pro_CompositionFilterPolicyDetail tbl_Pro_CompositionFilterPolicyDetail { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

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

        [InverseProperty(nameof(tbl_PD_RequestDetailTR_CFP_Item.tbl_PD_RequestDetailTR_CFP))]
        public virtual ICollection<tbl_PD_RequestDetailTR_CFP_Item> tbl_PD_RequestDetailTR_CFP_Items { get; set; }

    }

    [Table("tbl_PD_RequestDetailTR_CFP_Item")]
    public class tbl_PD_RequestDetailTR_CFP_Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_PD_RequestDetailTR_CFP))]
        public int FK_tbl_PD_RequestDetailTR_CFP_ID { get; set; }
        public virtual tbl_PD_RequestDetailTR_CFP tbl_PD_RequestDetailTR_CFP { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_ProductRegistrationDetail))]
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }
        public virtual tbl_Inv_ProductRegistrationDetail tbl_Inv_ProductRegistrationDetail { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        [Display(Name = "Required OR Return")]
        public bool RequiredTrue_ReturnFalse { get; set; }

        [MaxLength(50)]
        public string Remarks { get; set; }

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

        [InverseProperty(nameof(tbl_Inv_PDRequestDispensing.tbl_PD_RequestDetailTR_CFP_Item))]
        public virtual ICollection<tbl_Inv_PDRequestDispensing> tbl_Inv_PDRequestDispensings { get; set; }


    }

}
