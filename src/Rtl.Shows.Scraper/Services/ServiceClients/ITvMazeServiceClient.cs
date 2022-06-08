using Rtl.Shows.Scraper.Services.ServiceClients.Models;

namespace Rtl.Shows.Scraper.Services.ServiceClients;

public interface ITvMazeServiceClient
{
    IAsyncEnumerable<IList<Show>> GetShows(int pageIndex, CancellationToken cancellationToken);
    Task<IList<Cast>?> GetCastsByShowId(int showId, CancellationToken cancellationToken);
}