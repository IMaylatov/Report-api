namespace SofTrust.Report.Business.Service.DataSet
{
    using SofTrust.Report.Business.Service.DataSet.Reader;

    public abstract class DataSet
    {
        public string Name { get; set; }
        public abstract IDataSetReader ExecuteReader();
    }
}
