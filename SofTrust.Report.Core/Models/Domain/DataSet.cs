namespace SofTrust.Report.Core.Models.Domain
{
    using Mapster;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Core.Models.Dto;
    using System.Collections.Generic;

    public class DataSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public int ReportId { get; set; }

        public Report Report { get; set; }

        public DataSetDto AdaptToDto()
        {
            var dataSetDto = this.Adapt<DataSetDto>();
            dataSetDto.Data = this.Data != null ? JObject.Parse(this.Data) : null;
            return dataSetDto;
        }
    }
}
