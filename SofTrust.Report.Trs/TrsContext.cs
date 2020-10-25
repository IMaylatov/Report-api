namespace SofTrust.Report.Trs
{
    using Microsoft.EntityFrameworkCore;
    using SofTrust.Report.Trs.Models.Domain;

    public class TrsContext : DbContext
    {
        public TrsContext(DbContextOptions<TrsContext> options) : base(options)
        {
        }

        public DbSet<Host> Hosts { get; set; }
    }
}
