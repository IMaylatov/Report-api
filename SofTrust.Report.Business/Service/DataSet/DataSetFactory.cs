namespace SofTrust.Report.Business.Service.DataSet
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Service.DataSource;
    using System.Collections.Generic;
    using System.Linq;

    public class DataSetFactory
    {
        private const string DATASET_TYPE_SQLQUERY = "SqlQuery";

        public IDataSet Create(JToken dataSetJ, IEnumerable<IDataSource> dataSources)
        {
            IDataSet dataSet = null;
            switch (dataSetJ["type"].ToString())
            {
                case DATASET_TYPE_SQLQUERY:
                    {
                        var name = dataSetJ["name"].ToString();
                        var dataSourceName = dataSetJ["data"]["dataSourceName"].ToString();
                        var query = dataSetJ["data"]["query"].ToString();
                        dataSet = new SqlQueryDataSet(dataSources.FirstOrDefault(x => x.Name == dataSourceName) , query) { Name = name };
                    }
                    break;
                default:
                    break;
            }
            return dataSet;
        }
    }
}
