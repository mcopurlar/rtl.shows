namespace Rtl.Shows.WebApi.Services.Models;

public class PagedShowsRepresentation
{
    public IList<ShowRepresentation> Items { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}