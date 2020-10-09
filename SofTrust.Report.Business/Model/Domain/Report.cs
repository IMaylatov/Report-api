namespace SofTrust.Report.Business.Model.Domain
{
    using Mapster;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Model.Dto;
    using System.Collections.Generic;
    using System.Linq;

    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public ICollection<Template> Templates { get; set; }
        public List<ReportDataSource> ReportDataSources { get; set; }
        public List<ReportDataSet> ReportDataSets { get; set; }
        public List<ReportVariable> ReportVariables { get; set; }


        public ReportDto AdaptToDto()
        {
            var reportDto = this.Adapt<ReportDto>();

            reportDto.DataSources = this.ReportDataSources.Select(x => x.DataSource.AdaptToDto()).ToList();

            reportDto.DataSets = this.ReportDataSets.Select(x => x.DataSet.AdaptToDto()).ToList();

            reportDto.Variables = this.ReportVariables.Select(x => x.Variable.AdaptToDto()).ToList();

            return reportDto;
        }
    }
}
