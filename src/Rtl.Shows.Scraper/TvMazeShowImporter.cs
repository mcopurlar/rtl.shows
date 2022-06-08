using System.Diagnostics;
using Rtl.Shows.Scraper.Services;

namespace Rtl.Shows.Scraper;

class TvMazeShowImporter : BackgroundService
{
    private readonly ILogger<TvMazeShowImporter> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TvMazeShowImporter(ILogger<TvMazeShowImporter> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopWatch = new Stopwatch();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("TvMazeShowImporter started at: {time}", DateTimeOffset.Now);

            using (var scope = _serviceProvider.CreateScope())
            {
                var importer = scope.ServiceProvider.GetRequiredService<IImportShowService>();

                stopWatch.Start();

                await importer.ImportShows(stoppingToken);

                stopWatch.Stop();
                var elapsed = stopWatch.Elapsed;

                Console.WriteLine($"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}.{elapsed.Milliseconds / 10:00}");
            }

            _logger.LogInformation("TvMazeShowImporter completed at: {time}", DateTimeOffset.Now);

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}