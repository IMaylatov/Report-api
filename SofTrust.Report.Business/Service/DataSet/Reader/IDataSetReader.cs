namespace SofTrust.Report.Business.Service.DataSet.Reader
{
    public interface IDataSetReader
    {
        bool Read();
        int FieldCount { get; }
        string GetName(int i);
        object GetValue(int i);
    }
}
