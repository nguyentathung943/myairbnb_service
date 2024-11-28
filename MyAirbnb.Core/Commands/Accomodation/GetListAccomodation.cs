using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyAirbnb.DataAccess;
using MyAirbnb.DataAccess.Entities;
using MyAirbnb.Models.DTOs.Accomodation;
using Mapster;

using static MyAirbnb.Models.ObjectValidationHelper;

namespace MyAirbnb.Core.Commands.Accomodation;

public class GetListAccomodation
{
    public record Command(AccomodationGetListRequestDTO Filter, int? Offset = null, int? Limit = null) : IRequest<ErrorOr<AccomodationGetListResponseDTO>> { }

    public sealed class Handler : IRequestHandler<Command, ErrorOr<AccomodationGetListResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<AccomodationGetListResponseDTO>> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var errorMessage = ValidateAndGetFirstErrorMessage(request.Filter);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return Errors.InvalidDataModel(errorMessage);
                }

                var query = BuildQueryFromFilter(request.Filter);

                var accommodations = await query
                    .AsNoTracking()
                    .ProjectToType<AccomodationDTO>()
                    .ToListAsync(cancellationToken);

                var totalCount = await query.CountAsync();

                if (request is { Offset: { } offset })
                {
                    accommodations = accommodations.Skip(offset).ToList();
                }

                if (request is { Limit: { } limit })
                {
                    accommodations = accommodations.Take(limit).ToList();
                }

                var response = new AccomodationGetListResponseDTO
                {
                    Accomodations = accommodations,
                    TotalCount = totalCount
                };

                return response;
            }
            catch (Exception ex)
            {
                return Error.Failure(description: ex.Message);
            }
        }

        private IQueryable<Accommodation> BuildQueryFromFilter(AccomodationGetListRequestDTO filter)
        {
            var query = _context.Accommodations.AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(a =>
                    EF.Functions.ILike(a.Name, $"%{filter.SearchString}%") ||
                    EF.Functions.ILike(a.LocationTagName, $"%{filter.SearchString}%"));
            }

            if (filter.CheckInDate.HasValue && filter.CheckOutDate.HasValue)
            {
                query = query.Where(a => !a.Reservations.Any() || !a.Reservations.Any(r =>
                    (filter.CheckInDate.Value >= r.CheckInDate && filter.CheckInDate.Value < r.CheckOutDate) ||
                    (filter.CheckOutDate.Value > r.CheckInDate && filter.CheckOutDate.Value <= r.CheckOutDate)));
            }

            query = query.Where(a =>
                a.MaxAdultsAllowed >= filter.Adults &&
                a.MaxChildrenAllowed >= filter.Children &&
                a.MaxInfantsAllowed >= filter.Infants &&
                a.MaxPetsAllowed >= filter.Pets);

            return query;
        }
    }

    public class Errors
    {
        public static Error InvalidDataModel(string errorMessage) => Error.Conflict(ErrorCodes.InvalidDataModel, errorMessage);
    }

    public class ErrorCodes
    {
        public const string InvalidDataModel = "[[INVALID_DATA_MODEL]]";
    }
}
