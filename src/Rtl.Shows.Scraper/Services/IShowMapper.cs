using Rtl.Shows.Repository;
using Rtl.Shows.Scraper.Services.ServiceClients.Models;
using Cast = Rtl.Shows.Scraper.Services.ServiceClients.Models.Cast;

namespace Rtl.Shows.Scraper.Services;

public interface IShowMapper
{
    Task<Repository.Show> ToEntity(ShowsDbContext dbContext, ServiceClients.Models.Show show, IList<Cast> casts, CancellationToken cancellationToken);
}