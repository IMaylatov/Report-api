namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SofTrust.Report.Business;
    using SofTrust.Report.Business.Model;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/reportTypes")]
    [ApiController]
    public class ReportTypeController : ControllerBase
    {
        private readonly ReportContext reportContext;

        public ReportTypeController(ReportContext reportContext)
        {
            this.reportContext = reportContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReportType>> Get()
        {
            var reportTypes = this.reportContext.ReportTypes.ToList();
            return Ok(reportTypes);
        }
    }
}
