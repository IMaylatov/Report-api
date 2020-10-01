namespace SofTrust.Report.Business.Service.DataSource
{
    using SofTrust.Report.Business.Service.DataSource.Connection;
    using System.Data.SqlClient;

    public class MsSqlDataSource : DataSource
    {
        private readonly string connectionString;

        public MsSqlDataSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IDataSourceConnection CreateConnection()
        {
            return new MsSqlDataSourceConnection(new SqlConnection(connectionString));
        }
    }
}
