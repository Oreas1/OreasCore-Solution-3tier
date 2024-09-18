using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OreasModel
{
    [Table("tbl_Qa_DocumentControl")]
    public class tbl_Qa_DocumentControl
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "DocumentNo")]
        public string DocumentNo { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Required]
        [Display(Name = "Issued Date")]
        public DateTime IssuedDate { get; set; }

        [Required]
        [Display(Name = "Revision No")]
        public int RevisionNo { get; set; }
        
        [MaxLength(50)]
        [Display(Name = "Report Codes")]
        public string ReportCodes { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }
}
