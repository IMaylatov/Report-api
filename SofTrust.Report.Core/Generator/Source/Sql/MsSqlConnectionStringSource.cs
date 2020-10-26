namespace SofTrust.Report.Core.Generator.Source.Sql
{
    using SofTrust.Report.Core.Generator.Connection;
    using System.Data.SqlClient;

    public class MsSqlConnectionStringSource : ISource
    {
        private readonly string connectionString;

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
