namespace SofTrust.Report.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Core.Models.Domain;

    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {
        }

        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportDataSet> ReportDataSets { get; set; }
        public DbSet<ReportDataSource> ReportDataSources { get; set; }
        public DbSet<ReportVariable> ReportVariables { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Variable> Variables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReportDataSet>()
                .HasKey(t => new { t.ReportId, t.DataSetId });

            modelBuilder.Entity<ReportDataSource>()
                .HasKey(t => new { t.ReportId, t.DataSourceId });

            modelBuilder.Entity<ReportVariable>()
                .HasKey(t => new { t.ReportId, t.VariableId });
        }
    }
}
