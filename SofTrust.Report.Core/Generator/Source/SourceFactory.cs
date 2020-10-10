namespace SofTrust.Report.Core.Generator.Source
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Core.Models.Domain;

    public class SourceFactory
    {
        const string DATASOURCE_TYPE_MSSQL = "msSql";
        const string DATASOURCE_TYPE_POSTGRESQL = "postgreSql";

        public ISource Create(JToken dataSource)
        {
            return Create(dataSource["type"].ToString(), dataSource["name"].ToString(), dataSource["data"]);
        }

        public ISource Create(DataSource dataSource)
        {
            var dataJ = JToken.Parse(dataSource.Data);
            return Create(dataSource.Type, dataSource.Name, dataJ);
        }

        private ISource Create(string type, string name, JToken data)
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
