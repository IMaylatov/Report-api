namespace SofTrust.Report.Core.Generator.Source
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Core.Generator.Source.Sql;

    public class SourceFactory
    {
        private SqlSourceFactory sqlSourceFactory;

        public SourceFactory(SqlSourceFactory sqlSourceFactory)
        {
            this.sqlSourceFactory = sqlSourceFactory;
        }

        public ISource Create(JToken dataSource, JToken reportContext)
        {
            switch (dataSource["type"].ToString())
            {
                case SqlSourceFactory.DATASOURCE_TYPE_MSSQL:
                case SqlSourceFactory.DATASOURCE_TYPE_POSTGRESQL:
                    return sqlSourceFactory.Create(dataSource, reportContext);
            }

            return null;
        }
    }
}
