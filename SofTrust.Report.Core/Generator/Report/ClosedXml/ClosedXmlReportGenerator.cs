namespace SofTrust.Report.Core.Generator.Report.ClosedXml
{
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using System.IO;
    using System.Collections.Generic;
    using ClosedXML.Report;
    using SofTrust.Report.Core.Generator.Source;
    using SofTrust.Report.Core.Generator.DataReader;
    using SofTrust.Report.Core.Generator.DataAdapter;

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

        public override Stream Generate(JToken jReport, Stream bookStream, JToken jVariableValues)
        {
            var variables = this.GetVariables(jReport["variables"], jVariableValues);

            var dataSources = jReport["dataSources"].ToDictionary(x => x["name"].ToString(), x => dataSourceFactory.Create(x));

            var dataSets = jReport["dataSets"].ToDictionary(x => x["name"].ToString(), x => dataSetFactory.Create(x, dataSources, variables));

            var datas = dataSets
               .ToDictionary(x => x.Key.ToLower(), x => x.Value.GetData().ToListDictionaryAdapt());

            var reportStream = this.GenerateClosedXmlReport(bookStream, datas);

            return reportStream;
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
