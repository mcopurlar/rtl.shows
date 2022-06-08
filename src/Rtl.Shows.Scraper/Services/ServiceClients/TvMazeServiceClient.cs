using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Rtl.Shows.Scraper.Services.ServiceClients.Models;

namespace Rtl.Shows.Scraper.Services.ServiceClients;

class TvMazeServiceClient : ITvMazeServiceClient
{
    private readonly HttpClient _httpClient;

    public TvMazeServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async IAsyncEnumerable<IList<Show>> GetShows(int pageIndex, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (true)
        {
            var httpResponseMessage = await _httpClient.GetAsync($"shows?page={pageIndex}", cancellationToken);

            if ((int)httpResponseMessage.StatusCode == 404)
            {
                break;
            }

            pageIndex += 1;

            var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var items = JsonConvert.DeserializeObject<IList<Show>>(content);

            Console.WriteLine($"Received <{items?.Count}> <{nameof(Show)}s> from page <{pageIndex}> {DateTime.Now.ToString("hh.mm.ss.ffffff")}");

            yield return items ?? new List<Show>();
        }
    }

    public async Task<IList<Cast>?> GetCastsByShowId(int showId, CancellationToken cancellationToken = default)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"shows/{showId}/cast", cancellationToken);

        var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return null;
        }
        
        var items = JsonConvert.DeserializeObject<IList<Cast>>(content);

        Console.WriteLine($"Received <{items?.Count}> <{nameof(Cast)}s> for show <{showId}>");

        return items;
    }
}