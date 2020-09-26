namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Business.Service.DataSet.Reader;
    using SofTrust.Report.Business.Service.DataSource.Connection;

    public interface IDataSetCommand
    {
        IDataSourceConnection Connection { get; set; }
        IDataSetReader ExecuteReader();
    }
}
