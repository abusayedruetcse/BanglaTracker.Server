using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.BLL.Models;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Enums;
using BanglaTracker.Core.Interfaces;
using BanglaTracker.Core.Utils;

namespace BanglaTracker.BLL.Services
{
    public class TrainJourneyService : ITrainJourneyService
    {
        private readonly IRepository<TrainJourneyDto> _repository;
        private readonly ITrainJourneyRepository _trainJourneyRepository;
        private readonly IRepository<TrainJourneyDetail> _trainJourneyDetailRepository;
        private readonly IRepository<Station> _stationRepository;
        private readonly ILocationRepository _locationRepository;

        public TrainJourneyService(
            IRepository<TrainJourneyDto> repository,
            ITrainJourneyRepository trainJourneyRepository,
            IRepository<TrainJourneyDetail> trainJourneyDetailRepository,
            IRepository<Station> stationRepository,
            ILocationRepository locationRepository)
        {
            _repository = repository;
            _trainJourneyRepository = trainJourneyRepository;
            _trainJourneyDetailRepository = trainJourneyDetailRepository;
            _stationRepository = stationRepository;
            _locationRepository = locationRepository;
        }

        public async Task<TrainJourney> GetJourneyAsync(int journeyId)
        {
            // Fetch the train journey data
            var journey = await _trainJourneyRepository.GetByIdAsync(journeyId);
            if (journey == null)
            {
                throw new KeyNotFoundException($"Journey not found with id: {journeyId}");
            }
            return journey;
        }

        public async Task<List<int>> GetJourneyIdsByStatusAsync(JourneyStatus journeyStatus)
        {
            // Fetch the train journey data
            return await _trainJourneyRepository.FetchJourneyIdsByStatusAsync(journeyStatus);
        }

        #region Calculate Journey Metrics
        public async Task CalculateMetricsAsync(int journeyId)
        {
            // Step-1: Load Journey in Dto
            var journey = await GetTrainJourneyDtoAsync(journeyId);

            if (journey == null)
            {
                Console.WriteLine("Journey is not available");
                return;
            }

            // Step-2: Process JourneyDto
            if (IsAtDestination(journey))
            {
                await UpdateJourneyStatusAsync(journey.Id, JourneyStatus.Completed);
            }

            // Update journey with currentlocation before starting progress.
            await UpdateCurrentLocation(journey);

            UpdateJourneyProgress(journey);

            // Step-3: Store JourneyDto in DB
            await SaveTrainJourneyAsync(journey);

            await _trainJourneyRepository.SaveChangesAsync();
            await _trainJourneyDetailRepository.SaveChangesAsync();
        }

        private async Task<TrainJourneyDto> GetTrainJourneyDtoAsync(int journeyId)
        {
            // Load the TrainJourney data
            var trainJourney = await _trainJourneyRepository.GetByIdAsync(journeyId);
            if (trainJourney == null)
            {
                throw new Exception($"TrainJourney with Id {journeyId} not found.");
            }

            // Load associated TrainJourneyDetail data
            var trainJourneyDetails = (await _trainJourneyDetailRepository
                .FindAsync(d => d.TrainJourneyId == journeyId))
                .OrderBy(s => s.RouteStationOrder);

            // Load station information
            var stationIds = trainJourneyDetails.Select(d => d.StationId).Distinct().ToList();
            var stations = await _stationRepository.FindAsync(s => stationIds.Contains(s.Id));

            // Build the DTO
            var trainJourneyDto = new TrainJourneyDto
            {
                Id = trainJourney.Id,
                TotalTravelTime = trainJourney.TotalTravelTime,
                LastRecordedTravelTime = trainJourney.LastRecordedTravelTime,
                CurrentStationIndex = trainJourney.CurrentStationIndex,
                CurrentSpeed = trainJourney.CurrentSpeed,
                DistanceFromLastStation = trainJourney.DistanceFromLastStation,
                JourneyProgressPercent = trainJourney.JourneyProgressPercent,
                IsAtStation = trainJourney.IsAtStation,
                CurrentLocation = new LocationData
                {
                    Latitude = trainJourney.Latitude,
                    Longitude = trainJourney.Longitude
                },
                Stations = trainJourneyDetails.Select(detail =>
                {
                    var station = stations.FirstOrDefault(s => s.Id == detail.StationId);
                    return new StationDto
                    {
                        Id = detail.StationId,
                        Name = station?.Name ?? "Unknown",
                        Location = new LocationData
                        {
                            Latitude = station?.Latitude ?? 0,
                            Longitude = station?.Longitude ?? 0
                        },
                        Distance = detail.Distance,
                        ActualTravelTime = detail.ActualTravelTime,
                        AverageTravelTime = detail.AverageTravelTime,
                        ActualBreakTime = detail.ActualBreakTime,
                        AverageBreakTime = detail.AverageBreakTime,
                        EstimatedArrivalTime = detail.EstimatedArrivalTime
                    };
                }).OrderBy(s => s.Id).ToList()
            };

            return trainJourneyDto;
        }

