using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OreasModel
{
    public static class MyGlobalInfo
    {
        //--------------------when calendar is closing------------------------//
        public static string CalendarYearClosingID { get; set; }

        //--------------------when emailing on payrun------------------------//
        public static string PayrunEmailSalarySlips { get; set; }

    }
    public class VM_CalendarYear_Months_Adjustment
    {
        public int ID_M1 { get; set; }

        public int FK_tbl_WPT_CalendarYear_ID_M1 { get; set; }

        public DateTime MonthStart_M1 { get; set; }

        public DateTime MonthEnd_M1 { get; set; }

        public bool IsClosed_M1 { get; set; }

        public int ID_M2 { get; set; }

        public int FK_tbl_WPT_CalendarYear_ID_M2 { get; set; }

        public DateTime MonthStart_M2 { get; set; }

        public DateTime MonthEnd_M2 { get; set; }

        public bool IsClosed_M2 { get; set; }
    }
    public class VM_PurchaseOrderAgainstRequest
    {
        public int PurchaseRequestDetailID { get; set; }
        public int FK_tbl_Ac_ChartOfAccounts_ID { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double GSTPercentage { get; set; }
        public DateTime? TargetDate { get; set; }
        public int? MergingPOID { get; set; }
        public int FK_AspNetOreasPriority_ID { get; set; }
        public int FK_tbl_Inv_ProductRegistrationDetail_ID { get; set; }

    }

    //------------------------Excel data upload structure---------------//
    public class RewardExcelData
    {
        public string ATNo { get; set; }
        public double RewardAmount { get; set; }
        public bool withSalary { get; set; }

    }
    public class LoanExcelData
    {
        public string ATNo { get; set; }
        public double Amount { get; set; }
        public double Rate { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime ReceivingDate { get; set; }

    }
    public class COAExcelData
    {
        public int ParentID { get; set; }
        public string AccountName { get; set; }
        public int AcTypeID { get; set; }
        public int? WHTID { get; set; }
        public int? WHTSalesID { get; set; }
        public int? PayTermID { get; set;}
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string NTN { get; set; }
        public string STR { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ContactPersonName { get; set; }


    }
    public class JournalDocExcelData
    {
        public int MasterID { get; set; }
        public string AcCode { get; set; }
        public DateTime PostingDate { get; set; }
        public string Narration { get; set; }
        public double Amount { get; set; }   

    }
    public class ProdRegExcelData
    {
        public int ClassificationID { get; set; }
        public string ProductName { get; set; }
        //---------Root Unit------------//
        public int CategoryID { get; set; }
        public int UnitID { get; set; }
        public double Split_Into { get; set; }
        public double ReorderLevel { get; set; }
        public string ProductCode { get; set; }
        //---------Root Chlid------------//
        public int CategoryID2 { get; set; }
        public int UnitID2 { get; set; }
        public double Split_Into2 { get; set; }
        public double ReorderLevel2 { get; set; }
        public string ProductCode2 { get; set; }


    }
    public class PurchaseNoteExcelData
    {
        public string ProductCode { get; set; }
        public int? PONo { get; set; }
        public double Quantity { get; set; }
        public string MfgBatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public double Rate { get; set; }
        public double GSTPercentage { get; set; }
        public double WHTPercentage { get; set; }

    }
    public class IncrementExcelData
    {
        public string ATNo { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public double IncAmount { get; set; }
        public int IncByCode { get; set; }
        public double Arrear { get; set; }
        public int? ArrearMonthCode { get; set; }
    }
    public class EmployeeExcelData 
    {
        public int EmploymentTypeID { get; set; }
        public string EmployeeName { get; set; }
        public string MachineID { get; set; }
        public string Gender { get; set; }
        public int SectionID { get; set; }
        public int DesignationID { get; set; }
        public int EmployeeLevelID { get; set; }
        public int EducationID { get; set; }
        public DateTime JoiningDate { get; set; }
        public int ShiftID { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string MaritalStatus { get; set; }
        public string CellNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public string BloodGroup { get; set; }
        public double BasicSalary { get; set; }
        public int OTPolicyID { get; set; }
        public int TransactionModeID { get; set; }

    }

    //--------------------------xxxxxxxxxxxxxxxx---------------------//
}
