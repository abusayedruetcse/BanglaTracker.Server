namespace BanglaTracker.Core.Entities
{
    public class AppUser
    {
        public Guid InstallationId { get; set; }
        public DateTime LastActiveDateTime { get; set; } 
        public DateTime CreatedDateTime { get; set; }
    }
}
