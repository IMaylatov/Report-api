namespace SofTrust.Report.Business.Model.Dto
{
    public class DataSourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public dynamic Data { get; set; }
    }
}
