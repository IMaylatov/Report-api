namespace SofTrust.Report.Business.Service.DataSource
{
    using Newtonsoft.Json.Linq;

    public class DataSourceFactory
    {
        const string DATASOURCE_TYPE_MSSQL = "MsSql";
        const string DATASOURCE_TYPE_POSTGRESQL = "PostgreSql";

        public IDataSource Create(JToken dataSource)
        {
            IDataSource source = null;
            switch (dataSource["type"].ToString())
            {
                case DATASOURCE_TYPE_MSSQL:
                    source = new MsSqlDataSource(dataSource["data"]["connectionString"].ToString()) { Name = dataSource["name"].ToString() };
                    break;
                case DATASOURCE_TYPE_POSTGRESQL:
                    source = new NpgsqlDataSource(dataSource["data"]["connectionString"].ToString()) { Name = dataSource["name"].ToString() };
                    break;
                default:
                    break;
            }
            return source;
        }
    }
}
