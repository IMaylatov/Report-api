namespace SofTrust.Report.Core.Models.Dto
{
    public class VariableDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public dynamic Data { get; set; }
    }
}
