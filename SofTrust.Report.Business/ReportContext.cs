namespace SofTrust.Report.Business
{
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Business.Model;

    public class ReportContext : DbContext
    {
        public ReportContext(DbContextOptions<ReportContext> options) : base(options)
        {
        }

        //public DbSet<Report> Reports { get; set; }
        //public DbSet<ReportType> ReportTypes { get; set; }
        //public DbSet<Template> Templates { get; set; }
    }
}
