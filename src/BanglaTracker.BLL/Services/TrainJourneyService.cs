using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Enums;
using BanglaTracker.Core.Interfaces;
using BanglaTracker.Core.Utils;

namespace BanglaTracker.BLL.Services
{
    public class TrainJourneyService : ITrainJourneyService
    {
        private readonly IRepository<TrainJourney> _repository;
        private readonly ITrainJourneyRepository _trainJourneyRepository;

        public TrainJourneyService(
            IRepository<TrainJourney> repository, 
            ITrainJourneyRepository trainJourneyRepository)
        {
            _repository = repository;
            _trainJourneyRepository = trainJourneyRepository;
        }

        public async Task<TrainJourney> GetJourneyAsync(int trainId)
        {
            // Fetch the train journey data
            return await _repository.GetByIdAsync(trainId);
        }

        public async Task<List<int>> GetJourneyIdsByStatusAsync(JourneyStatus journeyStatus)
        {
            // Fetch the train journey data
            return await _trainJourneyRepository.FetchJourneyIdsByStatusAsync(journeyStatus);
        }

        public async Task CalculateMetricsAsync(int trainId)
        {
            var journey = await GetJourneyAsync(trainId);

            if (journey == null)
            {
                Console.WriteLine("Journey is not available");
                return;
            }

            if (IsAtDestination(journey))
            {
                await _trainJourneyRepository.UpdateJourneyStatusAsync(journey.Id, JourneyStatus.Completed);
            }

            UpdateJourneyProgress(journey);

            await _repository.UpdateAsync(journey);
            
            await _repository.SaveChangesAsync();
        }

        #region JourneyProgressCalculator

        /// <summary>
        /// Updates the journey progress for the train, calculating distance, speed, and 
        /// updating progress percentage and estimated arrival times for stations.
        /// </summary>
        public void UpdateJourneyProgress(TrainJourney journey)
        {
            // Increment total travel time by the configured timer interval
            journey.TotalTravelTime += TimeSpan.FromMinutes(5);  // TODO: Move interval time to configuration

            // Step 1: Update current distance from the last station and current speed
            UpdateDistanceAndCurrentVelocity(journey);

            if (IsTrainEnteringStationArea(journey))
            {
                // If the train is close to a station, treat it as "stopped" and exit
                return;
            }

            // Step 2: Update the journey progress as a percentage and possible reach times
            UpdateJourneyProgressPercent(journey);
            UpdatePossibleReachTime(journey);

            // Step 3: If close to a station, record the stop
            if (IsTrainReachingStation(journey))
            {
                StopJourneyAtStation(journey);
            }

            // Step 4: If the train has moved away from the station, update for starting journey
            if (HasTrainMovedAwayFromStation(journey))
            {
                StartJourneyFromStation(journey);
            }
        }        

        /// <summary>
        /// Calculates and updates the train’s current distance from the last station and speed.
        /// </summary>
        private void UpdateDistanceAndCurrentVelocity(TrainJourney journey)
        {
            var previousDistance = journey.DistanceFromLastStation;
            journey.DistanceFromLastStation = CalculateDistance(journey);

            // Update speed based on distance covered over the timer interval
            journey.CurrentSpeed = (journey.DistanceFromLastStation - previousDistance) / 5; // TODO: Move interval time to config
        }

        /// <summary>
        /// Updates the journey progress as a percentage of the distance covered toward the next station.
        /// </summary>
        private void UpdateJourneyProgressPercent(TrainJourney journey)
        {
            var nextStationDistance = journey.Stations[journey.CurrentStationIndex + 1].Distance;
            journey.JourneyProgressPercent = 100 - (int)((nextStationDistance - journey.DistanceFromLastStation) * 100 / nextStationDistance);
        }

        /// <summary>
        /// Updates estimated arrival times for the next station and subsequent stations based on current speed.
        /// </summary>
        private void UpdatePossibleReachTime(TrainJourney journey)
        {
            var nextStationDistance = journey.Stations[journey.CurrentStationIndex + 1].Distance;

            if (journey.CurrentSpeed > 0)
            {
                // Estimate time to reach the next station based on current speed
                var estimatedArrivalInMinutes = (nextStationDistance - journey.DistanceFromLastStation) / journey.CurrentSpeed;
                journey.Stations[journey.CurrentStationIndex + 1].EstimatedArrivalTime = TimeSpan.FromMinutes(estimatedArrivalInMinutes);
            }

            // Update subsequent stations’ estimated arrival times based on averages
            for (int i = journey.CurrentStationIndex + 2; i < journey.Stations.Count; i++)
            {
                journey.Stations[i].EstimatedArrivalTime =
                    journey.Stations[i - 1].EstimatedArrivalTime + journey.Stations[i - 1].AverageBreakTime + journey.Stations[i].AverageTravelTime;
            }
        }

