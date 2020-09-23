namespace SofTrust.Report.Business.Service.DataSource
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Service.DataSource.Command;

    public class DataSourceFactory : IDataSourceFactory
    {
        const string DATASOURCE_TYPE_MSSQL = "MsSql";
        const string DATASOURCE_TYPE_POSTGRESQL = "PostgreSql";

        public IDataSourceCommand Create(JToken dataSource)
        {
            IDataSourceCommand source = null;
            switch (dataSource["type"].ToString())
            {
                case DATASOURCE_TYPE_MSSQL:
                    source = new MsSqlDataSourceCommand(dataSource["data"]["connectionString"].ToString());
                    break;
                case DATASOURCE_TYPE_POSTGRESQL:
                    source = new NpgsqlDataSourceCommand(dataSource["data"]["connectionString"].ToString());
                    break;
                default:
                    break;
            }
            return source;
        }
    }
}
