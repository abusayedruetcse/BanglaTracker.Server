using BanglaTracker.BLL.DTOs;
using BanglaTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaTracker.BLL.Interfaces
{
    public interface ITrainJourneyService
    {
        Task<TrainJourney> GetJourneyAsync(int trainId);
        Task<TrainMetricsDto> CalculateMetricsAsync(int trainId);
    }
}
