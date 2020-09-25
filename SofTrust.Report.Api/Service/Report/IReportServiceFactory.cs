namespace SofTrust.Report.Api.Service.Report
{
    using SofTrust.Report.Business.Service.Report;

    public interface IReportServiceFactory
    {
        IReportService Create(string type);
    }
}
