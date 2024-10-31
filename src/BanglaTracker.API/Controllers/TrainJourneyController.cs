using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
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
