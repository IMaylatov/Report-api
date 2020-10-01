namespace SofTrust.Report.Business
{
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Business.Model;

    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportType> ReportTypes { get; set; }
        public DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReportType>().HasData(
                new ReportType { Id = 1, Name = "Malibu" },
                new ReportType { Id = 2, Name = "ClosedXml" });
        }
    }
}
