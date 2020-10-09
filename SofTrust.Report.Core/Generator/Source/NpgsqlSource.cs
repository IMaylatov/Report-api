namespace SofTrust.Report.Core.Generator.Source
{
    public class NpgsqlSource : ISource
    {
        private readonly string connectionString;
        public string Name { get; set; }

        public NpgsqlSource(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IConnection CreateConnection()
        {
            throw new System.NotImplementedException();
        }
    }
}
