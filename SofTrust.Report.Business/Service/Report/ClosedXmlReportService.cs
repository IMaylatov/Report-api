namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using SofTrust.Report.Business.Model;
    using SofTrust.Report.Business.Service.DataSource;
    using SofTrust.Report.Business.Service.DataSet;
    using SofTrust.Report.Business.Service.DataAdapter.Factory;
    using SofTrust.Report.Business.Service.Template;

    public class ClosedXmlReportService : IReportService
    {
        private readonly IDataSourceFactory dataSourceFactory;
        private readonly IDataSetFactory dataSetFactory;
        private readonly IDataSetAdapterFactory dataSetAdapterFactory;
        private readonly ITemplateFactory templateFactory;

        public ClosedXmlReportService(IDataSourceFactory dataSourceFactory, 
            IDataSetFactory dataSetFactory, 
            IDataSetAdapterFactory dataSetAdapterFactory, 
            ITemplateFactory templateFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
            this.dataSetFactory = dataSetFactory;
            this.dataSetAdapterFactory = dataSetAdapterFactory;
            this.templateFactory = templateFactory;
        }

        public FileStreamResult Run(JToken jReport, IFormFile template)
        {
            var parameters = jReport["parameters"]
                .Select(x => new Parameter { Name = x["name"].ToString(), Kind = x["kind"].ToString(), Value = x["value"] });

            var dataSources = jReport["dataSources"]
               .ToDictionary(x => x["name"].ToString(), x => dataSourceFactory.Create(x));

            var dataSets = jReport["dataSets"]
               .ToDictionary(x => x["name"].ToString(), x => dataSetFactory.Create(x));

            var dataSetAdapters = jReport["dataSets"]
                .ToDictionary(x => x["name"].ToString(), x => dataSetAdapterFactory.Create(jReport["type"].ToString(), x["type"].ToString()));

            var datas = dataSets
                .ToDictionary(x => x.Key, x => x.Value.Execute(parameters, dataSources, dataSetAdapters[x.Key]));

            var reportStream = templateFactory.Create(jReport["type"].ToString(), template);

            return new FileStreamResult(reportStream.Execute(parameters, datas), "application/octet-stream") { FileDownloadName = $"report.xlsx" };
        }
    }
}
