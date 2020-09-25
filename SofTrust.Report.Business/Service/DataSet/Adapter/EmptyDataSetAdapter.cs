namespace SofTrust.Report.Business.Service.DataAdapter
{
    public class EmptyDataSetAdapter : IDataSetAdapter
    {
        public object Adapt(object adaptee)
        {
            return adaptee;
        }
    }
}
