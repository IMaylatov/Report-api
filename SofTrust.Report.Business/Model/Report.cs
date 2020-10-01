namespace SofTrust.Report.Business.Model
{
    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }  
        public int TemplateId { get; set; }

        public ReportType Type { get; set; }
        public Template ReportTemplate { get; set; }
    }
}
