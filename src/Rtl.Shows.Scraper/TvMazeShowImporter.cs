using System.Diagnostics;
using Rtl.Shows.Scraper.Services;

namespace Rtl.Shows.Scraper;

class TvMazeShowImporter : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public TvMazeShowImporter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stopWatch = new Stopwatch();

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var importer = scope.ServiceProvider.GetRequiredService<IImportShowService>();

                stopWatch.Start();

                await importer.ImportShows(stoppingToken);

                importer = scope.ServiceProvider.GetRequiredService<IImportShowService>();
                await importer.ImportShows(stoppingToken, 0);

                stopWatch.Stop();
                var elapsed = stopWatch.Elapsed;
                Console.WriteLine($"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}.{elapsed.Milliseconds / 10:00}");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}