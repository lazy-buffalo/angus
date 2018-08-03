using LazyBuffalo.Angus.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LazyBuffalo.Angus.Api.Data
{
    public class AngusDbContext : DbContext
    {
        public DbSet<Cow> Cows { get; set; }
        public DbSet<GpsEntry> GpsEntries { get; set; }
        public DbSet<TemperatureEntry> TemperatureEntries { get; set; }

        public AngusDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cow>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Cow>()
                .HasMany(x => x.GpsEntries)
                .WithOne(x => x.Cow)
                .HasForeignKey(x => x.CowId);

            modelBuilder.Entity<Cow>()
                .HasMany(x => x.TemperatureEntries)
                .WithOne(x => x.Cow)
                .HasForeignKey(x => x.CowId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
