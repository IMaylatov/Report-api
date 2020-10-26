namespace SofTrust.Report.Core.Generator.Source.Sql
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Trs;
    using System.Linq;

    public class SqlSourceFactory
    {
        public const string DATASOURCE_TYPE_MSSQL = "msSql";
        public const string DATASOURCE_TYPE_POSTGRESQL = "postgreSql";

        const string DATASOURCE_CONNECTION_TYPE_CONNECTION_STRING = "connectionString";
        const string DATASOURCE_CONNECTION_TYPE_HOST = "host";

        private TrsContext trsContext;

        public SqlSourceFactory(TrsContext trsContext)
        {
            this.trsContext = trsContext;
        }

        public ISource Create(JToken dataSource, JToken reportContext)
        {
            var connectionString = string.Empty;
            switch (dataSource["data"]["connectionType"].ToString())
            {
                case DATASOURCE_CONNECTION_TYPE_CONNECTION_STRING:
                    connectionString = dataSource["data"]["connectionString"].ToString();
                    break;
                case DATASOURCE_CONNECTION_TYPE_HOST:
                    connectionString = GetConnectionString(reportContext["host"].ToString());
                    break;
                default:
                    break;
            }

            switch (dataSource["type"].ToString())
            {
                case DATASOURCE_TYPE_MSSQL:
                    return new MsSqlConnectionStringSource(connectionString);
                case DATASOURCE_TYPE_POSTGRESQL:
                    return new NpgsqlConnectionStringSource(connectionString);
            }

            return null;
        }

        private string GetConnectionString(string hostName)
        {
            var host = trsContext.Hosts.FirstOrDefault(x => x.Name == hostName);
            return host.ConnectionString;
        }
    }
}
