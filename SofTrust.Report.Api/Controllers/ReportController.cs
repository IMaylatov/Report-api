namespace SofTrust.Report.Api.Controllers
{
    using System.Collections.Generic;
    using ClosedXML.Excel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Service.Report;

    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportGeneratorFactory reportGeneratorFactory;

        public ReportController(ReportGeneratorFactory reportGeneratorFactory)
        {
            this.reportGeneratorFactory = reportGeneratorFactory;
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
            [FromForm(Name = "template")] IFormFile template)
        {
            var report = JToken.Parse(reportJson);

            var reportGenerator = this.reportGeneratorFactory.Create(report["type"].ToString());
            using (var templateStream = template.OpenReadStream())
            {
                return reportGenerator.Generate(report, templateStream);
            }
        }
    }
}