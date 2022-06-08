using Rtl.Shows.Repository;
using Rtl.Shows.Scraper.Services.ServiceClients.Models;

namespace Rtl.Shows.Scraper.Services;

public interface IShowMapper
{
    Task<Repository.Show> ToEntity(ShowsDbContext dbContext, ServiceClients.Models.Show show, IList<Cast> casts, CancellationToken cancellationToken);
}