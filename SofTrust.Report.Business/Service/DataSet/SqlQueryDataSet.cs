namespace SofTrust.Report.Business.Service.DataSet
{
    using System.Collections.Generic;
    using SofTrust.Report.Business.Service.DataSet.Reader;
    using SofTrust.Report.Business.Service.DataSource;
    using SofTrust.Report.Business.Model;
    using MoreLinq;

    public class SqlQueryDataSet : IDataSet
    {
        private readonly IDataSource dataSource;
        private readonly string query;
        private readonly IEnumerable<Parameter> parameters;
        private readonly int timeout;

        public string Name { get; set; }

        public SqlQueryDataSet(IDataSource dataSource, string query, IEnumerable<Parameter> parameters, int timeout)
        {
            this.dataSource = dataSource;
            this.query = query;
            this.parameters = parameters;
            this.timeout = timeout;
        }

        public IDataSetReader ExecuteReader()
        {
            var dataSourceConnection = this.dataSource.CreateConnection();
            var sqlQuery = query;
            parameters.ForEach(x => sqlQuery = sqlQuery.Replace($"@{x.Name}", $"@{x.Name.Replace(".", "_")}"));
            var command = dataSourceConnection.CreateCommand(sqlQuery);
            command.AddParameters(parameters);
            command.Connection.Open();
            command.Timeout = this.timeout;
            return command.ExecuteReader();
        }
    }
}
