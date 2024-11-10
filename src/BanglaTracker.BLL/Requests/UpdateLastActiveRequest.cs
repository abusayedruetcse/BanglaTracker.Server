namespace BanglaTracker.BLL.Requests
{
    public class UpdateLastActiveRequest
    {
        public Guid InstallationID { get; set; }
        public DateTime LastActiveTime { get; set; }
    }
}

