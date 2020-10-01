namespace SofTrust.Report.Business.Service.DataSource
{
    using Newtonsoft.Json.Linq;

    public class DataSourceFactory
    {
        const string DATASOURCE_TYPE_MSSQL = "MsSql";
        const string DATASOURCE_TYPE_POSTGRESQL = "PostgreSql";

        public DataSource Create(JToken dataSource)
        {
            switch (dataSource["type"].ToString())
            {
                case DATASOURCE_TYPE_MSSQL:
                    return new MsSqlDataSource(dataSource["data"]["connectionString"].ToString()) { Name = dataSource["name"].ToString() };
                case DATASOURCE_TYPE_POSTGRESQL:
                    return new NpgsqlDataSource(dataSource["data"]["connectionString"].ToString()) { Name = dataSource["name"].ToString() };
            }
            return null;
        }
    }
}