        private async Task UpdateJourneyStatusAsync(
            int journeyId,
            JourneyStatus journeyStatus)
        {
            var journeyTracking = await _trainJourneyRepository.GetByIdAsync(journeyId);

            if (journeyTracking == null)
            {
                throw new InvalidOperationException($"Journey with ID {journeyId} not found.");
            }

            journeyTracking.Status = (int)journeyStatus;
            journeyTracking.TrackingDateTime = DateTime.UtcNow;

            await _trainJourneyRepository.UpdateAsync(journeyTracking);
        }

        private async Task UpdateCurrentLocation(TrainJourneyDto journey)
        {
            var tracking = await _trainJourneyRepository.GetByIdAsync(journey.Id);

            var locationData = await _locationRepository.FetchLocationByInstallationIDAsync(tracking?.SensorNumber ?? Guid.Empty);

            if (locationData != null)
            {
                journey.CurrentLocation = locationData;
            }
        }

        private async Task SaveTrainJourneyAsync(TrainJourneyDto journeyDto)
        {
            // Map TrainJourneyDto to TrainJourney
            var trainJourney = await _trainJourneyRepository.GetByIdAsync(journeyDto.Id) ?? new TrainJourney();

            trainJourney.Id = journeyDto.Id;
            trainJourney.TotalTravelTime = journeyDto.TotalTravelTime;
            trainJourney.LastRecordedTravelTime = journeyDto.LastRecordedTravelTime;
            trainJourney.CurrentStationIndex = journeyDto.CurrentStationIndex;
            trainJourney.CurrentSpeed = journeyDto.CurrentSpeed;
            trainJourney.DistanceFromLastStation = journeyDto.DistanceFromLastStation;
            trainJourney.JourneyProgressPercent = journeyDto.JourneyProgressPercent;
            trainJourney.IsAtStation = journeyDto.IsAtStation;
            trainJourney.Latitude = journeyDto.CurrentLocation.Latitude;
            trainJourney.Longitude = journeyDto.CurrentLocation.Longitude;

            // Save TrainJourney
            if (trainJourney.Id == 0)
            {
                Console.WriteLine("Journey not found");
            }
            else
            {
                await _trainJourneyRepository.UpdateAsync(trainJourney);
            }

            // Get all existing TrainJourneyDetail records for this journey
            var existingDetails = await _trainJourneyDetailRepository.FindAsync(d => d.TrainJourneyId == trainJourney.Id);

            // Map TrainJourneyDto.Stations to TrainJourneyDetail
            var newDetails = journeyDto.Stations.Select(station => new TrainJourneyDetail
            {
                Id = existingDetails.FirstOrDefault(d => d.StationId == station.Id)?.Id ?? 0, // Preserve existing ID if it exists
                TrainJourneyId = trainJourney.Id,
                StationId = station.Id,
                ActualTravelTime = station.ActualTravelTime,
                AverageTravelTime = station.AverageTravelTime,
                ActualBreakTime = station.ActualBreakTime,
                AverageBreakTime = station.AverageBreakTime,
                EstimatedArrivalTime = station.EstimatedArrivalTime
            }).ToList();

            // Update or Insert TrainJourneyDetail
            foreach (var detail in newDetails)
            {
                if (detail.Id == 0)
                {
                    Console.WriteLine("Station not found");
                }
                else
                {
                    await _trainJourneyDetailRepository.UpdateAsync(detail);
                }
            }            
        }

        #endregion

        #region JourneyProgressCalculator

        /// <summary>
        /// Updates the journey progress for the train, calculating distance, speed, and 
        /// updating progress percentage and estimated arrival times for stations.
        /// </summary>
        public void UpdateJourneyProgress(TrainJourneyDto journey)
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
        private void UpdateDistanceAndCurrentVelocity(TrainJourneyDto journey)
        {
            var previousDistance = journey.DistanceFromLastStation;
            journey.DistanceFromLastStation = CalculateDistance(journey);

            // Update speed based on distance covered over the timer interval
            journey.CurrentSpeed = (journey.DistanceFromLastStation - previousDistance) / 5; // TODO: Move interval time to config
        }

        /// <summary>
        /// Updates the journey progress as a percentage of the distance covered toward the next station.
        /// </summary>
        private void UpdateJourneyProgressPercent(TrainJourneyDto journey)
        {
            var nextStationDistance = journey.Stations[journey.CurrentStationIndex + 1].Distance;
            journey.JourneyProgressPercent = 100 - (int)((nextStationDistance - journey.DistanceFromLastStation) * 100 / nextStationDistance);
        }

