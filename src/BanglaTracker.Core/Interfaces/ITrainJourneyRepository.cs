using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanglaTracker.Core.Interfaces
{
    public interface ITrainJourneyRepository
    {
        Task<List<int>> FetchJourneyIdsByStatusAsync(
            JourneyStatus journeyStatus);

        Task InitializeJourneyStartAsync(
            int journeyId,
            int stationIndex);

        Task UpdateJourneyStatusAsync(
            int journeyId,
            JourneyStatus journeyStatus);
    }

}
