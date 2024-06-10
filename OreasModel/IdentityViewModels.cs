using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OreasModel
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string returnUrl { get; set; }
    }
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        [Required]
        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        [Required]
        [Range(minimum:0,maximum:10)]
        public int AccessFailedCount { get; set; }

        [Required]
        public int FK_AspNetOreasAuthorizationScheme_ID { get; set; }

        [Required]
        public int MyID { get; set; }


        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

  
        public int? FK_tbl_WPT_Employee_ID { get; set; }

        public bool PurchaseRequestApprover { get; set; }
        public bool AcVoucherApprover { get; set; }

        public string EmailSignature { get; set; }
    }

}
