using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;

namespace BanglaTracker.BLL.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly IRepository<Station> _stationRepository;
        private readonly IRepository<Train> _trainRepository;
        private readonly IRepository<TrainJourney> _journeyRepository;
        private readonly IRepository<TrainJourneyDetail> _journeyDetailRepository;

        public DataImportService(
            IRepository<Station> stationRepository, 
            IRepository<Train> trainRepository,
            IRepository<TrainJourney> journeyRepository,
            IRepository<TrainJourneyDetail> journeyDetailRepository)
        {
            _stationRepository = stationRepository;
            _trainRepository = trainRepository;
            _journeyRepository = journeyRepository;
            _journeyDetailRepository = journeyDetailRepository;
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

        public async Task AddJourneysAsync(List<TrainJourney> journeys)
        {
            foreach (var journey in journeys)
            {
                await _journeyRepository.AddAsync(journey);
            }

            await _journeyRepository.SaveChangesAsync();
        }

        public async Task AddJourneyDetailsAsync(List<TrainJourneyDetail> routes)
        {
            foreach (var route in routes)
            {
                await _journeyDetailRepository.AddAsync(route);
            }

            await _journeyDetailRepository.SaveChangesAsync();
        }

    }

}
