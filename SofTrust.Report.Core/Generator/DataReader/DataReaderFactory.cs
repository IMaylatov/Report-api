namespace SofTrust.Report.Core.Generator.DataReader
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public class DataReaderFactory
    {
        private const string DATASET_TYPE_SQLQUERY = "SqlQuery";

        private readonly int timeout;

        public DataReaderFactory(IConfiguration configuration)
        {
            this.timeout = int.Parse(configuration["XlsxReport:DataSet:SqlQuery:CommandTimeout"]);
        }

        public IDataReader Create(JToken dataSetJ, IEnumerable<ISource> dataSources, IEnumerable<Parameter> parameters)
        {
            switch (dataSetJ["type"].ToString())
            {
                case DATASET_TYPE_SQLQUERY:
                    {
                        var name = dataSetJ["name"].ToString();
                        var dataSourceName = dataSetJ["data"]["dataSourceName"].ToString();
                        var query = dataSetJ["data"]["query"].ToString();
                        return new SqlQueryDataReader(dataSources.FirstOrDefault(x => x.Name == dataSourceName) , query, parameters, timeout) { Name = name };
                    }
            }
            return null;
        }

        public IDataReader CreateSqlQueryDataSet(string query, ISource dataSource)
        {
            return new SqlQueryDataReader(dataSource, query, new Parameter[] { }, timeout);
        }
    }
}
