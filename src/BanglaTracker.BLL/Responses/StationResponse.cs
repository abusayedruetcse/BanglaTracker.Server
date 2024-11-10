

using BanglaTracker.BLL.Models;

namespace BanglaTracker.BLL.Responses
{
    public class StationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LocationData Location { get; set; }
    }
}
