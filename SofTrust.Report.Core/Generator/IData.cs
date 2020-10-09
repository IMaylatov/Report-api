namespace SofTrust.Report.Core.Generator
{
    public interface IData
    {
        bool Read();
        int FieldCount { get; }
        string GetName(int i);
        object GetValue(int i);
    }
}
