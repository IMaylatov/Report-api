namespace SofTrust.Report.Infrastructure.Repository
{
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Core.Models.Domain;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReportRepository
    {
        private readonly ReportContext context;

        public ReportRepository(ReportContext reportContext)
        {
            this.context = reportContext;
        }

        public async Task<Report> UpdateGraphAsync(Report report)
        {
            var existingReport = await context.Reports
                       .Include(x => x.DataSources)
                       .Include(x => x.DataSets)
                       .Include(x => x.Variables)
                       .FirstOrDefaultAsync(x => x.Id == report.Id);

            context.Entry(existingReport).CurrentValues.SetValues(report);

            UpdateDataSources(report, existingReport);
            UpdateDataSets(report, existingReport);
            UpdateVariables(report, existingReport);

            return report;
        }

        private void UpdateDataSources(Report report, Report existingReport)
        {
            foreach (var dataSource in report.DataSources)
            {
                var existingDataSource = existingReport.DataSources.FirstOrDefault(p => p.Id == dataSource.Id);

                if (existingDataSource == null)
                {
                    existingReport.DataSources.Add(dataSource);
                }
                else
                {
                    context.Entry(existingDataSource).CurrentValues.SetValues(dataSource);
                }
            }
            foreach (var dataSource in existingReport.DataSources)
            {
                if (!report.DataSources.Any(p => p.Id == dataSource.Id))
                {
                    context.DataSources.Remove(dataSource);
                }
            }
        }

        private void UpdateDataSets(Report report, Report existingReport)
        {
            foreach (var dataSet in report.DataSets)
            {
                var existingDataSet = existingReport.DataSets.FirstOrDefault(p => p.Id == dataSet.Id);

                if (existingDataSet == null)
                {
                    existingReport.DataSets.Add(dataSet);
                }
                else
                {
                    context.Entry(existingDataSet).CurrentValues.SetValues(dataSet);
                }
            }
            foreach (var dataSet in existingReport.DataSets)
            {
                if (!report.DataSets.Any(p => p.Id == dataSet.Id))
                {
                    context.Remove(dataSet);
                }
            }
        }

        private void UpdateVariables(Report report, Report existingReport)
        {
            foreach (var variable in report.Variables)
            {
                var existingVariable = existingReport.Variables.FirstOrDefault(p => p.Id == variable.Id);

                if (existingVariable == null)
                {
                    existingReport.Variables.Add(variable);
                }
                else
                {
                    context.Entry(existingVariable).CurrentValues.SetValues(variable);
                }
            }
            foreach (var variable in existingReport.Variables)
            {
                if (!report.Variables.Any(p => p.Id == variable.Id))
                {
                    context.Remove(variable);
                }
            }
        }
    }
}
