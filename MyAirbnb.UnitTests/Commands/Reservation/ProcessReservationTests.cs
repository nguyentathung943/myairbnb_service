using MyAirbnb.Core.Commands.Reservation;
using MyAirbnb.DataAccess;
using MyAirbnb.Models.DTOs.Reservation;
using Xunit;

namespace MyAirbnb.UnitTests.Commands.Reservation;

public class ProcessReservationTests
{
    private readonly ProcessReservation.Handler _handler;
    private readonly ApplicationDbContext _context;

    public ProcessReservationTests()
    {
        _context = ApplicationDbContextHelper.Create();
        _handler = new ProcessReservation.Handler(_context);
    }

    /// <summary>
    /// Unit Test for Invalid Data Model
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ProcessReservation_InvalidDataModel_ReturnsError()
    {
        // Arrange
        ApplicationDbContextHelper.SeedReservation(_context,1,1,1);

        var reservationRequest = new AccomodationReservationRequestDTO
        {
            AccommodationId = 1,
            UserId = 1,
            CheckInDate = new DateTime(2024, 11, 20),
            CheckOutDate = new DateTime(2024, 11, 21),
            TotalPrice = 200,
            TotalAdults = 100,
            TotalChildren = 100,
            TotalInfants = 100,
            TotalPets = 100
        };

        var command = new ProcessReservation.Command(reservationRequest);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ProcessReservation.ErrorCodes.InvalidDataModel, result.FirstError.Code);
    }

    /// <summary>
    /// Unit Test for Accomodation Not Found
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ProcessReservation_AccomodationNotFound_ReturnsError()
    {
        // Arrange
        ApplicationDbContextHelper.SeedReservation(_context, 2, 2, 2);

        var reservationRequest = new AccomodationReservationRequestDTO
        {
            AccommodationId = 0,
            UserId = 2,
            CheckInDate = new DateTime(2024, 11, 27),
            CheckOutDate = new DateTime(2024, 11, 28),
            TotalPrice = 200,
            TotalAdults = 2,
            TotalChildren = 1,
            TotalInfants = 1,
            TotalPets = 1
        };

        var command = new ProcessReservation.Command(reservationRequest);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ProcessReservation.ErrorCodes.AccommodationNotFound, result.FirstError.Code);
    }

    /// <summary>
    /// Unit Test for User Not Found
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ProcessReservation_UserNotFound_ReturnsError()
    {
        // Arrange
        ApplicationDbContextHelper.SeedReservation(_context, 3, 3, 3);

        var reservationRequest = new AccomodationReservationRequestDTO
        {
            AccommodationId = 3,
            UserId = 0,
            CheckInDate = new DateTime(2024, 11, 27),
            CheckOutDate = new DateTime(2024, 11, 28),
            TotalPrice = 200,
            TotalAdults = 2,
            TotalChildren = 1,
            TotalInfants = 1,
            TotalPets = 1
        };

        var command = new ProcessReservation.Command(reservationRequest);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ProcessReservation.ErrorCodes.UserNotFound, result.FirstError.Code);
    }

    /// <summary>
    /// Unit Test for Invalid Date Range
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ProcessReservation_InvalidDateRange_ReturnsError()
    {
        // Arrange
        ApplicationDbContextHelper.SeedReservation(_context, 4, 4, 4);

        var reservationRequest = new AccomodationReservationRequestDTO
        {
            AccommodationId = 4,
            UserId = 4,
            CheckInDate = new DateTime(2024, 11, 29),
            CheckOutDate = new DateTime(2024, 11, 27),
            TotalPrice = 200,
            TotalAdults = 2,
            TotalChildren = 1,
            TotalInfants = 1,
            TotalPets = 1
        };

        var command = new ProcessReservation.Command(reservationRequest);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ProcessReservation.ErrorCodes.InvalidDateRange, result.FirstError.Code);
    }

    /// <summary>
    /// Unit Test for Overlapped Reservation Date
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ProcessReservation_OverlappedDateRange_ReturnsError()
    {
        // Arrange
        ApplicationDbContextHelper.SeedReservation(_context, 5, 5, 5);

        var reservationRequest = new AccomodationReservationRequestDTO
        {
            AccommodationId = 5,
            UserId = 5,
            CheckInDate = new DateTime(2024, 11, 29),
            CheckOutDate = new DateTime(2024, 11, 30),
            TotalPrice = 200,
            TotalAdults = 2,
            TotalChildren = 1,
            TotalInfants = 1,
            TotalPets = 1
        };

        var command = new ProcessReservation.Command(reservationRequest);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ProcessReservation.ErrorCodes.OverlappingReservation, result.FirstError.Code);
    }


    /// <summary>
    /// Unit Test for Capacity Exceed
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ProcessReservation_CapacityExceed_ReturnsError()
    {
        ApplicationDbContextHelper.SeedReservation(_context, 6, 6, 6);

        // Arrange
        var reservationRequest = new AccomodationReservationRequestDTO
        {
            AccommodationId = 6,
            UserId = 6,
            CheckInDate = new DateTime(2024, 11, 20),
            CheckOutDate = new DateTime(2024, 11, 21),
            TotalPrice = 200,
            TotalAdults = 9,
            TotalChildren = 1,
            TotalInfants = 1,
            TotalPets = 1
        };

        var command = new ProcessReservation.Command(reservationRequest);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(ProcessReservation.ErrorCodes.CapacityExceed, result.FirstError.Code);
    }

    public void Dispose()
    {
        ApplicationDbContextHelper.Destroy(_context);
    }
}
