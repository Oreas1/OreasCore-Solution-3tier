using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OreasModel
{
    public class VM_EmployeeEnrollment
    {
        public tbl_WPT_Employee tbl_WPT_Employee { get; set; }

        public tbl_WPT_EmployeeSalaryStructure tbl_WPT_EmployeeSalaryStructure { get; set; }

    }
    public class PayRunDetail_Emp_EmailDetail
    {
        public int tbl_WPT_PayRunDetail_Emp_ID { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string MonthYear { get; set; }
    }

    [Keyless]
    public class GetATOutComeOfEmployee
    {
        public int? RNo { get; set; }
        public DateTime? InstanceDate { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public bool? P { get; set; }
        public bool? A { get; set; }
        public bool? AHD { get; set; }
        public bool? AP { get; set; }
        public bool? HD { get; set; }
        public bool? LI { get; set; }
        public bool? EO { get; set; }
        public bool? HS { get; set; }
        public bool? HSP { get; set; }
        public int? OT { get; set; }
        public bool? OTD { get; set; }
        public int? SM { get; set; }
        public int? BSM { get; set; }
        public int? ASM { get; set; }
        public int? ShiftID { get; set; }
    }

    [Keyless]
    public class USP_WPT_DashboardTeamATSummary
    {
        public string DepartmentName { get; set; }
        public int TotalEmp { get; set; }
        public int TotalP { get; set; }
        public int TotalLI { get; set; }
    }

}
