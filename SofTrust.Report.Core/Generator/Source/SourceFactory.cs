namespace SofTrust.Report.Core.Generator.Source
{
    using Newtonsoft.Json.Linq;

    public class SourceFactory
    {
        const string DATASOURCE_TYPE_MSSQL = "msSql";
        const string DATASOURCE_TYPE_POSTGRESQL = "postgreSql";

        public ISource Create(JToken dataSource)
        {
            return Create(dataSource["name"].ToString(), dataSource["type"].ToString(), dataSource["data"]);
        }

        public ISource Create(string name, string type, JToken data)
        {
            switch (type)
            {
                case DATASOURCE_TYPE_MSSQL:
                    return new MsSqlSource(data["connectionString"].ToString()) { Name = name };
                case DATASOURCE_TYPE_POSTGRESQL:
                    return new NpgsqlSource(data["connectionString"].ToString()) { Name = name };
            }
            return null;
        }
    }
}
