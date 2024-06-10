using System;
using System.Collections.Generic;
using System.Text;

namespace OreasModel
{
    public enum EnumReportType
    {
        Periodic = 0, Serial = 1, OnlyID = 2, NonPeriodicNonSerial = 3, TillDate = 4
    }
    public class ReportCallingModel
    {
        public EnumReportType ReportType { get; set; }

        public string ReportName { get; set; }

        public List<string> SeekBy { get; set; }

        public List<string> GroupBy { get; set; }

        public List<string> OrderBy { get; set; }

    }
}
