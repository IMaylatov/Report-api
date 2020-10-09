namespace SofTrust.Report.Core.Generator.Data
{
    using System.Data.SqlClient;

    public class SqlData : IData
    {
        private SqlDataReader sqlDataReader;

        public SqlData(SqlDataReader sqlDataReader)
        {
            this.sqlDataReader = sqlDataReader;
        }

        public int FieldCount => this.sqlDataReader.FieldCount;

        public string GetName(int i)
        {
            return this.sqlDataReader.GetName(i);
        }

        public object GetValue(int i)
        {
            return this.sqlDataReader.GetValue(i);
        }

        public bool Read()
        {
            return this.sqlDataReader.Read();
        }
    }
}
