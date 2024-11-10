
namespace BanglaTracker.Core.Entities
{
    public class B2bTrainJourney
    {
        public int Id { get; set; }
        public int JourneyId { get; set; }
        public int NextJourneyId { get; set; }
        public int PreviousJourneyId { get; set; }
    }
}