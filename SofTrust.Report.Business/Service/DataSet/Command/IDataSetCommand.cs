namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Service;
    using SofTrust.Report.Business.Service.DataSet.Reader;
    using SofTrust.Report.Business.Service.DataSource.Connection;
    using System.Collections.Generic;

    public interface IDataSetCommand
    {
        IDataSourceConnection Connection { get; set; }
        IDataSetReader ExecuteReader();
        void AddParameters(IEnumerable<Parameter> parameters);
        int Timeout { set; }
    }
}
