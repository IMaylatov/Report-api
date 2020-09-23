namespace SofTrust.Report.Business.Service.DataSource.Command
{
    using SofTrust.Report.Business.Service.DataAdapter;

    public class NpgsqlDataSourceCommand : IDataSourceCommand
    {
        private readonly string connectionString;

        public NpgsqlDataSourceCommand(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public object Execute(object dataSet)
        {
            throw new System.NotImplementedException();
        }
    }
}
