namespace SofTrust.Report.Business.Model.Dto
{
    using System.Collections.Generic;
    using System.Linq;
    using Mapster;
    using SofTrust.Report.Business.Model.Domain;

    public class ReportDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<DataSourceDto> DataSources { get; set; }
        public ICollection<DataSetDto> DataSets { get; set; }
        public ICollection<VariableDto> Variables { get; set; }

        public Report AdaptToDomain()
        {
            var report = this.Adapt<Report>();

            report.ReportDataSources = this.DataSources.Select(x =>
            {
                var dataSource = x.Adapt<DataSource>();
                return new ReportDataSource()
                {
                    DataSource = dataSource,
                    DataSourceId = dataSource.Id,
                    Report = report,
                    ReportId = report.Id
                };
            }).ToList();

            report.ReportDataSets = this.DataSets.Select(x =>
            {
                var dataSet = x.Adapt<DataSet>();
                return new ReportDataSet()
                {
                    DataSet = dataSet,
                    DataSetId = dataSet.Id,
                    Report = report,
                    ReportId = report.Id
                };
            }).ToList();

            report.ReportVariables = this.Variables.Select(x =>
            {
                var variable = x.Adapt<Variable>();
                return new ReportVariable()
                {
                    Variable = variable,
                    VariableId = variable.Id,
                    Report = report,
                    ReportId = report.Id
                };
            }).ToList();

            return report;
        }
    }
}
