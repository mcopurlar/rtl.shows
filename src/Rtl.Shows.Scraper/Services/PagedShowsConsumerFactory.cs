using System.Threading.Channels;
using Rtl.Shows.Scraper.Services.ServiceClients.Models;

namespace Rtl.Shows.Scraper.Services;

class PagedShowsConsumerFactory : IPagedShowsConsumerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PagedShowsConsumerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public PagedShowsConsumer Create(int id, ChannelReader<IList<Show>> channelReader)
    {
        return new PagedShowsConsumer(id, channelReader, _serviceProvider);
    }
}