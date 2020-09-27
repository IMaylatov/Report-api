﻿namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using SofTrust.Report.Business.Service.DataSet;
    using ClosedXML.Report;
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
                    foreach (JProperty field in parameter["value"])
                    {
                        parameters.Add(new Parameter { Name = $"{parameterName}.{field.Name}", Value = field.Value });
                    }
                }
                else
                {
                    parameters.Add(new Parameter { Name = $"{parameterName}", Value = parameter["value"] });
                }
            }
            return parameters;
        }

        protected Dictionary<string, List<Dictionary<string, object>>> GetDatas(IEnumerable<IDataSet> dataSets)
        {
            return dataSets
               .ToDictionary(x => x.Name.ToLower(), x =>
               {
                   var reader = x.ExecuteReader();
                   var datas = new List<Dictionary<string, object>>();
                   while (reader.Read())
                   {
                       var data = new Dictionary<string, object>();
                       for (int i = 0; i < reader.FieldCount; i++)
                       {
                           var fieldName = reader.GetName(i);
                           if (string.IsNullOrWhiteSpace(fieldName))
                           {
                               fieldName = $"a{i}";
                           }
                           data.Add(data.ContainsKey(fieldName) ? $"{fieldName}{i}" : fieldName, reader.GetValue(i));
                       }
                       datas.Add(data);
                   }

                   return datas;
               });
        }

        protected Stream GenerateClosedXmlReport(Stream bookStream, Dictionary<string, List<Dictionary<string, object>>> datas)
        {
            var template = new XLTemplate(bookStream);

            template.AddVariable(datas);

            template.Generate();

            var reportStream = new MemoryStream();
            template.SaveAs(reportStream);
            reportStream.Position = 0;

            return reportStream;
        }

        protected FileStreamResult GetXlsxFileStreamResult(Stream stream)
        {
            return new FileStreamResult(stream, "application/octet-stream") { FileDownloadName = $"report.xlsx" };
        }
    }
}
