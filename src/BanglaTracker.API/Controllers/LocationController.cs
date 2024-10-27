using BanglaTracker.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanglaTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger)
        {
            _logger = logger;
        }

        // POST api/location
        [HttpPost]
        public IActionResult PostLocation([FromBody] LocationData locationData)
        {
            if (locationData == null)
            {
                return BadRequest("Location data is required.");
            }

            // Here, you could save the location data to a database or process it as needed.
            // For example:
            // _locationService.SaveLocation(locationData);

            _logger.LogInformation($"Received location: Lat={locationData.Latitude}, Lon={locationData.Longitude}, Timestamp={locationData.Timestamp}");

            // Return a success response
            return Ok(new { Message = "Location received successfully" });
        }
    }
}
