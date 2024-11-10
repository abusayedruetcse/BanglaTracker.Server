namespace BanglaTracker.BLL.DTOs
{
    public class AppUserDto
    {
        public int Id { get; set; }
        public Guid InstallationId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastActiveDateTime { get; set; } 
        public DateTime CreatedDateTime { get; set; }
    }
}
