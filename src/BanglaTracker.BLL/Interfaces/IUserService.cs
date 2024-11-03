using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;

namespace BanglaTracker.BLL.Interfaces
{   
    public interface IUserService
    {
        Task<bool> UpdateLastActiveAsync(Guid installationID, DateTime lastActiveTime);
    }


}
