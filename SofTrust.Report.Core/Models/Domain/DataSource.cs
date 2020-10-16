namespace SofTrust.Report.Core.Models.Domain
{
    using Mapster;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Core.Models.Dto;
    using System.Collections.Generic;

    public class DataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public int ReportId { get; set; }

        public Report Report { get; set; }

        public DataSourceDto AdaptToDto()
        {
            var dataSourceDto = this.Adapt<DataSourceDto>();
            dataSourceDto.Data = this.Data != null ? JObject.Parse(this.Data) : null;
            return dataSourceDto;
        }
    }
}
