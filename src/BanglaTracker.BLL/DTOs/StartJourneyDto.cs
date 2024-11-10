namespace BanglaTracker.BLL.DTOs
{
    public class StartJourneyDto
    {
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string CurrentStation { get; set; }
        public string TrainNumber { get; set; }
        public Guid InstallationID { get; set; }
    }
}
