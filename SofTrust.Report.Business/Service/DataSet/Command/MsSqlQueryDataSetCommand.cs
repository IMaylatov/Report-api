namespace SofTrust.Report.Business.Service.DataSet.Command
{
    using SofTrust.Report.Business.Model;
    using SofTrust.Report.Business.Service.DataSet.Reader;
    using SofTrust.Report.Business.Service.DataSource.Connection;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class MsSqlQueryDataSetCommand : IDataSetCommand
    {
        private SqlCommand command;

        public IDataSourceConnection Connection { get; set; }

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
                var newParameterName = parameter.Name.Replace(".", "_");

                switch (parameter.Value.Type)
                {
                    case Newtonsoft.Json.Linq.JTokenType.Array:
                        {
                            command.CommandText = command.CommandText.Replace($"@{newParameterName}", string.Join(",", parameter.Value));
                            break;
                        }
                    case Newtonsoft.Json.Linq.JTokenType.Date:
                        {
                            command.CommandText = command.CommandText.Replace($"@{newParameterName}", DateTime.Parse(parameter.Value.ToString()).ToString("s"));
                            break;
                        }
                    default:
                        {
                            command.CommandText = command.CommandText.Replace($"@{newParameterName}", parameter.Value.ToString());
                            break;
                        }
                }
            }
        }
    }
}
