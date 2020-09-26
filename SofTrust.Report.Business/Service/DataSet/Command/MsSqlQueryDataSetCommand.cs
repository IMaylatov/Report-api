namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Business.Service.DataSet.Reader;
    using SofTrust.Report.Business.Service.DataSource.Connection;
    using System.Data.SqlClient;

    public class MsSqlQueryDataSetCommand : IDataSetCommand
    {
        private SqlCommand command;

        public IDataSourceConnection Connection { get; set; }

        public MsSqlQueryDataSetCommand(IDataSourceConnection connection, SqlCommand command)
        {
            this.Connection = connection;
            this.command = command;
        }

        public IDataSetReader ExecuteReader()
        {
            return new SqlDataSetReader(command.ExecuteReader());
        }
    }
}
