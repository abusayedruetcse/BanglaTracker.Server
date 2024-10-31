using BanglaTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BanglaTracker.Infrastructure.Data
{
    public class TrackerDbContext : DbContext
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<LocationData> LocationDatas { get; set; }
        public DbSet<TrainJourney> TrainJourneys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationData>()
                .HasKey(ld => ld.Id); // Set the primary key

            modelBuilder.Entity<TrainJourney>()
                .HasKey(tj => tj.TrainId);
        }
    }
}
