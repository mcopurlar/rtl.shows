using Polly;
using Rtl.Shows.Repository;
using Rtl.Shows.Scraper;
using Rtl.Shows.Scraper.Services;
using Rtl.Shows.Scraper.Services.ServiceClients;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        var config = configuration.GetSection("Logging");

        builder.AddConsole();
        builder.AddDebug();
        builder.AddConfiguration(config);
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<TvMazeShowImporter>();
        services.AddHttpClient<ITvMazeServiceClient, TvMazeServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["TvMazeApi:BaseUrl"]);
            })
            .AddPolicyHandler(TvMazeRetryPolicy.GetRetryPolicy())
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(60)));
        
        services.AddScoped<IImportShowService, ImportShowService>();
        services.AddScoped<IShowMapper, ShowMapper>();
        services.AddScoped<IPagedShowsConsumerFactory, PagedShowsConsumerFactory>();

        services.AddShowsDbContext(configuration);
    })
    .Build();

await host.RunAsync();
