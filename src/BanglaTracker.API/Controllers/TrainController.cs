using BanglaTracker.BLL.DTOs;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.BLL.Requests;
using BanglaTracker.BLL.Responses;
using BanglaTracker.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BanglaTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService _trainService;

        public TrainController(
            ITrainService trainService)
        {
            _trainService = trainService;
        }

        // Add Train
        [HttpPost("add-train")]
        public async Task<IActionResult> AddTrainAsync([FromBody] TrainDto trainDto)
        {
            if (string.IsNullOrEmpty(trainDto.Name))
                return BadRequest("Train name is required.");

            var train = await _trainService.AddTrainAsync(trainDto);

            return Ok(new { message = "Train added successfully", trainId =train.Id });
        }

        // Add Station
        [HttpPost("add-station")]
        public async Task<IActionResult> AddStationAsync([FromBody] AddStationRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return BadRequest("Station name is required.");

            var stationDto = Mapper<AddStationRequest, StationDto>.Map(request);

            // Call the service to add the station
            var addedStation = await _trainService.AddStationAsync(stationDto);

            return Ok(new { message = "Station added successfully", stationId = addedStation.Id });
        }

        // Get Train by ID
        [HttpGet("get-train/{trainId}")]
        public async Task<IActionResult> GetTrainAsync(int trainId)
        {
            var train = await _trainService.GetTrainByIdAsync(trainId);

            if (train == null)
                return NotFound("Train not found.");

            return Ok(train);
        }

        // Get Station by ID
        [HttpGet("get-station/{stationId}")]
        public async Task<IActionResult> GetStationAsync(int stationId)
        {
            var stationDto = await _trainService.GetStationByIdAsync(stationId);

            if (stationDto == null)
                return NotFound("Station not found.");

            var station = Mapper<StationDto, StationResponse>.Map(stationDto);

            return Ok(station);
        }

        // Get All Trains
        [HttpGet("get-all-trains")]
        public async Task<IActionResult> GetAllTrainsAsync()
        {
            var trains = await _trainService.GetAllTrainsAsync();
            
            return Ok(trains);
        }

        // Get All Stations
        [HttpGet("get-all-stations")]
        public async Task<IActionResult> GetAllStationsAsync()
        {
            var stationDtos = await _trainService.GetAllStationsAsync();

            var stations = stationDtos.Select(s => Mapper<StationDto, StationResponse>.Map(s));

            return Ok(stations);
        }
    }

}
