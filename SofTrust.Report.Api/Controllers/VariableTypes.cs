namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Core.Generator.DataAdapter;
    using SofTrust.Report.Core.Generator.DataReader;
    using SofTrust.Report.Core.Generator.Source;
    using SofTrust.Report.Infrastructure;
    using System.Collections.Generic;

    [Route("api/variableType")]
    [ApiController]
    public class VariableTypes : ControllerBase
    {
        private readonly SourceFactory sourceFactory;
        private readonly DataReaderFactory dataReaderFactory;

        public VariableTypes(SourceFactory sourceFactory,
            DataReaderFactory dataReaderFactory)
        {
            this.sourceFactory = sourceFactory;
            this.dataReaderFactory = dataReaderFactory;
        }

        [HttpPost("select/data")]
        public ActionResult<IEnumerable<object>> GetSelectData(
            [FromForm(Name = "dataSource")] string dataSourceString,
            [FromForm(Name = "context")] string contextJson,
            [FromForm(Name = "query")] string query)
        {
            var dataSource = JToken.Parse(dataSourceString);
            var reportContext = JToken.Parse(contextJson);

            var source = sourceFactory.Create(dataSource, reportContext);

            var dataReader = dataReaderFactory.CreateSqlQueryDataSet(query, source);

            return dataReader.GetData().ToListDictionaryAdapt();
        }
    }
}
