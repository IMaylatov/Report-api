namespace SofTrust.Report.Trs.Models.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("trs_Host")]
    public class Host
    {
        [Key, Column("HostID")]
        public int Id { get; set; }

        [Column("HostName")]
        public string Name { get; set; }

        public string ConnectionString { get; set; }
    }
}
