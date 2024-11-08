using BanglaTracker.Core.DTOs;
using BanglaTracker.Core.Entities;

namespace BanglaTracker.BLL.Interfaces
{
    public interface IDataImportService
    {
        Task AddStationsAsync(List<Station> stations);
        Task AddTrainsAsync(List<Train> trains);
    }

}
