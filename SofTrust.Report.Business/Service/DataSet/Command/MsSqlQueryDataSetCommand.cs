namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Service;
    using SofTrust.Report.Business.Service.DataSet.Reader;
    using SofTrust.Report.Business.Service.DataSource.Connection;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Text.RegularExpressions;

    public class MsSqlQueryDataSetCommand : IDataSetCommand
    {
        private SqlCommand command;

        public IDataSourceConnection Connection { get; set; }

        public int Timeout { set => this.command.CommandTimeout = value; }

        public MsSqlQueryDataSetCommand(IDataSourceConnection connection, SqlCommand command)
        {
            this.Connection = connection;
            this.command = command;
        }

        public IDataSetReader ExecuteReader()
        {
            return new SqlDataSetReader(command.ExecuteReader());
        }

        public void AddParameters(IEnumerable<Parameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                switch (parameter.Type)
                {
                    case "select":
                    case "multipleSelect":
                        {
                            var keyField = parameter.Data["keyField"].ToString();
                            var keys = new List<string>();
                            foreach(var value in parameter.Value)
                            {
                                keys.Add(value[keyField].ToString());
                            }
                            var stringKeys = string.Join(",", keys);
                            if (string.IsNullOrWhiteSpace(stringKeys))
                            {
                                var dataSetQuery = parameter.Data["dataSet"]["data"]["query"];
                                stringKeys = $"select {keyField} from ({dataSetQuery}) {parameter.Name}_{keyField}";
                            }
                            var filterKeys = $"({keyField} in ({stringKeys}))";
                            command.CommandText = Regex.Replace(command.CommandText, $"@{parameter.Name}.Filtr", filterKeys, RegexOptions.IgnoreCase);
                            command.CommandText = Regex.Replace(command.CommandText, $"@{parameter.Name}.All.{keyField}", stringKeys, RegexOptions.IgnoreCase);
                            break;
                        }
                    case "date":
                        {
                            command.CommandText = Regex.Replace(command.CommandText, $"@{parameter.Name}.Value", 
                                DateTime.Parse(parameter.Value.ToString()).ToString("s"), RegexOptions.IgnoreCase);
                            command.CommandText = Regex.Replace(command.CommandText, $"@{parameter.Name}", 
                                DateTime.Parse(parameter.Value.ToString()).ToString("s"), RegexOptions.IgnoreCase);
                            break;
                        }
                    default:
                        {
                            command.CommandText = Regex.Replace(command.CommandText, $"@{parameter.Name}", parameter.Value.ToString(), RegexOptions.IgnoreCase);
                            break;
                        }
                }
            }
        }
    }
}
