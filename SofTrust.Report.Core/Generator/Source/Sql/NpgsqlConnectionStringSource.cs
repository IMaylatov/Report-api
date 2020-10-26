namespace SofTrust.Report.Core.Generator.Source.Sql
{
    public class NpgsqlConnectionStringSource : ISource
    {
        private readonly string connectionString;
        public string Name { get; set; }

        public NpgsqlConnectionStringSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IConnection CreateConnection()
        {
            throw new System.NotImplementedException();
        }
    }
}
