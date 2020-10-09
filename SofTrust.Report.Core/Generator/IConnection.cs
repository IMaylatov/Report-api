namespace SofTrust.Report.Core.Generator
{
    using System;

    public interface IConnection : IDisposable
    {
        void Open();
        ICommand CreateCommand(object cmd);
    }
}
