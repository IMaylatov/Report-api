namespace SofTrust.Report.Business.Service.DataSource.Connection
{
    using SofTrust.Report.Business.Service.DataSet.Command;
    using System;

    public interface IDataSourceConnection : IDisposable
    {
        void Open();
        IDataSetCommand CreateCommand(object cmd);
    }
}
