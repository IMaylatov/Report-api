namespace SofTrust.Report.Business.Model.Domain
{
    using Mapster;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Business.Model.Dto;
    using System.Collections.Generic;

    public class Variable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }

        public List<ReportVariable> ReportVariables { get; set; }

        internal VariableDto AdaptToDto()
        {
            var variableDto = this.Adapt<VariableDto>();
            variableDto.Data = this.Data != null ? JObject.Parse(this.Data) : null;
            return variableDto;
        }
    }
}
