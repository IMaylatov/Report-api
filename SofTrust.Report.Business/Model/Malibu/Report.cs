namespace SofTrust.Report.Business.Model.Malibu
{
    using ClosedXML.Excel;
    using System.IO;
    using System.Xml.Serialization;

    public class Report
    {
        public string Caption;
        public byte[] ReportTemplate;
        public byte[] ReportDesc;

        public MAIN DeserializeReportDesc()
        {
            using (var stream = new MemoryStream(ReportDesc))
            {
                stream.Position = 0;
                var serializer = new XmlSerializer(typeof(MAIN));
                return serializer.Deserialize(stream) as MAIN;
            }
        }

        public XLWorkbook DeserializeReportTemplate()
        {
            return new XLWorkbook(new MemoryStream(ReportTemplate));
        }
    }
}
