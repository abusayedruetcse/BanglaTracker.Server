using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Enums;
using BanglaTracker.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BanglaTracker.Infrastructure.Data.Repositories
{
    public class TrainJourneyRepository : Repository<TrainJourney>, ITrainJourneyRepository
    {
        private readonly TrackerDbContext _context;

        public TrainJourneyRepository(TrackerDbContext context) : base(context) // Pass context to the base constructor
        {
            _context = context;
        }

        public async Task<List<int>> FetchJourneyIdsByStatusAsync(JourneyStatus journeyStatus)
        {
            return await _context.TrainJourneys
                .Where(journey => journey.Status == (int)journeyStatus)
                .Select(j => j.Id)
                .ToListAsync();
        }      
    }

}
