using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAirbnb.Core.Commands.Accomodation;
using MyAirbnb.Models.DTOs.Accomodation;

namespace MyAirbnb.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class AccomodationController : ControllerBase
{
    private readonly ISender _sender;

    public AccomodationController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get List of Accomodations by Filter
    /// </summary>
    /// <param name="filter">Filter Conditions</param>
    /// <param name="offset">Pagination / Infinite scrolling</param>
    /// <param name="limit">Pagination / Infinite scrolling</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> GetListAccomodationsAsync(
        [FromBody] AccomodationGetListRequestDTO filter,
        [FromQuery] int? offset = 0,
        [FromQuery] int? limit = 10
    )
    {
        var getListResponse = await _sender.Send(new GetListAccomodation.Command(filter, offset, limit));

        if (getListResponse is { IsError: true, FirstError: { Description: { } errorMessage } })
        {
            return BadRequest(errorMessage);
        }

        return Ok(getListResponse.Value);
    }
}
