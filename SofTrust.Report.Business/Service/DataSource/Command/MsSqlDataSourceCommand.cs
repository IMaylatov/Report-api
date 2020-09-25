namespace SofTrust.Report.Business.Service.DataSource.Command
{
    using SofTrust.Report.Business.Service.DataAdapter;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class MsSqlDataSourceCommand : IDataSourceCommand
    {
        private readonly string connectionString;

        public MsSqlDataSourceCommand(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public object Execute(object sqlQuery)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(sqlQuery.ToString(), connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

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
            }
        }
    }
}
