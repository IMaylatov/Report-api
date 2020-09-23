namespace SofTrust.Report.Business.Service.DataSet
{
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Service.DataSet.Command;

    public interface IDataSetFactory
    {
        IDataSetCommand Create(JToken dataSet);
    }
}
