namespace SofTrust.Report.Core.Generator
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System.IO;

    public interface IReportGenerator
    {
        FileStreamResult Generate(JToken report, Stream template);
    }
}
