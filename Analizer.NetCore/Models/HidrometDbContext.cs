using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Analizer.NetCore.Models
{
    public partial class HidrometDbContext : DbContext
    {
        public HidrometDbContext(DbContextOptions<HidrometDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hidromet> Hidromet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hidromet>(entity =>
            {
                entity.Property(e => e.City)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");
            });
        }
    }
}
