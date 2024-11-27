
using MyAirbnb.Models.DTOs.Accomodation;
using MyAirbnb.Models.DTOs.Authentication;

namespace MyAirbnb.Models.DTOs.Reservation;

public class ReservationDTO
{
    // Identifier
    public int Id { get; set; }

    /// <summary>
    /// Accomodation Information
    /// </summary>
    public int AccommodationId { get; set; }
    public AccomodationDTO Accommodation { get; set; }

    /// <summary>
    /// User Information
    /// </summary>
    public int UserId { get; set; }
    public UserDTO User { get; set; }

    /// <summary>
    /// Check In Day
    /// </summary>
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Checkout Date
    /// </summary>
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Total Price
    /// </summary>
    public double TotalPrice { get; set; }

    /// <summary>
    /// Total Adults
    /// </summary>
    public int TotalAdults { get; set; }

    /// <summary>
    /// Total Children
    /// </summary>
    public int TotalChildren { get; set; }

    /// <summary>
    /// Total Infants
    /// </summary>
    public int TotalInfants { get; set; }

    /// <summary>
    /// Total Pets
    /// </summary>
    public int TotalPets { get; set; }
}
