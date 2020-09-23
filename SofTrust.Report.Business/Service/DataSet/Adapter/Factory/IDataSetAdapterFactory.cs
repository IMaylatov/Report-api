namespace SofTrust.Report.Business.Service.DataAdapter.Factory
{
    public interface IDataSetAdapterFactory
    {
        IDataSetAdapter Create(string templateType, string dataSetType);
    }
}
