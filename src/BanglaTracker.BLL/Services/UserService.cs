using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;

namespace BanglaTracker.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> UpdateLastActiveAsync(
            Guid installationID, 
            DateTime lastActiveTime)
        {
            var user = await _userRepository.FetchUserByInstallationIDAsync(installationID);

            if (user == null)
            {
                var appUser = new AppUser()
                {
                    InstallationId = installationID,
                    LastActiveDateTime = lastActiveTime,
                    CreatedDateTime = lastActiveTime
                };

                await _userRepository.AddAsync(appUser);
            }
            else
            {
                user.LastActiveDateTime = lastActiveTime;
            }
            
            await _userRepository.SaveChangesAsync();

            return true;
        }
    }


}
