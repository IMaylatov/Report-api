namespace SofTrust.Report.Business.Service.DataSource
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Service.DataSource.Command;

    public interface IDataSourceFactory
    {
        IDataSourceCommand Create(JToken dataSource);
    }
}
