namespace SofTrust.Report.Core.Generator
{
    using Newtonsoft.Json.Linq;

    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public JToken Data { get; set; }
        public JToken Value { get; set; }
    }
}
