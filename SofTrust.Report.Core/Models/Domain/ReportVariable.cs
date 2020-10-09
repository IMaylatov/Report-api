﻿namespace SofTrust.Report.Core.Models.Domain
{
    public class ReportVariable
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }

        public int VariableId { get; set; }
        public Variable Variable { get; set; }
    }
}