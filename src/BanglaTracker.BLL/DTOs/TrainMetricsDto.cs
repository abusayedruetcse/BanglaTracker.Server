namespace BanglaTracker.BLL.DTOs
{
    public class TrainMetricsDto
    {
        public TimeSpan TotalReachTime { get; set; }
        public double PercentageCovered { get; set; }
        public double CurrentVelocity { get; set; }
        public TimeSpan NextStationTime { get; set; }
    }

}
