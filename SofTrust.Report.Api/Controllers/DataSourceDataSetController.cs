﻿namespace SofTrust.Report.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SofTrust.Report.Business;
    using SofTrust.Report.Business.Service.DataSet;
    using SofTrust.Report.Business.Service.DataSource;
    using System.Collections.Generic;
    using SofTrust.Report.Service;
    using System.Threading.Tasks;

    [Route("api/dataSources/{dataSourceId}/dataSets")]
    [ApiController]
    public class DataSourceDataSetController : ControllerBase
    {
        private readonly ReportContext context;
        private readonly DataSourceFactory dataSourceFactory;
        private readonly DataSetFactory dataSetFactory;

        public DataSourceDataSetController(ReportContext reportContext,
            DataSourceFactory dataSourceFactory,
            DataSetFactory dataSetFactory)
        {
            this.context = reportContext;
            this.dataSourceFactory = dataSourceFactory;
            this.dataSetFactory = dataSetFactory;
        }

        [HttpGet("sqlQuery/items")]
        public async Task<ActionResult<IEnumerable<object>>> GetSqlQueryItems(int dataSourceId, string query, string valueField, string value, int take)
        {
            var dataSource = await this.context.DataSources.FindAsync(dataSourceId);
            var dataSourceE = dataSourceFactory.Create(dataSource);
            query = $"select top {take} * from ({query}) {valueField}Tmp where {valueField} like '%{value}%'";
            var dataSet = dataSetFactory.CreateSqlQueryDataSet(query, dataSourceE);

            var reader = dataSet.ExecuteReader();
            var datas = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                var data = new Dictionary<string, object>();
                var unnamedColumnIndex = 1;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var fieldName = reader.GetName(i);
                    if (string.IsNullOrWhiteSpace(fieldName))
                    {
                        fieldName = $"Column{unnamedColumnIndex++}";
                    }
                    data.Add(data.ContainsKey(fieldName) ? $"{fieldName}{i}" : fieldName, reader.GetValue(i));
                }
                datas.Add(data);
            }

            return datas;
        }
    }
}