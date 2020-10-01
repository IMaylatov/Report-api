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
        public async Task<IActionResult> Get(int reportId)
        {
            var report = await this.context.Reports
                .Include(x => x.Template)
                .Include(x => x.Type)
                .FirstOrDefaultAsync(x => x.Id == reportId);

            var templateStream = new MemoryStream(report.Template.Data);

            switch (report.Type.Name)
            {
                case "ClosedXml":
                    return new FileStreamResult(templateStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
                case "Malibu":
                    return new FileStreamResult(templateStream, "application/octet-stream") { FileDownloadName = $"report.mlbrpt" };
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<TemplateDto>> Put(
            int reportId,
            [FromForm(Name = "template")] IFormFile data)
        {
            var report = await this.context.Reports.Include(x => x.Template)
                .FirstOrDefaultAsync(x => x.Id == reportId);

            byte[] dataBytes;
            using (var templateStream = data.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                templateStream.CopyTo(memoryStream);
                dataBytes = memoryStream.ToArray();
            }

            if (report.Template == null)
            {
                report.Template = new Business.Model.Template { Data = dataBytes };
            }
            else
            {
                report.Template.Data = dataBytes;
            }

            await context.SaveChangesAsync();

            return Ok(report.Template.Adapt<TemplateDto>());
        }
    }
}
