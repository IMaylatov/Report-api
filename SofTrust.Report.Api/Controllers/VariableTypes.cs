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
        private readonly ReportContext context;
        private readonly SourceFactory sourceFactory;
        private readonly DataReaderFactory dataReaderFactory;

        public VariableTypes(ReportContext reportContext,
            SourceFactory sourceFactory,
            DataReaderFactory dataReaderFactory)
        {
            this.context = reportContext;
            this.sourceFactory = sourceFactory;
            this.dataReaderFactory = dataReaderFactory;
        }

        [HttpPost("select/data")]
        public ActionResult<IEnumerable<object>> GetSelectData(
            [FromForm(Name = "dataSource")] string dataSourceString,
            [FromForm(Name = "host")] string host,
            [FromForm(Name = "query")] string query,
            [FromForm(Name = "valueField")] string valueField,
            [FromForm(Name = "value")] string value,
            [FromForm(Name = "take")] int take)
        {
            var dataSource = JToken.Parse(dataSourceString);

            var source = sourceFactory.Create(dataSource["name"].ToString(), dataSource["type"].ToString(), dataSource["data"], host);
            query = $"select top {take} * from ({query}) {valueField}Tmp where {valueField} like '%{value}%'";
            var dataReader = dataReaderFactory.CreateSqlQueryDataSet(query, source);

            return dataReader.GetData().ToListDictionaryAdapt();
        }
    }
}
