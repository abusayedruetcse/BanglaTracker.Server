using BanglaTracker.BLL.Interfaces;
using BanglaTracker.BLL.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BanglaTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/User/UpdateLastActive
        [HttpPost("UpdateLastActiveTime")]
        public async Task<IActionResult> UpdateLastActiveTime([FromBody] UpdateLastActiveRequest request)
        {
            if (request == null || request.InstallationID == Guid.Empty)
            {
                return BadRequest("Invalid request.");
            }

            try
            {
                // Assuming your UserService handles the logic for updating last active time
                var success = await _userService.UpdateLastActiveAsync(request.InstallationID, request.LastActiveTime);

                if (!success)
                {
                    return NotFound("User not found or failed to update.");
                }

                return Ok("Last active time updated successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
