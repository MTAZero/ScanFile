namespace ScanFile.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ScanFileContext : DbContext
    {
        public ScanFileContext()
            : base("name=ScanFileContext")
        {
        }

        public virtual DbSet<FILEEXE> FILEEXEs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
