namespace BanglaTracker.BLL.Requests
{
    public class StartJourneyRequest
    {
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string CurrentStation { get; set; }
        public string TrainNumber { get; set; }
        public Guid InstallationID { get; set; }
    }
}
