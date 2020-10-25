namespace SofTrust.Report.Core.Generator.Source
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Trs;
    using System.Linq;

    public class SourceFactory
    {
        const string DATASOURCE_TYPE_MSSQL = "msSql";
        const string DATASOURCE_TYPE_POSTGRESQL = "postgreSql";

        const string DATASOURCE_CONNECTION_TYPE_CONNECTION_STRING = "connectionString";
        const string DATASOURCE_CONNECTION_TYPE_HOST = "host";

        private TrsContext trsContext;

        public SourceFactory(TrsContext trsContext)
        {
            this.trsContext = trsContext;
        }

        public ISource Create(JToken dataSource, string host)
        {
            return Create(dataSource["name"].ToString(), dataSource["type"].ToString(), dataSource["data"], host);
        }

        public ISource Create(string name, string type, JToken data, string host)
        {
            var connectionString = string.Empty;
            switch (data["connectionType"].ToString())
            {
                case DATASOURCE_CONNECTION_TYPE_CONNECTION_STRING:
                    connectionString = data["connectionString"].ToString();
                    break;
                case DATASOURCE_CONNECTION_TYPE_HOST:
                    connectionString = GetConnectionString(host);
                    break;
                default:
                    break;
            }

            switch (type)
            {
                case DATASOURCE_TYPE_MSSQL:
                    return new MsSqlConnectionStringSource(connectionString) { Name = name };
                case DATASOURCE_TYPE_POSTGRESQL:
                    return new NpgsqlConnectionStringSource(connectionString) { Name = name };
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
