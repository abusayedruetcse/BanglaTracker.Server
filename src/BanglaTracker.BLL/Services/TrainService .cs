using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;
using BanglaTracker.Core.Utils;
using System.Data;

namespace BanglaTracker.BLL.Services
{
    public class TrainService : ITrainService
    {
        private readonly IRepository<Station> _stationRepository;
        private readonly IRepository<Train> _trainRepository;

        public TrainService(
            IRepository<Station> stationRepository, 
            IRepository<Train> trainRepository)
        {
            _stationRepository = stationRepository;
            _trainRepository = trainRepository;
        }

        public async Task<TrainDto> AddTrainAsync(TrainDto trainDto)
        {
            var train = Mapper<TrainDto, Train>.Map(trainDto);
            
            await _trainRepository.AddAsync(train);
            await _trainRepository.SaveChangesAsync();

            return Mapper<Train, TrainDto>.Map(train);
        }

        public async Task<TrainDto> GetTrainByIdAsync(int trainId)
        {
            var train = await _trainRepository.GetByIdAsync(trainId);
            return train != null ? Mapper<Train, TrainDto>.Map(train) : null;
        }

        public async Task<StationDto> AddStationAsync(StationDto stationDto)
        {
            var station = Mapper<StationDto, Station>.Map(stationDto);

            await _stationRepository.AddAsync(station);
            await _stationRepository.SaveChangesAsync();

            return Mapper<Station, StationDto>.Map(station);
        }

        public async Task<StationDto> GetStationByIdAsync(int stationId)
        {
            var station = await _stationRepository.GetByIdAsync(stationId);
            return station != null ? Mapper<Station, StationDto>.Map(station) : null;
        }

        public async Task<IEnumerable<StationDto>> GetAllStationsAsync()
        {
            var stations = (await _stationRepository.GetAllAsync()).OrderBy(x => x.Name).ToList();
            return stations.Select(station => Mapper<Station, StationDto>.Map(station));
        }

        //public async Task<IEnumerable<TrainDto>> GetTrainsForRouteAsync(string fromStation, string toStation)
        //{
        //    // Query the Train repository for trains that pass through both stations
        //    var trains = await _trainRepository.GetAllAsync();
        //    var matchingTrains = trains.Where(train =>
        //            train.RouteStations.Contains(fromStation) &&
        //            train.RouteStations.Contains(toStation))
        //        .ToList();

        //    return matchingTrains.Select(train => Mapper<Train, TrainDto>.Map(train));
        //}

        public async Task UpdateStationAsync(StationDto stationDto)
        {
            var station = Mapper<StationDto, Station>.Map(stationDto);
            await _stationRepository.UpdateAsync(station);
            await _stationRepository.SaveChangesAsync();
        }

        public async Task DeleteStationAsync(int stationId)
        {
            var station = await _stationRepository.GetByIdAsync(stationId);
            if (station != null)
            {
                await _stationRepository.DeleteAsync(stationId);
                await _stationRepository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TrainDto>> GetAllTrainsAsync()
        {
            var trains = (await _trainRepository.GetAllAsync()).OrderBy(x => x.Name).ToList();
            return trains.Select(train => Mapper<Train, TrainDto>.Map(train));
        }       

        public async Task UpdateTrainAsync(TrainDto trainDto)
        {
            var train = Mapper<TrainDto, Train>.Map(trainDto);
            await _trainRepository.UpdateAsync(train);
            await _trainRepository.SaveChangesAsync();
        }

        public async Task DeleteTrainAsync(int trainId)
        {
            var train = await _trainRepository.GetByIdAsync(trainId);
            if (train != null)
            {
                await _trainRepository.DeleteAsync(trainId);
                await _trainRepository.SaveChangesAsync();
            }
        }
    }

}
