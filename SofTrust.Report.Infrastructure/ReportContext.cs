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
        public DbSet<Template> Templates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Variable> Variables { get; set; }
    }
}
