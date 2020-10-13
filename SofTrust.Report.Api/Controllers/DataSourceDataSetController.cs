namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SofTrust.Report.Core.Generator.DataReader;
    using SofTrust.Report.Core.Generator.Source;
    using SofTrust.Report.Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SofTrust.Report.Core.Generator.DataAdapter;

    [Route("api/dataSources/{dataSourceId}/dataSets")]
    [ApiController]
    public class DataSourceDataSetController : ControllerBase
    {
        private readonly ReportContext context;
        private readonly SourceFactory sourceFactory;
        private readonly DataReaderFactory dataReaderFactory;

        public DataSourceDataSetController(ReportContext reportContext,
            SourceFactory sourceFactory,
            DataReaderFactory dataReaderFactory)
        {
            this.context = reportContext;
            this.sourceFactory = sourceFactory;
            this.dataReaderFactory = dataReaderFactory;
        }

        [HttpGet("sqlQuery/items")]
        public async Task<ActionResult<IEnumerable<object>>> GetSqlQueryItems(int dataSourceId, string query, string valueField, string value, int take)
        {
            var dataSource = await this.context.DataSources.FindAsync(dataSourceId);
            var source = sourceFactory.Create(dataSource);
            query = $"select top {take} * from ({query}) {valueField}Tmp where {valueField} like '%{value}%'";
            var dataReader = dataReaderFactory.CreateSqlQueryDataSet(query, source);

            return dataReader.GetData().ToListDictionaryAdapt();
        }
    }
}
