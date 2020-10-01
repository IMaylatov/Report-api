namespace SofTrust.Report.Business.Model.Domain
{
    public class ReportDataSource
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }

        public int DataSourceId { get; set; }
        public DataSource DataSource { get; set; }
    }
}
