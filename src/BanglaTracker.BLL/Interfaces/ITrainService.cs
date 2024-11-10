using BanglaTracker.BLL.DTOs;

namespace BanglaTracker.BLL.Interfaces
{
    public interface ITrainService
    {
        Task<TrainDto> AddTrainAsync(TrainDto trainDto);
        Task<IEnumerable<TrainDto>> GetAllTrainsAsync();
        Task<TrainDto> GetTrainByIdAsync(int trainId);
        Task UpdateTrainAsync(TrainDto trainDto);
        Task DeleteTrainAsync(int trainId);
        Task<StationDto> AddStationAsync(StationDto stationDto);
        Task<IEnumerable<StationDto>> GetAllStationsAsync();
        Task<StationDto> GetStationByIdAsync(int stationId);
        //Task<IEnumerable<Train>> GetTrainsForRouteAsync(string fromStation, string toStation);
        Task UpdateStationAsync(StationDto station);
        Task DeleteStationAsync(int stationId);
    }

}
