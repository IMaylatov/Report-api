namespace SofTrust.Report.Business.Service.Template.Command
{
    using ClosedXML.Report;
    using Microsoft.AspNetCore.Http;
    using SofTrust.Report.Business.Service.DataAdapter;
    using System.Collections.Generic;
    using System.IO;

    public class ClosedXmlTemplateCommand : ITemplateCommand
    {
        private readonly IFormFile templateFile;

        public ClosedXmlTemplateCommand(IFormFile templateFile)
        {
            this.templateFile = templateFile;
        }

        public Stream Execute(Dictionary<string, string> parameters, object datas)
        {
            using (var templateStream = this.templateFile.OpenReadStream())
            {
                var template = new XLTemplate(templateStream);

                template.AddVariable(datas);

                template.Generate();

                var reportStream = new MemoryStream();
                template.SaveAs(reportStream);
                reportStream.Position = 0;
                return reportStream;
            }
        }
    }
}
