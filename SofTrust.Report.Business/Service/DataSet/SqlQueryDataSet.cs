namespace SofTrust.Report.Business.Service.DataSet
{
    using System.Collections.Generic;
    using SofTrust.Report.Business.Service.DataSet.Reader;
    using SofTrust.Report.Business.Service.DataSource;
    using SofTrust.Report.Business.Model;

    public class SqlQueryDataSet : IDataSet
    {
        private readonly IDataSource dataSource;
        private readonly string query;
        private readonly IEnumerable<Parameter> parameters;

        public string Name { get; set; }

        public SqlQueryDataSet(IDataSource dataSource, string query, IEnumerable<Parameter> parameters)
        {
            this.dataSource = dataSource;
            this.query = query;
            this.parameters = parameters;
        }

        public IDataSetReader ExecuteReader()
        {
            var dataSourceConnection = this.dataSource.CreateConnection();
            var sqlQuery = GenerateQuery();
            var command = dataSourceConnection.CreateCommand(sqlQuery);
            command.Connection.Open();
            return command.ExecuteReader();
        }

        private string GenerateQuery()
        {
            var genQuery = query;

            foreach(var parameter in parameters)
            {
                genQuery = genQuery.Replace($"@{parameter.Name}", parameter.Value.ToString());
            }

            return genQuery;
        }
    }
}
