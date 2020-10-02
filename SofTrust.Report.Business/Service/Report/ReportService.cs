namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Business.Model.Domain;
    using System.Threading.Tasks;
    using System.Linq;
    using MoreLinq;

    public class ReportService
    {
        private readonly ReportContext context;

        public ReportService(ReportContext reportContext)
        {
            this.context = reportContext;
        }

        public async void DeleteAsync(int reportId)
        {
            var report = await context.Reports
                .Include(x => x.ReportDataSources).ThenInclude(x => x.DataSource)
                .Include(x => x.ReportDataSets).ThenInclude(x => x.DataSet)
                .Include(x => x.ReportVariables).ThenInclude(x => x.Variable)
                .FirstOrDefaultAsync(x => x.Id == reportId);

            report.ReportDataSources.ForEach(x => context.DataSources.Remove(x.DataSource));
            report.ReportDataSets.ForEach(x => context.DataSets.Remove(x.DataSet));
            report.ReportVariables.ForEach(x => context.Variables.Remove(x.Variable));

            context.Reports.Remove(report);
        }

        public async Task<Report> UpdateAsync(Report report)
        {
            this.context.Reports.Update(report);

            var reportDataSourceIds = report.ReportDataSources.Select(x => x.DataSourceId).ToArray();
            var deleteReportDataSources = await this.context.ReportDataSources.Include(x => x.DataSource)
                .Where(existsDs => existsDs.ReportId == report.Id && !reportDataSourceIds.Contains(existsDs.DataSourceId))
                .ToListAsync();
            var reportDataSetids = report.ReportDataSets.Select(x => x.DataSetId).ToArray();
            var deleteReportDataSets = await this.context.ReportDataSets.Include(x => x.DataSet)
                .Where(existsDs => existsDs.ReportId == report.Id && !reportDataSetids.Contains(existsDs.DataSetId))
                .ToListAsync();
            var reportVaraibles = report.ReportVariables.Select(x => x.VariableId).ToArray();
            var deleteReportVariables = await this.context.ReportVariables.Include(x => x.Variable)
                .Where(existsV => existsV.ReportId == report.Id && !reportVaraibles.Contains(existsV.VariableId))
                .ToListAsync();

            deleteReportDataSources.ForEach(x =>
            {
                report.ReportDataSources.Remove(x);
                this.context.DataSources.Remove(x.DataSource);
            });

            deleteReportDataSets.ForEach(x =>
            {
                report.ReportDataSets.Remove(x);
                this.context.DataSets.Remove(x.DataSet);
            });

            deleteReportVariables.ForEach(x =>
            {
                report.ReportVariables.Remove(x);
                this.context.Variables.Remove(x.Variable);
            });

            return report;
        }
    }
}
