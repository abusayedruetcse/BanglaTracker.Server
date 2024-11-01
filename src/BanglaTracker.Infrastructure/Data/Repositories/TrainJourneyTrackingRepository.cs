using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Enums;
using BanglaTracker.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BanglaTracker.Infrastructure.Data.Repositories
{
    public class TrainJourneyTrackingRepository : Repository<TrainJourneyTracking>, ITrainJourneyTrackingRepository
    {
        private readonly TrackerDbContext _context;

        public TrainJourneyTrackingRepository(TrackerDbContext context) : base(context) // Pass context to the base constructor
        {
            _context = context;
        }

        public async Task<List<int>> FetchJourneyIdsByStatusAsync(JourneyStatus journeyStatus)
        {
            return await _context.TrainJourneyTrackings
                .Where(journey => journey.Status == journeyStatus)
                .Select(j => j.Id)
                .ToListAsync();
        }

        public async Task<TrainJourneyTracking?> GetJourneyTrackingByJourneyIdAsync(
            int journeyId)
        {
            var journeyTracking = await _context
                .TrainJourneyTrackings
                .FirstOrDefaultAsync(t => t.TrainJourneyId == journeyId);

            return journeyTracking;
        }       
    }

}
