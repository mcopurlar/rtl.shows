using Rtl.Shows.Repository;
using Rtl.Shows.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var serviceCollection = builder.Services;

serviceCollection.AddControllers();
serviceCollection.AddShowsDbContext(configuration);
serviceCollection.AddScoped<IShowMapper, ShowMapper>();
serviceCollection.AddScoped<IShowService, ShowService>();

var app = builder.Build();

app.MapControllers();

app.Run();

