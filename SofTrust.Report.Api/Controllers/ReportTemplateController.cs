namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.IO;
    using Mapster;
    using System.Linq.Dynamic.Core;
    using System.Linq;
    using System.Collections.Generic;
    using SofTrust.Report.Infrastructure;
    using SofTrust.Report.Core.Models.Dto;
    using SofTrust.Report.Core.Models.Domain;

    [Route("api/reports/{reportId}/templates")]
    [ApiController]
    public class ReportTemplateController : ControllerBase
    {
        private readonly ReportContext context;

        public ReportTemplateController(ReportContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemplateDto>>> GetReportTemplates(int reportId)
        {
            var templates = await this.context.Templates
                .Where(x => x.ReportId == reportId)
                .ToListAsync();

            return this.Ok(templates.Adapt<IEnumerable<TemplateDto>>());
        }

        [HttpGet("{templateId}")]
        public async Task<ActionResult<TemplateDto>> GetReportTemplateById(int reportId, int templateId)
        {
            var template = await this.context.Templates
                .FirstOrDefaultAsync(x => x.Id == templateId);

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

            var template = new Template { ReportId = reportId, Data = GetBytesFromFile(data) };
            this.context.Templates.Add(template);

            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportTemplates), new { reportId }, template.Adapt<TemplateDto>());
        }

        [HttpPut("{templateId}")]
        public async Task<ActionResult<TemplateDto>> UpdateReportTemplate(
            int reportId,
            int templateId,
            [FromForm(Name = "template")] IFormFile data)
        {
            var template = await this.context.Templates
                .FirstOrDefaultAsync(x => x.Id == templateId);

            template.Data = GetBytesFromFile(data);

            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportTemplates), new { reportId }, template.Adapt<TemplateDto>());
        }

        [HttpDelete("{templateId}")]
        public async Task<ActionResult<TemplateDto>> DeleteReportTemplate(int templateId)
        {
            var template = await this.context.Templates
                .FirstOrDefaultAsync(x => x.Id == templateId);
            if (template == null)
            {
                return NotFound();
            }

            this.context.Templates.Remove(template);
            await context.SaveChangesAsync();

            return this.Ok();
        }


        [HttpGet("{templateId}/data")]
        public async Task<IActionResult> GetReportTemplateData(int reportId, int templateId)
        {
            var report = await this.context.Reports
                .Include(x => x.Templates)
                .FirstOrDefaultAsync(x => x.Id == reportId);

            var template = report.Templates.FirstOrDefault(x => x.Id == templateId);
            if (template == null)
            {
                return NotFound();
            }

            var templateStream = new MemoryStream(template.Data);

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
