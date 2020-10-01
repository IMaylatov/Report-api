namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using SofTrust.Report.Business.Service.DataSet;
    using SofTrust.Report.Business.Model;

    public abstract class XlsxReportGenerator : IReportGenerator
    {
        public abstract FileStreamResult Generate(JToken report, Stream template);

        protected IEnumerable<Parameter> GetParameters(JToken jParameters)
        {
            var parameters = new List<Parameter>();
            foreach (var parameter in jParameters.Children())
            {
                var parameterName = parameter["name"];
                if (parameter["value"].Type == JTokenType.Object)
                {
                    foreach (JProperty property in parameter["value"])
                    {
                        this.GetParameters(parameterName.ToString(), property, parameters);
                    }
                }
                else
                {
                    parameters.Add(new Parameter { Name = parameterName.ToString(), Value = parameter["value"] });
                }
            }
            return parameters;
        }

        private void GetParameters(string prefix, JProperty parentProperty, List<Parameter> parameters)
        {
            foreach (var property in parentProperty)
            {
                if (property.Type == JTokenType.Object)
                {
                    foreach (JProperty childProperty in property)
                    {
                        this.GetParameters($"{prefix}.{parentProperty.Name}", childProperty, parameters);
                    }
                }
                else
                {
                    parameters.Add(new Parameter { Name = $"{prefix}.{parentProperty.Name}", Value = property });
                }
            }
        }

        protected Dictionary<string, List<Dictionary<string, object>>> GetDatas(IEnumerable<DataSet> dataSets)
        {
            return dataSets
               .ToDictionary(x => x.Name.ToLower(), x =>
               {
                   var reader = x.ExecuteReader();
                   var datas = new List<Dictionary<string, object>>();
                   while (reader.Read())
                   {
                       var data = new Dictionary<string, object>();
                       var unnamedColumnIndex = 1;
                       for (int i = 0; i < reader.FieldCount; i++)
                       {
                           var fieldName = reader.GetName(i);
                           if (string.IsNullOrWhiteSpace(fieldName))
                           {
                               fieldName = $"Column{unnamedColumnIndex++}";
                           }
                           data.Add(data.ContainsKey(fieldName) ? $"{fieldName}{i}" : fieldName, reader.GetValue(i));
                       }
                       datas.Add(data);
                   }

                   return datas;
               });
        }

        protected FileStreamResult GetXlsxFileStreamResult(Stream stream)
        {
            return new FileStreamResult(stream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
        }
    }
}
