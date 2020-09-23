namespace SofTrust.Report.Business.Service.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using MoreLinq;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Service.DataAdapter.Factory;
    using SofTrust.Report.Business.Service.DataSet;
    using SofTrust.Report.Business.Service.DataSource;
    using SofTrust.Report.Business.Service.Template;

    public class ReportService : IReportService
    {
        private readonly IDataSourceFactory dataSourceFactory;
        private readonly IDataSetFactory dataSetFactory;
        private readonly ITemplateFactory templateFactory;
        private readonly IDataSetAdapterFactory dataAdapterFactory;

        public ReportService(IDataSourceFactory dataSourceFactory,
            IDataSetFactory dataSetFactory,
            ITemplateFactory templateFactory,
            IDataSetAdapterFactory dataAdapterFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
            this.dataSetFactory = dataSetFactory;
            this.templateFactory = templateFactory;
            this.dataAdapterFactory = dataAdapterFactory;
        }

        public Stream Run(string templateType, IFormFile templateFile, JArray jParameters, JArray jDataSources, JArray jDataSets)
        {
            var parameters = jParameters.Children()
                .ToDictionary(x => x["name"].ToString(), x => x["value"].ToString());

            var dataSources = jDataSources.Children()
                .ToDictionary(x => x["name"].ToString(), x => dataSourceFactory.Create(x));

            var dataSets = jDataSets.Children()
                .ToDictionary(x => x["name"].ToString(), x => dataSetFactory.Create(x));

            var dataSetAdaptres = jDataSets.Children()
                .ToDictionary(x => x["name"].ToString(), x => dataAdapterFactory.Create(templateType, x["type"].ToString()));

            var datas = dataSets
                .ToDictionary(x => x.Key, x => x.Value.Execute(parameters, dataSources, dataSetAdaptres[x.Key]));

            return templateFactory.Create(templateType, templateFile)
                .Execute(parameters, datas);
        }
    }
}
