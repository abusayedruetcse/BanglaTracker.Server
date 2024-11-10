using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Enums;

namespace BanglaTracker.Core.Interfaces
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<AppUser?> FetchUserByInstallationIDAsync(
            Guid installationId);


    }

}
