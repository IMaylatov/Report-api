namespace SofTrust.Report.Business.Service.DataSet
{
    using System.Collections.Generic;
    using SofTrust.Report.Business.Model;
    using SofTrust.Report.Business.Service.DataSet.Reader;

    public interface IDataSet
    {
        public string Name { get; set; }
        IDataSetReader ExecuteReader();
    }
}
