namespace SofTrust.Report.Business.Service.DataSource
{
    using SofTrust.Report.Business.Service.DataSource.Connection;

    public abstract class DataSource
    {
        public string Name { get; set; }

        public abstract IDataSourceConnection CreateConnection();
    }
}