        /// <summary>
        /// Calculates the distance between the current train location and the last station location.
        /// </summary>
        private double CalculateDistance(TrainJourney journey)
        {
            var source = journey.Stations[journey.CurrentStationIndex].Location;
            var destination = journey.CurrentLocation;
            return LocationDistanceCalculator.CalculateDistance(source.Latitude, source.Longitude, destination.Latitude, destination.Longitude, 'K');
        }

        /// <summary>
        /// Starts the journey after a stop, calculating any break time spent at the station.
        /// </summary>
        private void StartJourneyFromStation(TrainJourney journey)
        {
            if (journey.IsAtStation)
            {
                var currentStation = journey.Stations[journey.CurrentStationIndex];
                var breakTime = journey.TotalTravelTime - journey.LastRecordedTravelTime;

                // Update break time records
                currentStation.ActualBreakTime = breakTime;
                currentStation.AverageBreakTime =
                    currentStation.AverageBreakTime == TimeSpan.Zero
                    ? breakTime
                    : (currentStation.AverageBreakTime * 9 + breakTime) / 10;

                journey.LastRecordedTravelTime = journey.TotalTravelTime;
                journey.IsAtStation = false;
            }
        }

        /// <summary>
        /// Records a stop at a station, updating the actual and average travel times.
        /// </summary>
        private void StopJourneyAtStation(TrainJourney journey)
        {
            var currentStation = journey.Stations[journey.CurrentStationIndex + 1];
            var travelTime = journey.TotalTravelTime - journey.LastRecordedTravelTime;

            // Update travel time records for the station
            currentStation.ActualTravelTime = travelTime;
            currentStation.AverageTravelTime =
                currentStation.AverageTravelTime == TimeSpan.Zero
                ? travelTime
                : (currentStation.AverageTravelTime * 9 + travelTime) / 10;

            journey.LastRecordedTravelTime = journey.TotalTravelTime;

            // Reset journey progress for the next segment
            journey.CurrentStationIndex++;
            journey.JourneyProgressPercent = 0;
            journey.DistanceFromLastStation = 0;
            journey.CurrentSpeed = 0;
            journey.IsAtStation = true;
        }

        /// <summary>
        /// Checks if the train has reached its final destination.
        /// </summary>
        private bool IsAtDestination(TrainJourney journey)
        {
            return journey.CurrentStationIndex + 1 >= journey.Stations.Count;
        }

        /// <summary>
        /// Checks if the train is entering the radius of the current station (e.g., within 0.5 km).
        /// </summary>
        private bool IsTrainEnteringStationArea(TrainJourney journey)
        {
            var distanceToStation = journey.DistanceFromLastStation;
            var stationRadiusThreshold = 0.5; // TODO: Move this threshold to configuration

            return distanceToStation < stationRadiusThreshold ||
                   (journey.Stations[journey.CurrentStationIndex].Distance - distanceToStation < stationRadiusThreshold 
                    && journey.IsAtStation);
        }

        /// <summary>
        /// Checks if the train is close enough to the next station to be considered as reaching it.
        /// </summary>
        private bool IsTrainReachingStation(TrainJourney journey)
        {
            var distanceToNextStation = journey.Stations[journey.CurrentStationIndex].Distance - journey.DistanceFromLastStation;
            var stationReachThreshold = 0.5; // TODO: Move this threshold to configuration

            return distanceToNextStation < stationReachThreshold && !journey.IsAtStation;
        }

        /// <summary>
        /// Checks if the train has moved away from the current station’s radius.
        /// </summary>
        private bool HasTrainMovedAwayFromStation(TrainJourney journey)
        {
            var movedAwayThreshold = 0.5; // TODO: Move this threshold to configuration

            return journey.DistanceFromLastStation > movedAwayThreshold;
        }

        #endregion JourneyProgressCalculator


    }

}
