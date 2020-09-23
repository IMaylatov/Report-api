namespace SofTrust.Report.Business.Service.Template
{
    using Microsoft.AspNetCore.Http;
    using SofTrust.Report.Business.Service.Template.Command;

    public interface ITemplateFactory
    {
        ITemplateCommand Create(string type, IFormFile templateFile);
    }
}
