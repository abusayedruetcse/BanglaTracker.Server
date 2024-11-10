using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Models;

namespace BanglaTracker.BLL.Interfaces
{
    public interface ILocationService
    {
        Task SaveLocationAsync(AppUserDto userDto);

    }

}
