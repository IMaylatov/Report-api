namespace SofTrust.Report.Api.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Core.Models.Domain;
    using SofTrust.Report.Core.Models.Dto;
    using SofTrust.Report.Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/dataSources")]
    [ApiController]
    public class DataSourceController : ControllerBase
    {
        private readonly ReportContext context;

        public DataSourceController(ReportContext reportContext)
        {
            this.context = reportContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListItemDataSourceDto>>> GetReports()
        {
            var dataSources = await this.context.DataSources
                .ToListAsync();
            return this.Ok(dataSources.Adapt<IEnumerable<ListItemDataSourceDto>>());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DataSourceDto>> GetDataSourceById(int id)
        {
            var dataSource = await this.context.DataSources
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dataSource == null)
            {
                return NotFound();
            }
            return this.Ok(dataSource.AdaptToDto());
        }

        [HttpPost]
        public async Task<ActionResult<DataSourceDto>> CreateDataSource([FromBody] DataSourceDto reportDto)
        {
            var dataSource = reportDto.Adapt<DataSource>();
            this.context.DataSources.Add(dataSource);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDataSourceById), new { id = dataSource.Id }, dataSource.AdaptToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DataSourceDto>> UpdateDataSources(int id, [FromBody] DataSourceDto reportDto)
        {
            var dataSource = reportDto.Adapt<DataSource>();
            this.context.DataSources.Update(dataSource);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDataSourceById), new { id = dataSource.Id }, dataSource.AdaptToDto());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDataSource(int id)
        {
            var dataSource = await context.DataSources.FindAsync(id);
            if (dataSource == null)
            {
                return NotFound();
            }

            this.context.DataSources.Remove(dataSource);
            await context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
