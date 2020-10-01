namespace SofTrust.Report.Business.Model.Domain
{
    using System.Collections.Generic;

    public class Variable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Kind { get; set; }

        public List<ReportVariable> ReportVariables { get; set; }
    }
}
