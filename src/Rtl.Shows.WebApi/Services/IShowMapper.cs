using Rtl.Shows.Repository;
using Rtl.Shows.WebApi.Services.Models;

namespace Rtl.Shows.WebApi.Services;

public interface IShowMapper
{
    PagedShowsRepresentation From(IList<Show> shows, int pageSize, int pageNumber);
}