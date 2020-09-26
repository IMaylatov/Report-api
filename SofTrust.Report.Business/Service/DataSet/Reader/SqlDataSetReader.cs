namespace SofTrust.Report.Business.Service.DataSet.Reader
{
    using System.Data.SqlClient;

    public class SqlDataSetReader : IDataSetReader
    {
        private SqlDataReader sqlDataReader;

        public SqlDataSetReader(SqlDataReader sqlDataReader)
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
