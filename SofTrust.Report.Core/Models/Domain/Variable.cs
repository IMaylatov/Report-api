﻿namespace SofTrust.Report.Core.Models.Domain
{
    using Mapster;
    using Newtonsoft.Json.Linq;
    using SofTrust.Report.Core.Models.Dto;
    using System.Collections.Generic;

    public class Variable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public int ReportId { get; set; }

        public Report Report { get; set; }

        internal VariableDto AdaptToDto()
        {
            var variableDto = this.Adapt<VariableDto>();
            variableDto.Data = this.Data != null ? JObject.Parse(this.Data) : null;
            return variableDto;
        }
    }
}
