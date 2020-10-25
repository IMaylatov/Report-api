namespace SofTrust.Report.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Mapster;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Core.Models.Dto;
    using SofTrust.Report.Infrastructure;
    using SofTrust.Report.Infrastructure.Repository;

    [Route("api/reports")]
    [Authorize]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportContext context;
        private readonly ReportRepository reportRepository;

        public ReportController(ReportContext reportContext,
            ReportRepository reportRepository)
        {
            this.context = reportContext;
            this.reportRepository = reportRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListItemReportDto>>> GetReports(string name = "")
        {
            var queryReports = this.context.Reports.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                queryReports = queryReports.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{name.ToLower()}%"));
            }

            var reports = await queryReports.ToListAsync();
            return this.Ok(reports.Adapt<IEnumerable<ListItemReportDto>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            var report = await this.context.Reports
                .Include(x => x.DataSources)
                .Include(x => x.DataSets)
                .Include(x => x.Variables)
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
            var report = reportDto.Adapt<Core.Models.Domain.Report>();
            this.context.Reports.Add(report);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report.AdaptToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReportDto>> UpdateReport(int id, [FromBody] ReportDto reportDto)
        {
            var report = reportDto.AdaptToDomain();
            report = await reportRepository.UpdateGraphAsync(report);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report.AdaptToDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReport(int id)
        {
            var report = await context.Reports
                   .FirstOrDefaultAsync(x => x.Id == id);

            if (report == null)
            {
                return NotFound();
            }

            context.Reports.Remove(report);
            await context.SaveChangesAsync();

            return this.Ok();
        }
    }
}