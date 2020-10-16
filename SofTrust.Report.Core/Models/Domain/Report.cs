namespace SofTrust.Report.Core.Models.Domain
{
    using Mapster;
    using SofTrust.Report.Core.Models.Dto;
    using System.Collections.Generic;
    using System.Linq;

    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public ICollection<Template> Templates { get; set; }
        public List<DataSource> DataSources { get; set; }
        public List<DataSet> DataSets { get; set; }
        public List<Variable> Variables { get; set; }


        public ReportDto AdaptToDto()
        {
            var reportDto = this.Adapt<ReportDto>();

            reportDto.DataSources = this.DataSources.Select(x => x.AdaptToDto()).ToList();
            reportDto.DataSets = this.DataSets.Select(x => x.AdaptToDto()).ToList();
            reportDto.Variables = this.Variables.Select(x => x.AdaptToDto()).ToList();

            return reportDto;
        }
    }
}
