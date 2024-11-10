using BanglaTracker.Core.Enums;

namespace BanglaTracker.Core.Entities
{
    public class TrainJourney
    {
        public int Id { get; set; }
        public int TrainId { get; set; }
        public Guid SensorNumber { get; set; } // Nullable if not always required
        public int Status { get; set; } = (int) JourneyStatus.NotStarted;
        public DateTime? TrackingDateTime { get; set; }
        public TimeSpan TotalTravelTime { get; set; } // Total time taken for the journey
        public TimeSpan LastRecordedTravelTime { get; set; }  // Last recorded travel time to calculate intervals between updates        
        public int CurrentStationIndex { get; set; } // Index of the current station
        public double CurrentSpeed { get; set; } // Current speed in kilometers per hour  
        public double Latitude { get; set; }    // Current position of the train
        public double Longitude { get; set; }   // Current position of the train
        public double DistanceFromLastStation { get; set; }  // Distance from the most recent station in kilometers
        public int JourneyProgressPercent { get; set; }  // Percentage of journey completed between the last and next station (e.g., 0%-100%)
        public bool IsAtStation { get; set; }
    }
}