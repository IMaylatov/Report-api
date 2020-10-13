namespace SofTrust.Report.Core.Generator.Report
{
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Collections.Generic;

    public abstract class XlsxReportGenerator : IReportGenerator
    {
        public abstract Stream Generate(JToken report, Stream template, JToken variables);

        protected IEnumerable<Variable> GetVariables(JToken jVariables)
        {
            var variables = new List<Variable>();
            foreach (var variable in jVariables.Children())
            {
                if (variable["value"].Type == JTokenType.Object)
                {
                    foreach (JProperty property in variable["value"])
                    {
                        this.GetVariables(variable["name"].ToString(), property, variables);
                    }
                }
                else
                {
                    variables.Add(
                        new Variable {
                            Name = variable["name"].ToString(), 
                            Type = variable["type"].ToString(), 
                            Data = variable["data"], 
                            Value = variable["value"] });
                }
            }
            return variables;
        }

        private void GetVariables(string prefix, JProperty parentProperty, List<Variable> variables)
        {
            foreach (var property in parentProperty)
            {
                if (property.Type == JTokenType.Object)
                {
                    foreach (JProperty childProperty in property)
                    {
                        this.GetVariables($"{prefix}.{parentProperty.Name}", childProperty, variables);
                    }
                }
                else
                {
                    variables.Add(new Variable { Name = $"{prefix}.{parentProperty.Name}", Value = property });
                }
            }
        }
    }
}
