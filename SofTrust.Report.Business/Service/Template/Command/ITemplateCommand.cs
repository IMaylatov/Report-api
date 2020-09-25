namespace SofTrust.Report.Business.Service.Template.Command
{
    using SofTrust.Report.Business.Model;
    using System.Collections.Generic;
    using System.IO;

    public interface ITemplateCommand
    {
        Stream Execute(IEnumerable<Parameter> parameters, object datas);
    }
}
