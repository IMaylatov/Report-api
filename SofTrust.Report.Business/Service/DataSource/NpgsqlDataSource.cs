namespace SofTrust.Report.Business.Service.DataSource
{
    using SofTrust.Report.Business.Service.DataSource.Connection;

    public class NpgsqlDataSource : IDataSource
    {
        private readonly string connectionString;

        public string Name { get; set; }

        public NpgsqlDataSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDataSourceConnection CreateConnection()
        {
            throw new System.NotImplementedException();
        }
    }
}
