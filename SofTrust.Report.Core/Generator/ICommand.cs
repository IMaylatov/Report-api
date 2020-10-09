﻿namespace SofTrust.Report.Core.Generator
{
    using System.Collections.Generic;

    public interface ICommand
    {
        IConnection Connection { get; set; }
        IData ExecuteReader();
        void AddParameters(IEnumerable<Parameter> parameters);
        int Timeout { set; }
    }
}
