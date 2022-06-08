using Microsoft.AspNetCore.Mvc;
using Rtl.Shows.WebApi.Services;

namespace Rtl.Shows.WebApi.Controllers;

[Route("shows")]
public class ShowController : Controller
{
    private readonly IShowService _showService;

    public ShowController(IShowService showService)
    {
        _showService = showService;
    }

    [HttpGet]
    public async Task<IActionResult> GetShows([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var pagedShowsRepresentation = await _showService.GetShows(pageNumber, pageSize);
        return Ok(pagedShowsRepresentation);
    }
}