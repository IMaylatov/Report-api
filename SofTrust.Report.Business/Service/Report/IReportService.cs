namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;
    using System.IO;

    public interface IReportService
    {
        Stream Run(string templatetype, IFormFile templateFile, JArray parameters, JArray dataSources, JArray dataSets);
    }
}
