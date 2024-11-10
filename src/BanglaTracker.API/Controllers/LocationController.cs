using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BanglaTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;
        private readonly ILocationService _locationService;

        public LocationController(
            ILogger<LocationController> logger,
            ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }

        // POST api/location
        [HttpPost]
        public async Task<IActionResult> TrackLocation([FromBody] AppUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Location data is required.");
            }

            await _locationService.SaveLocationAsync(userDto);

            _logger.LogInformation($"Received location: Lat={userDto.Latitude}, Lon={userDto.Longitude}, Timestamp={userDto.LastActiveDateTime}");

            // Return a success response
            return Ok(new { Message = "Location received successfully" });
        }
    }
}
