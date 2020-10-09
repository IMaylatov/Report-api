namespace SofTrust.Report.Core.Generator.Report.ClosedXml
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using System.IO;
    using System.Collections.Generic;
    using ClosedXML.Report;
    using SofTrust.Report.Core.Generator.Source;
    using SofTrust.Report.Core.Generator.DataReader;

    public class ClosedXmlReportGenerator : XlsxReportGenerator
    {
        private readonly SourceFactory dataSourceFactory;
        private readonly DataReaderFactory dataSetFactory;

        public ClosedXmlReportGenerator(SourceFactory dataSourceFactory,
            DataReaderFactory dataSetFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
            this.dataSetFactory = dataSetFactory;
        }

        public override FileStreamResult Generate(JToken jReport, Stream bookStream)
        {
            var parameters = this.GetParameters(jReport["variables"]);

            var dataSources = jReport["dataSources"].Select(x => dataSourceFactory.Create(x));

            var dataSets = jReport["dataSets"].Select(x => dataSetFactory.Create(x, dataSources, parameters));

            var datas = this.GetDatas(dataSets);

            var reportStream = this.GenerateClosedXmlReport(bookStream, datas);

            return this.GetXlsxFileStreamResult(reportStream);
        }

        private Stream GenerateClosedXmlReport(Stream bookStream, Dictionary<string, List<Dictionary<string, object>>> datas)
        {
            var template = new XLTemplate(bookStream);

            template.AddVariable(datas);

            template.Generate();

            var reportStream = new MemoryStream();
            template.SaveAs(reportStream);
            reportStream.Position = 0;

            return reportStream;
        }
    }
}
