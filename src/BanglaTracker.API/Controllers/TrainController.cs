﻿using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.DTOs;
using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;
using BanglaTracker.Core.Requests;
using BanglaTracker.Core.Responses;
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
            ITrainService trainService, 
            IRepository<StationDto> stationRepository)
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
            trains = new List<TrainDto>()
            {
                new TrainDto() {Id = 1, Name = "Dhumketu"},
                new TrainDto() {Id = 2, Name = "Bonolota"},
                new TrainDto() {Id = 3, Name = "Padma"},
                new TrainDto() {Id = 4, Name = "Silkcity"},
                new TrainDto() {Id = 5, Name = "Kopotakkho"}
            };
            trains = trains.OrderBy(x => x.Name).ToList();
            return Ok(trains);
        }

        // Get All Stations
        [HttpGet("get-all-stations")]
        public async Task<IActionResult> GetAllStationsAsync()
        {
            var stationDtos = await _trainService.GetAllStationsAsync();

            var stations = stationDtos.Select(s => Mapper<StationDto, StationResponse>.Map(s));

            stations = new List<StationResponse>()
            {
                new StationResponse() {Id = 1, Name = "Abdullahpur"},
                new StationResponse() {Id = 2, Name = "BB Setu"},
                new StationResponse() {Id = 3, Name = "Zamuna"},
                new StationResponse() {Id = 4, Name = "Tangail"},
                new StationResponse() {Id = 5, Name = "ABC"},
                new StationResponse() {Id = 1, Name = "Abdullahpur"},
                new StationResponse() {Id = 2, Name = "BB Setu"},
                new StationResponse() {Id = 3, Name = "Zamuna"},
                new StationResponse() {Id = 4, Name = "Tangail"},
                new StationResponse() {Id = 5, Name = "ABC"},
                new StationResponse() {Id = 1, Name = "Abdullahpur"},
                new StationResponse() {Id = 2, Name = "BB Setu"},
                new StationResponse() {Id = 3, Name = "Zamuna"},
                new StationResponse() {Id = 4, Name = "Tangail"},
                new StationResponse() {Id = 5, Name = "ABC"},
                new StationResponse() {Id = 1, Name = "Abdullahpur"},
                new StationResponse() {Id = 2, Name = "BB Setu"},
                new StationResponse() {Id = 3, Name = "Zamuna"},
                new StationResponse() {Id = 4, Name = "Tangail"},
                new StationResponse() {Id = 5, Name = "ABC"},
                new StationResponse() {Id = 1, Name = "Abdullahpur"},
                new StationResponse() {Id = 2, Name = "BB Setu"},
                new StationResponse() {Id = 3, Name = "Zamuna"},
                new StationResponse() {Id = 4, Name = "Tangail"},
                new StationResponse() {Id = 5, Name = "ABC"}
            };
            stations = stations.OrderBy(x => x.Name).ToList();

            return Ok(stations);
        }
    }

}
