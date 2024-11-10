
using BanglaTracker.BLL.Models;

namespace BanglaTracker.BLL.Interfaces
{
    public interface ILocationRepository
    {
        Task<LocationData?> FetchLocationByInstallationIDAsync(
            Guid installationId);
    }

}
