using BanglaTracker.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BanglaTracker.BLL.BackgroundServices
{
    public class TrainPositionUpdaterService : IHostedService, IDisposable
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
            _timer = new Timer(
                async _ => await UpdateTrainPositionAsync(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1) // TODO: update configure value of timer.
            );
            return Task.CompletedTask;
        }

        private async Task UpdateTrainPositionAsync()
        {
            _logger.LogInformation("Updating train position...");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var trainJourneyService = scope.ServiceProvider.GetRequiredService<ITrainJourneyService>();

                try
                {
                    await trainJourneyService.CalculateMetricsAsync(1);
                    _logger.LogInformation("Train position updated successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating train position.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Train Position Updater Service stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
