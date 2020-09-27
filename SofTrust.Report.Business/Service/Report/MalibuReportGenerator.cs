namespace SofTrust.Report.Business.Service.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using ClosedXML.Excel;
    using Microsoft.AspNetCore.Mvc;
    using MoreLinq;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Model.Malibu;
    using SofTrust.Report.Business.Service.DataSet;
    using SofTrust.Report.Business.Service.DataSource;
    using SofTrust.Report.Business.Model;

    public class MalibuReportGenerator : XlsxReportGenerator
    {
        private const string DEFAULT_DATASOURCE_NAME = "DataSource";

        private const string DATASET_INDEX_TEMPLATE = "{{index+1}}";

        private Regex COMMENT_DATASET_NAME_REGEX = new Regex(@"(?i)&&tab\.(\w*)(?-i)");
        private Regex COMMENT_DATASET_RECORDNUMBER_REGEX = new Regex(@"(?i)!--(\w*)\.RecordNumber(?-i)");
        private Regex PARAM_REGEX = new Regex(@"(?i)(&&Param\.[\.\w]*)(?-i)");

        public override FileStreamResult Generate(JToken jReport, Stream bookStream)
        {
            var parameters = this.GetParameters(jReport["parameters"]);

            var dataSource = new MsSqlDataSource(jReport["connection"]["connectionString"].ToString()) { Name = DEFAULT_DATASOURCE_NAME };
            var dataSources = new List<IDataSource>() { dataSource };

            var report = this.GetReport(bookStream);
            var reportDesc = report.DeserializeReportDesc();
            var reportBook = report.DeserializeReportTemplate();

            var dataSets = reportDesc.DATASET.Select(x => new SqlQueryDataSet(dataSource, x.SQL, parameters) { Name = x.NAME });

            var datas = this.GetDatas(dataSets);

            var closedXmlBookStream = ConvertBookMalibuToClosedXml(parameters, datas, reportBook);

            var reportStream = this.GenerateClosedXmlReport(closedXmlBookStream, datas);

            return this.GetXlsxFileStreamResult(reportStream);
        }

        private Report GetReport(Stream template)
        {
            var serializer = new XmlSerializer(typeof(Report));
            return serializer.Deserialize(template) as Report;
        }

        private Stream ConvertBookMalibuToClosedXml(IEnumerable<Parameter> parameters, Dictionary<string, List<Dictionary<string, object>>> datas, XLWorkbook book)
        {
            book.Worksheets
                .ForEach(sheet => sheet.RowsUsed()
                    .ForEach(row => row.CellsUsed()
                        .ForEach(cell => this.ConvertCellMalibuToClosedXml(cell, parameters, datas))));

            var bookStream = new MemoryStream();
            book.SaveAs(bookStream);
            bookStream.Position = 0;
            return bookStream;
        }

        private void ConvertCellMalibuToClosedXml(IXLCell cell, IEnumerable<Parameter> parameters, Dictionary<string, List<Dictionary<string, object>>> datas)
        {
            string cellValue;
            if (cell.TryGetValue(out cellValue) && !string.IsNullOrWhiteSpace(cellValue))
            {
                var matches = PARAM_REGEX.Matches(cellValue);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                        cellValue = cellValue.Replace(match.Value, parameters.FirstOrDefault(x => match.Value.Contains(x.Name, System.StringComparison.InvariantCultureIgnoreCase)).Value.ToString());
                    cell.SetValue(cellValue);
                }
            }

            if (cell.HasComment)
            {
                var commentMatch = COMMENT_DATASET_NAME_REGEX.Match(cell.Comment.Text);
                if (commentMatch.Success)
                {
                    var dataName = commentMatch.Groups[1].Value.ToLower();
                    if (datas.ContainsKey(dataName))
                    {
                        this.WriteTemplateData(cell, dataName, datas[dataName]);
                    }
                }
                else
                {
                    commentMatch = COMMENT_DATASET_RECORDNUMBER_REGEX.Match(cell.Comment.Text);
                    if (commentMatch.Success)
                    {
                        cell.Value = DATASET_INDEX_TEMPLATE;
                    }
                }
                cell.Comment.Delete();
            }
        }

        private void WriteTemplateData(IXLCell cell, string dataName, List<Dictionary<string, object>> data)
        {
            var startData = cell.Worksheet.Cell(cell.Address.RowNumber, 1).Address;
            var dataHeader = data.FirstOrDefault();
            foreach (var column in dataHeader)
            {
                cell.Value = $"{{{{item.{column.Key}}}}}";
                cell = cell.CellRight();
            }
            var endData = cell.Worksheet.Cell(cell.Address.RowNumber + 1, cell.Worksheet.LastColumnUsed().ColumnNumber()).Address;
            cell.Worksheet.Range($"{startData}:{endData}").AddToNamed(dataName);
        }
    } 
}
