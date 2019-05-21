using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analizer.NetCore.Models
{
    public class FireRiskContext:DbContext
    {
        public FireRiskContext(DbContextOptions<FireRiskContext> options):base(options)
        {

        }

        public DbSet<FireRiskItam> Itams { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FireRiskItam>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Precipitation).HasDefaultValue(0);
                e.Property(p => p.ClassOfFireRisk).IsRequired();
                e.Property(p => p.CompIndicator).IsRequired();
                e.Property(p => p.CompIndicatorDay).IsRequired();
                e.Property(p => p.Day).IsRequired();
                e.Property(p => p.Temperature).IsRequired();
                e.Property(p => p.DewPoint).IsRequired();


            });
            modelBuilder.Entity<FireRiskItam>()
                .HasOne(p => p.City)
                .WithMany(p => p.FireRiskItams)
                .HasForeignKey(p => p.CityId).IsRequired();
            
        }
    }
}
