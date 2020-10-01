namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business;
    using SofTrust.Report.Business.Service.Report;
    using System.IO;
    using System.Threading.Tasks;

    [Route("api/reportRun")]
    [ApiController]
    public class ReportRunController : ControllerBase
    {
        private readonly ReportContext context;
        private readonly ReportGeneratorFactory reportGeneratorFactory;

        public ReportRunController(
            ReportGeneratorFactory reportGeneratorFactory,
            ReportContext reportContext)
        {
            this.reportGeneratorFactory = reportGeneratorFactory;
            this.context = reportContext;
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
                return reportGenerator.Generate(report, templateStream);
            }
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Run(int id,
            [FromForm(Name = "report")] string reportJson)
        {
            var reportJ = JToken.Parse(reportJson);

            var report = await this.context.Reports
                .Include(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == id);
            var templateStream = new MemoryStream(report.Template.Data);

            var reportGenerator = this.reportGeneratorFactory.Create(reportJ["type"].ToString());
            return reportGenerator.Generate(reportJ, templateStream);
        }
    }
}
