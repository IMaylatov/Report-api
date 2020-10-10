namespace SofTrust.Report.Core.Generator
{
    public interface ISource
    {
        IConnection CreateConnection();
    }
}
