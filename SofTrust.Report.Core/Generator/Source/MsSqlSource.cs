namespace SofTrust.Report.Core.Generator.Source
{
    using SofTrust.Report.Core.Generator.Connection;
    using System.Data.SqlClient;

    public class MsSqlSource : ISource
    {
        private readonly string connectionString;

        public string Name { get; set; }

        public MsSqlSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IConnection CreateConnection()
        {
            return new MsSqlConnection(new SqlConnection(connectionString));
        }
    }
}
