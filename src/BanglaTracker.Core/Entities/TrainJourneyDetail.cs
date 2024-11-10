namespace BanglaTracker.Core.Entities
{
    public class TrainJourneyDetail
    {
        public int Id { get; set; }
        public int TrainJourneyId { get; set; }
        public int StationId { get; set; }
        public int RouteStationOrder { get; set; }
        public double Distance { get; set; }    // Distance in kilometers from the previous station
        public TimeSpan ActualTravelTime { get; set; }  // Actual time taken from the previous station in last trip
        public TimeSpan AverageTravelTime { get; set; }  // Average travel duration to this station
        public TimeSpan ActualBreakTime { get; set; }   // Actual duration spent at this station during the journey in last trip
        public TimeSpan AverageBreakTime { get; set; }  // Average break duration at this station over time
        public TimeSpan EstimatedArrivalTime { get; set; }  // Estimated possible time to arrive at this station        
    }
}