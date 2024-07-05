using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OreasModel
{
    public class TreeView_Department
    {
        public string sign { get; set; }

        public decimal ID { get; set; }

        public string DepartmentName { get; set; }

        public decimal ParentID { get; set; }

        public int ChildCount { get; set; }

        public string spacing { get; set; }
    }
    public class TreeView_ChartofAccounts
    {
        public string sign { get; set; }
        public decimal ID { get; set; }
        public string AccountName { get; set; }
        public decimal ParentID { get; set; }
        public int ChildCount { get; set; }
        public string spacing { get; set; }
        public bool IsParent { get; set; }

    }

    public class TreeView_ProductUnit
    {
        public string sign { get; set; }
        public decimal ID { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public double Split_Into { get; set; }
        public decimal ParentID { get; set; }
        public int ChildCount { get; set; }
        public string spacing { get; set; }
        public bool IsParent { get; set; }

    }

    [Keyless]
    public class UDTVF_Inv_GetLastPOWithBestSupplier
    {
        public int PONo { get; set; }
        public int NoDaysAgo { get; set; }
        public string AccountName { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double GSTPercentage { get; set; }
        public string BestSupplier { get; set; }
        public int BestPerformance { get; set; }

    }

    [Keyless]
    public class VM_Inv_WHMSearchModal
    {
        public int ID { get; set; }

        public string WareHouseName { get; set; }

        public string Prefix { get; set; }

    }

    [Keyless]
    public class VM_WPT_SectionSearchModal
    {
        public int ID { get; set; }
        public string SectionName { get; set; }
        public string OtherDetail { get; set; }

    }

    [Keyless]
    public class VM_WPT_EmployeeSearchModal
    {
        public int? ID { get; set; }

        public string EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        public string ATEnrollmentNo_Default { get; set; }

        public string Department { get; set; }

        public string Designation { get; set; }

        public string LevelName { get; set; }

        public string photo { get; set; }

        public string OtherInfo { get; set; }

    }

    [Keyless]
    public class VM_WPT_LeavePolicySearchModal
    {
        public int? ID { get; set; }

        public string PolicyName { get; set; }

        public string PolicyPrefix { get; set; }

        public double MonthBalance { get; set; }

        public double AnnualBalance { get; set; }

        public string LeaveType { get; set; }
    }

    [Keyless]
    public class VM_Ac_COASearchModal
    {
        public int? ID { get; set; }

        public string AccountCode { get; set; }

        public string AccountName { get; set; }

    }

    [Keyless]
    [NotMapped]
    public class VM_Inv_ProductSearchModal
    {
        public int ID { get; set; }

        public int? MasterProdID { get; set; }

        public string ProductName { get; set; }

        public string CategoryName { get; set; }

        public string MeasurementUnit { get; set; }

        public string Description { get; set; }

        public bool IsDecimal { get; set; }

        public double Split_Into { get; set; }

        public string OtherDetail { get; set; }

    }

    [Keyless]
    [NotMapped]
    public class VM_Inv_ReferenceSearchModal
    {
        public string ReferenceNo { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal? Balance { get; set; }

        public int? FK_tbl_Inv_PurchaseNoteDetail_ID_ReferenceNo { get; set; }

        public int? FK_tbl_Pro_BatchMaterialRequisitionDetail_PackagingMaster_ReferenceNo { get; set; }

        public string OtherDetail { get; set; }

    }


    [Keyless]
    [NotMapped]
    public class VM_Inv_OrderNoteSearchModal
    {
        public int ID { get; set; }
        public int DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime TargetDate { get; set; }
        public string ProductName { get; set; }
        public string MeasurementUnit { get; set; }
        public double Quantity { get; set; }
        public double SoldQty { get; set; }

    }

    public class VM_EmailAddress
    {
        public int Index { get; set; }

        public string EmailAddress { get; set; }

        public string EmailType { get; set; }

    }

    public class VM_Email
    {
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public bool WithFooter { get; set; }
        public List<VM_EmailAddress> VM_EmailAddresses { get; set; }

    }

    public static class FiscalYear
    {
        public static DateTime? Start = null;
        public static DateTime? End = null;

        public static void Set(DateTime? s, DateTime? e) 
        {
            Start = s;
            End = e;
        }
    }

    public static class Rpt_Shared
    {

       // public static string defaultconnectionstring;

        public static string LicenseBy = "Oreas";

        public static string LicenseByCellNo = "+92-345-2965345";

        public static string LicenseByAddress = "Karachi, Pakistan";

        public static byte[] LicenseByLogo = System.Convert.FromBase64String(@"iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAYAAAD0eNT6AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAADaGlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS4zLWMwMTEgNjYuMTQ1NjYxLCAyMDEyLzAyLzA2LTE0OjU2OjI3ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSJ4bXAuZGlkOjAxODAxMTc0MDcyMDY4MTE4MjJBODBCNjdENjAyRUIwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjY1NkE2OUExRDdCRDExRTM5ODc5RDA5MEFDQjFGMDkzIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjY1NkE2OUEwRDdCRDExRTM5ODc5RDA5MEFDQjFGMDkzIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKE1hY2ludG9zaCkiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpGQzdGMTE3NDA3MjA2ODExODIyQUJEQ0ZFMDU2Q0VGQSIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRp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");

        public static string LicenseToID = "Unknown";

        public static string LicenseTo = "Unknown Company";

        public static string LicenseToContactNo = "Unknown";

        public static string LicenseToAddress = "Unknown";

        public static byte[] LicenseToLogo = System.Convert.FromBase64String(@"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");

        public static string LetterHead_PaperSize = "Letter";
        public static int LetterHead_HeaderHeight = 100;
        public static int LetterHead_FooterHeight = 70;

        //----------------Email Settings-------------------//
        public static string LicenseToEmail = "";
        public static string LicenseToEmailPswd = "";
        public static int LicenseToEmailPortNo = 0;
        public static string LicenseToEmailHostName = "";
        public static string LicenseToEmailFooter = "";

        //----------------NTN STN-------------------//
        public static string LicenseToNTN = "";
        public static string LicenseToSTN = "";

        public static void Set(AspNetOreasCompanyProfile aspNetCompanyProfile) {
            LicenseBy = aspNetCompanyProfile.LicenseBy;
            LicenseByCellNo = aspNetCompanyProfile.LicenseByCellNo;
            LicenseByAddress = aspNetCompanyProfile.LicenseByAddress;

            if(aspNetCompanyProfile.LicenseByLogo != null)
                LicenseByLogo = aspNetCompanyProfile.LicenseByLogo;

            LicenseToID = aspNetCompanyProfile.LicenseToID;
            LicenseTo = aspNetCompanyProfile.LicenseTo;
            LicenseToContactNo = aspNetCompanyProfile.LicenseToContactNo;
            LicenseToAddress = aspNetCompanyProfile.LicenseToAddress;

            if (aspNetCompanyProfile.LicenseToLogo != null)
                LicenseToLogo = aspNetCompanyProfile.LicenseToLogo;
            
            LicenseToEmail = aspNetCompanyProfile.LicenseToEmail;
            LicenseToEmailPswd = aspNetCompanyProfile.LicenseToEmailPswd;
            LicenseToEmailPortNo = aspNetCompanyProfile.LicenseToEmailPortNo ?? 0;
            LicenseToEmailHostName = aspNetCompanyProfile.LicenseToEmailHostName;
            LicenseToEmailFooter = aspNetCompanyProfile.LicenseToEmailFooter;

            LicenseToNTN = aspNetCompanyProfile.LicenseToNTN;
            LicenseToSTN = aspNetCompanyProfile.LicenseToSTN;
        }
    }
}
