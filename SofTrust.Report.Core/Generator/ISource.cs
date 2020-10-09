namespace SofTrust.Report.Core.Generator
{
    public interface ISource
    {
        string Name { get; set; }

        IConnection CreateConnection();
    }
}
