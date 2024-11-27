using MyAirbnb.Core.Commands.Accomodation;
using MyAirbnb.DataAccess;
using MyAirbnb.Models.DTOs.Accomodation;
using Xunit;

namespace MyAirbnb.UnitTests.Commands.Accomodation;

public class GetListAccomodationTests : IDisposable
{
    private readonly GetListAccomodation.Handler _handler;
    private readonly ApplicationDbContext _context;

    public GetListAccomodationTests()
    {
        _context = ApplicationDbContextHelper.Create();
        _handler = new GetListAccomodation.Handler(_context);
    }

    /// <summary>
    /// Unit Test for Valid Filter Request
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_ValidFilter_ReturnsListOfAccomodations()
    {
        // Arrange
        ApplicationDbContextHelper.SeedAccomodation(_context, 1);
        var filter = new AccomodationGetListRequestDTO
        {
            SearchString = null,
            CheckInDate = new DateTime(2024, 12, 1),
            CheckOutDate = new DateTime(2024, 12, 7),
            Adults = 2,
            Children = 1,
            Infants = 1,
            Pets = 1
        };

        var command = new GetListAccomodation.Command(filter);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.IsType<AccomodationGetListResponseDTO>(result.Value);
    }

    /// <summary>
    /// Unit Test for Invalid Filter Request
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_InvalidFilter_ReturnsError()
    {
        // Arrange
        var filter = new AccomodationGetListRequestDTO
        {
            SearchString = "Beach",
            CheckInDate = new DateTime(2024, 12, 1),
            CheckOutDate = new DateTime(2024, 12, 7),
            Adults = 0,
            Children = 1,
            Infants = 0,
            Pets = 1
        };

        var command = new GetListAccomodation.Command(filter);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(GetListAccomodation.ErrorCodes.InvalidDataModel, result.FirstError.Code);
    }

    public void Dispose()
    {
        ApplicationDbContextHelper.Destroy(_context);
    }
}
