namespace BanglaTracker.Core.Enums
{
    public enum JourneyStatus
    {
        NotStarted,        // Journey is planned but hasn't started yet
        InProgress,        // Journey is currently underway
        StoppedAtStation,  // Train is currently stopped at a station
        Delayed,           // Journey is delayed for some reason (e.g., technical issues, bad weather)
        Completed,         // Journey has been completed successfully
        Canceled           // Journey has been canceled
    }
}
