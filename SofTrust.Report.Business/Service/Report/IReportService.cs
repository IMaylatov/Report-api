namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;

    public interface IReportService
    {
        FileStreamResult Run(JToken report, IFormFile template);
    }
}
