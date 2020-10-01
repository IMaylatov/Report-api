﻿namespace SofTrust.Report.Api.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Business;
    using SofTrust.Report.Business.Model;
    using SofTrust.Report.Business.Model.Dto;

    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportContext context;

        public ReportController(ReportContext reportContext)
        {
            this.context = reportContext;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListItemReportDto>>> Get()
        {
            var reports = await this.context.Reports
                .ToListAsync();
            return this.Ok(reports.Adapt<IEnumerable<ListItemReportDto>>());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportDto>> Get(int id)
        {
            var report = await this.context.Reports
                .Include(x => x.Type)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            return this.Ok(report.Adapt<ReportDto>());
        }

        [HttpPost]
        public async Task<ActionResult<SaveReportDto>> Post([FromBody] SaveReportDto reportDto)
        {
            var report = reportDto.Adapt<Report>();
            this.context.Reports.Add(report);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = report.Id }, report.Adapt<SaveReportDto>());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SaveReportDto>> Put(int id, [FromBody] SaveReportDto reportDto)
        {
            var report = reportDto.Adapt<Report>();
            this.context.Entry(report).State = EntityState.Modified;
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = report.Id }, report.Adapt<SaveReportDto>());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var report = await context.Reports.FindAsync(id);
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