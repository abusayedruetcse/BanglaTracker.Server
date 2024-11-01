using BanglaTracker.BLL.Interfaces;
using BanglaTracker.Core.Enums;
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
                TimeSpan.FromMinutes(5) // TODO: update configure value of timer.
            );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Train Position Updater Service stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async Task UpdateTrainPositionAsync()
        {
            _logger.LogInformation("Updating train position...");

            int maxConcurrency = 3; // TODO: Move this to config file.

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var trainJourneyService = scope.ServiceProvider.GetRequiredService<ITrainJourneyService>();

                // Retrieve all in progress journey's
                var journeyIds = await trainJourneyService.GetJourneyIdsByStatusAsync(JourneyStatus.InProgress);

                var tasks = new List<Task>();
                using (var semaphore = new SemaphoreSlim(maxConcurrency))
                {
                    foreach (var journeyId in journeyIds)
                    {
                        await semaphore.WaitAsync();

                        // Create a task for processing each journey
                        var task = Task.Run(async () =>
                        {
                            try
                            {
                                await trainJourneyService.CalculateMetricsAsync(journeyId);
                                _logger.LogInformation("Train position updated successfully.");
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Error updating train position.");
                            }
                            finally
                            {
                                // Release semaphore once processing is complete
                                semaphore.Release();
                            }
                        });

                        tasks.Add(task);
                    }

                    // Wait for all tasks to complete
                    await Task.WhenAll(tasks);
                }               
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
