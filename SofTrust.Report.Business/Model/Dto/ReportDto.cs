namespace SofTrust.Report.Business.Model.Dto
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public int? TemplateId { get; set; }

        public ReportType Type { get; set; }
    }
}
