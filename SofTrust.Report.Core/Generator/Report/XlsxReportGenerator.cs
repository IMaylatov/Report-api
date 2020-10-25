namespace SofTrust.Report.Core.Generator.Report
{
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class XlsxReportGenerator : IReportGenerator
    {
        public abstract Stream Generate(JToken report, Stream template, string host, JToken jVariableValues);

        protected IEnumerable<Variable> GetVariables(JToken jVariables, JToken jVariableValues)
        {
            var valueByNames = jVariableValues.Children().ToDictionary(x => x["name"].ToString(), x => x["value"]);
            var jVariablesWithValue = new JArray();
            foreach (var variable in jVariables)
            {
                var jVariableValue = new JObject();
                jVariableValue["name"] = variable["name"];
                jVariableValue["type"] = variable["type"];
                jVariableValue["data"] = variable["data"];
                jVariableValue["value"] = valueByNames[variable["name"].ToString()];
                jVariablesWithValue.Add(jVariableValue);
            }

            return GetVariables(jVariablesWithValue);
        }

        private IEnumerable<Variable> GetVariables(JToken jVariables)
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
                        new Variable
                        {
                            Name = variable["name"].ToString(),
                            Type = variable["type"].ToString(),
                            Data = variable["data"],
                            Value = variable["value"]
                        });
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
                    variables.Add(new Variable { Name = $"{prefix}.{parentProperty.Name}", Value = property, Type = property.Type.ToString().ToLower() });
                }
            }
        }
    }
}
