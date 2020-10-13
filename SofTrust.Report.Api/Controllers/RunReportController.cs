﻿namespace SofTrust.Report.Api.Controllers
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
    using System.Text.Json;
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
            [FromForm(Name = "variables")] string variableJson)
        {
            var report = JToken.Parse(reportJson);
            var variables = JToken.Parse(variableJson);

            var reportGenerator = this.reportGeneratorFactory.Create(report["type"].ToString());
            using (var templateStream = template.OpenReadStream())
            {
                var reportStream = reportGenerator.Generate(report, templateStream, variables);
                return new FileStreamResult(reportStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Run(int id,
            [FromForm(Name = "variables")] string variableJson)
        {
            var variables = JToken.Parse(variableJson);

            var report = await this.context.Reports
                .Include(x => x.ReportDataSources).ThenInclude(x => x.DataSource)
                .Include(x => x.ReportDataSets).ThenInclude(x => x.DataSet)
                .Include(x => x.ReportVariables).ThenInclude(x => x.Variable)
                .Include(x => x.Templates)
                .FirstOrDefaultAsync(x => x.Id == id);
            var reportDto = report.AdaptToDto();
            var serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var reportJ = JToken.FromObject(reportDto, serializer);
            var templateStream = new MemoryStream(report.Templates.FirstOrDefault().Data);

            var reportGenerator = this.reportGeneratorFactory.Create(report.Type);
            var reportStream = reportGenerator.Generate(reportJ, templateStream, variables);
            return new FileStreamResult(reportStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
        }
    }
}