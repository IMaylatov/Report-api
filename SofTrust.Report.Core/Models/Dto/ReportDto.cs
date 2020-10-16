namespace SofTrust.Report.Core.Models.Dto
{
    using Mapster;
    using SofTrust.Report.Core.Models.Domain;
    using System;
    using System.Collections.Generic;

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
            report.DataSources.ForEach(x => { x.Report = report; x.ReportId = report.Id; });
            report.DataSets.ForEach(x => { x.Report = report; x.ReportId = report.Id; });
            report.Variables.ForEach(x => { x.Report = report; x.ReportId = report.Id; });
            return report;
        }
    }
}
