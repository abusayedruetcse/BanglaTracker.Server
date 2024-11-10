using BanglaTracker.BLL.Interfaces;
using BanglaTracker.BLL.Models;
using Microsoft.EntityFrameworkCore;

namespace BanglaTracker.Infrastructure.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly TrackerDbContext _context;

        public LocationRepository(TrackerDbContext context)
        {
            _context = context;
        }

        public async Task<LocationData?> FetchLocationByInstallationIDAsync(
            Guid installationId)
        {
            return await _context.AppUsers
                .Where(user => user.InstallationId == installationId)
                .Select(loc => new LocationData()
                {
                    Longitude = loc.Longitude,
                    Latitude = loc.Latitude,
                })
                .FirstOrDefaultAsync();
        }
        
    }

}
