namespace BanglaTracker.Core.Entities
{
    public class LocationData
    {
        public Guid InstallationId { get; set; } // Primary key
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime ModifiedDateTime { get; set; } // Optional, to track when the location was recorded
    }
}
