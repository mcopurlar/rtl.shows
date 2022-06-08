using System.Threading.Channels;
using Polly;
using Polly.Retry;
using Rtl.Shows.Repository;
using Rtl.Shows.Scraper.Services.ServiceClients;
using Rtl.Shows.Scraper.Services.ServiceClients.Models;
using Show = Rtl.Shows.Scraper.Services.ServiceClients.Models.Show;

namespace Rtl.Shows.Scraper.Services;

class PagedShowsConsumer
{
    private readonly int _id;
    private readonly ChannelReader<IList<Show>> _channelReader;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITvMazeServiceClient _tvMazeServiceClient;
    private readonly IShowMapper _showMapper;

    public PagedShowsConsumer(int id, ChannelReader<IList<Show>> channelReader, IServiceProvider serviceProvider)
    {
        _id = id;
        _channelReader = channelReader;
        _serviceProvider = serviceProvider;
        _tvMazeServiceClient = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ITvMazeServiceClient>();
        _showMapper = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IShowMapper>();
    }

    public async Task ProcessPagedShows(CancellationToken cancellationToken)
    {
        Console.WriteLine($"Consumer {_id} started to listen");

        while (await _channelReader.WaitToReadAsync(cancellationToken))
        {
            if (_channelReader.TryRead(out var pagedShows))
            {
                Console.WriteLine($"Consumer <{_id}> picked <{pagedShows.Count}> shows");

                foreach (var show in pagedShows)
                {
                    var showsDbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ShowsDbContext>();

                    var casts = await _tvMazeServiceClient.GetCastsByShowId(show.Id, cancellationToken);
                    
                    if (casts == null)
                    {
                        Console.WriteLine($"FATAL : Failed to insert <{show.Id}>");
                        continue;
                    }
                    
                    var existingShow = await showsDbContext.Shows.FindAsync(new[] { (object)show.Id }, cancellationToken: cancellationToken);

                    if (existingShow == null)
                    {
                        var policy = GetRefreshDbContextPolicy(show, casts, cancellationToken);

                        await policy.ExecuteAsync(async () =>
                        {
                            await PersistShow(showsDbContext, show, casts, cancellationToken);
                        });
                    }
                }

                Console.WriteLine($"Consumer <{_id}> finished <{pagedShows.Count}> shows import");
            }
        }
    }

    private AsyncRetryPolicy GetRefreshDbContextPolicy(Show show, IList<Cast> casts, CancellationToken cancellationToken)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(10), async (_, _) =>
            {
                await using var refreshedShowsDbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ShowsDbContext>();
                await PersistShow(refreshedShowsDbContext, show, casts, cancellationToken);
            });
    }

    private async Task PersistShow(ShowsDbContext showsDbContext, Show show, IList<Cast> casts, CancellationToken cancellationToken)
    {
        var showToAdd = await _showMapper.ToEntity(showsDbContext, show, casts, cancellationToken);

        await showsDbContext.Shows.AddAsync(showToAdd, cancellationToken);
        await showsDbContext.SaveChangesAsync(cancellationToken);
    }
}