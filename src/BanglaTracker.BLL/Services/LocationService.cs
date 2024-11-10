using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.BLL.Models;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;

namespace BanglaTracker.BLL.Services
{
    public class LocationService : ILocationService
    {
        private readonly IUserRepository _userRepository;

        public LocationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task SaveLocationAsync(AppUserDto userDto)
        {
            var userData = await _userRepository.FetchUserByInstallationIDAsync(userDto.InstallationId);

            if (userData == null)
            {
                var user = new AppUser()
                {
                    InstallationId = Guid.NewGuid(),
                    Longitude = userDto.Longitude,
                    Latitude = userDto.Latitude,
                    LastActiveDateTime = DateTime.UtcNow,
                    CreatedDateTime = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);
            }
            else
            {
                userData.Longitude = userDto.Longitude;
                userData.Latitude = userDto.Latitude;
                userData.LastActiveDateTime = DateTime.UtcNow;

                await _userRepository.UpdateAsync(userData);
            }

            await _userRepository.SaveChangesAsync();
        }

    }

}
