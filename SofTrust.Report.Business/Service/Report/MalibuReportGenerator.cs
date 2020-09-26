namespace SofTrust.Report.Business.Service.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using ClosedXML.Excel;
    using ClosedXML.Report;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Model;
    using SofTrust.Report.Business.Model.Malibu;
    using SofTrust.Report.Business.Service.DataSet;
    using SofTrust.Report.Business.Service.DataSource;

    public class MalibuReportGenerator : IReportGenerator
    {
        private const string DEFAULT_DATASOURCE_NAME = "DataSource";

        private ClosedXmlReportGenerator closedXmlReportService;

        public MalibuReportGenerator(ClosedXmlReportGenerator closedXmlReportService)
        {
            this.closedXmlReportService = closedXmlReportService;
        }

        public FileStreamResult Generate(JToken jReport, Stream template)
        {
            var parameters = jReport["parameters"]
                .Select(x => new Parameter { Name = x["name"].ToString().ToLower(), Kind = x["kind"].ToString(), Value = x["value"] });

            var report = this.GetReport(template);
            var reportDesc = report.DeserializeReportDesc();
            var reportTemplateBook = report.DeserializeReportTemplate();

            var dataSources = new List<IDataSource>()
            {
                new MsSqlDataSource(jReport["connection"]["connectionString"].ToString()) { Name = DEFAULT_DATASOURCE_NAME }
            };

            var dataSets = reportDesc.DATASET
                .Select(x => new SqlQueryDataSet(dataSources.FirstOrDefault(), x.SQL) { Name = x.NAME });

            var datas = dataSets
               .ToDictionary(x => x.Name.ToLower(), x =>
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

            var reportStream = GenerateReport(parameters, datas, reportTemplateBook);

            return new FileStreamResult(reportStream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
        }

        private Report GetReport(Stream template)
        {
            var serializer = new XmlSerializer(typeof(Report));
            return serializer.Deserialize(template) as Report;
        }

        private Stream GenerateReport(IEnumerable<Parameter> parameters, Dictionary<string, List<Dictionary<string, object>>> datas, XLWorkbook book)
        {
            var documentParameter = this.GetDocumentParameter(parameters);

            foreach (var sheet in book.Worksheets)
            {
                var dataSetTemplates = new Dictionary<string, DataSetTemplate>();

                var rows = sheet.RowsUsed();
                foreach(var row in rows)
                {
                    var cells = row.CellsUsed();
                    foreach(var cell in cells)
                    {
                        if (cell.HasComment)
                        {
                            var dataSetNameMatch = new Regex(@"(?i)!--(\w*)\.RecordNumber(?-i)").Match(cell.Comment.Text);
                            if (dataSetNameMatch.Success && !dataSetTemplates.ContainsKey(dataSetNameMatch.Groups[1].Value.ToLower()))
                            {
                                dataSetTemplates.Add(dataSetNameMatch.Groups[1].Value.ToLower(), new DataSetTemplate { Address = cell.Address, RecordNumber = true });
                            }
                            else
                            {
                                dataSetNameMatch = new Regex(@"(?i)&&tab\.(\w*)(?-i)").Match(cell.Comment.Text);
                                if (dataSetNameMatch.Success && !dataSetTemplates.ContainsKey(dataSetNameMatch.Groups[1].Value.ToLower()))
                                {
                                    dataSetTemplates.Add(dataSetNameMatch.Groups[1].Value.ToLower(), new DataSetTemplate { Address = cell.Address });
                                }
                            }

                            cell.Comment.Delete();
                        }
                        string value;
                        cell.TryGetValue(out value);
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            var matches = new Regex(@"(?i)(&&Param\.Document\.\w*)(?-i)").Matches(value);
                            if (matches.Count > 0)
                            {
                                foreach (Match match in matches)
                                    value = value.Replace(match.Value, documentParameter[match.Value.Substring(17).ToLower()].ToString());
                                cell.SetValue(value);
                            }
                        }
                    }
                }

                foreach (var dataSetTemplateElement in dataSetTemplates)
                {
                    var data = datas[dataSetTemplateElement.Key.ToLower()] as List<Dictionary<string, object>>;
                    if (data.Count > 0)
                    {
                        var dataSetTemplate = dataSetTemplateElement.Value;
                        var address = dataSetTemplate.Address;
                        var startDataSet = address;
                        if (dataSetTemplate.RecordNumber)
                        {
                            var cell = sheet.Cell(address);
                            cell.Value = "{{index+1}}";
                            address = sheet.Cell(address.RowNumber, address.ColumnNumber + 1).Address;
                        }
                        var dataHeader = data.FirstOrDefault();
                        foreach(var column in dataHeader)
                        {
                            var cell = sheet.Cell(address);
                            cell.Value = $"{{{{item.{column.Key}}}}}";
                            address = sheet.Cell(address.RowNumber, address.ColumnNumber + 1).Address;
                        }
                        var endDataSet = sheet.Cell(address.RowNumber, address.ColumnNumber - 1).Address;
                        sheet.Range($"{startDataSet}:{endDataSet}").AddToNamed(dataSetTemplateElement.Key);
                    }
                }
            }

            var bookStream = new MemoryStream();
            book.SaveAs(bookStream);
            bookStream.Position = 0;
            var template = new XLTemplate(bookStream);

            template.AddVariable(datas);

            template.Generate();

            var reportStream = new MemoryStream();
            template.SaveAs(reportStream);
            reportStream.Position = 0;
            return reportStream;
        }

        private JToken GetDocumentParameter(IEnumerable<Parameter> parameters)
        {
            var documentParameter = parameters.FirstOrDefault(x => x.Name == "document");
            if (documentParameter != null)
            {
                return JToken.Parse(documentParameter.Value.ToString().ToLower());
            }
            return null;
        }
    } 
}
