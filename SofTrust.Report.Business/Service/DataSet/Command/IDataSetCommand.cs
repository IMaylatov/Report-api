namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Business.Service.DataAdapter;
    using SofTrust.Report.Business.Service.DataSource.Command;
    using System.Collections.Generic;

    public interface IDataSetCommand
    {
        object Execute(Dictionary<string, string> parameters, Dictionary<string, IDataSourceCommand> dataSources, IDataSetAdapter dataSetAdapter);
    }
}
