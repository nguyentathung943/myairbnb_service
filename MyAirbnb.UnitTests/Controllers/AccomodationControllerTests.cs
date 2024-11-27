using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyAirbnb.API.Controllers;
using MyAirbnb.Core.Commands.Accomodation;
using MyAirbnb.Models.DTOs.Accomodation;
using Xunit;

namespace MyAirbnb.UnitTests.Controllers;
public class AccomodationControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly AccomodationController _controller;

    public AccomodationControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new AccomodationController(_mockSender.Object);
    }

    /// <summary>
    /// Unit Test for Return Valid result from the filter
    /// </summary>
    /// <returns>OK()</returns>
    [Fact]
    public async Task GetListAccomodationsAsync_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var filter = new AccomodationGetListRequestDTO
        {
            SearchString = "beAch",
            CheckInDate = new DateTime(2023, 12, 1),
            CheckOutDate = new DateTime(2024, 12, 7),
            Adults = 2,
            Children = 1,
            Infants = 0,
            Pets = 1
        };

        ErrorOr<AccomodationGetListResponseDTO> response = new AccomodationGetListResponseDTO
        {
            Accomodations = new List<AccomodationDTO>
            {
                new AccomodationDTO
                {
                    Id = 1,
                    Name = "Beach House",
                    LocationTagName = "Beach",
                    Description = "A beautiful beach house with stunning ocean views.",
                    PricePerNight = 200.00m,
                    MaxAdultsAllowed = 4,
                    MaxChildrenAllowed = 2,
                    MaxInfantsAllowed = 1,
                    MaxPetsAllowed = 1
                }
            },
            TotalCount = 1
        };

        _mockSender.Setup(s => s.Send(It.IsAny<GetListAccomodation.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetListAccomodationsAsync(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<AccomodationGetListResponseDTO>(okResult.Value);
        Assert.Equal(1, returnValue.TotalCount);
    }

    /// <summary>
    /// Unit Test for Return Bad Request due to Invalid Request Model
    /// </summary>
    /// <returns>BadRequest()</returns>
    [Fact]
    public async Task GetListAccomodationsAsync_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var filter = new AccomodationGetListRequestDTO
        {
            SearchString = "Beach",
            CheckInDate = null,
            CheckOutDate = null,
            Adults = 0,
            Children = 1,
            Infants = 0,
            Pets = 1
        };

        ErrorOr<AccomodationGetListResponseDTO> response = Error.Conflict("Adults", "Invalid Number!");

        _mockSender.Setup(s => s.Send(It.IsAny<GetListAccomodation.Command>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetListAccomodationsAsync(filter);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid Number!", badRequestResult.Value);
    }
}
