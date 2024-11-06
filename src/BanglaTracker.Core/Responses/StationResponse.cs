using BanglaTracker.Core.Entities;

namespace BanglaTracker.Core.Responses
{
    public class StationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LocationData Location { get; set; }
    }
}
