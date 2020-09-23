namespace SofTrust.Report.Business.Service.Template
{
    using Microsoft.AspNetCore.Http;
    using SofTrust.Report.Business.Service.Template.Command;

    public class TemplateFactory : ITemplateFactory
    {
        const string TEMPLATE_TYPE_CLOSEDXML = "ClosedXml";

        public ITemplateCommand Create(string type, IFormFile templateFile)
        {
            switch (type)
            {
                case TEMPLATE_TYPE_CLOSEDXML:
                    return new ClosedXmlTemplateCommand(templateFile);
            }
            return null;
        }
    }
}
