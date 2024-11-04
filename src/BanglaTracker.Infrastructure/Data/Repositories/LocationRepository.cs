using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BanglaTracker.Infrastructure.Data.Repositories
{
    public class LocationRepository : Repository<LocationData>, ILocationRepository
    {
        private readonly TrackerDbContext _context;

        public LocationRepository(TrackerDbContext context) : base(context) // Pass context to the base constructor
        {
            _context = context;
        }

        public async Task<LocationData?> FetchLocationByInstallationIDAsync(
            Guid installationId)
        {
            return await _context.LocationDatas
                .Where(loc => loc.InstallationId == installationId)
                .FirstOrDefaultAsync();
        }
        
    }

}
