namespace SofTrust.Report.Core.Generator.DataReader
{
    using System.Collections.Generic;

    public class SqlQueryDataReader : IDataReader
    {
        private readonly ISource dataSource;
        private readonly string query;
        private readonly IEnumerable<Variable> variables;
        private readonly int timeout;

        public SqlQueryDataReader(ISource dataSource, string query, IEnumerable<Variable> parameters, int timeout)
        {
            this.dataSource = dataSource;
            this.query = query;
            this.variables = parameters;
            this.timeout = timeout;
        }

        public IData CreateReader()
        {
            var dataSourceConnection = this.dataSource.CreateConnection();
            var command = dataSourceConnection.CreateCommand(query);
            command.AddVariables(variables);
            command.Connection.Open();
            command.Timeout = this.timeout;
            return command.ExecuteReader();
        }
    }
}
