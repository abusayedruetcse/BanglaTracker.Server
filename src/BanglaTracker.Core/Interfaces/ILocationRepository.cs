using BanglaTracker.Core.Entities;

namespace BanglaTracker.Core.Interfaces
{
    public interface ILocationRepository : IRepository<LocationData>
    {
        Task<LocationData?> FetchLocationByInstallationIDAsync(
            Guid installationId);
    }

}
