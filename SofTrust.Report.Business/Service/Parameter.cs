namespace SofTrust.Report.Service
{
    using Newtonsoft.Json.Linq;

    public class Parameter
    {
        public string Name { get; set; }
        public JToken Value { get; set; }
    }
}
