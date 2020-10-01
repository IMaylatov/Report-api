namespace SofTrust.Report.Business.Model.Domain
{
    using Mapster;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Model.Dto;
    using System.Collections.Generic;

    public class DataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }

        public List<ReportDataSource> ReportDataSources { get; set; }

        public DataSourceDto AdaptToDto()
        {
            var dataSourceDto = this.Adapt<DataSourceDto>();
            dataSourceDto.Data = JObject.Parse(this.Data);
            return dataSourceDto;
        }
    }
}
