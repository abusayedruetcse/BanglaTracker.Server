using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Enums;

namespace BanglaTracker.Core.Interfaces
{
    public interface ITrainJourneyRepository : IRepository<TrainJourney>
    {
        Task<List<int>> FetchJourneyIdsByStatusAsync(
            JourneyStatus journeyStatus);
    }

}
