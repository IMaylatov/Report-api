namespace SofTrust.Report.Api.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Service.Report;

    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
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
            [FromForm(Name = "parameters")] string parametersJson,
            [FromForm(Name = "dataSources")] string dataSourcesJson,
            [FromForm(Name = "dataSets")] string dataSetsJson,
            [FromForm(Name = "templateType")] string templatetype,
            [FromForm(Name = "template")] IFormFile template)
        {
            var parameters = JArray.Parse(parametersJson);
            var dataSources = JArray.Parse(dataSourcesJson);
            var dataSets = JArray.Parse(dataSetsJson);

            var formedReport = reportService.Run(templatetype, template, parameters, dataSources, dataSets);

            return File(formedReport, "application/octet-stream", "response.xlsx");
        }
    }
}