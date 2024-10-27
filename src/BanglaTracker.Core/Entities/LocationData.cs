namespace BanglaTracker.Core.Entities
{
    public class LocationData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; } // Optional, to track when the location was recorded
    }
}
