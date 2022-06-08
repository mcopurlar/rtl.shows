using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rtl.Shows.Repository;

public static class RepositoryExtensions
{
    public static IServiceCollection AddShowsDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ShowsDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"), builder =>
            {
                builder.MigrationsAssembly("Rtl.Shows.Scraper");
            });
        }, ServiceLifetime.Transient);

        return services;
    }
}