        /// <summary>
        /// Updates estimated arrival times for the next station and subsequent stations based on current speed.
        /// </summary>
        private void UpdatePossibleReachTime(TrainJourneyDto journey)
        {           
            if (journey.CurrentSpeed > 0)
            {
                var nextStationDistance = journey.Stations[journey.CurrentStationIndex + 1].Distance;

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
        private double CalculateDistance(TrainJourneyDto journey)
        {
            var source = journey.Stations[journey.CurrentStationIndex].Location;
            var destination = journey.CurrentLocation;
            return LocationDistanceCalculator.CalculateDistance(source.Latitude, source.Longitude, destination.Latitude, destination.Longitude, 'K');
        }

        /// <summary>
        /// Starts the journey after a stop, calculating any break time spent at the station.
        /// </summary>
        private void StartJourneyFromStation(TrainJourneyDto journey)
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
        private void StopJourneyAtStation(TrainJourneyDto journey)
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
        private bool IsAtDestination(TrainJourneyDto journey)
        {
            return journey.CurrentStationIndex + 1 >= journey.Stations.Count;
        }

        /// <summary>
        /// Checks if the train is entering the radius of the current station (e.g., within 0.5 km).
        /// </summary>
        private bool IsTrainEnteringStationArea(TrainJourneyDto journey)
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
        private bool IsTrainReachingStation(TrainJourneyDto journey)
        {
            var distanceToNextStation = journey.Stations[journey.CurrentStationIndex].Distance - journey.DistanceFromLastStation;
            var stationReachThreshold = 0.5; // TODO: Move this threshold to configuration

            return distanceToNextStation < stationReachThreshold && !journey.IsAtStation;
        }

        /// <summary>
        /// Checks if the train has moved away from the current station’s radius.
        /// </summary>
        private bool HasTrainMovedAwayFromStation(TrainJourneyDto journey)
        {
            var movedAwayThreshold = 0.5; // TODO: Move this threshold to configuration

            return journey.DistanceFromLastStation > movedAwayThreshold;
        }

        #endregion JourneyProgressCalculator

        public async Task<(bool hasGranted, string message)> StartJourneyAsync(
            string fromStation,
            string toStation,
            string currentStation,
            string trainNumber,
            Guid sensorNumber)
        {
            // Validaton user to avoid multiple sensor for a single journey.
            var isSuccess = await UpdateTrackingToStartJourneyAsync(1, sensorNumber); // TODO: find the journey ID and replace here.
            if (!isSuccess)
            {
                return (false, "User is not allowed to start this journey.");
            }

            await InitializeJourneyStartAsync(1, 1); // TODO: find these journeyId, stationIdx and pass values
            return (true, "Journey started successfully.");
        }

        public async Task<bool> UpdateTrackingToStartJourneyAsync(
            int journeyId,
            Guid sensorNumber)
        {
            var journeyTracking = await _trainJourneyRepository.GetByIdAsync(journeyId);

            if (journeyTracking == null)
            {
                throw new InvalidOperationException($"Journey with ID {journeyId} not found.");
            }

            if (journeyTracking.SensorNumber == Guid.Empty)
            {
                journeyTracking.SensorNumber = sensorNumber;
                journeyTracking.Status = (int)JourneyStatus.InProgress;
                journeyTracking.TrackingDateTime = DateTime.UtcNow;

                await _trainJourneyRepository.UpdateAsync(journeyTracking);
                return true;
            }
            
            return false;
        }

        private async Task InitializeJourneyStartAsync(
            int journeyId,
            int stationIndex)
        {
            var journey = await _trainJourneyRepository.GetByIdAsync(journeyId);

            if (journey == null)
            {
                throw new InvalidOperationException($"Journey with ID {journeyId} not found.");
            }

            // Assuming JourneyProgress includes fields like CurrentStationIndex
            journey.CurrentStationIndex = stationIndex;
            
            // Considering that sensor is at the station now,
            // Need to calculate the arrival time for the next station, before starting the journey from station.
            // EstimatedTime(1) = AverageBreakTime(0) + AverageTravelTime(1)
            // Here, ignoring AverageBreakTime(0) (to mitigate the risk)
            //journey.Stations[stationIndex + 1].EstimatedArrivalTime = journey.Stations[stationIndex + 1].AverageTravelTime;

            var nextStation = (await _trainJourneyDetailRepository
                .FindAsync(td => td.TrainJourneyId == journeyId 
                    && td.RouteStationOrder == stationIndex + 1))
                .FirstOrDefault();
            if (nextStation != null)
            {
                nextStation.EstimatedArrivalTime = nextStation.AverageTravelTime;
                await _trainJourneyDetailRepository.UpdateAsync(nextStation);
                await _trainJourneyDetailRepository.SaveChangesAsync();
            }

            await _trainJourneyRepository.UpdateAsync(journey);

            await _trainJourneyRepository.SaveChangesAsync();
        }
        
    }

}
