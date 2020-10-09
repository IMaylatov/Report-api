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

        public List<ReportDataSet> ReportDataSets { get; set; }

        public DataSetDto AdaptToDto()
        {
            var dataSetDto = this.Adapt<DataSetDto>();
            dataSetDto.Data = JObject.Parse(this.Data);
            return dataSetDto;
        }
    }
}
