namespace SofTrust.Report.Core.Generator
{
    public interface IDataReader
    {
        string Name { get; set; }
        IData CreateReader();
    }
}
