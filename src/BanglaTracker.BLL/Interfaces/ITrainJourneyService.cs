﻿using BanglaTracker.BLL.DTOs;

using BanglaTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaTracker.BLL.Interfaces
{
    public interface ITrainJourneyService
    {
        Task<TrainJourneyDto> GetJourneyAsync(int trainId);

        Task CalculateMetricsAsync(int trainId);

        Task<List<int>> GetJourneyIdsByStatusAsync(
            JourneyStatus journeyStatus);

        Task<(bool hasGranted, string message)> StartJourneyAsync(
            string fromStation,
            string toStation,
            string currentStation,
            string trainNumber,
            Guid sensorNumber);
    }
}
