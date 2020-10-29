namespace SofTrust.Report.Api.Controllers
{
    using Mapster;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Core.Models.Domain;
    using SofTrust.Report.Core.Models.Dto;
    using SofTrust.Report.Infrastructure;
    using System.Threading.Tasks;

    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ReportContext context;

        public UserController(ReportContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetReportById(string id)
        {
            var report = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            return this.Ok(report.Adapt<UserDto>());
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateUser([FromBody] UserDto userDto)
        {
            var report = userDto.Adapt<User>();
            this.context.Users.Add(report);
            await this.context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReportById), new { id = report.Id }, report.Adapt<UserDto>());
        }
    }
}
