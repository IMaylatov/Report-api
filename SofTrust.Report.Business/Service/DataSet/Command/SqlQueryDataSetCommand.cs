namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Business.Service.DataAdapter;
    using SofTrust.Report.Business.Service.DataSource.Command;
    using System.Collections.Generic;

    public class SqlQueryDataSetCommand : IDataSetCommand
    {
        private readonly string dataSourceName;
        private readonly string query;

        public SqlQueryDataSetCommand(string dataSourceName, string query)
        {
            this.dataSourceName = dataSourceName;
            this.query = query;
        }

        public object Execute(Dictionary<string, string> parameters, Dictionary<string, IDataSourceCommand> dataSources, IDataSetAdapter dataSetAdapter)
        {
            var sqlQuery = GetQueryWithParameters(query, parameters);

            var data = dataSources[dataSourceName].Execute(sqlQuery);

            return dataSetAdapter.Adapt(data);
        }

        private string GetQueryWithParameters(string query, Dictionary<string, string> parameters)
        {
            foreach(var parameter in parameters)
            {
                query = query.Replace($"@{parameter.Key}", parameter.Value.ToString());
            }
            return query;
        }
    }
}
