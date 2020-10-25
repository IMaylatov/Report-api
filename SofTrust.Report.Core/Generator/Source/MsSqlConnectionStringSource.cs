namespace SofTrust.Report.Core.Generator.Source
{
    using SofTrust.Report.Core.Generator.Connection;
    using System.Data.SqlClient;

    public class MsSqlConnectionStringSource : ISource
    {
        private readonly string connectionString;

        public string Name { get; set; }

        public MsSqlConnectionStringSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IConnection CreateConnection()
        {
            return new MsSqlConnection(new SqlConnection(connectionString));
        }
    }
}
