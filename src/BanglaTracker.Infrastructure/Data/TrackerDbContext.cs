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

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<LocationData> LocationDatas { get; set; }
        public DbSet<TrainJourney> TrainJourneys { get; set; }
        public DbSet<TrainJourneyTracking> TrainJourneyTrackings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .HasKey(user => user.InstallationId);

            modelBuilder.Entity<LocationData>()
                .HasKey(ld => ld.InstallationId); // Set the primary key

            modelBuilder.Entity<TrainJourney>()
                .HasKey(tj => tj.Id);

            modelBuilder.Entity<TrainJourneyTracking>()
                .HasKey(tjt => tjt.Id);
        }
    }
}
