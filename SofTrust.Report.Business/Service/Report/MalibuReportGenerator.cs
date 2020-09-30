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
    using System.Diagnostics.CodeAnalysis;

    public class MalibuReportGenerator : XlsxReportGenerator
    {
        private const string DEFAULT_DATASOURCE_NAME = "DataSource";
        private const string DATASET_INDEX = "{{index}}";

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

            FillBookData(parameters, datas, reportBook, reportDesc.DATASET);

            var reportStream = new MemoryStream();
            reportBook.SaveAs(reportStream);
            reportStream.Position = 0;

            return this.GetXlsxFileStreamResult(reportStream);
        }

        private Report GetReport(Stream template)
        {
            var serializer = new XmlSerializer(typeof(Report));
            return serializer.Deserialize(template) as Report;
        }

        private void FillBookData(IEnumerable<Parameter> parameters, Dictionary<string, List<Dictionary<string, object>>> datas, XLWorkbook book, MAINDATASET[] dataSetDescs)
        {
            book.Worksheets
                .ForEach(sheet => sheet.RowsUsed()
                    .ForEach(row => row.CellsUsed()
                        .ForEach(cell => this.ConvertCellMalibuToClosedXml(cell, parameters, datas, dataSetDescs))));
        }

        private void ConvertCellMalibuToClosedXml(IXLCell cell, IEnumerable<Parameter> parameters, Dictionary<string, List<Dictionary<string, object>>> datas, MAINDATASET[] dataSetDescs)
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
                        this.WriteTemplateData(cell, datas[dataName], dataSetDescs.FirstOrDefault(x => x.NAME.Contains(dataName, System.StringComparison.InvariantCultureIgnoreCase)));
                    }
                }
                else
                {
                    if (COMMENT_DATASET_RECORDNUMBER_REGEX.Match(cell.Comment.Text).Success)
                    {
                        cell.Value = DATASET_INDEX;
                    }
                }
                cell.Comment.Delete();
            }
        }

        private void WriteTemplateData(IXLCell cell, List<Dictionary<string, object>> datas, MAINDATASET dataSetDesc)
        {
            if (datas.Count > 1)
            {
                cell.WorksheetRow().InsertRowsBelow(datas.Count - 1);
            }
            var rowNumber = cell.Address.ColumnNumber > 1 && cell.CellLeft().Value.ToString() == DATASET_INDEX ? 1 : 0;

            if (dataSetDesc.GROUP?.Length > 0)
            {
                var groupFields = new Stack<string>(dataSetDesc.GROUP.Select(x => x.GroupField));
                int countGroup = 0;
                var groupDatas = GroupDatas(datas.Select(x => new DataGroupWrapper { Data = x }), groupFields, ref countGroup).ToList();

                cell.WorksheetRow().InsertRowsBelow(countGroup + 1);

                var beginGroup = cell;

                var fieldIndexes = datas.First()
                    .Select((x, i) => new { field = x.Key, index = i })
                    .ToDictionary(x => x.field, x => x.index);
                this.WriteGroupDatas(groupDatas, dataSetDesc.GROUP, 0, fieldIndexes, ref rowNumber, ref cell);

                var groupCell = this.WriteGroupCell(beginGroup, dataSetDesc.GROUP, 0, fieldIndexes, cell);
                groupCell.Value = "Общий итог";

                cell.Worksheet.Rows(beginGroup.Address.RowNumber, cell.Address.RowNumber - 1).Group();
            }
            else
            {
                this.WriteRowData(datas, ref rowNumber, ref cell);
            }
        }

        private IEnumerable<DataGroupWrapper> GroupDatas(IEnumerable<DataGroupWrapper> dataWrappers, Stack<string> groupFields, ref int countGroup)
        {
            if (groupFields.Count > 0)
            {
                var groupDatas = dataWrappers.GroupBy(x => x.Data, new DataComparer(groupFields))
                    .Select(x => new DataGroupWrapper { Data = x.First().Data, Groups = x.ToList() }).ToList();
                groupFields.Pop();
                countGroup += groupDatas.Count();
                return this.GroupDatas(groupDatas, groupFields, ref countGroup);
            }
            else
            {
                return dataWrappers;
            }
        }

        private void WriteGroupDatas(IEnumerable<DataGroupWrapper> dataWrappers, MAINDATASETGROUP[] groups, int indexGroup,
            Dictionary<string, int> fieldIndexes, ref int rowNumber, ref IXLCell cell)
        {
            foreach (var dataWrapper in dataWrappers)
            {
                var beginGroup = cell;

                if (groups.Length - 1 > indexGroup)
                {
                    this.WriteGroupDatas(dataWrapper.Groups, groups, indexGroup + 1, fieldIndexes, ref rowNumber, ref cell);
                }
                else
                {
                    this.WriteRowData(dataWrapper.Groups.Select(x => x.Data), ref rowNumber, ref cell);
                }

                var groupCell = this.WriteGroupCell(beginGroup, groups, indexGroup, fieldIndexes, cell);
                groupCell.Value = $"{groupCell.CellAbove(groups.Length - indexGroup).Value} Итог";

                cell.Worksheet.Rows(beginGroup.Address.RowNumber, cell.Address.RowNumber - 1).Group();

                cell = cell.CellBelow();
            }
        }

        private void WriteRowData(IEnumerable<IDictionary<string, object>> datas, ref int rowNumber, ref IXLCell cell)
        {
            foreach (var dataRow in datas)
            {
                if (rowNumber > 0)
                {
                    cell.CellLeft().Value = rowNumber;
                    rowNumber = rowNumber + 1;
                }
                foreach (var dataCell in dataRow)
                {
                    cell.Value = dataCell.Value;
                    cell = cell.CellRight();
                }
                cell = cell.CellLeft(dataRow.Count).CellBelow();
            }
        }

        private IXLCell WriteGroupCell(IXLCell beginGroup, MAINDATASETGROUP[] groups, int indexGroup, Dictionary<string, int> fieldIndexes, IXLCell cell)
        {
            var indexGroupCell = fieldIndexes[groups[indexGroup].GroupField];
            var groupCell = cell.CellRight(indexGroupCell);
            groupCell.Style.Font.Bold = true;

            foreach (var field in groups[indexGroup].FIELD)
            {
                var indexFieldCell = fieldIndexes[field.index];
                var fieldCell = cell.CellRight(indexFieldCell);
                var subTotalOperation = this.SubTotalOperation(field.function);
                fieldCell.FormulaA1 = $"SUBTOTAL({subTotalOperation},{beginGroup.CellRight(indexFieldCell).Address}:{fieldCell.CellAbove().Address})";
                fieldCell.Style.Font.Bold = true;
            }
            return groupCell;
        }

        private int SubTotalOperation(string function)
        {
            switch (function)
            {
                case "cf_Sum":
                    return 9;
                case "cf_Count":
                    return 3;
                default:
                    return 0;
            }
        }
    }

    internal class DataGroupWrapper
    {
        public Dictionary<string, object> Data { get; set; }

        public IEnumerable<DataGroupWrapper> Groups { get; set; }
    }

    internal class DataComparer : IEqualityComparer<object>
    {
        private IEnumerable<string> fields;

        public DataComparer(IEnumerable<string> fields)
        {
            this.fields = fields;
        }

        public bool Equals([AllowNull] object x, [AllowNull] object y)
        {
            var xD = x as Dictionary<string, object>;
            var yD = y as Dictionary<string, object>;
            return fields.All(f => xD[f].ToString() == yD[f].ToString());
        }

        public int GetHashCode([DisallowNull] object obj)
        {
            var objD = obj as Dictionary<string, object>;
            return string.Join("", fields.Select(x => objD[x])).GetHashCode();
        }
    }
}
