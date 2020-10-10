namespace SofTrust.Report.Core.Generator.Command
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using SofTrust.Report.Core.Generator;
    using System.Data.SqlClient;
    using SofTrust.Report.Core.Generator.Data;
    using System.Linq;

    public class MsSqlQueryDataSetCommand : ICommand
    {
        private const string VARIABLE_TYPE_MULTIPLE_SELECT = "multipleSelect";
        private const string VARIABLE_TYPE_SELECT = "select";
        private const string VARIABLE_TYPE_DATE = "date";

        private SqlCommand command;

        public IConnection Connection { get; set; }

        public int Timeout { set => this.command.CommandTimeout = value; }

        public MsSqlQueryDataSetCommand(IConnection connection, SqlCommand command)
        {
            this.Connection = connection;
            this.command = command;
        }

        public IData ExecuteReader()
        {
            return new SqlData(command.ExecuteReader());
        }

        public void AddVariables(IEnumerable<Variable> variables)
        {
            foreach (var variable in variables)
            {
                switch (variable.Type)
                {
                    case VARIABLE_TYPE_MULTIPLE_SELECT:
                        {
                            var keyField = variable.Data["keyField"].ToString();
                            var keys = new List<string>();
                            foreach (var value in variable.Value)
                            {
                                keys.Add(value[keyField].ToString());
                            }
                            var stringKeys = string.Join(",", keys);
                            if (string.IsNullOrWhiteSpace(stringKeys))
                            {
                                var dataSetQuery = variable.Data["dataSet"]["data"]["query"];
                                stringKeys = $"select {keyField} from ({dataSetQuery}) {variable.Name}_{keyField}";
                            }
                            var filterKeys = $"({keyField} in ({stringKeys}))";

                            command.CommandText = Regex.Replace(command.CommandText, $"@{variable.Name}.Filtr", filterKeys, RegexOptions.IgnoreCase);
                            command.CommandText = Regex.Replace(command.CommandText, $"@{variable.Name}.All.{keyField}", stringKeys, RegexOptions.IgnoreCase);
                            break;
                        }
                    case VARIABLE_TYPE_DATE:
                        {
                            command.CommandText = Regex.Replace(command.CommandText, $"@{variable.Name}.Value", 
                                DateTime.Parse(variable.Value.ToString()).ToString("s"), RegexOptions.IgnoreCase);
                            command.CommandText = Regex.Replace(command.CommandText, $"@{variable.Name}", 
                                DateTime.Parse(variable.Value.ToString()).ToString("s"), RegexOptions.IgnoreCase);
                            break;
                        }
                    case VARIABLE_TYPE_SELECT:
                    default:
                        {
                            command.CommandText = Regex.Replace(command.CommandText, $"@{variable.Name}", variable.Value.ToString(), RegexOptions.IgnoreCase);
                            break;
                        }
                }
            }
        }
    }
}
