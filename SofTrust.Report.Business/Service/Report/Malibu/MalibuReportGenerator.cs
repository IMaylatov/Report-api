namespace SofTrust.Report.Business.Service.Report.Malibu
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
    using SofTrust.Report.Business.Service.DataSet;
    using SofTrust.Report.Business.Service.DataSource;
    using SofTrust.Report.Service;
    using Microsoft.Extensions.Configuration;
    using System;

    public class MalibuReportGenerator : XlsxReportGenerator
    {
        private const string DEFAULT_DATASOURCE_NAME = "DataSource";
        private const string DATASET_INDEX = "{{index}}";

        private Regex COMMENT_DATASET_NAME_REGEX = new Regex(@"(?i)&&tab\.(\w*)(?-i)");
        private Regex COMMENT_DATASET_RECORDNUMBER_REGEX = new Regex(@"(?i)!--(\w*)\.RecordNumber(?-i)");
        private Regex PARAM_REGEX = new Regex(@"(?i)(&&Param\.[\.\w]*)(?-i)");
        private Regex FORMULA_REGION_REGEX = new Regex(@"([A-Z][0-9]+)");
        private Regex FORMULA_REGION_ROW_REGEX = new Regex(@"([0-9]+)");

        private readonly int timeout;

        private readonly DataSourceFactory dataSourceFactory;

        public MalibuReportGenerator(IConfiguration configuration,
            DataSourceFactory dataSourceFactory)
        {
            this.timeout = int.Parse(configuration["XlsxReport:DataSet:SqlQuery:CommandTimeout"]);
            this.dataSourceFactory = dataSourceFactory;
        }

        public override FileStreamResult Generate(JToken jReport, Stream bookStream)
        {
            var parameters = this.GetParameters(jReport["variables"]);

            var dataSources = jReport["dataSources"].Select(x => dataSourceFactory.Create(x));
            var dataSource = dataSources.FirstOrDefault();

            var report = this.GetReport(bookStream);
            var reportDesc = report.DeserializeReportDesc();
            var reportBook = report.DeserializeReportTemplate();

            var dataSets = reportDesc.DATASET.Select(x => new SqlQueryDataSet(dataSource, x.SQL, parameters, timeout) { Name = x.NAME });

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
            var shiftRanges = 0;
            book.Worksheets
                .ForEach(sheet => sheet.RowsUsed()
                    .ForEach(row => row.CellsUsed()
                        .ForEach(cell => this.ConvertCellMalibuToClosedXml(cell, parameters, datas, dataSetDescs, ref shiftRanges))));
        }

        private void ConvertCellMalibuToClosedXml(IXLCell cell, IEnumerable<Parameter> parameters, Dictionary<string, List<Dictionary<string, object>>> datas,
            MAINDATASET[] dataSetDescs, ref int shiftRanges)
        {
            string cellValue;
            if (cell.TryGetValue(out cellValue) && !string.IsNullOrWhiteSpace(cellValue))
            {
                HandleCellValue(parameters, cell, cellValue);
            }

            if (cell.HasComment)
            {
                HandleCellComment(cell, datas, dataSetDescs, ref shiftRanges);
            }

            if (cell.HasFormula)
            {
                HandleCellFormula(cell, shiftRanges);
            }
        }

        private void HandleCellValue(IEnumerable<Parameter> parameters, IXLCell cell, string cellValue)
        {
            var matches = PARAM_REGEX.Matches(cellValue);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var parameter = parameters.FirstOrDefault(x => match.Value.Contains(x.Name, System.StringComparison.InvariantCultureIgnoreCase));
                    switch (parameter.Value.Type)
                    {
                        case JTokenType.Date:
                            cellValue = cellValue.Replace(match.Value, DateTime.Parse(parameter.Value.ToString()).ToShortDateString());
                            break;
                        default:
                            cellValue = cellValue.Replace(match.Value, parameter.Value.ToString());
                            break;
                    }
                }
                cell.SetValue(cellValue);
            }
        }

        private void HandleCellComment(IXLCell cell, Dictionary<string, List<Dictionary<string, object>>> datas, MAINDATASET[] dataSetDescs, ref int shiftRanges)
        {
            var commentMatch = COMMENT_DATASET_NAME_REGEX.Match(cell.Comment.Text);
            if (commentMatch.Success)
            {
                var dataName = commentMatch.Groups[1].Value.ToLower();
                if (datas.ContainsKey(dataName))
                {
                    this.WriteTemplateData(cell, datas[dataName],
                        dataSetDescs.FirstOrDefault(x => x.NAME.Contains(dataName, System.StringComparison.InvariantCultureIgnoreCase)), ref shiftRanges);
                }
            }
            else if (COMMENT_DATASET_RECORDNUMBER_REGEX.Match(cell.Comment.Text).Success)
            {
                cell.Value = DATASET_INDEX;
            }
            cell.Comment.Delete();
        }

        private void HandleCellFormula(IXLCell cell, int shiftRanges)
        {
            var formulaMatches = FORMULA_REGION_REGEX.Matches(cell.FormulaA1);
            foreach (Match formulaMatch in formulaMatches)
            {
                var rowMatch = int.Parse(FORMULA_REGION_ROW_REGEX.Match(formulaMatch.Value).Value);
                var newAddress = formulaMatch.Value.Replace(rowMatch.ToString(), (shiftRanges + rowMatch).ToString());
                cell.FormulaA1 = cell.FormulaA1.Replace(formulaMatch.Value, newAddress);
            }
        }

        private void WriteTemplateData(IXLCell cell, List<Dictionary<string, object>> datas, MAINDATASET dataSetDesc, ref int shiftRanges)
        {
            if (datas.Count > 1)
            {
                shiftRanges = shiftRanges + datas.Count - 1;
                cell.WorksheetRow().InsertRowsBelow(datas.Count - 1);
            }
            var rowNumber = cell.Address.ColumnNumber > 1 && cell.CellLeft().Value.ToString() == DATASET_INDEX ? 1 : 0;

            if (dataSetDesc.GROUP?.Length > 0 && datas.Count > 1)
            {
                var groupFields = new Stack<string>(dataSetDesc.GROUP.Select(x => x.GroupField));
                int countGroup = 0;
                var groupDatas = GetGroupDatas(datas.Select(x => new DataGroupWrapper { Data = x }), groupFields, ref countGroup).ToList();

                shiftRanges = shiftRanges + countGroup - dataSetDesc.GROUP.Length;
                cell.WorksheetRow().InsertRowsBelow(countGroup + 1);

                var beginGroup = cell;

                var fieldIndexes = datas.First()
                    .Select((x, i) => new { field = x.Key, index = i })
                    .ToDictionary(x => x.field, x => x.index);
                this.WriteGroupDatas(groupDatas, dataSetDesc.GROUP, 0, fieldIndexes, ref rowNumber, ref cell);

                var groupCell = GroupData(dataSetDesc.GROUP, 0, fieldIndexes, cell, beginGroup);
                groupCell.Value = "Общий итог";
            }
            else
            {
                this.WriteRowData(datas, ref rowNumber, ref cell);
            }
        }

        private IEnumerable<DataGroupWrapper> GetGroupDatas(IEnumerable<DataGroupWrapper> dataWrappers, Stack<string> groupFields, ref int countGroup)
        {
            if (groupFields.Count > 0)
            {
                var groupDatas = dataWrappers.GroupBy(x => x.Data, new DataComparer(groupFields))
                    .Select(x => new DataGroupWrapper { Data = x.First().Data, Groups = x.ToList() }).ToList();
                groupFields.Pop();
                countGroup += groupDatas.Count();
                return this.GetGroupDatas(groupDatas, groupFields, ref countGroup);
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

                var groupCell = GroupData(groups, indexGroup, fieldIndexes, cell, beginGroup);
                groupCell.Value = $"{groupCell.CellAbove(groups.Length - indexGroup).Value} Итог";

                cell = cell.CellBelow();
            }
        }

        private IXLCell GroupData(MAINDATASETGROUP[] groups, int indexGroup, Dictionary<string, int> fieldIndexes, IXLCell cell, IXLCell beginGroup)
        {
            var groupCell = this.WriteGroupCell(beginGroup, groups, indexGroup, fieldIndexes, cell);

            cell.Worksheet.Rows(beginGroup.Address.RowNumber, cell.Address.RowNumber - 1).Group();

            return groupCell;
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
}
