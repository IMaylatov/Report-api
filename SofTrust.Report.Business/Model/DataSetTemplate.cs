namespace SofTrust.Report.Business.Model
{
    using ClosedXML.Excel;

    public class DataSetTemplate
    {
        public bool RecordNumber { get; set; }
        public IXLAddress Address { get; set; }
    }
}
