namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Business.Service.DataAdapter;
    using SofTrust.Report.Business.Service.DataSource.Command;
    using System.Collections.Generic;
    using SofTrust.Report.Business.Model;

    public interface IDataSetCommand
    {
        object Execute(IEnumerable<Parameter> parameters, Dictionary<string, IDataSourceCommand> dataSources, IDataSetAdapter dataSetAdapter);
    }
}
