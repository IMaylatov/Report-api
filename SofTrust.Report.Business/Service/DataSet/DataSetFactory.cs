namespace SofTrust.Report.Business.Service.DataSet
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Service.DataSet.Command;

    public class DataSetFactory : IDataSetFactory
    {
        private const string DATASET_TYPE_SQLQUERY = "SqlQuery";

        public IDataSetCommand Create(JToken dataSet)
        {
            IDataSetCommand dataSetCommand = null;
            switch (dataSet["type"].ToString())
            {
                case DATASET_TYPE_SQLQUERY:
                    {
                        var dataSourceName = dataSet["data"]["dataSourceName"].ToString();
                        var query = dataSet["data"]["query"].ToString();
                        dataSetCommand = new SqlQueryDataSetCommand(dataSourceName, query);
                    }
                    break;
                default:
                    break;
            }
            return dataSetCommand;
        }
    }
}
