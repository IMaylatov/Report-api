namespace SofTrust.Report.Core.Generator.DataReader
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    public class DataReaderFactory
    {
        private const string DATASET_TYPE_SQLQUERY = "sqlQuery";

        private readonly int timeout;

        public DataReaderFactory(IConfiguration configuration)
        {
            this.timeout = int.Parse(configuration["XlsxReport:DataSet:SqlQuery:CommandTimeout"]);
        }

        public IDataReader Create(JToken dataSetJ, Dictionary<string, ISource> dataSources, IEnumerable<Variable> variables)
        {
            switch (dataSetJ["type"].ToString())
            {
                case DATASET_TYPE_SQLQUERY:
                    {
                        var dataSourceName = dataSetJ["data"]["dataSourceName"].ToString();
                        var query = dataSetJ["data"]["query"].ToString();
                        return new SqlQueryDataReader(dataSources[dataSourceName], query, variables, timeout);
                    }
            }
            return null;
        }

        public IDataReader CreateSqlQueryDataSet(string query, ISource dataSource)
        {
            return new SqlQueryDataReader(dataSource, query, new Variable[] { }, timeout);
        }
    }
}
