using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BanglaTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainJourneyController : ControllerBase
    {
        private readonly ITrainJourneyService _trainJourneyService;

        public TrainJourneyController(ITrainJourneyService trainJourneyService)
        {
            _trainJourneyService = trainJourneyService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartJourney([FromBody] StartJourneyRequest request)
        {
            var (hasGranted, message) = await _trainJourneyService.StartJourneyAsync(
                request.FromStation,
                request.ToStation,
                request.CurrentStation,
                request.TrainNumber,
                request.InstallationID);

            if (!hasGranted)
            {
                return Unauthorized(new { message });
            }

            return Ok(new { message });
        }

        [HttpGet("{trainId}")]
        public async Task<ActionResult<TrainJourney>> GetTrainJourney(int trainId)
        {
            var journey = await _trainJourneyService.GetJourneyAsync(trainId);
            return Ok(journey);
        }

        [HttpGet("{trainId}/metrics")]
        public async Task<ActionResult<TrainMetricsDto>> GetTrainMetrics(int trainId)
        {
            //var metrics = await _trainJourneyService.CalculateMetricsAsync(trainId);
            //return Ok(metrics);
            return Ok();
        }
    }

}
