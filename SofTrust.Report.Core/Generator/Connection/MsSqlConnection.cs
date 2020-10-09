namespace SofTrust.Report.Core.Generator.Connection
{
    using SofTrust.Report.Core.Generator.Command;
    using System;
    using System.Data.SqlClient;

    public class MsSqlConnection : IConnection, IDisposable
    {
        private SqlConnection connection;

        public MsSqlConnection(SqlConnection connection)
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

        public ICommand CreateCommand(object cmd)
        {
            return new MsSqlQueryDataSetCommand(this, new SqlCommand(cmd.ToString(), connection));
        }
    }
}
