namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System.IO;

    public interface IReportGenerator
    {
        FileStreamResult Generate(JToken report, Stream template);
    }
}
