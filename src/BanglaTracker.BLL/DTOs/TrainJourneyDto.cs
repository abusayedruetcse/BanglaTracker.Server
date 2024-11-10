
using BanglaTracker.BLL.Models;

namespace BanglaTracker.BLL.DTOs
{
    public class TrainJourneyDto
    {
        public int Id { get; set; }
        public List<StationDto> Stations { get; set; } = new List<StationDto>();
        public TimeSpan TotalTravelTime { get; set; } // Total time taken for the journey
        public TimeSpan LastRecordedTravelTime { get; set; }  // Last recorded travel time to calculate intervals between updates        
        public int CurrentStationIndex { get; set; } // Index of the current station
        public double CurrentSpeed { get; set; } // Current speed in kilometers per hour  
        public LocationData CurrentLocation { get; set; }   // Current position of the train
        public double DistanceFromLastStation { get; set; }  // Distance from the most recent station in kilometers
        public int JourneyProgressPercent { get; set; }  // Percentage of journey completed between the last and next station (e.g., 0%-100%)
        public bool IsAtStation { get; set; }
    }
}

