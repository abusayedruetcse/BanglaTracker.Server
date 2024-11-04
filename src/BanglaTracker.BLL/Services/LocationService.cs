using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;

namespace BanglaTracker.BLL.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task SaveLocationAsync(LocationData locationData)
        {
            var location = await _locationRepository.FetchLocationByInstallationIDAsync(locationData.InstallationId);

            if (location == null)
            {                
                await _locationRepository.AddAsync(locationData);
            }
            else
            {
                location.Longitude = locationData.Longitude;
                location.Latitude = locationData.Latitude;
                location.ModifiedDateTime = locationData.ModifiedDateTime;

                await _locationRepository.UpdateAsync(location);
            }

            await _locationRepository.SaveChangesAsync();
        }

    }

}
