namespace BanglaTracker.Core.Enums
{
    public enum JourneyStatus
    {
        NotStarted = 0,        // Journey is planned but hasn't started yet
        InProgress = 1,        // Journey is currently underway
        StoppedAtStation = 2,  // Train is currently stopped at a station
        Delayed = 3,           // Journey is delayed for some reason (e.g., technical issues, bad weather)
        Completed = 4,         // Journey has been completed successfully
        Canceled = 5          // Journey has been canceled
    }
}
