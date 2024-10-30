namespace BanglaTracker.Core.Entities
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double DistanceFromStart { get; set; } // Distance from the source station in kilometers
        public TimeSpan BreakTime { get; set; } // Average break time at this station
    }
}
