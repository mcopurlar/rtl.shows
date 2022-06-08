namespace Rtl.Shows.Scraper.Services;

public interface IImportShowService
{
    Task ImportShows(CancellationToken stoppingToken);
}