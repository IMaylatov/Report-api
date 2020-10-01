﻿namespace SofTrust.Report.Business.Model
{
    public class Template
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public int ReportId { get; set; }

        public Report Report { get; set; }
    }
}
