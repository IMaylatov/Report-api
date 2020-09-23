namespace SofTrust.Report.Business.Service.DataSource.Command
{
    using SofTrust.Report.Business.Service.DataAdapter;

    public interface IDataSourceCommand
    {
        object Execute(object dataSet);
    }
}
