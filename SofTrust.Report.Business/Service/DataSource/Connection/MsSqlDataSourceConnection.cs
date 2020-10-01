namespace SofTrust.Report.Business.Service.DataSource.Connection
{
    using SofTrust.Report.Business.Service.DataSet.Command;
    using System;
    using System.Data.SqlClient;

    public class MsSqlDataSourceConnection : IDataSourceConnection, IDisposable
    {
        private SqlConnection connection;

        public MsSqlDataSourceConnection(SqlConnection connection)
        {
            this.connection = connection;
        }

        public void Open()
        {
            this.connection.Open();
        }

        public void Dispose()
        {
            this.connection.Close();
        }

        public IDataSetCommand CreateCommand(object cmd)
        {
            return new MsSqlQueryDataSetCommand(this, new SqlCommand(cmd.ToString(), connection));
        }
    }
}
