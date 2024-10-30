using BanglaTracker.Core.Entities;

namespace BanglaTracker.BLL.Interfaces
{
    public interface ILocationService
    {
        Task SaveLocationAsync(LocationData locationData);

    }

}
