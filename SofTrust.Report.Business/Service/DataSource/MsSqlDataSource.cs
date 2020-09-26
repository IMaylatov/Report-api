namespace SofTrust.Report.Business.Service.DataSource
{
    using SofTrust.Report.Business.Service.DataSource.Connection;
    using System.Data.SqlClient;

    public class MsSqlDataSource : IDataSource
    {
        private readonly string connectionString;

        public string Name { get; set; }

        public MsSqlDataSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDataSourceConnection CreateConnection()
        {
            return new MsSqlDataSourceConnection(new SqlConnection(connectionString));
        }
    }
}
