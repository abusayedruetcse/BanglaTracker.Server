using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;
using BanglaTracker.Core.Utils;

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

        public async Task CalculateMetricsAsync(int trainId)
        {
            var journey = await GetJourneyAsync(trainId);

            if (journey == null)
            {
                Console.WriteLine("Journey is not available");
                return;
            }

            UpdateJourneyProgress(journey);

            await _repository.UpdateAsync(journey);
            
            await _repository.SaveChangesAsync();
        }

        #region JourneyProgressCalculator
        public void UpdateJourneyProgress(TrainJourney journey)
        {
            if (journey.CurrentStationIndex + 1 == journey.Stations.Count)
            {
                // skip if the train is at the destination station. 
                // TODO: remove this journey from timer
                return;
            }

            UpdateDistanceAndCurrentVelocity(journey);
            UpdateJourneyProgressPercent(journey);
            UpdatePossibleReachTime(journey);

            // started to run after taking a break
            if (journey.DistanceFromLastStation > 0)
            {
                UpdateForStartJourney(journey);
            }

            if (journey.JourneyProgressPercent >= 100)
            {
                UpdateForStopAtStation(journey);
            }
            
            journey.TotalTravelTime = journey.TotalTravelTime + TimeSpan.FromMinutes(5);    // TODO: move this inverval time in config file. use same value in background timer
        }
        
        private void UpdateDistanceAndCurrentVelocity(TrainJourney journey)
        {
            /* Velocity = (distance2 - distance1) / (t2 - t1) */

            var oldDistanceValue = journey.DistanceFromLastStation;

            journey.DistanceFromLastStation = CalculateDistance(journey);

            journey.CurrentSpeed = (journey.DistanceFromLastStation - oldDistanceValue) / 5; // TODO: move this inverval time in config file. use same value in background timer.
        }

        private void UpdateJourneyProgressPercent(TrainJourney journey)
        {
            var nextStationDistance = journey.Stations[journey.CurrentStationIndex + 1].Distance;
            journey.JourneyProgressPercent = (int)((nextStationDistance - journey.DistanceFromLastStation) * 100 / nextStationDistance);
        }

        private void UpdatePossibleReachTime(TrainJourney journey)
        {
            var nextStationDistance = journey.Stations[journey.CurrentStationIndex + 1].Distance;

            if (journey.CurrentSpeed > 0)
            {
                var possibleReachTime = (nextStationDistance - journey.DistanceFromLastStation) / journey.CurrentSpeed;
                journey.Stations[journey.CurrentStationIndex + 1].EstimatedArrivalTime = TimeSpan.FromMinutes(possibleReachTime);
            }

            for (int i = journey.CurrentStationIndex + 2; i < journey.Stations.Count; i++)
            {
                journey.Stations[i].EstimatedArrivalTime =
                    journey.Stations[i - 1].EstimatedArrivalTime + journey.Stations[i - 1].AverageBreakTime + journey.Stations[i].AverageTravelTime;
            }
        }

        private double CalculateDistance(TrainJourney journey)
        {
            // Actual distance calculation logic using current and last station location data
            var source = journey.Stations[journey.CurrentStationIndex].Location;
            var destination = journey.CurrentLocation;

            double distanceKm = LocationDistanceCalculator.CalculateDistance(source.Latitude, source.Longitude, destination.Latitude, destination.Longitude, 'K');

            return distanceKm; // Placeholder for distance calculation
        }
        
        private void UpdateForStartJourney(TrainJourney journey)
        {
            var currentStation = journey.Stations[journey.CurrentStationIndex];

            // Check if the train is stationary (e.g., paused at a station) and ready to log break time
            if (journey.IsAtStation)
            {
                // Calculate the time spent as a break at the current station
                var breakTime = journey.TotalTravelTime - journey.LastRecordedTravelTime;

                // Set the actual break time for the current station
                currentStation.ActualBreakTime = breakTime;

                // Update the average break time using the new break time
                currentStation.AverageBreakTime =
                    currentStation.AverageBreakTime == TimeSpan.Zero
                    ? breakTime
                    : (currentStation.AverageBreakTime * 9 + breakTime) / 10;

                // Update the last recorded travel time to mark this break
                journey.LastRecordedTravelTime = journey.TotalTravelTime;

                // Reset the IsAtStation flag, as break time has now been logged
                journey.IsAtStation = false;
            }
        }

        private void UpdateForStopAtStation(TrainJourney journey)
        {
            // Only update travel time if we've reached the next station (i.e., the journey progress is complete)
            if (journey.JourneyProgressPercent == 100)
            {
                var currentStation = journey.Stations[journey.CurrentStationIndex + 1];

                // Calculate travel time for reaching the current station
                var travelTime = journey.TotalTravelTime - journey.LastRecordedTravelTime;

                // Update current station's actual travel time
                currentStation.ActualTravelTime = travelTime;

                // Calculate a new average travel time, or set it for the first time if it's not already set
                currentStation.AverageTravelTime =
                    currentStation.AverageTravelTime == TimeSpan.Zero
                    ? travelTime
                    : (currentStation.AverageTravelTime * 9 + travelTime) / 10;

                // Update last recorded travel time for the next leg of the journey
                journey.LastRecordedTravelTime = journey.TotalTravelTime;

                // Reset journey progress for the next segment
                journey.CurrentStationIndex++;
                journey.JourneyProgressPercent = 0;
                journey.DistanceFromLastStation = 0;
                journey.CurrentSpeed = 0;
                journey.IsAtStation = true;                
            }
        }

        #endregion JourneyProgressCalculator
    }

}
