using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAirbnb.Core.Commands.Reservation;
using MyAirbnb.Models.DTOs.Reservation;

namespace MyAirbnb.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ReservationController : ControllerBase
{
    private readonly ISender _sender;

    public ReservationController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Process Reservation
    /// </summary>
    /// <param name="reservationRequest">Reservation Request</param>
    /// <returns>Reservation Information</returns>
    /// <response code="200">Returns the Reservation Information</response>
    /// <response code="400">If the Reservation Request is invalid</response>
    /// 
    [HttpPost]
    public async Task<IActionResult> ProcessReservationAsync([FromBody] AccomodationReservationRequestDTO reservationRequest)
    {
        var processReservationResponse = await _sender.Send(new ProcessReservation.Command(reservationRequest));

        if (processReservationResponse is { IsError: true, FirstError: { Description: { } errorMessage } })
        {
            return BadRequest(errorMessage);
        }

        return Ok(processReservationResponse.Value);
    }
}
