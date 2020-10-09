namespace SofTrust.Report.Core.Generator.Report.Malibu
{
    using System.Collections.Generic;

    internal class DataGroupWrapper
    {
        public Dictionary<string, object> Data { get; set; }

        public IEnumerable<DataGroupWrapper> Groups { get; set; }
    }
}
