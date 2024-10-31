using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;

namespace BanglaTracker.BLL.Services
{
    public class LocationService : ILocationService
    {
        private readonly IRepository<LocationData> _repository;

        public LocationService(IRepository<LocationData> repository)
        {
            _repository = repository;
        }

        public async Task SaveLocationAsync(LocationData locationData)
        {
            await _repository.AddAsync(locationData);

            await _repository.SaveChangesAsync();
        }

    }

}
