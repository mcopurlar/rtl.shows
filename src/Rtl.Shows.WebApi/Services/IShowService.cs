using Rtl.Shows.WebApi.Services.Models;

namespace Rtl.Shows.WebApi.Services;

public interface IShowService
{
    Task<PagedShowsRepresentation> GetShows(int pageNumber, int pageSize);
}