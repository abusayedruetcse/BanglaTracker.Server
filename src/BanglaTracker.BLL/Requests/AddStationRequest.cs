

using BanglaTracker.BLL.Models;

namespace BanglaTracker.BLL.Requests
{
    public class AddStationRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LocationData Location { get; set; }
    }
}
