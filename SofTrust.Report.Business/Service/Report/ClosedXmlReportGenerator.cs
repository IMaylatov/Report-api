namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using SofTrust.Report.Business.Service.DataSource;
    using SofTrust.Report.Business.Service.DataSet;
    using System.IO;

    public class ClosedXmlReportGenerator : XlsxReportGenerator
    {
        private readonly DataSourceFactory dataSourceFactory;
        private readonly DataSetFactory dataSetFactory;

        public ClosedXmlReportGenerator(DataSourceFactory dataSourceFactory, 
            DataSetFactory dataSetFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
            this.dataSetFactory = dataSetFactory;
        }

        public override FileStreamResult Generate(JToken jReport, Stream bookStream)
        {
            var parameters = this.GetParameters(jReport["parameters"]);

            var dataSources = jReport["dataSources"].Select(x => dataSourceFactory.Create(x));

            var dataSets = jReport["dataSets"].Select(x => dataSetFactory.Create(x, dataSources, parameters));

            var datas = this.GetDatas(dataSets);

            var reportStream = this.GenerateClosedXmlReport(bookStream, datas);

            return this.GetXlsxFileStreamResult(reportStream);
        }
    }
}
