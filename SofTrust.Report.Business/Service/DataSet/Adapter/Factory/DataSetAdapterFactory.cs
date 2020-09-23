namespace SofTrust.Report.Business.Service.DataAdapter.Factory
{
    public class DataSetAdapterFactory : IDataSetAdapterFactory
    {
        const string TEMPLATE_TYPE_CLOSEDXML = "ClosedXml";

        private const string DATASET_TYPE_SQLQUERY = "SqlQuery";

        public IDataSetAdapter Create(string templateType, string dataSetType)
        {
            if (templateType == TEMPLATE_TYPE_CLOSEDXML)
            {
                if (dataSetType == DATASET_TYPE_SQLQUERY)
                {
                    return new ClosedXmlSqlQueryAdapter();
                }
            }
            return null;
        }
    }
}
