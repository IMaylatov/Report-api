namespace SofTrust.Report.Business.Model
{
    using System.Collections.Generic;

    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }  

        public ReportType Type { get; set; }
        public ICollection<Template> Templates { get; set; }
    }
}
