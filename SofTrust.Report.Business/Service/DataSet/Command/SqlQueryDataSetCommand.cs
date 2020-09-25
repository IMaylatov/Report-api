namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Business.Service.DataAdapter;
    using SofTrust.Report.Business.Service.DataSource.Command;
    using System.Collections.Generic;
    using SofTrust.Report.Business.Model;
    using System.Text.RegularExpressions;
    using System.Linq;
    using Newtonsoft.Json.Linq;

    public class SqlQueryDataSetCommand : IDataSetCommand
    {
        private const string PREFIX_DOCUMENT_PARAMETER = "@document";

        private readonly string dataSourceName;
        private readonly string query;

        public SqlQueryDataSetCommand(string dataSourceName, string query)
        {
            this.dataSourceName = dataSourceName;
            this.query = query;
        }

        public object Execute(IEnumerable<Parameter> parameters, Dictionary<string, IDataSourceCommand> dataSources, IDataSetAdapter dataSetAdapter)
        {
            var sqlQuery = ReplaceParameters(query, parameters);

            var data = dataSources[dataSourceName].Execute(sqlQuery);

            return dataSetAdapter.Adapt(data);
        }

        private string ReplaceParameters(string query, IEnumerable<Parameter> parameters)
        {
            var documentParameter = this.GetDocumentParameter(parameters);

            var queryParameters = this.GetParameters(query);
            foreach (var parameter in queryParameters)
            {
                string matchParameterValue = parameter.StartsWith(PREFIX_DOCUMENT_PARAMETER)
                    ? documentParameter[parameter.Substring(PREFIX_DOCUMENT_PARAMETER.Length + 1)].ToString()
                    : parameters.FirstOrDefault(x => parameter.Contains(x.Name, System.StringComparison.InvariantCultureIgnoreCase)).Value.ToString();
                query = query.Replace(parameter, matchParameterValue, System.StringComparison.InvariantCultureIgnoreCase);
            }

            return query;
        }

        private IEnumerable<string> GetParameters(string query)
        {
            var parameters = new List<string>();

            var regex = new Regex(@"@([\w\\.]*)");
            var matches = regex.Matches(query);
            foreach (Match match in matches)
                parameters.Add(match.Value.ToLower());

            return parameters;
        }

        private JToken GetDocumentParameter(IEnumerable<Parameter> parameters)
        {
            var documentParameter = parameters.FirstOrDefault(x => PREFIX_DOCUMENT_PARAMETER.EndsWith(x.Name));
            if (documentParameter != null)
            {
                return JToken.Parse(documentParameter.Value.ToString().ToLower());
            }
            return null;
        }
    }
}
