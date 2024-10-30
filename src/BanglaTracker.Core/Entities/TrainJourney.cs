using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaTracker.Core.Entities
{
    public class TrainJourney
    {
        public int TrainId { get; set; }
        public List<Station> Stations { get; set; } = new List<Station>();
        public TimeSpan TotalTravelTime { get; set; } // Total time taken for the journey
        public TimeSpan TimeSpentAtStations { get; set; } // Total time spent at stations
        public double CurrentVelocity { get; set; } // Current velocity in km/h
        public int CurrentStationIndex { get; set; } // Index of the current station
    }
}
