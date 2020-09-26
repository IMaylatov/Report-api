namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using SofTrust.Report.Business.Model;
    using SofTrust.Report.Business.Service.DataSource;
    using SofTrust.Report.Business.Service.DataSet;
    using System.IO;
    using MoreLinq;
    using System.Collections.Generic;
    using ClosedXML.Report;

    public class ClosedXmlReportGenerator : IReportGenerator
    {
        private readonly DataSourceFactory dataSourceFactory;
        private readonly DataSetFactory dataSetFactory;

        public ClosedXmlReportGenerator(DataSourceFactory dataSourceFactory, 
            DataSetFactory dataSetFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
            this.dataSetFactory = dataSetFactory;
        }

        public FileStreamResult Generate(JToken jReport, Stream templateStream)
        {
            var parameters = jReport["parameters"]
                .Select(x => new Parameter { Name = x["name"].ToString(), Kind = x["kind"].ToString(), Value = x["value"] });

            var dataSources = jReport["dataSources"].Select(x => dataSourceFactory.Create(x));

            var dataSets = jReport["dataSets"].Select(x => dataSetFactory.Create(x, dataSources));

            var datas = dataSets
                .ToDictionary(x => x.Name, x =>
                {
                    var reader = x.ExecuteReader(parameters);
                    var datas = new List<Dictionary<string, object>>();
                    while (reader.Read())
                    {
                        var data = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            if (string.IsNullOrWhiteSpace(fieldName))
                            {
                                fieldName = $"a{i}";
                            }
                            data.Add(data.ContainsKey(fieldName) ? $"{fieldName}{i}" : fieldName, reader.GetValue(i));
                        }
                        datas.Add(data);
                    }

                    return datas;
                });

            var template = new XLTemplate(templateStream);

            template.AddVariable(datas);

            template.Generate();

            var reportStream = new MemoryStream();
            template.SaveAs(reportStream);
            reportStream.Position = 0;

            return new FileStreamResult(reportStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
        }
    }
}
