﻿namespace SofTrust.Report.Core.Models.Domain
{
    public class ReportDataSet
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }

        public int DataSetId { get; set; }
        public DataSet DataSet { get; set; }
    }
}