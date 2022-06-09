using Rtl.Shows.Repository;
using Rtl.Shows.WebApi.Services.Models;

namespace Rtl.Shows.WebApi.Services;

class ShowMapper : IShowMapper
{
    public PagedShowsRepresentation From(IList<Show> shows, int pageSize, int pageNumber)
    {
        var showRepresentations = shows.Select(s => new ShowRepresentation
        {
            Id = s.Id,
            Name = s.Name,
            Cast = s.ShowCasts.Select(sp => new CastRepresentation
            {
                Id = sp.Cast.Id,
                Name = sp.Cast.Name,
                Birthday = sp.Cast.Birthday?.ToShortDateString()
            }).ToList()
        }).ToList();

        return new PagedShowsRepresentation
        {
            Items = showRepresentations,
            PageSize = pageSize,
            PageNumber = pageNumber
        };
    }
}