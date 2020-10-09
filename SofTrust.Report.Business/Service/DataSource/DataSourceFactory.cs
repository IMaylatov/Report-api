namespace SofTrust.Report.Business.Service.DataSource
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public class DataSourceFactory
    {
        const string DATASOURCE_TYPE_MSSQL = "MsSql";
        const string DATASOURCE_TYPE_POSTGRESQL = "PostgreSql";

        public DataSource Create(JToken dataSource)
        {
            return Create(dataSource["type"].ToString(), dataSource["name"].ToString(), dataSource["data"]);
        }

        public DataSource Create(Model.Domain.DataSource dataSource)
        {
            var dataJ = JToken.Parse(dataSource.Data);
            return Create(dataSource.Type, dataSource.Name, dataJ);
        }

        private DataSource Create(string type, string name, JToken data)
        {
            switch (type)
            {
                case DATASOURCE_TYPE_MSSQL:
                    return new MsSqlDataSource(data["connectionString"].ToString()) { Name = name };
                case DATASOURCE_TYPE_POSTGRESQL:
                    return new NpgsqlDataSource(data["connectionString"].ToString()) { Name = name };
            }
            return null;
        }
    }
}
