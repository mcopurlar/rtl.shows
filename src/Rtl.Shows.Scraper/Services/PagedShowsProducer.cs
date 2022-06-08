using System.Threading.Channels;
using Rtl.Shows.Scraper.Services.ServiceClients.Models;

namespace Rtl.Shows.Scraper.Services;

class PagedShowsProducer
{
    private readonly ChannelWriter<IList<Show>> _channelWriter;

    public PagedShowsProducer(ChannelWriter<IList<Show>> channelWriter)
    {
        _channelWriter = channelWriter;
    }

    public async Task BeginProducing(IList<Show> shows, CancellationToken cancellationToken)
    {
        await _channelWriter.WriteAsync(shows, cancellationToken);
    }
}