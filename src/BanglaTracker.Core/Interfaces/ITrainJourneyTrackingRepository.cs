using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Enums;

namespace BanglaTracker.Core.Interfaces
{
    public interface ITrainJourneyTrackingRepository : IRepository<TrainJourneyTracking>
    {
        Task<List<int>> FetchJourneyIdsByStatusAsync(
            JourneyStatus journeyStatus);

        Task<TrainJourneyTracking?> GetJourneyTrackingByJourneyIdAsync(
            int journeyId);
    }

}
