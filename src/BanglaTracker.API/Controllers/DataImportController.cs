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
                new Station { Id = 2, Name = "Airport Railway Station", Longitude = 90.4086, Latitude = 23.8431 },
                new Station { Id = 3, Name = "Joydebpur", Longitude = 90.4316, Latitude = 23.9983 },
                new Station { Id = 4, Name = "Tangail", Longitude = 89.9261, Latitude = 24.2499 },
                new Station { Id = 5, Name = "B.B. East", Longitude = 89.7570, Latitude = 24.4823 },
                new Station { Id = 6, Name = "Ishwardi Bypass", Longitude = 89.0462, Latitude = 24.1385 },
                new Station { Id = 7, Name = "Abdulpur", Longitude = 88.9609, Latitude = 24.3624 },
                new Station { Id = 8, Name = "Natore", Longitude = 88.9414, Latitude = 24.4081 },
                new Station { Id = 9, Name = "Arani", Longitude = 88.8101, Latitude = 24.3995 },
                new Station { Id = 10, Name = "Rajshahi", Longitude = 88.6000, Latitude = 24.3745 }
            };

            await _dataImportService.AddStationsAsync(stations);

            var trains = new List<Train>
            {
                new Train { Id = 1, Name = "Silk City Express" },
                new Train { Id = 2, Name = "Padma Express" },
                new Train { Id = 3, Name = "Dhumketu Express" },
                new Train { Id = 4, Name = "Banalata Express" },
                new Train { Id = 5, Name = "Titumir Express" },
                new Train { Id = 6, Name = "Chitra Express" },
                new Train { Id = 7, Name = "Nilsagar Express" },
                new Train { Id = 8, Name = "Rajshahi Express" },
                new Train { Id = 9, Name = "Sundarban Express" }
            };

            await _dataImportService.AddTrainsAsync(trains);

            return Ok(new { Message = "Data initialized successfully" });
        }

        
    }
}
