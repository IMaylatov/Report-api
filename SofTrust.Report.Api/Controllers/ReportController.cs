namespace SofTrust.Report.Api.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Api.Service.Report;
    using SofTrust.Report.Business.Service.Report;

    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportServiceFactory reportServiceFactory;

        public ReportController(IReportServiceFactory reportServiceFactory)
        {
            this.reportServiceFactory = reportServiceFactory;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("run")]
        public IActionResult Run(
            [FromForm(Name = "report")] string reportJson,
            [FromForm(Name = "templateData")] IFormFile template)
        {
            var report = JToken.Parse(reportJson);

            var reportService = this.reportServiceFactory.Create(report["type"].ToString());
            
            return reportService.Run(report, template);
        }
    }
}