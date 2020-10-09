namespace SofTrust.Report.Business.Service.Report
{
    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Business.Model.Domain;
    using System.Linq;

    public class ReportRepository
    {
        private readonly ReportContext context;

        public ReportRepository(ReportContext reportContext)
        {
            this.context = reportContext;
        }

        public Report InsertGraph(Report report)
        {
            this.context.Entry(report).State = EntityState.Added;

            report.ReportDataSources.ForEach(x =>
            {
                this.context.Entry(x).State = EntityState.Added;
            });

            report.ReportDataSets.ForEach(x =>
            {
                this.context.Entry(x).State = EntityState.Added;
                this.context.Entry(x.DataSet).State = EntityState.Added;
            });

            report.ReportVariables.ForEach(x =>
            {
                this.context.Entry(x).State = EntityState.Added;
                this.context.Entry(x.Variable).State = EntityState.Added;
            });

            return report;
        }

        public Report UpdateGraph(Report report)
        {
            var existingReport = context.Reports
                       .Include(x => x.ReportDataSources).ThenInclude(x => x.DataSource)
                       .Include(x => x.ReportDataSets).ThenInclude(x => x.DataSet)
                       .Include(x => x.ReportVariables).ThenInclude(x => x.Variable)
                       .FirstOrDefault(x => x.Id == report.Id);

            context.Entry(existingReport).CurrentValues.SetValues(report);
            foreach (var reportDataSource in report.ReportDataSources)
            {
                var existingReportDataSource = existingReport.ReportDataSources
                    .FirstOrDefault(p => p.DataSourceId == reportDataSource.DataSourceId);

                if (existingReportDataSource == null)
                {
                    existingReport.ReportDataSources.Add(reportDataSource);
                }
                else
                {
                    context.Entry(existingReportDataSource).CurrentValues.SetValues(reportDataSource);
                    context.Entry(existingReportDataSource.DataSource).CurrentValues.SetValues(reportDataSource.DataSource);
                }
            }
            foreach(var reportDataSet in report.ReportDataSets)
            {
                var existingReportDataSet = existingReport.ReportDataSets
                    .FirstOrDefault(p => p.DataSetId == reportDataSet.DataSetId);

                if (existingReportDataSet == null)
                {
                    existingReport.ReportDataSets.Add(reportDataSet);
                }
                else
                {
                    context.Entry(existingReportDataSet).CurrentValues.SetValues(reportDataSet);
                    context.Entry(existingReportDataSet.DataSet).CurrentValues.SetValues(reportDataSet.DataSet);
                }
            }
            foreach (var reportVariable in report.ReportVariables)
            {
                var existingReportVariable = existingReport.ReportVariables
                    .FirstOrDefault(p => p.VariableId == reportVariable.VariableId);

                if (existingReportVariable == null)
                {
                    existingReport.ReportVariables.Add(reportVariable);
                }
                else
                {
                    context.Entry(existingReportVariable).CurrentValues.SetValues(reportVariable);
                    context.Entry(existingReportVariable.Variable).CurrentValues.SetValues(reportVariable.Variable);
                }
            }

            foreach (var reportDataSources in existingReport.ReportDataSources)
            {
                if (!report.ReportDataSources.Any(p => p.DataSourceId == reportDataSources.DataSourceId))
                {
                    context.Remove(reportDataSources);
                }
            }
            foreach (var reportDataSet in existingReport.ReportDataSets)
            {
                if (!report.ReportDataSets.Any(p => p.DataSetId == reportDataSet.DataSetId))
                {
                    context.Remove(reportDataSet.DataSet);
                    context.Remove(reportDataSet);
                }
            }
            foreach (var reportVariable in existingReport.ReportVariables)
            {
                if (!report.ReportVariables.Any(p => p.VariableId == reportVariable.VariableId))
                {
                    context.Remove(reportVariable.Variable);
                    context.Remove(reportVariable);
                }
            }

            return report;
        }

        public void DeleteGraph(int reportId)
        {
            var report = context.Reports
                   .Include(x => x.ReportDataSources)
                   .Include(x => x.ReportDataSets).ThenInclude(x => x.DataSet)
                   .Include(x => x.ReportVariables).ThenInclude(x => x.Variable)
                   .FirstOrDefault(x => x.Id == reportId);

            this.context.Entry(report).State = EntityState.Deleted;

            report.ReportDataSources.ForEach(x =>
            {
                this.context.Entry(x).State = EntityState.Deleted;
            });

            report.ReportDataSets.ForEach(x =>
            {
                this.context.Entry(x).State = EntityState.Deleted;
                this.context.Entry(x.DataSet).State = EntityState.Deleted;
            });

            report.ReportVariables.ForEach(x =>
            {
                this.context.Entry(x).State = EntityState.Deleted;
                this.context.Entry(x.Variable).State = EntityState.Deleted;
            });
        }
    }
}
