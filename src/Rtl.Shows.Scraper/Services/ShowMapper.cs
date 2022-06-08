using Microsoft.EntityFrameworkCore;
using Rtl.Shows.Repository;
using Rtl.Shows.Scraper.Services.ServiceClients.Models;

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
            ShowPersons = normalizedCastList.Select(cast =>
            {
                var existingPerson = existingCasts.FirstOrDefault(x => x.Id == cast.Person.Id);

                var person = existingPerson ?? new Repository.Person
                {
                    Id = cast.Person.Id,
                    Birthday = cast.Person.Birthday,
                    Name = cast.Person.Name
                };

                return new ShowPerson
                {
                    ShowId = show.Id,
                    Person = person
                };
            }).ToList()
        };
    }
}
