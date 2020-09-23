namespace SofTrust.Report.Business.Service.Template.Command
{
    using SofTrust.Report.Business.Service.DataAdapter;
    using System.Collections.Generic;
    using System.IO;

    public interface ITemplateCommand
    {
        Stream Execute(Dictionary<string, string> parameters, object datas);
    }
}
