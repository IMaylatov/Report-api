﻿namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.Extensions.DependencyInjection;
    using SofTrust.Report.Business.Service.Report.ClosedXml;
    using SofTrust.Report.Business.Service.Report.Malibu;
    using System;

    public class ReportGeneratorFactory
    {
        private const string TEMPLATE_TYPE_CLOSEDXML = "ClosedXml";
        private const string TEMPLATE_TYPE_MALIBU = "Malibu";

        private readonly IServiceProvider provider;

        public ReportGeneratorFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public IReportGenerator Create(string type)
        {
            switch (type)
            {
                case TEMPLATE_TYPE_CLOSEDXML:
                    return this.provider.GetService<ClosedXmlReportGenerator>();
                case TEMPLATE_TYPE_MALIBU:
                    return this.provider.GetService<MalibuReportGenerator>();
            }
            return null;
        }
    }
}
