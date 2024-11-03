namespace BanglaTracker.Core.Requests
{
    public class UpdateLastActiveRequest
    {
        public string InstallationID { get; set; }
        public DateTime LastActiveTime { get; set; }
    }
}

