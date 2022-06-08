using System.Threading.Channels;
using Rtl.Shows.Scraper.Services.ServiceClients.Models;

namespace Rtl.Shows.Scraper.Services;

interface IPagedShowsConsumerFactory
{
    PagedShowsConsumer Create(int id, ChannelReader<IList<Show>> channelReader);
}