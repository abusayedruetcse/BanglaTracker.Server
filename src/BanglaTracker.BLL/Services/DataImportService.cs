using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;

namespace BanglaTracker.BLL.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly IRepository<Station> _stationRepository;
        private readonly IRepository<Train> _trainRepository;

        public DataImportService(
            IRepository<Station> stationRepository, 
            IRepository<Train> trainRepository)
        {
            _stationRepository = stationRepository;
            _trainRepository = trainRepository;
        }

        public async Task AddStationsAsync(List<Station> stations)
        {
            foreach (var station in stations)
            {
                await _stationRepository.AddAsync(station);
            }
                        
            await _stationRepository.SaveChangesAsync();
        }

        public async Task AddTrainsAsync(List<Train> trains)
        {
            foreach (var train in trains)
            {
                await _trainRepository.AddAsync(train);
            }

            await _trainRepository.SaveChangesAsync();
        }
    }

}
