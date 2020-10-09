namespace SofTrust.Report.Core.Generator.DataReader
{
    using System.Collections.Generic;

    public class SqlQueryDataReader : IDataReader
    {
        private readonly ISource dataSource;
        private readonly string query;
        private readonly IEnumerable<Parameter> parameters;
        private readonly int timeout;

        public string Name { get; set; }

        public SqlQueryDataReader(ISource dataSource, string query, IEnumerable<Parameter> parameters, int timeout)
        {
            this.dataSource = dataSource;
            this.query = query;
            this.parameters = parameters;
            this.timeout = timeout;
        }

        public IData CreateReader()
        {
            var dataSourceConnection = this.dataSource.CreateConnection();
            var command = dataSourceConnection.CreateCommand(query);
            command.AddParameters(parameters);
            command.Connection.Open();
            command.Timeout = this.timeout;
            return command.ExecuteReader();
        }
    }
}
