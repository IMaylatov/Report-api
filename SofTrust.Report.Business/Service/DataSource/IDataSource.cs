namespace SofTrust.Report.Business.Service.DataSource
{
    using SofTrust.Report.Business.Service.DataSource.Connection;

    public interface IDataSource
    {
        public string Name { get; set; }
        IDataSourceConnection CreateConnection();
    }
}
