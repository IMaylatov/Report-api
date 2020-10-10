namespace SofTrust.Report.Core.Generator
{
    using Newtonsoft.Json.Linq;
    using System.IO;

    public interface IReportGenerator
    {
        Stream Generate(JToken report, Stream template);
    }
}
