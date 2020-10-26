namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
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
            [FromForm(Name = "template")] IFormFile template,
            [FromForm(Name = "context")] string contextJson)
        {
            var reportToken = JToken.Parse(reportJson);
            using (var templateStream = template.OpenReadStream())
            {
                var reportContext = JToken.Parse(contextJson);

                return Run(reportToken, templateStream, reportContext);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Run(int id,
            [FromForm(Name = "context")] string contextJson)
        {
            var report = await this.context.Reports
                .Include(x => x.DataSources)
                .Include(x => x.DataSets)
                .Include(x => x.Variables)
                .Include(x => x.Templates)
                .FirstOrDefaultAsync(x => x.Id == id);
            var reportDto = report.AdaptToDto();
            var reportToken = JToken.FromObject(reportDto, 
                new Newtonsoft.Json.JsonSerializer() { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            var templateStream = new MemoryStream(report.Templates.FirstOrDefault().Data);
            var reportContext = JToken.Parse(contextJson);

            return Run(reportToken, templateStream, reportContext);
        }

        private IActionResult Run(JToken reportToken, Stream templateStream, JToken reportContext)
        {
            var reportGenerator = this.reportGeneratorFactory.Create(reportToken["type"].ToString());

            var reportStream = reportGenerator.Generate(reportToken, templateStream, reportContext);
            return new FileStreamResult(reportStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
        }
    }
}