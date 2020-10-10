namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Core.Generator.Report;
    using SofTrust.Report.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/run/report")]
    [ApiController]
    public class RunReportController : ControllerBase
    {
        private readonly ReportContext context;
        private readonly ReportGeneratorFactory reportGeneratorFactory;

        public RunReportController(
            ReportGeneratorFactory reportGeneratorFactory,
            ReportContext context)
        {
            this.reportGeneratorFactory = reportGeneratorFactory;
            this.context = context;
        }

        [HttpPost]
        public IActionResult Run(
            [FromForm(Name = "report")] string reportJson,
            [FromForm(Name = "template")] IFormFile template)
        {
            var report = JToken.Parse(reportJson);

            var reportGenerator = this.reportGeneratorFactory.Create(report["type"].ToString());
            using (var templateStream = template.OpenReadStream())
            {
                var reportStream = reportGenerator.Generate(report, templateStream);
                return new FileStreamResult(reportStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Run(int id,
            [FromForm(Name = "report")] string reportJson)
        {
            var reportJ = JToken.Parse(reportJson);

            var report = await this.context.Reports
                .Include(x => x.Templates)
                .FirstOrDefaultAsync(x => x.Id == id);
            var templateStream = new MemoryStream(report.Templates.FirstOrDefault().Data);

            var reportGenerator = this.reportGeneratorFactory.Create(report.Type);
            var reportStream = reportGenerator.Generate(reportJ, templateStream);
            return new FileStreamResult(reportStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
        }
    }
}