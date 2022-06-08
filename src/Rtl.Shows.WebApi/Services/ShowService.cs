using Microsoft.EntityFrameworkCore;
using Rtl.Shows.Repository;
using Rtl.Shows.WebApi.Services.Models;

namespace Rtl.Shows.WebApi.Services;

class ShowService : IShowService
{
    private const int DefaultPageSize = 50;
    private const int MaxPageSize = 250;

    private readonly ShowsDbContext _showsDbContext;
    private readonly IShowMapper _showMapper;

    public ShowService(ShowsDbContext showsDbContext, IShowMapper showMapper)
    {
        _showsDbContext = showsDbContext;
        _showMapper = showMapper;
    }

    public async Task<PagedShowsRepresentation> GetShows(int pageNumber, int pageSize)
    {
        var mappedPageSize = pageSize == 0 ? DefaultPageSize : pageSize;

        if (pageSize > MaxPageSize)
        {
            mappedPageSize = MaxPageSize;
        }

        var shows = await _showsDbContext.Shows
            .Include(s => s.ShowPersons)
            .ThenInclude(s => s.Person)
            .Include(s => s.ShowPersons.OrderByDescending(sp => sp.Person.Birthday))
            .Skip(pageNumber * mappedPageSize)
            .Take(mappedPageSize)
            .ToListAsync();

        return _showMapper.From(shows, mappedPageSize, pageNumber);
    }
}