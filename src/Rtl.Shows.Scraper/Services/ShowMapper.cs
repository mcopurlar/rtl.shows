using Microsoft.EntityFrameworkCore;
using Rtl.Shows.Repository;
using Cast = Rtl.Shows.Scraper.Services.ServiceClients.Models.Cast;

namespace Rtl.Shows.Scraper.Services;

class ShowMapper : IShowMapper
{
    public async Task<Repository.Show> ToEntity(ShowsDbContext dbContext, ServiceClients.Models.Show show, IList<Cast> casts, CancellationToken cancellationToken)
    {
        var normalizedCastList = casts.DistinctBy(x => x.Person.Id);

        var existingCasts = await dbContext.Casts.Where(x => normalizedCastList.Select(c => c.Person.Id).Contains(x.Id)).ToListAsync(cancellationToken);

        return new Repository.Show
        {
            Id = show.Id,
            Name = show.Name,
            ShowCasts = normalizedCastList.Select(c =>
            {
                var existingCast = existingCasts.FirstOrDefault(x => x.Id == c.Person.Id);

                var cast = existingCast ?? new Repository.Cast
                {
                    Id = c.Person.Id,
                    Birthday = c.Person.Birthday,
                    Name = c.Person.Name
                };

                return new ShowCast
                {
                    ShowId = show.Id,
                    Cast = cast
                };
            }).ToList()
        };
    }
}
