using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using Rtl.Shows.Repository;
using Rtl.Shows.Scraper.Services.ServiceClients;
using Show = Rtl.Shows.Scraper.Services.ServiceClients.Models.Show;

namespace Rtl.Shows.Scraper.Services;

class ImportShowService : IImportShowService
{
    private const int TvMazePagedShowsItemCount = 250;
    private const int NumberOfConsumers = 50;

    private readonly ITvMazeServiceClient _tvMazeServiceClient;
    private readonly IPagedShowsConsumerFactory _pagedShowsConsumerFactory;
    private readonly ShowsDbContext _dbContext;
    private readonly Channel<IList<Show>> _channel;
    private readonly PagedShowsProducer _pagedShowsProducer;

    public ImportShowService(ITvMazeServiceClient tvMazeServiceClient, IPagedShowsConsumerFactory pagedShowsConsumerFactory, ShowsDbContext dbContext)
    {
        _tvMazeServiceClient = tvMazeServiceClient;
        _pagedShowsConsumerFactory = pagedShowsConsumerFactory;
        _dbContext = dbContext;

        _channel = Channel.CreateUnbounded<IList<Show>>(new UnboundedChannelOptions
        {
            SingleWriter = true,
            SingleReader = false
        });

        _pagedShowsProducer = new PagedShowsProducer(_channel.Writer);
    }
    public async Task ImportShows(CancellationToken stoppingToken)
    {
        var lastInsertedShow = await _dbContext.Shows.OrderByDescending(x => x.LastUpdatedAt).FirstOrDefaultAsync(stoppingToken);

        var pageIndex = 0;

        if (lastInsertedShow != null)
        {
            pageIndex = lastInsertedShow.Id / TvMazePagedShowsItemCount;
        }
        
        var consumerTasks = new List<Task>();

        for (int i = 0; i < NumberOfConsumers; i++)
        {
            consumerTasks.Add(_pagedShowsConsumerFactory.Create(i, _channel.Reader).ProcessPagedShows(stoppingToken));
        }

        Task producerTask = null;

        await foreach (var pagedShows in _tvMazeServiceClient.GetShows(pageIndex, stoppingToken).WithCancellation(stoppingToken))
        {
            producerTask = _pagedShowsProducer.BeginProducing(pagedShows, stoppingToken);
        }

        await producerTask.ContinueWith(_ => _channel.Writer.Complete(), stoppingToken);

        await Task.WhenAll(consumerTasks);
    }
}
