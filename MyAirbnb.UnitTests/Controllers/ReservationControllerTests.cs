
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyAirbnb.API.Controllers;
using MyAirbnb.Core.Commands.Reservation;
using MyAirbnb.Models.DTOs.Reservation;
using Xunit;

namespace MyAirbnb.UnitTests.Controllers;

public class ReservationControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly ReservationController _controller;

    public ReservationControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new ReservationController(_mockSender.Object);
    }

    /// <summary>
    /// Unit Test for Process Reservation Endpoint returns OK result
    /// </summary>
    /// <returns>OK()</returns>
    [Fact]
    public async Task ProcessReservationAsync_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new AccomodationReservationRequestDTO {};
        ErrorOr<ReservationDTO> response = new ReservationDTO() {AccommodationId = 1};

        _mockSender.Setup(s => s.Send(It.IsAny<ProcessReservation.Command>(), default))
                   .ReturnsAsync(response);

        // Act
        var result = await _controller.ProcessReservationAsync(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ReservationDTO>(okResult.Value);
        Assert.Equal(1, returnValue.AccommodationId);
    }

    /// <summary>
    /// Unit Test for Process Reservation Endpoint returns Bad Request result
    /// </summary>
    /// <returns>BadRequest()</returns>
    [Fact]
    public async Task ProcessReservationAsync_InValidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new AccomodationReservationRequestDTO { };
        ErrorOr<ReservationDTO> response = Error.NotFound("UserNotFound", "User Not Found!");

        _mockSender.Setup(s => s.Send(It.IsAny<ProcessReservation.Command>(), default))
                   .ReturnsAsync(response);

        // Act
        var result = await _controller.ProcessReservationAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("User Not Found!", badRequestResult.Value);
    }
}
