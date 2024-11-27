using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAirbnb.Core.Commands.Authentication;
using MyAirbnb.Models.DTOs.Authentication;

namespace MyAirbnb.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly ISender _sender;

    public AuthenticateController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Authenticate User
    /// </summary>
    /// <param name="credentials">The user's login credentials.</param>
    /// <returns>Authentication Result with Access Token</returns>
    /// <response code="200">Returns the authentication result with access token</response>
    /// <response code="400">If the credentials are invalid</response>
    [HttpPost]
    public async Task<IActionResult> AuthenticateUserAsync([FromBody] AuthenticationRequestDTO credentials)
    {
        var userAuthenticationResponse = await _sender.Send(new AuthenticateUser.Command(credentials));

        if (userAuthenticationResponse is { IsError: true, FirstError: { Description: { } errorMessage } })
        {
            return BadRequest(errorMessage);
        }

        return Ok(userAuthenticationResponse.Value);
    }
}
