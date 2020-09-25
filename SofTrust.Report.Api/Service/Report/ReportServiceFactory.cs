namespace SofTrust.Report.Api.Service.Report
{
    using Microsoft.Extensions.DependencyInjection;
    using SofTrust.Report.Business.Service.Report;
    using System;

    public class ReportServiceFactory : IReportServiceFactory
    {
        private const string TEMPLATE_TYPE_CLOSEDXML = "ClosedXml";
        private const string TEMPLATE_TYPE_MALIBU = "Malibu";

        private readonly IServiceProvider provider;

        public ReportServiceFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public IReportService Create(string type)
        {
            switch (type)
            {
                case TEMPLATE_TYPE_CLOSEDXML:
                    return this.provider.GetService<ClosedXmlReportService>();
                case TEMPLATE_TYPE_MALIBU:
                    return this.provider.GetService<MalibuReportService>();
            }
            return null;
        }
    }
}
