
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
        public DbSet<Train> Trains { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<TrainJourney> TrainJourneys { get; set; }
        public DbSet<TrainJourneyDetail> TrainJourneyDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .HasKey(user => user.InstallationId);

            modelBuilder.Entity<Train>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Station>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<TrainJourney>()
                .HasKey(tj => tj.Id);

            modelBuilder.Entity<TrainJourneyDetail>()
                .HasKey(tjd => tjd.Id);
        }
    }
}
