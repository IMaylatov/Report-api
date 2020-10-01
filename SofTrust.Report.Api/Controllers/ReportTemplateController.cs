namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SofTrust.Report.Business;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.IO;
    using SofTrust.Report.Business.Model.Dto;
    using Mapster;
    using System.Linq.Dynamic.Core;
    using System.Linq;

    [Route("api/reports/{reportId}/template")]
    [ApiController]
    public class ReportTemplateController : ControllerBase
    {
        private readonly ReportContext context;

        public ReportTemplateController(ReportContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<TemplateDto>> GetReportTemplate(int reportId)
        {
            var template = await this.context.Templates
                .FirstOrDefaultAsync(x => x.ReportId == reportId);

            return this.Ok(template.Adapt<TemplateDto>());
        }

        [HttpPost]
        public async Task<ActionResult<TemplateDto>> CreateReportTemplate(
            int reportId,
            [FromForm(Name = "template")] IFormFile data)
        {
            if (this.context.Templates.Any(x => x.ReportId == reportId))
            {
                return this.BadRequest();
            }

            var template = new Business.Model.Domain.Template { ReportId = reportId, Data = GetBytesFromFile(data) };
            this.context.Templates.Add(template);

            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportTemplate), new { reportId }, template.Adapt<TemplateDto>());
        }

        [HttpPut]
        public async Task<ActionResult<TemplateDto>> Put(
            int reportId,
            [FromForm(Name = "template")] IFormFile data)
        {
            var template = await this.context.Templates
                .FirstOrDefaultAsync(x => x.ReportId == reportId);

            template.Data = GetBytesFromFile(data);

            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportTemplate), new { reportId }, template.Adapt<TemplateDto>());
        }


        [HttpGet("data")]
        public async Task<IActionResult> Get(int reportId)
        {
            var report = await this.context.Reports
                .Include(x => x.Templates)
                .FirstOrDefaultAsync(x => x.Id == reportId);

            var templateStream = new MemoryStream(report.Templates.FirstOrDefault().Data);

            switch (report.Type)
            {
                case "ClosedXml":
                    return new FileStreamResult(templateStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
                case "Malibu":
                    return new FileStreamResult(templateStream, "application/octet-stream") { FileDownloadName = $"report.mlbrpt" };
            }

            return BadRequest();
        }

        private byte[] GetBytesFromFile(IFormFile data)
        {
            byte[] dataBytes;
            using (var templateStream = data.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                templateStream.CopyTo(memoryStream);
                dataBytes = memoryStream.ToArray();
            }

            return dataBytes;
        }
    }
}
