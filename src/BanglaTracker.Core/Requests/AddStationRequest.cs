
using BanglaTracker.Core.Entities;

namespace BanglaTracker.Core.Requests
{
    public class AddStationRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LocationData Location { get; set; }
    }
}
