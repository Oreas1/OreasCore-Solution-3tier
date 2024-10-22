using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OreasModel
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Discriminator { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasAuthorizationScheme))]
        public int FK_AspNetOreasAuthorizationScheme_ID { get; set; }
        public virtual AspNetOreasAuthorizationScheme AspNetOreasAuthorizationScheme { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MyID { get; set; }

        [ForeignKey(nameof(tbl_WPT_Employee))]
        public int? FK_tbl_WPT_Employee_ID { get; set; }
        public virtual tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        [Required]
        public bool PurchaseRequestApprover { get; set; }

        [Required]
        public bool AcVoucherApprover { get; set; }

        [MaxLength(500)]
        public string EmailSignature { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {

    }

    public class ApplicationUserRole : IdentityUserRole<string>
    { 
    
    }

    [Table("AspNetOreasGeneralSettings")]
    public class AspNetOreasGeneralSettings
    {
        [Key]
        public int ID { get; set; }                

        [Required]
        public bool OrderNoteRateAutoFromCRL { get; set; }
        [Required]
        public bool SalesNoteDetailRateAutoInsertFromCRL { get; set; }

        [Required]
        public bool AcBankVoucherAutoSupervised { get; set; }

        [Required]
        public bool AcCashVoucherAutoSupervised { get; set; }

        [Required]
        public bool AcJournalVoucherAutoSupervised { get; set; }

        [Required]
        public bool AcPurchaseNoteInvoiceAutoSupervised { get; set; }

        [Required]
        public bool AcPurchaseReturnNoteInvoiceAutoSupervised { get; set; }

        [Required]
        public bool AcSalesNoteInvoiceAutoSupervised { get; set; }

        [Required]
        public bool AcSalesReturnNoteInvoiceAutoSupervised { get; set; }

        [Required]
        public bool PurchaseOrderLocalAutoSupervised { get; set; }

        [Required]
        public bool PurchaseOrderImportAutoSupervised { get; set; }

        [Required]
        public bool CanChangePurchaseNoteReferenceNoByQC { get; set; }

        [Required]
        public string LetterHead_PaperSize { get; set; }

        [Required]
        public int LetterHead_HeaderHeight { get; set; }

        [Required]
        public int LetterHead_FooterHeight { get; set; }

    }

    [Table("AspNetOreasCompanyProfile")]
    public class AspNetOreasCompanyProfile
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string LicenseBy { get; set; }

        [Required]
        [MaxLength(20)]
        public string LicenseByCellNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string LicenseByAddress { get; set; }

        [Required]
        public byte[] LicenseByLogo { get; set; }

        [Required]
        [MaxLength(50)]
        public string LicenseToID { get; set; }

        [Required]
        [MaxLength(50)]
        public string LicenseTo { get; set; }

        [Required]
        [MaxLength(50)]
        public string LicenseToContactNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string LicenseToAddress { get; set; }


        public byte[] LicenseToLogo { get; set; }

        [EmailAddress]
        [MaxLength(50)]
        public string LicenseToEmail { get; set; }

        [MaxLength(50)]
        public string LicenseToEmailPswd { get; set; }

        public int? LicenseToEmailPortNo { get; set; }

        [MaxLength(50)]
        public string LicenseToEmailHostName { get; set; }

        //[AllowHtml]
        [MaxLength(1000)]
        public string LicenseToEmailFooter { get; set; }

        [MaxLength(50)]
        public string LicenseToNTN { get; set; }

        [MaxLength(50)]
        public string LicenseToSTN { get; set; }



    }

    [Table("AspNetOreasArea")]
    public class AspNetOreasArea
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Area { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [InverseProperty(nameof(AspNetOreasArea_Form.AspNetOreasArea))]
        public virtual ICollection<AspNetOreasArea_Form> AspNetOreasArea_Forms { get; set; }

        [InverseProperty(nameof(AspNetOreasAuthorizationScheme_Area.AspNetOreasArea))]
        public virtual ICollection<AspNetOreasAuthorizationScheme_Area> AspNetOreasAuthorizationScheme_Areas { get; set; }

    }

    [Table("AspNetOreasArea_Form")]
    public class AspNetOreasArea_Form
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasArea))]

        public int FK_AspNetOreasArea_ID { get; set; }
        public virtual AspNetOreasArea AspNetOreasArea { get; set; }

        [Required]
        [Display(Name = "Form Name")]
        public string FormName { get; set; }

        [InverseProperty(nameof(AspNetOreasAuthorizationScheme_Area_Form.AspNetOreasArea_Form))]
        public virtual ICollection<AspNetOreasAuthorizationScheme_Area_Form> AspNetOreasAuthorizationScheme_Area_Forms { get; set; }


    }

    [Table("AspNetOreasAuthorizationScheme")]
    public class AspNetOreasAuthorizationScheme
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsCredentialsDashBoard { get; set; }

        [Required]
        public bool IsWPTDashBoard { get; set; }

        [Required]
        public bool IsManagementDashBoard { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(AspNetOreasAuthorizationScheme_Section.AspNetOreasAuthorizationScheme))]
        public virtual ICollection<AspNetOreasAuthorizationScheme_Section> AspNetOreasAuthorizationScheme_Sections { get; set; }

        [InverseProperty(nameof(AspNetOreasAuthorizationScheme_WareHouse.AspNetOreasAuthorizationScheme))]
        public virtual ICollection<AspNetOreasAuthorizationScheme_WareHouse> AspNetOreasAuthorizationScheme_WareHouses { get; set; }

        [InverseProperty(nameof(AspNetOreasAuthorizationScheme_Area.AspNetOreasAuthorizationScheme))]
        public virtual ICollection<AspNetOreasAuthorizationScheme_Area> AspNetOreasAuthorizationScheme_Areas { get; set; }

        [InverseProperty(nameof(ApplicationUser.AspNetOreasAuthorizationScheme))]
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }

    [Table("AspNetOreasAuthorizationScheme_Section")]
    public class AspNetOreasAuthorizationScheme_Section
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasAuthorizationScheme))]
        public int FK_AspNetOreasAuthorizationScheme_ID { get; set; }
        public virtual AspNetOreasAuthorizationScheme AspNetOreasAuthorizationScheme { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_WPT_DepartmentDetail_Section))]
        public int FK_tbl_WPT_DepartmentDetail_Section_ID { get; set; }
        public virtual tbl_WPT_DepartmentDetail_Section tbl_WPT_DepartmentDetail_Section { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("AspNetOreasAuthorizationScheme_WareHouse")]
    public class AspNetOreasAuthorizationScheme_WareHouse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasAuthorizationScheme))]
        public int FK_AspNetOreasAuthorizationScheme_ID { get; set; }
        public virtual AspNetOreasAuthorizationScheme AspNetOreasAuthorizationScheme { get; set; }

        [Required]
        [ForeignKey(nameof(tbl_Inv_WareHouseMaster))]
        public int FK_tbl_Inv_WareHouseMaster_ID { get; set; }
        public virtual tbl_Inv_WareHouseMaster tbl_Inv_WareHouseMaster { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }

    [Table("AspNetOreasAuthorizationScheme_Area")]
    public class AspNetOreasAuthorizationScheme_Area
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasAuthorizationScheme))]
        public int FK_AspNetOreasAuthorizationScheme_ID { get; set; }
        public virtual AspNetOreasAuthorizationScheme AspNetOreasAuthorizationScheme { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasArea))]
        public int FK_AspNetOreasArea_ID { get; set; }
        public virtual AspNetOreasArea AspNetOreasArea { get; set; }

        [Required]
        [Display(Name = "Can View")]
        public bool CanView { get; set; }

        [Required]
        [Display(Name = "Can Add")]
        public bool CanAdd { get; set; }

        [Required]
        [Display(Name = "Can Edit")]
        public bool CanEdit { get; set; }

        [Required]
        [Display(Name = "Can Delete")]
        public bool CanDelete { get; set; }

        [Required]
        [Display(Name = "Can View Report")]
        public bool CanViewReport { get; set; }

        [Required]
        [Display(Name = "Can View Only Own Data")]
        public bool CanViewOnlyOwnData { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [InverseProperty(nameof(AspNetOreasAuthorizationScheme_Area_Form.AspNetOreasAuthorizationScheme_Area))]
        public virtual ICollection<AspNetOreasAuthorizationScheme_Area_Form> AspNetOreasAuthorizationScheme_Area_Forms { get; set; }

    }

    [Table("AspNetOreasAuthorizationScheme_Area_Form")]
    public class AspNetOreasAuthorizationScheme_Area_Form
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasAuthorizationScheme_Area))]
        public int FK_AspNetOreasAuthorizationScheme_Area_ID { get; set; }
        public virtual AspNetOreasAuthorizationScheme_Area AspNetOreasAuthorizationScheme_Area { get; set; }

        [Required]
        [ForeignKey(nameof(AspNetOreasArea_Form))]

        public int FK_AspNetOreasArea_Form_ID { get; set; }
        public virtual AspNetOreasArea_Form AspNetOreasArea_Form { get; set; }


        [Required]
        [Display(Name = "Can View")]
        public bool CanView { get; set; }

        [Required]
        [Display(Name = "Can Add")]
        public bool CanAdd { get; set; }

        [Required]
        [Display(Name = "Can Edit")]
        public bool CanEdit { get; set; }

        [Required]
        [Display(Name = "Can Delete")]
        public bool CanDelete { get; set; }

        [Required]
        [Display(Name = "Can View Report")]
        public bool CanViewReport { get; set; }

        [Required]
        [Display(Name = "Can View Only Own Data")]
        public bool CanViewOnlyOwnData { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }

    [Table("AspNetOreasPriority")]
    public class AspNetOreasPriority
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public byte PriorityLevel { get; set; }

        [InverseProperty(nameof(tbl_Inv_OrderNoteDetail.AspNetOreasPriority))]
        public virtual ICollection<tbl_Inv_OrderNoteDetail> tbl_Inv_OrderNoteDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseOrderDetail.AspNetOreasPriority))]
        public virtual ICollection<tbl_Inv_PurchaseOrderDetail> tbl_Inv_PurchaseOrderDetails { get; set; }

        [InverseProperty(nameof(tbl_Inv_PurchaseRequestDetail.AspNetOreasPriority))]
        public virtual ICollection<tbl_Inv_PurchaseRequestDetail> tbl_Inv_PurchaseRequestDetails { get; set; }
    }


}
