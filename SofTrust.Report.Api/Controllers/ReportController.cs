namespace SofTrust.Report.Api.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Business;
    using SofTrust.Report.Business.Model.Dto;
    using SofTrust.Report.Business.Service.Report;

    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportContext context;
        private readonly ReportService reportService;

        public ReportController(ReportContext reportContext,
            ReportService reportService)
        {
            this.context = reportContext;
            this.reportService = reportService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListItemReportDto>>> GetReports()
        {
            var reports = await this.context.Reports
                .ToListAsync();
            return this.Ok(reports.Adapt<IEnumerable<ListItemReportDto>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            var report = await this.context.Reports
                .Include(x => x.ReportDataSources).ThenInclude(x => x.DataSource)
                .Include(x => x.ReportDataSets).ThenInclude(x => x.DataSet)
                .Include(x => x.ReportVariables).ThenInclude(x => x.Variable)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            return this.Ok(report.AdaptToDto());
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateReport([FromBody] ReportDto reportDto)
        {
            var report = reportDto.AdaptToDomain();
            this.context.Reports.Add(report);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report.AdaptToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReportDto>> UpdateReport(int id, [FromBody] ReportDto reportDto)
        {
            var report = reportDto.AdaptToDomain();
            report = await this.reportService.UpdateAsync(report);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report.AdaptToDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReport(int id)
        {
            var report = await context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            this.reportService.DeleteAsync(id);
            await context.SaveChangesAsync();

            return this.Ok();
        }
    }
}