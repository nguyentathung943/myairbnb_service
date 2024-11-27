using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using MyAirbnb.API.Controllers;
using MyAirbnb.Models.DTOs.Authentication;
using MediatR;
using ErrorOr;
using MyAirbnb.Core.Commands.Authentication;

namespace MyAirbnb.UnitTests.Controllers;

public class AuthenticateControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly AuthenticateController _controller;

    public AuthenticateControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new AuthenticateController(_mockSender.Object);
    }

    /// <summary>
    /// Unit Test for Authentication Endpoint returns OK result
    /// </summary>
    /// <returns>OK()</returns>
    [Fact]
    public async Task AuthenticateUserAsync_ValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var credentials = new AuthenticationRequestDTO { UserName = "user", Password = "password" };
        ErrorOr<AuthenticationResponseDTO> response = new AuthenticationResponseDTO() { AccessToken = "this_is_valid_token" };

        _mockSender.Setup(s => s.Send(It.IsAny<AuthenticateUser.Command>(), default))
                   .ReturnsAsync(response);

        // Act
        var result = await _controller.AuthenticateUserAsync(credentials);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<AuthenticationResponseDTO>(okResult.Value);
        Assert.Equal("this_is_valid_token", returnValue.AccessToken);
    }

    /// <summary>
    /// Unit Test for Authentication Endpoint returns Bad Request result
    /// </summary>
    /// <returns>BadRequest()</returns>
    [Fact]
    public async Task AuthenticateUserAsync_InvalidCredentials_ReturnsBadRequest()
    {
        // Arrange
        var credentials = new AuthenticationRequestDTO { UserName = "testuser", Password = "wrongpassword" };
        ErrorOr<AuthenticationResponseDTO> response = Error.NotFound("InvalidCredentials", "Invalid Credentials!");

        _mockSender.Setup(s => s.Send(It.IsAny<AuthenticateUser.Command>(), default))
                   .ReturnsAsync(response);

        // Act
        var result = await _controller.AuthenticateUserAsync(credentials);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid Credentials!", badRequestResult.Value);
    }
}
