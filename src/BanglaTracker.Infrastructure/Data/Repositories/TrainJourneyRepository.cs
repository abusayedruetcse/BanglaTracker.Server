using BanglaTracker.Core.Enums;
using BanglaTracker.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BanglaTracker.Infrastructure.Data.Repositories
{
    public class TrainJourneyRepository : ITrainJourneyRepository
    {
        private readonly TrackerDbContext _context;

        public TrainJourneyRepository(TrackerDbContext context)
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

        public async Task InitializeJourneyStartAsync(
            int journeyId, 
            int stationIndex)
        {
            var journey = await _context.TrainJourneys.FindAsync(journeyId);

            if (journey == null)
            {
                throw new InvalidOperationException($"Journey with ID {journeyId} not found.");
            }

            // Assuming JourneyProgress includes fields like CurrentStationIndex and JourneyProgressPercent
            journey.CurrentStationIndex = stationIndex;
            
            _context.TrainJourneys.Update(journey);

            await UpdateJourneyStatusAsync(journeyId, JourneyStatus.InProgress);            
        }

        public async Task UpdateJourneyStatusAsync(
            int journeyId, 
            JourneyStatus journeyStatus)
        {
            var journeyTracking = await _context
                .TrainJourneyTrackings
                .FirstOrDefaultAsync(t => t.TrainJourneyId == journeyId);

            if (journeyTracking == null)
            {
                throw new InvalidOperationException($"Journey with ID {journeyId} not found.");
            }

            journeyTracking.Status = journeyStatus;
            journeyTracking.ModifyDateTime = DateTime.UtcNow;
            
            _context.TrainJourneyTrackings.Update(journeyTracking);
        }
    }

}
