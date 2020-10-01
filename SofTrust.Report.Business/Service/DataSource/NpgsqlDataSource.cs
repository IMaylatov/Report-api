namespace SofTrust.Report.Business.Service.DataSource
{
    using SofTrust.Report.Business.Service.DataSource.Connection;

    public class NpgsqlDataSource : DataSource
    {
        private readonly string connectionString;

        public NpgsqlDataSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IDataSourceConnection CreateConnection()
        {
            throw new System.NotImplementedException();
        }
    }
}
