namespace SofTrust.Report.Business.Model
{
    using Newtonsoft.Json.Linq;

    public class Parameter
    {
        public string Name { get; set; }
        public JToken Value { get; set; }
    }
}
