using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;

namespace BanglaTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataImportController : ControllerBase
    {
        private readonly IDataImportService _dataImportService;

        public DataImportController(
            IDataImportService dataImportService)
        {
            _dataImportService = dataImportService;
        }

        // Endpoint to initialize data
        [HttpPost("initialize")]
        public async Task<IActionResult> InitializeData()
        {
            var stations = new List<Station>
            {
                new Station { Id = 1, Name = "Dhaka", Longitude = 90.4125, Latitude = 23.8103 },
                new Station { Id = 2, Name = "Biman_Bandar", Longitude = 90.4086, Latitude = 23.8431 },
                new Station { Id = 3, Name = "Joydebpur", Longitude = 90.4316, Latitude = 23.9983 },
                new Station { Id = 4, Name = "Tangail", Longitude = 89.9261, Latitude = 24.2499 },
                new Station { Id = 5, Name = "BBSetu East", Longitude = 89.7570, Latitude = 24.4823 },
                new Station { Id = 6, Name = "SH M Monsur Ali", Longitude = 89.9261, Latitude = 24.2499 },
                new Station { Id = 7, Name = "Jamtail", Longitude = 89.7570, Latitude = 24.4823 },
                new Station { Id = 8, Name = "Ullapara", Longitude = 89.0462, Latitude = 24.1385 },
                new Station { Id = 9, Name = "Boral_Bridge", Longitude = 88.9609, Latitude = 24.3624 },
                new Station { Id = 10, Name = "Chatmohar", Longitude = 88.8101, Latitude = 24.3995 },
                new Station { Id = 11, Name = "Ishwardi Bypass", Longitude = 89.0462, Latitude = 24.1385 },
                new Station { Id = 12, Name = "Abdulpur", Longitude = 88.9609, Latitude = 24.3624 },
                new Station { Id = 13, Name = "Arani", Longitude = 88.8101, Latitude = 24.3995 },
                new Station { Id = 14, Name = "Rajshahi", Longitude = 88.6000, Latitude = 24.3745 }
            };

            await _dataImportService.AddStationsAsync(stations);

            var trains = new List<Train>
            {
                new Train { Id = 1, Name = "Dhumketu Express", TrainCode = 769 }, // d-r 6AM
                new Train { Id = 2, Name = "Dhumketu Express", TrainCode = 770 }, // r-d 11:20 PM
                new Train { Id = 3, Name = "SilkCity Express", TrainCode = 753 }, // d-r 2:40 PM
                new Train { Id = 4, Name = "SilkCity Express", TrainCode = 754 }, // r-d 7:40 AM
                new Train { Id = 5, Name = "Banalata Express", TrainCode = 791 }, // d-r 1:30 PM
                new Train { Id = 6, Name = "Banalata Express", TrainCode = 792 }, // r-d 7AM
                new Train { Id = 7, Name = "Padma Express", TrainCode = 759 }, // d-r 10:45 PM
                new Train { Id = 8, Name = "Padma Express", TrainCode = 760 }, // r-d 4:00 PM            
                new Train { Id = 9, Name = "Madhumati Express", TrainCode = 755 }, // d-r 3PM
                new Train { Id = 10, Name = "Madhumati Express", TrainCode = 756 }, // r-d 6:40 AM
            };

            await _dataImportService.AddTrainsAsync(trains);

            var journeys = new List<TrainJourney>()
            {
                new TrainJourney { Id = 1, TrainId = 1 },
                new TrainJourney { Id = 2, TrainId = 2 },
                new TrainJourney { Id = 3, TrainId = 3 },
                new TrainJourney { Id = 4, TrainId = 4 },
                new TrainJourney { Id = 5, TrainId = 5 },
                new TrainJourney { Id = 6, TrainId = 6 },
                new TrainJourney { Id = 7, TrainId = 7 },
                new TrainJourney { Id = 8, TrainId = 8 },
                new TrainJourney { Id = 9, TrainId = 9 },
                new TrainJourney { Id = 10, TrainId = 10 },
            };

            await _dataImportService.AddJourneysAsync(journeys);

            var journeyDetails = new List<TrainJourneyDetail>
            {
                new TrainJourneyDetail { Id = 1, TrainJourneyId = 1, StationId = 1, RouteStationOrder = 1, Distance = 0 }, //TODO: Add distance later
                new TrainJourneyDetail { Id = 2, TrainJourneyId = 1, StationId = 2, RouteStationOrder = 2, Distance = 0 },
                new TrainJourneyDetail { Id = 3, TrainJourneyId = 1, StationId = 3, RouteStationOrder = 3, Distance = 0 },
                new TrainJourneyDetail { Id = 4, TrainJourneyId = 1, StationId = 4, RouteStationOrder = 4, Distance = 0 },
                new TrainJourneyDetail { Id = 5, TrainJourneyId = 1, StationId = 5, RouteStationOrder = 5, Distance = 0 },
                new TrainJourneyDetail { Id = 6, TrainJourneyId = 1, StationId = 6, RouteStationOrder = 6, Distance = 0 },
                new TrainJourneyDetail { Id = 7, TrainJourneyId = 1, StationId = 7, RouteStationOrder = 7, Distance = 0 },
                new TrainJourneyDetail { Id = 8, TrainJourneyId = 1, StationId = 8, RouteStationOrder = 8, Distance = 0 },
                new TrainJourneyDetail { Id = 9, TrainJourneyId = 1, StationId = 9, RouteStationOrder = 9, Distance = 0 },
                new TrainJourneyDetail { Id = 10, TrainJourneyId = 1, StationId = 10, RouteStationOrder = 10, Distance = 0 },
                new TrainJourneyDetail { Id = 11, TrainJourneyId = 1, StationId = 11, RouteStationOrder = 11, Distance = 0 },
                new TrainJourneyDetail { Id = 12, TrainJourneyId = 1, StationId = 12, RouteStationOrder = 12, Distance = 0 },
                new TrainJourneyDetail { Id = 13, TrainJourneyId = 1, StationId = 13, RouteStationOrder = 13, Distance = 0 },
                new TrainJourneyDetail { Id = 14, TrainJourneyId = 1, StationId = 14, RouteStationOrder = 14, Distance = 0 },
            };

            await _dataImportService.AddJourneyDetailsAsync(journeyDetails);

            return Ok(new { Message = "Data initialized successfully" });
        }

        
    }
}
