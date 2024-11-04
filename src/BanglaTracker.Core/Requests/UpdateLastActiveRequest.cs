namespace BanglaTracker.Core.Requests
{
    public class UpdateLastActiveRequest
    {
        public Guid InstallationID { get; set; }
        public DateTime LastActiveTime { get; set; }
    }
}

