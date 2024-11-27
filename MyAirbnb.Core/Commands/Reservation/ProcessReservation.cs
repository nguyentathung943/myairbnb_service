using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyAirbnb.DataAccess;
using MyAirbnb.Models.DTOs.Reservation;
using static MyAirbnb.Models.ObjectValidationHelper;
using Mapster;

namespace MyAirbnb.Core.Commands.Reservation;

public class ProcessReservation
{
    public record Command(AccomodationReservationRequestDTO ReservationRequest) : IRequest<ErrorOr<ReservationDTO>> { }

    public sealed class Handler : IRequestHandler<Command, ErrorOr<ReservationDTO>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<ReservationDTO>> Handle(Command request, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Validate Request DTO
                var errorMessage = ValidateAndGetFirstErrorMessage(request.ReservationRequest);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return Errors.InvalidDataModel(errorMessage);
                }

                // Validate Accomodation
                var accommodation = await _context.Accommodations
                    .Include(a => a.Reservations)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == request.ReservationRequest.AccommodationId);

                if (accommodation is null)
                {
                    return Errors.AccommodationNotFound;
                }

                // Validate User
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == request.ReservationRequest.UserId);

                if (user is null)
                {
                    return Errors.UserNotFound;
                }

                // Validate Overlapped Date Range
                var checkInDate = request.ReservationRequest.CheckInDate;
                var checkOutDate = request.ReservationRequest.CheckOutDate;

                if (checkInDate >= checkOutDate)
                {
                    return Errors.InvalidDateRange;
                }

                if (accommodation is { Reservations : { Count: > 0 } })
                {
                    var isOverlappedReservation = accommodation.Reservations.Any(r => r.CheckOutDate >= checkInDate && r.CheckInDate <= checkOutDate);

                    if (isOverlappedReservation)
                    {
                        return Errors.OverlappingReservation;
                    }
                }

                // Validate Accomodation Capacity
                var isExceedCapacity =
                    accommodation.MaxAdultsAllowed < request.ReservationRequest.TotalAdults
                    || accommodation.MaxChildrenAllowed < request.ReservationRequest.TotalChildren
                    || accommodation.MaxInfantsAllowed < request.ReservationRequest.TotalInfants
                    || accommodation.MaxPetsAllowed < request.ReservationRequest.TotalPets;

                if (isExceedCapacity)
                {
                    return Errors.CapacityExceed;
                }

                var reservation = new DataAccess.Entities.Reservation
                {
                    AccommodationId = request.ReservationRequest.AccommodationId,
                    UserId = request.ReservationRequest.UserId,
                    CheckInDate = request.ReservationRequest.CheckInDate,
                    CheckOutDate = request.ReservationRequest.CheckOutDate,
                    TotalPrice = request.ReservationRequest.TotalPrice,
                    TotalAdults = request.ReservationRequest.TotalAdults,
                    TotalChildren = request.ReservationRequest.TotalChildren,
                    TotalInfants = request.ReservationRequest.TotalInfants,
                    TotalPets = request.ReservationRequest.TotalPets
                };

                _context.Reservations.Add(reservation);

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return reservation.Adapt<ReservationDTO>();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Error.Failure(description: ex.Message);
            }
        }
    }

    public class Errors
    {
        public static Error InvalidDataModel(string errorMessage) => Error.Conflict(ErrorCodes.InvalidDataModel, errorMessage);
        public static readonly Error AccommodationNotFound = Error.NotFound(ErrorCodes.AccommodationNotFound, "Accommodation Not Found!");
        public static readonly Error UserNotFound = Error.NotFound(ErrorCodes.UserNotFound, "User Not Found!");
        public static readonly Error OverlappingReservation = Error.Conflict(ErrorCodes.OverlappingReservation, "Reservation dates overlap with an existing reservation!");
        public static readonly Error CapacityExceed = Error.Conflict(ErrorCodes.CapacityExceed, "Capacity Exceeded!");
        public static readonly Error InvalidDateRange = Error.Conflict(ErrorCodes.InvalidDateRange, "Invalid Date Range!");
    }

    public class ErrorCodes
    {
        public const string InvalidDataModel = "[[INVALID_DATA_MODEL]]";
        public const string AccommodationNotFound = "[[ACCOMMODATION_NOT_FOUND]]";
        public const string UserNotFound = "[[USER_NOT_FOUND]]";
        public const string OverlappingReservation = "[[OVERLAPPING_RESERVATION]]";
        public const string CapacityExceed = "[[CAPACITY_EXCEEDED]]";
        public const string InvalidDateRange = "[[INVALID_DATE_RANGE]]";
    }
}
