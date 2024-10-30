using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;

namespace BanglaTracker.BLL.Services
{
    public class TrainJourneyService : ITrainJourneyService
    {
        private readonly IRepository<TrainJourney> _repository;

        public TrainJourneyService(IRepository<TrainJourney> repository)
        {
            _repository = repository;
        }

        public async Task<TrainJourney> GetJourneyAsync(int trainId)
        {
            // Fetch the train journey data
            return await _repository.GetByIdAsync(trainId);
        }

        public async Task<TrainMetricsDto> CalculateMetricsAsync(int trainId)
        {
            var journey = await GetJourneyAsync(trainId);

            // Calculate metrics based on current journey
            var metrics = new TrainMetricsDto
            {
                TotalReachTime = CalculateTotalReachTime(journey),
                PercentageCovered = CalculatePercentageCovered(journey),
                CurrentVelocity = journey.CurrentVelocity,
                NextStationTime = CalculateNextStationTime(journey)
            };

            return metrics;
        }

        private TimeSpan CalculateTotalReachTime(TrainJourney journey)
        {
            // Logic to calculate the total reach time for each station
            return new TimeSpan();
        }

        private double CalculatePercentageCovered(TrainJourney journey)
        {
            // Logic to calculate percentage of distance covered
            return 0;
        }

        private TimeSpan CalculateNextStationTime(TrainJourney journey)
        {
            // Logic to calculate approximate time to reach the next station
            return new TimeSpan();
        }
    }

}
