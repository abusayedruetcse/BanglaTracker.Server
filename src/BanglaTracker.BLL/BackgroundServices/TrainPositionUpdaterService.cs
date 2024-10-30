using BanglaTracker.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BanglaTracker.BLL.BackgroundServices
{
    public class TrainPositionUpdaterService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<TrainPositionUpdaterService> _logger;
        private Timer? _timer;

        public TrainPositionUpdaterService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<TrainPositionUpdaterService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Train Position Updater Service starting.");
            _timer = new Timer(UpdateTrainPosition, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void UpdateTrainPosition(object state)
        {
            _logger.LogInformation("Updating train position...");
            
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var trainJourneyService = scope.ServiceProvider.GetRequiredService<ITrainJourneyService>();
                // Use trainJourneyService to perform your work here
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Train Position Updater Service stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }


}
